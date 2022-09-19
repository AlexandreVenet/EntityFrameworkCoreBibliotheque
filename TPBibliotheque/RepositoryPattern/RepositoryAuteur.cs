using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPBibliotheque.ClassesData;

namespace TPBibliotheque.RepositoryPattern
{
	internal class RepositoryAuteur : RepositoryBase, IRepository<Auteur>
	{
		public bool Add(Auteur element)
		{
			if (element == null) throw new ArgumentNullException();

			_context.Auteurs.Add(element);
			return _context.SaveChanges() == 1;
		}

		public bool Delete(Auteur element)
		{
			if (element == null) throw new ArgumentNullException();

			_context.Auteurs.Remove(element);
			return _context.SaveChanges() > 0; // et non pas ==1 car jointures
		}

		public Auteur? Get(int id) // "?" juste pour éviter le soulignement
		{
			if(id < 0) throw new ArgumentOutOfRangeException();

			return _context.Auteurs
				.Include(x => x.Livres)
				.FirstOrDefault(x => x.Id == id); 
		}

		public ICollection<Auteur> GetAll()
		{
			return _context.Auteurs.ToList();
		}

		public ICollection<Auteur> GetAllFiltered(Func<Auteur, bool> predicate)
		{
			throw new NotImplementedException();
		}

		public bool Update(Auteur element)
		{
			if (element == null) throw new ArgumentNullException();

			_context.Auteurs.Update(element);
			return _context.SaveChanges() > 0;
		}
	}
}
