using Microsoft.Extensions.Logging;
using System.Text.Json.Nodes;

using Domain.ValueObjects;
using Application.Shared.Clients;
using Application.Shared.Data;

namespace Infrastructure.Map;

public class MapClient : IMapClient, IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<MapClient> _logger;
    
    public MapClient(ILogger<MapClient> logger)
    {
        _logger = logger;
        _httpClient = new HttpClient();
    }

    public async Task<Result<Coordinates>> GetCoordinates(string address, CancellationToken cancellationToken = default)
    {
        var escapedAddress=  Uri.EscapeDataString(address);
        string url =
            $"https://geocode-maps.yandex.ru/v1/?apikey=6eff2772-bb79-45df-b43d-2735c0e4aab9&geocode={escapedAddress}&format=json";
        
        var result = await _httpClient.GetAsync(url, cancellationToken).ConfigureAwait(false);

        if (!result.IsSuccessStatusCode)
        {
            _logger.LogError("Error getting coordinates for {address}, error: {error}", address, result.StatusCode);
            return Result<Coordinates>.Failure(
                new Error(nameof(GetCoordinates), "Geocode could not be retrieved"));
        }
        
        var jsonString  = await result.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
       
        _logger.LogInformation("Received json: {jsonString}", jsonString);
        
        var rootNode = JsonNode.Parse(jsonString);
        
        if (rootNode == null)
        {
            _logger.LogError("Error on parsing json while getting coordinates for {address}", address);
            return Result<Coordinates>.Failure(
                new Error(nameof(GetCoordinates), "Geocode could not be retrieved"));
        }

        var posJson =
            rootNode["response"]!["GeoObjectCollection"]!["featureMember"]![0]!["GeoObject"]!["Point"]!["pos"];

        var point = posJson?.GetValue<string>();

        if (point == null)
        {
            _logger.LogError("Error on parsing json while getting coordinates for {address}", address);
            return Result<Coordinates>.Failure(
                new Error(nameof(GetCoordinates), "Geocode could not be retrieved"));
        }
        
        _logger.LogInformation("Point: {point}", point);
        
        var pointArray = point.Split(" ").Select(double.Parse).ToArray();
        var coordinates = new Coordinates(longitude:  pointArray[0], latitude: pointArray[1]);
        
        return Result<Coordinates>.Success(coordinates);
    }

    public void Dispose()
    {
        _httpClient.Dispose();
        GC.SuppressFinalize(this);
    }
}