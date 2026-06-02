namespace ProdutosApi.Features.Produtos;

// ============================================================
// Response
// ============================================================

public record ObterTodosProdutosResponse(string Id, string Nome, decimal Preco);

// ============================================================
// Query
// ============================================================

public record ObterTodosProdutosQuery : IRequest<IEnumerable<ObterTodosProdutosResponse>>;

// ============================================================
// Repository
// ============================================================

public interface IObterTodosProdutosRepository
{
    Task<IEnumerable<Produto>> ObterTodosAsync(CancellationToken cancellationToken);
}

public class ObterTodosProdutosRepository(AppDbContext db) : IObterTodosProdutosRepository
{
    public async Task<IEnumerable<Produto>> ObterTodosAsync(CancellationToken cancellationToken)
        => await db.Produtos.AsNoTracking().ToListAsync(cancellationToken);
}

// ============================================================
// Handler
// ============================================================

public class ObterTodosProdutosHandler(IObterTodosProdutosRepository repository)
    : IRequestHandler<ObterTodosProdutosQuery, IEnumerable<ObterTodosProdutosResponse>>
{
    public async Task<IEnumerable<ObterTodosProdutosResponse>> Handle(
        ObterTodosProdutosQuery request,
        CancellationToken cancellationToken)
    {
        var produtos = await repository.ObterTodosAsync(cancellationToken);
        return produtos.Adapt<IEnumerable<ObterTodosProdutosResponse>>();
    }
}

// ============================================================
// Endpoint
// ============================================================

public class ObterTodosProdutosEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/produtos", async (
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var query = new ObterTodosProdutosQuery();
            var response = await sender.Send(query, cancellationToken);
            return Results.Ok(response);
        })
        .WithName("ObterTodosProdutos")
        .WithTags("Produtos")
        .Produces<IEnumerable<ObterTodosProdutosResponse>>();
    }
}
