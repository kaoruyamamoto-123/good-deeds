
using Application.Organizations.Commands.UpdateOrganization;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Application.Users.Models;
using Application.Users.Queries.GetUserInfo;
using Application.Users.Commands.UpdatePassword;
using Application.Organizations.Models;
using Application.Organizations.Queries.GetOrganizationByUserId;
using Application.Users.Commands.CreateOrganization;
using WebAPI.Models;

namespace WebAPI.Controllers;

[Route("{version:apiVersion}/users"), ApiVersionNeutral]
public class UsersController : BaseController
{
    /// <summary>
    /// Gets user info
    /// </summary>
    /// <returns>Returns new user ID</returns>
    /// <response code="200">Success</response>
    /// <response code="401">If user is unauthorized (doesn't have jwt token)</response>
    [HttpGet("info"), Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<UserInfo>> GetUserInfo(CancellationToken cancellationToken)
    {
        var query = new GetUserInfoQuery(UserId);
        var result = await Mediator.Send(query, cancellationToken);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return BadRequest(new { Detail = result.Error.Description });
    }

    /// <summary>
    /// Changes user password
    /// </summary>
    /// <param name="updateUserPasswordDto">Current and new passwords</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <response code="204">Success</response>
    /// <response code="401">If user is unauthorized</response>
    [HttpPut("password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateUserPassword(
        UpdateUserPasswordDto updateUserPasswordDto, CancellationToken cancellationToken)
    {
        var updatePasswordCommand = new UpdatePasswordCommand(
            UserId: UserId,
            CurrentPassword: updateUserPasswordDto.CurrentPassword,
            NewPassword: updateUserPasswordDto.NewPassword);

        var result = await Mediator.Send(updatePasswordCommand, cancellationToken);

        if (result.IsSuccess)
        {
            return NoContent();
        }

        return BadRequest(new { Detail = result.Error.Description });
    }
    
    [Authorize, HttpGet("organizations")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<OrganizationVm>> GetUserOrganization(CancellationToken cancellationToken)
    {
        var query = new GetOrganizationByUserIdQuery(UserId);
        
        var result = await Mediator.Send(query, cancellationToken);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        
        return BadRequest(new { Detail = result.Error.Description });
    }

    [Authorize, HttpPost("organizations")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateOrganization(CreateOrganizationDto createOrganizationDto,
        CancellationToken cancellationToken)
    {
        var command = new CreateOrganizationCommand(
            UserId: UserId,
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

    [Authorize, HttpPut("organizations")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> UpdateUserOrganization(
        CreateOrganizationDto updateOrganizationDto, CancellationToken cancellationToken)
    {
        var command = new UpdateOrganizationCommand(
            UserId: UserId,
            Title: updateOrganizationDto.Title,
            Description: updateOrganizationDto.Description, 
            Phone: updateOrganizationDto.Phone, 
            City: updateOrganizationDto.City,
            Street: updateOrganizationDto.Street,
            Link: updateOrganizationDto.Link,
            LogoPath: updateOrganizationDto.LogoPath,
            CategoryIds: updateOrganizationDto.CategoryIds);   
        
        var result = await Mediator.Send(command, cancellationToken);

        if (result.IsSuccess)
        {
            return NoContent();
        }
        
        return BadRequest(new { Detail = result.Error.Description });
    }
}