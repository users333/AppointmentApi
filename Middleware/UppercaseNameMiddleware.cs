using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace AppointmentApi.Middleware
{
    public class UppercaseNameMiddleware
    {
        private readonly RequestDelegate _next;

        public UppercaseNameMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!HttpMethods.IsPut(context.Request.Method))
            {
                await _next(context);
                return;
            }

            if (context.Request.Path.StartsWithSegments("/swagger"))
            {
                await _next(context);
                return;
            }

            var originalBodyStream = context.Response.Body;

            await using var memoryStream = new MemoryStream();
            context.Response.Body = memoryStream;


            await _next(context);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var body = await new StreamReader(context.Response.Body, Encoding.UTF8).ReadToEndAsync();

            context.Response.Body.Seek(0, SeekOrigin.Begin);

            if (!string.IsNullOrEmpty(context.Response.ContentType) &&
                context.Response.ContentType.Contains("application/json"))
            {

                var temp = body.Replace("\"Name\"", "\"NAME\"").Replace("\"name\"", "\"NAME\"");

                var modified = Regex.Replace(
                    temp,
                    "(\"NAME\"\\s*:\\s*\")(.*?)(\")",
                    m => $"{m.Groups[1].Value}{m.Groups[2].Value.ToUpper()}{m.Groups[3].Value}",
                    RegexOptions.Singleline);

                var modifiedBytes = Encoding.UTF8.GetBytes(modified);

                context.Response.ContentLength = modifiedBytes.Length;
                context.Response.Body = originalBodyStream;
                await context.Response.Body.WriteAsync(modifiedBytes, 0, modifiedBytes.Length);
            }
            else
            {

                context.Response.Body = originalBodyStream;
                await context.Response.WriteAsync(body);
            }
        }
    }
}
