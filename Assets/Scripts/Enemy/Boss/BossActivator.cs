using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossActivator : MonoBehaviour
{

    public GameObject actionBoss;

    private void OnTriggerEnter(Collider other)
    {
        // When player enters the area activate the boss
        if (other.CompareTag("Player"))
        {
            AudioController.instance.PlayFX(7);
            actionBoss.SetActive(true);
            
            // Deactivate the trigger area once the boss is active
            gameObject.SetActive(false);
        }
    }

}
