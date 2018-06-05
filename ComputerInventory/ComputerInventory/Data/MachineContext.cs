using ComputerInventory.Models;
using Microsoft.EntityFrameworkCore;

namespace ComputerInventory.Data
{
    class MachineContext : DbContext
    {
        public virtual DbSet<Machine> Machine { get; set; }
        public virtual DbSet<MachineType> MachineType { get; set; }
        public virtual DbSet<MachineWarranty> MachineWarranty { get; set; }
        public virtual DbSet<OperatingSys> OperatingSys { get; set; }
        public virtual DbSet<SupportLog> SupportLog { get; set; }
        public virtual DbSet<SupportTicket> SupportTicket { get; set; }
        public virtual DbSet<WarrantyProvider> WarrantyProvider { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=.;Database=BegEFCore2;Trusted_Connection=false;User ID=sa;Password=123");
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .ApplyConfiguration(new MachineConfig())
                .ApplyConfiguration(new MachineTypeConfig())
                .ApplyConfiguration(new MachineWarrantyConfig())
                .ApplyConfiguration(new OperatingSysConfig())
                .ApplyConfiguration(new SupportLogConfig())
                .ApplyConfiguration(new SupportTicketConfig())
                .ApplyConfiguration(new WarrantyProviderConfig());

            base.OnModelCreating(modelBuilder);
        }
    }
}
