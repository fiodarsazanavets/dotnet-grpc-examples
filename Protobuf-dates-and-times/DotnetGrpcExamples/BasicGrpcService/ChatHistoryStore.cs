namespace BasicGrpcService;

public interface IChatHistoryStore
{
    void AddEntry(string request, string response);
    Dictionary<int, (string, string)> GetHistory();
}

public class ChatHistoryStore : IChatHistoryStore
{
    private readonly Dictionary<int, (string, string)> _history;

    public ChatHistoryStore()
    {
        _history = new();
    }

    public void AddEntry(string request, string response)
    {
        var key = _history.Keys.Any() ?
            _history.Keys.Max() + 1 :
            1;

        _history[key] = (request, response);
    }

    public Dictionary<int, (string, string)> GetHistory()
    {
        return _history;
    }
}
