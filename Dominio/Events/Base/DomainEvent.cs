using System;

namespace Dominio.Events.Base
{
    public abstract class DomainEvent
    {
        public DateTime DataOcorrencia { get; private set; }

        protected DomainEvent()
        {
            DataOcorrencia = DateTime.UtcNow;
        }
    }
} 