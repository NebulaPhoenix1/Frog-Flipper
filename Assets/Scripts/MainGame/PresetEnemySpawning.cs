using System.Collections.Generic;
using UnityEngine;

public class PresetEnemySpawning : MonoBehaviour
{
    /* We have an array of spawning presets
     * When we start a level, pick one at random
     * When we pass them all, spawn a new random one above the last 
     * We should have 2 enemy presets spawned in at once 
     * Using a queue means we can easily remove the oldest preset when we want to
     */

    [SerializeField] private GameObject[] spawnPresets;
    [SerializeField] private float presetHeight;
    [SerializeField] private float nextSpawnY = 1.5f;
    private Queue<GameObject> activePresets = new Queue<GameObject>();

    void Start()
    {
        SpawnPreset();
        SpawnPreset();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Respawn")) //If player hits respawn trigger
        {
            //Spawn new preset above last
            SpawnPreset();
            //Remove oldest preset (if we have more than 3)
            if (activePresets.Count > 3)
            {
                GameObject oldest = activePresets.Dequeue();
                Destroy(oldest);
            }
        }
    }

    private void SpawnPreset()
    {
        //Instantiate random preset at start
        int randomIndex = Random.Range(0, spawnPresets.Length);
        GameObject spawned = Instantiate(spawnPresets[randomIndex], new Vector3(0, nextSpawnY, 0), Quaternion.identity);
        activePresets.Enqueue(spawned);
        nextSpawnY += presetHeight;
    }
}

