using UnityEngine;

public class EnemyRespawn : MonoBehaviour
{
    private float screenHalfWidthMeters;
    private float screenHalfHeightMeters;
    private float minExtraRespawnHeight = 2.0f;
    private float maxExtraRespawnHeight = 5.0f;
    private GameObject camera;

    [SerializeField] private GameObject staticEnemyPrefab;
    [SerializeField] private int respawnRate = 5; //Number of respawns before spawning new enemy
    private int respawnCount = 0;

    private void Start()
    {
        screenHalfWidthMeters = (Camera.main.orthographicSize * Camera.main.aspect);
        screenHalfHeightMeters = (Camera.main.orthographicSize);
        camera = GameObject.Find("Main Camera");
        Debug.Log(screenHalfWidthMeters + "m" + screenHalfHeightMeters + "m");
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
            //Debug.Log(randomX + " " + randomY);
            respawnCount++;
            if (respawnCount >= respawnRate)
            {
                respawnCount = 0;
                //Spawn new enemy off screen, should teleport due to EnemyFirstSpawn script
                Instantiate(staticEnemyPrefab, Vector3.zero, Quaternion.identity );
            }
        }
    }
}
