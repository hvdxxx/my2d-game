using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Настройки спавна")]
    public GameObject enemyPrefab;        // ← Сюда должен быть настоящий Prefab!
    public float spawnInterval = 3f;      // каждые 3 секунды
    public float spawnRadius = 15f;       // расстояние от игрока
    public int maxEnemies = 20;           // ограничим количество, чтобы не лагало

    private Transform player;
    private int currentEnemyCount = 0;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;

        // Запускаем спавн
        InvokeRepeating("SpawnEnemy", 1f, spawnInterval);
    }

    void SpawnEnemy()
    {

        if (player == null || enemyPrefab == null) return;
        if (currentEnemyCount >= maxEnemies) return;   // не спавним слишком много

        // Спавним по кругу вокруг игрока
        Vector2 randomDir = Random.insideUnitCircle.normalized;
        Vector2 spawnPos = (Vector2)player.position + randomDir * spawnRadius;
        GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        LevelSystem lv = player.GetComponent<LevelSystem>();

        if (lv != null)
        {
            enemy.GetComponent<Enemy>().ScaleStats(lv.currentLevel);
        }

        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        currentEnemyCount++;
    }

    // Этот метод будем вызывать из Enemy, когда враг умирает
    public void OnEnemyDied()
    {
        currentEnemyCount = Mathf.Max(0, currentEnemyCount - 1);
    }
}