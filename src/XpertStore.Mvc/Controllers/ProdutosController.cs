using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using XpertStore.Entities.Models;
using XpertStore.Mvc.Data;
using XpertStore.Mvc.Models;

namespace XpertStore.Mvc.Controllers
{
    public class ProdutosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProdutosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Produtos
        public async Task<IActionResult> Index()
        {
            List<Produto> produtos = await _context.Produto.ToListAsync();
            produtos.ForEach(produto => produto.Categoria = _context.Categoria.First(c => c.Id == produto.CategoriaId));
            return base.View(produtos);
        }

        // GET: Produtos/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produto = await _context.Produto
                .FirstOrDefaultAsync(m => m.Id == id);
            if (produto == null)
            {
                return NotFound();
            }

            return View(produto);
        }

        // GET: Produtos/Create
        public IActionResult Create()
        {
            ObterCategoriasViewBag();
            return View();
        }

        private void ObterCategoriasViewBag()
        {
            ViewBag.Categorias = _context.Categoria
                .Select(c => new SelectListItem()
                {
                    Text = c.Nome,
                    Value = c.Id.ToString()
                })
                .ToList();
        }

        // POST: Produtos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nome,Descricao,Preco,Imagem,Estoque,CategoriaId")] ProdutoViewModel produtoViewModel)
        {
            if (ModelState.IsValid)
            {
                Produto produto = MapProduto(produtoViewModel);

                _context.Add(produto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(produtoViewModel);
        }

        private Produto MapProduto(ProdutoViewModel produtoViewModel)
        {
            return new Produto
            {
                Id = Guid.NewGuid(),
                Nome = produtoViewModel.Nome,
                Descricao = produtoViewModel.Descricao,
                Imagem = produtoViewModel.Imagem,
                Preco = produtoViewModel.Preco,
                Estoque = produtoViewModel.Estoque,
                Categoria = _context.Categoria.First(c => c.Id == produtoViewModel.CategoriaId)
            };
        }

        // GET: Produtos/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produto = await _context.Produto.FindAsync(id);
            if (produto == null)
            {
                return NotFound();
            }
            ObterCategoriasViewBag();
            return View(produto);
        }

        // POST: Produtos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Nome,Descricao,Imagem,Preco,Estoque,CategoriaId")] ProdutoViewModel produtoViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var produto = MapProduto(produtoViewModel);
                    produto.Id = id;
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

        // GET: Produtos/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produto = await _context.Produto
                .FirstOrDefaultAsync(m => m.Id == id);
            if (produto == null)
            {
                return NotFound();
            }

            return View(produto);
        }

        // POST: Produtos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var produto = await _context.Produto.FindAsync(id);
            if (produto != null)
            {
                _context.Produto.Remove(produto);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProdutoExists(Guid id)
        {
            return _context.Produto.Any(e => e.Id == id);
        }
    }
}
