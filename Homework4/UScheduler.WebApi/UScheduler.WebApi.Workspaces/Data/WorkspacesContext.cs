using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UScheduler.WebApi.Workspaces.Data.Entities;

namespace UScheduler.WebApi.Workspaces.Data
{
    public class WorkspacesContext : DbContext
    {
        public DbSet<Workspace> Workspaces { get; set; }

        public WorkspacesContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Workspace>(builder =>
            {
                builder.Property(w => w.Title)
                    .HasMaxLength(64)
                    .IsRequired();

                builder.Property(w => w.Owner)
                    .IsRequired();

                builder.Property(w => w.AccessType)
                    .IsRequired();

                builder.Property(w => w.WorkspaceType)
                    .IsRequired();

                var colabsComparer = new ValueComparer<List<string>>(
                    (c1, c2) => c1.SequenceEqual(c2),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList());

                builder.Property(w => w.Colabs)
                    .HasConversion(
                        p => JsonConvert.SerializeObject(p),
                        p => JsonConvert.DeserializeObject<List<string>>(p))
                    .Metadata
                    .SetValueComparer(colabsComparer);
            });
        }
    }
}
