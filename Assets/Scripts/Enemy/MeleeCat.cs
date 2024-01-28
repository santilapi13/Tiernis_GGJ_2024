using System;
using UnityEngine;

public class MeleeCat : Enemy {

    protected override void Act() {
        Vector2 playerPosition = player.GetPosition();
        
        jumpCooldown -= Time.deltaTime;

        if (player.IsGrounded())
            playerLastPosition = playerPosition;
        else {
            if (Math.Abs(transform.position.x - playerLastPosition.x) >= 0.2f) 
                MoveTowardsPosition(playerLastPosition);
            return;
        }
        
        if (HaveEqualHeight(playerPosition) || Math.Abs(transform.position.x - playerLastPosition.x) > 4f || !isGrounded) {
            MoveTowardsPosition(playerPosition);
            return;
        }

        ChangeHeight(playerPosition.y);
    }

    private void ChangeHeight(float playerHeight) {
        rb.gravityScale = 1f;
        if (transform.position.y > playerHeight)
            GetOffPlatform();
        else
            Jump();
    }

    private void Jump() {
        if (jumpCooldown > 0) return;
        
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        jumpCooldown = 2f;
    }

    private void GetOffPlatform() {
        if (!isGrounded) return;
        
        var direction = new Vector2(playerLastPosition.x - transform.position.x, 0f);
        MoveTowardsPosition(direction);
    }
}
