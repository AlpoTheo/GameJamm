using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TopDown2DController : MonoBehaviour
{
    [Header("Hareket Ayarları")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float acceleration = 12f;
    [SerializeField] private float deceleration = 15f;
    [Range(0, 0.99f)] [SerializeField] private float momentumDamping = 0.9f;

    [Header("Animasyon")]
    [SerializeField] private Animator animator;
    [SerializeField] private string moveXParam = "MoveX";
    [SerializeField] private string moveYParam = "MoveY";
    [SerializeField] private string speedParam = "Speed";

    [Header("Referanslar")]
    [SerializeField] private SpriteRenderer characterSprite;

    private Rigidbody2D rb;
    private Vector2 movementInput;
    private Vector2 currentVelocity;
    private bool isMovementLocked = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; // 2D oyun için yerçekimini kapat
        rb.freezeRotation = true;
    }

    void Update()
    {
        if (!isMovementLocked)
        {
            GetInput();
            HandleAnimation();
        }
    }

    void FixedUpdate()
    {
        if (!isMovementLocked)
        {
            HandleMovement();
        }
    }

    private void GetInput()
    {
        // Klavye ve gamepad girişi
        movementInput.x = Input.GetAxisRaw("Horizontal");
        movementInput.y = Input.GetAxisRaw("Vertical");

        // Çapraz hareket için normalize et
        if (movementInput.magnitude > 1f)
        {
            movementInput.Normalize();
        }
    }

    private void HandleMovement()
    {
        Vector2 targetVelocity = movementInput * moveSpeed;
        
        // Yumuşak hızlanma/yavaşlama
        currentVelocity = Vector2.Lerp(
            currentVelocity, 
            targetVelocity, 
            (movementInput.magnitude > 0.1f ? acceleration : deceleration) * Time.fixedDeltaTime
        );

        // Momentum efekti
        rb.velocity = Vector2.Lerp(rb.velocity, currentVelocity, 1f - momentumDamping);
    }

    private void HandleAnimation()
    {
        if (animator != null)
        {
            // Hareket yönü için
            if (movementInput.magnitude > 0.1f)
            {
                animator.SetFloat(moveXParam, movementInput.x);
                animator.SetFloat(moveYParam, movementInput.y);
            }

            // Hareket hızı
            animator.SetFloat(speedParam, rb.velocity.magnitude);
        }

        // Sprite'ı x ekseninde çevirme (sağa/sola dönüş)
        if (characterSprite != null && movementInput.x != 0)
        {
            characterSprite.flipX = movementInput.x < 0;
        }
    }

    public void LockMovement(bool locked)
    {
        isMovementLocked = locked;
        if (locked)
        {
            rb.velocity = Vector2.zero;
            currentVelocity = Vector2.zero;
            
            if (animator != null)
            {
                animator.SetFloat(speedParam, 0f);
            }
        }
    }

    // Dışarıdan erişim için yardımcı metodlar
    public Vector2 GetMovementDirection() => movementInput;
    public float GetCurrentSpeed() => rb.velocity.magnitude;
    public bool IsMoving() => movementInput.magnitude > 0.1f;
}