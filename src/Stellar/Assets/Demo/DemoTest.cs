using System;
using System.Collections;
using System.Collections.Generic;
using Plugins.Stellar.Runtime;
using UnityEngine;

public class DemoTest : MonoBehaviour
{
    private ClassStateMachine<DemoTest> _classStateMachine;
    private readonly HashSet<WeakReference> _weakReferences = new HashSet<WeakReference>();
    
    private void Awake()
    {
        _classStateMachine = new ClassStateMachine<DemoTest>(this);
    }

    void Update()
    {
        _classStateMachine.Update();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _weakReferences.Add(new WeakReference(_classStateMachine.CurrentState));
            _classStateMachine.Enter<IdleState>();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        int collectCount = 0;
        
        foreach (var weakReference in _weakReferences)
        {
            if (!weakReference.IsAlive)
            {
                collectCount++;
            }
        }
        
        Debug.Log($"({collectCount}/{_weakReferences.Count})");
    }
}
