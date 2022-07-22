using System;
using UnityEngine;

public class AndroidAlertDialogCallback : AndroidJavaProxy
{
    private const string AlertDialogNativeClassName = "com.imyudin.unityplugin.AlertDialogCallback";

    private readonly Action _positiveCallback;
    private readonly Action _negativeCallback;
    
    public AndroidAlertDialogCallback(Action positiveCallback = null, Action negativeCallback = null) : base(AlertDialogNativeClassName)
    {
        _positiveCallback = positiveCallback;
        _negativeCallback = negativeCallback;
    }

    public void onPositive() => _positiveCallback?.Invoke();

    public void onNegative() => _negativeCallback?.Invoke();
}