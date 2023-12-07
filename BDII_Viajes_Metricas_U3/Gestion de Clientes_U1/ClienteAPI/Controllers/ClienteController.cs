using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClienteAPI.Data;
using ClienteAPI.Models;
using ClienteAPI.DTOs;
using AutoMapper;
using System.Text.Json;
using ClienteAPI.Services;
using Newtonsoft.Json;

namespace ClienteAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private int flag;
        private String texto = "Cliente";
        private readonly BdClientesContext _context;
        private readonly IMapper _mapper;
        private readonly RabbitMQService _rabbitMQService;
        /* private Prueba _prueba; */
        public ClienteController(IMapper mapper, BdClientesContext context, RabbitMQService rabbitMQService)
        {
            _mapper = mapper;
            _context = context;
            _rabbitMQService = rabbitMQService;
        }
        /* public void SetPrueba(Prueba prueba)
        {
            _prueba = prueba;
        } */
        Prueba prueba = new Prueba();

        // GET: api/Cliente
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes()
        {
            flag=1;
          _rabbitMQService.Validar(texto,flag);
          if (_context.Clientes == null)
          {
              return NotFound();
          }
            return await _context.Clientes.ToListAsync();
        }

        // GET: api/Cliente/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> GetCliente(int id)
        {
            flag=1;
          _rabbitMQService.Validar(texto,flag);
          if (_context.Clientes == null)
          {
              return NotFound();
          }
            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente == null)
            {
                return NotFound();
            }

            return cliente;
        }

        // PUT: api/Cliente/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCliente(int id, ClienteCreateDTO clienteDTO)
        {
            // LLAMADA A VALIDAR
          flag=3;
          _rabbitMQService.Validar(texto,flag);
          // FIN LLAMADA
            if (id != clienteDTO.IdCliente)
            {
                return BadRequest();
            }
            Cliente cliente = _mapper.Map<Cliente>(clienteDTO);
            _context.Entry(cliente).State = EntityState.Modified;
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClienteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Cliente
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ClienteCreateDTO>> PostCliente(ClienteCreateDTO clienteDTO)
        {
          if (_context.Clientes == null)
          {
              return Problem("Entity set 'BdClientesContext.Clientes'  is null.");
          }
          // LLAMADA A VALIDAR
          flag=2;
          _rabbitMQService.Validar(texto,flag);
          // FIN LLAMADA
          // PROBANDO METODOS
          /* prueba.Text = clienteDTO.Nombre;
          prueba.Date = DateTime.Parse("2023-11-11"); */
          /* prueba.Evento = "POST"; */
          /* string json = JsonSerializer.Serialize(prueba); */


          /* var jsonString = JsonConvert.SerializeObject(prueba);
          Console.WriteLine($"ESTRUCTURA JSON: {jsonString}"); */

          // USAMOS LA FUNCION SendMessage de la clase creada
          /* _rabbitMQService.SendMessage(prueba, "cola1"); */
          // Imprime en la consola los datos recibidos
          /* Console.WriteLine($"Datos recibidos para cliente: {clienteDTO.Nombre}"); */

          Cliente cliente = _mapper.Map<Cliente>(clienteDTO);
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCliente", new { id = cliente.IdCliente }, cliente);
        }

        // DELETE: api/Cliente/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            if (_context.Clientes == null)
            {
                return NotFound();
            }
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            flag=4;
            _rabbitMQService.Validar(texto,flag);
            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClienteExists(int id)
        {
            return (_context.Clientes?.Any(e => e.IdCliente == id)).GetValueOrDefault();
        }
    }
}
