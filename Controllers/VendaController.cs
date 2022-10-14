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
    public class VendaController : ControllerBase
    {
        private readonly VendaContext _context;
        public VendaController(VendaContext context)
        {
            _context = context;
        }

        [HttpPost("CriarVenda")]
        public IActionResult Criar(int idVendedor, Venda venda)
        {
            var vendedor = _context.Vendedores.Find(idVendedor);

            if (vendedor == null)
                return NotFound();
            
            var nomeVendedor = vendedor.Nome;
            venda.NomeVendedor = nomeVendedor;
            venda.Status = EnumStatusVenda.AguardandoPagamento;
            _context.Add(venda);
            _context.SaveChanges();
            return Ok(venda);
        }

        [HttpGet("BuscarVenda{id}")]
        public IActionResult ObterPorId(int id)
        {
            var venda = _context.Vendas.Find(id);

            if (venda == null)
                return NotFound();

            return Ok(venda);
        }

        [HttpPut("AtualizarStatusVenda{id}")]
        public IActionResult Atualizar(int id, EnumStatusVenda status)
        {
            var vendaBanco = _context.Vendas.Find(id);

            if (vendaBanco == null)
                return NotFound();

            if (vendaBanco.Status == EnumStatusVenda.AguardandoPagamento)
            {
                if (status != EnumStatusVenda.PagamentoAprovado && status != EnumStatusVenda.Cancelada)
                {
                    return BadRequest(new {Erro = "Só pode atualizar status do pedido para PagamentoAprovado ou Cancelada"});
                }
            }
            else if (vendaBanco.Status == EnumStatusVenda.PagamentoAprovado)
            {
                if (status != EnumStatusVenda.EnviadoParaTransportadora && status != EnumStatusVenda.Cancelada)
                {
                    return BadRequest(new {Erro = "Só pode atualizar status do pedido para EnviadoParaTransportadora ou Cancelada"});
                }
            }
            else if (vendaBanco.Status == EnumStatusVenda.EnviadoParaTransportadora)
            {
                if (status != EnumStatusVenda.Entregue)
                {
                    return BadRequest(new {Erro = "Só pode atualizar status do pedido para Entregue"});
                }
            }
            
            vendaBanco.Status = status;
            _context.Vendas.Update(vendaBanco);
            _context.SaveChanges();
            return Ok(vendaBanco);
        }
    }
}