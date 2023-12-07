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

namespace ClienteAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoResidenciumController : ControllerBase
    {
        private readonly BdClientesContext _context;
        private readonly IMapper _mapper;

        public TipoResidenciumController(IMapper mapper,BdClientesContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        // GET: api/TipoResidencium
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoResidencium>>> GetTipoResidencia()
        {
          if (_context.TipoResidencia == null)
          {
              return NotFound();
          }
            return await _context.TipoResidencia.ToListAsync();
        }

        // GET: api/TipoResidencium/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TipoResidencium>> GetTipoResidencium(byte id)
        {
          if (_context.TipoResidencia == null)
          {
              return NotFound();
          }
            var tipoResidencium = await _context.TipoResidencia.FindAsync(id);

            if (tipoResidencium == null)
            {
                return NotFound();
            }

            return tipoResidencium;
        }

        // PUT: api/TipoResidencium/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTipoResidencium(byte id, TipoResidenciumDTO tipoResidenciumDTO)
        {
            if (id != tipoResidenciumDTO.IdResidencia)
            {
                return BadRequest();
            }
            TipoResidencium tipoResidencium = _mapper.Map<TipoResidencium>(tipoResidenciumDTO);
            _context.Entry(tipoResidencium).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoResidenciumExists(id))
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

        // POST: api/TipoResidencium
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TipoResidenciumDTO>> PostTipoResidencium(TipoResidenciumDTO tipoResidenciumDTO)
        {
          if (_context.TipoResidencia == null)
          {
              return Problem("Entity set 'BdClientesContext.TipoResidencia'  is null.");
          }
          TipoResidencium tipoResidencium = _mapper.Map<TipoResidencium>(tipoResidenciumDTO);
            _context.TipoResidencia.Add(tipoResidencium);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTipoResidencium", new { id = tipoResidencium.IdResidencia }, tipoResidencium);
        }

        // DELETE: api/TipoResidencium/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTipoResidencium(byte id)
        {
            if (_context.TipoResidencia == null)
            {
                return NotFound();
            }
            var tipoResidencium = await _context.TipoResidencia.FindAsync(id);
            if (tipoResidencium == null)
            {
                return NotFound();
            }

            _context.TipoResidencia.Remove(tipoResidencium);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TipoResidenciumExists(byte id)
        {
            return (_context.TipoResidencia?.Any(e => e.IdResidencia == id)).GetValueOrDefault();
        }
    }
}
