using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Stage : MonoBehaviour {
    [SerializeField] private List<Enemy> enemies;
    [SerializeField] private List<int> enemiesPerGroup;
    [SerializeField] private float timeBetweenEnemies;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private GameObject stageCenter;
    [SerializeField] private Collider2D trigger;
    [SerializeField] private Collider2D wall1;
    [SerializeField] private Collider2D wall2;
    [SerializeField] private GameObject respawnPoint;
    [SerializeField] private Lanadero lanadero;
    
    private bool enableWave;
    private bool waveFinished;
    private float timer;
    private int nextEnemy;
    private int nextGroup;

    private void OnTriggerEnter2D(Collider2D other) {
        if (!other.CompareTag("Player")) return;
        
        if (lanadero) lanadero.gameObject.SetActive(true);
        virtualCamera.Priority = 20;
        virtualCamera.Follow = stageCenter.transform;
        trigger.enabled = false;
        wall1.enabled = true;
        wall2.enabled = true;
        StartWave();
        other.TryGetComponent(out Player player);
        player.SetRespawnPoint(respawnPoint.transform.position);
    }

    private void Update() {
        if (waveFinished)
            CheckEnemies();
        
        if (!enableWave) return;
        
        timer -= Time.deltaTime;
        if (timer > 0 && EnemiesAliveInCurrentGroup()) return;
        
        timer = timeBetweenEnemies;

        SpawnNextEnemiesGroup();
    }

    private void StartWave() {
        enableWave = true;
        waveFinished = false;
        timer = timeBetweenEnemies;

        int activeEnemies = 0;
        foreach (var enemy in enemies) {
            if (enemy.gameObject.activeSelf) activeEnemies++;
            enemy.GetComponent<Enemy>().enabled = true;
        }
        
        nextEnemy = activeEnemies;
        nextGroup = activeEnemies > 0 ? 1 : 0;
    }

    private void SpawnNextEnemiesGroup() {
        var enemiesAmount = enemiesPerGroup[nextGroup];
        for (int i = 0; i < enemiesAmount; i++)
        {
            enemies[nextEnemy].gameObject.SetActive(true);
            nextEnemy++;
        }

        nextGroup++;
        if (nextGroup < enemiesPerGroup.Count) return;
        
        waveFinished = true;
        enableWave = false;
    }

    private void CheckEnemies() {
        foreach (var enemy in enemies) {
            if (enemy.gameObject.activeSelf) return;
        }
        
        virtualCamera.Priority = 0;
        wall2.enabled = false;
        gameObject.SetActive(false);
    }

    private bool EnemiesAliveInCurrentGroup() {
        var amountInCurrentGroup = enemiesPerGroup[nextGroup - 1];
        var amountAlive = amountInCurrentGroup;
        
        for (int i = 1; i <= amountInCurrentGroup; i++) {
            if (!enemies[nextEnemy - i].gameObject.activeSelf) amountAlive--;
        }
        
        return amountAlive > 0;
    }

    /*
    public void Reset()
    {
        virtualCamera.Priority = 0;
        trigger.enabled = true;
        wall1.enabled = false;
        wall2.enabled = false;
        enableWave = false;
        waveFinished = false;
        timer = 0;
        nextEnemy = 0;
        nextGroup = 0;
        foreach (var enemy in enemies)
        {
            enemy.Reset(); 
        }
        
    }
    */
    
}
