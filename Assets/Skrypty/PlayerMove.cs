using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 5f; // prędkość ruchu
    public float moveHorizontal { get; private set; }
    public float moveVertical { get; private set; }

    private Vector2 lastMovementDirection; // zmienna do przechowywania ostatniego kierunku ruchu
    public GameObject sword; // referencja do obiektu miecza

    void Start()
    {
        lastMovementDirection = Vector2.zero; // początkowo brak ruchu
    }

    void Update()
    {
        // Pobierz dane wejściowe gracza (strzałki lub WASD)
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");

        // Oblicz wektor ruchu
        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0);

        // Zaktualizuj pozycję gracza
        transform.position += movement * moveSpeed * Time.deltaTime;

        // Zaktualizuj ostatni kierunek ruchu
        if (movement.magnitude > 0)
        {
            lastMovementDirection = movement.normalized; // Normalizujemy wektor, aby kierunek był jedynie istotny
        }

        // Ustaw pozycję miecza na podstawie ostatniego kierunku ruchu
        PositionSword();
    }

    void PositionSword()
    {
        if (sword == null) return;

        Vector3 swordPosition = transform.position; // Rozpoczynamy od pozycji gracza

        // Dopasowanie pozycji miecza do kierunku ruchu
        if (lastMovementDirection != Vector2.zero)
        {
            if (lastMovementDirection.x < 0) // Ruch w lewo
            {
                swordPosition.x = transform.position.x - 2f; // Ustaw miecz po lewej stronie
                swordPosition.y = transform.position.y;     // Pozostaw Y bez zmian
            }
            else if (lastMovementDirection.x > 0) // Ruch w prawo
            {
                swordPosition.x = transform.position.x + 2f; // Ustaw miecz po prawej stronie
                swordPosition.y = transform.position.y;     // Pozostaw Y bez zmian
            }

            if (lastMovementDirection.y < 0) // Ruch w dół
            {
                swordPosition.y = transform.position.y - 2f; // Ustaw miecz poniżej gracza
                swordPosition.x = transform.position.x;     // Pozostaw X bez zmian
            }
            else if (lastMovementDirection.y > 0) // Ruch w górę
            {
                swordPosition.y = transform.position.y + 2f; // Ustaw miecz powyżej gracza
                swordPosition.x = transform.position.x;     // Pozostaw X bez zmian
            }
        }

        // Zaktualizuj pozycję miecza
        sword.transform.position = swordPosition;
    }
}
