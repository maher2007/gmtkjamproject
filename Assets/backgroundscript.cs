using UnityEngine;

public class InfiniteParallax : MonoBehaviour
{
    private float startpos;
    private float lengthofsprite;
    public float amountofparallox;
    public Camera maincamire;
    private void Start()
    {
        startpos = transform.position.x;
        lengthofsprite = GetComponent<SpriteRenderer>().bounds.size.x;
    }
    private void Update()
    {
        Vector3 Position = maincamire.transform.position;
        float temp = Position.x * (1 - amountofparallox);
        float destance = Position.x * amountofparallox;
        Vector3 newpos = new Vector3(startpos + destance, maincamire.transform.position.y, transform.position.z);
        transform.position = newpos;
        if (temp > startpos + (lengthofsprite / 2))
        {
            startpos += lengthofsprite;
        }
        else if (temp < startpos - (lengthofsprite / 2))
        {
            startpos -= lengthofsprite;
        }
    }
}