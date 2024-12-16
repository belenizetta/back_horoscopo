using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace back_horoscopo.Infrastructure.Models;

public partial class HoroscopoContext : DbContext
{
    public HoroscopoContext()
    {
    }

    public HoroscopoContext(DbContextOptions<HoroscopoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Consulta> Consultas { get; set; }

    public virtual DbSet<Estadisticassigno> Estadisticassignos { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
     //   => optionsBuilder.UseMySql("server=localhost;database=Horoscopo;user=root", Microsoft.EntityFrameworkCore.ServerVersion.Parse("5.7.37-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("latin1_swedish_ci")
            .HasCharSet("latin1");

        modelBuilder.Entity<Consulta>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("consultas");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.FechaConsulta).HasColumnType("datetime");
            entity.Property(e => e.Signo).HasMaxLength(50);
            entity.Property(e => e.UduarioId).HasColumnType("int(11)");
        });

        modelBuilder.Entity<Estadisticassigno>(entity =>
        {
            entity
                .HasKey(e => e.Signo).HasName("PRIMARY");
                entity.ToTable("estadisticassigno");

            entity.Property(e => e.CantidadConsultas).HasColumnType("int(11)");
            entity.Property(e => e.Signo).HasMaxLength(50);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("usuarios");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.FechaNacimiento).HasColumnType("datetime");
            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
