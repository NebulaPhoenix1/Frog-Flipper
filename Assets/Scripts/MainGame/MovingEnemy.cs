using UnityEngine;

public class MovingEnemy : MonoBehaviour
{
    //Enemy that will move between two points
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float speed = 2f;
    private bool movingToA = true;
    private Vector3 currentTarget;

    void Start()
    {
        currentTarget = pointA.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position =  Vector3.MoveTowards(transform.position, currentTarget, speed * Time.deltaTime);
        if(Vector3.Distance(transform.position, currentTarget) < 0.1f)
        {
            movingToA = !movingToA;
            currentTarget = movingToA ? pointA.position : pointB.position;
        }
    }
}
