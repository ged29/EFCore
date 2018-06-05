using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;

namespace ComputerInventory.Models
{
    public partial class Machine
    {
        public int MachineId { get; set; }
        public string Name { get; set; }
        public string GeneralRole { get; set; }
        public string InstalledRoles { get; set; }

        public int MachineTypeId { get; set; }
        public MachineType MachineType { get; set; }

        public int OperatingSysId { get; set; }
        public OperatingSys OperatingSys { get; set; }

        public ICollection<SupportTicket> SupportTicket { get; set; }

        public Machine()
        {
            SupportTicket = new HashSet<SupportTicket>();
        }
    }

    public class MachineConfig : IEntityTypeConfiguration<Machine>
    {
        public void Configure(EntityTypeBuilder<Machine> builder)
        {
            builder.Property(p => p.MachineId).HasColumnName("MachineID");
            builder.Property(p => p.Name).IsRequired().HasMaxLength(25).IsUnicode(false);
            builder.Property(p => p.GeneralRole).IsRequired().HasMaxLength(25).IsUnicode(false);
            builder.Property(p => p.InstalledRoles).IsRequired().HasMaxLength(50).IsUnicode(false);

            builder.Property(p => p.MachineTypeId).HasColumnName("MachineTypeID");
            builder.Property(p => p.OperatingSysId).HasColumnName("OperatingSysID");

            builder.HasOne(p => p.MachineType)
                .WithMany(p => p.Machine)
                .HasForeignKey(p => p.MachineTypeId)
                .HasConstraintName("FK_MachineType")
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(p => p.OperatingSys)
                .WithMany(p => p.Machine)
                .HasForeignKey(p => p.OperatingSysId)
                .HasConstraintName("FK_OperatingSys")
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}