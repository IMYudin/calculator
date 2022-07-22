using System;
using System.Collections.Generic;
using UnityEngine;

public class KeypadView : BindableView<KeypadViewModel>
{
    [SerializeField] private RectTransform _keypadRoot;
    [SerializeField] private KeyView _keyViewTemplate;
    [SerializeField] private KeysRowView _keysRowTemplate;

    public event Action<RectTransform> RectTransformDimensionsChanged;
    
    private int _activeKeysRowIndex = -1;
    
    private readonly IList<KeysRowView> _keysRows = new List<KeysRowView>();
    private readonly IDictionary<KeyViewModel, KeyView> _keyViews = new Dictionary<KeyViewModel, KeyView>();

    protected override void ConfigureAfterBind()
    {
        CreateKeysIfNeeded(ViewModel.Keys);
        
        ViewModel.OnRowSizeChanged += ViewModel_OnRowSizeChanged;
        ViewModel.OnKeysCollectionChanged += ViewModel_OnKeysCollectionChanged;
    }

    private void OnRectTransformDimensionsChange()
    {
        RectTransformDimensionsChanged?.Invoke(_keypadRoot);
    }

    private void OnDestroy()
    {
        ViewModel.OnRowSizeChanged -= ViewModel_OnRowSizeChanged;
        ViewModel.OnKeysCollectionChanged -= ViewModel_OnKeysCollectionChanged;
    }

    private void ReLayout()
    {
        foreach (var keysRow in _keysRows)
        {
            keysRow.ReplaceAllKeys(_keypadRoot);
            keysRow.RemoveAllKeys();
            keysRow.Initialize(ViewModel.RowSize);
        }

        _activeKeysRowIndex = -1;
        
        foreach (var keyView in _keyViews.Values)
        {
            AddKeyViewToRow(keyView);
        }
    }

    private void CreateKeysIfNeeded(IList<KeyViewModel> keys)
    {
        if (keys == null) return;
        
        foreach (var key in keys)
        {
            if (_keyViews.ContainsKey(key)) continue;

            var keyView = Instantiate(_keyViewTemplate, _keypadRoot);
            keyView.BindTo(key);
            
            AddKeyViewToRow(keyView);
            _keyViews.Add(key, keyView);
        }
    }

    private void AddKeyViewToRow(KeyView keyView)
    {
        if (_activeKeysRowIndex == -1)
        {
            TryGetKeysRow(0);
        }

        if (!_keysRows[_activeKeysRowIndex].TryAdd(keyView))
        {
            TryGetKeysRow(++_activeKeysRowIndex);
            _keysRows[_activeKeysRowIndex].TryAdd(keyView);
        }

        void TryGetKeysRow(int index)
        {
            if (_keysRows.Count <= index)
            {
                var keysRow = Instantiate(_keysRowTemplate, _keypadRoot);
                keysRow.Initialize(ViewModel.RowSize);
                _keysRows.Add(keysRow);
            }

            _activeKeysRowIndex = index;
        }
    }
    
    private void ViewModel_OnRowSizeChanged(KeypadViewModel vm, int rowSize)
    {
        ReLayout();
    }

    private void ViewModel_OnKeysCollectionChanged(KeypadViewModel vm, IList<KeyViewModel> keys)
    {
        CreateKeysIfNeeded(keys);
    }
}