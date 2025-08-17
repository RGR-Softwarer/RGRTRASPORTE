# Sistema de Login Local

## Visão Geral

O sistema foi configurado para permitir login com qualquer usuário em ambiente de desenvolvimento local. Isso facilita o desenvolvimento e testes sem necessidade de configurar um servidor de autenticação.

## Como Funciona

### Ambiente Local (Desenvolvimento)
- **Detecção**: O sistema detecta automaticamente se está em ambiente local através da propriedade `environment.production`
- **Validação**: Apenas o email é validado (deve ser um email válido)
- **Senha**: Qualquer senha é aceita, incluindo senha vazia
- **Token**: Um token mock é gerado automaticamente
- **Feedback**: Mensagens informativas indicam que está em ambiente local

### Ambiente de Produção
- **Validação**: Email e senha são validados rigorosamente
- **Autenticação**: Requisição real para o servidor de autenticação
- **Token**: Token real retornado pelo servidor

## Uso

### Para Desenvolvedores
1. Execute o projeto em modo de desenvolvimento (`ng serve`)
2. Acesse a página de login
3. Digite qualquer email válido (ex: `dev@local.com`)
4. Digite qualquer senha ou deixe em branco
5. Clique em "Entrar"

### Exemplos de Login Local
```
Email: dev@local.com
Senha: (qualquer coisa ou vazio)

Email: teste@exemplo.com
Senha: 123456

Email: admin@local.dev
Senha: (deixe em branco)
```

## Configuração

### Arquivos Modificados
- `src/app/services/login/login.service.ts` - Lógica de autenticação local
- `src/app/paginas/auth/login/login.component.ts` - Validação condicional
- `src/app/paginas/auth/login/login.component.html` - Interface informativa
- `src/app/paginas/auth/login/login.component.scss` - Estilos da interface

### Variáveis de Ambiente
- `environment.production` - Controla se está em modo local ou produção
- `environment.apiBaseUrl` - URL da API (não usada em local)

## Segurança

⚠️ **Importante**: Este sistema é apenas para desenvolvimento local. Em produção, a autenticação real é sempre usada.

### Verificações de Segurança
- A funcionalidade só é ativada quando `environment.production = false`
- Em produção, o sistema usa sempre a autenticação real
- Tokens mock são claramente identificados com prefixo `local_token_`

## Troubleshooting

### Problemas Comuns

1. **Login não funciona em local**
   - Verifique se `environment.production = false`
   - Confirme que o email é válido

2. **Validação de senha ainda aparece**
   - Limpe o cache do navegador
   - Reinicie o servidor de desenvolvimento

3. **Mensagem de ambiente local não aparece**
   - Verifique se está executando em modo de desenvolvimento
   - Confirme que o arquivo `environment.ts` tem `production: false`

## Desenvolvimento Futuro

Para adicionar mais funcionalidades ao login local:

1. **Usuários Mock**: Criar uma lista de usuários predefinidos
2. **Perfis**: Simular diferentes tipos de usuário (admin, usuário comum, etc.)
3. **Permissões**: Simular diferentes níveis de acesso
4. **Logs**: Adicionar logs para debug em desenvolvimento