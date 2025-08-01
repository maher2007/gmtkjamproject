using System;
using UnityEngine;

public class killscript : MonoBehaviour
{
    public event Action Playerreset;
    [SerializeField] playercontroller playercontroller;    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private GameObject restartscreen;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("player")) 
        {
            playercontroller.TakeDamge(1);
            Playerreset?.Invoke();
        }
    }
}
