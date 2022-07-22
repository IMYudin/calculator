using System;

public interface IDataBindingContext
{
    T CreateUniqueViewModel<T>(Func<T> createFunc) where T : ViewModelBase;
    bool TryGetViewModel<T>(string typeName, out T viewModel) where T : ViewModelBase;
}