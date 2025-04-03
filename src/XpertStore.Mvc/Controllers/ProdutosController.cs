using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using XpertStore.Application.Services.Interfaces;
using XpertStore.Data.Data;
using XpertStore.Entities.Models;
using XpertStore.Mvc.Models;

namespace XpertStore.Mvc.Controllers;

[Authorize]
public class ProdutosController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IProdutoService _produtoService;
    private readonly IUserService _userService;

    public ProdutosController(
        ApplicationDbContext context,
        IProdutoService produtoService,
        IUserService userService)
    {
        _context = context;
        _produtoService = produtoService;
        _userService = userService;
    }

    public async Task<IActionResult> Index()
    {
        var user = await _userService.GetUserAsync(User);
        var produtos = await _produtoService.GetAllAsync(new Guid(user.Id));
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
            Produto produto = MapProduto(produtoViewModel);
            produto.Imagem = imgPrefixo + produtoViewModel.ImagemUpload.FileName;

            _context.Add(produto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(produtoViewModel);
    }

    private Produto MapProduto(ProdutoViewModel produtoViewModel)
    {
        var userId = new Guid(User.FindFirst(ClaimTypes.NameIdentifier).Value);

        return new Produto
        {
            Id = Guid.NewGuid(),
            Nome = produtoViewModel.Nome,
            Descricao = produtoViewModel.Descricao,
            Preco = produtoViewModel.Preco,
            Estoque = produtoViewModel.Estoque,
            Categoria = _context.Categorias.First(c => c.Id == produtoViewModel.CategoriaId),
            Vendedor = _context.Vendedores.First(v => v.Id == userId),
        };
    }

    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var produto = await _context.Produtos.FindAsync(id);
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
            CategoriaId = produto.CategoriaId
        };

        ObterCategoriasViewBag();
        return View(produtoViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, [Bind("Nome,Descricao,ImagemUpload,Preco,Estoque,CategoriaId")] ProdutoViewModel produtoViewModel)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var produto = MapProduto(produtoViewModel);
                produto.Id = id;

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
        var produto = await _context.Produtos.FindAsync(id);
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
