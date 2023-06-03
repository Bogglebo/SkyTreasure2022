using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2BossActivator : MonoBehaviour
{
    public GameObject[] warriors;

    private void OnTriggerEnter(Collider other)
    {
        // When player enters the area activate the boss
        if (other.CompareTag("Player"))
        {
            // Make the boss appear
            AudioController.instance.PlayFX(7);
            foreach(GameObject go in warriors)
            {
                go.SetActive(true);
            }
            // Deactivate the trigger area once the boss is active
            gameObject.SetActive(false);
        }
    }
}
