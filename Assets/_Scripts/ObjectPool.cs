using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ObjectPool : MonoBehaviour
{
    public GameObject prefab;
    public GameObject transformParent;
    public int poolSize;
    private List<GameObject> objects;

    void Start()
    {
        // Initialize the objects list and instantiate the pool of objects.
        objects = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab, transformParent.transform);
            obj.SetActive(false);
            objects.Add(obj);
        }
    }

    // This method retrieves an object from the pool.
    public GameObject GetObject()
    {
        foreach (GameObject obj in objects)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                return obj;
            }
        }
        // If no inactive objects are found, instantiate a new object and add it to the pool.
        GameObject newObj = Instantiate(prefab, transformParent.transform);
        newObj.SetActive(true);
        objects.Add(newObj);
        return newObj;
    }

    // This method returns an object to the pool by deactivating it.
    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
    }
    // This method returns all active objects in the pool to the pool by deactivating them.
    public void ReturnAll()
    {
        foreach (GameObject obj in objects)
        {
            if (obj.activeInHierarchy)
            {
                obj.SetActive(false);
            }
        }
    }
}

