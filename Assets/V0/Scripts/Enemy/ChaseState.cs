using UnityEngine;

public class ChaseState : EnemyState
{
    public ChaseState(EnemyController enemy) : base(enemy) { }

    public override void Update()
    {
        enemy.agent.SetDestination(enemy.player.position);

        RotateTowardsTarget(); // 👈 ADD THIS

        float distance = enemy.DistanceToPlayer();

        if (distance < enemy.attackRange)
        {
            enemy.ChangeState(enemy.attackState);
        }
        else if (distance > enemy.chaseRange)
        {
            enemy.ChangeState(enemy.patrolState);
        }
    }
    private void RotateTowardsTarget()
    {
        Vector3 direction = (enemy.player.position - enemy.transform.position).normalized;
        direction.y = 0f;

        if (direction == Vector3.zero) return;

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        enemy.transform.rotation = Quaternion.Slerp(
            enemy.transform.rotation,
            lookRotation,
            Time.deltaTime * 10f
        );
    }
}