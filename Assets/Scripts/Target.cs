using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

[RequireComponent(typeof(PoolObject))]
[RequireComponent(typeof(MeshRenderer))]
public class Target : MonoBehaviour
{
    public PoolObject PoolObject { get; private set; }

    private MeshRenderer _meshRenderer;

    private float _duration;
    private float _endPoint;
    private Tween tween;

    public bool IsIniting { get; private set; } = false;

    public UnityEvent<Target> OnDestroy = new UnityEvent<Target>();

    public void Initing(float speed, float endPoint)
    {
        IsIniting = true;

        _endPoint = endPoint;
        _duration = _endPoint / speed;

        _meshRenderer.material.color = Random.ColorHSV();

        Launch();
    }

    private void Awake()
    {
        PoolObject = GetComponent<PoolObject>();
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Launch()
    {
        tween = transform.DOLocalMoveY(_endPoint, _duration).SetEase(Ease.Linear);
        tween.onComplete = () =>
        {
            Destroy();
        };
    }

    public void Tap()
    {
        Destroy();
    }

    private void Destroy()
    {
        tween.Kill();
        OnDestroy?.Invoke(this);
    }
}
