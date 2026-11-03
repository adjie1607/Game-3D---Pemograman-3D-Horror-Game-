using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float chaseRange = 8f;
    public float moveSpeed = 3.5f;

    private int currentPoint = 0;
    private Transform player;
    private NavMeshAgent agent;
    private bool chasing = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        GoToNextPoint();
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= chaseRange)
        {
            chasing = true;
            agent.SetDestination(player.position);
        }
        else if (chasing && distance > chaseRange + 3f)
        {
            chasing = false;
            GoToNextPoint();
        }

        if (!chasing && !agent.pathPending && agent.remainingDistance < 0.5f)
            GoToNextPoint();
    }

    void GoToNextPoint()
    {
        if (patrolPoints.Length == 0) return;

        agent.destination = patrolPoints[currentPoint].position;
        currentPoint = (currentPoint + 1) % patrolPoints.Length;
    }
}
