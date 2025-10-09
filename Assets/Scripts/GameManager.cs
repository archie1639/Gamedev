using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton pattern
    
    [Header("UI References")]
    public GameObject gameOverPanel;
    
    private void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        // Make sure game over panel is hidden at start
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
        
        // Make sure game is running
        Time.timeScale = 1f;
    }
    
    public void ShowGameOver()
    {
        if (gameOverPanel != null)
        {
            Debug.Log("Showing Game Over Panel...");
            gameOverPanel.SetActive(true);
            Debug.Log("Panel active state: " + gameOverPanel.activeSelf);
        }
        else
        {
            Debug.LogError("Game Over Panel is NULL! Did you assign it in the Inspector?");
        }
        
        // Pause the game
        Time.timeScale = 0f;
        
        Debug.Log("Game Over! Time.timeScale = " + Time.timeScale);
    }
    
    // Called by Restart button
    public void RestartGame()
    {
        // Resume time
        Time.timeScale = 1f;
        
        // Reload current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    // Called by Exit button
    public void ExitGame()
    {
        Debug.Log("Exiting game...");

        // Resume time in case the game was paused
        Time.timeScale = 1f;

        #if UNITY_EDITOR
            // Stop play mode in the Unity Editor
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            // Quit the application in a built game
            Application.Quit();
        #endif
    }
}