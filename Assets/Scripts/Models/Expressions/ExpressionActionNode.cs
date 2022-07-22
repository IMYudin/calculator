using Newtonsoft.Json;

public class ExpressionActionNode : ExpressionNode
{
    [JsonProperty]
    public OperatorType Value { get; private set; }
    
    public void Modify(OperatorType value)
    {
        IsModified = true;
        Value = value;
    }
}