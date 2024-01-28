using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CatBoss : Enemy {
    [SerializeField] private float attackCooldown;
    [SerializeField] private GameObject projectile;
    private bool dying = false;

    
    protected override void Act(){
        if (dying) return;
        
        if(attackCooldown > 0) {
            attackCooldown -= Time.deltaTime;
            return;
        }

        Shoot();
        
        attackCooldown = 5f;
    }

    private void Shoot(){
        for(int i = 0; i < 3; i++){
            Invoke("ShootBullet", i * 0.5f);
        }
    }

    private void ShootBullet(){
        AudioManager.Instance.PlaySFX("cat_boss_bullet", false);
        GameObject bullet = Instantiate(projectile, transform.position, Quaternion.identity);
        bullet.GetComponent<EnemyBullet>().setParameters(Vector2.left, false);
    }

    protected override void Die() {
        dying = true;
        animator.SetTrigger("DeathTrigger");
    }

    public void EndLevel() {
        SceneManager.LoadScene("Creditos");
    }

    private void OnEnable() {
        AudioManager.Instance.PlayIntroAndLoop(2, 3);
    }
}
