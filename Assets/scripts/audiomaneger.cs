using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class audiomaneger : MonoBehaviour
{
    public static audiomaneger Instance;
    
    public AudioClip click;
    public AudioClip hover;

    private AudioSource soundplayer;
    private AudioSource musicplayer;

    [SerializeField] private AudioSource gameplaySource;
    [SerializeField] private AudioSource menuSource;

    [SerializeField] private AudioClip gameplayMusic;
    [SerializeField] private AudioClip menuMusic;

    private AudioSource currentSource;
    private AudioSource nextSource;

    public AudioMixerGroup sfxMixerGroup;

    private bool gameStart;
    private Coroutine transitionRoutine;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Instance == null) 
        { Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else { Destroy(gameObject); }
        soundplayer = gameObject.AddComponent<AudioSource>();
        musicplayer = gameObject.AddComponent<AudioSource>();

        if (sfxMixerGroup != null)
        {
            soundplayer.outputAudioMixerGroup = sfxMixerGroup;
        }

        if (menuSource != null && menuMusic != null)                   // Playing Menu Music at the beginning
        {
            menuSource.clip = menuMusic;
            menuSource.loop = true;
            menuSource.volume = 1f;
            menuSource.Play();
            currentSource = menuSource;
            nextSource = gameplaySource;
        }

        else
        {
            Debug.LogWarning("Menu source or music is not assigned!");
        }

        gameStart = true;

    }
    
   public void playsound(AudioClip clip)
    {
        soundplayer.PlayOneShot(clip);
    }
    public void playmusic(AudioClip clip)
    {
        musicplayer.PlayOneShot(clip);
    }


   public void PlayGameplayMusic()                        // Function for transitioning to Gameplay Music
    {
        if(currentSource == gameplaySource && gameplaySource.clip == gameplayMusic) return;

        TransitionToMusic(gameplayMusic);
    }


    public void PlayMenuMusic()                           // Function for transitioning to Menu Music
    {
       // Debug.Log("PlayMenuMusic function is called");

        if (currentSource == menuSource && menuSource.clip == menuMusic) return;

        TransitionToMusic(menuMusic);
    }


    public void StopGameplayMusic()
    {
        gameplaySource.Stop();
    }
    public void TransitionToMusic(AudioClip newClip, float fadeDuration = 1.5f)
    {
       // Debug.Log("TransitionToMusic function is called");

        if (transitionRoutine != null)
            StopCoroutine(transitionRoutine); // Stop any running transition

        transitionRoutine = StartCoroutine(CrossfadeToNewClip(newClip, fadeDuration));
    }

    private IEnumerator CrossfadeToNewClip(AudioClip newClip, float duration)
    {

        Debug.Log($"Starting crossfade. New clip: {newClip.name}");

        AudioSource fromSource = currentSource;
        AudioSource toSource   = nextSource;

        if(currentSource==menuSource)
        {
            //Debug.Log("Current Source is {menuSource}");
            fromSource = menuSource;
            toSource = gameplaySource;
            
        }

        else if (currentSource == gameplaySource)
        {
            
           // Debug.Log("Current Source is {gameplaySource}");
            fromSource = gameplaySource;
            toSource   = menuSource;
            
        }

        toSource.clip = newClip;
        toSource.volume = 0f;              
        toSource.loop = true;
        toSource.Play();
          
        if(!gameStart)
        {
            duration = 0.3f;
        }

        Debug.Log("Fade Duration is" + duration + " seconds");

        float t = 0f;

        while (t < duration)
        {

            //Debug.Log("Transitioning from {fromSource} to {toSource}");
            t += Time.unscaledDeltaTime;
            float progress = t / duration;

            fromSource.volume = Mathf.Lerp(1f, 0f, progress);
            toSource.volume = Mathf.Lerp(0f, 1f, progress);

            yield return null;
        }
        Debug.Log("Crossfade complete");

        fromSource.Stop();
        fromSource.volume = 1f;
        toSource.volume = 1f;

        currentSource = toSource;
        nextSource = fromSource;

        gameStart = false;
    }

}
