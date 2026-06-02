namespace ProdutosApi.Config;

using ProdutosApi.Features.Produtos;
using ProdutosApi.Shared;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(typeof(Program).Assembly);
        services.AddCarter();

        services.AddScoped<ICriarProdutoRepository, CriarProdutoRepository>();
        services.AddScoped<IObterProdutoPorIdRepository, ObterProdutoPorIdRepository>();
        services.AddScoped<IObterTodosProdutosRepository, ObterTodosProdutosRepository>();
        services.AddScoped<IAtualizarProdutoRepository, AtualizarProdutoRepository>();
        services.AddScoped<IExcluirProdutoRepository, ExcluirProdutoRepository>();

        return services;
    }
}
