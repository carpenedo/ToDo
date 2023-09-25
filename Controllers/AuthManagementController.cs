using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

[Route("api/[controller]")]
[ApiController]
public class AuthManagementController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly JwtConfig _jwtConfig;

    public AuthManagementController(UserManager<IdentityUser> userManager, IOptionsMonitor<JwtConfig> optionsMonitor)
    {
        _userManager = userManager;
        _jwtConfig = optionsMonitor.CurrentValue;
    }

    [HttpPost]
    [Route("Register")]
    public async Task<IActionResult> Register([FromBody] UserRegistrationDto user)
    {
        if (ModelState.IsValid)
        {
            var existingUser = await _userManager.FindByEmailAsync(user.Email);

            if (existingUser != null)
            {
                return BadRequest(new RegistrationResponse()
                {
                    Errors = new List<String>(){
                "Email already in use"
                },
                    Success = false
                });
            }

            var newUser = new IdentityUser() { Email = user.Email, UserName = user.Email};
        }

        return BadRequest(new RegistrationResponse()
        {
            Errors = new List<String>(){
                "Invalid payload"
                },
            Success = false
        });
    }
}