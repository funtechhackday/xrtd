using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StagesModel : MonoBehaviour
{

    #region Singleton
    public static StagesModel inst;
    private void Awake()
    {
        if (inst == null)
            inst = this;
    }
    #endregion

    public List<StageGoals> stages = new List<StageGoals>();
}

[System.Serializable]
public class StageGoals
{
    public int StageName;
    public int EnemyCount;
    public int SoldiersCount;
}
