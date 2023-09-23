using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour, IPool<T>
{
    public Queue<T> ListPool;
    private readonly T prefab;
    private readonly Transform transformParent;

    public ObjectPool(T prefab, Transform transformParent, int poolSize) // tạo contructor cho class
    {
        ListPool = new Queue<T>(poolSize);
        this.prefab = prefab;
        this.transformParent = transformParent;
        for (int i = 0; i < poolSize; i++)
        {
            ListPool.Enqueue(Create());  // cứ mỗi 1 vòng for -> sẽ tạo 1 object và thêm vào cuối hàng đợi trong danh sách  bằng lệnh: Enqueue(T)
        }
    }
    public ObjectPool(List<T> prefabList, Transform _transformParent, int poolSize)
    {
        ListPool = new Queue<T>(poolSize);     
        transformParent = _transformParent;    
        for (int i = 0; i < poolSize; i++)
        {
            prefab = prefabList[i];
            ListPool.Enqueue(Create());
        }
        prefab = prefabList[Random.Range(0, prefabList.Count)];
    }

    private T Create() // tạo 1 object 
    {
        T newObj = Object.Instantiate(prefab, transformParent);
        newObj.gameObject.SetActive(false);
        newObj.Init(Return);
        return newObj;
    }

    private void Return(T obj) // trả object truyền vào về lại danh sách 
    {
        obj.gameObject.SetActive(false);
        ListPool.Enqueue(obj);
    }

    public T Get() // lấy object đầu trong danh sách 
    {
        if (ListPool.Count == 0)
        {
            T newObj = Create();
            ListPool.Enqueue(newObj);
        }

        T Obj = ListPool.Dequeue();
        Obj.gameObject.SetActive(true);
        return Obj;
    }

}
