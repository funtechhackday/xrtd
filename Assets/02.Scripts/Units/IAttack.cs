using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IAttack : MonoBehaviour
{
    public Unit parent;
    public IGetDamage target;
    public Unit.UnitType myType;
    public Unit.UnitType[] enemyTypes;
    public int AttackDamage = 10;
    public int AttackRadius = 5;
    public float TimeBeforeAttacks = 2;
    public bool isAttack;
    public List<Unit> Enemys = new List<Unit>();

    public void SetParent(Unit parent) { this.parent = parent; myType = parent.Type; }

    bool isCoroutineStart = false;

    public virtual void Attack()
    {
        if(target != null && target.isLive && inAttackRadius(target.transform.position))
        {
            isAttack = true;
            if (!isCoroutineStart)
                StartCoroutine(DoAttack());
            return;
        }
        target = null;
        Enemys.Clear();
        switch(myType)
        {
            case Unit.UnitType.Soldier:
                {
                    Enemys.AddRange(MarkersManager.inst.enemys);
                        break;
                }
            case Unit.UnitType.Zombie:
                {
                    Enemys.AddRange(MarkersManager.inst.units);
                    Enemys.Add(MarkersManager.inst.mainBase.transform.GetComponent<Unit>());
                    break;
                }
        }
        if(Enemys.Count>0)
        {
            var minDistance = (float)AttackRadius;
            foreach(var e in Enemys)
            {
                var dist = Vector3.Distance(e.transform.position, transform.position);
                if (dist < minDistance)
                {
                    minDistance = dist;
                    target = e.HealthController;                    
                }
            }
        }
        if (target != null)
        {
            isAttack = true;
            StartCoroutine(DoAttack());
        }
        else
            isAttack = false;
    }

    public virtual void UnderAttack(IAttack enemy)
    {
        if (target == null)
        //if (inAttackRadius(enemy.transform.position))
        {
            target = enemy.parent.HealthController;
            if (parent.Type == Unit.UnitType.Zombie)
                parent.MoveController.Target = enemy.transform;
        }
    }

    bool inAttackRadius(Vector3 pos)
    {
        var dist = Vector3.Distance(pos, transform.position);
        if (dist < AttackRadius)
            return true;
        return false;
    }

    IEnumerator DoAttack()
    {
        while (target != null && target.isLive)
        {
            isCoroutineStart = true;
            target.GetDamage(this);
            parent.transform.LookAt(new Vector3((target.transform.position.x), 0, (target.transform.position.z)));
            DPanel.Add(this, string.Format( "Unit <color=yellow>{0}</color> attacks unit <color=red>{1}</color>",parent.Type,target.parent.Type));
            yield return new WaitForSeconds(TimeBeforeAttacks);
        }
        if (parent.Type == Unit.UnitType.Zombie)
            parent.MoveController.Target = MarkersManager.inst.mainBase.MarkerPosition;
        isAttack = false;
        isCoroutineStart = false;
        StopCoroutine(DoAttack());
    }
}
