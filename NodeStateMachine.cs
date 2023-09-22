using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public abstract partial class NodeStateMachine<T, V> : Node where T : Node where V : StateNode<T, V>
{

    public V CurrentState;
    public V PreviousState;

    [Export]
    public string CurrentStateName;
    [Export]
    public string PreviousStateName;

    public Dictionary<string, V> States = new();
    public Dictionary<Type, V> StateTypes = new();

    private List<V> _states = new();

    public Callable ChangeStateCallable;
    public SharedState SharedState;

    [Export]
    public Node Actor;

    [Signal]
    public delegate void StateChangedEventHandler(Node newState, Node oldState);
    [Signal]
    public delegate void InitialisedEventHandler();

    public override void _Ready()
    {
        base._Ready();
        ChangeStateCallable = new Callable(this, MethodName.ChangeState);
        foreach (var child in GetChildren())
        {
            if (child is V node)
            {
                _states.Add(node);
                StateTypes.Add(node.GetType(), node);
                var name = child.Name.ToString().ToLower();
                States.Add(name, node);
                node.ChangeState += ChangeState;
                node.Actor = (T)Actor;
            }
        }

        ChangeState(new ObjectWrapper<Type>(_states[0].GetType()));

        EmitSignal(SignalName.Initialised);
    }

    public void PopState()
    {
        if (PreviousState != null)
            ChangeState(new ObjectWrapper<Type>(PreviousState.GetType()));
    }

    public void ChangeState(ObjectWrapper<Type> newStateName)
    {
        var value = newStateName.Value;
        var isValid = StateTypes.TryGetValue(value, out var newState);
        if (!isValid)
        {
            GD.PrintErr("Invalid state name: " + newStateName);
            return;
        }

        PreviousState = CurrentState;
        PreviousStateName = CurrentStateName;
        CurrentState = newState;
        CurrentStateName = newState.Name;

        PreviousState?.Exit(CurrentState);
        CurrentState?.Enter(PreviousState);

        EmitSignal(SignalName.StateChanged, CurrentState, PreviousState);
    }

    public void ChangeState(string newStateName)
    {
        var isValid = States.TryGetValue(newStateName, out var newState);
        if (!isValid)
        {
            GD.PrintErr("Invalid state name: " + newStateName);
            return;
        }

        PreviousState = CurrentState;
        PreviousStateName = CurrentStateName;
        CurrentState = newState;
        CurrentStateName = newStateName;

        PreviousState?.Exit(CurrentState);
        CurrentState?.Enter(PreviousState);

        EmitSignal(SignalName.StateChanged, CurrentState, PreviousState);

    }
    public virtual void GetTransition(double delta) { }


}