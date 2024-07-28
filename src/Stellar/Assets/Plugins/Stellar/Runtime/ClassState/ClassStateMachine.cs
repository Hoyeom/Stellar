using System;

namespace Plugins.Stellar.Runtime
{
    public class ClassStateMachine<TEntity>
    {
        private readonly TEntity _entity;
        private IClassState<TEntity> _currentState;

        public ClassStateMachine(in TEntity entity, IClassState<TEntity> classState)
        {
            _entity = entity;
            _currentState = classState;
            _currentState.EnterState(_entity);
        }

        public void Enter<TState>() where TState : IClassState<TEntity>, new()
        {
            _currentState?.ExitState();
            _currentState = new TState();
            _currentState.EnterState(_entity);
        }

        private void Enter(Type type)
        {
            if (!typeof(IClassState<TEntity>).IsAssignableFrom(type))
            {
                throw new ArgumentException($"{type.Name} does not implement IClassState<{typeof(TEntity).Name}>");
            }

            _currentState?.ExitState();
            _currentState = (IClassState<TEntity>)Activator.CreateInstance(type);
            _currentState.EnterState(_entity);
        }

        public void Update()
        {
            _currentState.UpdateState();
        }

        public void FixedUpdate()
        {
            _currentState.FixedUpdateState();
        }

        public WeakReference GetWeakReferenceCurrentState()
        {
            return new WeakReference(_currentState);
        }
    }
}