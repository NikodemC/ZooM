using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using ZooM.Core.Exceptions;

namespace ZooM.Api.Middleware
{
    public class DomainExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public DomainExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (DomainException ex)
            {
                context.Response.StatusCode = 400;

                var result = new ErrorResult {Result = ex.Message};
                var json = JsonConvert.SerializeObject(result);

                await context.Response.WriteAsync(json);
            }
        }

        private class ErrorResult
        {
            public string Result { get; set; }
        }
    }
}