
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class CloudFlappyController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float forwardSpeed = 4f;
    [SerializeField] private float jumpForce = 6f;
    [SerializeField] private CloudFlappyGameManager gameManager;

    private Rigidbody2D rb;
    private bool canMove;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!canMove || Mouse.current == null)
            return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            rb.linearVelocity = new Vector2(
                forwardSpeed,
                0f
            );

            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlaySFX("CloudJump");
        }
    }

    private void FixedUpdate()
    {
        if (!canMove)
            return;

        rb.linearVelocity = new Vector2(
            forwardSpeed,
            rb.linearVelocity.y
        );
    }

    public void SetCanMove(bool value)
    {
        canMove = value;

        if (!value)
        {
            rb.linearVelocity = Vector2.zero;
            rb.gravityScale = 0f;
        }
        else
        {
            rb.gravityScale = 1f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (gameManager != null)
            gameManager.HandleCollision(collision.collider);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (gameManager != null)
            gameManager.HandleCollision(other);
    }
}
