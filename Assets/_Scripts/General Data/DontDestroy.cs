using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    public static DontDestroy Instance;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject); return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
