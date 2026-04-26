using UnityEngine;

public class Echo : MonoBehaviour
{
    [Header("Настройки призрака")]
    public GameObject projectilePrefab; // Префаб пули (тот же, что у игрока)
    public float damageMultiplier = 0.5f; // Урон призрака (50% от твоего)
    public float fireRate = 1.0f;       // Скорость стрельбы

    private float nextFireTime;
    private Vector2 shootDirection;
    private float damage;

    // Этот метод мы вызовем из скрипта игрока при создании Эха
    public void Initialize(Vector2 direction, float playerDamage, GameObject prefab)
    {
        shootDirection = direction;
        damage = playerDamage * damageMultiplier;
        projectilePrefab = prefab;

        if (direction.x != 0)
        {
            // Если летит влево (x < 0), зеркалим спрайт по оси X
            GetComponent<SpriteRenderer>().flipX = direction.x < 0;
        }

        // Делаем его полупрозрачным программно
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            Color c = sr.color;
            c.a = 0.5f; // 50% прозрачности
            c.b = 1f;   // Добавим синевы
            sr.color = c;
        }
    }

    void Update()
    {
        // Эхо просто стоит и стреляет в заданном направлении
        if (Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        if (projectilePrefab == null) return;

        GameObject bullet = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        Projectile projScript = bullet.GetComponent<Projectile>();

        if (projScript != null)
        {
            // Используем твой метод Launch! 
            // 12f — это скорость, можешь поменять на любую
            projScript.Launch(shootDirection, 12f, damage);
        }
    }
}