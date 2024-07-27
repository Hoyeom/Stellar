using System;
using UnityEngine;

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


        public virtual void Enter() { }
        public virtual void Exit() { }
        public virtual void Update() { }
        public virtual void FixedUpdate() { }
    }
}