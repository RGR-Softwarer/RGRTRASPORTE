using Dominio.Entidades.Localidades;
using Dominio.Entidades.Pessoas;
using Dominio.Entidades.Pessoas.Passageiros;
using Dominio.Entidades.Veiculos;
using Dominio.Entidades.Viagens;
using Dominio.Enums.Pessoas;
using Dominio.Enums.Veiculo;
using Dominio.Enums.Viagens;
using Dominio.ValueObjects;
using Infra.CrossCutting.Auth;
using Infra.Data.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace RGRTRASPORTE.Controllers.Seed
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeedController : ControllerBase
    {
        private readonly CadastroContext _cadastroContext;
        private readonly TransportadorContext _transportadorContext;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ILogger<SeedController> _logger;

        public SeedController(
            CadastroContext cadastroContext,
            TransportadorContext transportadorContext,
            IPasswordHasher passwordHasher,
            ILogger<SeedController> logger)
        {
            _cadastroContext = cadastroContext;
            _transportadorContext = transportadorContext;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        /// <summary>
        /// Popula o banco de dados com dados de teste
        /// </summary>
        [HttpPost("popular")]
        public async Task<IActionResult> PopularDados()
        {
            try
            {
                _logger.LogInformation("Iniciando seed de dados de teste...");

                // 1. Criar Localidades
                var localidades = await CriarLocalidades();
                _logger.LogInformation($"Criadas {localidades.Count} localidades");

                // 2. Criar Motoristas
                var motoristas = await CriarMotoristas();
                _logger.LogInformation($"Criados {motoristas.Count} motoristas");

                // 3. Criar Veículos
                var veiculos = await CriarVeiculos();
                _logger.LogInformation($"Criados {veiculos.Count} veículos");

                // 4. Criar Passageiros
                var passageiros = await CriarPassageiros(localidades);
                _logger.LogInformation($"Criados {passageiros.Count} passageiros");

                // 5. Criar Viagens Passadas (Finalizadas)
                var viagensPassadas = await CriarViagensPassadas(localidades, motoristas, veiculos, passageiros);
                _logger.LogInformation($"Criadas {viagensPassadas.Count} viagens passadas");

                // 6. Criar Viagens Atuais (Agendadas e Em Andamento)
                var viagensAtuais = await CriarViagensAtuais(localidades, motoristas, veiculos, passageiros);
                _logger.LogInformation($"Criadas {viagensAtuais.Count} viagens atuais");

                // Coletar emails disponíveis para login
                var emailsPassageiros = passageiros.Select(p => p.Email).ToList();
                var emailsMotoristas = motoristas.Select(m => m.Email).ToList();

                return Ok(new
                {
                    sucesso = true,
                    mensagem = "Dados de teste criados com sucesso",
                    resumo = new
                    {
                        localidades = localidades.Count,
                        motoristas = motoristas.Count,
                        veiculos = veiculos.Count,
                        passageiros = passageiros.Count,
                        viagensPassadas = viagensPassadas.Count,
                        viagensAtuais = viagensAtuais.Count,
                        totalViagens = viagensPassadas.Count + viagensAtuais.Count
                    },
                    credenciais = new
                    {
                        info = "Use qualquer um dos emails abaixo para fazer login. Senha padrão: 123456",
                        passageiros = emailsPassageiros,
                        motoristas = emailsMotoristas,
                        exemplo = new
                        {
                            email = emailsPassageiros.FirstOrDefault() ?? emailsMotoristas.FirstOrDefault(),
                            senha = "123456"
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao popular dados de teste");
                return StatusCode(500, new
                {
                    sucesso = false,
                    mensagem = "Erro ao popular dados de teste",
                    erro = ex.Message
                });
            }
        }

        private async Task<List<Localidade>> CriarLocalidades()
        {
            var localidades = new List<Localidade>();

            // Localidades de origem (bairros residenciais)
            var bairros = new[]
            {
                ("Centro", "SP", "São Paulo", "01310-100", "Centro", "Av. Paulista", "1000"),
                ("Vila Mariana", "SP", "São Paulo", "04020-000", "Vila Mariana", "Rua Domingos de Morais", "500"),
                ("Pinheiros", "SP", "São Paulo", "05422-000", "Pinheiros", "Rua dos Pinheiros", "800"),
                ("Butantã", "SP", "São Paulo", "05508-000", "Butantã", "Av. Corifeu de Azevedo Marques", "3000"),
                ("Lapa", "SP", "São Paulo", "05089-000", "Lapa", "Rua Guaicurus", "200")
            };

            // Localidades de destino (universidades)
            var universidades = new[]
            {
                ("USP - Butantã", "SP", "São Paulo", "05508-000", "Butantã", "Av. Prof. Luciano Gualberto", "380", -23.5581m, -46.7314m),
                ("PUC-SP", "SP", "São Paulo", "05014-901", "Perdizes", "Rua Monte Alegre", "984", -23.5289m, -46.6756m),
                ("Mackenzie", "SP", "São Paulo", "01302-907", "Consolação", "Rua da Consolação", "930", -23.5505m, -46.6608m),
                ("UNIFESP", "SP", "São Paulo", "04023-062", "Vila Clementino", "Rua Botucatu", "740", -23.6025m, -46.6408m),
                ("FGV", "SP", "São Paulo", "01310-100", "Bela Vista", "Rua Itapeva", "432", -23.5621m, -46.6564m)
            };

            // Criar bairros
            foreach (var (nome, estado, cidade, cep, bairro, logradouro, numero) in bairros)
            {
                var cepLimpo = cep.Replace("-", "").Replace(".", "").Trim();
                var endereco = new Endereco(estado, cidade, cepLimpo, bairro, logradouro, numero);
                var localidade = Localidade.CriarLocalidade(nome, endereco, -23.5505m, -46.6333m);
                localidades.Add(localidade);
            }

            // Criar universidades
            foreach (var (nome, estado, cidade, cep, bairro, logradouro, numero, lat, lng) in universidades)
            {
                var cepLimpo = cep.Replace("-", "").Replace(".", "").Trim();
                var endereco = new Endereco(estado, cidade, cepLimpo, bairro, logradouro, numero);
                var localidade = Localidade.CriarLocalidade(nome, endereco, lat, lng);
                localidades.Add(localidade);
            }

            _cadastroContext.Localidades.AddRange(localidades);
            await _cadastroContext.SaveChangesAsync();

            return localidades;
        }

        private string GerarCPFValido(string baseNumero)
        {
            // Gera um CPF válido baseado em um número base
            // Remove formatação e garante 9 dígitos
            var cpf = baseNumero.Replace(".", "").Replace("-", "").Trim();
            if (cpf.Length > 9) cpf = cpf.Substring(0, 9);
            while (cpf.Length < 9) cpf += "0";

            // Calcula primeiro dígito verificador
            int soma = 0;
            for (int i = 0; i < 9; i++)
                soma += int.Parse(cpf[i].ToString()) * (10 - i);
            int resto = soma % 11;
            int digito1 = resto < 2 ? 0 : 11 - resto;
            cpf += digito1.ToString();

            // Calcula segundo dígito verificador
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(cpf[i].ToString()) * (11 - i);
            resto = soma % 11;
            int digito2 = resto < 2 ? 0 : 11 - resto;
            cpf += digito2.ToString();

            return cpf;
        }

        private async Task<List<Motorista>> CriarMotoristas()
        {
            var motoristas = new List<Motorista>();

            var hojeUtc = DateTime.UtcNow.Date;
            var dadosMotoristas = new[]
            {
                ("João Silva", "111444777", "123456789", "11987654321", "joao.silva@email.com", SexoEnum.Masculino, "12345678901", CategoriaCNHEnum.D, hojeUtc.AddYears(3)),
                ("Maria Santos", "222555888", "234567890", "11976543210", "maria.santos@email.com", SexoEnum.Feminino, "23456789012", CategoriaCNHEnum.D, hojeUtc.AddYears(2)),
                ("Pedro Oliveira", "333666999", "345678901", "11965432109", "pedro.oliveira@email.com", SexoEnum.Masculino, "34567890123", CategoriaCNHEnum.D, hojeUtc.AddYears(4)),
                ("Ana Costa", "444777000", "456789012", "11954321098", "ana.costa@email.com", SexoEnum.Feminino, "45678901234", CategoriaCNHEnum.B, hojeUtc.AddYears(1)),
                ("Carlos Souza", "555888111", "567890123", "11943210987", "carlos.souza@email.com", SexoEnum.Masculino, "56789012345", CategoriaCNHEnum.D, hojeUtc.AddYears(5))
            };

            foreach (var (nome, cpfBase, rg, telefone, email, sexo, cnh, categoria, validade) in dadosMotoristas)
            {
                var cpfValido = GerarCPFValido(cpfBase);
                var motorista = Motorista.CriarMotorista(
                    nome,
                    new CPF(cpfValido),
                    rg,
                    telefone,
                    email,
                    sexo,
                    cnh,
                    categoria,
                    validade,
                    $"Motorista {nome}"
                );
                // Definir senha padrão: "123456"
                var senhaHash = _passwordHasher.HashPassword("123456");
                motorista.DefinirSenha(senhaHash);
                motoristas.Add(motorista);
            }

            _cadastroContext.Motoristas.AddRange(motoristas);
            await _cadastroContext.SaveChangesAsync();

            return motoristas;
        }

        private async Task<List<Veiculo>> CriarVeiculos()
        {
            var veiculos = new List<Veiculo>();

            var hojeUtc = DateTime.UtcNow.Date;
            var dadosVeiculos = new[]
            {
                ("ABC1234", "Sprinter", "Mercedes-Benz", "9BW12345678901234", 2023, 2023, "Branco", "12345678901", hojeUtc.AddMonths(6), TipoCombustivelEnum.Diesel, StatusVeiculoEnum.Disponivel, "Van 15 lugares"),
                ("DEF5678", "Master", "Renault", "9BW23456789012345", 2022, 2022, "Azul", "23456789012", hojeUtc.AddMonths(8), TipoCombustivelEnum.Diesel, StatusVeiculoEnum.Disponivel, "Van 12 lugares"),
                ("GHI9012", "Daily", "Iveco", "9BW34567890123456", 2023, 2023, "Prata", "34567890123", hojeUtc.AddMonths(10), TipoCombustivelEnum.Diesel, StatusVeiculoEnum.Disponivel, "Van 18 lugares"),
                ("JKL3456", "Marcopolo", "Volvo", "9BW45678901234567", 2021, 2021, "Branco", "45678901234", hojeUtc.AddMonths(4), TipoCombustivelEnum.Diesel, StatusVeiculoEnum.Disponivel, "Ônibus 40 lugares"),
                ("MNO7890", "Paradiso", "Mercedes-Benz", "9BW56789012345678", 2022, 2022, "Azul", "56789012345", hojeUtc.AddMonths(12), TipoCombustivelEnum.Diesel, StatusVeiculoEnum.Disponivel, "Ônibus 44 lugares")
            };

            foreach (var (placa, modelo, marca, chassi, anoModelo, anoFabricacao, cor, renavam, vencimento, combustivel, status, observacao) in dadosVeiculos)
            {
                var veiculo = Veiculo.CriarVeiculo(
                    new Placa(placa),
                    modelo,
                    marca,
                    chassi,
                    anoModelo,
                    anoFabricacao,
                    cor,
                    renavam,
                    vencimento,
                    combustivel,
                    status,
                    observacao,
                    null
                );
                veiculos.Add(veiculo);
            }

            _transportadorContext.Veiculos.AddRange(veiculos);
            await _transportadorContext.SaveChangesAsync();

            return veiculos;
        }

        private async Task<List<Passageiro>> CriarPassageiros(List<Localidade> localidades)
        {
            var passageiros = new List<Passageiro>();

            // Pegar algumas localidades para usar
            var localidadeResidencia = localidades[0]; // Centro
            var localidadeEmbarque = localidades[1]; // Vila Mariana
            var localidadeDesembarque = localidades[5]; // USP

            var dadosPassageiros = new[]
            {
                ("Ana Paula Silva", "111444777", "11911111111", "ana.silva@email.com", SexoEnum.Feminino),
                ("Bruno Santos", "222555888", "11922222222", "bruno.santos@email.com", SexoEnum.Masculino),
                ("Carla Oliveira", "333666999", "11933333333", "carla.oliveira@email.com", SexoEnum.Feminino),
                ("Daniel Costa", "444777000", "11944444444", "daniel.costa@email.com", SexoEnum.Masculino),
                ("Eduarda Souza", "555888111", "11955555555", "eduarda.souza@email.com", SexoEnum.Feminino),
                ("Felipe Lima", "666999222", "11966666666", "felipe.lima@email.com", SexoEnum.Masculino),
                ("Gabriela Alves", "777000333", "11977777777", "gabriela.alves@email.com", SexoEnum.Feminino),
                ("Henrique Pereira", "888111444", "11988888888", "henrique.pereira@email.com", SexoEnum.Masculino),
                ("Isabela Rocha", "999222555", "11999999999", "isabela.rocha@email.com", SexoEnum.Feminino),
                ("Juliano Martins", "000333666", "11900000000", "juliano.martins@email.com", SexoEnum.Masculino),
                ("Larissa Ferreira", "111444778", "11911111112", "larissa.ferreira@email.com", SexoEnum.Feminino),
                ("Marcos Rodrigues", "222555889", "11922222223", "marcos.rodrigues@email.com", SexoEnum.Masculino),
                ("Natália Barbosa", "333666990", "11933333334", "natalia.barbosa@email.com", SexoEnum.Feminino),
                ("Otávio Gomes", "444777001", "11944444445", "otavio.gomes@email.com", SexoEnum.Masculino),
                ("Patrícia Dias", "555888112", "11955555556", "patricia.dias@email.com", SexoEnum.Feminino)
            };

            foreach (var (nome, cpfBase, telefone, email, sexo) in dadosPassageiros)
            {
                var cpfValido = GerarCPFValido(cpfBase);
                var passageiro = Passageiro.CriarPassageiro(
                    nome,
                    new CPF(cpfValido),
                    telefone,
                    email,
                    sexo,
                    localidadeResidencia.Id,
                    localidadeEmbarque.Id,
                    localidadeDesembarque.Id,
                    $"Estudante {nome}"
                );
                // Definir senha padrão: "123456"
                var senhaHash = _passwordHasher.HashPassword("123456");
                passageiro.DefinirSenha(senhaHash);
                passageiros.Add(passageiro);
            }

            _cadastroContext.Passageiros.AddRange(passageiros);
            await _cadastroContext.SaveChangesAsync();

            return passageiros;
        }

        private async Task<List<Viagem>> CriarViagensPassadas(
            List<Localidade> localidades,
            List<Motorista> motoristas,
            List<Veiculo> veiculos,
            List<Passageiro> passageiros)
        {
            var viagens = new List<Viagem>();

            // Viagens finalizadas da última semana
            var hoje = DateTime.UtcNow.Date;
            var random = new Random();

            for (int i = 1; i <= 10; i++)
            {
                var dataViagemPassada = hoje.AddDays(-i);
                var horarioSaida = new TimeSpan(7, 0, 0); // 7h
                var horarioChegada = new TimeSpan(8, 30, 0); // 8h30

                var origem = localidades[random.Next(0, 5)]; // Bairros
                var destino = localidades[random.Next(5, 10)]; // Universidades
                var veiculo = veiculos[random.Next(0, veiculos.Count)];
                var motorista = motoristas[random.Next(0, motoristas.Count)];

                // Cria viagem com data de hoje primeiro (para passar na validação)
                var viagem = Viagem.CriarViagemRegular(
                    hoje, // Data temporária (hoje)
                    horarioSaida,
                    horarioChegada,
                    veiculo.Id,
                    motorista.Id,
                    origem.Id,
                    destino.Id,
                    15, // quantidade de vagas
                    25.5m, // distância em km
                    $"Viagem {i} - {origem.Nome} para {destino.Nome}",
                    "POLILINHA_EXEMPLO_" + i,
                    TipoTrechoViagemEnum.Ida,
                    true
                );

                // Adicionar alguns passageiros antes de finalizar
                var numPassageiros = random.Next(5, Math.Min(12, passageiros.Count));
                var passageirosSelecionados = passageiros.OrderBy(x => random.Next()).Take(numPassageiros).ToList();

                foreach (var passageiro in passageirosSelecionados)
                {
                    try
                    {
                        viagem.AdicionarPassageiro(passageiro);
                    }
                    catch
                    {
                        // Ignora se já estiver na viagem
                    }
                }

                // Finalizar a viagem
                viagem.IniciarViagem();
                viagem.FinalizarViagem();

                viagens.Add(viagem);
            }

            // Salva as viagens primeiro
            _transportadorContext.Viagens.AddRange(viagens);
            await _transportadorContext.SaveChangesAsync();

            // Agora atualiza as datas para o passado usando SQL direto
            for (int i = 0; i < viagens.Count; i++)
            {
                var viagem = viagens[i];
                var dataViagemPassada = hoje.AddDays(-(i + 1));
                
                // Atualiza a data da viagem diretamente no banco usando SQL
                // O PeriodoViagem é mapeado como shadow property VIA_DATA
                await _transportadorContext.Database.ExecuteSqlRawAsync(
                    @"UPDATE ""T_VIAGEM"" 
                      SET ""VIA_DATA"" = {0} 
                      WHERE ""VIA_ID"" = {1}",
                    dataViagemPassada, viagem.Id);
            }

            return viagens;
        }

        private async Task<List<Viagem>> CriarViagensAtuais(
            List<Localidade> localidades,
            List<Motorista> motoristas,
            List<Veiculo> veiculos,
            List<Passageiro> passageiros)
        {
            var viagens = new List<Viagem>();
            var random = new Random();
            var hoje = DateTime.UtcNow.Date;

            // Viagens agendadas para hoje e próximos dias
            for (int i = 0; i < 5; i++)
            {
                var dataViagem = hoje.AddDays(i);
                var horarioSaida = new TimeSpan(7, 0, 0); // 7h
                var horarioChegada = new TimeSpan(8, 30, 0); // 8h30

                var origem = localidades[random.Next(0, 5)]; // Bairros
                var destino = localidades[random.Next(5, 10)]; // Universidades
                var veiculo = veiculos[random.Next(0, veiculos.Count)];
                var motorista = motoristas[random.Next(0, motoristas.Count)];

                var viagem = Viagem.CriarViagemRegular(
                    dataViagem,
                    horarioSaida,
                    horarioChegada,
                    veiculo.Id,
                    motorista.Id,
                    origem.Id,
                    destino.Id,
                    15, // quantidade de vagas
                    25.5m, // distância em km
                    $"Viagem Agendada {i + 1} - {origem.Nome} para {destino.Nome}",
                    "POLILINHA_EXEMPLO_AGENDADA_" + i,
                    TipoTrechoViagemEnum.Ida,
                    true
                );

                // Adicionar alguns passageiros
                var numPassageiros = random.Next(3, Math.Min(10, passageiros.Count));
                var passageirosSelecionados = passageiros.OrderBy(x => random.Next()).Take(numPassageiros).ToList();

                foreach (var passageiro in passageirosSelecionados)
                {
                    try
                    {
                        viagem.AdicionarPassageiro(passageiro);
                    }
                    catch
                    {
                        // Ignora se já estiver na viagem
                    }
                }

                viagens.Add(viagem);
            }

            // Uma viagem em andamento (hoje)
            var viagemEmAndamento = Viagem.CriarViagemRegular(
                hoje,
                new TimeSpan(6, 30, 0), // 6h30
                new TimeSpan(8, 0, 0), // 8h
                veiculos[0].Id,
                motoristas[0].Id,
                localidades[0].Id,
                localidades[5].Id,
                15,
                20.0m,
                "Viagem em Andamento - Centro para USP",
                "POLILINHA_EM_ANDAMENTO",
                TipoTrechoViagemEnum.Ida,
                true
            );

            // Adicionar passageiros
            var passageirosEmAndamento = passageiros.Take(8).ToList();
            foreach (var passageiro in passageirosEmAndamento)
            {
                try
                {
                    viagemEmAndamento.AdicionarPassageiro(passageiro);
                }
                catch { }
            }

            // Iniciar a viagem
            viagemEmAndamento.IniciarViagem();
            viagens.Add(viagemEmAndamento);

            _transportadorContext.Viagens.AddRange(viagens);
            await _transportadorContext.SaveChangesAsync();

            return viagens;
        }
    }
}

