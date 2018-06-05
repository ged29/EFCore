using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace ComputerInventory.Models
{
    public partial class MachineWarranty
    {
        public int MachineWarrantyId { get; set; }
        public int MachineId { get; set; }
        public string ServiceTag { get; set; }
        public DateTime WarrantyExpiration { get; set; }

        public int WarrantyProviderId { get; set; }
        public WarrantyProvider WarrantyProvider { get; set; }
    }

    public class MachineWarrantyConfig : IEntityTypeConfiguration<MachineWarranty>
    {
        public void Configure(EntityTypeBuilder<MachineWarranty> builder)
        {
            builder.Property(p => p.MachineWarrantyId).HasColumnName("MachineWarrantyID");
            builder.Property(p => p.MachineId).HasColumnName("MachineID");
            builder.Property(p => p.ServiceTag).IsRequired().HasMaxLength(20).IsUnicode(false);
            builder.Property(p => p.WarrantyExpiration).HasColumnType("date");

            builder.Property(p => p.WarrantyProviderId).HasColumnName("WarrantyProviderID");

            builder.HasOne(p => p.WarrantyProvider)
                .WithMany(p => p.MachineWarranty)
                .HasForeignKey(p => p.WarrantyProviderId)                
                .HasConstraintName("FK_WarrantyProvider")
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}