namespace JsonTranscodingExample;

public interface ITodosRepository
{
    IEnumerable<(int id, string description)> GetTodos();
    string GetTodo(int id);
    void InsertTodo(string description);
    void UpdateTodo(int id, string description);
    void DeleteTodo(int id);
}

internal class TodosRepository : ITodosRepository
{
    private readonly Dictionary<int, string> todos = new Dictionary<int, string>();
    private int currentId = 1;

    public IEnumerable<(int id, string description)> GetTodos()
    {
        var results = new List<(int id, string description)>();

        foreach (var item in todos)
        {
            results.Add((item.Key, item.Value));
        }

        return results;
    }

    public string GetTodo(int id)
    {
        return todos[id];
    }

    public void InsertTodo(string description)
    {
        todos[currentId] = description;
        currentId++;
    }

    public void UpdateTodo(int id, string description)
    {
        todos[id] = description;
    }

    public void DeleteTodo(int id)
    {
        todos.Remove(id);
    }
}
