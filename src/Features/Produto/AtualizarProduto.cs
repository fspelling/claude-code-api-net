namespace ProdutosApi.Features.Produtos;

// ============================================================
// Request / Response
// ============================================================

public record AtualizarProdutoRequest(string Nome, decimal Preco);

public record AtualizarProdutoResponse(string Id, string Nome, decimal Preco);

// ============================================================
// Command
// ============================================================

public record AtualizarProdutoCommand(string Id, string Nome, decimal Preco)
    : IRequest<AtualizarProdutoResponse?>;

// ============================================================
// Validator
// ============================================================

public class AtualizarProdutoValidator : AbstractValidator<AtualizarProdutoCommand>
{
    public AtualizarProdutoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id é obrigatório.");

        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("Nome é obrigatório.");
    }
}

// ============================================================
// Repository
// ============================================================

public interface IAtualizarProdutoRepository
{
    Task<Produto?> ObterPorIdAsync(string id, CancellationToken cancellationToken);
    Task<Produto> AtualizarAsync(Produto produto, CancellationToken cancellationToken);
}

public class AtualizarProdutoRepository(AppDbContext db) : IAtualizarProdutoRepository
{
    public async Task<Produto?> ObterPorIdAsync(string id, CancellationToken cancellationToken)
        => await db.Produtos.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

    public async Task<Produto> AtualizarAsync(Produto produto, CancellationToken cancellationToken)
    {
        db.Produtos.Update(produto);
        await db.SaveChangesAsync(cancellationToken);
        return produto;
    }
}

// ============================================================
// Handler
// ============================================================

public class AtualizarProdutoHandler(IAtualizarProdutoRepository repository)
    : IRequestHandler<AtualizarProdutoCommand, AtualizarProdutoResponse?>
{
    public async Task<AtualizarProdutoResponse?> Handle(
        AtualizarProdutoCommand request,
        CancellationToken cancellationToken)
    {
        var produto = await repository.ObterPorIdAsync(request.Id, cancellationToken);
        if (produto is null) return null;

        produto.Nome = request.Nome;
        produto.Preco = request.Preco;

        var atualizado = await repository.AtualizarAsync(produto, cancellationToken);
        return atualizado.Adapt<AtualizarProdutoResponse>();
    }
}

// ============================================================
// Endpoint
// ============================================================

public class AtualizarProdutoEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/produtos/{id}", async (
            string id,
            [FromBody] AtualizarProdutoRequest request,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var command = new AtualizarProdutoCommand(id, request.Nome, request.Preco);
            var response = await sender.Send(command, cancellationToken);
            return response is null ? Results.NotFound() : Results.Ok(response);
        })
        .WithName("AtualizarProduto")
        .WithTags("Produtos")
        .Produces<AtualizarProdutoResponse>()
        .Produces(StatusCodes.Status404NotFound)
        .ProducesValidationProblem();
    }
}
