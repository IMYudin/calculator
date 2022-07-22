using System;
using System.Collections.Generic;

public class KeypadModel : ModelBase
{
    public event Action<KeypadModel, int> OnKeypadRowSizeChanged;
    public event Action<KeypadModel, IList<KeyModel>> OnKeysCollectionChanged;

    private readonly IKeypadProvider _keypadProvider;
    private int _keysInRowCount;

    public int KeysInRowCount
    {
        get => _keysInRowCount;
        private set
        {
            if (_keysInRowCount != value)
            {
                _keysInRowCount = value;
                OnKeypadRowSizeChanged?.Invoke(this, _keysInRowCount);
            }
        }
    }
    public IList<KeyModel> Keys { get; private set; }

    public KeypadModel(IKeypadProvider keypadProvider)
    {
        _keypadProvider = keypadProvider;
    }

    public void Load()
    {
        SyncRowSize();
        SyncKeys();
    }
    
    public void AddKeys(IList<KeyModel> keys)
    {
        _keypadProvider.AddKeys(keys);
        SyncKeys();
    }

    public void UpdateAvailableKeys(KeyActionType keyActionType)
    {
        foreach (var key in Keys)
        {
            key.UpdateState(keyActionType);
        }
    }

    private void SyncRowSize()
    {
        KeysInRowCount = _keypadProvider.KeysInRowCount;
    }

    private void SyncKeys()
    {
        Keys = _keypadProvider.GetAllKeys();
        OnKeysCollectionChanged?.Invoke(this, Keys);
    }
}