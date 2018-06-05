using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace ComputerInventory.Models
{
    public partial class SupportTicket
    {
        public int SupportTicketId { get; set; }
        public DateTime DateReported { get; set; }
        public DateTime? DateResolved { get; set; }
        public string IssueDescription { get; set; }
        public string IssueDetail { get; set; }
        public string TicketOpenedBy { get; set; }

        public int MachineId { get; set; }
        public Machine Machine { get; set; }

        public ICollection<SupportLog> SupportLog { get; set; }

        public SupportTicket()
        {
            SupportLog = new HashSet<SupportLog>();
        }
    }

    public class SupportTicketConfig : IEntityTypeConfiguration<SupportTicket>
    {
        public void Configure(EntityTypeBuilder<SupportTicket> builder)
        {
            builder.Property(p => p.SupportTicketId).HasColumnName("SupportTicketID");
            builder.Property(p => p.DateReported).HasColumnType("date");
            builder.Property(p => p.DateResolved).HasColumnType("date");
            builder.Property(p => p.IssueDescription).IsRequired().HasMaxLength(150).IsUnicode(false);
            builder.Property(p => p.IssueDetail).IsUnicode(false);
            builder.Property(p => p.TicketOpenedBy).IsRequired().HasMaxLength(50).IsUnicode(false);

            builder.Property(p => p.MachineId).HasColumnName("MachineID");

            builder.HasOne(p => p.Machine)
                .WithMany(p => p.SupportTicket)
                .HasForeignKey(p => p.MachineId)
                .HasConstraintName("FK_MachineID")
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
