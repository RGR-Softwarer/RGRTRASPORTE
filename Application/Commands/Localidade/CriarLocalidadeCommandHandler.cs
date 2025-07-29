using Application.Commands.Localidade;
using Application.Common;
using Dominio.Interfaces.Infra.Data;
using Dominio.ValueObjects;
using MediatR;

namespace Application.Commands.Localidade
{
    public class CriarLocalidadeCommandHandler : IRequestHandler<CriarLocalidadeCommand, BaseResponse<long>>
    {
        private readonly IGenericRepository<Dominio.Entidades.Localidades.Localidade> _localidadeRepository;

        public CriarLocalidadeCommandHandler(
            IGenericRepository<Dominio.Entidades.Localidades.Localidade> localidadeRepository)
        {
            _localidadeRepository = localidadeRepository;
        }

        public async Task<BaseResponse<long>> Handle(CriarLocalidadeCommand request, CancellationToken cancellationToken)
        {
            var endereco = new Endereco(
                request.Estado,
                request.Cidade,
                request.Cep,
                request.Bairro,
                request.Logradouro,
                request.Numero,
                request.Complemento);

            var localidade = Dominio.Entidades.Localidades.Localidade.CriarLocalidade(
                request.Nome,
                endereco,
                request.Latitude,
                request.Longitude);

            await _localidadeRepository.AdicionarAsync(localidade);
            return BaseResponse<long>.Ok(localidade.Id);
        }
    }
} 