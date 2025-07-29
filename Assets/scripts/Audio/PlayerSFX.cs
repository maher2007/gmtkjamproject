using UnityEngine;
using UnityEngine.Playables;

public class PlayerSFX : MonoBehaviour
{
    [Header("Player Audio Source")]
    [SerializeField] private AudioSource playerAudioSource;

    [Header("WallSlide Source")]
    [SerializeField] private AudioSource wallSlideSource;


    [Header("Movement Clips")]
    [SerializeField] private AudioClip[] footstepClips;
    [SerializeField] private AudioClip[] jumpClips;
    [SerializeField] private AudioClip[] doubleJumpClips;
    [SerializeField] private AudioClip[] dashClips;
    [SerializeField] private AudioClip[] landClips;
    [SerializeField] private AudioClip[] wallSlideClips;

    [Header("Combat Clips")]
    [SerializeField] private AudioClip[] attackClips;
    [SerializeField] private AudioClip[] damageClips;
    [SerializeField] private AudioClip[] deathClips;


    private playerstatelist playerState;

    private void Awake()
    {
        playerState = GetComponent<playerstatelist>();

        if (wallSlideClips.Length > 0)
        {
            wallSlideSource.clip = wallSlideClips[Random.Range(0, wallSlideClips.Length)];       // set default Wall Slide Clip variation
        }
        wallSlideSource.loop = true;
    }



   // PLAYER MOVEMENT SFX
   
    
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


    public void PlayDoubleJump()
    {
        PlaySFX(doubleJumpClips);
    }



    public void PlayDash()
    {
        if(!playerState.Dashing)
        PlaySFX(dashClips);
    }


    public void PlayLand()
    {
        PlaySFX(landClips);
    }

    public void PlayWallSlideClip()
    {

     //   if (playerState.sliding)        // add if sliding state is added to the playerstatelist script
            WallSlideHandle();
                         
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


    //  WALL SLIDE LOGIC
    private void WallSlideHandle()                                                            
    {      
        if (!wallSlideSource.isPlaying)
            {

                wallSlideSource.clip = wallSlideClips[Random.Range(0, wallSlideClips.Length)];   // Randomise Variation
                wallSlideSource.pitch = Random.Range(0.95f, 1.05f);                             // Randomise Pitch

                wallSlideSource.Play();
            }

        

        else
        {
            if (wallSlideSource.isPlaying)
            {
                wallSlideSource.Stop();

            }
        }
    }



    //  SFX PLAY FUNCTION

    private void PlaySFX(AudioClip[] audioClip)
    {
        if (audioClip != null && audioClip.Length > 0 && playerAudioSource != null)
        {
            AudioClip selectedClip = audioClip[Random.Range(0, audioClip.Length)];         // Randomise Sound
            playerAudioSource.pitch = Random.Range(0.95f, 1.05f);                          // Randomise Pitch

            playerAudioSource.PlayOneShot(selectedClip);
        }

    }
}
