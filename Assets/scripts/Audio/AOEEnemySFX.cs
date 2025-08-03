using UnityEngine;
using UnityEngine.Playables;

public class AOEEnemySFX : MonoBehaviour
{
    [Header("AOEEnemy Audio Source")]
    [SerializeField] private AudioSource aoeEnemyAudioSource;
    


    [Header("AOE Movement Clips")]
    [SerializeField] private AudioClip[] footstepAOEClips;
         

    [Header("AOE Combat Clips")]
    [SerializeField] private AudioClip[] attackAOEClips;
    [SerializeField] private AudioClip[] damageAOEClips;
    [SerializeField] private AudioClip[] deathAOEClips;
   


    private AOE_EnemyStateList enemyState;


    private void Awake()
    {
        enemyState = GetComponent<AOE_EnemyStateList>();
    }



    // AOE ENEMY MOVEMENT SFX


    //Call via Animation Event
    public void PlayFootstepAOE()
    {
         if (enemyState.ISwalking)              // Use if grounded state is added to playerstatelist script
        PlaySFX(footstepAOEClips);

    }

   
   


    // ENEMY ROBOT COMBAT SFX SOUNDS

    public void PlayAttackAOE()
    {
        PlaySFX(attackAOEClips);
    }


    public void PlayDamageAOE()
    {
        PlaySFX(damageAOEClips);
    }

    public void PlayDeathAOE()
    {
        PlaySFX(deathAOEClips);
    }




    //  SFX PLAY FUNCTION

    private void PlaySFX(AudioClip[] audioClip)
    {
        if (audioClip != null && audioClip.Length > 0 && aoeEnemyAudioSource != null)
        {
            AudioClip selectedClip = audioClip[Random.Range(0, audioClip.Length)];         // Randomise Sound
            aoeEnemyAudioSource.pitch = Random.Range(0.95f, 1.05f);                          // Randomise Pitch

            aoeEnemyAudioSource.PlayOneShot(selectedClip);
        }

    }
}
