using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PartyInvites.Models
{
    public class DataContext : DbContext
    {
        public DbSet<GuestResponse> Responses { get; set; }

        public DataContext(DbContextOptions options)
            : base(options)
        {
        }
    }
}