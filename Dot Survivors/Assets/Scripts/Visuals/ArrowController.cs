using UnityEngine;

public class ArrowController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float radius = 1.5f;

    private Vector2 currentDirection = Vector2.right;

    public void UpdateArrow(Vector2 newDirection)
    {
        if (newDirection != Vector2.zero && newDirection != currentDirection)
        {
            currentDirection = newDirection.normalized;

            Vector2 offset = currentDirection * radius;
            transform.position = (Vector2)player.position + offset;

            float angle = Mathf.Atan2(currentDirection.y, currentDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}