using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Unit
{

    [SerializeField]
    public int lives = 5;

    [SerializeField]
    public float speed = 3.0f;

    [SerializeField]
    public float jumpForce = 3f;

    new private Rigidbody2D rigidbody;
    private Animator animator;
    private SpriteRenderer sprite;

    private Bullet bullet;

    //[SerializeField]
    //public GameObject blood;
    //private Vector3 bloodPosition;
    //private List<GameObject> allBlood = new List<GameObject>();

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

        bullet = Resources.Load<Bullet>("Bullet");

        //bloodPosition = transform.position;
    }

    //private void Start()
    //{
    //    blood.GetComponent<ParticleSystem>().enableEmission = false;
    //}

    private void FixedUpdate()
    {
        CheckGrounded();
    }

    // Update is called once per frame
    void Update()
    {
        if(isGrounded) State = CharState.Idle;

        if (Input.GetButtonDown("Fire1")) Shoot();
        if (Input.GetButton("Horizontal")) Run();
        if (isGrounded && Input.GetButton("Jump")) Jump();

        //if((allBlood != null) && (allBlood.Count > 0))
        //{
        //    foreach(GameObject bloodOne in allBlood)
        //    {
        //        bloodOne.transform.position = transform.position;
        //    }
        //}
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

    private void Shoot()
    {
        Vector3 position = transform.position;
        position.x += 0.5f;

        //Debug.Log(position.y);

        Bullet newBullet = Instantiate(bullet, position, bullet.transform.rotation) as Bullet;

        newBullet.Direction = newBullet.transform.right * (sprite.flipX ? -1.0f : 1.0f);
    }

    private void CheckGrounded()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.3f);

        isGrounded = colliders.Length > 1;
    }

    public override void ReciveDamage()
    {
        lives--;

        State = CharState.Jump;

        rigidbody.velocity = Vector3.zero;
        rigidbody.AddForce(transform.up * 7.0f, ForceMode2D.Impulse);

        //blood.GetComponent<ParticleSystem>().enableEmission = true;

        //GameObject newBlood = Instantiate(blood, transform.position, blood.transform.rotation) as GameObject;
        //allBlood.Add(newBlood);
        //Debug.Log("blood count=" + allBlood.Count);
    }

}

public enum CharState
{
    Idle,
    Jump,
    Run
}
