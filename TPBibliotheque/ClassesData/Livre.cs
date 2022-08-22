using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPBibliotheque.ClassesData
{
	internal class Livre
	{
		#region Colonnes de base

		public int Id { get; set; }

		[Required]
		public Categorie Categorie { get; set; }

		[StringLength(150)]
		[Required]
		public string Titre { get; set; }
	
		[Required]
		public DateTime Date_Publication { get; set; }

		[Unicode(false)]
		[MaxLength(10)]
		[Required]
		public string Isbn_10 { get; set; }

		[Unicode(false)]
		[MaxLength(15)]
		[Required]
		public string Isbn_13 { get; set; }

		#endregion



		#region Clés étrangères

		// La suppression d'un auteur ou éditeur supprime les ouvrages correspondants.

		public int AuteurId { get; set; } // non nullable pour opérer la suppression en cascade
		public virtual Auteur Auteur { get; set; } // même comportement (objet nullable par défaut, pas besoin de spécifier)

		public int EditeurId { get; set; } // idem
		public virtual Editeur Editeur { get; set; } // idem

		#endregion



		#region Overrides

		public override string ToString()
		{
			return $"{Id}. {Categorie}. {Titre}, {Date_Publication.ToString("d")}, ISBN10 : {Isbn_10}, ISBN13 : {Isbn_13}";
		}

		#endregion
	}
}
