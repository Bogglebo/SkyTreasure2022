using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    // Static variable accesible from anywhere in the game
    public static int theScore;

    // Create a static instance of the UI Controller 
    public static UIController instance;

    // UI variables containing health and score
    public Text healthText;
    public Text scoreText;

    // UI variable for pause menu
    public GameObject pauseMenu;

    // Black image background used for fading
    public Image blackScreen;
    public float fadeSpeed = 2f;
    public bool fadeToBlack, fadeFromBlack;

    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeToBlack)
        {
            // Make the colour go from 0 to 1 (Alpha value)
            // transparent to opaque
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g,
                blackScreen.color.b, Mathf.MoveTowards(blackScreen.color.a, 1f,
                fadeSpeed * Time.deltaTime));
            // Once the black screen is at 255, stop trying to fade to black
            if (blackScreen.color.a == 1f)
            {
                fadeToBlack = false;
            }
        }

        if (fadeFromBlack)  // Opaque to transparent
        {
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g,
                blackScreen.color.b, Mathf.MoveTowards(blackScreen.color.a, 0f,
                fadeSpeed * Time.deltaTime));
            if (blackScreen.color.a == 0f)
            {
                fadeFromBlack = false;
            }
        }
    }

    // When player chooses Resume from the Pause Menu
    public void Resume()
    {
        SystemController.instance.PauseState();
    }

    // When player chooses Main Menu from the Pause Menu
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
