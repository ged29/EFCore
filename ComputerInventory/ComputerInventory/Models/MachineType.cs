using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;

namespace ComputerInventory.Models
{
    public partial class MachineType
    {
        public int MachineTypeId { get; set; }
        public string Description { get; set; }

        public ICollection<Machine> Machine { get; set; }

        public MachineType()
        {
            Machine = new HashSet<Machine>();
        }
    }

    public class MachineTypeConfig : IEntityTypeConfiguration<MachineType>
    {
        public void Configure(EntityTypeBuilder<MachineType> builder)
        {
            builder.Property(p => p.MachineTypeId).HasColumnName("MachineTypeID");
            builder.Property(p => p.Description).HasMaxLength(15).IsUnicode(false);
        }
    }
}