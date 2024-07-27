using Plugins.Stellar.Runtime;
using UnityEngine;

namespace Demo
{
    public class IdleState : ClassState<DemoTest>
    {
        public override void Enter()
        {
            Debug.Log("Enter");
        }

        public override void Exit()
        {
            Debug.Log("Exit");
        }
    }
}