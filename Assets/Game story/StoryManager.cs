using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryManager : MonoBehaviour
{
    [SerializeField] private List<int> _storySet;
    [SerializeField] private List<Sprite> _storyList;
    [SerializeField] private List<string> _storyNarrations;
    [SerializeField] private GameObject _endCredits;

    private Image _image;
    private Text _text;
    private int _storyCount;
    private int _storyIndex;

    void Start()
    {
        _endCredits.SetActive(false);
        _image = GetComponent<Image>();
        _text = GetComponentInChildren<Text>();
        _storyCount = 0;
        _storyIndex = 0;
        _image.sprite = _storyList[_storyCount];
        _text.text = _storyNarrations[_storyCount];
        FindObjectOfType<Manager>().SetLevel(0);
        StartCoroutine(PlayStory(0));
    }

    public void advanceStory()
    {
        FindObjectOfType<Manager>().SetLevel(0);
        StartCoroutine(PlayStory(_storyIndex));
    }

    IEnumerator PlayStory(int i)
    {
        _storyIndex++;
        print(_storyIndex);
        print("story set: " + i);
        for (int j = 0; j < _storySet[i]; j++)
        {
            _image.sprite = _storyList[_storyCount];
            _text.text = _storyNarrations[_storyCount];
            _storyCount++;
            yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.Space));
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
            yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.Space));
        }
        print("finished");
        FindObjectOfType<Manager>().SetLevel(i + 1);
        if (_storyIndex == _storySet.Count)
        {
            _endCredits.SetActive(true);
        }
    }
}
