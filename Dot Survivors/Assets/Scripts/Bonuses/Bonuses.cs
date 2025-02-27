using UnityEngine;

public class Bonuses : MonoBehaviour
{
    [SerializeField] int healAmount;

    private Transform player;
    private bool isAttracted = false;
    private float attractionSpeed = 4f;
    private float pickupRange = 3f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
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
                playerStats.Heal(healAmount);
            }
            Destroy(gameObject);
        }
    }
}

public enum BonusType
{
    Health,
    Damage
}
