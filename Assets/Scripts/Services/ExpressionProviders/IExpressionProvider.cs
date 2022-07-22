public interface IExpressionProvider
{
    ExpressionModelData LoadExpressionModel();
    void SaveExpressionModel(ExpressionModel model);
}