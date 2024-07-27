namespace Plugins.Stellar.Runtime
{
    public interface IClassState<in TEntity>
    {
        public void EnterState(TEntity entity);
        public void ExitState();
        public void UpdateState();
        public void FixedUpdateState();
    }
}