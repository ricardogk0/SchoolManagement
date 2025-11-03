# SchoolManagement API

API para gestão escolar, construída em .NET 8 com Entity Framework Core, versionamento de API, autenticação JWT e documentação via Swagger. A infraestrutura local é orquestrada com Docker Compose usando SQL Server e RabbitMQ.

## Sumário
- Visão Geral
- Tecnologias
- Requisitos
- Subir infraestrutura com Docker Compose
- Configurar conexão com o banco (SQL Server container)
- Executar a API (Dev)
- Documentação e Swagger
- Autenticação (JWT) e exemplo de login
- Principais endpoints
- Dicas e troubleshooting

## Visão Geral
- A API aplica migrations automaticamente ao iniciar.
- Toda chamada (fora do `auth/login`) requer autenticação e papel `Admin`.
- Documentação interativa via Swagger, com versionamento (`v1`).

## Tecnologias
- `.NET 8`, `ASP.NET Core`
- `Entity Framework Core` (SQL Server)
- `Swagger` (Swashbuckle) e `API Versioning`
- `JWT` para autenticação e autorização
- `Docker Compose` com `SQL Server` e `RabbitMQ`

## Requisitos
- Docker Desktop instalado e rodando
- .NET SDK 8 instalado
- PowerShell ou terminal equivalente

## Subir infraestrutura com Docker Compose
No diretório raiz do projeto (`d:\Projetos\SchoolManagement`):

```bash
docker-compose up -d
```

Serviços provisionados:
- `sqlserver` (porta `1433`):
  - Usuário: `sa`
  - Senha: `YourStrong!Passw0rd` (definida no compose; altere em produção)
- `rabbitmq` (portas `5673:5672` para AMQP e `15673:15672` para UI):
  - UI de gerenciamento: `http://localhost:15673`
  - Usuário/Senha: `user` / `user`

Verifique saúde dos containers:
```bash
docker ps
docker logs sqlserver --tail 50
docker logs rabbitmq --tail 50
```

## Configurar conexão com o banco (SQL Server container)
Por padrão, o `appsettings.json` usa `LocalDB`. Para usar o SQL Server do Docker, altere a connection string em `src/SchoolManagement.API/appsettings.Development.json` (ou `appsettings.json`) para algo como:

```json
{
  "DatabaseSettings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=SchoolManagementDB;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

Notas:
- `TrustServerCertificate=True` evita erros de certificado em ambiente local.
- A aplicação executa `db.Database.Migrate()` no startup, criando/atualizando o schema automaticamente.

## Executar a API (Dev)
Do diretório raiz ou diretamente no projeto da API:

```bash
# Restaurar e compilar
dotnet restore
dotnet build

# Executar a API
dotnet run --project src/SchoolManagement.API/SchoolManagement.API.csproj
```

URLs padrão (conforme `launchSettings.json`):
- `http://localhost:5232`
- `https://localhost:7223`

Ambiente:
- A API utiliza `ASPNETCORE_ENVIRONMENT=Development` por padrão nos perfis de execução.

## Documentação e Swagger
- Acesse `https://localhost:7223/swagger` ou `http://localhost:5232/swagger`.
- A UI lista os grupos por versão (`v1`).
- A segurança está configurada com `Bearer` no Swagger; após obter o token, clique em `Authorize` e informe: `Bearer {seu_token}`.

## Autenticação (JWT) e exemplo de login
Endpoint de login (não requer token):
- `POST /api/v1/auth/login`

Credenciais padrão (em `appsettings.Development.json`):
- Email: `admin@school.local`
- Senha: `admin123`

Exemplo (cURL):
```bash
curl -X POST "https://localhost:7223/api/v1/auth/login" \
  -H "Content-Type: application/json" \
  -d '{"email":"admin@school.local","password":"admin123"}'
```

Resposta:
```json
{
  "token": "<jwt>",
  "expiresAt": "2025-01-01T12:34:56Z"
}
```

Uso do token nas requisições subsequentes:
```
Authorization: Bearer <jwt>
```

## Principais endpoints (v1)

Estudantes (`/api/v1/students`):
- `GET /api/v1/students` — lista paginada e filtrável (`Filters`).
- `GET /api/v1/students/{id}` — detalhe.
- `POST /api/v1/students` — cria estudante.
- `PUT /api/v1/students/{id}` — atualiza estudante.
- `PATCH /api/v1/students/{id}/soft-delete` — desativa (soft delete).

Turmas (`/api/v1/classes`):
- `GET /api/v1/classes` — lista paginada e filtrável.
- `GET /api/v1/classes/{id}` — detalhe.
- `POST /api/v1/classes` — cria turma.
- `PUT /api/v1/classes/{id}` — atualiza turma.
- `PATCH /api/v1/classes/{id}/soft-delete` — desativa (soft delete).

Matrículas (`/api/v1/registrations`):
- `POST /api/v1/registrations` — matricula estudante em uma turma.
- `GET /api/v1/registrations/class/{classId}/students` — lista estudantes de uma turma.

Observações:
- Os controllers estão versionados (`V1`).
- Os endpoints estão anotados no Swagger com resumo e descrição.

## Dicas e troubleshooting
- Erro de conexão com SQL Server:
  - Verifique se o container `sqlserver` está saudável: `docker ps`.
  - Confirme host/porta e credenciais (`sa` / `YourStrong!Passw0rd`).
  - Use `TrustServerCertificate=True` na connection string local.
- Migrations não aplicadas:
  - Veja logs de startup da API; há uma mensagem de sucesso/erro de migração.
  - Confirme se o banco está acessível e a connection string aponta para o `localhost,1433` do container.
- RabbitMQ:
  - Provisionado pelo compose; pode ser utilizado para mensageria futura. UI em `http://localhost:15673` (`user/user`). Não é obrigatório para os endpoints básicos acima.

---
Se quiser ajustar credenciais de admin ou o segredo do JWT, edite as seções `AdminCredentials` e `Jwt` em `src/SchoolManagement.API/appsettings.Development.json` (ou variáveis de ambiente em produção).