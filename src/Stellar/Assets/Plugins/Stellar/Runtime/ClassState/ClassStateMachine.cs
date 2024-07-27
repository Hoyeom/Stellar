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
        
        
        #region Stateless
#if false

        public class ClassStateMachineConfigure<TEntity, TTrigger>
        {
            private readonly Dictionary<TTrigger, Type> _typeMap;


            public ClassStateMachineConfigure()
            {
                _typeMap = new Dictionary<TTrigger, Type>();
            }

            public ClassStateMachineConfigure<TEntity, TTrigger> Permit<T>(TTrigger trigger) where T : IClassState<TEntity>
            {
                var type = typeof(T);
                _typeMap[trigger] = type;
                return this;
            }

            public bool CanFire(TTrigger trigger)
            {
                return _typeMap.ContainsKey(trigger);
            }

            [CanBeNull]
            public Type Fire(TTrigger trigger)
            {
                return _typeMap[trigger];
            }
        }
        
        private readonly Dictionary<Type, ClassStateMachineConfigure<TEntity, TTrigger>> _configureMap = new();

        public ClassStateMachineConfigure<TEntity, TTrigger> Configure<T>() where T : IClassState<TEntity>
        {
            var type = typeof(T);
            
            if (!_configureMap.TryGetValue(type, out var configure))
            {
                _configureMap[type] = new ClassStateMachineConfigure<TEntity, TTrigger>();
            }

            return _configureMap[type];
        }

        public bool CanFire(TTrigger trigger)
        {
            var type = _currentState.GetType();
            
            if (_configureMap.TryGetValue(type, out var configure))
            {
                return configure.CanFire(trigger);
            }
            
            return false;
        }

        public void Fire(TTrigger trigger)
        {
            var currentType = _currentState.GetType();
            var type = _configureMap[currentType].Fire(trigger);
            Enter(type);
        }
#endif
        #endregion
    }
}