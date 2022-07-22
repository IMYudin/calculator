using System;

public interface IAlertDialogsProvider
{
    void CreateAlert(string message, string positiveButton, string negativeButton, Action positiveCallback = null, Action negativeCallback = null);
    void ShowAlert();
}
