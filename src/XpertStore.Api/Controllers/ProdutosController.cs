using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using XpertStore.Api.Controllers.Base;
using XpertStore.Api.Models;
using XpertStore.Data.Models;
using XpertStore.Data.Models.Base;
using XpertStore.Data.Repositories;

namespace XpertStore.Api.Controllers;

public class ProdutosController : BaseApiController
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly ICategoriaRepository _categoriaRepository;
    private readonly IVendedorRepository _vendedorRepository;

    protected Guid? UserId { get; set; } = null;

    public ProdutosController(
        IAppIdentityUser user,
        IProdutoRepository produtoRepository,
        ICategoriaRepository categoriaRepository,
        IVendedorRepository vendedorRepository)
    {
        if (user.IsAuthenticated()) UserId = user.GetUserId();
        _produtoRepository = produtoRepository;
        _categoriaRepository = categoriaRepository;
        _vendedorRepository = vendedorRepository;
    }

    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<IEnumerable<ProdutoViewModel>>> Get(string? categoria, Guid? categoriaId)
    {
        if (_produtoRepository.IsNull())
        {
            return NotFound();
        }

        var produtos = await _produtoRepository.GetProdutosCategoriaVendedorAsync();

        if (!string.IsNullOrEmpty(categoria))
            produtos.Where(p => p.Categoria.Descricao.Contains(categoria));

        if (categoriaId == null)
            produtos.Where(p => p.Categoria.Id == categoriaId);

        var produtosViewModel = _MapToProdutosViewModel(produtos);

        return Ok(produtosViewModel);
    }

    [HttpGet("{id:Guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<ProdutoViewModel>> Get(Guid id)
    {
        if (_produtoRepository.IsNull())
        {
            return NotFound();
        }

        var produto = await _produtoRepository.GetProdutoCategoriaVendedorByIdAndUserIdAsync(id, UserId.Value);

        if (produto == null)
        {
            return NotFound();
        }

        return Ok(produto);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<ProdutoViewModel>> Post(ProdutoViewModel produtoViewModel)
    {
        if (_produtoRepository.IsNull())
        {
            return Problem("Erro ao criar um produto, contate o suporte!");
        }

        if (!ModelState.IsValid)
        {
            return ValidationProblem(new ValidationProblemDetails(ModelState)
            {
                Title = "Um ou mais erros de validação ocorreram!"
            });
        }

        var produto = _MapToProduto(produtoViewModel);

        var categoria = await _categoriaRepository.GetByIdAsync(produtoViewModel.Categoria.Id);
        if (categoria == null)
        {
            return Problem("Erro ao criar um produto, contate o suporte!");
        }
        produto.Categoria = categoria;

        var vendedor = await _vendedorRepository.GetByIdAsync(UserId!.Value);
        if (vendedor == null)
        {
            return Problem("Erro ao criar um produto, contate o suporte!");
        }
        produto.Vendedor = vendedor;

        var result = await _produtoRepository.CreateAsync(produto);

        return CreatedAtAction(nameof(Get), new { id = result.Id }, produtoViewModel);
    }

    [HttpPut("{id:Guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Put(Guid id, ProdutoViewModel produtoViewModel)
    {
        if (id != produtoViewModel.Id) return BadRequest();

        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        if (produtoViewModel.Vendedor.Id != UserId)
        {
            return Unauthorized();
        }

        try
        {
            await _produtoRepository.UpdateAsync(_MapToProduto(produtoViewModel));
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

        return NoContent();
    }

    [HttpDelete("{id:Guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Delete(Guid id)
    {
        if (_produtoRepository.IsNull())
        {
            return NotFound();
        }

        var result = await _produtoRepository.DeleteAsync(id);

        if (result == null)
        {
            return NotFound();
        }

        return NoContent();
    }

    private IEnumerable<ProdutoViewModel> _MapToProdutosViewModel(IEnumerable<Produto> produtos)
    {
        var produtoViewModelList = new List<ProdutoViewModel>();

        foreach (var produto in produtos)
        {
            produtoViewModelList.Add(_MapToProdutoViewModel(produto));
        }

        return produtoViewModelList;
    }

    private ProdutoViewModel _MapToProdutoViewModel(Produto produto)
    {
        VendedorViewModel vendedor = _MapToVendedorViewModel(produto.Vendedor);

        CategoriaViewModel categoria = _MapToCategoriaViewModel(produto.Categoria);

        return new ProdutoViewModel
        {
            Id = produto.Id,
            Nome = produto.Nome,
            Descricao = produto.Descricao,
            Imagem = produto.Imagem,
            Preco = produto.Preco,
            Estoque = produto.Estoque,
            Categoria = categoria,
            Vendedor = vendedor,
        };
    }

    private static CategoriaViewModel _MapToCategoriaViewModel(Categoria categoria)
    {
        return new CategoriaViewModel()
        {
            Id = categoria.Id,
            Nome = categoria.Nome,
            Descricao = categoria.Descricao,
        };
    }

    private static VendedorViewModel _MapToVendedorViewModel(Vendedor vendedor)
    {
        return new VendedorViewModel()
        {
            Id = vendedor.Id,
        };
    }

    private Produto _MapToProduto(ProdutoViewModel produtoViewModel)
    {
        Vendedor vendedor = _MapToVendedor(produtoViewModel.Vendedor);

        Categoria categoria = _MapToCategoria(produtoViewModel.Categoria);

        return new Produto
        {
            Id = produtoViewModel.Id,
            Nome = produtoViewModel.Nome,
            Descricao = produtoViewModel.Descricao,
            Imagem = produtoViewModel.Imagem,
            Preco = produtoViewModel.Preco,
            Estoque = produtoViewModel.Estoque,
            Categoria = categoria,
            Vendedor = vendedor,
        };
    }

    private static Categoria _MapToCategoria(CategoriaViewModel categoriaViewModel)
    {
        return new Categoria()
        {
            Id = categoriaViewModel.Id,
            Nome = categoriaViewModel.Nome,
            Descricao = categoriaViewModel.Descricao,
        };
    }

    private static Vendedor _MapToVendedor(VendedorViewModel vendedorViewModel)
    {
        return new Vendedor()
        {
            Id = vendedorViewModel.Id,
        };
    }
}
