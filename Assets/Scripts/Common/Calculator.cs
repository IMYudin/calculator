using System;

public class Calculator : ICalculatorProvider, IDisposable
{
    private readonly CalculatorModel _calculatorModel;
    
    public Calculator(CalculatorModel calculatorModel)
    {
        _calculatorModel = calculatorModel;
    }

    public void Run()
    {
        _calculatorModel.Load();
    }

    public void ExecuteCommand(KeyModel keyModel)
    {
        _calculatorModel.ExecuteKeyCommand(keyModel);
    }

    public void Dispose()
    {
        
    }
}