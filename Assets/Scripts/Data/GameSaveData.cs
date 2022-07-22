using System;
using System.Collections.Generic;

[Serializable]
public class GameSaveData
{
    public ExpressionModelData ExpressionModelData { get; set; }
}

[Serializable]
public class ExpressionModelData
{
    public ExpressionState State { get; set; }
    public IList<ExpressionNode> Nodes { get; set; }
}