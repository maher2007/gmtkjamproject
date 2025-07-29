using UnityEngine;
using UnityEngine.Playables;

public class EnemySFX : MonoBehaviour
{
    [Header("Enemy Audio Source")]
    [SerializeField] private AudioSource enemyAudioSource;


    [Header("Movement Clips")]
    [SerializeField] private AudioClip[] footstepClips;
    [SerializeField] private AudioClip[] jumpClips;   
    [SerializeField] private AudioClip[] landClips;

    [Header("Combat Clips")]
    [SerializeField] private AudioClip[] attackClips;
    [SerializeField] private AudioClip[] damageClips;
    [SerializeField] private AudioClip[] deathClips;





    // ENEMY MOVEMENT SFX


    //Call via Animation Event
    public void PlayFootstep()
    {
        // if (!playerState.grounded)              // Use if grounded state is added to playerstatelist script
        PlaySFX(footstepClips);

    }

    public void PlayJump()
    {
        PlaySFX(jumpClips);
    }

    public void PlayLand()
    {
        PlaySFX(landClips);
    }



    // PLAYER COMBAT SFX SOUNDS

    public void PlayAttack()
    {
        PlaySFX(attackClips);
    }


    public void PlayDamage()
    {
        PlaySFX(damageClips);
    }

    public void PlayDeath()
    {
        PlaySFX(deathClips);
    }





    //  SFX PLAY FUNCTION

    private void PlaySFX(AudioClip[] audioClip)
    {
        if (audioClip != null && audioClip.Length > 0 && enemyAudioSource != null)
        {
            AudioClip selectedClip = audioClip[Random.Range(0, audioClip.Length)];         // Randomise Sound
            enemyAudioSource.pitch = Random.Range(0.95f, 1.05f);                          // Randomise Pitch

            enemyAudioSource.PlayOneShot(selectedClip);
        }

    }
}
