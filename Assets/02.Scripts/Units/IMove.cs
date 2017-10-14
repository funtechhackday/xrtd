using UnityEngine;


public abstract class IMove : MonoBehaviour
{
    public bool isMove;
    public abstract void Move();
    public Transform Target;
}
