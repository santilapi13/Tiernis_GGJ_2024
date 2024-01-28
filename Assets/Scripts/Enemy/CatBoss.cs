using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatBoss : Enemy {
    [SerializeField] private GameObject[] wave;
    [SerializeField] private Transform waveSpawnPoint;
    [SerializeField] private Transform lastEnemyPosition;
    [SerializeField] private float attackCooldown;
    [SerializeField] private GameObject projectile;
    private int attackNumber = 0;    

    
    protected override void Act(){
        if(attackCooldown > 0) {
            attackCooldown -= Time.deltaTime;
            return;
        }

        Shoot();
        
        attackCooldown = 5f;
    }

    /*
    private void SpawnWave(){
        for(int i = 0; i < wave.Length; i++){
            if (i == wave.Length - 1) {
                Instantiate(wave[i], lastEnemyPosition.position, Quaternion.identity);
                wave[i].GetComponent<RangeCat>().SetDirection(new Vector2(lastEnemyPosition.position.x + 5f, lastEnemyPosition.position.y));
            } else {
                Instantiate(wave[i], waveSpawnPoint.position, Quaternion.identity);
            }
        }
    }
    */

    private void Shoot(){
        for(int i = 0; i < 3; i++){
            Invoke("ShootBullet", i * 0.5f);
        }
    }

    private void ShootBullet(){
        GameObject bullet = Instantiate(projectile, transform.position, Quaternion.identity);
        bullet.GetComponent<EnemyBullet>().setParameters(Vector2.left, false);
    }
}
