# Godot Mono State Machine
Simple state machine so I don't have to write it all over again.

Add a `StateMachine` child to your scene. Create scenes that inherit from `State` and add them as children to the `StateMachine` node. Wire up the `StateMachine` in the scene's `_Input` method with 

```
StateMachine.CurrentState.HandleInput(@event);
```

And you should be ready to go.