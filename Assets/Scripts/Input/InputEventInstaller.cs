using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

/// <summary>
/// 入力を注入する
/// </summary>
public class InputEventInstaller : MonoInstaller
{
    /// <summary>
    /// 発射ボタン
    /// </summary>
    [SerializeField] private Button _shotButton ;
    
    /// <summary>
    ///　ゲームを開始させるボタン
    /// </summary>
    [SerializeField] private Button _gameStartPanelButton ;
    
    public override void InstallBindings()
    {
        //プラットフォームに応じて、注入する入力を分ける
#if UNITY_EDITOR
        Container.Bind(typeof(IInputEventProvider), 
                typeof(IInitializable), typeof(IDisposable))
            .To<MouseInputProvider>().AsSingle().WithArguments(_shotButton,_gameStartPanelButton);
#elif UNITY_ANDROID
        Container.Bind(typeof(IInputEventProvider), 
                typeof(IInitializable), typeof(IDisposable))
            .To<TouchInputProvider>().AsSingle().WithArguments(_shotButton,_gameStartPanelButton);
#endif
    }
}