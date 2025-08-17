using Unity.VisualScripting;
using UnityEngine;

public abstract class BaseState<T> : IState<T>
{
    protected T owner;

    public virtual void Init(T owner)
    {
        this.owner = owner;
    }
    public abstract void Enter();
    public abstract void Update();
    public abstract void FixedUpdate();
    public abstract void LateUpdate();
    public abstract void Exit();
}
