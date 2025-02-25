using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Manager : MonoBehaviour
{
    [SerializeField] private Sprite[] _bgImage;
    [SerializeField] private GameObject[] _enemyType;
    [SerializeField] private GameObject _CombatCam;
    [SerializeField] private GameObject _PlatformCam;
    [SerializeField] private GameObject[] _set;
    public static int level;
    [SerializeField] private GameObject[] _platforms;
    [SerializeField] private GameObject[] _CombatOverDisplay;
    [SerializeField] private GameObject _Story;
    [SerializeField] private GameObject _combatUI;

    private GameObject _enemyUnit;

    void ManageScenario()
    {
        switch (level)
        {
            case 0:
                _combatUI.SetActive(false);
                _set[0].SetActive(false);
                _set[1].SetActive(false);
                break;
            case 1:
                _combatUI.SetActive(true);
                print("Tutorial");
                _Story.SetActive(false);
                _set[0].SetActive(false);
                _set[1].SetActive(true);
                _PlatformCam.SetActive(false);
                _CombatCam.SetActive(true);
                _platforms[0].SetActive(false);
                _platforms[1].SetActive(false);
                _platforms[2].SetActive(false);
                _CombatCam.GetComponentInChildren<SpriteRenderer>().sprite = _bgImage[0];
                _enemyUnit = Instantiate(_enemyType[0], GameObject.Find("Enemy Location").GetComponent<Transform>().position, Quaternion.identity);
                _enemyUnit.GetComponent<EnemyScript>()._tutorial = true;
                FindObjectOfType<CharacterStats>().SetEnemy();
                break;
            case 2:
                _combatUI.SetActive(false);
                print("Level " + level);
                _Story.SetActive(false);
                _set[0].SetActive(true);
                _set[1].SetActive(false);
                _PlatformCam.SetActive(true);
                _CombatCam.SetActive(false);
                _PlatformCam.GetComponentInChildren<SpriteRenderer>().sprite = _bgImage[level - 2];
                _platforms[0].SetActive(true);
                _platforms[1].SetActive(false);
                _platforms[2].SetActive(false);
                break;
            case 3:
                _combatUI.SetActive(false);
                print("Level " + level);
                _Story.SetActive(false);
                _set[0].SetActive(true);
                _set[1].SetActive(false);
                _PlatformCam.SetActive(true);
                _CombatCam.SetActive(false);
                _PlatformCam.GetComponentInChildren<SpriteRenderer>().sprite = _bgImage[level - 2];
                _platforms[0].SetActive(false);
                _platforms[1].SetActive(true);
                _platforms[2].SetActive(false);
                break;
            case 4:
                _combatUI.SetActive(false);
                print("Level " + level);
                _Story.SetActive(false);
                _set[0].SetActive(true);
                _set[1].SetActive(false);
                _PlatformCam.SetActive(true);
                _CombatCam.SetActive(false);
                _PlatformCam.GetComponentInChildren<SpriteRenderer>().sprite = _bgImage[level - 2];
                _platforms[0].SetActive(false);
                _platforms[1].SetActive(false);
                _platforms[2].SetActive(true);
                break;
            case > 4 and < 8:
                _combatUI.SetActive(true);
                print("Arena " + (level - 3));
                _Story.SetActive(false);
                _set[0].SetActive(false);
                _set[1].SetActive(true);
                _PlatformCam.SetActive(false);
                _CombatCam.SetActive(true);
                _platforms[0].SetActive(false);
                _platforms[1].SetActive(false);
                _platforms[2].SetActive(false);
                FindObjectOfType<SpawnerScript>().ResetGame();
                FindObjectOfType<SpawnerScript>().ResumeGame();
                _CombatCam.GetComponentInChildren<SpriteRenderer>().sprite = _bgImage[level - 5];
                Transform enemyLocation = GameObject.Find("Enemy Location").GetComponent<Transform>();
                _enemyUnit = Instantiate(_enemyType[level - 5], enemyLocation.position, Quaternion.identity);
                FindObjectOfType<CharacterStats>().SetEnemy();
                break;
        }
    }

    public void SetLevel(int _level)
    {
        level = _level;
        ManageScenario();
    }

    public void Teleport()
    {
        _Story.SetActive(true);
        _Story.GetComponent<StoryManager>().advanceStory();
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(1);
    }

    public void Restart()
    {
        print("Restart");
        _CombatOverDisplay[0].SetActive(false);
        _CombatOverDisplay[1].SetActive(false);
        Destroy(_enemyUnit);
        SetLevel(level);
    }

    public void QuitGame()
    {
        SceneManager.LoadScene(0);
    }

    public void ResumeGame()
    {
        _CombatOverDisplay[0].SetActive(false);
        _CombatOverDisplay[1].SetActive(false);
        FindObjectOfType<tetrominoBehaviour>().AllowMove(true);
        if (level == 1)
        {
            _Story.SetActive(true);
            _Story.GetComponent<StoryManager>().advanceStory();
        }
        else
        {
            level -= 3;
            ManageScenario();
            print("Exit Combat");
        }
    }

    public void EnterCombat()
    {
        level += 3;
        ManageScenario();
        print("Enter Combat");
    }

    public void CombatComplete(bool _WinState)
    {
        FindObjectOfType<SpawnerScript>().KeepSpawning = false;
        print("win: " + _WinState);
        FindObjectOfType<tetrominoBehaviour>().AllowMove(false);
        _CombatOverDisplay[0].SetActive(!_WinState);
        _CombatOverDisplay[1].SetActive(_WinState);
    }
}
