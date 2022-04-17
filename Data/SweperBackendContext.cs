using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SweperBackend.Data
{
    public class SweperBackendContext : DbContext
    {
        public SweperBackendContext (DbContextOptions<SweperBackendContext> options)
            : base(options)
        {
        }

        public DbSet<SweperBackend.Data.User> User { get; set; }
    }
}
