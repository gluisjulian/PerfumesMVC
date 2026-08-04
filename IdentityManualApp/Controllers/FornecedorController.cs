using IdentityManualApp.Data;
using IdentityManualApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityManualApp.Controllers
{
    public class FornecedorController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public FornecedorController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private string? ObterUsuario()
        {
            return _userManager.GetUserId(User);
        }

        // GET: Fornecedor
        public async Task<IActionResult> Index()
        {
            var fornecedores = await _context.Fornecedores
                .AsNoTracking()
                .OrderBy(f => f.Nome)
                .Where(x => x.UsuarioId == ObterUsuario())
                .ToListAsync();

            return View(fornecedores);
        }

        // GET: Fornecedor/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var fornecedor = await _context.Fornecedores
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == id);

            if (fornecedor == null) return NotFound();

            return View(fornecedor);
        }

        // GET: Fornecedor/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Fornecedor/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nome,Cnpj,Telefone,Email,Endereco")] Fornecedor fornecedor)
        {
            if (!ModelState.IsValid) return View(fornecedor);

            fornecedor.UsuarioId = ObterUsuario();

            _context.Add(fornecedor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Fornecedor/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var fornecedor = await _context.Fornecedores.FindAsync(id);
            if (fornecedor == null) return NotFound();

            return View(fornecedor);
        }

        // POST: Fornecedor/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Cnpj,Telefone,Email,Endereco")] Fornecedor fornecedor)
        {
            if (id != fornecedor.Id) return NotFound();
            if (!ModelState.IsValid) return View(fornecedor);

            try
            {
                _context.Update(fornecedor);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Fornecedores.AnyAsync(f => f.Id == id)) return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Fornecedor/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var fornecedor = await _context.Fornecedores
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == id);

            if (fornecedor == null) return NotFound();

            return View(fornecedor);
        }

        // POST: Fornecedor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fornecedor = await _context.Fornecedores.FindAsync(id);
            if (fornecedor != null)
            {
                _context.Fornecedores.Remove(fornecedor);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
