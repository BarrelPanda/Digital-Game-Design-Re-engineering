using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;

public class tetrominoBehaviour : MonoBehaviour
{
    [SerializeField] private Vector3 _RotationPoint;
    [SerializeField] private float _fallTime = 0.8f;
    [SerializeField] private GameObject _AllignmentBlock;

    private GameObject _allignment;
    private float _previousTime;
    private Vector3 _Overflow;
    private SpawnerScript _spawner;

    public static int height = 15;
    public static int width = 15;
    public static Transform[,] grid = new Transform[width, height];
    public bool nextblockDisplay = false;
    static public bool allowMove = true;
    public static bool reset;

    private void Start()
    {
        _spawner = FindObjectOfType<SpawnerScript>().GetComponent<SpawnerScript>();
        _allignment = Instantiate(_AllignmentBlock, transform.position, Quaternion.identity,FindObjectOfType<SpawnerScript>().transform);
        UpdateAliignment();
        _Overflow = new Vector3(7, 12, 0);
        allowMove = true;
    }

    void Update()
    {
        if (reset)
        {
            Destroy(gameObject);
        }

        if (nextblockDisplay)
        {
            Destroy(_allignment);
            this.enabled = false;
        }
        else
        {
            if (allowMove)
            {
                if (transform.position == _Overflow)
                {
                    CheckOverflow();
                }
                Movement();
            }
        }
    }

    void CheckOverflow()
    {
        if (!ValidMove(this.transform))
        {
            _spawner.KeepSpawning = false;
            FindObjectOfType<Manager>().CombatComplete(false);
            Destroy(this.gameObject);
        }
    }

    void Movement()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            transform.position += Vector3.left;
            if (!ValidMove(this.transform))
                transform.position -= Vector3.left;
            UpdateAliignment();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            transform.position += Vector3.right;
            if (!ValidMove(this.transform))
                transform.position -= Vector3.right;
            UpdateAliignment();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            transform.RotateAround(transform.TransformPoint(_RotationPoint), Vector3.forward, 90);
            if (!ValidMove(this.transform))
                transform.RotateAround(transform.TransformPoint(_RotationPoint), Vector3.forward, -90);
            UpdateAliignment();
        }

        if (Time.time - _previousTime > (Input.GetKey(KeyCode.S) ? _fallTime / 10 : _fallTime))
        {
            transform.position += Vector3.down;
            if (!ValidMove(this.transform))
            {
                transform.position -= Vector3.down;
                AddtoGrid();
                CheckLine();
                this.enabled = false;
                FindObjectOfType<SpawnerScript>().NewTetromino();
            }
            _previousTime = Time.time;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            while (ValidMove(this.transform))
            {
                transform.position += Vector3.down;
            }
            transform.position -= Vector3.down;
            AddtoGrid();
            CheckLine();
            this.enabled = false;
            _spawner.NewTetromino();
        }
    }

    void CheckLine()
    {
        int _Combo = 0;
        for (int i = height-1; i >= 0; i--) //checks from the top down
        {
            if (HasLine(i))
            {
                _Combo++;
                DeleteLine(i);
                RowDown(i);
            }
        }
        if (_Combo > 0)
            FindObjectOfType<CharacterStats>().Attack(_Combo);
    }
    bool HasLine(int i)
    {
        for (int j = 0; j < width; j++) //checks from left to right
        {
            if (grid[j,i] == null) //if theres an empty space then no line
                return false;
        }
        return true;
    }
    void DeleteLine(int i)
    {
        for (int j = 0; j < width; j++) //going from left to right
        {
            Destroy(grid[j,i].gameObject); //remove game object
            grid[j,i] = null; //reset grid value
        }
    }
    void RowDown(int i)
    {
        for (int y = i; y < height; y++) //counts from bottom up
        {
            for (int j = 0; j < width; j++) // goes from left to right
            {
                if (grid[j,y] != null) 
                {
                    grid[j,y - 1] = grid[j,y];
                    grid[j,y] = null;
                    grid[j, y - 1].transform.position += Vector3.down;
                }
            }
        }
    }

    void UpdateAliignment()
    {
        _allignment.transform.position = this.transform.position;
        _allignment.transform.rotation = this.transform.rotation;
        while (ValidMove(_allignment.transform))
        {
            _allignment.transform.position += Vector3.down;
        }
        _allignment.transform.position -= Vector3.down;
    }

    void AddtoGrid()
    {
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.position.x);
            int roundedY = Mathf.RoundToInt(children.position.y);

            grid[roundedX, roundedY] = children;

            Destroy(_allignment);
        }
    }

    bool ValidMove(Transform x)
    {
        foreach (Transform children in x)
        {
            int roundedX = Mathf.RoundToInt(children.position.x);
            int roundedY = Mathf.RoundToInt(children.position.y);

            if (roundedX < 0 || roundedX >= width || roundedY < 0 || roundedY >= height)
            {
                return false;
            }

            if (grid[roundedX, roundedY] != null)
                return false;
        }
        return true;
    }

    public void AllowMove(bool state)
    {
        allowMove = state;
    }

    public void ResetGame()
    {
        grid = new Transform[width, height];
    }
}
