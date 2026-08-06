using IdentityManualApp.Data;
using IdentityManualApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PerfumesMVC.Models;
using PerfumesMVC.ViewModels.Perfume;

namespace PerfumesMVC.Controllers
{
    [Authorize]
    public class PerfumeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PerfumeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private string? ObterUsuario()
        {
            return _userManager.GetUserId(User);
        }

        public IActionResult Index()
        {
            var perfumes =  _context.Perfumes
                .Where(p => p.UsuarioId == ObterUsuario())
                .OrderBy(p => p.Nome)
                .ToList();

            return View(perfumes);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PerfumeViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var perfume = new Perfume
            {
                Nome = model.Nome,
                Descricao = model.Descricao,
                UsuarioId = _userManager.GetUserId(User)
            };

            if (model.ImagemArquivo != null && model.ImagemArquivo.Length > 0)
            {
                using var memoryStream = new MemoryStream();
                await model.ImagemArquivo.CopyToAsync(memoryStream);

                perfume.ImagemDados = memoryStream.ToArray();
                perfume.ImagemTipo = model.ImagemArquivo.ContentType;
                perfume.ImagemNome = model.ImagemArquivo.FileName;
            }

            _context.Perfumes.Add(perfume);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Imagem(int id)
        {
            var perfume = await _context.Perfumes.FindAsync(id);
            if (perfume?.ImagemDados == null)
                return NotFound();

            return File(perfume.ImagemDados, perfume.ImagemTipo ?? "image/jpeg");
        }
    }
}
