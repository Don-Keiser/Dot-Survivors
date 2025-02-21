using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyConfig enemyConfig;

    private Transform player;
    private PlayerStats playerStats;
    private float damageTimer;
    [SerializeField] float health;
    private float moveSpeed;
    [SerializeField] int damage;
    private float damageInterval;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerStats = player.GetComponent<PlayerStats>();
        damageTimer = 0f;

        // Load stats from EnemyConfig
        health = enemyConfig.health;
        moveSpeed = enemyConfig.moveSpeed;
        damage = enemyConfig.damage;
        damageInterval = enemyConfig.damageInterval;
    }

    void Update()
    {
        MoveTowardsPlayer();
    }

void MoveTowardsPlayer()
{
    if (player != null)
    {
        Vector2 direction = (player.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            damageTimer += Time.deltaTime;
            if (damageTimer >= damageInterval)
            {
                playerStats.TakeDamage(damage);
                damageTimer = 0f;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            damageTimer = 0f;
        }
    }

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        DropXp();
        Destroy(gameObject);
    }

    private void DropXp()
    {
        XPOrbConfig drop = enemyConfig.GetRandomDrop();
        if (drop != null)
        {
            Instantiate(drop.xpOrbPrefab, transform.position, Quaternion.identity);
        }
    }
}