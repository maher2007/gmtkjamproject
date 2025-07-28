using UnityEngine;

public class play : MonoBehaviour
{
    public void PlayGame()
    {
        SimpleFade fade = Object.FindAnyObjectByType<SimpleFade>();

        if (fade != null)
        {
            fade.FadeToScene("settings");
        }
       else
        {
            // Fallback if fade manager not found
            UnityEngine.SceneManagement.SceneManager.LoadScene("settings");
        }
    }
}
