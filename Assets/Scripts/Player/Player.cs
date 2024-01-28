using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor.Animations;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    private Rigidbody2D PlayerRb;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private AnimatorController[] animatorControllers;
    private int currentAnimatorControllerIndex = 0;
    [SerializeField] private AnimatorController currentAnimatorController;
    [SerializeField] private Animator animator;
    [SerializeField] private float speed = 10.0f;
    [SerializeField] private bool bulletCooldown = false;
    
    [SerializeField] private float jumpForce = 10.0f;

    float xScale;

    [SerializeField] private GameObject bulletPrefab;
    private Vector2 respawnPoint;
    [SerializeField] private bool isGrounded = false;
    [SerializeField] private bool dying = false;

    [SerializeField] private float invulnerabilityTime = 3f;
    [SerializeField] private int lives = 3;
    [SerializeField] private bool isInvulnerable = false;
    
    void Start() {
        currentAnimatorControllerIndex = 0;
        currentAnimatorController = animatorControllers[currentAnimatorControllerIndex];
        animator.runtimeAnimatorController = currentAnimatorController;
        PlayerRb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        xScale = transform.localScale.x;
        respawnPoint = transform.position;
    }
    
    public Vector2 GetPosition() {
        return ((Component)this).transform.position;
    }
    
    public bool IsGrounded() {
        return isGrounded;
    }
    
    void Update() {
        if (dying) return;

        if (Input.GetKeyDown(KeyCode.Space) && !bulletCooldown)
        {
            bulletCooldown = true;
            animator.SetTrigger("AttackTrigger");
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.Return))
            Evolve();
        
        if((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && isGrounded)
            Jump(); 
        
        animator.SetBool("Move", Input.GetAxis("Horizontal") != 0);

    }

    void FixedUpdate() {
        if (dying) return;
        
        float horizontalInput = Input.GetAxis("Horizontal");
        
        if (isGrounded)   
            PlayerRb.velocity = new Vector2(horizontalInput * speed, PlayerRb.velocity.y);
        else
            PlayerRb.velocity = new Vector2(horizontalInput * speed * 0.75f, PlayerRb.velocity.y);

        if (horizontalInput == 0 || bulletCooldown)
            return;
        
        var rotation = (horizontalInput > 0f) ? xScale : -xScale;
        
        transform.localScale = new Vector3(rotation, transform.localScale.y, transform.localScale.z);
        
    }

    void Jump()
    {
        PlayerRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
    
    
    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Ground"))
        {
            animator.SetBool("Jump", false);
            isGrounded = true;
            return;
        }
        
        if(collision.gameObject.CompareTag("Enemy"))
            TakeDamage();
    }

    private void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Ground")) {
            animator.SetBool("Jump", false);
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Ground"))
        {
            animator.SetBool("Jump", true);
            isGrounded = false;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Enemy"))
            TakeDamage();
    }

    public void Shoot() {
        float x = (transform.localScale.x > 0f) ? 1f : -1f;

        var bullet1 = Instantiate(bulletPrefab, ((Component)this).transform.position + new Vector3(x,0,0), Quaternion.identity);
        bullet1.GetComponent<Bullet>().SetValues(new Vector2(x, 1),  Math.Abs(PlayerRb.velocity.x));

        var bullet2 = Instantiate(bulletPrefab, ((Component)this).transform.position + new Vector3(x,0,0), Quaternion.identity);
        bullet2.GetComponent<Bullet>().SetValues(new Vector2(x, 0),  Math.Abs(PlayerRb.velocity.x));

        var bullet3 = Instantiate(bulletPrefab, ((Component)this).transform.position + new Vector3(x,0,0), Quaternion.identity);
        bullet3.GetComponent<Bullet>().SetValues(new Vector2(x, -1), Math.Abs(PlayerRb.velocity.x));

        bulletCooldown = false;
    }

    public void TakeDamage()
    {
        if (isInvulnerable) return;
        isInvulnerable = true;
        Physics2D.IgnoreLayerCollision(6, 7);
        isGrounded = true;
        dying = true;
        animator.SetBool("Jump", false);
        animator.SetBool("Move", false);
        animator.SetTrigger("DeathTrigger");
    }

    public void Respawn() {
        lives--;
        if (lives <= 0) {
            Die();
            return;
        }
        transform.position = respawnPoint;
        dying = false;
        bulletCooldown = false;
        for(int i = 0 ; i < 6 ; i++)
            Invoke("ChangeColor", i * 0.5f);
        StartCoroutine(Invulnerablility());
    }

    private void ChangeColor() {
        if (spriteRenderer.color == Color.gray)
            spriteRenderer.color = Color.white;
        else
            spriteRenderer.color = Color.gray;
    }
    
    public void Die() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("DeadScreen");
    }

    private IEnumerator Invulnerablility() {
        yield return new WaitForSeconds(invulnerabilityTime);
        Physics2D.IgnoreLayerCollision(6, 7, false);
        dying = false;
        isInvulnerable = false;
    }

    public void SetRespawnPoint(Vector2 position) {
        respawnPoint = position;
    }

    public void Evolve() {
        currentAnimatorControllerIndex++;
        currentAnimatorController = animatorControllers[currentAnimatorControllerIndex];
        animator.runtimeAnimatorController = currentAnimatorController;
        // TODO: Enable evolve particle
    }
}
