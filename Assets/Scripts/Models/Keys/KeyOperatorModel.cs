
public class KeyOperatorModel : KeyModel
{
    public OperatorType OperatorType { get; }
     
    public KeyOperatorModel(KeyActionType actionType, OperatorType operatorType, string descriptor,  int size) : base(actionType, descriptor, size)
    {
        OperatorType = operatorType;
    }
}