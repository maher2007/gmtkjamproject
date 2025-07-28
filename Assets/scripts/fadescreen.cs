using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class SimpleFade : MonoBehaviour
{
    public static SimpleFade Instance;
    public Image fadeImage;        
    public float fadeDuration = 1f;


    // Call this to start fading out (screen goes black)

    void Awake()
    {
        // Singleton pattern - only one instance allowed
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes

            // Make sure the fadeImage also persists
            if (fadeImage != null)
                DontDestroyOnLoad(fadeImage.transform.root.gameObject);
        }
        else
        {
            Destroy(gameObject); // Destroy duplicates
        }
    }
    public IEnumerator FadeOut()
    {
        Debug.Log("FadeOut started"); // At the start of FadeOut
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = t / fadeDuration;
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;  
        }
        fadeImage.color = new Color(0, 0, 0, 1);
    }

    // Call this to start fading in (screen becomes transparent)
    public IEnumerator FadeIn()
    {
        Debug.Log("Fadein started"); // At the start of FadeOut
        float t = fadeDuration;
        while (t > 0)
        {
            t -= Time.deltaTime;
            float alpha = t / fadeDuration;
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;  
        }
        fadeImage.color = new Color(0, 0, 0, 0);
    }
    public void FadeToScene(string sceneName)
    {
        StartCoroutine(FadeAndLoadScene(sceneName));
    }

    private IEnumerator FadeAndLoadScene(string sceneName)
    {

        fadeImage.color = new Color(0, 0, 0, 0);
        fadeImage.gameObject.SetActive(true);

        // 2. Fade to black
        yield return StartCoroutine(FadeOut());

        // 3. Optional: Test without scene loading
        // yield break;

        // 4. Load scene (if uncommented)
        SceneManager.LoadScene(sceneName);

        // 5. Force black screen in new scene
        fadeImage.color = Color.black;

        // 6. Fade back in
        yield return StartCoroutine(FadeIn());
    }

}