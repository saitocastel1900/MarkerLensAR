using System;
using Commons.Utility;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class FoamBulletCore : MonoBehaviour
{
    /// <summary>
    /// 初期化したか
    /// </summary>
    public IReactiveProperty<bool> IsInitialized => _isInitialized;
    private readonly BoolReactiveProperty _isInitialized = new BoolReactiveProperty(false);

    /// <summary>
    /// 
    /// </summary>
    public IReactiveProperty<bool> IsHitProp => _isHitProp;
    private BoolReactiveProperty _isHitProp = new BoolReactiveProperty(false);

    /// <summary>
    /// 
    /// </summary>
    public Vector3 Direction => _direction;
    private Vector3 _direction;

    /// <summary>
    /// 
    /// </summary>
    public float Velocity => _velocity;
    private float _velocity;

    private void Start()
    {
        this.gameObject
            .OnCollisionEnterAsObservable()
            .Subscribe(hit =>
            {
                var hitable = hit.gameObject.GetComponent<IHitable>();
                if (hitable != null)
                {
                    hitable.Hit(hit.contacts[0].point);
                    _isHitProp.Value= true;
                }
            })
            .AddTo(this.gameObject);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="velocity"></param>
    /// <returns></returns>
    public IObservable<Unit> InitializeFoamBullet(Vector3 direction, float velocity)
    {
        _direction = direction;
        _velocity = velocity;
        _isInitialized.Value = true;
        _isInitialized.AddTo(this.gameObject);

        return Observable.Merge(
                this.gameObject.OnBecameInvisibleAsObservable(),
                _isHitProp.Where(isHit=>isHit==true).AsUnitObservable()
                )
            .FirstOrDefault()
            .Do(_ =>
            {
                _isInitialized.Value = false;
                _isHitProp.Value= false;
            });
    }
}