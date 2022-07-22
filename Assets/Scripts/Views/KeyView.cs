using UnityEngine;
using UnityEngine.UI;

public class KeyView : BindableView<KeyViewModel>
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private LayoutElement _layoutElement;
    [SerializeField] private Button _button;
    [SerializeField] private Text _keyText;

    public RectTransform RectTransform => _rectTransform;
    public int KeySize => ViewModel.KeySize;

    private void Awake()
    {
        _button.onClick.AddListener(OnKeyClicked);
    }

    protected override void ConfigureAfterBind()
    {
        _keyText.text = ViewModel.KeyDescription;
        _layoutElement.flexibleWidth = ViewModel.KeySize;
        _button.interactable = ViewModel.IsInteractable;

        ViewModel.OnKeyStateChanged += ViewModel_OnKeyStateChanged;
    }
    
    private void OnDestroy()
    {
        ViewModel.OnKeyStateChanged -= ViewModel_OnKeyStateChanged;
    }

    private void OnKeyClicked() => ViewModel?.CallKeyCommand();
    
    private void ViewModel_OnKeyStateChanged(KeyViewModel vm, bool isInteractable)
    {
        _button.interactable = ViewModel.IsInteractable;
    }
}