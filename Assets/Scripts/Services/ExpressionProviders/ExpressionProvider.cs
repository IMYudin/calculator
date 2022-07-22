public class ExpressionProvider : IExpressionProvider
{
    private readonly IDataProvider<GameSaveData> _dataProvider;
    
    public ExpressionProvider(IDataProvider<GameSaveData> dataProvider)
    {
        _dataProvider = dataProvider;
    }

    public ExpressionModelData LoadExpressionModel()
    {
        return _dataProvider.Data.ExpressionModelData;
    }

    public void SaveExpressionModel(ExpressionModel expressionModel)
    {
        _dataProvider.Data.ExpressionModelData ??= new ExpressionModelData();
        _dataProvider.Data.ExpressionModelData.State = expressionModel.State;
        _dataProvider.Data.ExpressionModelData.Nodes = expressionModel.Nodes;
        
        _dataProvider.Save();
    }
}