namespace Plugins.Stellar.Runtime
{
    public abstract class ClassState<TEntity> : IClassState<TEntity>
    {
        protected TEntity Entity;
        
        public void EnterState(TEntity entity)
        {
            Entity = entity;
            Enter();
        }
        
        public void ExitState()   
        {
            Exit();
            Entity = default;
        }

        public void UpdateState()
        {
            Update();
        }

        public void FixedUpdateState()
        {
            FixedUpdate();
        }


        protected virtual void Enter() { }
        protected virtual void Exit() { }
        protected virtual void Update() { }
        protected virtual void FixedUpdate() { }
    }
}