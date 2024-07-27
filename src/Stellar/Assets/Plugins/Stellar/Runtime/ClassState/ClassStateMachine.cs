namespace Plugins.Stellar.Runtime
{
    public class ClassStateMachine<TEntity>
    {
        private readonly TEntity _entity;
        public IClassState<TEntity> CurrentState { get; private set; }

        public ClassStateMachine(in TEntity entity)
        {
            _entity = entity;
            CurrentState = new NoneState<TEntity>();
        }
        
        public void Enter<TState>() where TState : IClassState<TEntity>, new()
        {
            CurrentState?.ExitState();
            CurrentState = new TState();
            CurrentState.EnterState(_entity);
        }

        public void Update()
        {
            CurrentState.UpdateState();
        }
        
        public void FixedUpdate()
        {
            CurrentState.FixedUpdateState();
        }
    }
}