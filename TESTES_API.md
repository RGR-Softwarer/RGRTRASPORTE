# Guia de Testes - API RGR Transporte

## 🚀 Como Testar os Endpoints

### 1. Preparação

#### Iniciar o Servidor
```bash
cd RGRTRASPORTE\RGRTRASPORTE
dotnet run
```

O servidor estará disponível em:
- HTTP: `http://localhost:5000`
- HTTPS: `https://localhost:5001`
- Swagger: `https://localhost:5001/swagger`

### 2. Testar Endpoints de Confirmação

#### 2.1. Confirmar Presença

**Endpoint:** `POST /api/Viagem/{id}/confirmar-presenca`

**Exemplo usando curl:**
```bash
curl -X POST "https://localhost:5001/api/Viagem/1/confirmar-presenca" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer SEU_TOKEN" \
  -d '{
    "viagemId": 1,
    "passageiroId": 1
  }'
```

**Exemplo usando Postman:**
1. Método: `POST`
2. URL: `https://localhost:5001/api/Viagem/1/confirmar-presenca`
3. Headers:
   - `Content-Type: application/json`
   - `Authorization: Bearer SEU_TOKEN`
4. Body (JSON):
```json
{
  "viagemId": 1,
  "passageiroId": 1
}
```

**Resposta esperada:**
```json
{
  "sucesso": true,
  "data": true,
  "mensagem": "Presença confirmada com sucesso"
}
```

#### 2.2. Cancelar Presença

**Endpoint:** `POST /api/Viagem/{id}/cancelar-presenca`

```bash
curl -X POST "https://localhost:5001/api/Viagem/1/cancelar-presenca" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer SEU_TOKEN" \
  -d '{
    "viagemId": 1,
    "passageiroId": 1,
    "motivo": "Não poderei comparecer"
  }'
```

### 3. Testar Endpoints de Consulta

#### 3.1. Viagens Pendentes de Confirmação

**Endpoint:** `GET /api/Passageiro/{id}/viagens-pendentes`

```bash
curl -X GET "https://localhost:5001/api/Passageiro/1/viagens-pendentes" \
  -H "Authorization: Bearer SEU_TOKEN"
```

**Resposta esperada:**
```json
{
  "sucesso": true,
  "data": [
    {
      "id": 1,
      "dataViagem": "2025-01-21T00:00:00",
      "origem": "Chapecó",
      "destino": "Xaxim",
      "status": "Agendada",
      "tipoTrecho": 1,
      "tipoTrechoDescricao": "Ida",
      "viagemParId": 2,
      "veiculo": {
        "id": 1,
        "placa": "ABC1234",
        "modelo": "Van"
      },
      "motorista": {
        "id": 1,
        "nome": "João Silva",
        "documento": "12345678900"
      }
    }
  ]
}
```

#### 3.2. Minhas Viagens (Passageiro)

**Endpoint:** `GET /api/Passageiro/{id}/minhas-viagens`

```bash
curl -X GET "https://localhost:5001/api/Passageiro/1/minhas-viagens?dataInicio=2025-01-20&dataFim=2025-01-27" \
  -H "Authorization: Bearer SEU_TOKEN"
```

#### 3.3. Viagens do Motorista (Hoje)

**Endpoint:** `GET /api/Viagem/motorista/{motoristaId}/viagens-hoje`

```bash
curl -X GET "https://localhost:5001/api/Viagem/motorista/1/viagens-hoje?data=2025-01-20" \
  -H "Authorization: Bearer SEU_TOKEN"
```

### 4. Testar ViagemDto Completo

#### 4.1. Obter Viagem por ID

**Endpoint:** `GET /api/Viagem/{id}`

```bash
curl -X GET "https://localhost:5001/api/Viagem/1" \
  -H "Authorization: Bearer SEU_TOKEN"
```

**Verificar se retorna:**
- ✅ `id` (long)
- ✅ `dataViagem`
- ✅ `origem` (nome da localidade)
- ✅ `destino` (nome da localidade)
- ✅ `status`
- ✅ `tipoTrecho` (1 = Ida, 2 = Volta)
- ✅ `viagemParId`
- ✅ `veiculo` (objeto completo)
- ✅ `motorista` (objeto completo)
- ✅ `passageiros` (lista completa)

### 5. Testar Criação de Viagem IDA/VOLTA

