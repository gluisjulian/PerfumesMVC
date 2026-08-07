using IdentityManualApp.Data;
using IdentityManualApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PerfumesMVC.ViewModels.Dashboard;

namespace PerfumesMVC.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DashboardController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
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
            DashboardViewModel model = new DashboardViewModel
            {
                Perfume = await _context.Perfumes.Include(p => p.Receita).Where(p => p.UsuarioId == ObterUsuario()).ToListAsync(),
                QtdePerfumes = _context.Perfumes.Count()
            };
            return View(model);
        }
    }
}
