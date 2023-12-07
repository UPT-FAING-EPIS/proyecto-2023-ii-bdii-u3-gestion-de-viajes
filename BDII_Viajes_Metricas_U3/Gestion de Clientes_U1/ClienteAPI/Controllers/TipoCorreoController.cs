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
    public class TipoCorreoController : ControllerBase
    {
        private readonly BdClientesContext _context;

        private readonly IMapper _mapper;
        public TipoCorreoController(IMapper mapper, BdClientesContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        // GET: api/TipoCorreo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoCorreo>>> GetTipoCorreos()
        {
          if (_context.TipoCorreos == null)
          {
              return NotFound();
          }
            return await _context.TipoCorreos.ToListAsync();
        }

        // GET: api/TipoCorreo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TipoCorreo>> GetTipoCorreo(byte id)
        {
          if (_context.TipoCorreos == null)
          {
              return NotFound();
          }
            var tipoCorreo = await _context.TipoCorreos.FindAsync(id);

            if (tipoCorreo == null)
            {
                return NotFound();
            }

            return tipoCorreo;
        }

        // PUT: api/TipoCorreo/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTipoCorreo(byte id, TipoCorreoDTO tipoCorreoDTO)
        {
            if (id != tipoCorreoDTO.IdTipoCorreo)
            {
                return BadRequest();
            }

            TipoCorreo tipoCorreo = _mapper.Map<TipoCorreo>(tipoCorreoDTO);
            _context.Entry(tipoCorreo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoCorreoExists(id))
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

        // POST: api/TipoCorreo
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TipoCorreoDTO>> PostTipoCorreo(TipoCorreoDTO tipoCorreoDTO)
        {
          if (_context.TipoCorreos == null)
          {
              return Problem("Entity set 'BdClientesContext.TipoCorreos'  is null.");
          }
          TipoCorreo tipoCorreo = _mapper.Map<TipoCorreo>(tipoCorreoDTO);
            _context.TipoCorreos.Add(tipoCorreo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTipoCorreo", new { id = tipoCorreo.IdTipoCorreo }, tipoCorreo);
        }

        // DELETE: api/TipoCorreo/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTipoCorreo(byte id)
        {
            if (_context.TipoCorreos == null)
            {
                return NotFound();
            }
            var tipoCorreo = await _context.TipoCorreos.FindAsync(id);
            if (tipoCorreo == null)
            {
                return NotFound();
            }

            _context.TipoCorreos.Remove(tipoCorreo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TipoCorreoExists(byte id)
        {
            return (_context.TipoCorreos?.Any(e => e.IdTipoCorreo == id)).GetValueOrDefault();
        }
    }
}
