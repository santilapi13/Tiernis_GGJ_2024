using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public abstract class Enemy : MonoBehaviour {
    [SerializeField] protected Health health;
    [SerializeField] protected Player player;
    [SerializeField] protected Animator animator;
    
    [SerializeField] protected float speed;
    [SerializeField] protected float jumpForce;

    private float xScale;
    [SerializeField] protected bool isGrounded;
    [SerializeField] protected float jumpCooldown = 0;
    
    [SerializeField] protected Rigidbody2D rb;
    protected Vector2 playerLastPosition;

    private void Awake() {
        xScale = transform.localScale.x;
    }
    
    private void Update() {
        Act();
    }

    public void TakeDamage(int damage) {
        health.TakeDamage(damage);
        if (health.GetCurrentHealth() > 0) return;
        Die();
    }

    private void Die() {
        Destroy(gameObject);
    }

    protected abstract void Act();

    protected void MoveTowardsPosition(Vector2 targetPosition) {
        Transform t = transform;
        Vector2 currentPosition = t.position;
        float direction = currentPosition.x < targetPosition.x ? 1 : -1;
        FixRotation(direction);
        rb.velocity = new Vector2( direction * speed, rb.velocity.y);
    }

    protected void FixRotation(float direction) {
        Transform t = transform;
        Vector2 scale = t.localScale;
        float rotation = direction > 0 ? -xScale : xScale;
        
        t.localScale = new Vector2(rotation, scale.y);
    }
    
    protected bool HaveEqualHeight(Vector2 position) {
        return Math.Abs(transform.position.y - position.y) < 0.2;
    }
    
    private void OnCollisionExit2D(Collision2D other) {
        if (other.gameObject.CompareTag("Ground"))
            isGrounded = false;
    }
    
    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }
}
