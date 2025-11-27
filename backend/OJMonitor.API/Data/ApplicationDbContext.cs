using Microsoft.EntityFrameworkCore;

namespace OJMonitor.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // 暂时不定义DbSet，等需要时再添加
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // 暂时不需要配置
        }
    }
}