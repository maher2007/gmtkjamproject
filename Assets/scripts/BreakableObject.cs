using System.Collections;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    [SerializeField]GameObject GameObject;
    [SerializeField] private playercontroller controller;
    [SerializeField] private int blockdurablity;
    private float XPosotion;
    private float YPosotion;
    private float ZPosotion;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        XPosotion = transform.position.x;
        YPosotion = transform.position.y;
        ZPosotion = transform.position.z;
        controller = GameObject.GetComponent<playercontroller>();
    }

    // Update is called once per frame
    void Update()
    {
        if (blockdurablity == 0)
        {
            Destroy(gameObject);
        }
    }
    private void OnEnable()
    {
        playercontroller.Break += BlockDurablity;
    }
    private void BlockDurablity()
    {
        blockdurablity--;
        StartCoroutine(Shake());
        Debug.Log("1");
    }
    IEnumerator Shake()
    {
        gameObject.transform.position = new Vector3(XPosotion,YPosotion - 0.1f ,ZPosotion);
        yield return new WaitForSeconds(0.1f);
        gameObject.transform.position = new Vector3(XPosotion,YPosotion,ZPosotion);
    }
}
