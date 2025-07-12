using Hrms.Core.Abstractions.Managers;
using Hrms.Core.Models;
using Hrms.Core.Models.Account;
using Hrms.Core.Models.Employee;
using Hrms.Core.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Hrms.Api.Controllers
{
    [Route("user")]
    [Produces("application/json")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IUserManager _userManager;
        private readonly IEmployeeManager _employeeManager;
        private readonly ICompanyManager _companyManager;

        public UserController(ILogger<UserController> logger,
            IUserManager userManager,
            IEmployeeManager employeeManager,
            ICompanyManager companyManager)
        {
            _logger = logger;
            _userManager = userManager;
            _employeeManager = employeeManager;
            _companyManager = companyManager;
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }

            var user = await _userManager.GetByEmailAsync(model.Email);

            if (user == null
                || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return BadRequest("Invalid email or password.");
            }

            if (user.Status == Constants.RecordStatus.Inactive)
            {
                return BadRequest("Your account is inactive. Please contact to administrator.");
            }

            var roles = await _userManager.GetRolesAsync(user);

            try
            {
                var token = GenerateBearerToken(user, roles);

                return Ok(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login");

                return BadRequest(ex.Message);
            }
        }

        [HttpPost("ad-login")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AdLogin([FromQuery] string accessToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }

            var graphUser = await GetGraphUserAsync(accessToken);

            if (graphUser == null)
            {
                return BadRequest("User not found");
            }

            try
            {
                var user = await _userManager.GetByEmailAsync(graphUser.Mail);

                if (user == null)
                {
                    await _employeeManager.AddAsync(new EmployeeModel
                    {
                        FirstName = graphUser.GivenName,
                        LastName = graphUser.Surname,
                        Email = graphUser.Mail,
                        Phone = graphUser.MobilePhone ?? string.Empty,
                        CompanyId = 1
                    });

                    user = await _userManager.GetByEmailAsync(graphUser.Mail);
                }

                var roles = await _userManager.GetRolesAsync(user);
                var token = GenerateBearerToken(user, roles);

                return Ok(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login");

                return BadRequest(ex.Message);
            }
        }

        [HttpGet("list")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetList()
        {
            return Ok(await _userManager.GetListAsync());
        }

        [HttpPut("change-password")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }
            var user = await _userManager.FindAsync(User.GetUserId());

            if (user == null
            || !await _userManager.CheckPasswordAsync(user, model.CurrentPassword))
            {
                return BadRequest("Invalid current password.");
            }
            try
            {
                await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.Password);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("reset-password")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ResetPassword([FromBody]ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }

            var userId = await _employeeManager.GetUserIdAsync(model.EmployeeId);
            var user = await _userManager.FindAsync(userId);
            try
            {
                await _userManager.ResetPasswordAsync(user, model.Password, User.GetUserId());
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #region private helpers
        private string GenerateBearerToken(Core.Entities.User user, List<string> roles)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(AppSettings.Jwt.Secret);

            var companyId = _companyManager.GetIdByUserIdAsync(user.Id).Result;

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.GivenName, $"{user.FirstName} {user.LastName}".Trim()),
                        new Claim(Constants.ClaimType.CompanyId, companyId.ToString())
                    }),
                Audience = AppSettings.Jwt.Audience,
                Issuer = AppSettings.Jwt.Issuer,
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials =
                        new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            foreach (var role in roles)
            {
                tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, role));
            }

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        private async Task<GraphUserModel> GetGraphUserAsync(string token)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, Constants.GraphUrl);
            request.Headers.Add("Authorization", token);
            var response = await client.SendAsync(request);
            var user = JsonSerializer.Deserialize<GraphUserModel>(await response.Content.ReadAsStringAsync());
            return user;
        }
        #endregion
    }
}
