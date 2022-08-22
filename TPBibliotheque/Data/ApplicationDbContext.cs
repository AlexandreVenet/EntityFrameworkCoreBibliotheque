
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPBibliotheque.ClassesData;

namespace TPBibliotheque.Data
{
	internal class ApplicationDbContext : DbContext
	{
		#region Properties

		public DbSet<Livre> Livres { get; set; }
		public DbSet<Auteur> Auteurs { get; set; }
		public DbSet<Editeur> Editeurs { get; set; }

		#endregion



		#region Overrides

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			// config de connexion avec les secrets
			var config = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory()) // Définir le chemin au répertoire actuel
				.AddUserSecrets<Program>() // Rendre les secrets disponibles au niveau de la classe Program
				.Build(); // Construire

			// Configuration de la connexion du contexte à la bdd
			optionsBuilder.UseSqlServer(config.GetConnectionString("Default"));
		}

		#endregion
	}
}
