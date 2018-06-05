using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace ComputerInventory.Models
{
    public partial class SupportLog
    {
        public int SupportLogId { get; set; }
        public DateTime SupportLogEntryDate { get; set; }
        public string SupportLogEntry { get; set; }
        public string SupportLogUpdatedBy { get; set; }

        public int SupportTicketId { get; set; }
        public SupportTicket SupportTicket { get; set; }
    }

    public class SupportLogConfig : IEntityTypeConfiguration<SupportLog>
    {
        public void Configure(EntityTypeBuilder<SupportLog> builder)
        {
            builder.Property(p => p.SupportLogId).HasColumnName("SupportLogID");
            builder.Property(p => p.SupportLogEntry).IsRequired().IsUnicode(false);
            builder.Property(p => p.SupportLogEntryDate).HasColumnType("date");
            builder.Property(p => p.SupportLogUpdatedBy).IsRequired().HasMaxLength(50).IsUnicode(false);

            builder.HasOne(p => p.SupportTicket)
                .WithMany(p => p.SupportLog)
                .HasForeignKey(p => p.SupportTicketId)
                .HasConstraintName("FK_SupportTicket")
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}