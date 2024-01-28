using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wool : MonoBehaviour {
    [SerializeField] GameObject explosionParticle;

    private void Start() {
        Physics2D.IgnoreLayerCollision(7, 9);
    }
    
    void OnCollisionEnter2D(Collision2D other) {
        Explosion();
    }

    private void Explosion() {
        explosionParticle.transform.SetParent(null);
        explosionParticle.SetActive(true);
        GenerateRadius();
        Destroy(gameObject, 0.1f);
    }

    private void GenerateRadius() {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1f);
        foreach (Collider2D nearbyObject in colliders) {
            if(nearbyObject.tag != "Player")
                continue;
            
            nearbyObject.TryGetComponent(out Player player);
            
            if (player != null)
                player.TakeDamage();
        }
    }

}
