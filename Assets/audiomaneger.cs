using UnityEngine;

public class audiomaneger : MonoBehaviour
{
    public static audiomaneger Instance;
    public AudioClip music;
    public AudioClip click;
    public AudioClip hover;
    private AudioSource soundplayer;
    private AudioSource musicplayer;


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

    }

    // Update is called once per frame
   public void playsound(AudioClip clip)
    {
        soundplayer.PlayOneShot(clip);
    }
    public void playmusic(AudioClip clip)
    {
        musicplayer.PlayOneShot(clip);
    }
}
