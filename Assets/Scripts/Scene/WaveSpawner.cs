using System;
using UnityEngine;
using System.Collections;

public class WaveSpawner : MonoBehaviour
{
    //----------------Enemies----------------
    private int enemiesAlive;

    [SerializeField] GameObject enemyDistance;

    [SerializeField] GameObject enemyMele;

    [SerializeField] Transform[] spawnPoints;

    [SerializeField] Transform[] wayPoints;

    [SerializeField] private Wave[] waves;
    //----------------Waves----------------
    [SerializeField] private float timeBetweenWaves=20f;
    public event Action<int,int>WaveAction;

    public event Action WavesCompleted;

    private int currentWave;

    private Coroutine wavesCoroutine;



    //----------------------LogicSpawns----------------------
    void Start()
    {
        StartWaves();
    }

    void StartWaves()
    {   
        wavesCoroutine=StartCoroutine(RunWaves());
    }

    public void StopWaves()
    {   
        if (wavesCoroutine == null) return;
        StopCoroutine(wavesCoroutine);
        wavesCoroutine = null;
    }
    private void SpawnEnemy(GameObject enemy)
    {
        if (spawnPoints.Length == 0 || enemy == null) return; //Comprobamos antes de spawnear

        int spawnPoint=UnityEngine.Random.Range(0,spawnPoints.Length); //Punto de reaparicion aleatorio
        
        Vector3 originSpawn=spawnPoints[spawnPoint].transform.position; //Posicion elegida


        Enemy e=Instantiate(enemy,originSpawn,Quaternion.identity).GetComponentInChildren<Enemy>(); //Instanciacion del enemigo

        AssignWaypoints(e);

        e.Died+=HandleEnemyDead;

        enemiesAlive++;

        WaveAction?.Invoke(currentWave,enemiesAlive); //Evento disparado
    }
    
    private IEnumerator SpawnWave(Wave wave)//Corutina para iniciar oleada actual
    {
        for(int i=0; i < wave.distanceCount; i++)
        {
            SpawnEnemy(enemyDistance);

            yield return new WaitForSeconds(wave.spawnInterval);
        }

        for(int i=0; i < wave.meleeCount; i++)
        {
            SpawnEnemy(enemyMele);

            yield return new WaitForSeconds(wave.spawnInterval);
        }
    }

    private IEnumerator RunWaves()//Corutina para iniciar las oleadas
    {
        for(int i=0; i < waves.Length; i++)
        {   
            currentWave=i+1;

            bool isLast = i == waves.Length - 1;
           
            yield return SpawnWave(waves[i]);

            yield return new WaitUntil(()=>enemiesAlive<=0);
           if(!isLast) yield return new WaitForSeconds(timeBetweenWaves);
        }

        WavesCompleted?.Invoke();
    }

    //----------------------Enemy----------------------

    private void AssignWaypoints(Enemy e)
    {
        e.Initialize(wayPoints);
    }

    private void HandleEnemyDead(Entity e)
    {
        e.Died-=HandleEnemyDead;
        enemiesAlive--;
        WaveAction?.Invoke(currentWave,enemiesAlive);
    }

}
