using Godot;

public abstract partial class State : Node
{
	[Signal]
	public delegate void ChangeStateEventHandler(string newStateName);

	public abstract void Enter(State previousState);
	public abstract void Exit(State nextState);
	public abstract void HandleInput(InputEvent @event);
	public abstract void Update(double delta);
	public abstract void OnAnimationFinished(string animName);
}