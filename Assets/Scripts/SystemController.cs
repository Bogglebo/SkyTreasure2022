using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SystemController : MonoBehaviour
{
    // Create a static instance of the System Controller 
    public static SystemController instance;

    private Vector3 respawnPosition;    // Variable to hold respawn position
    private Vector3 cameraSpawnPosition;  // Variable to hold camera respawn position

    // Game object for the death particle effect
    public GameObject deathEffect;

    // Variables to access other controller instances
    private PlayerController playerController;

    // Level to load at end of level
    public string nextLevel;

    // As soon as the game starts, before Start() is called
    private void Awake()
    {
        // Set the SystemController to this instance
        instance = this;
        // Set the variable for the PlayerController instance
        playerController = PlayerController.instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Make the mouse invisible during gameplay Esc to reinstate
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        // Set the respawn position when the player dies
        respawnPosition = instance.transform.position;
        // Set the camera respawn position
        cameraSpawnPosition = CameraController.instance.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // If the player presses the Escape key pause the game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseState();
        }
    }

    // Method to handle player respawn after death
    public void Respawn()
    {
        StartCoroutine(RespawnCoroutine());
    }

    // Start a timer on a separate thread from the main update
    public IEnumerator RespawnCoroutine()
    {
        // Disable the player controller and play the death sound effect
        playerController.gameObject.SetActive(false);
        AudioController.instance.PlayFX(4);
        // Fade the UI to black
        UIController.instance.fadeToBlack = true;
        // Create an instance of the death effect adjusting to the middle of the player
        Instantiate(deathEffect, playerController.transform.position +
            new Vector3(0f, 1f, 0f), playerController.transform.rotation);
        // Deactivate the player and wait for 2 seconds before respawning
        yield return new WaitForSeconds(2f);
        // Reset the player's health and fade the UI back in
        HealthController.instance.ResetHealth();
        UIController.instance.fadeFromBlack = true;
        // Respawn the player and camera at their designated positions
        playerController.transform.position = respawnPosition;
        CameraController.instance.transform.position = cameraSpawnPosition;
        playerController.gameObject.SetActive(true);
    }

    // Set the respawn point of the player to be used with checkpoints
    public void SetSpawnPoint(Vector3 position)
    {
        respawnPosition = position;
        Debug.Log("Spawn point set");
    }

    // Method to facilitate Pause Menu
    public void PauseState()
    {
        // If the pause key is pressed in either state call/cancel the pause menu
        if (UIController.instance.pauseMenu.activeInHierarchy)
        {
            UIController.instance.pauseMenu.SetActive(false);
            Time.timeScale = 1f;  // Reset the time to 1 when we stop pause
            // Deactivate the mouse cursor
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            UIController.instance.pauseMenu.SetActive(true);
            Time.timeScale = 0f;  // Set the time to 0 to pause update loops
                                  // Reactivate the mouse cursor
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public IEnumerator LevelEnd()
    {
        AudioController.instance.PlayFX(2);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(nextLevel);
    }
}
