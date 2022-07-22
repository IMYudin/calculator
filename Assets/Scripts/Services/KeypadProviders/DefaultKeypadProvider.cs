using System.Collections.Generic;

public class DefaultKeypadProvider : IKeypadProvider
{
    private readonly IList<KeyModel> _keyModels;
    
    public int KeysInRowCount { get; set; }

    public DefaultKeypadProvider()
    {
        KeysInRowCount = 4;
        
        _keyModels = new List<KeyModel>
        {
            new KeyOperandModel(KeyActionType.Operand, 0, "0", 1),
            new KeyOperandModel(KeyActionType.Operand, 1, "1", 1),
            new KeyOperandModel(KeyActionType.Operand, 2, "2", 1),
            new KeyOperatorModel(KeyActionType.Operator, OperatorType.Sum, "+", 1),
            
            new KeyOperandModel(KeyActionType.Operand, 3, "3", 1),
            new KeyOperandModel(KeyActionType.Operand, 4, "4", 1),
            new KeyOperandModel(KeyActionType.Operand, 5, "5", 1),
            new KeyOperatorModel(KeyActionType.Operator, OperatorType.Sub, "-", 1),
            
            new KeyOperandModel(KeyActionType.Operand, 6, "6", 1),
            new KeyOperandModel(KeyActionType.Operand, 7, "7", 1),
            new KeyOperandModel(KeyActionType.Operand, 8, "8", 1),
            new KeyOperatorModel(KeyActionType.Operator, OperatorType.Mul, "x", 1),
            
            new KeyOperandModel(KeyActionType.Operand, 9, "9", 1),
            new (KeyActionType.Confirm, "Confirm", 2),
            new KeyOperatorModel(KeyActionType.Operator, OperatorType.Div, "/", 1),
            
            new (KeyActionType.Result, "Get Result", 3),
            new (KeyActionType.Clear, "C", 1),
        };
    }

    public IList<KeyModel> GetAllKeys() => _keyModels;
    
    public void AddKeys(IList<KeyModel> keys)
    {
        foreach (var key in keys)
        {
            _keyModels.Add(key);
        }
    }
}