using System;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    private int actualWave;

    private int enemiesInWave;

    [SerializeField] GameObject enemyDistance;

    [SerializeField] GameObject enemyMele;

    [SerializeField] Transform[] spawnPoints;

    [SerializeField] Transform[] wayPoints;

    public Action<int,int>waveAction;

    void Start()
    {
        actualWave=1;
        waveAction?.Invoke(actualWave,enemiesInWave);
        SpawnEnemy(enemyDistance);
    }

    private void SpawnEnemy(GameObject enemy)
    {
        if (spawnPoints.Length == 0 || enemy == null) return; //Comprobamos antes de spawnear

        int spawnPoint=UnityEngine.Random.Range(0,spawnPoints.Length); //Punto de reaparicion aleatorio
        
        Vector3 originSpawn=spawnPoints[spawnPoint].transform.position; //Posicion elegida


        Enemy e=Instantiate(enemy,originSpawn,Quaternion.identity).GetComponentInChildren<Enemy>(); //Instanciacion del enemigo

        AssignWaypoints(e);

        e.Died+=HandleEnemyDead;

        enemiesInWave++;

        waveAction?.Invoke(actualWave,enemiesInWave); //Evento disparado
    }

    private void AssignWaypoints(Enemy e)
    {
        e.Waypoints=wayPoints;
    }

    private void HandleEnemyDead(Entity e)
    {
        e.Died-=HandleEnemyDead;
        enemiesInWave--;
        waveAction?.Invoke(actualWave,enemiesInWave);
    }

}
