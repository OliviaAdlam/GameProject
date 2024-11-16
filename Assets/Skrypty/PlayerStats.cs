using UnityEngine;
using System.Collections;

public class PlayerStats : MonoBehaviour
{
    public int maxLives = 10; 
    private int currentLives;

    private int playerScore = 0; // to potem dodajemy do monetek w domku

    public Transform attackPoint; // Punkt, z którego atakujemy
    public float attackRange = 1f; // zasięg ataku
    public int attackDamage = 1; // Obrażenia zadawane przeciwnikom
    public LayerMask enemyLayers; //Warstwa dla przeciwników
    public GameObject sword; // referencja do miecza

    private PlayerMove playerMove; // Odwołanie do skryptu PlayerMove

    void Start()
    {
        currentLives = 3;
        // będzie wyświetlanie życia w górnym rogu
        sword.SetActive(false); // Ukryj miecz na start
        playerMove = GetComponent<PlayerMove>(); // Pobierz skrypt PlayerMove
    }

    void Update()
    {
        //update pozycji "miecza"
        UpdateSwordPosition();

        // Atak po naciśnięciu Spacji
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(PerformAttack());
        }
    }

    // Perform attack with the sword
    IEnumerator PerformAttack()
    {
        sword.SetActive(true); // Show the sword when attacking

        // Znajdź przeciwników w zasięgu
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(attackDamage);
                Debug.Log("Zadano obrażenia przeciwnikowi: " + enemy.name);
            }
        }

        // Poczekaj chwilę, aby symulować czas trwania ataku
        yield return new WaitForSeconds(0.3f);

        // Ukryj miecz
        sword.SetActive(false);
    }

    // Zmień pozycję "miecza"
    void UpdateSwordPosition()
    {
        if (sword == null) return;

        // Ustaw pozycję miecza biorąc pod uwagę kierunek ruchu
        Vector3 swordPosition = playerMove.transform.position;

        // Dopasowanie pozycji miecza do kierunku ruchu gracza
        if (playerMove != null)
        {
            swordPosition = playerMove.sword.transform.position; // Pozyskaj pozycje obliczoną w playerMove
        }

        // Zaktualizuj pozycję miecza
        sword.transform.position = swordPosition;
    }

    

    // Kolizja z monetami i przeciwnikami
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            Coin coin = other.gameObject.GetComponent<Coin>();
            if (coin != null)
            {
                playerScore += coin.value;
                Debug.Log("MoNety: " + playerScore);
                Destroy(other.gameObject);
            }
        }

        if (other.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                TakeDamage(enemy.damage);
                Debug.Log("Oberwano w twarz!");
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentLives -= damage;
        Debug.Log("Pozostało żyć: " + currentLives);

        if (currentLives <= 0)
        {
            Debug.Log("Gracz zginął!");
            Destroy(gameObject); //Zniszcz owce
        }
    }

    public void Heal(int amount)
    {
        // Bossy mogą losowo wypadać serca na leczenie
    }
}
