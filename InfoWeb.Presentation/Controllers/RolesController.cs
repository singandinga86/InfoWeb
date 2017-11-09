using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InfoWeb.Domain.Entities;
using InfoWeb.Domain.Interfaces;
using System.Net;
using InfoWeb.Presentation.Models;

namespace InfoWeb.Presentation.Controllers
{
    [Route("api/Roles")]
    public class RolesController: Controller
    {
        private readonly IRoleRepository roleRepository;
        private readonly IUnitOfWork unitOfWork;
        public RolesController(IRoleRepository roleRepository, IUnitOfWork unitOfWork)
        {
            this.roleRepository = roleRepository;
            this.unitOfWork = unitOfWork;
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
        public IActionResult Create([FromBody]Role role)
        {
            if(ModelState.IsValid)
            {
                roleRepository.Add(role);
                try
                {
                    unitOfWork.Commit();
                }
                catch (Exception e)
                {
                    return BadRequest(new ValidationResult("Error interno del servidor."));
                }
            }
            else
            {
                return BadRequest(new ValidationResult("Error en los datos de entrada."));
            }

            return Ok();
        }

        [HttpPut()]
        public IActionResult Update([FromBody]Role role)
        {
            if (ModelState.IsValid)
            {
                var targetRole = roleRepository.GetById(role.Id);
                if (targetRole != null)
                {
                    targetRole.Name = role.Name;
                    roleRepository.Update(targetRole);
                    try
                    {
                        unitOfWork.Commit();
                    }
                    catch (Exception e)
                    {
                        return BadRequest(new ValidationResult("Error interno del servidor."));
                    }
                }
                else
                {
                    return BadRequest(new ValidationResult("Rol no encontrado."));
                }
            }
            else
            {
                return BadRequest(new ValidationResult("Error en los datos de entrada."));
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Remove(int id)
        {
            if(id > 0)
            {
                Role targetRole = roleRepository.GetById(id);
                roleRepository.Remove(targetRole);

                try {
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

            return Ok();
        }
    }
}
