using UnityEngine;

public class HeightFollow : MonoBehaviour
{
    [SerializeField] private Transform transformToFollow;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(transform.position.x, transformToFollow.position.y);
    }
}
