using Dominio.Events.Base;

namespace Dominio.Events.Viagens
{
    public class HorarioViagemAlteradoEvent : DomainEvent
    {
        public long ViagemId { get; }
        public TimeSpan HorarioSaidaAnterior { get; }
        public TimeSpan HorarioChegadaAnterior { get; }
        public TimeSpan NovoHorarioSaida { get; }
        public TimeSpan NovoHorarioChegada { get; }

        public HorarioViagemAlteradoEvent(
            long viagemId, 
            TimeSpan horarioSaidaAnterior, 
            TimeSpan horarioChegadaAnterior,
            TimeSpan novoHorarioSaida, 
            TimeSpan novoHorarioChegada)
        {
            ViagemId = viagemId;
            HorarioSaidaAnterior = horarioSaidaAnterior;
            HorarioChegadaAnterior = horarioChegadaAnterior;
            NovoHorarioSaida = novoHorarioSaida;
            NovoHorarioChegada = novoHorarioChegada;
        }
    }
} 