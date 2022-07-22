using UnityEngine;

public abstract class ViewBase : MonoBehaviour
{
}

public abstract class BindableView<T> : ViewBase, IBindableView where T : ViewModelBase
{
    public T ViewModel { get; private set; }

    public void BindTo(ViewModelBase viewModel)
    {
        ViewModel = (T)viewModel;
        
        ConfigureAfterBind();
    }

    protected abstract void ConfigureAfterBind();
}