using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Example.ProducerConsumer.WebApi.Infrastructure
{
	// You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
	public class ExceptionMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<ExceptionMiddleware> _logger;

		public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
		{
			_next = next;
			_logger = logger;
		}

		public async Task Invoke(HttpContext httpContext)
		{
			try
			{
				await _next(httpContext);
			}
			catch (Exception ex)
			{
				await HandleException(httpContext, ex);
			}
		}

		private async Task HandleException(HttpContext httpContext, Exception ex)
		{
			var baseEx = ex.GetBaseException();
			var req = httpContext.Request;
			// Allows using several time the stream in ASP.Net Core
			req.EnableRewind();
			req.Body.Position = 0;
			// Arguments: Stream, Encoding, detect encoding, buffer size 
			// AND, the most important: keep stream opened
			using (StreamReader reader
					  = new StreamReader(req.Body, Encoding.UTF8, true, 1024, true))
			{
				string bodyStr = reader.ReadToEnd();
				JObject json = JObject.Parse(bodyStr);
				string conversationId;
				string correlationId;
				conversationId = json.GetValue(nameof(conversationId)).Value<string>();
				correlationId = json.GetValue(nameof(correlationId)).Value<string>();
				string error = $"Web Api Exception: ConversationId:{conversationId}, CorrelationId:{correlationId} "
					+ $"Exception: {baseEx.Message}";
				httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
				await httpContext.Response.WriteAsync("");
				_logger.LogCritical(error);
				// Rewind, so the core is not lost when it looks the body for the request
				req.Body.Position = 0;
			}
		}
	}

	// Extension method used to add the middleware to the HTTP request pipeline.
	public static class ExceptionMiddlewareExtensions
	{
		public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<ExceptionMiddleware>();
		}
	}
}
