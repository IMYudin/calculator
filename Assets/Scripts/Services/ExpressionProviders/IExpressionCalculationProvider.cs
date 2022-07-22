public interface IExpressionCalculationProvider
{
    bool TryCalculate(
        ExpressionValueNode arg1, 
        ExpressionValueNode arg2,
        ExpressionActionNode action,
        out ExpressionValueNode result);
}