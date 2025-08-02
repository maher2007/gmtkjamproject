using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private float parallaxSpeed;
    private float length;
    private float startPos;
    private Transform cam;

    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        cam = Camera.main.transform;
    }

    void Update()
    {
        float temp = cam.position.x * (1 - parallaxSpeed);
        float distance = cam.position.x * parallaxSpeed;
        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);

        // Infinite scrolling (optional)
        if (temp > startPos + length) startPos += length;
        else if (temp < startPos - length) startPos -= length;
    }
}