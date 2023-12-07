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
    public class TiposDocumentoController : ControllerBase
    {
        private readonly BdClientesContext _context;
        private readonly IMapper _mapper;

        public TiposDocumentoController(IMapper mapper, BdClientesContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        // GET: api/TiposDocumento
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TiposDocumento>>> GetTiposDocumentos()
        {
          if (_context.TiposDocumentos == null)
          {
              return NotFound();
          }
            return await _context.TiposDocumentos.ToListAsync();
        }

        // GET: api/TiposDocumento/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TiposDocumento>> GetTiposDocumento(byte id)
        {
          if (_context.TiposDocumentos == null)
          {
              return NotFound();
          }
            var tiposDocumento = await _context.TiposDocumentos.FindAsync(id);

            if (tiposDocumento == null)
            {
                return NotFound();
            }

            return tiposDocumento;
        }

        // PUT: api/TiposDocumento/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTiposDocumento(byte id, TiposDocumentoDTO tiposDocumentoDTO)
        {
            if (id != tiposDocumentoDTO.IdTipoDocumento)
            {
                return BadRequest();
            }
            TiposDocumento tiposDocumento = _mapper.Map<TiposDocumento>(tiposDocumentoDTO);
            _context.Entry(tiposDocumento).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TiposDocumentoExists(id))
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

        // POST: api/TiposDocumento
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TiposDocumentoDTO>> PostTiposDocumento(TiposDocumentoDTO tiposDocumentoDTO)
        {
          if (_context.TiposDocumentos == null)
          {
              return Problem("Entity set 'BdClientesContext.TiposDocumentos'  is null.");
          }
          TiposDocumento tiposDocumento = _mapper.Map<TiposDocumento>(tiposDocumentoDTO);
            _context.TiposDocumentos.Add(tiposDocumento);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTiposDocumento", new { id = tiposDocumento.IdTipoDocumento }, tiposDocumento);
        }

        // DELETE: api/TiposDocumento/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTiposDocumento(byte id)
        {
            if (_context.TiposDocumentos == null)
            {
                return NotFound();
            }
            var tiposDocumento = await _context.TiposDocumentos.FindAsync(id);
            if (tiposDocumento == null)
            {
                return NotFound();
            }

            _context.TiposDocumentos.Remove(tiposDocumento);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TiposDocumentoExists(byte id)
        {
            return (_context.TiposDocumentos?.Any(e => e.IdTipoDocumento == id)).GetValueOrDefault();
        }
    }
}
