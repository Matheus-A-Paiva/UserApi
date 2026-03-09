# User API

API REST para gerenciamento de usuários com autenticação JWT, desenvolvida em C# (.NET) com arquitetura em camadas, contendo docker para inicialização.

## Como executar

Subir o banco:
 ```
docker compose up --build -d
```

Aplicar migrations:
```
dotnet ef database update
```

Swagger:
```
http://localhost:5050/swagger/index.html
```

## Fluxo inicial

1. Registrar um usuário

2. Acessar o banco PostgreSQL
```
docker exec -it userdb psql -U postgres -d userdb
```
3. Tornar o usuário Admin
```
UPDATE "Usuarios"
SET "Perfil" = 2
WHERE "Email" = 'seu_email';
```
4. Fazer login e usar o token JWT nos endpoints protegidos.