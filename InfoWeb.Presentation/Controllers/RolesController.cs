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
                if (isRoleTaken(role.Name, -1) == false)
                {
                    roleRepository.Add(role);
                    try
                    {
                        unitOfWork.Commit();
                        return Ok("Rol <strong>" + role.Name + "</strong> creado correctamente.");
                    }
                    catch (Exception e)
                    {
                        return BadRequest(new ValidationResult("Error interno del servidor."));
                    }
                }
                else
                {
                    return BadRequest(new ValidationResult("El rol <strong>" + role.Name + "</strong> ya existe."));
                }
            }
          
            return BadRequest(new ValidationResult("Error en los datos de entrada."));
        }

        [HttpPut()]
        public IActionResult Update([FromBody]Role role)
        {
            if (ModelState.IsValid)
            {
                var targetRole = roleRepository.GetById(role.Id);
                if (targetRole != null)
                {
                    if (isRoleTaken(role.Name, role.Id) == false)
                    {
                        targetRole.Name = role.Name;
                        roleRepository.Update(targetRole);
                        try
                        {
                            unitOfWork.Commit();
                            return Ok("Rol <strong>" + role.Name + "</strong> actualizado correctamente.");
                        }
                        catch (Exception e)
                        {
                            return BadRequest(new ValidationResult("Error interno del servidor."));
                        }
                    }
                    else
                    {
                        return BadRequest(new ValidationResult("El rol < strong > " + role.Name + " </ strong > ya existe."));
                    }
                }
                else
                {
                    return BadRequest(new ValidationResult("Rol no encontrado."));
                }
            }
           return BadRequest(new ValidationResult("Error en los datos de entrada."));
        }

        [HttpDelete("{id}")]
        public IActionResult Remove(int id)
        {
            if(id > 0)
            {
                Role targetRole = roleRepository.GetById(id);

                if (targetRole != null)
                {
                    if (roleRepository.CanItemBeRemoved(id))
                    {
                        roleRepository.Remove(targetRole);

                        try
                        {
                            unitOfWork.Commit();
                            return Ok("Rol <strong>" + targetRole.Name + "</strong> eliminado correctamente.");
                        }
                        catch (Exception e)
                        {
                            return BadRequest(new ValidationResult("Error interno del servidor."));
                        }
                    }
                    else
                    {
                        return BadRequest(new ValidationResult("El usuario <strong>" + targetRole.Name + "</strong> no puede ser eliminado. Tiene usuarios asociadas a él."));
                    }
                }

            }
    
            return BadRequest(new ValidationResult("Error en los datos de entrada."));
        }

        private bool isRoleTaken(string userName, int id)
        {
            var user = this.roleRepository.GetByName(userName);

            if (user == null || (user != null && id > 0 && user.Id == id))
            {
                return false;
            }

            return true;
        }
    }
}
