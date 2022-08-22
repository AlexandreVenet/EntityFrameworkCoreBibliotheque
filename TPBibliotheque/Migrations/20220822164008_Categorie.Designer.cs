﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TPBibliotheque.Data;

#nullable disable

namespace TPBibliotheque.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20220822164008_Categorie")]
    partial class Categorie
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("TPBibliotheque.ClassesData.Auteur", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("Date_Naissance")
                        .HasColumnType("datetime2");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Prenom")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Auteurs");
                });

            modelBuilder.Entity("TPBibliotheque.ClassesData.Editeur", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Editeurs");
                });

            modelBuilder.Entity("TPBibliotheque.ClassesData.Livre", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("AuteurId")
                        .HasColumnType("int");

                    b.Property<int>("Categorie")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date_Publication")
                        .HasColumnType("datetime2");

                    b.Property<int>("EditeurId")
                        .HasColumnType("int");

                    b.Property<string>("Isbn_10")
                        .IsRequired()
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnType("varchar(10)");

                    b.Property<string>("Isbn_13")
                        .IsRequired()
                        .HasMaxLength(15)
                        .IsUnicode(false)
                        .HasColumnType("varchar(15)");

                    b.Property<string>("Titre")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.HasKey("Id");

                    b.HasIndex("AuteurId");

                    b.HasIndex("EditeurId");

                    b.ToTable("Livres");
                });

            modelBuilder.Entity("TPBibliotheque.ClassesData.Livre", b =>
                {
                    b.HasOne("TPBibliotheque.ClassesData.Auteur", "Auteur")
                        .WithMany("Livres")
                        .HasForeignKey("AuteurId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TPBibliotheque.ClassesData.Editeur", "Editeur")
                        .WithMany("Livres")
                        .HasForeignKey("EditeurId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Auteur");

                    b.Navigation("Editeur");
                });

            modelBuilder.Entity("TPBibliotheque.ClassesData.Auteur", b =>
                {
                    b.Navigation("Livres");
                });

            modelBuilder.Entity("TPBibliotheque.ClassesData.Editeur", b =>
                {
                    b.Navigation("Livres");
                });
#pragma warning restore 612, 618
        }
    }
}
