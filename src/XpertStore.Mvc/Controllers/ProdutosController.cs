using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using XpertStore.Data.Models;
using XpertStore.Data.Models.Base;
using XpertStore.Data.Repositories.Interfaces;
using XpertStore.Mvc.Models;

namespace XpertStore.Mvc.Controllers;

[Authorize]
public class ProdutosController : Controller
{
    private Guid UserId;
    private readonly IProdutoRepository _produtoRepository;
    private readonly ICategoriaRepository _categoriaRepository;
    private readonly IVendedorRepository _vendedorRepository;

    public ProdutosController(
        IAppIdentityUser user,
        IProdutoRepository produtoRepository,
        ICategoriaRepository categoriaRepository,
        IVendedorRepository vendedorRepository
        )
    {
        _produtoRepository = produtoRepository;
        _categoriaRepository = categoriaRepository;
        _vendedorRepository = vendedorRepository;

        if (user.IsAuthenticated()) UserId = user.GetUserId();
    }

    public async Task<IActionResult> Index()
    {
        List<Produto> produtos = await _produtoRepository.GetProdutosByUserId(UserId);
        return base.View(produtos);
    }

    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var produto = await _produtoRepository.GetById(id.Value);
        if (produto == null)
        {
            return NotFound();
        }

        return View(produto);
    }

    public async Task<IActionResult> Create()
    {
        await ObterCategoriasViewBag();
        return View();
    }

    private async Task ObterCategoriasViewBag()
    {
        List<Categoria> categorias = await _categoriaRepository.GetAll();
        ViewBag.Categorias = categorias
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
            Produto produto = await MapCreateProduto(produtoViewModel);
            produto.Imagem = imgPrefixo + produtoViewModel.ImagemUpload.FileName;

            await _produtoRepository.Create(produto);

            return RedirectToAction(nameof(Index));
        }
        return View(produtoViewModel);
    }

    private async Task<Produto> MapCreateProduto(ProdutoViewModel produtoViewModel)
    {
        var produto = new Produto()
        {
            Nome = produtoViewModel.Nome,
            Descricao = produtoViewModel.Descricao,
            Preco = produtoViewModel.Preco,
            Estoque = produtoViewModel.Estoque,
            Categoria = await _categoriaRepository.GetById(produtoViewModel.CategoriaId),
            Vendedor = await _vendedorRepository.GetById(UserId)
        };

        return produto;
    }

    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var produto = await _produtoRepository.GetById(id.Value);

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

        await ObterCategoriasViewBag();
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

                var produto = await MapEditProduto(produtoViewModel);

                if (produtoViewModel.ImagemUpload != null)
                {
                    var imgPrefixo = Guid.NewGuid() + "_";
                    if (!await UploadArquivo(produtoViewModel.ImagemUpload, imgPrefixo))
                    {
                        return View(produto);
                    }

                    produto.Imagem = imgPrefixo + produtoViewModel.ImagemUpload.FileName;
                }

                await _produtoRepository.Update(produto);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_produtoRepository.ProdutoExists(id))
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

    private async Task<Produto> MapEditProduto(ProdutoViewModel produtoViewModel)
    {
        var produto = await _produtoRepository.GetById(produtoViewModel.Id);

        produto.Nome = produtoViewModel.Nome;
        produto.Descricao = produtoViewModel.Descricao;
        produto.Preco = produtoViewModel.Preco;
        produto.Estoque = produtoViewModel.Estoque;
        produto.Categoria = await _categoriaRepository.GetById(produtoViewModel.CategoriaId)!;
        produto.Vendedor = await _vendedorRepository.GetById(UserId)!;

        return produto;
    }

    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var produto = await _produtoRepository.GetById(id.Value);

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
        var produto = await _produtoRepository.GetById(id);

        if (produto.Vendedor.Id != UserId)
        {
            return RedirectToAction(nameof(Index));
        }

        if (produto != null)
        {
            await _produtoRepository.Delete(produto);
        }

        return RedirectToAction(nameof(Index));
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
