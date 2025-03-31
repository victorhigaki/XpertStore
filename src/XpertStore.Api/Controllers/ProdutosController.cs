using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using XpertStore.Data.Data;
using XpertStore.Entities.Models;

namespace XpertStore.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/produtos")]
public class ProdutosController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ProdutosController(ApplicationDbContext context)
    {
        _context = context;
    }

    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<IEnumerable<Produto>>> GetProduto()
    {
        if (_context.Produtos == null)
        {
            return NotFound();
        }

        return await _context.Produtos.ToListAsync();
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<Produto>> GetProdutos(int id)
    {
        if (_context.Produtos == null)
        {
            return NotFound();
        }

        var produto = await _context.Produtos.FindAsync(id);

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
    public async Task<ActionResult<Produto>> PostProduto(Produto produto)
    {
        if (_context.Produtos == null)
        {
            return Problem("Erro ao criar um produto, contate o suporte!");
        }

        if (!ModelState.IsValid)
        {
            //return BadRequest(ModelState);

            //return ValidationProblem(ModelState);

            return ValidationProblem(new ValidationProblemDetails(ModelState)
            {
                Title = "Um ou mais erros de validação ocorreram!"
            });
        }

        _context.Produtos.Add(produto);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetProduto), new { id = produto.Id }, produto);
    }

    [HttpPut("{id:Guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> PutProduto(Guid id, Produto produto)
    {
        if (id != produto.Id) return BadRequest();

        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        _context.Entry(produto).State = EntityState.Modified;

        try
        {
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

        return NoContent();
    }

    [HttpDelete("{id:Guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> DeleteProduto(Guid id)
    {
        if (_context.Produtos == null)
        {
            return NotFound();
        }

        var produto = await _context.Produtos.FindAsync(id);

        if (produto == null)
        {
            return NotFound();
        }

        _context.Produtos.Remove(produto);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ProdutoExists(Guid id)
    {
        return (_context.Produtos?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
