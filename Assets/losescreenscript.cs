using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class losescreenscript : MonoBehaviour
{
    [SerializeField] private string tomainmenu;
    [SerializeField] private string restartscene;

    public void restart()
    {
        audiomaneger.Instance.playsound(audiomaneger.Instance.click);
        SceneManager.LoadScene(restartscene);
    }

    // Update is called once per frame
    public void hover()
    {
        audiomaneger.Instance.playsound(audiomaneger.Instance.hover);
    }
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

