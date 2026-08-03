using IdentityManualApp.Data;
using IdentityManualApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace IdentityManualApp.Controllers
{
    // Toda a controller exige usuário autenticado. Quem tentar acessar
    // sem estar logado é redirecionado para /Account/Login (ver Program.cs).
    [Authorize]
    public class ProdutoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProdutoController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private string? ObterUsuario()
        {
            return _userManager.GetUserId(User);
        }

        // GET: Produto
        public async Task<IActionResult> Index()
        {

            var produtos = await _context.Produtos
                .AsNoTracking()
                .Include(p => p.Fornecedor)
                .OrderBy(p => p.Nome)
                .Where(x => x.UsuarioId == ObterUsuario())
                .ToListAsync();

            return View(produtos);
        }

        // GET: Produto/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var produto = await _context.Produtos
                .AsNoTracking()
                .Include(p => p.Fornecedor)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (produto == null) return NotFound();

            return View(produto);
        }

        // GET: Produto/Create
        public IActionResult Create()
        {
            CarregarFornecedores();
            return View();
        }

        // POST: Produto/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nome,Descricao,Preco,Quantidade,FornecedorId")] Produto produto)
        {
            if (!ModelState.IsValid)
            {
                CarregarFornecedores(produto.FornecedorId);
                return View(produto);
            }

            // Registra qual usuário logado cadastrou o produto.
            produto.UsuarioId = _userManager.GetUserId(User);

            _context.Add(produto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Produto/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null) return NotFound();

            CarregarFornecedores(produto.FornecedorId);
            return View(produto);
        }

        // POST: Produto/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Descricao,Preco,Quantidade,FornecedorId,UsuarioId")] Produto produto)
        {
            if (id != produto.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                CarregarFornecedores(produto.FornecedorId);
                return View(produto);
            }

            try
            {
                _context.Update(produto);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Produtos.AnyAsync(p => p.Id == id)) return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Produto/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var produto = await _context.Produtos
                .AsNoTracking()
                .Include(p => p.Fornecedor)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (produto == null) return NotFound();

            return View(produto);
        }

        // POST: Produto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto != null)
            {
                _context.Produtos.Remove(produto);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private void CarregarFornecedores(int? fornecedorSelecionadoId = null)
        {
            ViewBag.FornecedorId = new SelectList(
                _context.Fornecedores.AsNoTracking().OrderBy(f => f.Nome),
                "Id", "Nome", fornecedorSelecionadoId);
        }
    }
}
