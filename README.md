# Catalogo - CRUD de Produtos
## Migrations
A abordagem escolhida para este projeto foi code first com banco de dados Postgresql. Para executar as migrações, é necessário configurar a connection string e executar no terminal:
```
dotnet ef database update --context=CatalogoDbContext --startup-project=src/Catalogo.WebApp.API/Catalogo.WebApp.API.csproj --project=src/Catalogo.Data/Catalogo.Data.csproj```
```

A estrutura do projeto é a seguinte:
- Core
  - Este projeto contém tudo que será compartilhado entre todos outros projetos.
- Data
  - Este projeto possui todas as classes responsáveis por tratar da persistência dos dados da aplicação.
- Domain
  - O Domain é o projeto que contém os Models e interfaces responsáveis por tratar a regra de negócio da aplicação.
- WebApp.API
  - Este projeto contém a API e os serviços de aplicação que serão responsáveis por integarir com o usuário.

Para testar é possível utilizar o Swagger: https://localhost:5001/swagger