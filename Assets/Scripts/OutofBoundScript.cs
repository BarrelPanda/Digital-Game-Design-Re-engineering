using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutofBoundScript : MonoBehaviour
{ 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.transform.position = Vector3.zero;
        }
    }
}
