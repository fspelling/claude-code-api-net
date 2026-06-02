## **Papel**
Você é um desenvolvedor .NET Senior especialista em:
- Arquitetura Vertical Slice
- Minimal APIs
- CQRS
- MediatR
- Clean Code

## **Entrada**

### **Regra de negocio**

Validar apenas os campos obrigatorios passado pelo usuario.

Todas as validações devem ser implementadas utilizando FluentValidation.<br>
O Endpoint deve apenas encaminhar a requisição para o MediatR.

### **Modelagem da Entidade (Produto)**

Dados da entidade Produto:
- Id: string (campo obrigatorio)
- Nome: string (campo obrigatorio)
- Preco: decimal

### **Arquitetura - Padrao**
Sera utilziado o padrao de arquitura `Architecture Vertical Slice (VSA)`!!<br>
Vale destacar que nessa arquitetura as classes das camadas de cada feature, deveram ser criadas tudo em um arquivo de cada feature!!

### **Arquitetura - Estrutura**
Segue a estrutura de diretorios e arquivo que devem seguir em nosso projeto:

```text
src/
├── Config/
├── DataBase/
├── Entities/
├── Features/
│   └── Produto/
│       ├── CriarProduto.cs
│       ├── ObterProdutoPorId.cs
│       ├── ObterTodosProdutos.cs
│       ├── AtualizarProduto.cs
│       └── ExcluirProduto.cs
├── Shared/
├── appsettings.json
└── Program.cs
```

### **Stack de tecnologias**

Segue as tecnologias pincipais que sera utilziado nesse projeto:

- .NET ApiMinimal .NET 8.0.
- FluentValidation.
- Mapster.
- MediatR.
- Scalar.
- Carter

## **Etapas**

1. Criar esqueleto do projeto, utilziando a arquitetura VSA.
2. Criar configuracoes do DataBaseContext (`Utilizar SQLite local`).
3. Criar camadas da feature:
    - Endpoint: Mapeamento da rota referente ao controller (utilizando o modulo ICarterModule).
    - Command ou Query: Entidade que representa a funcionalidade da feature, podendo ser um command ou query (seguindo o modelo de CQRS).
    - Handler: Mediacao do caso de uso, onde sera feito todo o gerenciamento do caso de uso da feature.
    - Repository: Responsvael por realizar a iteracao com o banco de dados, podendo persistir ou obter alguma informacao.
        - Cada feature deve possuir seu próprio Repository interno.
        - Não utilizar DbContext diretamente nos Handlers.

## **Expectativa**
Criar um projeto ApiMinimal .NET 8.0, para gerenciar um CRUD basico de uma entidade chamada de Produtos, e gere o código completo dentro do diretório `src/`.