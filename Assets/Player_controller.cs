using UnityEngine;
using UnityEngine.InputSystem;

public class Player_controller : MonoBehaviour
{
    public float speed = 5f;

    private InputSystem_Actions controls; // Убедись, что название совпадает с твоим классом (у тебя было InputSystem_Actions)
    public Vector2 movementInput;

    void Awake()
    {
        controls = new InputSystem_Actions();

        // Подписываемся на движение
        controls.Player.Move.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => movementInput = Vector2.zero;
    }

    void OnEnable()
    {
        controls.Player.Enable();
    }

    void OnDisable()
    {
        controls.Player.Disable();
    }

    void Update()
    {
        // 1. ПЕРЕДВИЖЕНИЕ
        Vector3 moveDirection = new Vector3(movementInput.x, movementInput.y, 0f);
        if (moveDirection.magnitude > 0.01f)
        {
            moveDirection.Normalize();
        }
        transform.position += moveDirection * speed * Time.deltaTime;

        // 2. ПОВОРОТ ЗА МЫШКОЙ
        RotateTowardsMouse();
    }

    private void RotateTowardsMouse()
    {
        // Получаем позицию мыши на экране
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();

        // Переводим её в мировые координаты (как мы делали в скрипте оружия)
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, Mathf.Abs(Camera.main.transform.position.z)));
        mouseWorldPos.z = 0; // Нам не нужна глубина в 2D

        // Вычисляем направление от персонажа к мыши
        Vector2 lookDirection = (Vector2)mouseWorldPos - (Vector2)transform.position;

        // Считаем угол в градусах
        // Atan2 возвращает угол в радианах, поэтому умножаем на Rad2Deg
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

        // Поворачиваем объект вокруг оси Z
        // Если твой персонаж нарисован смотрящим ВПРАВО, то вычитай 0.
        // Если персонаж нарисован смотрящим ВВЕРХ, вычти 90 (angle - 90f).
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}