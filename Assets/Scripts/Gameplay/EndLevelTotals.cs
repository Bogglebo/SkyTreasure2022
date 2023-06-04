using UnityEngine;
using UnityEngine.UI;

public class EndLevelTotals : MonoBehaviour
{
    // Script to display the totals at the end of the game

    // Variables to hold the total score & treasure count from player prefs
    private int totalScore;
    private int totalTreasure;
    // Components to be added to the inspector from the placeholders
    public Canvas canvas;
    public Text scoreText;
    public Text treasureText;


    void Start()
    {
        canvas = GetComponent<Canvas>();
        totalScore = PlayerPrefs.GetInt("Player Score");
        totalTreasure = PlayerPrefs.GetInt("Treasure Count");
        // Display information
        scoreText.text = "Total Score: " + totalScore + " of 15650";
        treasureText.text = "Total Treasure: " + totalTreasure + " of 77";
    }
}
