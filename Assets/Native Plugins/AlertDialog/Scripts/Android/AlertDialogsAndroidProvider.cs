using System;
using UnityEngine;

public class AlertDialogsAndroidProvider : IAlertDialogsProvider
{
    private const string UnityClassName = "com.unity3d.player.UnityPlayer";
    private const string UnityActivityFieldName = "currentActivity";
    private const string AlertDialogsNativeName = "com.imyudin.unityplugin.AlertDialogsHandler";
    
    private const string ReceiveUnityActivityMethodName = "receiveUnityActivity";
    private const string CreateAlertMethodName = "createAlertDialog";
    private const string ShowAlertMethodName = "showAlertDialog";
    
    private AndroidJavaClass _unityClass;
    private AndroidJavaObject _unityActivity;
    private readonly AndroidJavaObject _nativeAlertDialogsHandler;

    public AlertDialogsAndroidProvider()
    {
        _unityClass = new AndroidJavaClass(UnityClassName);
        _unityActivity = _unityClass.GetStatic<AndroidJavaObject>(UnityActivityFieldName);
        _nativeAlertDialogsHandler = new AndroidJavaObject(AlertDialogsNativeName);
        
        _nativeAlertDialogsHandler.CallStatic(ReceiveUnityActivityMethodName, _unityActivity);
    }

    public void CreateAlert(string message, string positiveButton, string negativeButton, Action positiveCallback = null, Action negativeCallback = null)
    {
        _nativeAlertDialogsHandler.Call(CreateAlertMethodName, message, positiveButton, negativeButton, new AndroidAlertDialogCallback(positiveCallback, negativeCallback));
    }

    public void ShowAlert()
    {
        _nativeAlertDialogsHandler.Call(ShowAlertMethodName);
    }
}