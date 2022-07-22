using UnityEngine;

public class Binding : MonoBehaviour
{
    [SerializeField] private ViewBase _view;
    [SerializeField, ViewModelType] private string _viewModel;
    
    private void Awake()
    {
        if (_view is IBindableView bindableView &&
            ServicesContainer.Instance.GetService<IDataBindingContext>()
                .TryGetViewModel(_viewModel, out ViewModelBase viewModel))
        {
            bindableView.BindTo(viewModel);   
        }
    }
}