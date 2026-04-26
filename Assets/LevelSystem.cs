using UnityEngine;

public class LevelSystem : MonoBehaviour
{
    public int currentLevel = 1;
    public int experience = 0;
    public int expToNextLevel = 100;

    public GameObject echoPrefab; // Сюда перетащи префаб EchoGhost

    // Метод для добавления опыта (вызывай его, когда подбираешь кристалл)
    public void AddExperience(int amount)
    {
        experience += amount;
        if (experience >= expToNextLevel)
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        currentLevel++;
        experience = 0;
        expToNextLevel = Mathf.RoundToInt(expToNextLevel * 1.2f); // Увеличиваем порог опыта

        SpawnEcho();
        Debug.Log("Уровень повышен! Теперь уровень: " + currentLevel);
    }


    void SpawnEcho()
    {
    if (echoPrefab == null) return;

    Vector3 spawnPos = transform.position - (transform.right * 1.0f);
    GameObject newEcho = Instantiate(echoPrefab, spawnPos, Quaternion.identity);

    PlayerWeapon weapon = GetComponent<PlayerWeapon>();
    
    // ВМЕСТО анимации берем направление прицела из скрипта оружия
    Vector2 aimingDir = Vector2.right; // значение на всякий случай
    if (weapon != null)
    {
        aimingDir = weapon.GetMouseDirection();
    }

    if (newEcho.TryGetComponent<Echo>(out Echo echoScript))
    {
        // Теперь Эхо запомнит именно то, куда ты целился мышкой!
        echoScript.Initialize(aimingDir, weapon.damage, weapon.projectilePrefab);
    }
    }    

}