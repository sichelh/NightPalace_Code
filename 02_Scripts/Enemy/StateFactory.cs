using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 상태 객체를 캐싱해서 재사용할 수 있도록 관리
public class StateFactory<T>
{
    private readonly Dictionary<System.Type, IState<T>> _states = new();
    private readonly T _owner;

    public StateFactory(T owner)
    {
        _owner = owner;
    }

    public Type Get<Type>() where Type : IState<T>, new()
    {
        var type = typeof(Type);

        if (_states.TryGetValue(type, out IState<T> state))
        {
            //Debug.Log($"[StateFactory] reused: {type.Name}");
            return (Type)state;
        }

        Type newState = new Type();
        newState.Init(_owner); // Init 호출
        _states[type] = newState;
        //Debug.Log($"[StateFactory] cached: {type.Name}");
        return newState;
    }
}
