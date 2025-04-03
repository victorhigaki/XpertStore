using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using XpertStore.Application.Services.Interfaces;
using XpertStore.Mvc.Models;

namespace XpertStore.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProdutoService _produtoService;
        private readonly IUserService _userService;

        public HomeController(
            ILogger<HomeController> logger,
            IProdutoService produtoService,
            IUserService userService)
        {
            _logger = logger;
            _produtoService = produtoService;
            _userService = userService;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userService.GetUserAsync(User);
            Guid? userId = user != null ? new Guid(user.Id) : null;
            var produtos = await _produtoService.GetAllAsync(userId);
            List<ProdutoHomeViewModel> produtoViewModel = [];
            foreach (var produto in produtos)
            {
                produtoViewModel.Add(new ProdutoHomeViewModel
                {
                    Id = produto.Id,
                    Nome = produto.Nome,
                    Descricao = produto.Descricao,
                    Imagem = produto.Imagem,
                });
            }

            return View(produtoViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
