using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState<T>
{
    void Init(T owner);
    void Enter();
    void Update();
    void FixedUpdate();
    void LateUpdate();
    void Exit();
}
