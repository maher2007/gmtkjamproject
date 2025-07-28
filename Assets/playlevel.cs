using UnityEngine;

public class playlevel : MonoBehaviour
{
    [SerializeField] private string toscene;
    public void hover()
    {
        audiomaneger.Instance.playsound(audiomaneger.Instance.hover);
    }
    public void Playlevel()
    {
        audiomaneger.Instance.playsound(audiomaneger.Instance.click);
        SimpleFade fade = FindAnyObjectByType<SimpleFade>();

        if (fade != null)
        {
            fade.FadeToScene(toscene);
        }
        else
        {
            // Fallback if fade manager not found
            UnityEngine.SceneManagement.SceneManager.LoadScene(toscene);
        }
    }
}
