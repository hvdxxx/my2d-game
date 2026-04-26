using UnityEngine;

public class PlayerVision : MonoBehaviour
{
    [Header("Радиус видимости")]
    [Range(1f, 30f)]
    public float visionRadius = 5f;

    [Header("Мягкость краёв")]
    [Range(0.05f, 1f)]
    public float edgeSoftness = 0.35f;

    private Transform fogTransform;
    private Material fogMaterial;
    private Camera mainCamera;

    void Start()
    {
        GameObject fogObj = GameObject.Find("FogOfWar");
        if (fogObj != null)
        {
            fogTransform = fogObj.transform;
            SpriteRenderer sr = fogObj.GetComponent<SpriteRenderer>();
            if (sr != null && sr.material != null)
            {
                fogMaterial = sr.material;
            }
        }

        mainCamera = Camera.main;
    }

    void LateUpdate()
    {
        if (fogTransform == null || mainCamera == null || fogMaterial == null) return;

        fogTransform.position = transform.position;

        // Растягиваем на весь экран с запасом
        float height = mainCamera.orthographicSize * 2.2f;
        float width = height * mainCamera.aspect;
        fogTransform.localScale = new Vector3(width * 1.6f, height * 1.6f, 1f);

        // Передаём параметры в шейдер
        fogMaterial.SetFloat("_Radius", visionRadius);
        fogMaterial.SetFloat("_Softness", edgeSoftness);
    }
}