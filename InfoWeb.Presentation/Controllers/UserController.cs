using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using InfoWeb.Domain.Interfaces;
using InfoWeb.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using InfoWeb.Presentation.InputModels;
using System.Net;
using InfoWeb.Presentation.Models;

namespace InfoWeb.Presentation.Controllers
{
    [Route("api/[controller]")]
    public class UserController: Controller
    {
        private readonly IUserRepository userRepository;
        private readonly IRoleRepository roleRepository;
        private readonly IUnitOfWork unitOfWork;

        public UserController(IUserRepository userRepository, IRoleRepository roleRepository,
                              IUnitOfWork unitOfWork)
        {
            this.userRepository = userRepository;
            this.roleRepository = roleRepository;
            this.unitOfWork = unitOfWork;
        }

        [HttpPost("login")]
        public User Login([FromBody]User user)
        {
            var loggedUser = userRepository.Users.Where(u => u.Name == user.Name && u.Password == user.Password).Include(u => u.Role).FirstOrDefault();


            if(loggedUser == null)
            {
                 Response.StatusCode = 401;
            }

            return loggedUser;

            //return loggedUser;

        }

        [HttpGet("list")]
        public IEnumerable<User> List()
        {
            IEnumerable<User> result = userRepository.Users.Include(u => u.Role).ToList();

            return result;
        }

        [HttpGet("search/{searchValue?}")]
        public IEnumerable<User> GetSearchUser([FromRoute]string searchValue = "")
        {
            return userRepository.getUserSearch(searchValue);
        }


        [HttpGet("listAdmin")]
        public IEnumerable<User> ListAdmin()
        {
            IEnumerable<User> result = userRepository.Users.Include(u => u.Role).Where(u => u.Role.Name == "TAM" || u.Role.Name == "PM").ToList();

            return result;
        }

        [HttpGet("listTechnicians")]
        public IEnumerable<User> GetTechnicians()
        {
            return userRepository.getUsersByRoleName("TEC"); ;
        }

        [HttpGet("listOpManagers")]
        public IEnumerable<User> GetOpManagers()
        {
            return userRepository.getUsersByRoleName("OM");
        }

        [HttpGet("{id}")]
        public User GetUserById([FromRoute]int id)
        {
            return userRepository.GetById(id);
        }

        [HttpPost]
        public IActionResult Add([FromBody]UserInputModel userData)
        {
            if(ModelState.IsValid)
            {
                var role = roleRepository.GetById(userData.Role.Id);
                if(role != null)
                {
                    if (isUsernameTaken(userData.Name, -1) == false)
                    {
                        var user = new User()
                        {
                            Name = userData.Name,
                            Password = userData.Password,
                            Role = userData.Role
                        };
                        userRepository.Add(user);

                        try
                        {
                            unitOfWork.Commit();
                            return Ok("Usuario <strong>" + user.Name + "</strong> creado correctamente.");
                        }
                        catch (Exception e)
                        {
                            return BadRequest(new ValidationResult("Error interno del servidor."));
                        }
                    }
                    else
                    {
                        return BadRequest(new ValidationResult("El usuario " + userData.Name + " ya existe."));
                    }
                }
            }

            return BadRequest(new ValidationResult("Error en los datos de entrada."));
        }

        [HttpPut]
        public IActionResult Update([FromBody]UserInputModel userData)
        {
            if (ModelState.IsValid)
            {
                var role = roleRepository.GetById(userData.Role.Id);
                var user = userRepository.GetById(userData.Id);

                if (user != null && role != null)
                {
                    if (isUsernameTaken(userData.Name.ToLower(), userData.Id) == false)
                    {
                        user.Name = userData.Name;
                        user.Role = role;
                        user.Password = userData.Password;

                        userRepository.Update(user);
                        try
                        {
                            unitOfWork.Commit();
                            return Ok("Usuario <strong>" + user.Name + "</strong> actualizado correctamente.");
                        }
                        catch (Exception e)
                        {
                            return BadRequest(new ValidationResult("Error interno del servidor."));
                        }
                    }
                    else
                    {
                        return BadRequest(new ValidationResult("El usuario " + userData.Name + " ya existe."));
                    }
                }
            }
            return BadRequest(new ValidationResult("Error en los datos de entrada."));            
        }

        [HttpDelete("{id}")]
        public IActionResult Remove([FromRoute]int id)
        {
            var user = userRepository.GetById(id);
            if(user != null)
            {
                userRepository.Remove(user);
                try
                {
                    unitOfWork.Commit();
                }
                 catch(Exception e)
                {
                    return BadRequest(new ValidationResult("Error interno del servidor."));
                }
            }
            else
            {
                return BadRequest(new ValidationResult("Error en los datos de entrada."));
            }

            return Ok("Usuario <strong>" + user.Name + "</strong> eliminado correctamente.");
        }

        private bool isUsernameTaken(string userName, int id)
        {
            var user = this.userRepository.GetByName(userName);

            if (user == null || (user != null && id > 0 && user.Id == id))
            {
                return false;
            }

            return true;
        }

       /* private bool ValidateUserInput(UserInputModel userData)
        {
            return (userData != null 
                && userData.Password == userData.PasswordConfirmation
                && userData.Password.Trim().Length > 0
                && userData.Name.Trim().Length > 0
                && userData.Role != null);
        }*/

    }
}
