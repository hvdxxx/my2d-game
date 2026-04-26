using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage = 25f;
    public float lifetime = 3f; // Жизнь снаряда (чтобы не летел вечно)

    private Rigidbody2D rb;

    void Awake()
    {
        // Снаряд ОБЯЗАТЕЛЬНО должен иметь компонент Rigidbody2D!
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        // Чтобы не засорять память, удаляем через 3 секунды
        Destroy(gameObject, lifetime);
    }

    // Метод для запуска снаряда
    public void Launch(Vector2 direction, float speed, float damageValue)
    {
        this.damage = damageValue;

        if (rb != null)
        {
            // Даем скорость физическому телу
            rb.linearVelocity = direction * speed;

            // (Дополнительно) Разворачиваем снаряд «лицом» по направлению полета
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Проверяем, не враг ли это
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                enemy.Infect(); // Твоя механика заражения
            }

            // После попадания удаляем снаряд
            Destroy(gameObject);
        }
    }
}