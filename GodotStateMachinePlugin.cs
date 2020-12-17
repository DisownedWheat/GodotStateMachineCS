using Godot;
using System;

[Tool]
public class GodotStateMachinePlugin : EditorPlugin
{

	public override void _EnterTree()
	{
		base._EnterTree();
		var machineScript = GD.Load<Script>("res://addons/godot-state-machine-cs/GodotStateMachine.cs");
		var stateScript = GD.Load<Script>("res://addons/godot-state-machine-cs/State.cs");
		var icon = GetEditorInterface().GetBaseControl().GetIcon("Script", "EditorIcons");
		AddCustomType("StateMachine", "Node", machineScript, icon);
		AddCustomType("State", "Node", stateScript, icon);
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		RemoveCustomType("StateMachine");
		RemoveCustomType("State");
	}
}
