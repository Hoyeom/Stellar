using Plugins.Stellar.Runtime;
using UnityEngine;

namespace Demo
{
    public class MoveState : ClassState<DemoTest>
    {
        public override void Enter()
        {
            Debug.Log("MoveState");
        }
    }
    
    public class IdleState : ClassState<DemoTest>
    {
        public override void Enter()
        {
            Debug.Log("IdleState");
        }
    }
}