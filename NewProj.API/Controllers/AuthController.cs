using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NewProj.API.Models.DTO;
using NewProj.API.Repositories;

namespace NewProj.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }

        // POST: /api/Auth/Register
        [HttpPost]
        [Route("Register")]

        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            var idetityUser = new IdentityUser
            {
                UserName = registerRequestDto.UserName,
                Email = registerRequestDto.UserName
            };

            // Erstellung von neue User
            var identityResult = await userManager.CreateAsync(idetityUser, registerRequestDto.Password);

            if (identityResult.Succeeded)
            {
                // Add Roles to User
                // If Nutzer hat seine Role in Request gegeben hat, dann wir geben diese Role
                if (registerRequestDto.Roles != null && registerRequestDto.Roles.Any()) 
                {
                    identityResult = await userManager.AddToRolesAsync(idetityUser, registerRequestDto.Roles);

                    if (identityResult.Succeeded)
                    {
                        return Ok("User was registered. Bitte Login");
                    }
                }
                
            }
            return BadRequest("Etwas schief gegangen");
        }

        // POST: /api/AUth/Login
        [HttpPost]
        [Route("Login")]

        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            // Suchen die Nutzer in Database
            var user = await userManager.FindByEmailAsync(loginRequestDto.UserName);

            if (user != null) 
            {
                // Überprüfen die Password
                var checkPasswordResult = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);

                if (checkPasswordResult)
                {
                    // Get Role for this user
                    var roles = await userManager.GetRolesAsync(user);

                    if (roles != null)
                    {
                        // Erstellen den Token
                        var jwtToken = tokenRepository.CreateJWTToken(user, roles.ToList());

                        // Umwandeln zum DTO
                        var response = new LoginResponseDto
                        {
                            JwtToken = jwtToken
                        };

                        return Ok(response);
                    }
                }
            }

            return BadRequest("Falsche Username oder Password");

        }

    }
}
