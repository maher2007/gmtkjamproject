using System;
using UnityEngine;

public class healScript : MonoBehaviour
{
    GameObject[] gameobject;
    playercontroller controller;
    private void Start()
    {
        gameobject = GameObject.FindGameObjectsWithTag("player");
        controller = gameObject.GetComponent<playercontroller>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == gameobject[1])
        {
            controller.healnow();
            Debug.Log(1);
            Destroy(gameObject,1);
        }
    }
}
