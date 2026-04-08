using UnityEngine;

public class AttackState : EnemyState
{
    private float attackCooldown = 1.5f;
    private float lastAttackTime;

    public AttackState(EnemyController enemy) : base(enemy) { }

    public override void Enter()
    {
        enemy.agent.isStopped = true;
    }

    public override void Exit()
    {
        enemy.agent.isStopped = false;
    }

    public override void Update()
    {
        RotateTowardsTarget(); 

        float distance = enemy.DistanceToPlayer();

        if (distance > enemy.attackRange)
        {
            enemy.ChangeState(enemy.chaseState);
            return;
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

    private void Attack()
    {
        Debug.Log("Enemy Attacks!");
    }
}