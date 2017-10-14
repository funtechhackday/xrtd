using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    public UnitType Type;
    public int ID;
    public Transform Target;
    public IMove MoveController;
    public IAttack AttackController;
    public IGetDamage HealthController;


    public float FaultDistance = 10;
    public float MoveSpeed = .3f;

    private void Start()
    {
        if (MoveController == null)
            MoveController = GetComponent<IMove>();
        if (MoveController == null)
            MoveController = transform.GetComponentInChildren<IMove>();
        if (MoveController != null) MoveController.SetParent(this);
        if (AttackController == null)
            AttackController = transform.GetComponentInChildren<IAttack>();
        if (AttackController != null) AttackController.SetParent(this);
        if (HealthController == null)
            HealthController = transform.GetComponentInChildren<IGetDamage>();
        if (HealthController != null) HealthController.SetParent(this);
        if (MoveController != null)
            MoveController.Target = Target;
    }

    public enum UnitType
    {
        Base=0,
        Soldier = 1,
        Zombie = 2,
    }

    private void OnDestroy()
    {
        MarkersManager.inst.RemoweUnit(this);
    }

    private void Update()
    {
        if (MarkersManager.inst.mainBase != null)
        {
            if (HealthController != null)
            {
                if (!HealthController.isLive)
                    return;
            }
            if (AttackController != null)
            {
                AttackController.Attack();
                if (AttackController.isAttack)
                    return;
            }
            if (MoveController != null)
            {
                MoveController.Move();
            }
        }
    }
}
