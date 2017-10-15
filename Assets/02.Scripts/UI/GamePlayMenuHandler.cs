using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayMenuHandler : MonoBehaviour {

    public GameObject HPCanvasPrefab;

    public delegate void EnemiesCountChanged();
    public static event EnemiesCountChanged onEnemiesCountChanged;

    public delegate void SoldiersCountChanged();
    public static event SoldiersCountChanged onSoldiersCountChanged;

    public delegate void StageCountChanged();
    public static event StageCountChanged onStageCountChanged;

    private int _stageCount;
    private int _enemiesCount;
    private int _soldiersCount;

    private GameObject EnemiesCount;
    private GameObject SoldiersCount;
    private GameObject StageCount;
    private GameObject TowerInfo;

    public int EnemyKilled
    {
        get { return _enemiesCount; }
        set { _enemiesCount = value; }
    }

    public int SoldierAlive
    {
        get { return _soldiersCount; }
        set { _soldiersCount = value; }
    }

    public static GamePlayMenuHandler inst;
    private void Awake()
    {
        if (inst == null)
            inst = this;
        onEnemiesCountChanged += changeEnemiesInfoText;
        onSoldiersCountChanged += changeSoldiersInfoText;
        onStageCountChanged += changeStageInfoText;

        MenuUICanvas.onStageReady += ShowCurrentGameplayData;
    }

    private void Start()
    {
        EnemiesCount = this.gameObject.transform.GetChild(3).gameObject;
        SoldiersCount = this.gameObject.transform.GetChild(2).gameObject;
        StageCount = this.gameObject.transform.GetChild(1).gameObject;
        TowerInfo = this.gameObject.transform.GetChild(4).gameObject;

        //StatsReset();
    }

    public void StatsReset()
    {
        _stageCount = 1;
        _enemiesCount = 0;
        _soldiersCount = 0;
    }

    public void LoadInitialStage()
    {
        SetEnemiesCount(0);
        SetSoldiersCount(0);
        SetStageCount(1);
        //UpdateHPBarForTower(1.0f);
    }

    public void ShowCurrentGameplayData()
    {
        SetEnemiesCount(_enemiesCount);
        SetSoldiersCount(_soldiersCount);
        SetStageCount(MenuUICanvas.inst.GetStageCount());
    }

    //public void SetHPBarForUnit(GameObject unit)
    //{
    //    GameObject hpBar = Instantiate(HPCanvasPrefab);
    //    hpBar.transform.position = unit.transform.position;
    //    hpBar.transform.parent = unit.transform;
    //    hpBar.name = "HPBar";
    //}

    public void UpdateHPBarForUnit(Unit unit, float value)
    {
        if (unit.HPBar != null)
            unit.HPBar.fillAmount =((float)unit.HealthController.CurrentHealth / (float)unit.HealthController.MaxHealth);
    }

    public void UpdateHPBarForTower(Unit unit, float value)
    {
        if (unit.HPBar != null)
            unit.HPBar.fillAmount =( (float)unit.HealthController.CurrentHealth/ (float)unit.HealthController.MaxHealth);
    }

    public void SetEnemiesCount(int count)
    {
        _enemiesCount = count;
        if (onEnemiesCountChanged != null)
        {
            onEnemiesCountChanged();
            var s = GetCurStage(MenuUICanvas.inst.GetStageCount());
            if (s.EnemyCount <= count)
                MenuUICanvas.inst.StartCoroutine();
        }
    }

    public StageGoals GetCurStage(int number)
    {
        foreach(var s in StagesModel.inst.stages)
        {
            if (s.StageName == number)
                return s;
        }
        return null;
    }

    public void SetSoldiersCount(int count)
    {
        _soldiersCount = count;
        if (onSoldiersCountChanged != null)
        {
            onSoldiersCountChanged();
        }
    }

    public void SetStageCount(int count)
    {
        _stageCount = count;
        if (onStageCountChanged != null)
        {
            onStageCountChanged();
        }
    }

    private void changeEnemiesInfoText()
    {
        EnemiesCount.GetComponent<Text>().text = string.Format("Enemies: {0} / {1}", _enemiesCount, GetCurStage(MenuUICanvas.inst.GetStageCount()).EnemyCount);
    }

    private void changeSoldiersInfoText()
    {
        SoldiersCount.GetComponent<Text>().text = string.Format("Defenders: {0} / {1}", _soldiersCount, GetCurStage(MenuUICanvas.inst.GetStageCount()).SoldiersCount);
    }

    private void changeStageInfoText()
    {
        StageCount.GetComponent<Text>().text = string.Format("Stage {0}", _stageCount);
    }




}
