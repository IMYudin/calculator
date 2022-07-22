using Newtonsoft.Json;

public class ExpressionValueNode : ExpressionNode
{ 
    [JsonProperty]
    public uint Value { get; private set; }
    
    public void Modify(uint value)
    {
        IsModified = true;
        Value = Value * 10 + value;
    }
}