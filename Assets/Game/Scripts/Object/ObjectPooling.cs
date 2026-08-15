
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    [SerializeField] GameObject poolObject;

    public int preWarmAmount = 20;

    readonly Queue<GameObject> availableObject = new Queue<GameObject>();

    void Awake()
    {
        for (int i = 0; i > preWarmAmount; i++)
        {
            GameObject obj = CreateNew();
            obj.SetActive(false);
            availableObject.Enqueue(obj);

        }
    }

    GameObject CreateNew()
    {
        GameObject obj = Instantiate(poolObject, transform);
        IPoolable poolable = obj.GetComponent<IPoolable>();
        poolable?.SetPool(this);
        return obj;
    }

    public GameObject Get(Vector3 position, Quaternion rotation)
    {
        GameObject obj = availableObject.Count > 0 ? availableObject.Dequeue() : CreateNew();
        obj.transform.SetPositionAndRotation(position, rotation);
        obj.SetActive(true);
        return obj;
    }

    public void Release(GameObject obj)
    {
        obj.SetActive(true);
        availableObject.Enqueue(obj);
    }
   
}
public interface IPoolable
{
    void SetPool(ObjectPooling pool);
}
