using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    public UnitType Type;
    public int ID;
    public Transform Target;
    public IMove MoveController;


    public float FaultDistance = 10;
    public float MoveSpeed = .3f;

    private void Start()
    {
        if (MoveController == null)
            MoveController = transform.GetComponentInChildren<IMove>();
    }

    public enum UnitType
    {
        Soldier = 1,
    }

    private void OnDestroy()
    {
        MarkersManager.inst.RemoweUnit(this);
    }

    private void Update()
    {
        if (MoveController != null)
        {
            MoveController.Target = Target;
            MoveController.Move();
            if (MoveController.isMove)
                DPanel.Add(this, string.Format("Unit type <color=yellow>[{0}]</color> with ID <color=blue>[{1}]</color> was <color=green>MOVED</color> to target", Type, ID));
        }
    }
}
