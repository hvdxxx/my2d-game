using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Характеристики врага")]
    public float maxHealth = 50f;
    public float moveSpeed = 2.5f;
    public GameObject expGemPrefab;

    private float currentHealth;
    private Transform player;
    private bool isInfected = false;
    private float infectionTimer = 0f;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Простой ИИ — идёт к игроку
        if (player != null)
        {
            Vector2 direction = ((Vector2)player.position - (Vector2)transform.position).normalized;
            transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;
        }

        // Если заражён — урон со временем + распространение
        if (isInfected)
        {
            infectionTimer += Time.deltaTime;
            if (infectionTimer >= 1f) // каждую секунду
            {
                TakeDamage(8f);       // DoT урон
                infectionTimer = 0f;
                SpreadInfection();
            }
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.red;
            Invoke("ResetColor", 0.15f);
        }

        if (currentHealth <= 0)
        {
            // --- НОВЫЙ БЛОК ДЛЯ ОПЫТА ---
            // Ищем на игроке скрипт LevelSystem
            LevelSystem levelSystem = player.GetComponent<LevelSystem>();
            if (expGemPrefab != null)
            {
                Instantiate(expGemPrefab, transform.position, Quaternion.identity);
            }
            if (levelSystem != null)
            {
                levelSystem.AddExperience(20); // Даем 20 единиц опыта за врага
            }

            // Сообщаем спавнеру, что враг умер (чтобы он мог спавнить новых)
            EnemySpawner spawner = Object.FindAnyObjectByType<EnemySpawner>();
            if (spawner != null) spawner.OnEnemyDied();

            Destroy(gameObject);

        }
    }

    public void ScaleStats(int playerLevel)
    {
        // Увеличиваем ХП на 15% за каждый уровень игрока
        float multiplier = 1f + (playerLevel * 0.10f);
        maxHealth *= multiplier;
        currentHealth = maxHealth;

        // Можно и скорость чуть-чуть поднять
        moveSpeed += (playerLevel * 0.02f);
    }

    private void ResetColor()
    {
        if (spriteRenderer != null && isInfected)
            spriteRenderer.color = Color.green;
        else if (spriteRenderer != null)
            spriteRenderer.color = Color.white;
    }

    public void Infect()
    {
        if (!isInfected)
        {
            isInfected = true;
            spriteRenderer.color = Color.green; // визуально видно заражение
        }
    }

    private void SpreadInfection()
    {
        Collider2D[] nearbyEnemies = Physics2D.OverlapCircleAll(transform.position, 3f);
        foreach (Collider2D col in nearbyEnemies)
        {
            if (col.CompareTag("Enemy") && col.gameObject != gameObject)
            {
                Enemy otherEnemy = col.GetComponent<Enemy>();
                if (otherEnemy != null)
                {
                    otherEnemy.Infect();
                }
            }
        }
    }

    // Для отладки — рисуем радиус заражения в редакторе
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 3f);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                // Наносим 10 урона в секунду (через Time.deltaTime)
                playerHealth.TakeDamage(10f * Time.deltaTime);
            }
        }
    }

}