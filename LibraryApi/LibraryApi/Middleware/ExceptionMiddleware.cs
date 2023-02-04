using System;
using System.Threading.Tasks;
using LibraryApi.DTO;
using LibraryApi.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace LibraryApi.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<ExceptionMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (CustomException ex)
            {
                await WriteResponse(context, 400, ex.ErrorCode);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.GetType().Name} - {ex.Message} \n {ex.StackTrace}");
                await WriteResponse(context, 500, ErrorCode.CommonErrors.InternalError);
            }
        }

        private async Task WriteResponse(HttpContext context, int statusCode, ErrorCode code)
        {
            var errorResponse = new ErrorResponse
            {
                Code = code.Value
            };
            var result = JsonConvert.SerializeObject(errorResponse);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsync(result);
        }
    }
}