using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{

    [SerializeField]
    public int lives = 5;

    [SerializeField]
    public float speed = 3.0f;

    [SerializeField]
    public float jumpForce = 4f;

    new private Rigidbody2D rigidbody;
    private Animator animator;
    private SpriteRenderer sprite;

    private CharState State
    {
        get { return (CharState) animator.GetInteger("State"); }
        set { animator.SetInteger("State", (int) value); }
    }

    private bool isGrounded = false;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        CheckGrounded();
    }

    // Update is called once per frame
    void Update()
    {
        if(isGrounded) State = CharState.Idle;

        if (Input.GetButton("Horizontal")) Run();
        if (isGrounded && Input.GetButton("Jump")) Jump();
    }

    private void Run()
    {
        if (isGrounded) State = CharState.Run;

        Vector3 direction = transform.right * Input.GetAxis("Horizontal");

        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);

        sprite.flipX = direction.x < 0.0f;
    }

    private void Jump()
    {
        State = CharState.Jump;

        rigidbody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

    private void CheckGrounded()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.3f);

        isGrounded = colliders.Length > 1;
    }
}

public enum CharState
{
    Idle,
    Jump,
    Run
}
