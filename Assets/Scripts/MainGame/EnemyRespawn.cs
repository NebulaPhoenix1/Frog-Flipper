using UnityEngine;

public class EnemyRespawn : MonoBehaviour
{
    [SerializeField] private bool isStatic;
    private int screenHalfWidthMeters;
    private int screenHalfHeightMeters;
    private int minExtraRespawnHeight = 2;
    private int maxExtraRespawnHeight = 5;
    private GameObject camera;

    private void Start()
    {
        screenHalfWidthMeters = (int)(Camera.main.orthographicSize * Camera.main.aspect);
        screenHalfHeightMeters = (int)(Camera.main.orthographicSize);
        camera = GameObject.Find("Main Camera");
        //Debug.Log(screenWidthMeters + "m");
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision with: " + collision.name);
        if (collision.CompareTag("Enemy")) //If enemy hits respawn trigger
        {
            //Debug.Log("Respawn Triggered");
            //Teleport enemy above camera at random X 
            float randomX = Random.Range(-screenHalfWidthMeters, screenHalfWidthMeters);
            //We need to add camera Y pos to random Y to ensure respawn is always above camera
            float randomY = Random.Range(screenHalfHeightMeters + minExtraRespawnHeight, screenHalfHeightMeters + maxExtraRespawnHeight) + camera.transform.position.y;
            collision.transform.position = new Vector2(randomX, randomY);
            Debug.Log(transform.position);
        }
    }
}
