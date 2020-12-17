using Godot;
using System;
using System.Collections.Generic;

public class GodotStateMachine : Node
{
	public Dictionary<String, State> States = new Dictionary<String, State>();
	public State CurrentState;
	public String CurrentStateName;
	public Stack<State> StateStack = new Stack<State>();
	public delegate void StateChanged(Stack<State> StateStack);

	public override void _Ready()
	{
		base._Ready();
		foreach (var item in GetChildren())
		{
			if (!(item is State))
			{
				continue;
			}
			var child = item as State;
			States.Add(child.Name, child);
			child.Connect(nameof(State.ChangeState), this, nameof(this.ChangeState));
			if (child.Name == nameof(Idle))
			{
				CurrentState = child;
			}
		}
	}

	public override void _PhysicsProcess(float delta)
	{
		base._PhysicsProcess(delta);
		if (CurrentState == null)
		{
			return;
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
				StateStack.Push(CurrentState);
			}
			StateStack.Push(States[NextState]);
		}
		CurrentState = StateStack.Pop();
		CurrentState.Enter();
		EmitSignal(nameof(StateChanged), StateStack);
	}
}
