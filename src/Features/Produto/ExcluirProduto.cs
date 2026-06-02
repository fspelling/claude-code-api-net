namespace ProdutosApi.Features.Produtos;

// ============================================================
// Command
// ============================================================

public record ExcluirProdutoCommand(string Id) : IRequest<bool>;

// ============================================================
// Validator
// ============================================================

public class ExcluirProdutoValidator : AbstractValidator<ExcluirProdutoCommand>
{
    public ExcluirProdutoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id é obrigatório.");
    }
}

// ============================================================
// Repository
// ============================================================

public interface IExcluirProdutoRepository
{
    Task<Produto?> ObterPorIdAsync(string id, CancellationToken cancellationToken);
    Task ExcluirAsync(Produto produto, CancellationToken cancellationToken);
}

public class ExcluirProdutoRepository(AppDbContext db) : IExcluirProdutoRepository
{
    public async Task<Produto?> ObterPorIdAsync(string id, CancellationToken cancellationToken)
        => await db.Produtos.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

    public async Task ExcluirAsync(Produto produto, CancellationToken cancellationToken)
    {
        db.Produtos.Remove(produto);
        await db.SaveChangesAsync(cancellationToken);
    }
}

// ============================================================
// Handler
// ============================================================

public class ExcluirProdutoHandler(IExcluirProdutoRepository repository)
    : IRequestHandler<ExcluirProdutoCommand, bool>
{
    public async Task<bool> Handle(
        ExcluirProdutoCommand request,
        CancellationToken cancellationToken)
    {
        var produto = await repository.ObterPorIdAsync(request.Id, cancellationToken);
        if (produto is null) return false;

        await repository.ExcluirAsync(produto, cancellationToken);
        return true;
    }
}

// ============================================================
// Endpoint
// ============================================================

public class ExcluirProdutoEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/produtos/{id}", async (
            string id,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var command = new ExcluirProdutoCommand(id);
            var excluido = await sender.Send(command, cancellationToken);
            return excluido ? Results.NoContent() : Results.NotFound();
        })
        .WithName("ExcluirProduto")
        .WithTags("Produtos")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound)
        .ProducesValidationProblem();
    }
}
