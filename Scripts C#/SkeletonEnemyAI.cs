using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;


public class SkeletonEnemyAI : MonoBehaviour
{
    public HeroKnight heroKnight;
    public GameObject recibirGolpeAudio;
    public Animator animator;
    public float movementSpeed = 3f;
    public float attackDamage = 10f;
    public int maxHits = 2;

    //public Transform groundCheck;
    //public LayerMask groundLayer;

    private GameObject player;
    private Rigidbody2D rb;
    private AudioSource golpeAudio;
    private int currentHits = 0;
    private bool isDestroyed = true;
    private bool isAttacking = false;
    private bool isFacingRight = false;

    //private bool isOnGround = false;

    private Color originalColor;
    private SpriteRenderer spriteRenderer;
    private bool isFlashing = false;

    private void Start()
    {
        golpeAudio = recibirGolpeAudio.GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        SetDestroyedState();
    }

    private void Update()
    {
        if (isDestroyed && !isAttacking)
        {
            //--- CheckGround();
            Vector2 playerPosition = player.transform.position;
            Vector2 enemyPosition = transform.position;

            float distanceToPlayer = playerPosition.x - enemyPosition.x;

            if (Mathf.Abs(distanceToPlayer) < 3.5f) //--- && isOnGround)
            {
                if (distanceToPlayer > 0 && !isFacingRight)
                {
                    Flip();
                }
                else if (distanceToPlayer < 0 && isFacingRight)
                {
                    Flip();
                }

                StartAttack();
            }
        }
        else if (!isDestroyed && isAttacking)
        {
            Vector2 playerPosition = player.transform.position;

            transform.position = Vector3.Lerp(transform.position, playerPosition, Time.deltaTime);

            if((playerPosition.x < transform.position.x) && isFacingRight)
            {
                Flip();
            }
            else if ((playerPosition.x > transform.position.x) && !isFacingRight)
            {
                Flip();
            }
        }

        if (heroKnight.health <= 0)
        {
            animator.enabled = false;
            rb.velocity = Vector2.zero;       
        }
    }

    /*private void CheckGround()
    {
        isOnGround = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }*/

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //--if (collision.gameObject.CompareTag("Player"))
        //--{
            //heroKnight.recibirDano(2);
        //--}
    }

    private void StartAttack()
    {
        isAttacking = false;
        isDestroyed = false;

        animator.SetBool("isDestroyed", false);
        Invoke("EnableAttack", 1.0f);
    }

    private void EnableAttack()
    {
        isAttacking = true;
        //--animator.SetTrigger("Attack");
        animator.SetBool("isAttacking", true);
    }

    public void TakeHit()
    {
        if (isDestroyed || isFlashing) return;

        currentHits++;
        golpeAudio.PlayOneShot(golpeAudio.clip);
        StartCoroutine(FlashWhite());
        if (currentHits >= maxHits)
        {
            
            
            DisableEnemy();
        }
    }

    private IEnumerator FlashWhite()
    {
        isFlashing = true;
        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = originalColor;
        isFlashing = false;
    }

    private void DisableEnemy()
    {
        isDestroyed = true;
        isAttacking = false;

        animator.SetBool("isRebuilding", true);
        animator.SetBool("isDestroyed", true);
        animator.SetBool("isAttacking", false);
        //--animator.SetTrigger("Destroyed");
        rb.velocity = Vector2.zero;
        this.GetComponent<BoxCollider2D>().enabled = false;
        rb.bodyType = RigidbodyType2D.Static;
        Invoke("DestroyEnemy", 1.0f);
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    /*private void RebuildEnemy()
    {
        isDestroyed = false;
        currentHits = 0;
        animator.SetTrigger("Rebuild");
    }*/

    private void SetDestroyedState()
    {
        isDestroyed = true;
        //animator.SetTrigger("Destroyed");
        animator.SetBool("isDestroyed", true);
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    

}
