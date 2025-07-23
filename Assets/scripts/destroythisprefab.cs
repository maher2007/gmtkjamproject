using UnityEngine;

public class destroythisprefab : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject ,GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
    }
}
