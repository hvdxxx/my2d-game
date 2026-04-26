using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;

    // Хэши параметров
    private readonly int horizontalHash = Animator.StringToHash("Horizontal");
    private readonly int verticalHash = Animator.StringToHash("Vertical");
    private readonly int isMovingHash = Animator.StringToHash("IsMoving");

    // Запоминаем последнее направление
    public Vector2 lastDirection = new Vector2(0, -1); // начинаем лицом вниз

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Vector2 input = GetComponent<Player_controller>().movementInput;

        bool isMovingNow = input.magnitude > 0.01f;

        if (isMovingNow)
        {
            // Обновляем последнее направление только когда движемся
            lastDirection = input.normalized;
        }

        // Всегда передаём в Animator последнее направление
        animator.SetFloat(horizontalHash, lastDirection.x);
        animator.SetFloat(verticalHash, lastDirection.y);

        // Переключаем между Idle и Walk
        animator.SetBool(isMovingHash, isMovingNow);
    }
}