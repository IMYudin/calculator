using System.Collections.Generic;

public class CalculatorModel : ModelBase
{
    private readonly ExpressionModel _expressionModel;
    private readonly KeypadModel _keypadModel;
    
    public CalculatorModel(ExpressionModel expressionModel, KeypadModel keypadModel)
    {
        _expressionModel = expressionModel;
        _keypadModel = keypadModel;
    }

    public void Load()
    {
        _expressionModel.Load();
        _keypadModel.Load();
        
        _keypadModel.UpdateAvailableKeys(GetAvailableKeyActionType(_expressionModel.State));
    }

    public void AddKeys(IList<KeyModel> keys)
    {
        _keypadModel.AddKeys(keys);
    }

    public void ExecuteKeyCommand(KeyModel keyModel)
    {
        switch (keyModel.ActionType)
        {
            case KeyActionType.Operand:
                if (keyModel is KeyOperandModel keyOperandModel)
                {
                    _expressionModel.TryModifyOperandNode(keyOperandModel.Value);
                }
                break;
            
            case KeyActionType.Operator:
                if (keyModel is KeyOperatorModel keyOperatorModel)
                {
                    _expressionModel.TryModifyOperatorNode(keyOperatorModel.OperatorType);
                }
                break;
            
            case KeyActionType.Confirm:
                _expressionModel.Confirm();
                break;
            
            case KeyActionType.Result:
                _expressionModel.CalculateResult();
                break;
            
            case KeyActionType.Clear:
                _expressionModel.Clear();
                break;
        }
        
        _keypadModel.UpdateAvailableKeys(GetAvailableKeyActionType(_expressionModel.State));
    }

    private KeyActionType GetAvailableKeyActionType(ExpressionState expressionState)
    {
        switch (expressionState)
        {
            case ExpressionState.Default:
                return KeyActionType.Operand;
            
            case ExpressionState.OperandInput:
                return KeyActionType.Operand | KeyActionType.Confirm;
            
            case ExpressionState.OperatorNoneInput:
                return KeyActionType.Operator;
            
            case ExpressionState.OperatorInput:
                return KeyActionType.Operator | KeyActionType.Confirm;
            
            case ExpressionState.ResultReady:
                return KeyActionType.Result;
            
            case ExpressionState.SuccessfulResult:
            case ExpressionState.ExceptionReadyResult:
            case ExpressionState.ExceptionResult:
                return KeyActionType.Clear;
            
            default:
                return KeyActionType.All; 
        }
    }
}