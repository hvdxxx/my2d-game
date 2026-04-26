using UnityEngine;

public class VisionMaskScaler : MonoBehaviour
{
    [Header("Радиус видимости (должен быть такой же, как в PlayerVision)")]
    public float visionRadius = 5f;

    void Start()
    {
        // Сразу делаем круг большого размера
        transform.localScale = Vector3.one * visionRadius * 2f;
    }

    // Если радиус меняется во время игры — вызывай этот метод
    public void UpdateScale(float newRadius)
    {
        visionRadius = newRadius;
        transform.localScale = Vector3.one * visionRadius * 2f;
    }
}