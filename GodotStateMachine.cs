using Godot;
using System;
using System.Collections.Generic;

using StateStack = Godot.Collections.Array<State>;

public class GodotStateMachine : Node
{
    // Sets the default state
    [Export]
    public NodePath DefaultState;
    // Sets whether we revert to the default state whenever the current state is null
    [Export]
    public bool RevertToDefaultState = false;

    public State DefaultStateObj { get; private set; }

    public Dictionary<String, State> States = new Dictionary<String, State>();
    public State CurrentState;
    public String CurrentStateName;
    public StateStack StateStack = new StateStack();

    [Signal]
    public delegate void StateChanged(State newState, StateStack StateStack);

    public override void _Ready()
    {
        base._Ready();
        if (DefaultState == null)
        {
            var parent = GetParent<Spatial>();
            GD.PushError("Default state not set on node: " + parent.Name);
            throw new Exception("Default state not set");
        }

        DefaultStateObj = GetNode<State>(DefaultState);

        foreach (var item in GetChildren())
        {
            if (!(item is State))
            {
                continue;
            }
            var child = item as State;
            States.Add(child.Name, child);
            child.Connect(nameof(State.ChangeState), this, nameof(this.ChangeState));
            if (child == DefaultStateObj)
            {
                CurrentState = child;
            }
        }

        CurrentState.Enter();
    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        if (CurrentState == null)
        {
            if (RevertToDefaultState)
            {
                CurrentState = DefaultStateObj;
                CurrentState.Enter();
            }
            else
            {
                return;
            }
        }
        CurrentState.Update(delta);
    }

    public void ChangeState(String NextState, bool push = false)
    {

        if ((NextState != nameof(Previous)) && !(States.ContainsKey(NextState)))
        {
            GD.PushError("State not in state dictionary: " + NextState.ToString());
            return;
        }

        if (CurrentState != null)
        {
            CurrentState.Exit();
        }
        if (!(NextState == nameof(Previous)))
        {
            if (push)
            {
                StateStack.Add(CurrentState);
            }
            StateStack.Add(States[NextState]);
        }
        CurrentState = StateStack[StateStack.Count - 1];
        StateStack.RemoveAt(StateStack.Count - 1);
        CurrentState.Enter();
        EmitSignal(nameof(StateChanged), CurrentState, StateStack);
    }
}
