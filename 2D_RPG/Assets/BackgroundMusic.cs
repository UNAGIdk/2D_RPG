using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundMusic : MonoBehaviour
{
    private AudioSource audioSource;

    public AudioClip menuTheme;
    public AudioClip gameTheme;
    public AudioClip GARRTheme;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        switch (SceneManager.GetActiveScene().name)
        {
            case "Main Menu":
                audioSource.volume = 0.8f;
                audioSource.clip = menuTheme;
                audioSource.Play();
                break;
            case "Garr Dungeon":
                audioSource.volume = 0.4f;
                audioSource.clip = GARRTheme;
                audioSource.Play();
                break;
            default:
                audioSource.volume = 0.7f;
                audioSource.clip = gameTheme;
                audioSource.Play();
                break;
        }
    }
}
