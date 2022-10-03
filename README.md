# Godot Mono State Machine
Simple state machine so I don't have to write it all over again.

To start clone the repository into your `addons` folder (or create it in the root of your project if it doesn't exist). 

Add a `GodotStateMachine` child to your scene. Create scenes that inherit from `State` and add them as children to the `GodotStateMachine` node. Wire up the `GodotStateMachine` in the scene's `_Input` method with 

```cs
// Of course it would much better to add a property called StateMachine or similar
// on your class
GetNode<GodotStateMachine>("GodotStateMachine").CurrentState.HandleInput(@event);
```

And you should be ready to go.