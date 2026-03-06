# Como Testar Autenticação na API

## Problema: Erro 401 Unauthorized

O erro 401 indica que a requisição não está autenticada ou o token é inválido.

## Solução: Passos para Testar Corretamente

### 1. Obter Token JWT

Primeiro, faça login para obter o token:

**Endpoint:** `POST http://localhost:5001/api/auth/login`

**Headers:**
```
Content-Type: application/json
X-Tenant-Id: localhost:5001
```

**Body:**
```json
{
  "email": "joao.silva@email.com",
  "senha": "123456"
}
```

**Resposta esperada:**
```json
{
  "dados": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "user": {
      "id": 1,
      "email": "joao.silva@email.com",
      "nome": "João Silva",
      "tipo": "motorista"
    }
  },
  "sucesso": true,
  "mensagem": "Login realizado com sucesso"
}
```

### 2. Usar o Token nas Requisições

Para acessar endpoints protegidos, inclua o token no header `Authorization`:

**Headers obrigatórios:**
```
Authorization: Bearer {seu_token_aqui}
X-Tenant-Id: localhost:5001
Content-Type: application/json
```

### 3. Exemplo: Buscar Histórico de Viagens

**Endpoint:** `GET http://localhost:5001/api/Viagem/motorista/1/historico?dataInicio=2025-09-29T00:53:27.952&dataFim=2025-12-27T00:53:27.952`

**Headers:**
```
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
X-Tenant-Id: localhost:5001
```

## Testando no Swagger

1. Acesse `http://localhost:5001/swagger`
2. Clique no botão **"Authorize"** (cadeado no topo)
3. Cole o token JWT no campo (sem a palavra "Bearer")
4. Clique em **"Authorize"** e depois **"Close"**
5. Agora todas as requisições no Swagger incluirão o token automaticamente

## Testando com Postman/Thunder Client

1. Crie uma requisição para o endpoint
2. Na aba **Headers**, adicione:
   - `Authorization`: `Bearer {seu_token}`
   - `X-Tenant-Id`: `localhost:5001`
3. Execute a requisição

## Possíveis Problemas

### Token Expirado
- Tokens expiram após o tempo configurado em `JwtSettings.ExpirationInMinutes` (padrão: 1440 minutos = 24 horas)
- Solução: Faça login novamente para obter um novo token

### Token sem Role Correta
- O token deve ter a role "passageiro" ou "motorista"
- Verifique o token em https://jwt.io para ver as claims

### Header X-Tenant-Id Faltando
- Se faltar, o middleware retorna 400 (Bad Request), não 401
- Sempre inclua o header `X-Tenant-Id` nas requisições

### Token Inválido
- Verifique se está usando o token completo (sem cortar)
- Verifique se o formato está correto: `Bearer {token}`

## Credenciais de Teste

Após executar o seed (`POST /api/seed/popular`), você pode usar:

**Motoristas:**
- Email: `joao.silva@email.com` | Senha: `123456`
- Email: `maria.santos@email.com` | Senha: `123456`
- Email: `pedro.oliveira@email.com` | Senha: `123456`

**Passageiros:**
- Email: `ana.silva@email.com` | Senha: `123456`
- Email: `bruno.santos@email.com` | Senha: `123456`
- Email: `carla.oliveira@email.com` | Senha: `123456`

