using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Todo;

namespace JsonTranscodingExample.Services;

public class TodoService : Todo.Todo.TodoBase
{
    private readonly ITodosRepository repository;

    public TodoService(ITodosRepository repository)
    {
        this.repository = repository;
    }

    public override Task<GetTodosReply> GetAll(Empty request, ServerCallContext context)
    {
        var result = new GetTodosReply();

        result.Todos.AddRange(repository.GetTodos()
            .Select(i => new GetTodoReply
            {
                Id = i.id,
                Description = i.description
            }));

        return Task.FromResult(result);
    }

    public override Task<GetTodoReply> Get(GetTodoRequest request, ServerCallContext context)
    {
        var todoDescription = repository.GetTodo(request.Id);

        return Task.FromResult(new GetTodoReply
        {
            Id = request.Id,
            Description = todoDescription
        });
    }

    public override Task<Empty> Post(PostTodoRequest request, ServerCallContext context)
    {
        repository.InsertTodo(request.Description);

        return Task.FromResult(new Empty());
    }

    public override Task<Empty> Put(PutTodoRequest request, ServerCallContext context)
    {
        repository.UpdateTodo(request.Id, request.Description);

        return Task.FromResult(new Empty());
    }

    public override Task<Empty> Delete(DeleteTodoRequest request, ServerCallContext context)
    {
        repository.DeleteTodo(request.Id);

        return Task.FromResult(new Empty());
    }
}