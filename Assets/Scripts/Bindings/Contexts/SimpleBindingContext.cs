using System;
using System.Collections.Generic;

public class SimpleBindingContext : IDataBindingContext
{
    private readonly IDictionary<string, ViewModelBase> _viewModelsByTypes = new Dictionary<string, ViewModelBase>();

    public T CreateUniqueViewModel<T>(Func<T> createFunc) where T : ViewModelBase
    {
        var viewModel = createFunc.Invoke();
        _viewModelsByTypes.Add(viewModel.GetType().FullName, viewModel);

        return viewModel;
    }

    public bool TryGetViewModel<T>(string typeName, out T viewModel) where T : ViewModelBase
    {
        viewModel = null;

        if (!_viewModelsByTypes.ContainsKey(typeName)) return false;

        if (_viewModelsByTypes[typeName] is T resultViewModel)
        {
            viewModel = resultViewModel;
            return true;
        }

        return false;
    }
}