using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Clients.Models;
using System.Data.SqlClient;
using System.Data.Common;
using System.Net;
using Vereyon.Web;

namespace Clients.Controllers
{
    public class HomeController : Controller
    {
        private IFlashMessage flashMessages;
        private IClientRepository repository;
        
        public HomeController(IFlashMessage flashMessages, IClientRepository repository)
        {
            this.repository = repository;
            this.flashMessages = flashMessages;
        }
        public IActionResult Index(string searchCriteria = "")
        {
            IEnumerable<Clientes> model = repository.GetClients(searchCriteria);
            ViewBag.SearchCriteria = searchCriteria;
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View("ClientInput");
        }

        [HttpPost]
        public IActionResult Create(Clientes client)
        {
            if (ModelState.IsValid)
            {
                string successMessage="";
                int errorCode = 0;
               
                errorCode = repository.Create(client);
                successMessage = "Se ha creado un nuevo clliente con el nombre: " + client.NombreCompleto;

                this.AddModelErrors(client, errorCode, successMessage);

                if(errorCode == 0)
                {
                    return RedirectToAction("Index");
                }               
                
            }
            
            return View("ClientInput",client);
        }

        [HttpGet]
        public IActionResult Update(long id)
        {
            var targetClient = repository.GetClient(id);
            if(targetClient != null)
            {
                return View(targetClient);
            }
            return RedirectToAction("Index");            
        }

        [HttpPost]
        public IActionResult Update(Clientes client)
        {
            if (ModelState.IsValid)
            {
                string successMessage = "";
                int errorCode = 0;

                errorCode = repository.Update(client);
                successMessage = "Se ha actualizado el elemento de forma correcta";

                this.AddModelErrors(client, errorCode, successMessage);

                if (errorCode == 0)
                {
                    return RedirectToAction("Index");
                }

            }
            return View(client);

        }

        [HttpDelete]
        public void Delete(long id)
        {
            var errorCode = repository.Delete(id);
            
            if(errorCode != 0)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            else
            {
                flashMessages.Confirmation("Elemento eliminado satisfactoriamente");
            }
        }

        public IActionResult Error()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Report()
        {
            var clients = repository.GetClients("");

            return View(clients);
        }

        private void AddModelErrors(Clientes client, int code, string flashMessage)
        {
            switch (code)
            {
                case 1:
                    ModelState.AddModelError(nameof(Clientes.Identificacion), "El identificador " + client.Identificacion + " ya está registrado");
                    break;
                case 2:
                    ModelState.AddModelError(nameof(Clientes.Identificacion), "El identificador debe variar entre 0 y 9,999,999,999");
                    break;
                case 3:
                    ModelState.AddModelError(nameof(Clientes.Telefono), "El telefono " + client.Identificacion + "debe variar entre 0 y 9,999,999,999");
                    break;
                case 0:
                    flashMessages.Confirmation(flashMessage);
                    break;
            }
        }

              
        
    }
}
