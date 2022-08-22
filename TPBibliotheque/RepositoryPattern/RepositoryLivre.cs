
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPBibliotheque.ClassesData;

namespace TPBibliotheque.RepositoryPattern
{
	internal class RepositoryLivre : RepositoryBase, IRepository<Livre>
	{
		public bool Add(Livre element)
		{
			if (element == null) throw new ArgumentNullException();

			_context.Livres.Add(element);
			return _context.SaveChanges() > 0;
		}

		public bool Delete(Livre element)
		{
			if (element == null) throw new ArgumentNullException();

			_context.Livres.Remove(element);
			return _context.SaveChanges() == 1;
		}

		public Livre? Get(int id)
		{
			if(id < 0) throw new ArgumentOutOfRangeException();

			return _context.Livres
				.Include(x => x.Auteur)
				.Include(x => x.Editeur)
				.FirstOrDefault(x => x.Id == id); // "?" juste pour éviter le soulignement
		}

		public ICollection<Livre> GetAll()
		{
			return _context.Livres.ToList();
		}

		public ICollection<Livre> GetAllFiltered(Func<Livre, bool> predicate)
		{
			throw new NotImplementedException();
		}

		public bool Update(Livre element)
		{
			if (element == null) throw new ArgumentNullException();

			_context.Livres.Update(element);
			return _context.SaveChanges() > 0;
		}
	}
}
