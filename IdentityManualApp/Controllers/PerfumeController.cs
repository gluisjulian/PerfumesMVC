using IdentityManualApp.Data;
using IdentityManualApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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


        public IActionResult Details(int id)
        {
            var perfume = _context.Perfumes.Find(id);
            return View(perfume);
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            var perfume = _context.Perfumes
                .FirstOrDefault(p => p.Id == id && p.UsuarioId == ObterUsuario());

            if (perfume == null)
                return NotFound();

            var viewModel = new PerfumeEditViewModel
            {
                Id = perfume.Id,
                Nome = perfume.Nome,
                Descricao = perfume.Descricao,
                ImagemAtualNome = perfume.ImagemNome
            };

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PerfumeViewModel model)
        {
            if (id != model.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(model);

            var perfume = await _context.Perfumes
                .FirstOrDefaultAsync(p => p.Id == id && p.UsuarioId == ObterUsuario());

            if (perfume == null)
                return NotFound();

            perfume.Nome = model.Nome;
            perfume.Descricao = model.Descricao;

            // Só troca a imagem se o usuário enviou um novo arquivo
            if (model.ImagemArquivo != null && model.ImagemArquivo.Length > 0)
            {
                using var memoryStream = new MemoryStream();
                await model.ImagemArquivo.CopyToAsync(memoryStream);

                perfume.ImagemDados = memoryStream.ToArray();
                perfume.ImagemTipo = model.ImagemArquivo.ContentType;
                perfume.ImagemNome = model.ImagemArquivo.FileName;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Perfumes.AnyAsync(p => p.Id == id))
                    return NotFound();
                throw;
            }

            TempData["Sucesso"] = "Perfume atualizado com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            if (id == null) return NotFound();

            var perfume = _context.Perfumes
                .FirstOrDefault(p => p.Id == id);

            if (perfume == null) return NotFound();

            return View(perfume);
        }

        // POST: Produto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var perfume = await _context.Perfumes.FindAsync(id);
            if (perfume != null)
            {
                _context.Perfumes.Remove(perfume);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
