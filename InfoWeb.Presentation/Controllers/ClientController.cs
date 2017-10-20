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
    [Route("api/Client")]
    public class ClientController : Controller
    {
        private IClientRepository clientRepository;
        public ClientController(IClientRepository clientRepository)
        {
            this.clientRepository = clientRepository;
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
        public void Add([FromBody]Client client)
        {
            if (ModelState.IsValid)
            {
                client.Projects = null;
                try
                {
                    clientRepository.Add(client);
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

        [HttpPut]
        public void Update([FromBody] Client client)
        {
            if (ModelState.IsValid)
            {
                var targetClient = clientRepository.GetById(client.Id);
                if (targetClient != null)
                {
                    targetClient.Name =client.Name;
                    try
                    {
                        clientRepository.Update(targetClient);
                    }
                    catch(Exception e)
                    {
                        Response.StatusCode = (int)HttpStatusCode.Conflict;
                    }
                    
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.NotFound;
                }

            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
        }

        [HttpDelete("{id}")]
        public void Remove([FromRoute] int id)
        {
            var target = clientRepository.GetById(id);
            if (target != null)
            {
                clientRepository.Remove(target);
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
        }
    }
}
