using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField, ListDrawerSettings(ListElementLabelName = "Name")] private Pool[] _pools;

    public static PoolManager Instance { private set; get; }
    
    private void Awake()
    {
        Instance = this;
        foreach (Pool pool in _pools)
        {
            pool.Init(this);
        }
    }
    
    public PoolObject GetObject(PoolObject poolObject, Vector3 pos, Quaternion rot)
    {
        foreach (Pool pool in _pools)
        {
            if (pool.PoolObject == poolObject)
            {
                PoolObject po = pool.GetObject();
                po.transform.position = pos;
                po.transform.rotation = rot;
                return po;
            }
        }
        throw new UnityException("Либо ты не добавил такой объект в пул, либо ты ввёл мне NULL");
    }
    
    public PoolObject GetObject(string objectName, Vector3 pos, Quaternion rot)
    {
        foreach (Pool pool in _pools)
        {
            if (pool.Name == objectName)
            {
                PoolObject po = pool.GetObject();
                po.transform.position = pos;
                po.transform.rotation = rot;
                return po;
            }
        }
        throw new UnityException("Такого пула ты не создавал! Добавь его в список менеджера ;)");
    }

    public void ReturnObject(PoolObject poolObject)
    {
        foreach (Pool pool in _pools)
        {
            if (pool.Name == poolObject.Name)
            {
                pool.ReturnObject(poolObject);
                return;
            }
        }
        throw new UnityException("Некуда возвращать этот объект! Пул таких не принимает!");
    }
    

    [Serializable]
    private class Pool
    {
        public string Name = "New Pool";
        public int StartCount = 10;
        public PoolObject PoolObject;
        private Queue<PoolObject> _queue = new Queue<PoolObject>();
        private PoolManager _poolManager;
        
        [ShowIf("@PoolObject"),PreviewField(100, ObjectFieldAlignment.Left), ShowInInspector] private GameObject ObjectPreview => PoolObject != null ? PoolObject.gameObject : null;
        
        
        public void Init(PoolManager poolManager)
        {
            _poolManager = poolManager;
            for (int i = 0; i < StartCount; i++)
            {
                PoolObject poolObject = Instantiate(PoolObject);
                poolObject.transform.SetParent(poolManager.transform);
                poolObject.Init(Name, poolManager);
                _queue.Enqueue(poolObject);
            }
        }

        public PoolObject GetObject()
        {
            if (_queue.Count > 0)
            {
                PoolObject po = _queue.Dequeue();
                po.gameObject.SetActive(true);
                return po;
            }
                
            else
            {
                PoolObject po = Instantiate(PoolObject);
                po.transform.SetParent(_poolManager.transform);
                po.Init(Name, _poolManager);
                po.gameObject.SetActive(true);
                return po;
            }
        }

        public void ReturnObject(PoolObject poolObject)
        {
            poolObject.gameObject.SetActive(false);
            _queue.Enqueue(poolObject);
        }
    }
}
