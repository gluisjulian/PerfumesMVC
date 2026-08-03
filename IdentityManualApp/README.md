# IdentityManualApp

Projeto ASP.NET Core MVC (.NET 10) com autenticação implementada **manualmente**
usando `UserManager<ApplicationUser>` e `SignInManager<ApplicationUser>` do
ASP.NET Core Identity — sem usar as Razor Pages/Areas padrão do Identity
(`Areas/Identity/Pages`) e sem scaffolding automático.

## Estrutura

- **Models**: `Fornecedor`, `Produto`, `ApplicationUser` (estende `IdentityUser`)
- **Data/ApplicationDbContext.cs**: herda de `IdentityDbContext<ApplicationUser>`
- **Controllers/AccountController.cs**: `Register`, `Login`, `Logout`, `AccessDenied` — todos implementados manualmente
- **Controllers/FornecedorController.cs**: CRUD público (não exige login)
- **Controllers/ProdutoController.cs**: CRUD com `[Authorize]` na classe — **exige usuário autenticado**
- **Views/Account**: telas de Login e Registro próprias (não são as páginas padrão do Identity)

## Como o requisito "autenticação manual" foi atendido

- Não há pasta `Areas/Identity` nem `services.AddDefaultIdentity()` / `AddRazorPages()`.
- O `Program.cs` usa `AddIdentity<ApplicationUser, IdentityRole>()` apenas para
  registrar os serviços (UserManager, SignInManager, stores no EF Core), mas
  todo o fluxo de tela e lógica de registro/login/logout foi escrito à mão em
  `AccountController` e nas Views correspondentes.
- `ProdutoController` usa `[Authorize]`, e o cookie de autenticação está
  configurado para redirecionar para `/Account/Login` (nossa própria rota).

## Pré-requisitos

- .NET SDK 10.0.10 (ou compatível) instalado
- SQL Server LocalDB (incluso no Visual Studio) ou ajuste a connection string
  em `appsettings.json` para outro SQL Server

## Como rodar

```bash
# 1. Restaurar pacotes
dotnet restore

# 2. Criar a migration inicial (cria tabelas do Identity + Fornecedor + Produto)
dotnet ef migrations add InitialCreate

# 3. Aplicar a migration no banco (LocalDB por padrão)
dotnet ef database update

# 4. Rodar a aplicação
dotnet run
```

Se o comando `dotnet ef` não for encontrado, instale a ferramenta global:

```bash
dotnet tool install --global dotnet-ef
```

## Fluxo de uso

1. Acesse `/Account/Register` para criar uma conta.
2. Após o cadastro, você já é logado automaticamente.
3. Cadastre fornecedores em `/Fornecedor` (não exige login).
4. Tente acessar `/Produto` sem estar logado — você será redirecionado para `/Account/Login`.
5. Logado, você pode criar produtos vinculados a um fornecedor.

## Observações

- As regras de senha (tamanho mínimo, exigência de dígito, etc.) estão
  configuradas em `Program.cs`, dentro de `AddIdentity(options => ...)`.
- `Produto.UsuarioId` guarda o Id do usuário logado que cadastrou o produto,
  útil caso queira evoluir para produtos "por usuário" no futuro.
