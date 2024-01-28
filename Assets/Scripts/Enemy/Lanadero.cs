using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lanadero : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;
    private int index = 0;

    [SerializeField] private float speed = 1f;
    [SerializeField] private float attackTime = 1f;
    private float initialAttackTime;
    [SerializeField] GameObject[] bombPrefab;
    
    void Start() {
        initialAttackTime = attackTime;
    }
    
    void Update() {
        Move();
        Attack();
    }

    private void Move()
    {
        Vector2 targetPosition = new Vector2(waypoints[index].transform.localPosition.x, transform.localPosition.y);
        transform.localPosition = Vector2.MoveTowards(transform.localPosition, targetPosition, speed * Time.deltaTime);
        
        if (Vector2.Distance(transform.localPosition, targetPosition) > 0.1f)
            return;
        
        index++;
        if (index == waypoints.Length)
        {
            index = 0;
        }
    }

    private void Attack() {
        attackTime -= Time.deltaTime;
        if (attackTime > 0)
            return;
        
        attackTime = initialAttackTime;
        var lana = Instantiate(bombPrefab[UnityEngine.Random.Range(0,bombPrefab.Length)], transform.position, Quaternion.identity);
        lana.transform.Rotate(Vector3.up, 180f);
    }
}
