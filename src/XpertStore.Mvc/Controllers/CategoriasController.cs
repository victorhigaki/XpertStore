using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

    // GET: Categorias
    public async Task<IActionResult> Index()
    {
        return View(await _context.Categoria.ToListAsync());
    }

    // GET: Categorias/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var categoria = await _context.Categoria
            .FirstOrDefaultAsync(m => m.Id == id);
        if (categoria == null)
        {
            return NotFound();
        }

        return View(categoria);
    }

    // GET: Categorias/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Categorias/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

    // GET: Categorias/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var categoria = await _context.Categoria.FindAsync(id);
        if (categoria == null)
        {
            return NotFound();
        }
        return View(categoria);
    }

    // POST: Categorias/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

    // GET: Categorias/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var categoria = await _context.Categoria
            .FirstOrDefaultAsync(m => m.Id == id);
        if (categoria == null)
        {
            return NotFound();
        }

        return View(categoria);
    }

    // POST: Categorias/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var categoria = await _context.Categoria.FindAsync(id);

        if (await CategoriaEmUso(id))
        {
            return BadRequest("Categoria com Produto associado");
        }

        if (categoria != null)
        {
            _context.Categoria.Remove(categoria);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool CategoriaExists(Guid id)
    {
        return _context.Categoria.Any(e => e.Id == id);
    }

    private async Task<bool> CategoriaEmUso(Guid categoriaId)
    {
        return await _context.Produto.AnyAsync(p => p.CategoriaId == categoriaId);
    }
}
