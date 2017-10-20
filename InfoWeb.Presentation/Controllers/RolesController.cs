using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InfoWeb.Domain.Entities;
using InfoWeb.Domain.Interfaces;
using System.Net;

namespace InfoWeb.DistributedServices.Controllers
{
    [Route("api/Roles")]
    public class RolesController: Controller
    {
        private readonly IRoleRepository roleRepository;
        public RolesController(IRoleRepository roleRepository)
        {
            this.roleRepository = roleRepository;
        }

        [HttpGet]
        public IEnumerable<Role> List()
        {
            return roleRepository.GetAll();
        }

        [HttpGet("{roleId}")]
        public Role GetRole(int roleId)
        {
            return roleRepository.GetById(roleId);
        }

        [HttpPost]
        public void Create([FromBody]Role role)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    roleRepository.Add(role);
                    Response.StatusCode = (int)HttpStatusCode.Created;
                }
                catch (Exception e)
                {
                    Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                }
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }                      
        }

        [HttpPut()]
        public void Update([FromBody]Role role)
        {
            if (ModelState.IsValid)
            {
                var targetRole = roleRepository.GetById(role.Id);
                if(targetRole != null)
                {
                    targetRole.Name = role.Name;
                    try
                    {
                        roleRepository.Update(targetRole);
                        Response.StatusCode = (int)HttpStatusCode.OK;
                    }
                    catch (Exception e)
                    {
                        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
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
        public void Remove(int id)
        {
            if(id > 0)
            {
                Role targetRole = roleRepository.GetById(id);

                try {
                    roleRepository.Remove(targetRole);
                    Response.StatusCode = (int)HttpStatusCode.OK;
                }
                catch(Exception e)
                {
                    Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                }
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }            
        }
    }
}
