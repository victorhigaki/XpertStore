using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using XpertStore.Data.Data;
using XpertStore.Entities.Models;
using XpertStore.Mvc.Data;

namespace XpertStore.Mvc.Controllers;

[Authorize]
public class CategoriasController : Controller
{
    private readonly ApplicationDbContext _context;

    public CategoriasController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _context.Categorias.ToListAsync());
    }

    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var categoria = await _context.Categorias
            .FirstOrDefaultAsync(m => m.Id == id);
        if (categoria == null)
        {
            return NotFound();
        }

        return View(categoria);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Nome,Descricao")] Categoria categoria)
    {
        if (ModelState.IsValid)
        {
            categoria.Id = Guid.NewGuid();
            _context.Add(categoria);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(categoria);
    }

    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var categoria = await _context.Categorias.FindAsync(id);
        if (categoria == null)
        {
            return NotFound();
        }
        return View(categoria);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, [Bind("Id,Nome,Descricao")] Categoria categoria)
    {
        if (id != categoria.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(categoria);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoriaExists(categoria.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(categoria);
    }

    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var categoria = await _context.Categorias
            .FirstOrDefaultAsync(m => m.Id == id);
        if (categoria == null)
        {
            return NotFound();
        }

        return View(categoria);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var categoria = await _context.Categorias.FindAsync(id);

        if (await CategoriaEmUso(id))
        {
            return BadRequest("Categoria com Produto associado");
        }

        if (categoria != null)
        {
            _context.Categorias.Remove(categoria);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool CategoriaExists(Guid id)
    {
        return _context.Categorias.Any(e => e.Id == id);
    }

    private async Task<bool> CategoriaEmUso(Guid categoriaId)
    {
        return await _context.Produtos.AnyAsync(p => p.Categoria.Id == categoriaId);
    }
}
