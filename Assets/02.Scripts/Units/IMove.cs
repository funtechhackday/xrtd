using UnityEngine;


public class IMove : MonoBehaviour
{
    public Unit parent;
    public bool isMove;
    public virtual void Move() { }
    public Transform Target;
    public void SetParent(Unit parent) { this.parent = parent; }
}
