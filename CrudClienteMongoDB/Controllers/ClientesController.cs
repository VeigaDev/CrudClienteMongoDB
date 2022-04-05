using CrudClienteMongoDB.Config;
using CrudClienteMongoDB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrudClienteMongoDB.Controllers
{
    public class ClientesController : Controller
    {
        private ClienteContexto _dbAccess;

        public ClientesController(IOptions<ConfigDb> options)
        {
            _dbAccess = new ClienteContexto(options);
        }
        public async Task<IActionResult> Index()
        {
            return View(await _dbAccess.Clientes.Find(q => true).ToListAsync());
        }

        [HttpGet]
        public IActionResult NewCustomer()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> NewCustomer(Clientes dadosCliente)
        {
            dadosCliente.CustomerId = Guid.NewGuid();
            dadosCliente.RegisterDate = DateTime.UtcNow;
            if (dadosCliente.CPF_CNPJ !=null)
                dadosCliente.CPF_CNPJ = dadosCliente.CPF_CNPJ.Replace(".", "").Replace("-", "").Replace("/", "").Trim();
            var msg = ValidateCustomer(dadosCliente);
            if (msg == string.Empty)
            {
                await _dbAccess.Clientes.InsertOneAsync(dadosCliente);
                return RedirectToAction(nameof(Index));
            }
            else
                return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> EditClient(Guid customerId)
        {
            Clientes customer = await _dbAccess.Clientes.Find(q => q.CustomerId == customerId).FirstOrDefaultAsync();
            return View(customer);
        }

        [HttpPost]
        public async Task<IActionResult> EditClient(Clientes customers)
        {
            await _dbAccess.Clientes.ReplaceOneAsync(q => q.CustomerId == customers.CustomerId, customers);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveClient(Guid customerId)
        {
            await _dbAccess.Clientes.DeleteOneAsync(q => q.CustomerId == customerId);
            return RedirectToAction(nameof(Index));
        }

        public object ValidateCustomer(Clientes customers)
        {
            var msg = string.Empty;
            if (customers != null)
            {
                if (customers.FantasyName == null && customers.CompanyName == null)
                {
                    msg = "É necessário preencher o campo FantasyName ou CompanyName";
                    return msg;
                }
                if (customers.CPF_CNPJ != null && customers.CPF_CNPJ.Length < 11)
                {
                    msg = "É necessário preencher o campo CPF_CNPJ corretamente";
                    return msg;
                }
                if (customers.Customer == false && customers.Supplier == false)
                {
                    msg = "É necessário selecionar pelo menos um campo para Customer ou Supplier";
                    return msg;
                }
                else
                    return string.Empty;
            }
            else
                msg = "É necessário preencher os campos para cadastro de um novo cliente";
            return msg;
        }
    }
}
