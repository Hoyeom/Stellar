using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Demo;
using Plugins.Stellar.Runtime;
using Stellar.Runtime.Extension;
using UnityEngine;
using Object = UnityEngine.Object;

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


    private async UniTaskVoid Start()
    {
        ViewTree<string> tree = new ViewTree<string>("Root");

        ViewTreeNode<string> child1 = new ViewTreeNode<string>("Child 1");
        ViewTreeNode<string> child2 = new ViewTreeNode<string>("Child 2");

        tree.Root.AddChild(child1);
        tree.Root.AddChild(child2);

        ViewTreeNode<string> grandchild1 = new ViewTreeNode<string>("Grandchild 1");
        ViewTreeNode<string> grandchild2 = new ViewTreeNode<string>("Grandchild 2");

        child1.AddChild(grandchild1);
        child1.AddChild(grandchild2);

        // 트리 구조 출력
        PrintTree(tree.Root, "");

        return;
        
        bool isOpen = false;

        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (isOpen)
                {
                    // viewNav.CloseAsync<ViewTest>().Forget();
                }
                else
                {
                    // viewNav.OpenAsync<ViewTest, ViewModel>(new ViewModel()).Forget();
                }

                isOpen = !isOpen;
            }            
            await UniTask.Yield();
        }

    }
    
    void PrintTree<T>(ViewTreeNode<T> node, string indent)
    {
        Debug.Log(indent + node.Value);

        foreach (var child in node.Children)
        {
            PrintTree(child, indent + "  ");
        }
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