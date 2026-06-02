namespace ProdutosApi.Features.Produtos;

// ============================================================
// Request / Response
// ============================================================

public record CriarProdutoRequest(string Nome, decimal Preco);

public record CriarProdutoResponse(string Id, string Nome, decimal Preco);

// ============================================================
// Command
// ============================================================

public record CriarProdutoCommand(string Nome, decimal Preco) : IRequest<CriarProdutoResponse>;

// ============================================================
// Validator
// ============================================================

public class CriarProdutoValidator : AbstractValidator<CriarProdutoCommand>
{
    public CriarProdutoValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("Nome é obrigatório.");
    }
}

// ============================================================
// Repository
// ============================================================

public interface ICriarProdutoRepository
{
    Task<Produto> CriarAsync(Produto produto, CancellationToken cancellationToken);
}

public class CriarProdutoRepository(AppDbContext db) : ICriarProdutoRepository
{
    public async Task<Produto> CriarAsync(Produto produto, CancellationToken cancellationToken)
    {
        db.Produtos.Add(produto);
        await db.SaveChangesAsync(cancellationToken);
        return produto;
    }
}

// ============================================================
// Handler
// ============================================================

public class CriarProdutoHandler(ICriarProdutoRepository repository)
    : IRequestHandler<CriarProdutoCommand, CriarProdutoResponse>
{
    public async Task<CriarProdutoResponse> Handle(
        CriarProdutoCommand request,
        CancellationToken cancellationToken)
    {
        var produto = new Produto
        {
            Id = Guid.NewGuid().ToString(),
            Nome = request.Nome,
            Preco = request.Preco
        };

        var criado = await repository.CriarAsync(produto, cancellationToken);
        return criado.Adapt<CriarProdutoResponse>();
    }
}

// ============================================================
// Endpoint
// ============================================================

public class CriarProdutoEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/produtos", async (
            [FromBody] CriarProdutoRequest request,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var command = request.Adapt<CriarProdutoCommand>();
            var response = await sender.Send(command, cancellationToken);
            return Results.Created($"/api/produtos/{response.Id}", response);
        })
        .WithName("CriarProduto")
        .WithTags("Produtos")
        .Produces<CriarProdutoResponse>(StatusCodes.Status201Created)
        .ProducesValidationProblem();
    }
}
