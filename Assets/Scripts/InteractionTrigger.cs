using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionTrigger : MonoBehaviour
{
    [SerializeField] private int _type;
    [SerializeField] private GameObject[] _affecting;

    [SerializeField] private string _message = "enter your text here";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            switch (_type)
            {
                case 1:
                    SignBehaviour(true);
                    break;
                case 2:
                    ButtonBehaviour(true);
                    break;
                case 3:
                    EnemyBehaviour(collision);
                    break;
                case 4 or 5:
                    TravelBehaviour();
                    break;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            switch (_type)
            {
                case 1:
                    SignBehaviour(false);
                    break;
            }
        }
    }
     
    void SignBehaviour(bool state)
    {
        _affecting[0].SetActive(state);
        _affecting[0].GetComponentInChildren<Text>().text = _message;
    }

    void ButtonBehaviour(bool state)
    {
        for (int i = 0; i <  _affecting.Length; i++)
        _affecting[i].SetActive(!_affecting[i].activeInHierarchy);
    }

    void EnemyBehaviour(Collider2D Player)
    {
        Destroy(this.gameObject);
        FindObjectOfType<Manager>().EnterCombat();
    }

    void TravelBehaviour()
    {
        FindObjectOfType<CharacterMovement>().transform.position = Vector3.zero;
        FindObjectOfType<Manager>().Teleport();
    }
}
