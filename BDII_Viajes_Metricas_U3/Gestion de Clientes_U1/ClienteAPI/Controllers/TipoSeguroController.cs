using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClienteAPI.Data;
using ClienteAPI.Models;
using AutoMapper;
using ClienteAPI.DTOs;

namespace ClienteAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoSeguroController : ControllerBase
    {
        private readonly BdClientesContext _context;
        private readonly IMapper _mapper;

        public TipoSeguroController(IMapper mapper, BdClientesContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        // GET: api/TipoSeguro
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoSeguro>>> GetTipoSeguros()
        {
          if (_context.TipoSeguros == null)
          {
              return NotFound();
          }
            return await _context.TipoSeguros.ToListAsync();
        }

        // GET: api/TipoSeguro/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TipoSeguro>> GetTipoSeguro(byte id)
        {
          if (_context.TipoSeguros == null)
          {
              return NotFound();
          }
            var tipoSeguro = await _context.TipoSeguros.FindAsync(id);

            if (tipoSeguro == null)
            {
                return NotFound();
            }

            return tipoSeguro;
        }

        // PUT: api/TipoSeguro/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTipoSeguro(byte id, TipoSeguroDTO tipoSeguroDTO)
        {
            if (id != tipoSeguroDTO.IdSeguro)
            {
                return BadRequest();
            }
            TipoSeguro tipoSeguro = _mapper.Map<TipoSeguro>(tipoSeguroDTO);
            _context.Entry(tipoSeguro).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoSeguroExists(id))
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

        // POST: api/TipoSeguro
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TipoSeguro>> PostTipoSeguro(TipoSeguroDTO tipoSeguroDTO)
        {
          if (_context.TipoSeguros == null)
          {
              return Problem("Entity set 'BdClientesContext.TipoSeguros'  is null.");
          }
          TipoSeguro tipoSeguro = _mapper.Map<TipoSeguro>(tipoSeguroDTO);
            _context.TipoSeguros.Add(tipoSeguro);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTipoSeguro", new { id = tipoSeguro.IdSeguro }, tipoSeguro);
        }

        // DELETE: api/TipoSeguro/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTipoSeguro(byte id)
        {
            if (_context.TipoSeguros == null)
            {
                return NotFound();
            }
            var tipoSeguro = await _context.TipoSeguros.FindAsync(id);
            if (tipoSeguro == null)
            {
                return NotFound();
            }

            _context.TipoSeguros.Remove(tipoSeguro);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TipoSeguroExists(byte id)
        {
            return (_context.TipoSeguros?.Any(e => e.IdSeguro == id)).GetValueOrDefault();
        }
    }
}
