using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tuuto.Model
{
    class DraftDbContext : DbContext
    {
        public DbSet<DraftModel> Draft { get; set; }
        public DbSet<MediaData> MediaData { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=draft.db");
        }
    }
}
