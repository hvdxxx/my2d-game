using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeapon : MonoBehaviour
{
    [Header("Настройки оружия")]
    public GameObject projectilePrefab;     // Префаб снаряда (шарик/стрела)
    public float projectileSpeed = 12f;      // Скорость полета
    public float damage = 25f;               // Урон
    public float fireRate = 0.4f;            // Задержка между выстрелами

    private float nextFireTime = 0f;
    private InputSystem_Actions inputActions;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        // Подписываемся на действие Shoot (обычно это ЛКМ в Input Actions)
        inputActions.Player.Shoot.performed += OnShoot;
    }

    private void OnDisable()
    {
        inputActions.Player.Shoot.performed -= OnShoot;
        inputActions.Player.Disable();
    }

    private void OnShoot(InputAction.CallbackContext context)
    {
        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Shoot();
        }
    }

    private void Shoot()
    {
        if (projectilePrefab == null) return;

        // 1. Получаем позицию мыши на экране
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();

        // 2. Превращаем экранные координаты в мировые (где стоят объекты игры)
        // Важно: берем Z как у камеры, чтобы расчет был плоским
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, Mathf.Abs(Camera.main.transform.position.z)));

        // Нам не нужна глубина (Z), обнуляем её для 2D
        mouseWorldPos.z = 0;

        // 3. Вычисляем направление: (Куда хотим попасть - Откуда стреляем)
        Vector2 direction = ((Vector2)mouseWorldPos - (Vector2)transform.position).normalized;

        // 4. Создаем снаряд в позиции игрока
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        // 5. Передаем данные в скрипт снаряда
        Projectile projScript = projectile.GetComponent<Projectile>();
        if (projScript != null)
        {
            // Передаем и направление, и скорость (чтобы менять её в одном месте)
            projScript.Launch(direction, projectileSpeed, damage);
        }
    }
    public Vector2 GetMouseDirection()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 direction = (mousePosition - (Vector2)transform.position).normalized;
        return direction;
    }
}