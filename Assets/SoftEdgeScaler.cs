using UnityEngine;

public class SoftEdgeScaler : MonoBehaviour
{
    [Header("Радиус видимости (должен быть такой же, как в PlayerVision)")]
    public float softRadius = 5f;

    void Start()
    {
        // Сразу делаем круг большого размера
        transform.localScale = Vector3.one * softRadius * 2f;
    }

    // Если радиус меняется во время игры — вызывай этот метод
    public void UpdateScale(float newRadius)
    {
        softRadius = newRadius;
        transform.localScale = Vector3.one * softRadius * 2f;
    }
}