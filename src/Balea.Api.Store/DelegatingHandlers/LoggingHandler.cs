using Balea.Server.Diagnostics;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Balea.Api.Store.DelegatingHandlers
{
    public class LoggingHandler : DelegatingHandler
    {
        private readonly ILogger<LoggingHandler> _logger;

        public LoggingHandler(ILogger<LoggingHandler> logger)
        {
            Ensure.Argument.NotNull(logger, nameof(logger));
            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, 
            CancellationToken cancellationToken)
        {
            Log.ClientAuthorizationRequestBegin(_logger, request);

            var response = await base.SendAsync(request, cancellationToken);

            Log.ClientAuthorizationRequestEnd(_logger, response);

            return response;
        }
    }
}
