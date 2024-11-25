using AutoMapper;
using NotebookLM.Application.Contracts.Persistence;
using NotebookLM.Application.DTOs.Identity;
using NotebookLM.Domain.Entities;
using NotebookLM.Domain.Enums;
using NotebookLM.Domain.Helpers;
using NotebookLM.Domain.Utilities;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NotebookLM.Persistence.Identity;

internal class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly JwtConfiguration _jwtConfiguration;
    private readonly IMapper _mapper;

    public AuthService(
        SignInManager<User> signInManager,
        UserManager<User> userManager,
        JwtConfiguration jwtConfiguration,
        IMapper mapper)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _jwtConfiguration = jwtConfiguration;
        _mapper = mapper;
    }


    public async Task<(IdentityResult, string?)> LoginAsync(LoginDto login)
    {
        var emailExist = await _userManager.FindByEmailAsync(login.Email);

        if (emailExist is null || !(await _userManager.CheckPasswordAsync(emailExist, login.Password)))
            return (IdentityResult.Failed(new IdentityError()
            {
                Description = "Invalid email and/or password"
            }), null);

        var signInResult = await _signInManager.PasswordSignInAsync(emailExist, login.Password, false, false);

        if (!signInResult.Succeeded)
            return (IdentityResult.Failed(new IdentityError()
            {
                Description = "Error to attempt sign-in"
            }), null);

        var roleUser = await _userManager.GetRolesAsync(emailExist);

        return (IdentityResult.Success, GetToken(emailExist, roleUser.First()));
    }

    public async Task<IdentityResult> RegisterAsync(RegisterDto register)
    {
        var userExists = await _userManager.FindByEmailAsync(register.Email);

        if (userExists is not null)
            return IdentityResult.Failed(new IdentityError()
            {
                Description = "Email already in use"
            });

        //var userToRegister = new User
        //{
        //    Name = register.Name,
        //    Email = register.Email,
        //    LastName = register.LastName,
        //    UserName = register.Email
        //};
        var userToRegister = _mapper.Map<User>(register);

        var createUserResult = await _userManager.CreateAsync(userToRegister, register.Password);

        if (!createUserResult.Succeeded)
        {
            return IdentityResult.Failed(new IdentityError()
            {
                Description = string.Join(", ", createUserResult.Errors.Select(e => e.Description))
            });
        }

        var addRoleResult = await AddRoleToUser(userToRegister, Role.User);

        if (!addRoleResult.Succeeded)
            return addRoleResult;

        
        return IdentityResult.Success;
    }

    public async Task<IdentityResult> AddRoleToUser(User user, Role role)
    {
        var addRoleResult = await _userManager.AddToRoleAsync(user, role.ToStringEnum());
        if (!addRoleResult.Succeeded)
        {
            var deleteUserResult = await _userManager.DeleteAsync(user);
            if (!deleteUserResult.Succeeded)
                return IdentityResult.Failed(new IdentityError()
                {
                    Description = string.Join(", ",
                        deleteUserResult.Errors.Select(e => e.Description),
                        addRoleResult.Errors.Select(e => e.Description))
                });

            return IdentityResult.Failed(new IdentityError()
            {
                Description = string.Join(", ", addRoleResult.Errors.Select(e => e.Description))
            });
        }
        return IdentityResult.Success;
    }

    public string GetToken(User user, string role)
    {
        byte[] key = Encoding.ASCII.GetBytes(_jwtConfiguration.Key);

        var jwtToken = new JwtSecurityToken(
            issuer: _jwtConfiguration.Issuer,
            audience: _jwtConfiguration.Audience,
            claims: GetClaims(user, role),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256),
            expires: DateTime.UtcNow.AddHours(_jwtConfiguration.DurationInHours)
        );
        return new JwtSecurityTokenHandler().WriteToken(jwtToken);
    }

    public List<Claim> GetClaims(User user, string role)
    {
        var claimList = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim("name", user.Name),
            new Claim("lastName", user.LastName),
            new Claim("email", user.Email),
            new Claim(ClaimTypes.Role,role)
        };
        return claimList;
    }
}
