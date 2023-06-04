using UnityEngine;
using UnityEngine.SceneManagement;

// Script to manage Player score
public class ScoreController : MonoBehaviour
{
    // Create a static instance of the ScoreController
    public static ScoreController instance;

    // Variable to hold the score accessible throughout the game
    public int theScore = 0;
    // Variable to hold the tresure count accessible throughout the game
    public int treasureCount = 0;

    public void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        if (currentSceneName != "Level01")
        {
            // Start settings for all Levels other than Level 1
            theScore = PlayerPrefs.GetInt("Player Score");
            treasureCount = PlayerPrefs.GetInt("Treasure Count");
            UIController.instance.scoreText.text = "Score: " + theScore;
            UIController.instance.treasureCountText.text = "Treasure: " + treasureCount;
        }
        else  // Start settings for Level 1
        {
            UpdateScore(theScore);
            UIController.instance.treasureCountText.text = "Treasure: " + treasureCount;
        }
    }

    // Method to update the score
    public void UpdateScore(int changeScore)
    {
        theScore += changeScore;
        UIController.instance.scoreText.text = "Score: " + theScore;
    }

    // Method to update the treasure count
    public void UpdateTreasureCount()
    {
        treasureCount++;
        UIController.instance.treasureCountText.text = "Treasure: " + treasureCount;
    }
}
