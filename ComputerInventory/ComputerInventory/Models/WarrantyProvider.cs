using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;

namespace ComputerInventory.Models
{
    public partial class WarrantyProvider
    {
        public int WarrantyProviderId { get; set; }
        public string ProviderName { get; set; }
        public int? SupportExtension { get; set; }
        public string SupportNumber { get; set; }

        public ICollection<MachineWarranty> MachineWarranty { get; set; }

        public WarrantyProvider()
        {
            MachineWarranty = new HashSet<MachineWarranty>();
        }
    }

    public class WarrantyProviderConfig : IEntityTypeConfiguration<WarrantyProvider>
    {
        public void Configure(EntityTypeBuilder<WarrantyProvider> builder)
        {
            builder.Property(p => p.WarrantyProviderId).HasColumnName("WarrantyProviderID");
            builder.Property(e => e.ProviderName)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(e => e.SupportNumber)
                .IsRequired()
                .HasMaxLength(10)
                .IsUnicode(false);
        }
    }
}
