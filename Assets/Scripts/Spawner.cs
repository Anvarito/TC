using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Spawner : MonoBehaviour
{
    [SerializeField] private PoolObject _poolObject;
    [SerializeField] private PoolManager _poolManager;

    [Header("Settings")]
    [SerializeField] private float _spawnDelay;
    [SerializeField, Range(0, 15)] private float _endPoint;
    [SerializeField] private float _speed;

    private BoxCollider _collider;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        StartCoroutine(Spawning());
    }
    private IEnumerator Spawning()
    {
        yield return new WaitForSeconds(_spawnDelay);
        Target target = _poolManager.GetObject(_poolObject, GetSpawnPoint(), _poolObject.transform.rotation).GetComponent<Target>();

        if (!target.IsIniting)
            target.OnDestroy.AddListener(RemoveTarget);

        target.Initing(_speed, _endPoint);

        //Debug.DrawLine(target.transform.position, new Vector3(target.transform.position.x, target.transform.position.y + _endPoint, 0), Random.ColorHSV(), 10);

        StartCoroutine(Spawning());
    }

    private void RemoveTarget(Target target)
    {
        _poolManager.ReturnObject(target.PoolObject);
    }

    private Vector3 GetSpawnPoint()
    {
        Bounds bounds = _collider.bounds;

        float spawnX = Random.Range(bounds.min.x, bounds.max.x);
        float spawnY = Random.Range(bounds.min.y, bounds.max.y);

        Vector3 spawnPoint = new Vector3(spawnX, spawnY, 0);
        return spawnPoint;
    }
}
