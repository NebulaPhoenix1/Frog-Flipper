using UnityEngine;
using UnityEngine.Events;

public class EnemyCollision : MonoBehaviour
{
    public UnityEvent onPlayerHit;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Enemy hit player");
            // Here you can add code to handle what happens when the enemy collides with the player
            onPlayerHit.Invoke();
        }
    }
}
