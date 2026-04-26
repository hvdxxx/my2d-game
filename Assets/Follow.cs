using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform target;           // Сюда перетащи своего Player

    [Header("Settings")]
    public Vector3 offset = new Vector3(0, 0, -10); // Отступ камеры (Z = -10 важно для 2D)
    public float smoothSpeed = 0.10f;                // Чем меньше — тем плавнее

    void LateUpdate()   // LateUpdate важно! Выполняется после движения игрока
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        transform.position = smoothedPosition;
    }
}