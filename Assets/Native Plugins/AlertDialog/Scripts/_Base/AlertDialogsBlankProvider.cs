using System;

public class AlertDialogsBlankProvider : IAlertDialogsProvider
{
    private Action _positiveCallback;
    
    public void CreateAlert(string message, string positiveButton, string negativeButton, Action positiveCallback = null, Action negativeCallback = null)
    {
        _positiveCallback = positiveCallback;
    }

    public void ShowAlert()
    {
        _positiveCallback?.Invoke();
    }
}