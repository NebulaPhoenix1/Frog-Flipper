using UnityEngine;

public class EnemyRespawn : MonoBehaviour
{

    [SerializeField] private bool isStatic;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Respawn")) //If enemy hits respawn trigger
        {
            Debug.Log("Respawn Triggered");
        }
    }
}
