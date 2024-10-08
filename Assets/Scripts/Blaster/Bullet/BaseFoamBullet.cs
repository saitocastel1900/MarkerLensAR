using UniRx;
using UnityEngine;

/// <summary>
/// 弾の基底
/// </summary>
public abstract class BaseFoamBullet : MonoBehaviour
{
    /// <summary>
    /// 弾
    /// </summary>
    protected FoamBulletCore _foamBulletCore;

    private void Start()
    {
        _foamBulletCore = this.gameObject.GetComponent<FoamBulletCore>();
        
        _foamBulletCore
            .IsInitialized
            .Where(isInitialized=>isInitialized==true)
            .Subscribe(_ => OnInitialize())
            .AddTo(this);

        OnStart();
    }

    protected virtual void OnStart()
    {
    }

    protected abstract void OnInitialize();
}