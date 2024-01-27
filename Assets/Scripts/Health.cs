using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {
    [SerializeField] private int currentHealth;
    [SerializeField] private int initialHealth;

    private void Start() {
        currentHealth = initialHealth;
    }

    public void TakeDamage(int damage) {
        currentHealth -= damage;
    }

    public int GetCurrentHealth() {
        return currentHealth;
    }

}
