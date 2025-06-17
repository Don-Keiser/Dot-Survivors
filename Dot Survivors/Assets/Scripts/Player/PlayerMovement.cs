using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float baseMoveSpeed = 5f;
    private float moveSpeed;
    public Rigidbody2D rb;

    private Vector2 movement;
    public Vector2 lastMovementDirection { get; private set; } = Vector2.right; // Default to right

    [SerializeField] private ArrowController arrowController;

    private void Start()
    {
        moveSpeed = baseMoveSpeed;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();

        if (movement != Vector2.zero)
        {
            lastMovementDirection = movement.normalized;

            if (arrowController != null)
                arrowController.UpdateArrow(lastMovementDirection);
        }
    }

    public void SetMoveSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}