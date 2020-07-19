using System;
using FakeBrewery.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FakeBrewery.Infra.Data
{
    public class BreweryContext : DbContext
    {
        public DbSet<Beer> Beers { get; set; }
        public DbSet<Brewery> Breweries { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Wholesaler> Wholesalers { get; set; }
        
        public BreweryContext(DbContextOptions<BreweryContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Data configuring

            modelBuilder.Entity<Brewery>()
                .HasMany(br => br.Beers)
                .WithOne(b => b.Brewery);

            modelBuilder.Entity<Stock>()
                .HasOne(s => s.Beer)
                .WithMany(b => b.Stocks)
                .HasForeignKey(b => b.BeerId);

            modelBuilder.Entity<Stock>()
                .HasOne(s => s.Wholesaler)
                .WithMany(w => w.Stocks)
                .HasForeignKey(s => s.WholesalerId);

            // Data seeding

            modelBuilder.Entity<Brewery>().HasData(
                new Brewery() { Id = new Guid("cd876cae-ff5b-429d-970b-11af42900f1b"), Name = "Abbaye de Leffe" },
                new Brewery() { Id = new Guid("91bff65f-96f2-4bd1-8b2e-eeaef2b46555"), Name = "Abbaye d'Orval" },
                new Brewery() { Id = new Guid("a5a1d759-7471-431e-92c0-0f40c35bc855"), Name = "Abbaye de Westmalle" },
                new Brewery() { Id = new Guid("d661055d-5c38-4201-b937-79b1b5d77f8f"), Name = "Brasserie Bosteels" }
            );

            modelBuilder.Entity<Wholesaler>().HasData(
                new Wholesaler() { Id = new Guid("28cade6d-4d40-4fa2-96f5-e535a07aad7b"), Name = "BeerLovers' Shop" },
                new Wholesaler() { Id = new Guid("9779f2fa-6f60-4fa9-9b18-28fb2505be6e"), Name = "Beer Market" }
            );
        }
    }
}