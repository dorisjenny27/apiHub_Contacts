using AutoMapper;
using ContactHub.Helpers;
using ContactHub.Model.DTOs;
using ContactHub.Model.Entity;
using ContactHub.Services;
using ContactHub.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ContactHub.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ContactAPIController : ControllerBase
    {
        private readonly IContactService _contactService;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly IPhotoManager _photoManager;
        private readonly IRepository _repository;
        private readonly IAuthService _authService;

        public ContactAPIController(IContactService contactService, UserManager<User> userManager,
            IMapper mapper, RoleManager<IdentityRole> roleManager, IPhotoManager photoManager,
            IRepository repository, IAuthService authService)
        {
            _contactService = contactService;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _photoManager = photoManager;
            _repository = repository;
            _authService = authService;
        }

        [HttpGet("Id")]
        public async Task <IActionResult> UserById(string Id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(Id);
                if (user == null)
                {
                    return NotFound($"No record found for user with id: {Id}");
                }
                var userToReturn = _mapper.Map<UserToReturnDTO>(user);
                
                return Ok(userToReturn);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("all")]
        public IActionResult GetAllUsers(int page, int perPage)
        {
            try
            {
                var users = _userManager.Users;
                var usersToReturnList = new List<UserToReturnDTO>();
                foreach (var user in users)
                {
                    usersToReturnList.Add(_mapper.Map<UserToReturnDTO>(user));
                }
                var paginated = UtilityMethods.Paginate<UserToReturnDTO>(usersToReturnList, page, perPage);
                return Ok(paginated);

            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Update")]
        public async Task <IActionResult> UpdateUser(string Id, [FromBody] UserUpdateDTO model)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(Id);
                if (user == null)
                    return BadRequest($"No record found for user with Id: {Id}");

                var vResult = await _authService.ValidateLoggedInUser(User, user.Id);
                if (vResult["Code"] == "400")
                    return Unauthorized(vResult["Message"]);
                //Update the appUser object with the userToUpdate DTO using automapper

                _mapper.Map<UserUpdateDTO, User>(model, user);


                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    var errList = "";
                    foreach (var err in updateResult.Errors)
                    {
                        errList += err.Description + ",\n";
                    }
                    return BadRequest(errList);
                }
                return Ok(_mapper.Map<UserToReturnDTO>(user));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("Delete")]
        [Authorize(Roles = "regular")]
        public async Task<IActionResult> DeleteUser(string Id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(Id);
                if (user == null)
                    return BadRequest($"No record found for user with Id: {Id}");

                var delResult = await _userManager.DeleteAsync(user);
                if (!delResult.Succeeded)
                {
                    var errList = "";
                    foreach (var err in delResult.Errors)
                    {
                        errList += err.Description + ",\n";
                    }
                    return BadRequest(errList);
                }
                return Ok("Deleted successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("Updatephoto")]
        public async Task<IActionResult> AddPhoto(string Id, [FromForm] UserToUploadPhotoDTO model)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(Id);
                if (user == null)
                    return BadRequest($"No record found for user with Id: {Id}");

                var uploadResult = await _photoManager.UploadImage(model.Photo);
                if (!uploadResult.IsSuccess)
                {
                    return BadRequest(uploadResult.Message);
                }
                //Update photo details on user in DB
                user.PhotoUrl = uploadResult.PhotoUrl;
                user.PhotoId = uploadResult.PublicId;
                await _userManager.UpdateAsync(user);

          return Ok(_mapper.Map<UserToReturnDTO>(user));

    }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete-photo")]
        public async Task<IActionResult> DeletePhoto(string Id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(Id);
                if (user == null)
                    return BadRequest($"No record found for user with Id: {Id}");

                var deleteResult = await _photoManager.DeleteImage(user.PhotoId);
                if (!deleteResult)
                {
                    return BadRequest("Failed to delete photo");
                }
                user.PhotoUrl = null;
                user.PhotoId = null;
                await _userManager.UpdateAsync(user);

                return Ok(_mapper.Map<UserToReturnDTO>(user));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Search")]
        [Authorize(Roles = "regular, admin")]
        public IActionResult GetAll(int page, int perPage, string searchTerm)
        {
            try
            {
                var users = _userManager.Users;

                var searchResult = users.Where(x => x.Email == searchTerm ||
                                               x.UserName == searchTerm ||
                                               x.Id == searchTerm).ToList();

                var usersToReturnList = new List<UserToReturnDTO>();
                var paginated = UtilityMethods.Paginate<User>(searchResult, page, perPage);
                foreach (var user in paginated)
                {
                    usersToReturnList.Add(_mapper.Map<UserToReturnDTO>(user));
                }
                return Ok(paginated);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
