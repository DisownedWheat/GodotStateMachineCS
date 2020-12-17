tool
extends EditorPlugin

func _enter_tree():
	var gui = get_editor_interface().get_base_control()
	add_custom_type(
		"StateMachine", "Node",
		preload("res://addons/GodotStateMachineCS/GodotStateMachine.cs"),
		gui.get_icon("Script" ,"EditorIcons")
	)
	add_custom_type(
		"State", "Node",
		preload("res://addons/GodotStateMachineCS/State.cs"),
		gui.get_icon("Script", "EditorIcons")
	)

func _exit_tree():
	remove_custom_type("StateMachine")
	remove_custom_type("State")
