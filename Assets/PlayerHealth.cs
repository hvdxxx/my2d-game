using UnityEngine;
using UnityEngine.SceneManagement; // Нужно для перезагрузки сцены

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log("Здоровье игрока: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Пока просто перезагружаем уровень при смерти
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}