using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPBibliotheque.RepositoryPattern
{
	internal interface IRepository<T> where T : class
	{
		// Version synchrone

		// INSERT
		public bool Add(T element);

		// DELETE
		public bool Delete(T element);

		// UPDATE
		public bool Update(T element);

		// SELECT par id
		public T? Get(int id);

		// SELECT tous
		public ICollection<T> GetAll();

		// SELECT plusieurs selon filtre
		public ICollection<T> GetAllFiltered(Func<T, bool> predicate);



		// Version aynchrone

		/*
		internal interface IRepositorySup<T> where T : class
		{
			// INSERT
			public Task<bool> AddAsync(T element);

			// DELETE
			public Task<bool> DeleteAsync(T element);

			// UPDATE
			public Task<bool> UpdateAsync(T element);

			// SELECT par id
			public Task<T?> GetByIdAsync(int id);

			// SELECT tous
			public Task<ICollection<T>> GetAllAsync();

			// SELECT plusieurs selon filtre
			public Task<ICollection<T>> FilterAsync(Func<T, bool> predicate);
		}
		*/


		// Version asynchrone et événementielle

		/*
		internal interface IRepositorySupEvents<T> where T : class
		{
			public event Action<string> ElementAdded;
			public event Action<string> ElementDeleted;
			public event Action<string> ElementUpdated;

			// On crée ensuite dans les classes héritées les méthodes qui vont s'abonner à ces événements.
			// Puis on déclenche ces événements dans les méthodes suivantes, là où on a besoin.

			// INSERT
			public Task<bool> AddAsync(T element);

			// DELETE
			public Task<bool> DeleteAsync(T element);

			// UPDATE
			public Task<bool> UpdateAsync(T element);

			// SELECT par id
			public Task<T?> GetByIdAsync(int id);

			// SELECT tous
			public Task<ICollection<T>> GetAllAsync();

			// SELECT plusieurs selon filtre
			public Task<ICollection<T>> FilterAsync(Func<T, bool> predicate);
		}
		*/
	}
}
