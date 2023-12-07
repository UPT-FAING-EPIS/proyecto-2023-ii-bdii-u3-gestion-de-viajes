using System;
using System.Collections.Generic;
using ClienteAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ClienteAPI.Data;

public partial class BdClientesContext : DbContext
{
    public BdClientesContext()
    {
    }

    public BdClientesContext(DbContextOptions<BdClientesContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cliente> Clientes { get; set; }
    public virtual DbSet<TipoCorreo> TipoCorreos { get; set; }

    public virtual DbSet<TipoDireccion> TipoDireccions { get; set; }

    public virtual DbSet<TipoResidencium> TipoResidencia { get; set; }

    public virtual DbSet<TipoSeguro> TipoSeguros { get; set; }

    public virtual DbSet<TiposDocumento> TiposDocumentos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ClienteDB");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.IdCliente);

            entity.ToTable("CLIENTES");

            entity.Property(e => e.IdCliente).HasColumnName("ID_CLIENTE");
            entity.Property(e => e.ApeMaterno)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("APE_MATERNO");
            entity.Property(e => e.ApePaterno)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("APE_PATERNO");
            entity.Property(e => e.Genero)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("GENERO");
            entity.Property(e => e.NomCliente)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("NOM_CLIENTE");
            entity.Property(e => e.Numero).HasColumnName("NUMERO");
        });

        modelBuilder.Entity<TipoCorreo>(entity =>
        {
            entity.HasKey(e => e.IdTipoCorreo);

            entity.ToTable("TIPO_CORREO");

            entity.Property(e => e.IdTipoCorreo)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID_TIPO_CORREO");
            entity.Property(e => e.DesTipoCorreo)
                .HasMaxLength(200)
                .HasColumnName("DES_TIPO_CORREO");
            entity.Property(e => e.IdCli).HasColumnName("ID_CLI");
            entity.Property(e => e.TipoCorreo1)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("TIPO_CORREO");

            entity.HasOne(d => d.IdCliNavigation).WithMany(p => p.TipoCorreos)
                .HasForeignKey(d => d.IdCli)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TIPO_CORREO_CLIENTES");
        });

        modelBuilder.Entity<TipoDireccion>(entity =>
        {
            entity.HasKey(e => e.IdDireccion);

            entity.ToTable("TIPO_DIRECCION");

            entity.Property(e => e.IdDireccion)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID_DIRECCION");
            entity.Property(e => e.Calle)
                .HasMaxLength(50)
                .HasColumnName("CALLE");
            entity.Property(e => e.DesTipoDireccion)
                .HasMaxLength(200)
                .HasColumnName("DES_TIPO_DIRECCION");
            entity.Property(e => e.IdCli).HasColumnName("ID_CLI");
            entity.Property(e => e.Referencia)
                .HasColumnType("text")
                .HasColumnName("REFERENCIA");
            entity.Property(e => e.TipoDireccion1)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("TIPO_DIRECCION");

            entity.HasOne(d => d.IdCliNavigation).WithMany(p => p.TipoDireccions)
                .HasForeignKey(d => d.IdCli)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TIPO_DIRECCION_CLIENTES");
        });

        modelBuilder.Entity<TipoResidencium>(entity =>
        {
            entity.HasKey(e => e.IdResidencia);

            entity.ToTable("TIPO_RESIDENCIA");

            entity.Property(e => e.IdResidencia)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID_RESIDENCIA");
            entity.Property(e => e.Ciudad)
                .HasMaxLength(50)
                .HasColumnName("CIUDAD");
            entity.Property(e => e.DesTipResi)
                .HasMaxLength(30)
                .HasColumnName("DES_TIP_RESI");
            entity.Property(e => e.IdCli).HasColumnName("ID_CLI");
            entity.Property(e => e.Pais)
                .HasMaxLength(50)
                .HasColumnName("PAIS");
            entity.Property(e => e.Provincia)
                .HasMaxLength(50)
                .HasColumnName("PROVINCIA");

            entity.HasOne(d => d.IdCliNavigation).WithMany(p => p.TipoResidencia)
                .HasForeignKey(d => d.IdCli)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TIPO_RESIDENCIA_CLIENTES");
        });

        modelBuilder.Entity<TipoSeguro>(entity =>
        {
            entity.HasKey(e => e.IdSeguro);

            entity.ToTable("TIPO_SEGURO");

            entity.Property(e => e.IdSeguro)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID_SEGURO");
            entity.Property(e => e.Cobertura)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("COBERTURA");
            entity.Property(e => e.DocumentoSeguro)
                .HasColumnType("image")
                .HasColumnName("DOCUMENTO_SEGURO");
            entity.Property(e => e.IdCli).HasColumnName("ID_CLI");
            entity.Property(e => e.Poliza)
                .HasMaxLength(30)
                .HasColumnName("POLIZA");
            entity.Property(e => e.TipSeguro)
                .HasMaxLength(40)
                .HasColumnName("TIP_SEGURO");

            entity.HasOne(d => d.IdCliNavigation).WithMany(p => p.TipoSeguros)
                .HasForeignKey(d => d.IdCli)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TIPO_SEGURO_CLIENTES");
        });

        modelBuilder.Entity<TiposDocumento>(entity =>
        {
            entity.HasKey(e => e.IdTipoDocumento);

            entity.ToTable("TIPOS_DOCUMENTOS");

            entity.Property(e => e.IdTipoDocumento)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID_TIPO_DOCUMENTO");
            entity.Property(e => e.DesTipoDocumento)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("DES_TIPO_DOCUMENTO");
            entity.Property(e => e.FechaEmision)
                .HasColumnType("date")
                .HasColumnName("FECHA_EMISION");
            entity.Property(e => e.FechaVencimiento)
                .HasColumnType("date")
                .HasColumnName("FECHA_VENCIMIENTO");
            entity.Property(e => e.IdCli).HasColumnName("ID_CLI");
            entity.Property(e => e.Imagen)
                .HasColumnType("image")
                .HasColumnName("IMAGEN");
            entity.Property(e => e.NumDocumento).HasColumnName("NUM_DOCUMENTO");

            entity.HasOne(d => d.IdCliNavigation).WithMany(p => p.TiposDocumentos)
                .HasForeignKey(d => d.IdCli)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TIPOS_DOCUMENTOS_CLIENTES");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
