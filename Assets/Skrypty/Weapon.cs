using UnityEngine;

public class Weapon : MonoBehaviour
{
    public string weaponName;
    public float damage;
    public float attackSpeed;
    public Sprite weaponSprite;
    private PlayerMove playerMove;
    private GameObject player;
    private Vector3 relativePosition;
    public float offsetDistance = 2f;
    private float weaponLength;
    private Transform attackPoint;

    public void SetInitialPosition(Vector3 position)
    {
        transform.position = position;
    }

    public void SetPlayer(GameObject player)
    {
        this.player = player;
        playerMove = player.GetComponent<PlayerMove>();
    }

    void Start()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer && weaponSprite)
        {
            spriteRenderer.sprite = weaponSprite;
            // Używamy pełnego rozmiaru sprite'a
            weaponLength = spriteRenderer.bounds.size.y;
        }

        attackPoint = transform.Find("AttackPoint");
        if (attackPoint != null)
        {
            UpdateAttackPointPosition();
        }
        
        if (GlobalData.currentWeaponName == weaponName)
        {
            damage = GlobalData.currentWeaponDamage;
            attackSpeed = GlobalData.currentWeaponSpeed;
        }
    }

    void UpdateAttackPointPosition()
    {
        if (attackPoint != null)
        {
            // Ustawiamy attack point na samym końcu broni
            attackPoint.localPosition = Vector3.up * (weaponLength / 2f);
        }
    }

    void Update()
    {
        if (player != null && playerMove != null)
        {
            Vector2 direction = playerMove.GetLastMovementDirection();
            if (direction == Vector2.zero) direction = Vector2.right;

            Vector3 targetPosition = player.transform.position + (Vector3)(direction.normalized * offsetDistance);
            transform.position = targetPosition;

            // Zmieniamy kąt na -90 stopni (było +90)
            float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) - 90f;
            transform.rotation = Quaternion.Euler(0, 0, angle);
            
            // Aktualizujemy pozycję attack pointa
            UpdateAttackPointPosition();
        }
    }
} 