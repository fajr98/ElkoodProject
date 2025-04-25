// Copyright (c) Phinexes. All rights reserved.

namespace ElkoodProject.Controllers;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ElkoodProject.Application.Contracts.Users.Dtos;
using ElkoodProject.User.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

[ApiController]
[Route("api/v1.0/users")]
public sealed class UsersController : ControllerBase
{
    private readonly UserManager<IdentityUser> _usersService;
    private readonly IConfiguration _configuration;

    public UsersController(UserManager<IdentityUser> usersService, IConfiguration configuration)
    {
        _usersService = usersService;
        _configuration = configuration;
    }

    [HttpPost("guest/register")]
    public async Task<IActionResult> AddGuestAsync([FromBody] AddUserDto addUserDto)
    {
        var isExistsUser = await _usersService.FindByEmailAsync(addUserDto.Email);

        if (isExistsUser is not null)
        {
            return BadRequest("Email already in use");
        }

        var user = new IdentityUser
        {
            Email = addUserDto.Email,
            UserName = addUserDto.UserName,
            PhoneNumber = addUserDto.Mobile,
            SecurityStamp = Guid.NewGuid().ToString()
        };

        var userResult = await _usersService.CreateAsync(user, addUserDto.Password);

        if (!userResult.Succeeded)
        {
            return BadRequest(userResult.Errors);
        }

        await _usersService.AddToRoleAsync(user, StaticRoles.GUEST);

        var token = await _usersService.GenerateEmailConfirmationTokenAsync(user);

        var conResult = await _usersService.ConfirmEmailAsync(user, token);
        if (!conResult.Succeeded)
        {
            return BadRequest(userResult.Errors);
        }

        return Ok();
    }

    [HttpPost("owner/register")]
    [Authorize(Policy = "OwnerRolePolicy")]
    public async Task<IActionResult> AddOwnerAsync([FromBody] AddUserDto addUserDto)
    {
        var isExistsUser = await _usersService.FindByEmailAsync(addUserDto.Email);

        if (isExistsUser is not null)
        {
            return BadRequest("Email already in use");
        }

        var user = new IdentityUser
        {
            Email = addUserDto.Email,
            UserName = addUserDto.UserName,
            PhoneNumber = addUserDto.Mobile,
            SecurityStamp = Guid.NewGuid().ToString()
        };

        var userResult = await _usersService.CreateAsync(user, addUserDto.Password);

        if (!userResult.Succeeded)
        {
            return BadRequest(userResult.Errors);
        }

        await _usersService.AddToRoleAsync(user, StaticRoles.OWNER);

        var token = await _usersService.GenerateEmailConfirmationTokenAsync(user);

        var conResult = await _usersService.ConfirmEmailAsync(user, token);
        if (!conResult.Succeeded)
        {
            return BadRequest(userResult.Errors);
        }

        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var user = await _usersService.FindByEmailAsync(loginDto.Email);

        if (user is null)
        {
            return Unauthorized("Invalid credentials");
        }

        var pass = await _usersService.CheckPasswordAsync(user, loginDto.Password);

        if (!pass)
        {
            return BadRequest("Invalid password");
        }

        var userRole = await _usersService.GetRolesAsync(user);

        var authClaims = new List<Claim>
        {
            new ("Name",user.Email!),
            new ("Id",user.Id),
            new ("JWTID",Guid.NewGuid().ToString())
        };

        foreach (var role in userRole)
        {
            authClaims.Add(new Claim("Role", role));
        }

        var token = GenerateNewJWT(authClaims);
        var result = new
        {
            e = token
        };

        return Ok(JsonConvert.SerializeObject(result));
    }

    private string GenerateNewJWT(List<Claim> authClaims)
    {
        var authSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]!));

        var tokenObj = new JwtSecurityToken(
            issuer: _configuration["JWT:ValidIssure"],
            audience: _configuration["JWT:ValidAudience"],
            expires: DateTime.Now.AddDays(1),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSecret, SecurityAlgorithms.HmacSha256)
            );

        var token = new JwtSecurityTokenHandler().WriteToken(tokenObj);

        return token;
    }
}
