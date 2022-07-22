using System;
using Newtonsoft.Json;

[Serializable]
public abstract class ExpressionNode
{
    [JsonProperty]
    public bool IsModified { get; protected set; }
}