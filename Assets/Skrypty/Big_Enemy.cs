using UnityEngine;
using System.Collections;

public class Big_Enemy : Enemy
{
    public float speed = 2.0f;
    public float attackRange = 1.5f;
    public int attackDamage = 2;
    public float attackCooldown = 2.0f;
    public float rotationSpeed = 5.0f;

    private Transform player;
    private float lastAttackTime = 0;
    private Animator animator;

    void Start()
    {
        base.Start();

        animator = GetComponent<Animator>();
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("Player not found in the scene!");
        }
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        RotateTowardsPlayer();

        if (distanceToPlayer > attackRange)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
            animator.SetBool("isAttacking", false);
        }
        else
        {
            AttackPlayer();
        }
    }

    void RotateTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        if (direction.x < 0)
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * rotationSpeed);
        }
        else
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0, 180, 0), Time.deltaTime * rotationSpeed);
        }
    }

    void AttackPlayer()
    {
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            Debug.Log("Big_Enemy attacks the player!");
            animator.SetBool("isAttacking", true);
            PlayerStats playerStats = player.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(attackDamage);
            }
            lastAttackTime = Time.time;
            StartCoroutine(ResetAttackAnimation());
        }
    }

    IEnumerator ResetAttackAnimation()
    {
        yield return new WaitForSeconds(1.0f);
        animator.SetBool("isAttacking", false);
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
    }
}
