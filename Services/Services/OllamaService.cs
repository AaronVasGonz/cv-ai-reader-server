
using OllamaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services;

public interface IOllamaService
{
    Task<string> GetOllamaResponseAsync(string userPrompt, string model);
}

public class OllamaService(Uri baseUri) : IOllamaService
{

    private readonly OllamaApiClient _ollamaApiClient = new OllamaApiClient(baseUri);

    public async Task<string> GetOllamaResponseAsync(string userPrompt, string model)
    {
        try
        {
            _ollamaApiClient.SelectedModel = model;
            var responseBuilder = new StringBuilder();
            await foreach (var stream in _ollamaApiClient.GenerateAsync(userPrompt))
            {
                responseBuilder.Append(stream.Response);
            }
            return responseBuilder.ToString();
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
