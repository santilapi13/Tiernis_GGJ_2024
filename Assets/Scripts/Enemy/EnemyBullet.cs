using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifeTime = 5f;


    [SerializeField] private bool isCurved;
    [SerializeField] private float curveSpeed = 5f;

   

    private Rigidbody2D rb;
    private Vector2 direction;
    

    private float maxFlyngTyme = 2f;
    private float flyngTime = 0f;
    private bool onMoving = false;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    
    void Update()
    {
        if (lifeTime <= 0) Destroy(gameObject);

        lifeTime -= Time.deltaTime;
        
        rb.velocity = new Vector2(direction.x * speed * Time.deltaTime , rb.velocity.y);
    }

    public void setParameters(Vector2 direction, bool isCurved)
    {
        this.isCurved = isCurved;
        this.direction = direction;
        if(isCurved)
        {
            rb.gravityScale = 1f;
            speed = 0.5f * speed;
            rb.AddForce(new Vector2(0, 1) * curveSpeed, ForceMode2D.Impulse);

        }
    }

}
