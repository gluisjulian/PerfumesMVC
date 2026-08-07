using PerfumesMVC.Models;

namespace PerfumesMVC.ViewModels.Dashboard
{
    public class DashboardViewModel
    {
        public int QtdePerfumes { get; set; }
        public List<PerfumesMVC.Models.Perfume> Perfume { get; set; } = new();

    }
}
