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
                parent.transform.LookAt(new Vector3((Target.transform.position.x) , 0, (Target.transform.position.z)));
                parent.transform.position += (new Vector3(Target.position.x - transform.position.x, 0, Target.position.z - transform.position.z).normalized * MoveSpeed);
                //transform.position = Vector3.Lerp(transform.position, Target.position, MoveSpeed);
            }
            else
                isMove = false;
        }
    }

    //DPanel.Add(this, string.Format("Unit type <color=yellow>[{0}]</color> with ID <color=blue>[{1}]</color> was <color=green>MOVED</color> to target", Type, ID));
}

