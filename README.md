# Produtos API

API RESTful para gerenciamento de produtos, construída com .NET 9, seguindo a arquitetura **Vertical Slice (VSA)** com **Minimal APIs**, **CQRS** e **MediatR**.

## Stack

| Tecnologia | Versão | Função |
|---|---|---|
| .NET | 9.0 | Runtime / SDK |
| Carter | 8.2.1 | Roteamento de endpoints |
| MediatR | 12.3.0 | Mediação CQRS |
| FluentValidation | 11.9.2 | Validação de entrada |
| Mapster | 7.4.0 | Mapeamento de objetos |
| EF Core SQLite | 9.0.5 | Persistência |
| Scalar | 2.3.5 | Documentação interativa |

## Estrutura do projeto

```
src/
├── Config/
│   └── DependencyInjection.cs      # Registro de serviços
├── DataBase/
│   └── AppDbContext.cs              # Contexto EF Core (SQLite)
├── Entities/
│   └── Produto.cs                   # Entidade de domínio
├── Features/
│   └── Produto/
│       ├── CriarProduto.cs          # POST /api/produtos
│       ├── ObterProdutoPorId.cs     # GET  /api/produtos/{id}
│       ├── ObterTodosProdutos.cs    # GET  /api/produtos
│       ├── AtualizarProduto.cs      # PUT  /api/produtos/{id}
│       └── ExcluirProduto.cs        # DELETE /api/produtos/{id}
├── Shared/
│   ├── ValidationBehavior.cs        # Pipeline behavior MediatR
│   └── ValidationExceptionHandler.cs
├── appsettings.json
└── Program.cs
```

Cada feature concentra em um único arquivo: `Request/Response`, `Command/Query`, `Validator`, `Repository` e `Endpoint`.

## Pré-requisitos

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)

## Como executar

```bash
# Restaurar dependências
dotnet restore src/

# Executar a aplicação
dotnet run --project src/
```

O banco SQLite (`produtos.db`) é criado automaticamente na primeira execução.

A documentação interativa estará disponível em:

```
http://localhost:<porta>/scalar/v1
```

## Endpoints

| Método | Rota | Descrição |
|---|---|---|
| `POST` | `/api/produtos` | Cria um produto |
| `GET` | `/api/produtos` | Lista todos os produtos |
| `GET` | `/api/produtos/{id}` | Busca produto por ID |
| `PUT` | `/api/produtos/{id}` | Atualiza um produto |
| `DELETE` | `/api/produtos/{id}` | Remove um produto |

### Corpo da requisição — criar / atualizar

```json
{
  "nome": "Notebook",
  "preco": 4999.90
}
```

### Entidade Produto

| Campo | Tipo | Obrigatório |
|---|---|---|
| `id` | `string` (GUID) | Gerado automaticamente |
| `nome` | `string` | Sim |
| `preco` | `decimal` | Não |

## Padrão de arquitetura

Cada feature segue o fluxo:

```
Endpoint (Carter)
    └── Command / Query (MediatR)
            └── Validator (FluentValidation) ← pipeline behavior
            └── Handler
                    └── Repository (EF Core / SQLite)
```
