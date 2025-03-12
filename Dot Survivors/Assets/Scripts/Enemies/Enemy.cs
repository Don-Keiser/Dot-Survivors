using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public EnemyConfig enemyConfig;

    private Transform player;
    private PlayerStats playerStats;
    private float damageTimer;
    [SerializeField] private float health;
    private float moveSpeed;
    [SerializeField] private int damage;
    private float damageInterval;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject hitEffectPrefab;
    [SerializeField] private Color hitColor;
    [SerializeField] private Animator animator;
    private bool isDying = false;

    private static readonly float FlashDuration = 0.1f;
    private static readonly float MinRandomSpeed = 0.8f;
    private static readonly float MaxRandomSpeed = 1.5f;
    private static readonly int DeathVariants = 3;
    private static readonly int HitEffectCount = 8;

    private Camera mainCamera;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerStats = player.GetComponent<PlayerStats>();
        if (player == null || playerStats == null)
        {
            Debug.LogError("Player or PlayerStats not found!");
            enabled = false;
            return;
        }

        damageTimer = 0f;
        animator = GetComponent<Animator>();

        // Load stats from EnemyConfig
        health = enemyConfig.health;
        moveSpeed = enemyConfig.moveSpeed;
        damage = enemyConfig.damage;
        damageInterval = enemyConfig.damageInterval;

        mainCamera = Camera.main;
    }

    void Update()
    {
        MoveTowardsPlayer();
    }

    void MoveTowardsPlayer()
    {
        if (player != null && !isDying)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            RotateTowardsPlayer(direction);

            transform.position = Vector2.MoveTowards(transform.position, player.position, enemyConfig.moveSpeed * Time.deltaTime);
        }
    }

    private void RotateTowardsPlayer(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (isDying) return;

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
        if (isDying) return;

        health -= damageAmount;
        StartCoroutine(FlashRed());

        if (health > 0)
        {
            SpawnHitEffect();
        }
        else
        {
            Die();
        }
    }

    private void SpawnHitEffect()
    {
        if (hitEffectPrefab != null)
        {
            for (int i = 0; i < HitEffectCount; i++)
            {
                Vector2 spawnPos = (Vector2)transform.position + UnityEngine.Random.insideUnitCircle * 0.2f;
                GameObject hitEffect = Instantiate(hitEffectPrefab, spawnPos, Quaternion.identity);
                hitEffect.GetComponent<SpriteRenderer>().color = hitColor;
                Rigidbody2D rb = hitEffect.GetComponent<Rigidbody2D>();

                Vector2 randomDirection = UnityEngine.Random.insideUnitCircle.normalized;
                rb.linearVelocity = randomDirection * UnityEngine.Random.Range(1.5f, 5f);
            }
        }
    }

    private IEnumerator FlashRed()
    {
        if (spriteRenderer != null)
        {
            Color originalColor = Color.white;

            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(FlashDuration);
            spriteRenderer.color = originalColor;
        }
    }

    private void Die()
    {
        if (isDying) return;

        isDying = true;
        DropXp();

        float randomSpeed = Random.Range(MinRandomSpeed, MaxRandomSpeed);
        animator.speed = randomSpeed;

        int deathVariant = Random.Range(0, DeathVariants);
        animator.SetInteger("DeathVariant", deathVariant);

        animator.SetTrigger("Die");
    }

    public void OnDeathAnimationComplete()
    {
        Destroy(gameObject);
    }

    private void DropXp()
    {
        BonusConfig drop = enemyConfig.GetRandomDrop();
        if (drop != null)
        {
            Instantiate(drop.bonusPrefab, transform.position, Quaternion.identity);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Boundary"))
        {
            RepositionEnemy();
        }
    }

    private void RepositionEnemy()
    {
        float camHeight = mainCamera.orthographicSize;
        float camWidth = camHeight * mainCamera.aspect;
        float spawnDistance = camWidth * 1.01f;

        Vector2 newPos = Vector2.zero;
        int side = Random.Range(0, 4); // 0 = left, 1 = right, 2 = top, 3 = bottom

        switch (side)
        {
            case 0: // Left
                newPos = GetLeftSpawnPosition(camHeight, spawnDistance);
                break;
            case 1: // Right
                newPos = GetRightSpawnPosition(camHeight, spawnDistance);
                break;
            case 2: // Top
                newPos = GetTopSpawnPosition(camWidth, spawnDistance);
                break;
            case 3: // Bottom
                newPos = GetBottomSpawnPosition(camWidth, spawnDistance);
                break;
        }

        transform.position = newPos;
    }

    private Vector2 GetLeftSpawnPosition(float camHeight, float spawnDistance)
    {
        return new Vector2(mainCamera.transform.position.x - spawnDistance, Random.Range(mainCamera.transform.position.y - camHeight, mainCamera.transform.position.y + camHeight));
    }

    private Vector2 GetRightSpawnPosition(float camHeight, float spawnDistance)
    {
        return new Vector2(mainCamera.transform.position.x + spawnDistance, Random.Range(mainCamera.transform.position.y - camHeight, mainCamera.transform.position.y + camHeight));
    }

    private Vector2 GetTopSpawnPosition(float camWidth, float spawnDistance)
    {
        return new Vector2(Random.Range(mainCamera.transform.position.x - camWidth, mainCamera.transform.position.x + camWidth), mainCamera.transform.position.y + spawnDistance);
    }

    private Vector2 GetBottomSpawnPosition(float camWidth, float spawnDistance)
    {
        return new Vector2(Random.Range(mainCamera.transform.position.x - camWidth, mainCamera.transform.position.x + camWidth), mainCamera.transform.position.y - spawnDistance);
    }
}