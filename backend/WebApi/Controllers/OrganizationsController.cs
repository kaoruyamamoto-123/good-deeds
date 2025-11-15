using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

using Application.Organizations.Models;
using Application.Organizations.Queries.GetOrganizationList;
using Application.Organizations.Commands.CreateOrganization;

using WebAPI.Models;

namespace WebAPI.Controllers;

[Route("{version:apiVersion}/organization"), ApiVersionNeutral]
public class OrganizationsController : BaseController
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateOrganization(
        CreateOrganizationDto createOrganizationDto, CancellationToken cancellationToken)
    {
        var command = new CreateOrganizationCommand(
            Title: createOrganizationDto.Title,
            Description: createOrganizationDto.Description, 
            Phone: createOrganizationDto.Phone, 
            City: createOrganizationDto.City,
            Street: createOrganizationDto.Street,
            Link: createOrganizationDto.Link,
            LogoPath: createOrganizationDto.LogoPath,
            CategoryIds: createOrganizationDto.CategoryIds);
        
        var result = await Mediator.Send(command, cancellationToken);

        if (result.IsSuccess)
        {
            return Created();
        }
        
        return BadRequest(new { Detail = result.Error.Description });
    }
    
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<OrganizationListVm>> GetList(CancellationToken cancellationToken = default)
    {
        var query = new GetOrganizationListQuery();
        var result = await Mediator.Send(query, cancellationToken);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        throw new Exception(result.Error.Description);
    }
}