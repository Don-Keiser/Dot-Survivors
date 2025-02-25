using UnityEngine;
using System.Collections;

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

    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject hitEffectPrefab;
    [SerializeField] Color hitColor;
    [SerializeField] Animator animator;
    private bool isDying = false;

    private static readonly float FlashDuration = 0.1f;
    private static readonly float FadeDuration = 0.3f;
    private static readonly float MinRandomSpeed = 0.8f;
    private static readonly float MaxRandomSpeed = 1.5f;
    private static readonly int DeathVariants = 3;

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
            for (int i = 0; i < 8; i++)
            {
                Vector2 spawnPos = (Vector2)transform.position + UnityEngine.Random.insideUnitCircle * 0.2f;
                GameObject hitEffect = Instantiate(hitEffectPrefab, spawnPos, Quaternion.identity);
                hitEffect.GetComponent<SpriteRenderer>().color = hitColor;
                Rigidbody2D rb = hitEffect.GetComponent<Rigidbody2D>();

                Vector2 randomDirection = UnityEngine.Random.insideUnitCircle.normalized;
                rb.linearVelocity = randomDirection * UnityEngine.Random.Range(1.5f, 5f);

                StartCoroutine(FadeOutAndDestroy(hitEffect));
            }
        }
    }

    private IEnumerator FadeOutAndDestroy(GameObject obj)
    {
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        float elapsedTime = 0f;

        while (elapsedTime < FadeDuration)
        {
            elapsedTime += Time.deltaTime;
            if (sr != null)
            {
                Color c = sr.color;
                c.a = Mathf.Lerp(1f, 0f, elapsedTime / FadeDuration);
                sr.color = c;
            }
            yield return null;
        }

        Destroy(obj);
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
        XPOrbConfig drop = enemyConfig.GetRandomDrop();
        if (drop != null)
        {
            Instantiate(drop.xpOrbPrefab, transform.position, Quaternion.identity);
        }
    }
}