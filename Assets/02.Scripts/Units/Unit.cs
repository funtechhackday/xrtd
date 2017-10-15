using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{

    public UnitType Type;
    public int ID;
    public Animator anim;
    public CurrentAnimationState animState;
    public Transform Target;
    public IMove MoveController;
    public IAttack AttackController;
    public IGetDamage HealthController;
    public Image HPBar;

    public float FaultDistance = 10;
    public float MoveSpeed = .3f;

    private void Start()
    {
        if (anim == null)
            anim = transform.GetComponentInChildren<Animator>();
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
        if (HPBar == null)
            HPBar = transform.GetComponentInChildren<Image>();
    }

    public enum UnitType
    {
        Base=0,
        Soldier = 1,
        Zombie = 2,
    }

    public enum CurrentAnimationState
    {
        idle=0,
        move=1,
        attack=2,
        die=3,
    }

    private void OnDestroy()
    {
        if(Type == UnitType.Soldier)
        MarkersManager.inst.RemoweUnit(this);
        if (Type == UnitType.Zombie)
            MarkersManager.inst.RemoweEnemy(this);
    }

    private void Update()
    {
        if (MarkersManager.inst.mainBase != null)
        {
            if (HealthController != null)
            {
                if (!HealthController.isLive)
                {
                    SwitchAnimation(CurrentAnimationState.die);
                    return;
                }
            }
            if (AttackController != null)
            {
                AttackController.Attack();
                if (AttackController.isAttack)
                {
                    SwitchAnimation(CurrentAnimationState.attack);
                    return;
                }
            }
            if (MoveController != null)
            {
                MoveController.Move();
                if (MoveController.isMove)
                    SwitchAnimation(CurrentAnimationState.move);
                else
                    SwitchAnimation(CurrentAnimationState.idle);
            }
        }
    }

    void SwitchAnimation(CurrentAnimationState newState)
    {
        if(animState != newState)
        {
            switch(newState)
            {
                case CurrentAnimationState.idle:
                    if (Type == UnitType.Soldier)
                    {
                        animState = newState;
                        anim.SetTrigger("Idle");
                    }
                    break;
                case CurrentAnimationState.move:
                    animState = newState;
                    anim.SetTrigger("Move");
                    break;
                case CurrentAnimationState.attack:
                    animState = newState;
                    anim.SetTrigger("Attack");
                    break;
                case CurrentAnimationState.die:
                    if (Type !=  UnitType.Base)
                    {
                        animState = newState;
                        anim.SetTrigger("Die");
                    }
                    break;
            }
        }
    }
}
