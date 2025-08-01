using UnityEngine;

public class fadescript : MonoBehaviour
{
    [SerializeField] GameObject GameObject;
    [SerializeField] string toscene;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("player"))
        {
            fade();
        }
        Debug.Log(collision.gameObject);
    }

    private void fade()
    {
        SimpleFade fade = FindAnyObjectByType<SimpleFade>();

        if (fade != null)
        {
            fade.FadeToScene(sceneName: toscene);
        }
        else
        {
            // Fallback if fade manager not found
            UnityEngine.SceneManagement.SceneManager.LoadScene(toscene);
        }
    }
}
