using Microsoft.Extensions.Logging;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WishListClient.src.Interfaces;

namespace WishListClient.src.Services
{
    public class ExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<ExceptionHandler> _logger;

        public ExceptionHandler(ILogger<ExceptionHandler> logger) =>
            _logger = logger;

        public string? GetMessageException(Exception ex)
        {
            _logger.LogError(ex, "Ошибка при выполнении HTTP-запроса");

            return ex switch
            {
                ApiException apiEx => apiEx.StatusCode switch
                {
                    HttpStatusCode.Unauthorized => null,
                    HttpStatusCode.Forbidden => "Доступ запрещен.",
                    HttpStatusCode.NotFound => "Данные не найдены.",
                    HttpStatusCode.BadRequest => "Сервер получил некорректные данные.",
                    >= HttpStatusCode.InternalServerError => "Ошибка на сервере. Попробуйте позже.",
                    _ => $"Ошибка сервера: {(int)apiEx.StatusCode}"
                },
                TaskCanceledException => "Превышено время ожидания. Проверьте соединение интернета.",
                HttpRequestException => "Нет подключения к интернету или сервер недоступен.",
                _ => "Возникла неизвестна ошибка."
            };
        }
    }
}