#### 5.1. Criar Viagem IDA

**Endpoint:** `POST /api/Viagem`

```json
{
  "dataViagem": "2025-01-21T00:00:00",
  "horarioSaida": "07:00:00",
  "horarioChegada": "08:30:00",
  "veiculoId": 1,
  "motoristaId": 1,
  "localidadeOrigemId": 1,
  "localidadeDestinoId": 2,
  "quantidadeVagas": 20,
  "distancia": 50.5,
  "descricaoViagem": "Viagem IDA Chapecó -> Xaxim",
  "polilinhaRota": "",
  "tipoTrecho": 1,
  "ativo": true
}
```

#### 5.2. Criar Viagem VOLTA (vinculada)

**Endpoint:** `POST /api/Viagem`

```json
{
  "dataViagem": "2025-01-21T00:00:00",
  "horarioSaida": "17:00:00",
  "horarioChegada": "18:30:00",
  "veiculoId": 1,
  "motoristaId": 1,
  "localidadeOrigemId": 2,
  "localidadeDestinoId": 1,
  "quantidadeVagas": 20,
  "distancia": 50.5,
  "descricaoViagem": "Viagem VOLTA Xaxim -> Chapecó",
  "polilinhaRota": "",
  "tipoTrecho": 2,
  "ativo": true,
  "viagemParId": 1
}
```

### 6. Testar no Swagger

1. Acesse: `https://localhost:5001/swagger`
2. Clique em "Authorize" e adicione o token JWT
3. Teste os endpoints diretamente na interface

### 7. Testar com Postman Collection

Crie uma collection no Postman com:

**Variáveis:**
- `baseUrl`: `https://localhost:5001`
- `token`: (obtido do login)

**Requests:**
1. Login → Salvar token
2. Obter Viagens Pendentes
3. Confirmar Presença
4. Cancelar Presença
5. Obter Viagens do Motorista
6. Obter Viagem por ID

### 8. Verificar Banco de Dados

#### 8.1. Verificar Migration Aplicada

```sql
-- Verificar colunas na T_VIAGEM
SELECT column_name, data_type, column_default 
FROM information_schema.columns 
WHERE table_name = 'T_VIAGEM' 
AND column_name IN ('VIA_TIPO_TRECHO', 'VIA_VIAGEM_PAR_ID');

-- Verificar colunas na T_VIAGEM_PASSAGEIRO
SELECT column_name, data_type, column_default 
FROM information_schema.columns 
WHERE table_name = 'T_VIAGEM_PASSAGEIRO' 
AND column_name IN ('VIP_STATUS_CONFIRMACAO', 'VIP_DATA_LIMITE_CONFIRMACAO', 'VIP_PASSAGEIRO_FIXO');
```

#### 8.2. Verificar Dados

```sql
-- Ver viagens com TipoTrecho
SELECT VIA_ID, VIA_TIPO_TRECHO, VIA_VIAGEM_PAR_ID 
FROM T_VIAGEM;

-- Ver passageiros com StatusConfirmacao
SELECT VIP_ID, VIP_VIAGEM_ID, VIP_PASSAGEIRO_ID, 
       VIP_STATUS_CONFIRMACAO, VIP_PASSAGEIRO_FIXO 
FROM T_VIAGEM_PASSAGEIRO;
```

### 9. Testes Automatizados (Opcional)

Crie testes unitários ou de integração:

```csharp
[Fact]
public async Task Deve_Confirmar_Presenca_Passageiro()
{
    // Arrange
    var command = new ConfirmarPresencaCommand(1, 1);
    
    // Act
    var result = await _handler.Handle(command, CancellationToken.None);
    
    // Assert
    Assert.True(result.Sucesso);
    Assert.True(result.Data);
}
```

## ✅ Checklist de Testes

- [ ] Migration executada com sucesso
- [ ] Servidor iniciado sem erros
- [ ] Swagger acessível
- [ ] Endpoint de confirmar presença funciona
- [ ] Endpoint de cancelar presença funciona
- [ ] Endpoint de viagens pendentes retorna dados
- [ ] Endpoint de viagens do motorista retorna dados
- [ ] ViagemDto retorna todos os campos (origem, destino, veiculo, motorista, passageiros)
- [ ] Criação de viagem IDA funciona
- [ ] Criação de viagem VOLTA vinculada funciona
- [ ] Banco de dados tem as novas colunas


