using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class EnemyCollision : MonoBehaviour
{
    public UnityEvent onPlayerHit;
    private Rigidbody2D playerRB;
    private Camera mainCamera;
    private InputAction jumpAction;

    //Camera zoom on to enemy we hit
    [SerializeField] private float zoomDuration = 0.5f;
    [SerializeField] private float zoomAmount = 2.0f;

    private void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
        jumpAction = InputSystem.actions.FindAction("Jump"); //Get action for jump (left mouse)
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Enemy hit player");
            jumpAction.Disable(); //Disable jump input
            playerRB.linearVelocity = Vector2.zero;
            // Here you can add code to handle what happens when the enemy collides with the player
            onPlayerHit.Invoke();
        }
    }
}
