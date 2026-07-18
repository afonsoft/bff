using System.Net;

namespace Eaf.Template.Bff.Tests.Helpers;

/// <summary>
/// HttpMessageHandler fake para simular respostas HTTP em testes unitários.
/// </summary>
public class MockHttpMessageHandler : HttpMessageHandler
{
    private readonly Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> _sendFunc;

    public MockHttpMessageHandler(Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendFunc)
    {
        _sendFunc = sendFunc;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return await _sendFunc(request, cancellationToken);
    }
}
