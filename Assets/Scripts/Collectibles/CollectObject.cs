using UnityEngine;

public class CollectObject : MonoBehaviour
{
    // Check tag of object to manage the collision process
    // Update score, play sound and deactivate objects

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            string item = gameObject.tag;
            switch (item)
            {
                case "Coin":
                    AudioController.instance.PlayFX(0);
                    ScoreController.instance.UpdateScore(50);
                    this.gameObject.SetActive(false);
                    break;

                case "Gem":
                    AudioController.instance.PlayFX(1);
                    ScoreController.instance.UpdateScore(100);
                    this.gameObject.SetActive(false);
                    break;
                default:
                    Debug.Log("Object has no tag");
                    break;
            }
        }
    }




}
