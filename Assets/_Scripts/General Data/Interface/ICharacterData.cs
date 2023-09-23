using System.Collections.Generic;

public interface ICharacterData<T>
{
    public void Initialized();
    public T GetController(string controllerName);
    public List<T> GetControllers();
    
}
