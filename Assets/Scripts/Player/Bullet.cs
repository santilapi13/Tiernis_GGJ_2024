using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifeTime = 5f;

    private Rigidbody2D rb;
    private Vector2 direction;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        rb.velocity = new Vector2(direction.x * speed, direction.y);

        if (lifeTime <= 0) Destroy(gameObject);
        lifeTime -= Time.deltaTime;
    }

    public void SetValues(Vector2 direction, float playerSpeed) {
        this.direction = direction;
        this.speed += playerSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player") || other.CompareTag("Bullet") || other.CompareTag("Wall")) return;

        Destroy(gameObject);
    }
}
