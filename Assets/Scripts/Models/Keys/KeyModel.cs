using System;

[Flags]
public enum KeyActionType : byte
{
    None = 0,
    Operand = 1 << 0,
    Operator = 1 << 1,
    Confirm = 1 << 2,
    Result = 1 << 3,
    Clear = 1 << 4,
    
    All = Operand | Operator | Confirm | Result | Clear
}

public class KeyModel : ModelBase
{
    public event Action<KeyModel, bool> OnKeyStateChanged;

    private bool _isInteractable;
    
    private Guid Id { get; }
    public KeyActionType ActionType { get; }
    public string Descriptor { get; }
    public int Size { get; }

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

    public KeyModel(KeyActionType actionType, string descriptor,  int size)
    {
        Id = Guid.NewGuid();
        ActionType = actionType;
        Descriptor = descriptor;
        Size = size;
    }

    public void UpdateState(KeyActionType actionType)
    {
        IsInteractable = (ActionType & actionType) != 0;
    }
    
    public override bool Equals(object obj)
    {
        return obj is KeyModel keyModel &&
               Id == keyModel.Id &&
               Descriptor == keyModel.Descriptor;
    }
    
    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Descriptor);
    }
}