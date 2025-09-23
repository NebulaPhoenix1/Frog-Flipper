using UnityEngine;

public class EnemyFirstSpawn : MonoBehaviour
{
    [SerializeField] private bool isStatic;
    private float screenHalfWidthMeters;
    private float screenHalfHeightMeters;
    private float minExtraRespawnHeight = 1.0f;
    private float maxExtraRespawnHeight = 6.0f;
    private float cameraSpawnMinHeight = 2.0f;
    private GameObject camera;
    private float randomY, randomX;

    //If first spawn should be within camera view (used for very first set of enemies)
    [SerializeField] private bool spawnInCameraSpace = false;
    void Start()
    {
        screenHalfWidthMeters = (Camera.main.orthographicSize * Camera.main.aspect);
        screenHalfHeightMeters = (Camera.main.orthographicSize);
        camera = GameObject.Find("Main Camera");

        //Debug.Log(screenWidthMeters + "m");
        //Teleport enemy above camera at random X 
        float randomX = Random.Range(-screenHalfWidthMeters, screenHalfWidthMeters);
        if (spawnInCameraSpace)
        {
            //Spawn slightly above the player and within camera view
            randomY = Random.Range(cameraSpawnMinHeight, screenHalfHeightMeters);
        }
        else
        {
            randomY = Random.Range(screenHalfHeightMeters + minExtraRespawnHeight, screenHalfHeightMeters + maxExtraRespawnHeight) + camera.transform.position.y;
        }
        //We need to add camera Y pos to random Y to ensure respawn is always above camera

        transform.position = new Vector2(randomX, randomY);
        //Debug.Log(transform.position);
    }

    //If enemy is within another, just try to spawn again
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")) //If enemy hits respawn trigger
        {
            randomX = Random.Range(-screenHalfWidthMeters, screenHalfWidthMeters);
            randomY = Random.Range(screenHalfHeightMeters + minExtraRespawnHeight, screenHalfHeightMeters + maxExtraRespawnHeight) + camera.transform.position.y;
            transform.position = new Vector2(randomX, randomY);
            Debug.Log("Respawned to: " + transform.position);
        }
    }
}
