using UnityEngine;

// Script to control playing audio within the game
public class AudioController : MonoBehaviour
{
    // Create a static instance of the Audio Controller
    public static AudioController instance;

    // Arrays to hold music selection and FX selection
    public AudioSource[] audioSongs;
    public AudioSource[] audioSoundFX;

    // Variable to play different song for each level
    public int gameLevel;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        PlaySong(gameLevel - 1);
    }

    // Update is called once per frame
    void Update()
    {
    }

    // Play the song number at the index requested
    public void PlaySong(int index)
    {
        audioSongs[index].Play();
    }

    // Play the sound effect at the index requested
    public void PlayFX(int index)
    {
        audioSoundFX[index].Play();
    }
}
