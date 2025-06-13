﻿using Dominio.Dtos;
using Dominio.Interfaces.Infra.Data;
using Infra.CrossCutting.Handlers.Notifications;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Application.Common;
using Microsoft.Extensions.Logging;

namespace RGRTRASPORTE.Controllers
{
    public abstract class AbstractControllerBase : ControllerBase
    {
        private readonly INotificationContext _notificationHandler;

        protected AbstractControllerBase(INotificationContext notificationHandler)
        {
            _notificationHandler = notificationHandler;
        }

        protected Task<ActionResult> RGRResult(HttpStatusCode statusCode = HttpStatusCode.OK, object value = null, int? pagina = null, int? limite = null, int? quantidade = null)
        {
            var existeErro = _notificationHandler.HasNotifications();

            // Se há erro, ignora o value e retorna só as mensagens de erro
            if (existeErro)
            {
                var resposta = new RetornoGenericoDto
                {
                    Dados = null,
                    Mensagens = _notificationHandler.GetNotifications().Select(x => x.Message).ToList(),
                    Sucesso = false,
                    Pagina = pagina,
                    Limite = limite,
                    Quantidade = quantidade
                };

                return Task.FromResult<ActionResult>(new ObjectResult(resposta)
                {
                    StatusCode = (int)statusCode
                });
            }

            // Se não há erro, extrai os dados do BaseResponse se for o caso
            object dados = value;
            if (value != null)
            {
                var valueType = value.GetType();
                
                // Verifica se é um BaseResponse<T>
                if (valueType.IsGenericType && valueType.GetGenericTypeDefinition() == typeof(BaseResponse<>))
                {
                    var dadosProperty = valueType.GetProperty("Dados");
                    if (dadosProperty != null)
                    {
                        dados = dadosProperty.GetValue(value);
                    }
                }
            }

            var respostaSucesso = new RetornoGenericoDto
            {
                Dados = dados,
                Mensagens = new List<string>(),
                Sucesso = true,
                Pagina = pagina,
                Limite = limite,
                Quantidade = quantidade ?? (dados is System.Collections.IEnumerable enumerable && dados is not string ? 
                    enumerable.Cast<object>().Count() : null)
            };

            return Task.FromResult<ActionResult>(new ObjectResult(respostaSucesso)
            {
                StatusCode = (int)statusCode
            });
        }
    }
}