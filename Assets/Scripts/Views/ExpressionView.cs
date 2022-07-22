using UnityEngine;
using UnityEngine.UI;

public class ExpressionView : BindableView<ExpressionViewModel>
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private KeypadView _keypadView;
    [SerializeField] private Text _emptyDescriptionText;
    [SerializeField] private Text _descriptionText;

    private void Awake()
    {
        _keypadView.RectTransformDimensionsChanged += KeypadView_OnRectTransformDimensionsChanged;
    }

    private void OnDestroy()
    {
        ViewModel.OnExpressionDescriptionChanged -= ViewModel_OnExpressionDescriptionChanged;
        ViewModel.OnExpressionDescriptionStateChanged -= ViewModel_OnExpressionDescriptionStateChanged;
        
        _keypadView.RectTransformDimensionsChanged -= KeypadView_OnRectTransformDimensionsChanged;
    }

    protected override void ConfigureAfterBind()
    {
        ViewModel.OnExpressionDescriptionChanged += ViewModel_OnExpressionDescriptionChanged;
        ViewModel.OnExpressionDescriptionStateChanged += ViewModel_OnExpressionDescriptionStateChanged;
        
        _descriptionText.text = ViewModel.ExpressionDescription;

        UpdateDescriptionTextsActivity();
    }

    private void UpdateDescriptionTextsActivity()
    {
        _emptyDescriptionText.gameObject.SetActive(ViewModel.IsExpressionDescriptionEmpty);
        _descriptionText.gameObject.SetActive(!ViewModel.IsExpressionDescriptionEmpty);
    }

    private void KeypadView_OnRectTransformDimensionsChanged(RectTransform rectTransform)
    {
        _rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.x, -rectTransform.rect.height);
    }
    
    private void ViewModel_OnExpressionDescriptionChanged(ExpressionViewModel _, string description)
    {
        _descriptionText.text = description;
    }
    
    private void ViewModel_OnExpressionDescriptionStateChanged(ExpressionViewModel _, bool isEmpty)
    {
        UpdateDescriptionTextsActivity();
    }
}