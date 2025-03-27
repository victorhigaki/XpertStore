using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using XpertStore.Api.Controllers.Base;
using XpertStore.Data.Data;
using XpertStore.Domain.Entities;

namespace XpertStore.Api.Controllers;

public class Categoriasontroller : BaseApiController
{
    private readonly ApplicationDbContext _context;

    public Categoriasontroller(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<IEnumerable<Categoria>>> Get()
    {
        if (_context.Categorias == null)
        {
            return NotFound();
        }

        return await _context.Categorias.ToListAsync();
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<Categoria>> Get(int id)
    {
        if (_context.Categorias == null)
        {
            return NotFound();
        }

        var categoria = await _context.Categorias.FindAsync(id);

        if (categoria == null)
        {
            return NotFound();
        }

        return Ok(categoria);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<Categoria>> Post(Categoria categoria)
    {
        if (_context.Categorias == null)
        {
            return Problem("Erro ao criar um categoria, contate o suporte!");
        }

        if (!ModelState.IsValid)
        {
            return ValidationProblem(new ValidationProblemDetails(ModelState)
            {
                Title = "Um ou mais erros de validação ocorreram!"
            });
        }

        _context.Categorias.Add(categoria);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(Get), new { id = categoria.Id }, categoria);
    }

    [HttpPut("{id:Guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Put(Guid id, Categoria categoria)
    {
        if (id != categoria.Id) return BadRequest();

        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        _context.Entry(categoria).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CategoriaExists(id))
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

    [HttpDelete("{id:Guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Delete(Guid id)
    {
        if (_context.Categorias == null)
        {
            return NotFound();
        }

        var produto = await _context.Categorias.FindAsync(id);

        if (produto == null)
        {
            return NotFound();
        }

        _context.Categorias.Remove(produto);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool CategoriaExists(Guid id)
    {
        return (_context.Categorias?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
