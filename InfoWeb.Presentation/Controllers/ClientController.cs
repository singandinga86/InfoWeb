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
                client.Projects = null;
                clientRepository.Add(client);
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
                return BadRequest(new ValidationResult("Error en los datos de entrada."));
            }

            return Ok();
        }

        [HttpPut]
        public IActionResult Update([FromBody] Client client)
        {
            if (ModelState.IsValid)
            {
                var targetClient = clientRepository.GetById(client.Id);
                if (targetClient != null)
                {
                    targetClient.Name =client.Name;
                    clientRepository.Update(targetClient);
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

            }
            else
            {
                return BadRequest(new ValidationResult("Error en los datos de entrada."));
            }

            return Ok();
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
    }
}
