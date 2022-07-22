using Newtonsoft.Json;

public class ExpressionNotificationNode : ExpressionNode
{
    [JsonProperty]
    public string Notification { get; private set; }
    
    public void Modify(string notification)
    {
        IsModified = true;
        Notification = notification;
    }
}