using AutoMapper;
using ContactHub.Mappers;
using ContactHub.Model;
using ContactHub.Model.DTOs;
using ContactHub.Model.Entity;
using Microsoft.AspNetCore.Identity;

namespace ContactHub.Services
{
    public class ContactService : IContactService
    {
        private readonly IMapper _autoMapper;
        private readonly UserManager<User> _userManager;        
        public ContactService(IMapper autoMapper)
        {
            _autoMapper = autoMapper;
        }

        public UserToAddDTO CreateUser(UserToAddDTO newUser)
        {
            throw new NotImplementedException();
        }

        public bool DeleteUser(string Id)
        {
            throw new NotImplementedException();
        }

        public List<UserToAddDTO> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public UserToAddDTO GetUserById(string Id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RegisterUser(LoginUser user)
        {
            throw new NotImplementedException();
        }

        public UserToAddDTO UpdateUser(UserToAddDTO updateUser)
        {
            throw new NotImplementedException();
        }

        /*      private static List<User> users = new List<User>
              {
                  new User() { Id = "1", FirstName = "Doris", LastName = "Okereke", Phone = "08144445588", Address = "Royal Pine Estate",
                      Notes = "Ikoyi Client", CreatedDate = DateTime.Now },
                  new User() { Id = "2", FirstName = "Maximillian", LastName = "Ejemba", Phone = "08187626932", Address = "Imperial Oak Estate",
                      Notes = "Owerri Client", CreatedDate = DateTime.Now },
                  new User() { Id = "3", FirstName = "Samuel", LastName = "Ugboro", Phone = "07030048245", Address = "Victoria Crest Estate",
                      Notes = "Iyana Ipaja Client", CreatedDate = DateTime.Now }
              };*/
        /*  public List<UserDTO> GetAllUsers()
          {
              return _myMapper.ToUserDTOList(users);
              //return _autoMapper.Map<UserDTO>(user);
          }*/

        /* public UserDTO GetUserById(string Id)
         {
             var user = users.FirstOrDefault(user => user.Id == Id);
             if (user != null)
             {
                return _myMapper.ToUserDTO(user);
                //return _autoMapper.Map<UserDTO>(user);
             }
             return null;
         }
 */
        /* public UserDTO CreateUser(UserDTO newUser)
         {
             var user = new User();
             user.Id = newUser.Id;
             user.FirstName = newUser.FirstName;
             user.LastName = newUser.LastName;
             user.Phone = newUser.PhoneNumber;
             user.Address = newUser.Address;
             user.Notes = newUser.Notes;
             user.CreatedDate = newUser.CreatedDate;
             //user.Photo = newUser.Photo;
             users.Add(user);

             return _myMapper.ToUserDTO(user);
             //return _autoMapper.Map<UserDTO>(user);
         }*/

        /*public UserDTO UpdateUser(UserDTO updateUser)
        {
            var user = users.FirstOrDefault(user => user.Id == updateUser.Id);
            if (user != null)
            {
                user.FirstName = updateUser.FirstName;
                user.LastName = updateUser.LastName;
                user.Phone = updateUser.PhoneNumber;
                user.Address = updateUser.Address;
                user.Notes = updateUser.Notes;
                user.CreatedDate = updateUser.CreatedDate;
                //user.Photo = updateUser.Photo;

                return _myMapper.ToUserDTO(user);
                //return _autoMapper.Map<UserDTO>(user);
            }
            return null;
        }*/

        /* public bool DeleteUser(string Id)
         {
             var user = users.FirstOrDefault(user => user.Id == Id);
             if (user == null)
             {
                 return false;
             }
             users.Remove(user);
             return true;
         }*/

        /* public async Task<bool> RegisterUser(LoginUser user)
         {
             var identityUser = new User
             {
                 //UserName = user.Username,
                 //Email = user.Email,
             };

             var result = await _userManager.CreateAsync(identityUser, user.Password);
             return result.Succeeded;

         }*/
    }
}
