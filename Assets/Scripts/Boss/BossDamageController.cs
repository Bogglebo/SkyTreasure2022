using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDamageController : MonoBehaviour
{
    public BossBattleController bossController;
    private PlayerController playerController;

    private void Start()
    {
        playerController = PlayerController.instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bossController.DamageBoss();
            playerController.Bounce();
        }
    }


}
