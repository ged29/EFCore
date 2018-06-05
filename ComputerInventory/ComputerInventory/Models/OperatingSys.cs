using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;

namespace ComputerInventory.Models
{
    public partial class OperatingSys
    {
        public int OperatingSysId { get; set; }
        public string Name { get; set; }
        public bool StillSupported { get; set; }

        public ICollection<Machine> Machine { get; set; }

        public OperatingSys()
        {
            Machine = new HashSet<Machine>();
        }
    }

    public class OperatingSysConfig : IEntityTypeConfiguration<OperatingSys>
    {
        public void Configure(EntityTypeBuilder<OperatingSys> builder)
        {
            builder.Property(p => p.OperatingSysId).HasColumnName("OperatingSysID");

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(35)
                .IsUnicode(false);
        }
    }
}
