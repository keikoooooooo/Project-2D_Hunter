using System;

public interface IPool<T>
{
    void Init(Action<T> returnAction); 
}