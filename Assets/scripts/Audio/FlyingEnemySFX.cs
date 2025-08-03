using UnityEngine;
using UnityEngine.Playables;

public class FlyingEnemySFX : MonoBehaviour
{
    [Header("Flying Enemy Audio Source")]
    [SerializeField] private AudioSource flyingEnemyAudioSource;
    


    [Header("Movement Clips")]  
    [SerializeField] private AudioClip[] flyClips;


    [Header("AOE Combat Clips")]  
    [SerializeField] private AudioClip[] damageBirdClips;
    [SerializeField] private AudioClip[] deathBirdClips;



   

    //FLYING ENEMY MOVEMENT SFX


    //Call via Animation Event
   

    public void PlayFly()
    {
        
    }



    // ENEMY BIRD COMBAT SFX SOUNDS




    public void PlayDamageFlying()
    {
        PlaySFX(damageBirdClips);
    }

    public void PlayDeathFlying()
    {
        PlaySFX(deathBirdClips);
    }




    //  SFX PLAY FUNCTION

    private void PlaySFX(AudioClip[] audioClip)
    {
        if (audioClip != null && audioClip.Length > 0 && flyingEnemyAudioSource != null)
        {
            AudioClip selectedClip = audioClip[Random.Range(0, audioClip.Length)];              // Randomise Sound
            flyingEnemyAudioSource.pitch = Random.Range(0.95f, 1.05f);                          // Randomise Pitch

            flyingEnemyAudioSource.PlayOneShot(selectedClip);
        }

    }
}
