using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton property
    public static GameManager I { get; private set; }


    [Header("Cfg")]
    [SerializeField] private float fadeoutSpeed = 0.5f;


    [Header("Setup - Music")]
    [SerializeField] private AudioClip musicAlarm;
    [SerializeField] private AudioClip musicTransition;
    [SerializeField] private AudioClip musicGhost;

    [Header("Setup - SFX")]
    [SerializeField] private AudioSource audioSourceSfx;
    [SerializeField] private AudioClip sfxGhostShot;
    [SerializeField] private AudioClip sfxEnemyShot;


    // Properties
    public int TopScore = 0;


    // Private
    private AudioSource audioSource;



    #region Monobehaviour

    private void Awake()
    {
        SetupSingleton();

        audioSource = GetComponent<AudioSource>();
    }

    #endregion




    #region Public

    // FUTURE: the audio system is pretty crappy, last minute job

    public void PlaySfx(string id) // FUTURE: this shouldn't be a string, but I'm running out of time!!
    {
        if (id == "GHOST_SHOT")
        {
            audioSourceSfx.PlayOneShot(sfxGhostShot);
        }
        else if (id == "ENEMY_SHOT")
        {
            audioSourceSfx.PlayOneShot(sfxEnemyShot);
        }
    }


    public IEnumerator PlayMusic(string id) // FUTURE: this shouldn't be a string, but I'm running out of time!!
    {
        // Fade out
        // FUTURE: cross-fade, instead of just fading out
        if (audioSource.clip != null)
        {
            float volume = audioSource.volume;
            while (volume > 0)
            {
                volume -= fadeoutSpeed * Time.deltaTime;
                audioSource.volume = Mathf.Max(volume, 0);
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
        audioSource.Stop();
        audioSource.volume = 1f;

        if (id == "ALARM")
        {
            audioSource.clip = musicAlarm;
            audioSource.loop = true;
            audioSource.volume = 0.3f;
            audioSource.Play();
        }
        if (id == "TRANSITION")
        {
            audioSource.clip = musicTransition;
            audioSource.loop = false;
            audioSource.Play();
        }
        else if (id == "GHOST")
        {
            audioSource.clip = musicGhost;
            audioSource.loop = true;
            audioSource.Play();
        }
    }
        
    #endregion




    #region Private

    private void SetupSingleton()
    {
        // Singleton setup
        if (I != null && I != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            I = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    #endregion
}
