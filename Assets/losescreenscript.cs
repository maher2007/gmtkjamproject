using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class losescreenscript : MonoBehaviour
{
    [SerializeField] private string tomainmenu;
    [SerializeField] private string restartscene;

    private void OnEnable()                                                //Transition to Menu Music
    {
        if (audiomaneger.Instance != null)
        {
            audiomaneger.Instance.StopGameplayMusic();
            audiomaneger.Instance.PlayMenuMusic();        
        }
    }
    public void restart()
    {
        audiomaneger.Instance.playsound(audiomaneger.Instance.click);
        audiomaneger.Instance.PlayGameplayMusic();                          //Transition to Gameplay Music

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

