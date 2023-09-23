using UnityEngine;

public class SingletonManager <T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _reference;
    public static bool IsValid => _reference != null;

    public bool isDontDestroySingleton;

    public static T Instance
    {
        get
        {
            if(!IsValid)
                _reference = FindAnyObjectByType<T>();
            return _reference;
        }
    }
    protected virtual void Awake()
    {
        if (IsValid && _reference != this)
            Destroy(gameObject);
        else
        {
            _reference = (T)(MonoBehaviour)this;
            if (isDontDestroySingleton)
            {
                DontDestroyOnLoad(gameObject);
            }
        }
    }

}
