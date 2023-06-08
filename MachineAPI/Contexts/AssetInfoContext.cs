using MachineAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace MachineAPI.Contexts
{
    public class AssetInfoContext :DbContext
    {
        public DbSet<MachineDetails> Details { get; set; }

        public AssetInfoContext(DbContextOptions<AssetInfoContext> details) : base(details)
        {
        }
    }
}
