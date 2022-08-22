using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPBibliotheque.ClassesData;

namespace TPBibliotheque.RepositoryPattern
{
	internal class RepositoryEditeur : RepositoryBase, IRepository<Editeur>
	{
		public bool Add(Editeur element)
		{
			if (element == null) throw new ArgumentNullException();

			_context.Editeurs.Add(element);
			return _context.SaveChanges() == 1;
		}

		public bool Delete(Editeur element)
		{
			if (element == null) throw new ArgumentNullException();

			_context.Editeurs.Remove(element);
			return _context.SaveChanges() > 0; // et non ==1 car jointures
		}

		public Editeur? Get(int id)
		{
			if(id < 0) throw new ArgumentOutOfRangeException();

			return _context.Editeurs
				.Include(x => x.Livres).ThenInclude(x => x.Auteur)
				.FirstOrDefault(x => x.Id == id); // "?" juste pour éviter le soulignement
		}

		public ICollection<Editeur> GetAll()
		{
			return _context.Editeurs.ToList();
		}

		public ICollection<Editeur> GetAllFiltered(Func<Editeur, bool> predicate)
		{
			throw new NotImplementedException();
		}

		public bool Update(Editeur element)
		{
			if (element == null) throw new ArgumentNullException();

			_context.Editeurs.Update(element);
			return _context.SaveChanges() > 0;
		}
	}
}
