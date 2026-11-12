using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using zooManagerPro.Models;

namespace zooManagerPro.Context;

public partial class ZooManagerProContext : DbContext
{
    public ZooManagerProContext()
    {
    }

    public ZooManagerProContext(DbContextOptions<ZooManagerProContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Animal> Animals { get; set; }

    public virtual DbSet<AnimalsFeeding> AnimalsFeedings { get; set; }

    public virtual DbSet<ClimateZone> ClimateZones { get; set; }

    public virtual DbSet<DietaryRegiman> DietaryRegimen { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Enclosure> Enclosures { get; set; }

    public virtual DbSet<EnclosureStatus> EnclosureStatuses { get; set; }

    public virtual DbSet<Feature> Features { get; set; }

    public virtual DbSet<Gender> Genders { get; set; }

    public virtual DbSet<HealthStatus> HealthStatuses { get; set; }

    public virtual DbSet<Species> Species { get; set; }

    public virtual DbSet<TypeOfEnclosure> TypeOfEnclosures { get; set; }

    public virtual DbSet<TypeOfFeeding> TypeOfFeedings { get; set; }

    public virtual DbSet<UnitOfMass> UnitOfMasses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost; Database=zooManagerPro; Username=postgres; Password=123;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Animal>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("_animal__pk");

            entity.ToTable("Animal");

            entity.HasOne(d => d.DietaryRegimen).WithMany(p => p.Animals)
                .HasForeignKey(d => d.DietaryRegimenId)
                .HasConstraintName("animal_dietaryregimen_fk");

            entity.HasOne(d => d.Enclosure).WithMany(p => p.Animals)
                .HasForeignKey(d => d.EnclosureId)
                .HasConstraintName("animal_enclosure_fk");

            entity.HasOne(d => d.Gender).WithMany(p => p.Animals)
                .HasForeignKey(d => d.GenderId)
                .HasConstraintName("animal_gender_fk");

            entity.HasOne(d => d.HealthStatus).WithMany(p => p.Animals)
                .HasForeignKey(d => d.HealthStatusId)
                .HasConstraintName("animal_healthstatus_fk");

            entity.HasOne(d => d.Species).WithMany(p => p.Animals)
                .HasForeignKey(d => d.SpeciesId)
                .HasConstraintName("animal_species_fk");

            entity.HasMany(d => d.Features).WithMany(p => p.Animals)
                .UsingEntity<Dictionary<string, object>>(
                    "AnimalFeature",
                    r => r.HasOne<Feature>().WithMany()
                        .HasForeignKey("FeatureId")
                        .HasConstraintName("animalfeature_features_fk"),
                    l => l.HasOne<Animal>().WithMany()
                        .HasForeignKey("AnimalId")
                        .HasConstraintName("animalfeature_animal_fk"),
                    j =>
                    {
                        j.HasKey("AnimalId", "FeatureId").HasName("animalfeature_pk");
                        j.ToTable("AnimalFeature");
                    });
        });

        modelBuilder.Entity<AnimalsFeeding>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("_animalsfeeding__pk");

            entity.ToTable("AnimalsFeeding");

            entity.Property(e => e.FeedingDateTime).HasColumnType("timestamp without time zone");

            entity.HasOne(d => d.Animal).WithMany(p => p.AnimalsFeedings)
                .HasForeignKey(d => d.AnimalId)
                .HasConstraintName("animalsfeeding_animal_fk");

            entity.HasOne(d => d.Employee).WithMany(p => p.AnimalsFeedings)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("animalsfeeding_employee_fk");

            entity.HasOne(d => d.TypeOfFeeding).WithMany(p => p.AnimalsFeedings)
                .HasForeignKey(d => d.TypeOfFeedingId)
                .HasConstraintName("animalsfeeding_typeoffeeding_fk");

            entity.HasOne(d => d.UnitOfMass).WithMany(p => p.AnimalsFeedings)
                .HasForeignKey(d => d.UnitOfMassId)
                .HasConstraintName("animalsfeeding_unitofmass_fk");
        });

        modelBuilder.Entity<ClimateZone>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("_climatezone__pk");

            entity.ToTable("ClimateZone");
        });

        modelBuilder.Entity<DietaryRegiman>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("_dietaryregimen__pk");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("_employee__pk");

            entity.ToTable("Employee");
        });

        modelBuilder.Entity<Enclosure>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("_enclosure__pk");

            entity.ToTable("Enclosure");

            entity.HasOne(d => d.ClimateZone).WithMany(p => p.Enclosures)
                .HasForeignKey(d => d.ClimateZoneId)
                .HasConstraintName("enclosure_climatezone_fk");

            entity.HasOne(d => d.EnclosureStatus).WithMany(p => p.Enclosures)
                .HasForeignKey(d => d.EnclosureStatusId)
                .HasConstraintName("enclosure_enclosurestatus_fk");

            entity.HasOne(d => d.TypeOfEnclosure).WithMany(p => p.Enclosures)
                .HasForeignKey(d => d.TypeOfEnclosureId)
                .HasConstraintName("enclosure_typeofenclosure_fk");
        });

        modelBuilder.Entity<EnclosureStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("_enclosurestatus__pk");

            entity.ToTable("EnclosureStatus");
        });

        modelBuilder.Entity<Feature>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("_features__pk");
        });

        modelBuilder.Entity<Gender>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("_gender__pk");

            entity.ToTable("Gender");
        });

        modelBuilder.Entity<HealthStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("_healthstatus__pk");

            entity.ToTable("HealthStatus");
        });

        modelBuilder.Entity<Species>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("_species__pk");
        });

        modelBuilder.Entity<TypeOfEnclosure>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("_typeofenclosure__pk");

            entity.ToTable("TypeOfEnclosure");
        });

        modelBuilder.Entity<TypeOfFeeding>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("_typeoffeeding__pk");

            entity.ToTable("TypeOfFeeding");
        });

        modelBuilder.Entity<UnitOfMass>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("_unitofmass__pk");

            entity.ToTable("UnitOfMass");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
