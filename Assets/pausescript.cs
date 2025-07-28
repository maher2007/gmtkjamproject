using UnityEngine;

public class pausescript : MonoBehaviour
{
    public GameObject pauseMenuCanvas;
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
    }

    public void TogglePause()
    {
        audiomaneger.Instance.playsound(audiomaneger.Instance.click);
        isPaused = !isPaused;
        pauseMenuCanvas.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void hover()
    {
        audiomaneger.Instance.playsound(audiomaneger.Instance.hover);
    }
}
