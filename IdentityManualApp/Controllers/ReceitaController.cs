using IdentityManualApp.Data;
using IdentityManualApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PerfumesMVC.Models;

namespace PerfumesMVC.Controllers
{
    public class ReceitaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReceitaController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private string? ObterUsuario()
        {
            return _userManager.GetUserId(User);
        }

        public async Task<IActionResult> Index()
        {

            var receitas = await _context.Receitas
                .AsNoTracking()
                .Include(p => p.ReceitaProdutos)
                .ThenInclude(rp => rp.Produto)
                .OrderBy(p => p.Id)
                .Where(x => x.UsuarioId == ObterUsuario())
                .ToListAsync();

            return View(receitas);
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var receita = await _context.Receitas
                .AsNoTracking()
                .Include(p => p.ReceitaProdutos)
                .ThenInclude(rp => rp.Produto)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (receita == null) return NotFound();

            return View(receita);
        }


        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Receita receita)
        {
            if (!ModelState.IsValid)
            {
                CarregarProdutos();
                return View(receita);
            }

            // Registra qual usuário logado cadastrou o produto.
            receita.UsuarioId = _userManager.GetUserId(User);

            _context.Add(receita);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var receita = await _context.Receitas.FindAsync(id);
            if (receita == null) return NotFound();

            CarregarProdutos();
            return View(receita);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Receita receita)
        {
            if (id != receita.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                CarregarProdutos();
                return View(receita);
            }

            try
            {
                _context.Update(receita);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Receitas.AnyAsync(r => r.Id == id)) return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var receita = await _context.Receitas
                .AsNoTracking()
                .Include(p => p.ReceitaProdutos)
                .ThenInclude(rp => rp.Produto)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (receita == null) return NotFound();

            return View(receita);
        }

        // POST: Produto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var receita = await _context.Receitas.FindAsync(id);
            if (receita != null)
            {
                _context.Produtos.Remove(receita);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private void CarregarProdutos(int? produtoSelecionadoId = null)
        {
            ViewBag.ProdutoId = new SelectList(
                _context.Produtos.AsNoTracking().OrderBy(f => f.Nome).Where(f => f.UsuarioId == ObterUsuario()),
                "Id", "Nome", produtoSelecionadoId);
        }
    }
}
