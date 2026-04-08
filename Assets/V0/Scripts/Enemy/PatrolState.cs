using UnityEngine;
using UnityEngine.AI;

public class PatrolState : EnemyState
{
    private Vector3 patrolPoint;
    EnemyController enemy;

    public PatrolState(EnemyController enemy) : base(enemy)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        SetNewPatrolPoint();
    }

    public override void Update()
    {

        enemy.agent.SetDestination(patrolPoint);

        // Switch to Chase
        if (enemy.DistanceToPlayer() < enemy.chaseRange)
        {
            enemy.ChangeState(enemy.chaseState);
        }

        // If reached point → pick new one
        if (Vector3.Distance(enemy.transform.position, patrolPoint) < 0.1f)
        {
            SetNewPatrolPoint();
            Debug.Log("reached new point set to -> patrol point " + patrolPoint);
        }
    }

    private void SetNewPatrolPoint()
    {
        for (int i = 0; i < 10; i++)
        {
            Vector3 randomDir = new Vector3(
                Random.Range(-10f, 10f),
                0f,
                Random.Range(-10f, 10f)
            );

            Vector3 targetPos = enemy.transform.position + randomDir;

            // ❗ ensure it's not too close
            if (Vector3.Distance(enemy.transform.position, targetPos) < 3f)
                continue;

            NavMeshHit hit;

            if (NavMesh.SamplePosition(targetPos, out hit, 10f, NavMesh.AllAreas))
            {
                patrolPoint = hit.position;
                return;
            }
        }

        patrolPoint = enemy.transform.position;
    }
}