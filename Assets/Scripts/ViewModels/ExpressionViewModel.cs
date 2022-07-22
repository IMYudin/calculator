using System;
using System.Collections.Generic;

public class ExpressionViewModel : ViewModelBase
{
    private static readonly IDictionary<OperatorType, string> OperatorDescriptions = new Dictionary<OperatorType, string>
    {
        { OperatorType.Sum, "+" },
        { OperatorType.Sub, "-" },
        { OperatorType.Mul, "x" },
        { OperatorType.Div, "/" },
        { OperatorType.Eq, "=" }
    };

    public event Action<ExpressionViewModel, bool> OnExpressionDescriptionStateChanged;
    public event Action<ExpressionViewModel, string> OnExpressionDescriptionChanged;
    
    private readonly ExpressionModel _expressionModel;
    private readonly ICalculatorProvider _calculatorProvider;
    private readonly IAlertDialogsProvider _alertDialogsProvider;
    private readonly INotificationsProvider _notificationsProvider;

    private bool _isExpressionDescriptionEmpty;
    private string _expressionDescription;

    public bool IsExpressionDescriptionEmpty
    {
        get => _isExpressionDescriptionEmpty;
        private set
        {
            if (_isExpressionDescriptionEmpty != value)
            {
                _isExpressionDescriptionEmpty = value;
                OnExpressionDescriptionStateChanged?.Invoke(this, _isExpressionDescriptionEmpty);
            }
        }
    }
    
    public string ExpressionDescription
    {
        get => _expressionDescription;
        private set
        {
            if (_expressionDescription != value)
            {
                _expressionDescription = value;
                OnExpressionDescriptionChanged?.Invoke(this, _expressionDescription);
            }
        }
    }
    
    public ExpressionViewModel(
        ExpressionModel expressionModel, 
        ICalculatorProvider calculatorProvider, 
        IAlertDialogsProvider alertDialogsProvider,
        INotificationsProvider notificationsProvider)
    {
        _expressionModel = expressionModel;
        _calculatorProvider = calculatorProvider;
        _alertDialogsProvider = alertDialogsProvider;
        _notificationsProvider = notificationsProvider;

        _expressionModel.OnExpressionChanged += ExpressionModel_OnExpressionChanged;

        UpdateExpressionDescription(_expressionModel);
        ShowAlertDialogIfNeeded(expressionModel);
    }
    
    public override void Dispose()
    {
        _expressionModel.OnExpressionChanged -= ExpressionModel_OnExpressionChanged;

        base.Dispose();
    }

    private void UpdateExpressionDescription(ExpressionModel expressionModel)
    {
        IsExpressionDescriptionEmpty = expressionModel.State == ExpressionState.Default;
        ExpressionDescription = GenerateDescription(expressionModel);
    }

    private string GenerateDescription(ExpressionModel expressionModel)
    {
        string description = string.Empty;

        if (expressionModel.Nodes != null)
        {
            foreach (var node in expressionModel.Nodes)
            {
                if (node is ExpressionValueNode operandNode)
                {
                    description += operandNode.Value.ToString();
                }
                
                if (node is ExpressionNotificationNode notificationNode)
                {
                    description += notificationNode.Notification;
                }

                if (node is ExpressionActionNode operatorNode)
                {
                    if (!OperatorDescriptions.ContainsKey(operatorNode.Value)) continue;
                    
                    description += OperatorDescriptions[operatorNode.Value];
                }
            }
        }

        return description;
    }

    private void ShowAlertDialogIfNeeded(ExpressionModel expressionModel)
    {
        if (expressionModel.State != ExpressionState.ExceptionReadyResult) return;
        
        _alertDialogsProvider?.CreateAlert(
            "Get Premium to unlock this feature", 
            "Get Premium", 
            "Cancel", 
            expressionModel.ApplyExceptionResult,
            () =>
            {
                _notificationsProvider.ShowNotification(string.Empty, "No money – no honey");
                _expressionModel.SetState(ExpressionState.ExceptionResult);
            });
        
        
        _alertDialogsProvider?.ShowAlert();
    }
    
    private void ExpressionModel_OnExpressionChanged(ExpressionModel expressionModel)
    {
        UpdateExpressionDescription(_expressionModel);

        ShowAlertDialogIfNeeded(expressionModel);
    }
}