using UnityEngine;

// Script to manage Player score
public class ScoreController : MonoBehaviour
{
    // Create a static instance of the ScoreController
    public static ScoreController instance;

    // Static variable to hold the score accessible throughout the game
    public static int theScore;

    public void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        theScore = 0;
        UpdateScore(0);
    }

    // Method to update the score
    public void UpdateScore(int changeScore)
    {
        theScore += changeScore;
        UIController.instance.scoreText.text = "Score: " + theScore;
    }
}
