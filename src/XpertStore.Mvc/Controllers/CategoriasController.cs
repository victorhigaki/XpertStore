using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using XpertStore.Data.Models;
using XpertStore.Data.Repositories.Interfaces;
using XpertStore.Mvc.Models;

namespace XpertStore.Mvc.Controllers;

[Authorize]
public class CategoriasController : Controller
{
    private readonly ICategoriaRepository _categoriaRepository;
    private readonly IProdutoRepository _produtoRepository;

    public CategoriasController(
        ICategoriaRepository categoriaRepository,
        IProdutoRepository produtoRepository)
    {
        _categoriaRepository = categoriaRepository;
        _produtoRepository = produtoRepository;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _categoriaRepository.GetAll());
    }

    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var categoria = await _categoriaRepository.GetById(id);
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
    public async Task<IActionResult> Create([Bind("Id,Nome,Descricao")] CategoriaViewModel categoria)
    {
        if (ModelState.IsValid)
        {
            var model = new Categoria
            {
                Nome = categoria.Nome,
                Descricao = categoria.Descricao,
            };

            await _categoriaRepository.Create(model);

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

        var categoria = await _categoriaRepository.GetById(id);
        if (categoria == null)
        {
            return NotFound();
        }

        CategoriaViewModel categoriaViewModel = new()
        {
            Descricao = categoria.Descricao,
            Nome = categoria.Nome,
        };

        return View(categoriaViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, [Bind("Id,Nome,Descricao")] CategoriaViewModel categoriaViewModel)
    {
        if (id != categoriaViewModel.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var model = await _categoriaRepository.GetById(id);
                if (model == null)
                {
                    return NotFound();
                }

                model.Nome = categoriaViewModel.Nome;
                model.Descricao = categoriaViewModel.Descricao;

                await _categoriaRepository.Update(model);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoriaExists(categoriaViewModel.Id))
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
        return View(categoriaViewModel);
    }

    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var categoria = await _categoriaRepository.GetById(id);

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
        var categoria = await _categoriaRepository.GetById(id);

        if (await _produtoRepository.ProdutoIsUsingCategoria(id))
        {
            return BadRequest("Categoria com Produto associado");
        }

        await _categoriaRepository.Delete(categoria);

        return RedirectToAction(nameof(Index));
    }

    private bool CategoriaExists(Guid id)
    {
        return _categoriaRepository.Exists(id);
    }
}
