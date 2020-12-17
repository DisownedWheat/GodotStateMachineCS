using Godot;
using System;

public class State : Node
{
	[Signal]
	public delegate void ChangeState(String NextState, bool push);

	public virtual void Enter() { }
	public virtual void Exit() { }
	public virtual void HandleInput(InputEvent @event) { }
	public virtual void Update(float delta) { }
	public virtual void OnAnimationFinished(String AnimationName) { }
}
