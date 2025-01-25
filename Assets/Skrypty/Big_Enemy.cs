using UnityEngine;
using System.Collections;

public class Big_Enemy : Enemy
{
    public float speed = 2.0f;          // Prędkość poruszania się
    public float attackRange = 1.5f;   // Zasięg ataku
    public int attackDamage = 2;       // Obrażenia zadawane graczowi
    public float attackCooldown = 2.0f; // Czas między atakami
    public float rotationSpeed = 5.0f; // Szybkość obrotu

    private Transform player;          // Pozycja gracza
    private float lastAttackTime = 0;  // Czas ostatniego ataku
    private Animator animator;         // Referencja do Animatora

    void Start()
    {
        // Pobranie Animatora z obiektu
        animator = GetComponent<Animator>();

        // Znalezienie gracza
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

        // Obliczenie dystansu do gracza
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Obracanie Big_Enemy w stronę gracza tylko w osi X
        RotateTowardsPlayer();

        if (distanceToPlayer > attackRange)
        {
            // Poruszanie się w kierunku gracza
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            // Wyłącz animację ataku, jeśli poruszamy się
            animator.SetBool("isAttacking", false);
        }
        else
        {
            // Atakowanie gracza
            AttackPlayer();
        }
    }

    void RotateTowardsPlayer()
    {
        // Obliczenie kierunku do gracza w poziomie (z uwzględnieniem osi X)
        Vector3 direction = (player.position - transform.position).normalized;

        // Obracamy postać tylko w osi X, zachowując jej rotację w innych osiach
        if (direction.x < 0)
        {
            // Gracz jest po prawej stronie, więc obracamy się w prawo
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * rotationSpeed);
        }
        else
        {
            // Gracz jest po lewej stronie, więc obracamy się w lewo
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0, 180, 0), Time.deltaTime * rotationSpeed);
        }
    }

    void AttackPlayer()
    {
        // Sprawdzenie, czy można zaatakować
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            Debug.Log("Big_Enemy attacks the player!");

            // Włącz animację ataku
            animator.SetBool("isAttacking", true);

            PlayerStats playerStats = player.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(attackDamage); // Zadanie obrażeń graczowi
            }

            lastAttackTime = Time.time; // Zaktualizowanie czasu ataku

            // Wyłącz animację po zakończeniu ataku
            StartCoroutine(ResetAttackAnimation());
        }
    }

    IEnumerator ResetAttackAnimation()
    {
        yield return new WaitForSeconds(1.0f); // Czas trwania animacji ataku
        animator.SetBool("isAttacking", false);
    }

    public override void TakeDamage(int damage)
    {
        // Zmniejszenie obrażeń o połowę
        int reducedDamage = damage / 2;
        base.TakeDamage(reducedDamage);
        Debug.Log($"Big_Enemy took reduced damage: {reducedDamage}!");
    }
}
