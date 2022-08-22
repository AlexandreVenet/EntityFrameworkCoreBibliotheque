using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPBibliotheque.ClassesData
{
	internal class Auteur
	{
		#region Colonnes de base

		public int Id { get; set; }

		[StringLength(50)]
		[Required]
		public string Prenom { get; set; }

		[StringLength(50)]
		[Required]
		public string Nom { get; set; }

		[Required]
		public DateTime Date_Naissance { get; set; }

		#endregion



		#region Clés étrangères

		public virtual ICollection<Livre> Livres { get; set; }
		public int NbreLivres
		{
			get => Livres.Count;
		}

		#endregion



		#region Overrides

		public override string ToString()
		{
			return $"{Id}. {Prenom} {Nom}, naissance : {Date_Naissance.ToString("d")}";
		}

		#endregion
	}
}
