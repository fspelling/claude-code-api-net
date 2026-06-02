namespace ProdutosApi.Features.Produtos;

// ============================================================
// Response
// ============================================================

public record ObterProdutoPorIdResponse(string Id, string Nome, decimal Preco);

// ============================================================
// Query
// ============================================================

public record ObterProdutoPorIdQuery(string Id) : IRequest<ObterProdutoPorIdResponse?>;

// ============================================================
// Validator
// ============================================================

public class ObterProdutoPorIdValidator : AbstractValidator<ObterProdutoPorIdQuery>
{
    public ObterProdutoPorIdValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id é obrigatório.");
    }
}

// ============================================================
// Repository
// ============================================================

public interface IObterProdutoPorIdRepository
{
    Task<Produto?> ObterPorIdAsync(string id, CancellationToken cancellationToken);
}

public class ObterProdutoPorIdRepository(AppDbContext db) : IObterProdutoPorIdRepository
{
    public async Task<Produto?> ObterPorIdAsync(string id, CancellationToken cancellationToken)
        => await db.Produtos.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
}

// ============================================================
// Handler
// ============================================================

public class ObterProdutoPorIdHandler(IObterProdutoPorIdRepository repository)
    : IRequestHandler<ObterProdutoPorIdQuery, ObterProdutoPorIdResponse?>
{
    public async Task<ObterProdutoPorIdResponse?> Handle(
        ObterProdutoPorIdQuery request,
        CancellationToken cancellationToken)
    {
        var produto = await repository.ObterPorIdAsync(request.Id, cancellationToken);
        return produto?.Adapt<ObterProdutoPorIdResponse>();
    }
}

// ============================================================
// Endpoint
// ============================================================

public class ObterProdutoPorIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/produtos/{id}", async (
            string id,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var query = new ObterProdutoPorIdQuery(id);
            var response = await sender.Send(query, cancellationToken);
            return response is null ? Results.NotFound() : Results.Ok(response);
        })
        .WithName("ObterProdutoPorId")
        .WithTags("Produtos")
        .Produces<ObterProdutoPorIdResponse>()
        .Produces(StatusCodes.Status404NotFound)
        .ProducesValidationProblem();
    }
}
