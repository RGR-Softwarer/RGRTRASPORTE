using Application.Dtos;
using Infra.CrossCutting.Auth;
using Microsoft.AspNetCore.Mvc;
using Dominio.Interfaces.Infra.Data.Passageiros;
using Dominio.Interfaces.Infra.Data;
using Dominio.Entidades.Pessoas;
using Dominio.Entidades.Pessoas.Passageiros;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace RGRTRASPORTE.Controllers.Auth
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IJwtService _jwtService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly CadastroContext _cadastroContext;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            IJwtService jwtService,
            IPasswordHasher passwordHasher,
            CadastroContext cadastroContext,
            ILogger<AuthController> logger)
        {
            _jwtService = jwtService;
            _passwordHasher = passwordHasher;
            _cadastroContext = cadastroContext;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest? request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new RetornoGenericoDto<object>
                    {
                        Dados = null,
                        Sucesso = false,
                        Mensagem = "Dados de login não fornecidos"
                    });
                }

                if (string.IsNullOrWhiteSpace(request.Email))
                {
                    return BadRequest(new RetornoGenericoDto<object>
                    {
                        Dados = null,
                        Sucesso = false,
                        Mensagem = "Email é obrigatório"
                    });
                }

                if (string.IsNullOrWhiteSpace(request.Senha))
                {
                    return BadRequest(new RetornoGenericoDto<object>
                    {
                        Dados = null,
                        Sucesso = false,
                        Mensagem = "Senha é obrigatória"
                    });
                }

                // Buscar passageiro
                var passageiro = await _cadastroContext.Passageiros
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.Email.ToLower() == request.Email.ToLower() && p.Situacao);

                if (passageiro != null)
                {
                    // Exigir senha sempre - não permitir login sem senha
                    if (string.IsNullOrWhiteSpace(passageiro.Senha))
                    {
                        _logger.LogWarning("Tentativa de login em passageiro sem senha: {Email}", request.Email);
                        return Unauthorized(new RetornoGenericoDto<object>
                        {
                            Dados = null,
                            Sucesso = false,
                            Mensagem = "Usuário não possui senha cadastrada. Contate o administrador."
                        });
                    }

                    if (!_passwordHasher.VerifyPassword(request.Senha, passageiro.Senha))
                    {
                        return Unauthorized(new RetornoGenericoDto<object>
                        {
                            Dados = null,
                            Sucesso = false,
                            Mensagem = "Email ou senha inválidos"
                        });
                    }

                    var token = _jwtService.GenerateToken(
                        passageiro.Id.ToString(),
                        passageiro.Email,
                        new[] { "passageiro" });

                    var response = new LoginResponse
                    {
                        Token = token,
                        User = new UserInfo
                        {
                            Id = passageiro.Id,
                            Email = passageiro.Email,
                            Nome = passageiro.Nome,
                            Tipo = "passageiro"
                        }
                    };

                    return Ok(new RetornoGenericoDto<LoginResponse>
                    {
                        Dados = response,
                        Sucesso = true,
                        Mensagem = "Login realizado com sucesso"
                    });
                }

                // Buscar motorista
                var motorista = await _cadastroContext.Motoristas
                    .AsNoTracking()
                    .FirstOrDefaultAsync(m => m.Email.ToLower() == request.Email.ToLower() && m.Situacao);

                if (motorista != null)
                {
                    // Exigir senha sempre - não permitir login sem senha
                    if (string.IsNullOrWhiteSpace(motorista.Senha))
                    {
                        _logger.LogWarning("Tentativa de login em motorista sem senha: {Email}", request.Email);
                        return Unauthorized(new RetornoGenericoDto<object>
                        {
                            Dados = null,
                            Sucesso = false,
                            Mensagem = "Usuário não possui senha cadastrada. Contate o administrador."
                        });
                    }

                    if (!_passwordHasher.VerifyPassword(request.Senha, motorista.Senha))
                    {
                        return Unauthorized(new RetornoGenericoDto<object>
                        {
                            Dados = null,
                            Sucesso = false,
                            Mensagem = "Email ou senha inválidos"
                        });
                    }

                    var token = _jwtService.GenerateToken(
                        motorista.Id.ToString(),
                        motorista.Email,
                        new[] { "motorista" });

                    var response = new LoginResponse
                    {
                        Token = token,
                        User = new UserInfo
                        {
                            Id = motorista.Id,
                            Email = motorista.Email,
                            Nome = motorista.Nome,
                            Tipo = "motorista"
                        }
                    };

                    return Ok(new RetornoGenericoDto<LoginResponse>
                    {
                        Dados = response,
                        Sucesso = true,
                        Mensagem = "Login realizado com sucesso"
                    });
                }

                return Unauthorized(new RetornoGenericoDto<object>
                {
                    Dados = null,
                    Sucesso = false,
                    Mensagem = "Email não encontrado ou usuário inativo"
                });
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("Tenant"))
            {
                _logger.LogError(ex, "Erro de tenant ao realizar login para email {Email}", request?.Email ?? "N/A");
                return StatusCode((int)HttpStatusCode.InternalServerError, new RetornoGenericoDto<object>
                {
                    Dados = null,
                    Sucesso = false,
                    Mensagem = "Erro de configuração do servidor. Contate o administrador."
                });
            }
            catch (Exception ex) when (ex.Message.Contains("Connection") || ex.Message.Contains("database") || ex.Message.Contains("Database"))
            {
                _logger.LogError(ex, "Erro de conexão ao realizar login para email {Email}", request?.Email ?? "N/A");
                return StatusCode((int)HttpStatusCode.InternalServerError, new RetornoGenericoDto<object>
                {
                    Dados = null,
                    Sucesso = false,
                    Mensagem = "Erro de conexão com o banco de dados. Tente novamente mais tarde."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao realizar login para email {Email}. Erro: {Erro}", request?.Email ?? "N/A", ex.Message);
                
                return StatusCode((int)HttpStatusCode.InternalServerError, new RetornoGenericoDto<object>
                {
                    Dados = null,
                    Sucesso = false,
                    Mensagem = "Erro ao processar solicitação de login. Tente novamente mais tarde."
                });
            }
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
    }

    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public UserInfo User { get; set; } = null!;
    }

    public class UserInfo
    {
        public long Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
    }
}

