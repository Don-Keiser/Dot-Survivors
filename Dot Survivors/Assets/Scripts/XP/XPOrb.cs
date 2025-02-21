using UnityEngine;

public class XPOrb : MonoBehaviour
{
    [SerializeField] XPOrbConfig xpOrbConfig;
    [SerializeField] int xpAmount;

    private Transform player;
    private bool isAttracted = false;
    private float attractionSpeed = 4f;
    private float pickupRange = 3f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        xpAmount = xpOrbConfig.xpAmount;
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
                playerStats.GainXP(xpAmount);
            }
            Destroy(gameObject);
        }
    }
}
