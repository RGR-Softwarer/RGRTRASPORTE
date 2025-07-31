using System;
using MediatR;

namespace Dominio.Events.Base
{
    public abstract class DomainEvent : INotification
    {
        public DateTime DataOcorrencia { get; private set; }

        protected DomainEvent()
        {
            DataOcorrencia = DateTime.UtcNow;
        }
    }
} 
