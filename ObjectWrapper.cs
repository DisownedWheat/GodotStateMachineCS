using Godot;
using System;

public partial class ObjectWrapper<T> : RefCounted
{
    public T Value { get; init; }

    public ObjectWrapper(T obj)
    {
        Value = obj;
    }
}
