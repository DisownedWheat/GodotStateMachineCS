using Godot;
using System.Collections.Generic;

[GlobalClass]
public abstract partial class NodeStateMachine : Node
{
    [Export]
    public bool Process = true;

    public State CurrentState;
    public State PreviousState;

    public Dictionary<string, State> States = new();

    public Callable ChangeStateCallable;

    [Signal]
    public delegate void StateChangedEventHandler(Node newState, Node oldState);
    [Signal]
    public delegate void InitialisedEventHandler();

    public override void _Ready()
    {
        base._Ready();
        ChangeStateCallable = new Callable(this, MethodName.ChangeState);
    }

    public virtual void Init()
    {
        foreach (var child in GetChildren())
        {
            if (child is State node)
            {
                var name = child.Name.ToString().ToLower();
                States.Add(name, node);
                node.ChangeState += ChangeState;
                if (name == "idle")
                {
                    ChangeState(name);
                    node.Enter(null);
                }
            }
        }
        EmitSignal(SignalName.Initialised);
    }

    public void PopState()
    {
        if (PreviousState != null)
            ChangeState(PreviousState.Name.ToString().ToLower());
    }

    public virtual void ChangeState(string newStateName)
    {
        var isValid = States.TryGetValue(newStateName, out var newState);
        if (!isValid)
        {
            GD.PrintErr("Invalid state name: " + newStateName);
            return;
        }

        PreviousState = CurrentState;
        CurrentState = newState;

        PreviousState?.Exit(CurrentState);
        CurrentState?.Enter(PreviousState);

        EmitSignal(SignalName.StateChanged, CurrentState, PreviousState);
    }

    public virtual void Update(double delta)
    {
        if (!Process)
        {
            return;
        }
        CurrentState?.Update(delta);
    }
    public virtual void GetTransition(double delta) { }
}