using UnityEngine;
using UnityEngine.SceneManagement;

// Script to manage Player score
public class ScoreController : MonoBehaviour
{
    // Create a static instance of the ScoreController
    public static ScoreController instance;

    // Static variable to hold the score accessible throughout the game
    public int theScore = 0;

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
            theScore = PlayerPrefs.GetInt("Player Score");
            UIController.instance.scoreText.text = "Score " + theScore;
            Debug.Log("We are not in Level 1 and the score is " + theScore);
        }
        else
        {
            Debug.Log("We are in Level 1 and the score should be " + theScore);
            UpdateScore(theScore);
        }
    }

    // Method to update the score
    public void UpdateScore(int changeScore)
    {
        theScore += changeScore;
        UIController.instance.scoreText.text = "Score: " + theScore;
    }
}
