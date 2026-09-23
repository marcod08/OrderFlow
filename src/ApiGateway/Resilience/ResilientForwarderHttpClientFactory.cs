using System.Net;
using Microsoft.Extensions.Http.Resilience;
using Polly;
using Polly.Timeout;
using Yarp.ReverseProxy.Forwarder;

namespace ApiGateway.Resilience;

public class ResilientForwarderHttpClientFactory : ForwarderHttpClientFactory
{
    protected override HttpMessageHandler WrapHandler(ForwarderHttpClientContext context, HttpMessageHandler handler)
    {
        var baseHandler = base.WrapHandler(context, handler);

        // Strategies wrap each other in the order they are added (first = outermost).
        // The timeout is innermost so that it applies to each single attempt, not to the whole retry sequence.
        var pipeline = new ResiliencePipelineBuilder<HttpResponseMessage>()
            .AddRetry(new Polly.Retry.RetryStrategyOptions<HttpResponseMessage>
            {
                ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                    .HandleResult(r => r.StatusCode == HttpStatusCode.ServiceUnavailable)
                    .Handle<HttpRequestException>()
                    .Handle<TimeoutRejectedException>(),
                MaxRetryAttempts = 3,
                Delay = TimeSpan.FromMilliseconds(500),
                OnRetry = args =>
                {
                Console.WriteLine($"[Resilience] Retry attempt {args.AttemptNumber + 1} after failure: {args.Outcome.Exception?.Message}");
                return ValueTask.CompletedTask;
                }
            })
            .AddCircuitBreaker(new Polly.CircuitBreaker.CircuitBreakerStrategyOptions<HttpResponseMessage>
            {
                ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                    .HandleResult(r => r.StatusCode == HttpStatusCode.ServiceUnavailable)
                    .Handle<HttpRequestException>()
                    .Handle<TimeoutRejectedException>(),
                FailureRatio = 0.5,
                MinimumThroughput = 5,
                BreakDuration = TimeSpan.FromSeconds(10)
            })
            .AddTimeout(TimeSpan.FromSeconds(2))
            .Build();

        return new ResilienceHandler(pipeline) {InnerHandler = baseHandler};
    }
}