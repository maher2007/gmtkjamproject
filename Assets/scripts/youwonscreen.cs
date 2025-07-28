using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class youwonscreen : MonoBehaviour
{
    [SerializeField] private string tomainmenu;
    [SerializeField] private string toscene;

    public void hover()
    {
        audiomaneger.Instance.playsound(audiomaneger.Instance.hover);
    }
    public void nextscene()
    {
        audiomaneger.Instance.playsound(audiomaneger.Instance.click);
        SimpleFade fade = FindAnyObjectByType<SimpleFade>();

        if (fade != null)
        {
            StopAllCoroutines(); // Clear any existing fades

            fade.FadeToScene(toscene);
        }
        else
        {
            // Fallback if fade manager not found
            SceneManager.LoadScene(toscene);
        }
    }
    // Update is called once per frame
    public void mainmenu()
    {
        audiomaneger.Instance.playsound(audiomaneger.Instance.click);
        SimpleFade fade = FindAnyObjectByType<SimpleFade>();

        if (fade != null)
        {
            StopAllCoroutines(); // Clear any existing fades

            fade.FadeToScene(tomainmenu);
        }
        else
        {
            // Fallback if fade manager not found
            SceneManager.LoadScene(tomainmenu);
        }

    }
}

