using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using XpertStore.Application.Services.Interfaces;
using XpertStore.Application.Models.Base;
using XpertStore.Data.Data;
using XpertStore.Entities.Models;

namespace XpertStore.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProdutosController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IProdutoService _produtoService;
    private readonly IUserService _userService;
    protected Guid? UserId { get; set; } = null;

    public ProdutosController(ApplicationDbContext context,
        IProdutoService produtoService,
        IUserService userService,
        IAppIdentityUser user)
    {
        _context = context;
        _produtoService = produtoService;
        _userService = userService;

        if (user.IsAuthenticated()) UserId = user.GetUserId();
    }

    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<IEnumerable<Produto>>> Get(string? categoria, Guid? categoriaId)
    {
        if (_context.Produtos == null)
        {
            return NotFound();
        }

        var produtos = await _context.Produtos
                                .Include(p => p.Categoria)
                                .Include(p => p.Vendedor)
                                .ToListAsync();

        if (!string.IsNullOrEmpty(categoria))
            produtos.Where(p => p.Categoria.Descricao.Contains(categoria));

        if (categoriaId == null)
            produtos.Where(p => p.Categoria.Id == categoriaId);

        return produtos;
    }

    [HttpGet("{id:Guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<Produto>> Get(Guid id)
    {
        if (_context.Produtos == null)
        {
            return NotFound();
        }

        var produto = await _context.Produtos
                                        .Include(p => p.Categoria)
                                        .Include(p => p.Vendedor)
                                        .Where(p => p.Vendedor.Id == UserId)
                                        .FirstOrDefaultAsync(p => p.Id == id);

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
    public async Task<ActionResult<Produto>> Post(Produto produto)
    {
        if (_context.Produtos == null)
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

        var categoria = await _context.Categorias.FindAsync(produto.Categoria.Id);
        if (categoria == null)
        {
            return Problem("Erro ao criar um produto, contate o suporte!");
        }
        produto.Categoria = categoria;

        var vendedor = await _context.Vendedores.FindAsync(produto.Vendedor.Id);
        if (vendedor == null)
        {
            return Problem("Erro ao criar um produto, contate o suporte!");
        }
        produto.Vendedor = vendedor;

        _context.Produtos.Add(produto);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(Get), new { id = produto.Id }, produto);
    }

    [HttpPut("{id:Guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Put(Guid id, Produto produto)
    {
        if (id != produto.Id) return BadRequest();

        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        if (produto.Vendedor.Id != UserId)
        {
            return Unauthorized();
        }

        _context.Entry(produto).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!Exists(id))
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
        if (_context.Produtos == null)
        {
            return NotFound();
        }

        var produto = await _context.Produtos.FindAsync(id);


        if (produto.Vendedor.Id != UserId)
        {
            return Unauthorized();
        }

        if (produto == null)
        {
            return NotFound();
        }

        _context.Produtos.Remove(produto);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool Exists(Guid id)
    {
        return (_context.Produtos?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
