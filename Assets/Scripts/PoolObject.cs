using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObject : MonoBehaviour
{
    public string Name { get; private set; }
    private PoolManager _manager;

    public void Init(string name, PoolManager manager)
    {
        Name = name;
        _manager = manager;
        gameObject.SetActive(false);
    }
    
    public void ReturnInPool()
    {
        if(_manager) _manager.ReturnObject(this);
    }
}
