public class StateMachine<T>
{
    protected BaseState<T> currentState;
    
    public BaseState<T> CurrentState => currentState;
    protected T owner { get; private set; }     //입력값을 받는 클래스가 new StateMachine의 주인이다

    // 멤버 선언 -> public StateMachine<입력값을 받는 클래스> StateMachine { get; private set; }
    // start() -> StateMachine = new StateMachine<입력값을 받는 클래스>(this);
    public StateMachine(T owner)
    {
        this.owner = owner;
    }

    // start() -> StateMachine.ChangeState(new PlayerIdleState(this));     //처음 상태 초기화
    public void ChangeState(BaseState<T> newState)
    {
        if(currentState?.GetType() == newState.GetType()) return;

        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

    public void Update()
    {
        currentState?.Update();
    }

    public void FixedUpdate()
    {
        currentState?.FixedUpdate();
    }

    public void LateUpdate()
    {
        currentState?.LateUpdate();
    }
}
