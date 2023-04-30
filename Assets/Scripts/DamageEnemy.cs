using UnityEngine;

public class DamageEnemy : MonoBehaviour
{


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyHealthController>().EnemyDamaged();
        }
    }
}
