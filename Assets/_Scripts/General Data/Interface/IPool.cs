using System;

public interface IPool<out T>
{
    void Init(Action<T> returnAction); 
}