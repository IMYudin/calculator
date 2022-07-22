using System;

public class KeyViewModel : ViewModelBase
{
    public event Action<KeyViewModel, bool> OnKeyStateChanged;
    
    private readonly KeyModel _keyModel;
    private readonly ICalculatorProvider _calculatorProvider;
    private bool _isInteractable;

    public bool IsInteractable
    {
        get => _isInteractable;
        private set
        {
            if (_isInteractable != value)
            {
                _isInteractable = value;
                OnKeyStateChanged?.Invoke(this, _isInteractable);
            }
        }
    }
    public string KeyDescription => _keyModel.Descriptor;
    public int KeySize => _keyModel.Size;
    
    public KeyViewModel(KeyModel keyModel, ICalculatorProvider calculatorProvider)
    {
        _keyModel = keyModel;
        _calculatorProvider = calculatorProvider;

        _keyModel.OnKeyStateChanged += KeyModel_OnKeyStateChanged;
    }
    
    public override void Dispose()
    {
        _keyModel.OnKeyStateChanged -= KeyModel_OnKeyStateChanged;

        base.Dispose();
    }

    public void CallKeyCommand()
    {
        _calculatorProvider.ExecuteCommand(_keyModel);
    }

    private void KeyModel_OnKeyStateChanged(KeyModel _, bool isInteractable)
    {
        IsInteractable = isInteractable;
    }

    public bool IsModelEqualTo(KeyModel keyModel) => keyModel.Equals(_keyModel);
    
    public override bool Equals(object obj)
    {
        return obj is KeyViewModel keyViewModel &&
               _keyModel.Equals(keyViewModel._keyModel);
    }
    
    public override int GetHashCode()
    {
        return _keyModel.GetHashCode();
    }
}