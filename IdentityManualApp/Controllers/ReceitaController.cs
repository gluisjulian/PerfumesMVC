using IdentityManualApp.Data;
using IdentityManualApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PerfumesMVC.Models;
using PerfumesMVC.ViewModels.Receita;

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
            var produtos = _context.Produtos
            .OrderBy(x => x.Nome)
            .Select(x => new ProdutoReceitaViewModel
            {
                ProdutoId = x.Id,
                NomeProduto = x.Nome,
                UnidadeMedida = x.UnidadeMedida
            }).ToList();


            var model = new ReceitaCreateViewModel
            {
                Produtos = produtos
            };


            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReceitaCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);


            var receita = new Receita
            {
                Titulo = model.Titulo,
                Descricao = model.Descricao,
                UsuarioId = _userManager.GetUserId(User)
            };


            foreach (var item in model.Produtos.Where(x => x.Selecionado))
            {
                receita.ReceitaProdutos.Add(new ReceitaProduto
                {
                    ProdutoId = item.ProdutoId,
                    Quantidade = item.Quantidade,
                    PorcentagemProduto = item.PorcentagemProduto,
                    UnidadeMedida = item.UnidadeMedida
                });
            }

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
                _context.Receitas.Remove(receita);
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
