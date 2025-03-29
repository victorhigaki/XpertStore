using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using XpertStore.Api.Controllers.Base;
using XpertStore.Data.Data;
using XpertStore.Entities.Models;

namespace XpertStore.Api.Controllers;

[Authorize]
public class CategoriasController : BaseCrudController<Categoria>
{
    private readonly ApplicationDbContext _context;

    public CategoriasController(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    [HttpDelete("{id:Guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public override async Task<IActionResult> Delete(Guid id)
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

        if (await CategoriaEmUso(id))
        {
            return Problem("Categoria com Produto associado");
        }

        _context.Categorias.Remove(categoria);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> CategoriaEmUso(Guid categoriaId)
    {
        return await _context.Produtos.AnyAsync(p => p.CategoriaId == categoriaId);
    }
}
