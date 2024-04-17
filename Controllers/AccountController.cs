using AutoMapper;
using ContactHub.Model;
using ContactHub.Model.DTOs;
using ContactHub.Model.Entity;
using ContactHub.Services;
using ContactHub.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ContactHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IContactService _contactService;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        public AccountController(IContactService contactService, UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager, IAuthService authService, IMapper mapper)
        {
            _contactService = contactService;
            _userManager = userManager;
            _roleManager = roleManager;
            _authService = authService;
            _mapper = mapper;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> CreateUser([FromBody] UserToAddDTO model)
        {
            try
            {
                // Check if email already exists
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                    return BadRequest("Email already exists!");

                var appUser = new User
                {
                    Email = model.Email,
                    UserName = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Address = model.Address

                };
                var addResult = await _userManager.CreateAsync(appUser, model.Password);
                if (!addResult.Succeeded)
                {
                    var errList = "";
                    foreach (var err in addResult.Errors)
                    {
                        errList += err.Description + ",\n";
                    }
                    return BadRequest(errList);
                }

                // add role to user
                await _userManager.AddToRoleAsync(appUser, "regular");
                return Ok($"User added with Id: {appUser.Id}");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("LogIn")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            try
            {
                var loginResult = await _authService.Login(model.Email, model.Password);
                return Ok(loginResult);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
