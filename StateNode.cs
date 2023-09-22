using Godot;
using System;

public partial class StateNode<T, V> : Node where T : Node where V : StateNode<T, V>
{
    public T Actor { get; set; }

    [Signal]
    public delegate void ChangeStateEventHandler(ObjectWrapper<Type> newState);

    public override void _Ready()
    {
        base._Ready();

        SetPhysicsProcess(false);
        SetProcess(false);
        SetProcessInput(false);

    }

    public virtual void Enter(V previousState)
    {
        SetPhysicsProcess(true);
        SetProcess(true);
        SetProcessInput(true);
    }
    public virtual void Exit(V nextState)
    {
        SetPhysicsProcess(false);
        SetProcess(false);
        SetProcessInput(false);
    }
    public virtual void OnAnimationFinished(string animName) { }
    public virtual void ConnectSignals() { }


    protected ObjectWrapper<Type> GetWrapper(Type type)
    {
        return new ObjectWrapper<Type>(type);
    }

}