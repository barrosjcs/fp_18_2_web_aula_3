﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fp_stack.web.Middlewares
{
    public class LogMiddleware
    {
        private RequestDelegate _next;

        public LogMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            context.Request.EnableRewind();

            var request = await FormatRequest(context.Request);
            var log = new LoggerConfiguration()
            .WriteTo.Logentries("9b4c08d3-cd6d-4612-a0b5-68912c04c465")
            .CreateLogger();
            log.Information($"request {request}");

            context.Request.Body.Position = 0;

            await _next(context);

            #region Exemplo
            ////var stopwatch = new Stopwatch();
            ////stopwatch.Start();
            //await _next(context);
            ////stopwatch.Stop();
            ////Console.WriteLine( $"demorou {stopwatch.ElapsedMilliseconds} ms " );
            #endregion

        }
        private static async Task<string> FormatRequest(HttpRequest request)
        {
            var body = request.Body;
            request.EnableRewind();
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer, 0, buffer.Length);
            var bodyAsText = Encoding.UTF8.GetString(buffer);
            request.Body = body;

            var messageObjToLog = new { scheme = request.Scheme, host = request.Host, path = request.Path, queryString = request.Query, requestBody = bodyAsText };

            return JsonConvert.SerializeObject(messageObjToLog);
        }
    }
}
