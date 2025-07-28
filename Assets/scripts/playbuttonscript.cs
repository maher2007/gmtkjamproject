using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string toscene;
    public void hover()
    {
        audiomaneger.Instance.playsound(audiomaneger.Instance.hover);
    }
    public void PlayGame()
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
