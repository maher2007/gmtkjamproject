using System.Collections;
using Unity.Hierarchy;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    [SerializeField]GameObject GameObject;
    [SerializeField] private playercontroller controller;
    [SerializeField] private int blockdurablity;
    private float XPosotion;
    private float YPosotion;
    private float ZPosotion;
    private bool canbreak;
    [SerializeField]LayerMask player;
    [SerializeField] Transform playercheck;
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
        if (blockdurablity <= 0)
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
        if (onplayer())
        {
            blockdurablity--;
            StartCoroutine(Shake());
        }
    }
    IEnumerator Shake()
    {
        gameObject.transform.position = new Vector3(XPosotion,YPosotion - 0.1f ,ZPosotion);
        yield return new WaitForSeconds(0.1f);
        gameObject.transform.position = new Vector3(XPosotion,YPosotion,ZPosotion);
    }

    public bool onplayer()
    {
        if (Physics2D.Raycast(playercheck.position, Vector2.up, 1f, player))
        {
            return true;
        }
        else 
        {
            return false;
        }
    }
}
