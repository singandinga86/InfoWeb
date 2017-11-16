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
    [Route("api/Client")]
    public class ClientController : Controller
    {
        private IClientRepository clientRepository;
        private IUnitOfWork unitOfwork;
        public ClientController(IClientRepository clientRepository, IUnitOfWork unitOfwork)
        {
            this.clientRepository = clientRepository;
            this.unitOfwork = unitOfwork;
        }

        [HttpGet]
        public IEnumerable<Client> Get()
        {
            return clientRepository.GetAll();
        }

        [HttpGet("{clientId}")]
        public Client GetClient([FromRoute] int clientId)
        {
            return clientRepository.GetById(clientId);
        }

        [HttpPost]
        public IActionResult Add([FromBody]Client client)
        {
            if (ModelState.IsValid)
            {
                if (isClientNameTaken(client.Name, -1) == false)
                {
                    client.Projects = null;
                    clientRepository.Add(client);
                    try
                    {
                        unitOfwork.Commit();
                        return Ok();
                    }
                    catch (Exception e)
                    {
                        return BadRequest(new ValidationResult("Error interno del servidor."));
                    }
                }
                else
                {
                    return BadRequest(new ValidationResult("El cliente ya existe."));
                }
                
            }
            
            return BadRequest(new ValidationResult("Error en los datos de entrada."));
        }

        [HttpPut]
        public IActionResult Update([FromBody] Client client)
        {
            if (ModelState.IsValid)
            {
                var targetClient = clientRepository.GetById(client.Id);
                if (targetClient != null)
                {
                    if (isClientNameTaken(client.Name, client.Id) == false)
                    {
                        targetClient.Name = client.Name;
                        clientRepository.Update(targetClient);
                        try
                        {
                            unitOfwork.Commit();
                            return Ok();
                        }
                        catch (Exception e)
                        {
                            return BadRequest(new ValidationResult("Error interno del servidor."));
                        }
                    }
                    else
                    {
                        return BadRequest(new ValidationResult("El cliente ya existe."));
                    }
                    
                }
                else
                {
                    return BadRequest(new ValidationResult("Cliente no encontrado."));
                }

            }
            return BadRequest(new ValidationResult("Error en los datos de entrada."));
        }

        [HttpDelete("{id}")]
        public IActionResult Remove([FromRoute] int id)
        {
            var target = clientRepository.GetById(id);
            if (target != null)
            {
                clientRepository.Remove(target);
                try
                {
                    unitOfwork.Commit();
                }
                catch(Exception e)
                {
                    return BadRequest(new ValidationResult("Error interno del servidor."));
                }
            }
            else
            {
                return BadRequest(new ValidationResult("Cliente no encontrado."));
            }

            return Ok();
        }

        private bool isClientNameTaken(string name, int id)
        {
            var target = clientRepository.GetByName(name);
        
            if (target == null || (target != null && id > 0 && target.Id == id))
            {
                return false;
            }

            return true;

        }
    }
}
