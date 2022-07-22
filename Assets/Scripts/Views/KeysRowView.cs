using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeysRowView : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private HorizontalLayoutGroup _layoutGroup;
    [SerializeField] private LayoutElement _rowLayoutElement;

    public int _keysCount;
    public int _availableSpace;
    
    private readonly IList<KeyView> _keys = new List<KeyView>();

    public void Initialize(int space)
    {
        _keysCount = space;
        _availableSpace = space;

        UpdateRowHeight();
    }

    private void OnRectTransformDimensionsChange()
    {
        UpdateRowHeight();
    }

    public bool TryAdd(KeyView keyView)
    {
        if (_availableSpace < keyView.KeySize) return false;
        
        keyView.RectTransform.SetParent(_rectTransform);
        _keys.Add(keyView);
        
        _availableSpace -= keyView.KeySize;

        return true;
    }

    public void ReplaceAllKeys(RectTransform parent)
    {
        foreach (var key in _keys)
        {
            key.RectTransform.SetParent(parent);
        }
    }

    public void RemoveAllKeys()
    {
        _keys.Clear();
    }

    private void UpdateRowHeight()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(_rectTransform);
        _rowLayoutElement.preferredHeight = CalculateRowHeight(_keysCount);
    }

    private float CalculateRowHeight(int keysCount)
    {
        float widthWithoutPadding = _rectTransform.rect.width - _layoutGroup.padding.left - _layoutGroup.padding.right;
        return (widthWithoutPadding - _layoutGroup.spacing * (keysCount - 1)) / keysCount;
    }
}