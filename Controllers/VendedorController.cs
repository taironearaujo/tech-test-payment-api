using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using tech_test_payment_api.Context;
using tech_test_payment_api.Models;

namespace tech_test_payment_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VendedorController : ControllerBase
    {
        private readonly VendaContext _context;
        public VendedorController(VendaContext context)
        {
            _context = context;
        }

        [HttpGet("BuscarVendedor{id}")]
        public IActionResult ObterPorId(int id)
        {
            var vendedor = _context.Vendedores.Find(id);

            if (vendedor == null)
                return NotFound();

            return Ok(vendedor);
        }

        [HttpPost("CadastrarVendedor")]
        public IActionResult Cadastro(Vendedor vendedor)
        {
            _context.Add(vendedor);
            _context.SaveChanges();
            return Ok(vendedor);
        }

        [HttpPut("AtualizarVendedor{id}")]
        public IActionResult Atualizar(int id, Vendedor vendedor)
        {
            var vendedorBanco = _context.Vendedores.Find(id);

            if (vendedorBanco == null)
                return NotFound();

            vendedorBanco.Cpf = vendedor.Cpf;
            vendedorBanco.Nome = vendedor.Nome;
            vendedorBanco.Email = vendedor.Email;
            vendedorBanco.Telefone = vendedor.Telefone;
            _context.Vendedores.Update(vendedorBanco);
            _context.SaveChanges();
            return Ok(vendedorBanco);
        }

        [HttpDelete("DeletarVendedor{id}")]
        public IActionResult Deletar(int id)
        {
            var vendedorBanco = _context.Vendedores.Find(id);

            if (vendedorBanco == null)
                return NotFound();

            _context.Vendedores.Remove(vendedorBanco);
            _context.SaveChanges();
            return NoContent();
        }
    }
}