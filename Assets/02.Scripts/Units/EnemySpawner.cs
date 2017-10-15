using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    #region Singleton
    public static EnemySpawner inst;
    private void Awake()
    {
        if (inst == null)
            inst = this;
    }
    #endregion

    public float spawnTime = 5;

    public GameObject EnemyPrefab;
    public Transform Enemys;

    public int MaxEnemyOnStage = 0;
    public int CurrentEnemyOnStage = 0;

    private void Start()
    {
        MenuUICanvas.onStageReady += StartSpawn;
    }

    public void StartSpawn()
    {
        MaxEnemyOnStage=GamePlayMenuHandler.inst.GetCurStage(MenuUICanvas.inst.GetStageCount()).EnemyCount;
        StartCoroutine(Spawn());
    }
    public void StopSpawn()
    {
        StopCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {

        while (MarkersManager.inst.mainBase != null && MaxEnemyOnStage > CurrentEnemyOnStage)
        {
            var spawn = GetRandomPlace();
            if (spawn != null)
            {
                var e = Instantiate(EnemyPrefab, spawn.position, spawn.rotation, Enemys) as GameObject;
                var unit = e.GetComponent<Unit>();
                if(unit!=null)
                MarkersManager.inst.AddNewEnemy(unit);
                CurrentEnemyOnStage++;
            }
            yield return new WaitForSeconds(spawnTime);
        }
    }

    Transform GetRandomPlace()
    {
        if(transform.childCount>0)
        {
            return transform.GetChild(Random.Range(0, transform.childCount));
        }
        return null;
    }
}
