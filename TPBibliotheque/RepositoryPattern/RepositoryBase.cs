using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPBibliotheque.Data;

namespace TPBibliotheque.RepositoryPattern
{
	// Classe à hériter et qui contient le lien avec le contexte de
	// Abstraite pour ne pas rendre possible son instanciation
	internal abstract class RepositoryBase
	{
		// propriété héritable
		protected ApplicationDbContext _context = new();
	}
}
