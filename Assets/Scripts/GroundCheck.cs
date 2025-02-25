using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] private CharacterMovement _playerMove;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player")
        {
            _playerMove.GroundCheck(true);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag != "Player")
        {
            _playerMove.GroundCheck(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag != "Player")
        {
            _playerMove.GroundCheck(false);
        }
    }
}
