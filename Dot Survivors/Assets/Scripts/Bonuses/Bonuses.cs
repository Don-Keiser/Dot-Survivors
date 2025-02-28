using UnityEngine;

public class Bonuses : MonoBehaviour
{
    [SerializeField] BonusConfig bonusConfig;
    [SerializeField] int bonusAmount;

    private Transform player;
    private bool isAttracted = false;
    private float attractionSpeed = 4f;
    private float pickupRange = 3f;

    [SerializeField] BonusType bonusType;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        bonusAmount = bonusConfig.bonusAmount;
    }

    private void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= pickupRange)
        {
            isAttracted = true;
        }

        if (isAttracted)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, attractionSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStats playerStats = collision.gameObject.GetComponent<PlayerStats>();
            if (playerStats != null) 
            {
                ApplyBonus(playerStats, bonusType);
            }
            Destroy(gameObject);
        }
    }

    private void ApplyBonus(PlayerStats playerStats, BonusType bonusType)
    {
        switch (bonusType)
        {
            case BonusType.Health:
                playerStats.Heal(bonusAmount);
                break;
            case BonusType.XP:
                playerStats.GainXP(bonusAmount);
                break;
        }
    }
}

public enum BonusType
{
    Health,
    XP,
    Damage
}
