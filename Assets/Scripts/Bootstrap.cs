using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    private FileDataProvider<GameSaveData> _dataProvider;
    private IAlertDialogsProvider _alertDialogsProvider;
    private INotificationsProvider _notificationsProvider;
    
    private void Awake()
    {
        ConfigureServices();

        GetSimpleCalculator().Run();
    }

    private void ConfigureServices()
    {
        _dataProvider = new FileDataProvider<GameSaveData>();
        _dataProvider.Load();

        _alertDialogsProvider = AlertDialogsProvider.CreateAlertDialogsProvider();
        _notificationsProvider = NotificationsProvider.CreateNotificationsProvider();

        ServicesContainer.Instance.AddService<IDataBindingContext>(new SimpleBindingContext());
    }

    private Calculator GetSimpleCalculator()
    {
        IKeypadProvider keypadProvider = new DefaultKeypadProvider();
        KeypadModel keypadModel = new (keypadProvider);
        
        IExpressionProvider expressionProvider = new ExpressionProvider(_dataProvider);
        IExpressionCalculationProvider expressionCalculationProvider = new ExpressionCalculationProvider();
        ExpressionModel expressionModel = new (expressionProvider, expressionCalculationProvider);

        CalculatorModel calculatorModel = new (expressionModel, keypadModel);
        Calculator calculator = new (calculatorModel);

        ServicesContainer.Instance.GetService<IDataBindingContext>().CreateUniqueViewModel(() => new KeypadViewModel(keypadModel, calculator));
        ServicesContainer.Instance.GetService<IDataBindingContext>().CreateUniqueViewModel(() => new ExpressionViewModel(expressionModel, calculator, _alertDialogsProvider, _notificationsProvider));

        return calculator;
    }
}