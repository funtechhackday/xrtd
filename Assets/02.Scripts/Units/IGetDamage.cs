using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IGetDamage : MonoBehaviour
{
    public Unit parent;
    public bool isLive = true;
    public int MaxHealth = 100;
    public int CurrentHealth = 100;
    public int Armor = 2;
    public virtual void Die()
    {
        isLive = false;
        switch (parent.Type)
        {
            case Unit.UnitType.Zombie:
                {
                    MarkersManager.inst.RemoweEnemy(parent);
                    break;
                }
            case Unit.UnitType.Soldier:
                {
                    MarkersManager.inst.RemoweUnit(parent);
                    break;
                }
            case Unit.UnitType.Base:
                {
                    DPanel.Add(this, "<color=red>PLAYER LOSE</color>");
                    DPanel.Add(this, "<color=red>PLAYER LOSE</color>");
                    DPanel.Add(this, "<color=red>PLAYER LOSE</color>");
                    break;
                }
        }
        
        Destroy(parent.gameObject,.2f);
    }
    public virtual void GetDamage(IAttack enemy)
    {
        CurrentHealth -= (enemy.AttackDamage - Armor);
        if (CurrentHealth < 0)
        {
            DPanel.Add(this,"Unit type <color=yellow>["+parent.Type+"]</color> was <color=red>DIE</color>");
            Die();
            return;
        }
        if(parent!=null && parent.AttackController != null)
        parent.AttackController.UnderAttack(enemy);
    }
    public void SetParent(Unit parent) { this.parent = parent; }
}
