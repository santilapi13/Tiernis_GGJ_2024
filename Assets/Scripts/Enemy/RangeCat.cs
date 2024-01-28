using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeCat : Enemy
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float shotCooldown = 0;
    private float currentCooldown = 0;


    protected override void Act()
    {
        var playerPosition = player.GetPosition();

        if (Math.Abs(transform.position.x - shootPoint.position.x) > 1f) {
            MoveTowardsPosition(shootPoint.position);
            return;
        }

        if (currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;
            return;
        }

        var isCurved = (playerPosition.y > transform.position.y + 0.5f) ? true : false;

        Vector2 direction = player.GetPosition().x < transform.position.x ? new Vector2(-1, 0) : new Vector2(1, 0);
        Vector2 possition = new Vector2(transform.position.x + direction.x * 0.2f, transform.position.y + 0.2f);
        var bulletInstance = Instantiate(bullet, transform.position, Quaternion.identity);
        bulletInstance.GetComponent<EnemyBullet>().setParameters(direction, isCurved);

        currentCooldown = shotCooldown;
    }
    
}