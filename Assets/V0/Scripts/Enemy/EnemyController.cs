using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;

    [Header("Settings")]
    public float chaseRange = 10f;
    public float attackRange = 2f;

    private EnemyStateMachine stateMachine;

    // States
    public PatrolState patrolState;
    public ChaseState chaseState;
    public AttackState attackState;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        stateMachine = new EnemyStateMachine();

        patrolState = new PatrolState(this);
        chaseState = new ChaseState(this);
        attackState = new AttackState(this);
    }

    private void Start()
    {
        stateMachine.Initialize(patrolState);
    }

    private void Update()
    {
        stateMachine.Update();
    }

    public void ChangeState(EnemyState newState)
    {
        stateMachine.ChangeState(newState);
    }

    public float DistanceToPlayer()
    {
        return Vector3.Distance(transform.position, player.position);
    }
}