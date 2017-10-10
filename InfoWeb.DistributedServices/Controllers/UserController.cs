using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using InfoWeb.Domain.Interfaces;
using InfoWeb.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InfoWeb.DistributedServices.Controllers
{
    [Route("api/[controller]")]
    public class UserController: Controller
    {
        private readonly IUserRepository userRepository;
        public UserController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
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

        [HttpGet()]
        public IEnumerable<User> List()
        {
            IEnumerable<User> result = userRepository.Users.Include(u => u.Role).ToList();

            return result;
        }
    }
}
