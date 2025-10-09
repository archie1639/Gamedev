using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Menu UI")]
    public GameObject menuPanel; // Assign your Canvas or menu panel here
    
    [Header("Game Objects")]
    public GameObject player; // Assign your player here
    // Add more game objects that should be disabled at start
    
    void Start()
    {
        // Pause the game at start
        Time.timeScale = 0f;
        
        // Disable player and other game objects
        //if (player != null)
            //player.SetActive(false);
    }
    
    // Call this function when Start button is clicked
    public void StartGame()
    {
        Debug.Log("Start button clicked!"); // Check if this appears in Console
        
        // Hide the menu
        if (menuPanel != null)
        {
            menuPanel.SetActive(false);
            Debug.Log("Menu hidden");
        }
        else
        {
            Debug.LogWarning("Menu Panel is not assigned!");
        }
        
        // Enable player and game objects
        if (player != null)
        {
            player.SetActive(true);
            Debug.Log("Player enabled");
        }
        else
        {
            Debug.LogWarning("Player is not assigned!");
        }
        
        // Resume the game
        Time.timeScale = 1f;
        Debug.Log("Game started! Time.timeScale = " + Time.timeScale);
    }
    
    // Optional: For loading a different scene instead
    public void LoadGameScene(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }
}