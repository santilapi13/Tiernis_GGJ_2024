using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    private Rigidbody2D PlayerRb;
    private Transform PlayerTransform;
    [SerializeField] private float speed = 10.0f;
    [SerializeField] private float jumpForce = 10.0f;
    [SerializeField] private float maxJumpTime = 1.5f;
    [SerializeField] private float firstImpulse = 10.0f;
    [SerializeField] private float bulletCooldown = 0f;
    [SerializeField] private Animator animator;

    float xScale;

    [SerializeField] private GameObject bulletPrefab;
    private Vector2 respawnPoint;
    
    private float currentJumpTime = 0f;
    private bool isJumping = false;
    [SerializeField] private bool isGrounded = false;
    [SerializeField] private bool dying = false;

    [SerializeField] private float invulnerabilityTime = 3f;
    
    void Start() {
        PlayerRb = GetComponent<Rigidbody2D>();
        PlayerTransform = GetComponent<Transform>();
        xScale = PlayerTransform.localScale.x;
        respawnPoint = PlayerTransform.position;
    }
    
    public Vector2 GetPosition() {
        return transform.position;
    }
    
    public bool IsGrounded() {
        return isGrounded;
    }
    
    void Update() {
        if (dying) return;
        
        Jump();

        if (Input.GetKeyDown(KeyCode.Space) && bulletCooldown <= 0f) {
            animator.SetTrigger("AttackTrigger");
            return;
        }
        
        animator.SetBool("Move", Input.GetAxis("Horizontal") != 0);

        bulletCooldown -= Time.deltaTime;

    }

    void FixedUpdate() {
        if (dying) return;
        
        float horizontalInput = Input.GetAxis("Horizontal");
        
        if (isGrounded)   
            PlayerRb.velocity = new Vector2(horizontalInput * speed, PlayerRb.velocity.y);
        else
            PlayerRb.velocity = new Vector2(horizontalInput * speed * 0.75f, PlayerRb.velocity.y);

        if (horizontalInput == 0)
            return;
        
        var rotation = (horizontalInput > 0f) ? xScale : -xScale;
        
        PlayerTransform.localScale = new Vector3(rotation, PlayerTransform.localScale.y, PlayerTransform.localScale.z);
        
    }

    void Jump() {
        if((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && isGrounded)
        {
            isJumping = true;
            currentJumpTime = 0f;
            PlayerRb.AddForce(Vector2.up * firstImpulse, ForceMode2D.Impulse);
        }

        if (!isJumping) return;

        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && currentJumpTime < maxJumpTime)
        {
            currentJumpTime += Time.deltaTime;
            float jumpForceMultiplier = Mathf.Clamp01(currentJumpTime / maxJumpTime);
            PlayerRb.AddForce(Vector2.up * jumpForce * jumpForceMultiplier, ForceMode2D.Impulse);
        }

        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
            isJumping = false;
        
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

    void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Ground"))
        {
            animator.SetBool("Jump", true);
            isGrounded = false;
        }
    }

    public void Shoot() {
        bulletCooldown = 0.5f;

        float x = (PlayerTransform.localScale.x > 0f) ? 1f : -1f;

        var bullet1 = Instantiate(bulletPrefab, transform.position + new Vector3(x,0,0), Quaternion.identity);
        bullet1.GetComponent<Bullet>().SetValues(new Vector2(x, 1),  Math.Abs(PlayerRb.velocity.x));

        var bullet2 = Instantiate(bulletPrefab, transform.position + new Vector3(x,0,0), Quaternion.identity);
        bullet2.GetComponent<Bullet>().SetValues(new Vector2(x, 0),  Math.Abs(PlayerRb.velocity.x));

        var bullet3 = Instantiate(bulletPrefab, transform.position + new Vector3(x,0,0), Quaternion.identity);
        bullet3.GetComponent<Bullet>().SetValues(new Vector2(x, -1), Math.Abs(PlayerRb.velocity.x));
    }

    public void TakeDamage() {
        Physics2D.IgnoreLayerCollision(6, 7);
        isGrounded = true;
        dying = true;
        animator.SetBool("Jump", false);
        animator.SetBool("Move", false);
        animator.SetTrigger("DeathTrigger");
    }

    public void Respawn() {
        Debug.Log("ENTRA");
        PlayerTransform.position = respawnPoint;
        dying = false;
        StartCoroutine(Invulnerablility());
    }

    private IEnumerator Invulnerablility() {
        yield return new WaitForSeconds(invulnerabilityTime);
        Physics2D.IgnoreLayerCollision(6, 7, false);
        dying = false;
    }

    public void SetRespawnPoint(Vector2 position) {
        respawnPoint = position;
    }
}
