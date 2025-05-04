using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public class Player : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    [SerializeField] float jumpPower = 5f;

    Vector2 movement;
    Rigidbody2D myRigidbody;
    Animator myAnimator;
    CapsuleCollider2D myCapsuleCollider;


    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myCapsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (myAnimator.GetBool("isAttacking"))
            {
                myRigidbody.velocity = Vector2.zero;
            }

        Run();
        Sprite();
    }

    void OnMove(InputValue value)
    {
        movement = value.Get<Vector2>();
        Debug.Log(movement);
    }

    private void Run()
    {
        Vector2 Velocity = new Vector2(movement.x * speed, myRigidbody.velocity.y);
        myRigidbody.velocity = Velocity;

        bool isMoving = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("isRunning", isMoving);

    }

    void OnJump(InputValue value)
    {
        if (!myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; }

        if (value.isPressed)
        {
            myRigidbody.velocity += new Vector2(0f, jumpPower);
        }

    }

    void OnFire(InputValue value)
    {
        if (value.isPressed)
        {
            myAnimator.SetBool("isAttacking", true);

            
            // Animasyonun süresince bekleyip tekrar false yap
            StartCoroutine(ResetAttack());
        }
    }

    IEnumerator ResetAttack()
    {
        // Animasyonun süresini bekleyin (örneðin 0.5 saniye)
        yield return new WaitForSeconds(0.6f); // Bu deðeri animasyonunuzun uzunluðuna göre ayarlayýn
        myAnimator.SetBool("isAttacking", false);
    }

    void Sprite()
    {
        bool isMoving = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;

        if (isMoving)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), 1f);

        }
    }

}
