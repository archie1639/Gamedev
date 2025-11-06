using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    
    [Header("Music Settings")]
    public AudioClip[] musicTracks; // Array for multiple tracks
    [Range(0f, 1f)]
    public float volume = 0.5f;
    
    [Header("Playback Options")]
    public bool playRandomTrack = true; // Play random track
    public bool shufflePlaylist = false; // Play all tracks in random order
    
    private AudioSource audioSource;
    private int currentTrackIndex = 0;
    
    void Awake()
    {
        // Singleton pattern - keeps music playing across scenes
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        // Setup audio source
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = false; // We'll handle looping ourselves
        audioSource.volume = volume;
        
        // Start playing music
        if (musicTracks != null && musicTracks.Length > 0)
        {
            PlayNextTrack();
        }
        else
        {
            Debug.LogWarning("No music tracks assigned to MusicManager!");
        }
    }
    
    void Update()
    {
        // Check if current track finished, play next one
        if (!audioSource.isPlaying && musicTracks.Length > 0)
        {
            PlayNextTrack();
        }
    }
    
    void PlayNextTrack()
    {
        if (musicTracks.Length == 0) return;
        
        if (playRandomTrack)
        {
            // Play a random track
            currentTrackIndex = Random.Range(0, musicTracks.Length);
        }
        else if (shufflePlaylist)
        {
            // Shuffle through all tracks
            currentTrackIndex = Random.Range(0, musicTracks.Length);
        }
        else
        {
            // Play tracks in order
            currentTrackIndex = (currentTrackIndex + 1) % musicTracks.Length;
        }
        
        audioSource.clip = musicTracks[currentTrackIndex];
        audioSource.Play();
        
        Debug.Log("Now playing: " + musicTracks[currentTrackIndex].name);
    }
    
    // Optional: Functions to control music
    public void PlayMusic()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
    
    public void StopMusic()
    {
        audioSource.Stop();
    }
    
    public void PauseMusic()
    {
        audioSource.Pause();
    }
    
    public void UnpauseMusic()
    {
        audioSource.UnPause();
    }
    
    public void SetVolume(float newVolume)
    {
        volume = Mathf.Clamp01(newVolume);
        audioSource.volume = volume;
    }
    
    public void SkipToNextTrack()
    {
        audioSource.Stop();
        PlayNextTrack();
    }
}