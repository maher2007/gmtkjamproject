using UnityEngine;

public class camerafollow : MonoBehaviour
{
    public GameObject Player;
    [SerializeField] private float followSpeed = 1.0f;
    [SerializeField] private Vector3 offset;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, Player.transform.position + offset, followSpeed);
        
    }
}
