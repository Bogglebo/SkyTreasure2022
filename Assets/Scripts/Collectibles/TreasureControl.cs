using JetBrains.Annotations;
using System.Collections;
using System.Diagnostics.Tracing;
using UnityEditor.Build;
using UnityEngine;

public class TreasureControl : MonoBehaviour
    // Script to control combination of Treasure Chest animations & score
{
    // Variables to turn visibility on/off for various parts of chest
    public GameObject chestClosed, chestOpen, chestCoins;
    public int objectAmount = 500;        // Score value of chest
    // Counter to control how many times coroutine updates Score & Treasure count
    public int counter = 0; 

    // Start is called before the first frame update
    void Start()
    {
        chestClosed.SetActive(true);
        chestOpen.SetActive(false);
        chestCoins.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Combine animations for chest opening and closing
            chestClosed.SetActive(false);
            StartCoroutine(Waiter());
            // Update counter after coroutine called
            counter++;
            // Update score and treasure count on first coroutine call
            if (counter == 1)
            {
                ScoreController.instance.UpdateScore(objectAmount);
                ScoreController.instance.UpdateTreasureCount();
                AudioController.instance.PlayFX(2);
            }
        }
    }
    public IEnumerator Waiter()
    {
        chestOpen.SetActive(true);
        chestCoins.SetActive(true);
        yield return new WaitForSeconds(1);
        chestCoins.SetActive(false);
        chestOpen.SetActive(false);
        this.GetComponentInParent<ParticleSystem>().Stop();
        // Disable box collider to prevent treasure chest respawning
        GameObject disableTreasure = GameObject.Find("TreasureChest");
        BoxCollider boxCollider = disableTreasure.GetComponent<BoxCollider>();
        boxCollider.enabled = false;
    }
}
