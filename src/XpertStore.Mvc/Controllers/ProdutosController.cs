using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using XpertStore.Data.Data;
using XpertStore.Data.Models;
using XpertStore.Data.Models.Base;
using XpertStore.Data.Repositories;
using XpertStore.Mvc.Models;

namespace XpertStore.Mvc.Controllers;

[Authorize]
public class ProdutosController : Controller
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly ICategoriaRepository _categoriaRepository;
    private readonly IVendedorRepository _vendedorRepository;
    private Guid UserId;

    public ProdutosController(
        IProdutoRepository produtoRepository,
        IAppIdentityUser user,
        ICategoriaRepository categoriaRepository,
        IVendedorRepository vendedorRepository)
    {
        if (user.IsAuthenticated()) UserId = user.GetUserId();
        _produtoRepository = produtoRepository;
        _categoriaRepository = categoriaRepository;
        _vendedorRepository = vendedorRepository;
    }

    public async Task<IActionResult> Index()
    {
        return base.View(await _produtoRepository.GetProdutosCategoriaVendedorAsync());
    }

    public async Task<IActionResult> Details(Guid id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var produtoViewModel = await GetProdutoById(id);

        if (produtoViewModel == null)
        {
            return NotFound();
        }

        return View(produtoViewModel);
    }

    public async Task<IActionResult> Create()
    {
        await ObterCategoriasViewBag();
        return View();
    }

    private async Task ObterCategoriasViewBag()
    {
        ViewBag.Categorias = (await _categoriaRepository.GetAllAsync())
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

            await _produtoRepository.CreateAsync(produto);
            return RedirectToAction(nameof(Index));
        }
        return View(produtoViewModel);
    }

    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var produtoViewModel = await GetProdutoById(id.Value);
        if (produtoViewModel == null)
        {
            return NotFound();
        }

        await ObterCategoriasViewBag();
        return View(produtoViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, [Bind("Id,Nome,Descricao,Imagem,ImagemUpload,Preco,Estoque,CategoriaId")] ProdutoViewModel produtoViewModel)
    {
        if (ModelState.IsValid)
        {
            try
            {
                if (produtoViewModel.ImagemUpload != null)
                {
                    var imgPrefixo = Guid.NewGuid() + "_";
                    if (!await UploadArquivo(produtoViewModel.ImagemUpload, imgPrefixo))
                    {
                        return View(produtoViewModel);
                    }

                    produtoViewModel.Imagem = imgPrefixo + produtoViewModel.ImagemUpload.FileName;
                }

                var produto = await MapProduto(produtoViewModel);
                await _produtoRepository.UpdateAsync(produto);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_produtoRepository.Exists(id))
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

        var produto = await GetProdutoById(id.Value);

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
        var produto = await GetProdutoById(id);

        if (produto.VendedorId != UserId)
        {
            return RedirectToAction(nameof(Index));
        }

        if (produto != null)
        {
            await _produtoRepository.DeleteAsync(id);
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

    private async Task<ProdutoViewModel> GetProdutoById(Guid id)
    {
        return MapProdutoViewModel(await _produtoRepository.GetByIdAsync(id));
    }

    private ProdutoViewModel MapProdutoViewModel(Produto produto)
    {
        return new ProdutoViewModel
        {
            Id = produto.Id,
            Nome = produto.Nome,
            Descricao = produto.Descricao,
            Imagem = produto.Imagem,
            Preco = produto.Preco,
            Estoque = produto.Estoque,
            CategoriaId = produto.CategoriaId,
            VendedorId = UserId,
        };
    }

    private async Task<Produto> MapProduto(ProdutoViewModel produtoViewModel)
    {
        return new Produto
        {
            Id = produtoViewModel.Id,
            Nome = produtoViewModel.Nome,
            Descricao = produtoViewModel.Descricao,
            Preco = produtoViewModel.Preco,
            Estoque = produtoViewModel.Estoque,
            Imagem = produtoViewModel.Imagem,
            Categoria = await _categoriaRepository.GetByIdAsync(produtoViewModel.CategoriaId),
            CategoriaId = produtoViewModel.CategoriaId,
            Vendedor = await _vendedorRepository.GetByIdAsync(UserId),
            VendedorId = UserId,
        };
    }
}
