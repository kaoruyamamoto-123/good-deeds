using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Models;

public class SearchOrganizationDto
{
    [FromQuery(Name = "searchString")] public string? SearchString { get; set; }
    [FromQuery(Name = "categoryIds")] public string? CategoryIds { get; set; }
    [FromQuery(Name = "city")] public string? City { get; set; }
}