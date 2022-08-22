using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPBibliotheque.ClassesData
{
	internal class Editeur
	{
		#region Colonnes de base

		public int Id { get; set; }

		[StringLength(50)]
		[Required]
		public string Nom { get; set; }

		#endregion



		#region Clés étrangères

		public virtual ICollection<Livre> Livres { get; set; }

		#endregion



		#region Overrides

		public override string ToString()
		{
			return $"{Id}. {Nom}";
		}

		#endregion
	}
}
