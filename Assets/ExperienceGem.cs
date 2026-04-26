using UnityEngine;

public class ExperienceGem : MonoBehaviour
{
    public int expAmount = 25; // Сколько опыта дает этот камень
    public float attractSpeed = 5f; // Скорость притягивания к игроку
    private Transform player;
    private bool isFollowing = false;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        if (player == null) return;

        // Если игрок подошел близко (например, на 3 метра)
        float distance = Vector2.Distance(transform.position, player.position);
        if (distance < 3f) isFollowing = true;

        if (isFollowing)
        {
            // Летим к игроку
            transform.position = Vector3.MoveTowards(transform.position, player.position, attractSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Добавляем опыт через LevelSystem
            LevelSystem lv = other.GetComponent<LevelSystem>();
            if (lv != null)
            {
                lv.AddExperience(expAmount);
            }

            Destroy(gameObject); // Камень исчезает
        }
    }
}