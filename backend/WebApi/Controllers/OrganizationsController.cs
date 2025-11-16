using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Application.Organizations.Models;
using Application.Organizations.Commands.CreateOrganizationAnonymous;
using Application.Organizations.Commands.DeleteOrganization;
using Application.Organizations.Commands.ActivateOrganization;
using Application.Organizations.Queries.GetOrganizationList;
using Application.Organizations.Queries.GetFilteredOrganizationList;
using Application.Organizations.Queries.GetNotActiveOrganizationList;
using Application.Organizations.Queries.GetOrganizationById;
using WebAPI.Models;

namespace WebAPI.Controllers;

[Route("{version:apiVersion}/organizations"), ApiVersionNeutral]
public class OrganizationsController : BaseController
{
    [HttpPost("anonymous")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateOrganization(
        CreateOrganizationDto createOrganizationDto, CancellationToken cancellationToken)
    {
        var command = new CreateOrganizationAnonymousCommand(
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

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetOrganizationById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetOrganizationByIdQuery(id);
        
        var result = await Mediator.Send(query, cancellationToken);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        
        return BadRequest(new { result.Error.Description });
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<OrganizationListVm>> GetList(CancellationToken cancellationToken)
    {
        var query = new GetOrganizationListQuery();
        var result = await Mediator.Send(query, cancellationToken);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        throw new Exception(result.Error.Description);
    }

    [HttpGet("search")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<OrganizationListVm>> GetOrganizations(
        [FromQuery] SearchOrganizationDto searchOrganizationDto, CancellationToken cancellationToken)
    {
        var categoryIds = searchOrganizationDto.CategoryIds?
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .ToList();

        var query = new GetFilteredOrganizationListQuery(
            searchOrganizationDto.SearchString,
            categoryIds,
            searchOrganizationDto.City);
        
        var result = await Mediator.Send(query, cancellationToken);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        
        throw new Exception(result.Error.Description);
    }


    [Authorize("Admin"), HttpGet("notactive")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<OrganizationListVm>> GetNotPostedOrganization(
        CancellationToken cancellationToken)
    {
        var query = new GetNotActiveOrganizationListQuery();
        
        var result = await Mediator.Send(query, cancellationToken);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        
        return BadRequest(new { result.Error.Description });
    }

    [Authorize(Policy = "Admin"), HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteOrganizationCommand(id);
        
        var result = await Mediator.Send(command, cancellationToken);

        if (result.IsSuccess)
        {
            return NoContent();
        }
        
        return BadRequest(new { result.Error.Description });
    }

    [Authorize(Policy = "Admin"), HttpPut("activate/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> PostOrganization(Guid id, CancellationToken cancellationToken)
    {
        var command = new ActivateOrganizationCommand(id);
        
        var result = await Mediator.Send(command, cancellationToken);

        if (result.IsSuccess)
        {
            return NoContent();
        }
        
        return BadRequest(new { result.Error.Description });
    }
}