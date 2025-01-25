using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
  [Header("Patrolling")]
  public Transform[] patrolPoints;
  public float patrolSpeed = 3.5f;

  [Header("Chasing")]
  public Transform player;
  public float chaseSpeed = 5f;
  public float detectionRange = 10f;
  public float loseRange = 15f;

  [Header("Field of View")]
  public float fieldOfView = 45f;

  private NavMeshAgent navMeshAgent;
  private int currentPatrolIndex;
  private bool isChasing = false;

  private void Start()
  {
    navMeshAgent = GetComponent<NavMeshAgent>();
    navMeshAgent.speed = patrolSpeed;

    currentPatrolIndex = 0;
    GoToNextPatrolPoint();
  }

  private void Update()
  {
    if (isChasing)
    {
      ChasePlayer();
    }

    else
    {
      Patrol();

      if (CanSeePlayer())
      {
        StartChasing();
      }
    }
  }

  private void Patrol()
  {
    if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.5f)
    {
      GoToNextPatrolPoint();
    }
  }

  private void GoToNextPatrolPoint()
  {
    if (patrolPoints.Length.Equals(0)) return;

    navMeshAgent.destination = patrolPoints[currentPatrolIndex].position;
    currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
  }

  private void ChasePlayer()
  {
    float distanceToPlayer = Vector3.Distance(transform.position, player.position);
    if (distanceToPlayer > loseRange)
    {
      StopChasing();
    }
    else
    {
      navMeshAgent.destination = player.position; 
     }
  }

  private void StartChasing()
  {
    isChasing = true;
    navMeshAgent.speed = chaseSpeed;
        
  }

  private void StopChasing()
  {
    isChasing = false;
    navMeshAgent.speed = patrolSpeed;
    GoToNextPatrolPoint();
  }

  private bool CanSeePlayer()
  {
    float distanceToPlayer = Vector3.Distance(transform.position, player.position);

    if (distanceToPlayer > detectionRange) return false;

    Vector3 directionToPlayer = (player.position - transform.position).normalized;
    float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

    if (angleToPlayer > fieldOfView / 2f) return false;

    if (Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit, detectionRange))
    {
      if (hit.transform.CompareTag("Player"))
      {
        return true;
      }
    }

    return false;
  }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag(Constants.Player_TAG))
            GameManager.Instance.CanvasDead();
    }
}
