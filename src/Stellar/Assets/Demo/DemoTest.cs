using System;
using System.Collections;
using System.Collections.Generic;
using Demo;
using Plugins.Stellar.Runtime;
using UnityEngine;

public class DemoTest : MonoBehaviour
{
    public enum Trigger
    {
        IdleCommand,
        MoveCommand,
    }
    
    private ClassStateMachine<DemoTest> _classStateMachine;
    private readonly HashSet<WeakReference> _weakReferences = new HashSet<WeakReference>();
    private Vector2 _input;
    private int _collectCount;

    private void Awake()
    {
        _classStateMachine = new ClassStateMachine<DemoTest>(this, new IdleState());
    }

    
    void Update()
    {

        var input  = Vector2.right * Input.GetAxisRaw("Horizontal") + Vector2.up * Input.GetAxisRaw("Vertical");
        
        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            if (_input != input)
            {
                _weakReferences.Add(_classStateMachine?.GetWeakReferenceCurrentState());
                _classStateMachine?.Enter<MoveState>();
                _input = input;
            }
        }
        else
        {
            if (_input != input)
            {
                _weakReferences.Add(_classStateMachine?.GetWeakReferenceCurrentState());
                _classStateMachine?.Enter<IdleState>();
                _input = input;
            }
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            _weakReferences.Add(_classStateMachine?.GetWeakReferenceCurrentState());
            _weakReferences.Add(new WeakReference(_classStateMachine));
            _classStateMachine = null;
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

        if (_collectCount != collectCount)
        {
            Debug.Log($"({collectCount}/{_weakReferences.Count})");
            _collectCount = collectCount;
        }
        
        _classStateMachine?.Update();
    }
}