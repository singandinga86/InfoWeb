﻿using System;
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

namespace InfoWeb.DistributedServices.Controllers
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


        [HttpGet("listAdmin")]
        public IEnumerable<User> ListAdmin()
        {
            IEnumerable<User> result = userRepository.Users.Include(u => u.Role).Where(u => u.Role.Name == "TAM" || u.Role.Name == "PM").ToList();

            return result;
        }

        [HttpGet("listTechnicians")]
        public IEnumerable<User> GetTechnicians()
        {
            return userRepository.getUsersByRoleName("TEC");
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
        public void Add([FromBody]UserInputModel userData)
        {
            if(ModelState.IsValid)
            {
                var role = roleRepository.GetById(userData.Role.Id);
                if(role != null)
                {
                    var user = new User()
                    {
                        Name = userData.Name,
                        Password = userData.Password,
                        Role = userData.Role
                    };
                    userRepository.Add(user);

                    try {
                        unitOfWork.Commit();    
                        Response.StatusCode = (int)HttpStatusCode.Created;
                    }
                    catch(Exception e)
                    {
                        Response.StatusCode = (int)HttpStatusCode.Conflict;
                    }
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                }
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
        }

        [HttpPut]
        public void Update([FromBody]UserInputModel userData)
        {
            if (ModelState.IsValid)
            {
                var role = roleRepository.GetById(userData.Role.Id);
                var user = userRepository.GetById(userData.Id);

                if(user != null && role != null)
                {
                    user.Name = userData.Name;
                    user.Role = role;
                    user.Password = userData.Password;

                    userRepository.Update(user);
                    try
                    {
                        unitOfWork.Commit();
                    }
                    catch(Exception e)
                    {
                        Response.StatusCode = (int)HttpStatusCode.Conflict;
                    }
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                }
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
        }

        [HttpDelete("{id}")]
        public void Remove([FromRoute]int id)
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
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                }
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
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
