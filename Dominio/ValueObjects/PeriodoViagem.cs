using Dominio.Exceptions;

namespace Dominio.ValueObjects
{
    public class PeriodoViagem
    {
        public DateTime Data { get; private set; }
        public TimeSpan HoraSaida { get; private set; }
        public TimeSpan HoraChegada { get; private set; }

        private PeriodoViagem() { }

        public PeriodoViagem(DateTime data, TimeSpan horaSaida, TimeSpan horaChegada)
        {
            ValidarPeriodo(data, horaSaida, horaChegada);
            
            Data = data.Date;
            HoraSaida = horaSaida;
            HoraChegada = horaChegada;
        }

        private void ValidarPeriodo(DateTime data, TimeSpan horaSaida, TimeSpan horaChegada)
        {
            if (data.Date < DateTime.Today)
                throw new DomainException("A data da viagem não pode ser anterior a hoje");

            if (horaSaida >= horaChegada)
                throw new DomainException("A hora de saída deve ser anterior à hora de chegada");
        }

        public bool ConflitaCom(PeriodoViagem outro)
        {
            if (Data != outro.Data)
                return false;

            return (HoraSaida <= outro.HoraChegada && HoraChegada >= outro.HoraSaida);
        }

        public override bool Equals(object obj)
        {
            if (obj is not PeriodoViagem outro)
                return false;

            return Data == outro.Data &&
                   HoraSaida == outro.HoraSaida &&
                   HoraChegada == outro.HoraChegada;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Data, HoraSaida, HoraChegada);
        }
    }
} 
