using Godot;
using System;

[GlobalClass]
public abstract partial class StateMachine<T> : Node
{
    public T State;
    public T PreviousState;

    [Signal]
    public delegate void StateChangedEventHandler(ObjectWrapper<T> newState, ObjectWrapper<T> oldState);

    public virtual void ChangeState(T newState)
    {
        PreviousState = State;
        State = newState;

        if (PreviousState != null)
            ExitState(PreviousState, newState);
        if (newState != null)
            EnterState(newState, PreviousState);

        var n = new ObjectWrapper<T>(newState);
        var p = new ObjectWrapper<T>(PreviousState);
        EmitSignal(SignalName.StateChanged, n, p);
    }

    public virtual void EnterState(T newState, T oldState) { }
    public virtual void ExitState(T oldState, T newState) { }
    public virtual void GetTransition(double delta) { }
    public abstract void UpdateState(double delta);
}
