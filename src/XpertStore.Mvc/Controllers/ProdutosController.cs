using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using XpertStore.Application.Models.Base;
using XpertStore.Data.Data;
using XpertStore.Entities.Models;
using XpertStore.Mvc.Models;

namespace XpertStore.Mvc.Controllers;

[Authorize]
public class ProdutosController : Controller
{
    private readonly ApplicationDbContext _context;
    private Guid UserId;

    public ProdutosController(ApplicationDbContext context, IAppIdentityUser user)
    {
        _context = context;

        if (user.IsAuthenticated()) UserId = user.GetUserId();
    }

    public async Task<IActionResult> Index()
    {
        List<Produto> produtos = await _context.Produtos
                                                    .Include(p => p.Categoria)
                                                    .Include(p => p.Vendedor)
                                                    .Where(p => p.Vendedor.Id == UserId)
                                                    .ToListAsync();
        return base.View(produtos);
    }

    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var produto = await _context.Produtos
            .FirstOrDefaultAsync(m => m.Id == id);
        if (produto == null)
        {
            return NotFound();
        }

        return View(produto);
    }

    public IActionResult Create()
    {
        ObterCategoriasViewBag();
        return View();
    }

    private void ObterCategoriasViewBag()
    {
        ViewBag.Categorias = _context.Categorias
            .Select(c => new SelectListItem()
            {
                Text = c.Nome,
                Value = c.Id.ToString()
            })
            .ToList();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Nome,Descricao,ImagemUpload,Preco,Estoque,CategoriaId")] ProdutoViewModel produtoViewModel)
    {
        if (ModelState.IsValid)
        {
            var imgPrefixo = Guid.NewGuid() + "_";
            if (!await UploadArquivo(produtoViewModel.ImagemUpload, imgPrefixo))
            {
                return View(produtoViewModel);
            }
            Produto produto = await MapProduto(produtoViewModel);
            produto.Imagem = imgPrefixo + produtoViewModel.ImagemUpload.FileName;

            _context.Add(produto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(produtoViewModel);
    }

    private async Task<Produto> MapProduto(ProdutoViewModel produtoViewModel)
    {
        var produto = produtoViewModel.Id != null ?
             await _context.Produtos.FindAsync(produtoViewModel.Id)
             : new Produto();

        produto.Nome = produtoViewModel.Nome;
        produto.Descricao = produtoViewModel.Descricao;
        produto.Preco = produtoViewModel.Preco;
        produto.Estoque = produtoViewModel.Estoque;
        produto.Categoria = _context.Categorias.First(c => c.Id == produtoViewModel.CategoriaId);
        Vendedor vendedor = await _context.Vendedores.FindAsync(UserId);
        produto.Vendedor = vendedor;
      
        return produto;
    }

    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var produto = await _context.Produtos
                                        .Include(p => p.Categoria)
                                        .Include(p => p.Vendedor)
                                        .FirstOrDefaultAsync(p => p.Id == id);
        if (produto == null)
        {
            return NotFound();
        }

        var produtoViewModel = new ProdutoViewModel
        {
            Id = produto.Id,
            Nome = produto.Nome,
            Descricao = produto.Descricao,
            Imagem = produto.Imagem,
            Preco = produto.Preco,
            Estoque = produto.Estoque,
            CategoriaId = produto.Categoria.Id
        };

        ObterCategoriasViewBag();
        return View(produtoViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, [Bind("Id,Nome,Descricao,ImagemUpload,Preco,Estoque,CategoriaId")] ProdutoViewModel produtoViewModel)
    {
        if (ModelState.IsValid)
        {
            try
            {

                var produto = await MapProduto(produtoViewModel);

                if (produtoViewModel.ImagemUpload != null)
                {
                    var imgPrefixo = Guid.NewGuid() + "_";
                    if (!await UploadArquivo(produtoViewModel.ImagemUpload, imgPrefixo))
                    {
                        return View(produto);
                    }

                    produto.Imagem = imgPrefixo + produtoViewModel.ImagemUpload.FileName;
                }

                _context.Update(produto);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProdutoExists(id))
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
        return View(produtoViewModel);
    }

    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var produto = await _context.Produtos
                                        .Include(p => p.Categoria)
                                        .Include(p => p.Vendedor)
                                        .FirstOrDefaultAsync(m => m.Id == id);
        if (produto == null)
        {
            return NotFound();
        }

        return View(produto);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        if (_context.Produtos == null)
        {
            Problem("");
        }

        var produto = await _context.Produtos
                                        .Include(p => p.Vendedor)
                                        .FirstOrDefaultAsync(p => p.Id == id);

        if (produto.Vendedor.Id != UserId)
        {
            return RedirectToAction(nameof(Index));
        }

        if (produto != null)
        {
            _context.Produtos.Remove(produto);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ProdutoExists(Guid id)
    {
        return _context.Produtos.Any(e => e.Id == id);
    }

    private async Task<bool> UploadArquivo(IFormFile arquivo, string imgPrefixo)
    {
        if (arquivo.Length <= 0) return false;

        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", imgPrefixo + arquivo.FileName);

        if (!Directory.Exists(Directory.GetCurrentDirectory() + "/wwwroot/images"))
            Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/wwwroot/images");

        if (System.IO.File.Exists(path))
        {
            ModelState.AddModelError(string.Empty, "Já existe um arquivo com este nome!");
            return false;
        }

        using (var stream = new FileStream(path, FileMode.Create))
        {
            await arquivo.CopyToAsync(stream);
        }

        return true;
    }
}
