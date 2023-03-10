using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IO;
using Newtonsoft.Json;

namespace LibraryApi.Middleware
{
    public class LoggingMiddleware
    {
        private readonly ILogger _logger;
        private readonly RequestDelegate _next;
        private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;

        public LoggingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<LoggingMiddleware>();
            _recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
        }

        public async Task Invoke(HttpContext context)
        {
            await LogRequest(context);
            await LogResponse(context);
        }

        private async Task LogRequest(HttpContext context)
        {
            context.Request.EnableBuffering();

            await using var requestStream = _recyclableMemoryStreamManager.GetStream();
            await context.Request.Body.CopyToAsync(requestStream);

            var requestBody = FormatJson(ReadStreamInChunks(requestStream));
            var queryString = context.Request.QueryString.ToString();

            _logger.LogInformation($"Request for {context.Request.Path} ({context.Request.Method}): " +
                                   (string.IsNullOrEmpty(queryString)
                                       ? ""
                                       : $"QueryString: {queryString}") +
                                   (string.IsNullOrEmpty(requestBody) ? "" : $"Request Body: {requestBody}"));
            context.Request.Body.Position = 0;
        }

        private async Task LogResponse(HttpContext context)
        {
            var originalBodyStream = context.Response.Body;

            await using var responseBody = _recyclableMemoryStreamManager.GetStream();
            context.Response.Body = responseBody;

            await _next(context);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            _logger.LogInformation($"Response for {context.Request.Path} ({context.Request.Method}): " +
                                   $"Status code: {context.Response.StatusCode}" +
                                   (string.IsNullOrEmpty(text)
                                       ? ""
                                       : $" Response Body: {FormatJson(text)}"));

            await responseBody.CopyToAsync(originalBodyStream);
        }

        private static string ReadStreamInChunks(Stream stream)
        {
            const int readChunkBufferLength = 4096;

            stream.Seek(0, SeekOrigin.Begin);

            using var textWriter = new StringWriter();
            using var reader = new StreamReader(stream);

            var readChunk = new char[readChunkBufferLength];
            int readChunkLength;

            do
            {
                readChunkLength = reader.ReadBlock(readChunk, 0, readChunkBufferLength);
                textWriter.Write(readChunk, 0, readChunkLength);
            } while (readChunkLength > 0);

            return textWriter.ToString();
        }

        private string FormatJson(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            try
            {
                var obj = JsonConvert.DeserializeObject(input);
                return JsonConvert.SerializeObject(obj, Formatting.Indented);
            }
            catch (Exception)
            {
                return input;
            }
        }
    }
}