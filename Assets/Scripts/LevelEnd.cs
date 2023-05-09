using UnityEngine;


// Script to handle activity when player stands on level end portal

public class LevelEnd : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(SystemController.instance.LevelEnd());
        }
    }
}
