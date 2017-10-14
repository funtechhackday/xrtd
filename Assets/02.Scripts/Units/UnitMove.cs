using System;
using UnityEngine;

public class UnitMove : IMove
{
    public float MoveSpeed = 5;
    public float FaultDistance = 10;
    public Rigidbody rg;
    private void Start()
    {
        rg = GetComponent<Rigidbody>();
        isMove = false;
    }  

    public override void Move()
    {
        if (Target != null)
        {
            if (Vector3.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(Target.position.x, Target.position.z)) > FaultDistance)
            {
                isMove = true;
                parent.transform.position += (Target.position - transform.position).normalized * MoveSpeed;
                //transform.position = Vector3.Lerp(transform.position, Target.position, MoveSpeed);
            }
            else
                isMove = false;
        }
    }

    //DPanel.Add(this, string.Format("Unit type <color=yellow>[{0}]</color> with ID <color=blue>[{1}]</color> was <color=green>MOVED</color> to target", Type, ID));
}

