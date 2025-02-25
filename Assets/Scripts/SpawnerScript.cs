using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    [SerializeField] private GameObject[] _Tetromino;
    [SerializeField] private Transform _NBDisplay;
    [SerializeField] private GameObject _TContainerPrefab;
    private GameObject _NextBlock;
    private GameObject _NextBlockDisplay;
    private GameObject _TContainer;
    public bool KeepSpawning;

    void Start()
    {
        _TContainer = Instantiate(_TContainerPrefab, this.transform);
        KeepSpawning = true;
        _NextBlock = _Tetromino[Random.Range(0, _Tetromino.Length)];
        NewTetromino();
    }

    private void Update()
    {
        foreach (Transform children in transform)
        {
            if (children.childCount == 0) Destroy(children.gameObject);
        }
    }

    public void NewTetromino()
    {
        if (KeepSpawning)
        {
            Destroy(_NextBlockDisplay);
            Instantiate(_NextBlock, transform.position, Quaternion.identity, _TContainer.transform);
            _NextBlock = _Tetromino[Random.Range(0, _Tetromino.Length)];
            _NextBlockDisplay = Instantiate(_NextBlock, _NBDisplay.position, Quaternion.identity, _NBDisplay.transform);
            _NextBlockDisplay.GetComponent<tetrominoBehaviour>().nextblockDisplay = true;
        }
    }

    public void ResetGame()
    {
        Destroy(_NextBlockDisplay);
        Destroy(_TContainer);
        _TContainer = Instantiate(_TContainerPrefab, this.transform);
    }

    public void ResumeGame()
    {
        KeepSpawning = true;
        NewTetromino();
    }
}
