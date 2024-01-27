using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour {
    private Rigidbody2D PlayerRb;
    private Transform PlayerTransform;
    [SerializeField] private float speed = 10.0f; 
    [SerializeField] private float jumpForce = 10.0f;
    [SerializeField] private float maxJumpTime = 1.5f;
    [SerializeField] private float firstImpulse = 10.0f;
    [SerializeField] private float bulledColeDown = 0f;
    float xScale;

    [SerializeField] private GameObject bulledPrefab;
    private Vector3 respawnPoint;
    
    private float currentJumpTime = 0f;
    private bool isJumping = false;
    private bool isGrounded = false;

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
        Jump();

        if (Input.GetKeyDown(KeyCode.Space) && bulledColeDown <= 0f) {
            Shoot();
            return;
        }

        bulledColeDown -= Time.deltaTime;

    }

    void FixedUpdate() {
        float horizontalInput = Input.GetAxis("Horizontal");
        
        if (isGrounded)   
            PlayerRb.velocity = new Vector2(horizontalInput * speed, PlayerRb.velocity.y);
        else
            PlayerRb.velocity = new Vector2(horizontalInput * speed * 0.75f, PlayerRb.velocity.y);

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
            isGrounded = true;
            return;
        }
        
        if(collision.gameObject.CompareTag("Enemy"))
            TakeDamage();
    }

    void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    public void Shoot() {

       bulledColeDown = 0.5f;

       float x = (PlayerTransform.localScale.x > 0f) ? 1f : -1f;

       var bullet1 = Instantiate(bulledPrefab, transform.position + new Vector3(x,0,0), Quaternion.identity);
       bullet1.GetComponent<Bullet>().SetValues(new Vector2(x, 1),  Math.Abs(PlayerRb.velocity.x));
       
       var bullet2 = Instantiate(bulledPrefab, transform.position + new Vector3(x,0,0), Quaternion.identity);
       bullet2.GetComponent<Bullet>().SetValues(new Vector2(x, 0),  Math.Abs(PlayerRb.velocity.x));
       
       var bullet3 = Instantiate(bulledPrefab, transform.position + new Vector3(x,0,0), Quaternion.identity);
       bullet3.GetComponent<Bullet>().SetValues(new Vector2(x, -1), Math.Abs(PlayerRb.velocity.x));
    }

    private void TakeDamage() {
        PlayerTransform.position = respawnPoint;
        StartCoroutine(Invulnerablility());
    }

    private IEnumerator Invulnerablility() {
        Physics2D.IgnoreLayerCollision(6, 7);
        yield return new WaitForSeconds(invulnerabilityTime);
        Physics2D.IgnoreLayerCollision(6, 7, false);
    }
    
    
}
