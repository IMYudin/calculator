using System;
using System.Collections.Generic;

public enum ExpressionState
{
    Default = 0,
    OperandInput = 1,
    OperatorNoneInput = 2,
    OperatorInput = 3,
    ResultReady = 4,
    SuccessfulResult = 5,
    ExceptionReadyResult = 6,
    ExceptionResult = 7,
}

public class ExpressionModel : ModelBase
{
    public event Action<ExpressionModel> OnExpressionChanged; 

    private readonly IExpressionProvider _expressionProvider;
    private readonly IExpressionCalculationProvider _expressionCalculationProvider;
    
    private ExpressionState _state;

    public ExpressionState State { get; private set; }
    public IList<ExpressionNode> Nodes { get; private set; }

    private ExpressionNode LastNode => Nodes?[^1];

    public ExpressionModel(IExpressionProvider expressionProvider, IExpressionCalculationProvider expressionCalculationProvider)
    {
        _expressionProvider = expressionProvider;
        _expressionCalculationProvider = expressionCalculationProvider;
        
        InitializeNodes();
    }

    public void TryModifyOperandNode(uint value)
    {
        if (LastNode is not ExpressionValueNode operandNode) return;
        
        operandNode.Modify(value);
        
        if (State == ExpressionState.Default && operandNode.IsModified)
        {
            State = ExpressionState.OperandInput;
        }
        
        SaveModel();
        
        OnExpressionChanged?.Invoke(this);
    }
    
    public void TryModifyOperatorNode(OperatorType value)
    {
        if (LastNode is not ExpressionActionNode operatorNode) return;
        
        operatorNode.Modify(value);

        if (State == ExpressionState.OperatorNoneInput && operatorNode.IsModified)
        {
            State = ExpressionState.OperatorInput;
        }
        
        SaveModel();
        
        OnExpressionChanged?.Invoke(this);
    }

    public void Confirm()
    {
        switch (State)
        {
            case ExpressionState.OperandInput:
                if (Nodes.Count == 1)
                {
                    AddExpressionNode(new ExpressionActionNode());
                    SetState(ExpressionState.OperatorNoneInput);
                }
                else
                {
                    SetState(ExpressionState.ResultReady);
                }
                break;
            
            case ExpressionState.OperatorInput:
                AddExpressionNode(new ExpressionValueNode());
                SetState(ExpressionState.OperandInput);
                break;
        }
    }

    public void CalculateResult()
    {
        if (!_expressionCalculationProvider.TryCalculate(
                (ExpressionValueNode)Nodes[0], 
                (ExpressionValueNode)Nodes[2], 
                (ExpressionActionNode)Nodes[1], 
                out ExpressionValueNode result))
        {
            SetState(ExpressionState.ExceptionReadyResult);
        }
        else
        {
            var equalNode = new ExpressionActionNode();
            equalNode.Modify(OperatorType.Eq);
            
            Nodes.Add(equalNode);
            Nodes.Add(result);
            
            SetState(ExpressionState.SuccessfulResult);
        }
    }

    public void ApplyExceptionResult()
    {
        ExpressionNotificationNode notificationNode = new ();
        notificationNode.Modify(((ExpressionValueNode)Nodes[0]).Value == 0 ? "¯\\_(ツ)_/¯" : "∞");

        Nodes.Clear();
        Nodes.Add(notificationNode);
        
        SetState(ExpressionState.ExceptionResult);
    }

    public void Clear()
    {
        Nodes.Clear();
        InitializeNodes();
        
        SetState(ExpressionState.Default);
    }

    public void Load()
    {
        SyncModel();
        OnExpressionChanged?.Invoke(this);
    }

    public void SetState(ExpressionState state)
    {
        if (State == state) return;
        
        State = state;
        SaveModel();
        
        OnExpressionChanged?.Invoke(this);
    }
    
    private void InitializeNodes()
    {
        Nodes ??= new List<ExpressionNode>();
        Nodes.Add(new ExpressionValueNode());
    }

    private void SyncModel()
    {
        var model = _expressionProvider.LoadExpressionModel();
        
        if (model == null) return;
        
        State = model.State;

        if (model.Nodes != null)
        {
            Nodes = new List<ExpressionNode>(model.Nodes);
        }
    }

    private void SaveModel()
    {
        _expressionProvider.SaveExpressionModel(this);
    }

    private void AddExpressionNode(ExpressionNode expressionNode)
    {
        Nodes.Add(expressionNode);
        SaveModel();
        
        OnExpressionChanged?.Invoke(this);
    }
}