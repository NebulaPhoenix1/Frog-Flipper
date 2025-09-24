using UnityEngine;

public class FollowObjectY : MonoBehaviour
{

    [SerializeField] private Transform target; // The object to follow
    [SerializeField] private float offset; // Optional offset on the Y axis
    public void UpdateY()
    {
        if (target != null)
        {
            Vector3 newPosition = transform.position;
            newPosition.y = target.position.y + offset;
            transform.position = newPosition;
        }
    }
    
}
