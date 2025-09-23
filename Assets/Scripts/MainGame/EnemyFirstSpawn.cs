using UnityEngine;

public class EnemyFirstSpawn : MonoBehaviour
{
    [SerializeField] private bool isStatic;
    private int screenHalfWidthMeters;
    private int screenHalfHeightMeters;
    private int minExtraRespawnHeight = 1;
    private int maxExtraRespawnHeight = 3;
    private GameObject camera;

    void Start()
    {
        screenHalfWidthMeters = (int)(Camera.main.orthographicSize * Camera.main.aspect);
        screenHalfHeightMeters = (int)(Camera.main.orthographicSize);
        camera = GameObject.Find("Main Camera");

        //Debug.Log(screenWidthMeters + "m");
        //Teleport enemy above camera at random X 
        float randomX = Random.Range(-screenHalfWidthMeters, screenHalfWidthMeters);
        //We need to add camera Y pos to random Y to ensure respawn is always above camera
        float randomY = Random.Range(screenHalfHeightMeters + minExtraRespawnHeight, screenHalfHeightMeters + maxExtraRespawnHeight) + camera.transform.position.y;
        transform.position = new Vector2(randomX, randomY);
        Debug.Log(transform.position);
    }
}
