using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    public AudioSource introMusic;
    public float musicDuration = 2f;

    void Start()
    {
        // Play music when the MenuScene loads
        if (introMusic != null)
        {
            introMusic.Play();
        }
    }

    public void PlayGame()
    {
        Debug.Log("PlayGame button clicked!");
        StartCoroutine(LoadGameSceneAfterDelay());
    }

    IEnumerator LoadGameSceneAfterDelay()
    {
        // Wait before loading (optional, adjust or remove if you want instant load)
        yield return new WaitForSeconds(musicDuration);

        // Load the game scene
        SceneManager.LoadScene("GameScene"); // Replace with your exact scene name
    }

    public void QuitGame()
    {
        Application.Quit();
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}