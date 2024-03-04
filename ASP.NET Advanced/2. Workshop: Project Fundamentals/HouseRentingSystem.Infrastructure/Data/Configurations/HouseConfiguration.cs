using HouseRentingSystem.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseRentingSystem.Infrastructure.Data.Configurations
{
    public class HouseConfiguration : IEntityTypeConfiguration<House>
    {
        public void Configure(EntityTypeBuilder<House> builder)
        {
            builder
                .HasOne(e => e.Category)
                .WithMany(e => e.Houses)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
              .HasOne(e => e.Agent)
              .WithMany(e => e.Houses)
              .HasForeignKey(e => e.AgentId)
              .OnDelete(DeleteBehavior.Restrict);

            var data = new DataSeeder();

            builder.HasData(new House[] { data.FirstHouse, data.SecondHouse, data.ThirdHouse});
        }
    }
}
