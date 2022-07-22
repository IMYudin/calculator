using System;
using System.Collections.Generic;

public class KeypadViewModel : ViewModelBase
{
    public event Action<KeypadViewModel, int> OnRowSizeChanged;
    public event Action<KeypadViewModel, IList<KeyViewModel>> OnKeysCollectionChanged;
    
    private readonly KeypadModel _keypadModel;
    private readonly ICalculatorProvider _calculatorProvider;

    private int _rowSize;

    public int RowSize
    {
        get => _rowSize;
        private set
        {
            if (_rowSize != value)
            {
                _rowSize = value;
                OnRowSizeChanged?.Invoke(this, _rowSize);
            }
        }
        
    }
    public List<KeyViewModel> Keys { get; } = new ();

    public KeypadViewModel(KeypadModel keypadModel, ICalculatorProvider calculatorProvider)
    {
        _keypadModel = keypadModel;
        _calculatorProvider = calculatorProvider;

        keypadModel.OnKeypadRowSizeChanged += KeypadModel_OnKeypadRowSizeChanged;
        keypadModel.OnKeysCollectionChanged += KeypadModel_OnKeysCollectionChanged;

        RowSize = keypadModel.KeysInRowCount;
        AddKeys(keypadModel.Keys);
    }
    
    public override void Dispose()
    {
        _keypadModel.OnKeypadRowSizeChanged -= KeypadModel_OnKeypadRowSizeChanged;
        _keypadModel.OnKeysCollectionChanged -= KeypadModel_OnKeysCollectionChanged;
        
        base.Dispose();
    }

    private void AddKeys(IList<KeyModel> keys)
    {
        if (keys == null) return;
        
        foreach (var key in keys)
        {
            if (Keys.Exists(k => k.IsModelEqualTo(key))) continue;
            
            Keys.Add(new KeyViewModel(key, _calculatorProvider));
        }
        
        OnKeysCollectionChanged?.Invoke(this, Keys);
    }
    
    private void KeypadModel_OnKeypadRowSizeChanged(KeypadModel keypadModel, int rowSize)
    {
        RowSize = rowSize;
    }

    private void KeypadModel_OnKeysCollectionChanged(KeypadModel keypadModel, IList<KeyModel> keys)
    {
        AddKeys(keys);
    }
}