using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPBibliotheque.ClassesData;
using TPBibliotheque.Data;
using TPBibliotheque.RepositoryPattern;

namespace TPBibliotheque.ClassesIHM
{
	/// <summary>
	/// IHM avec menu.
	/// </summary>
	internal class IHM
	{
		#region Fields

		/// <summary>
		/// Conserver le choix utilisateur lors de la navigation. 
		/// <br/> Valeur réinitialisée lors d'un choix d'option.
		/// </summary>
		private int _userChoice;

		/// <summary>
		/// Context de connexion à la bdd.
		/// </summary>
		private ApplicationDbContext _context = new();

		/// <summary>
		/// Repository de Auteur.
		/// </summary>
		private RepositoryAuteur _repositoryAuteur = new();

		/// <summary>
		/// Repository de Editeur.
		/// </summary>
		private RepositoryEditeur _repositoryEditeur = new();

		/// <summary>
		/// Repository de Livre.
		/// </summary>
		private RepositoryLivre _repositoryLivre = new();

		#endregion



		#region Constructors

		/// <summary>
		/// Démarrage automatique de l'IHM.
		/// </summary>
		public IHM()
		{
			Demarrer();
		}

		#endregion



		#region Programme : général

		public void Demarrer()
		{
			Title("Accueil bibliothèque numérique");

			Console.WriteLine("Bienvenue dans ce programme.");

			Menu(new Option[]
			{
				new("Auteurs", Auteurs),
				new("Éditeurs", Editeurs),
				new("Livres", Livres),
				new("Quitter", ()=> Environment.Exit(0))
			}, ref _userChoice);
		}

		public void Auteurs()
		{
			Title("Auteurs");

			Console.WriteLine("Gérons des auteurs.");

			Menu(new Option[]
			{
				new("Lire tout", AuteursGetAll),
				new("Lire", AuteursGet),
				new("Ajouter", AuteursAdd),
				new("Modifier", AuteursUpdate),
				new("Supprimer", AuteursDelete),
				new("Retour", Demarrer)
			}, ref _userChoice);
		}

		public void Editeurs()
		{
			Title("Éditeurs");

			Console.WriteLine("Gérons des maisons d'édition.");

			Menu(new Option[]
			{
				new("Consulter tout", EditeursGetAll),
				new("Consulter", EditeursGet),
				new("Ajouter", EditeursAdd),
				new("Modifier", EditeursUpdate),
				new("Supprimer", EditeursDelete),
				new("Retour", Demarrer)
			}, ref _userChoice);
		}

		public void Livres()
		{
			Title("Livres");

			Console.WriteLine("Gérons des livres.");

			Menu(new Option[]
			{
				new("Consulter tout", LivresGetAll),
				new("Consulter", LivresGet),
				new("Ajouter", LivresAddAuteur),
				new("Modifier", LivresUpdate),
				new("Supprimer", LivresDelete),
				new("Retour", Demarrer)
			}, ref _userChoice);
		}

		#endregion



		#region Programme : auteurs

		// SELECT tout
		private void AuteursGetAll()
		{
			Title("Lire tous les auteurs");

			// v1 : standard
			//List<Auteur> liste = _context.Auteurs.ToList();

			// v2 : Repository Pattern
			ICollection<Auteur> liste = _repositoryAuteur.GetAll();

			if (liste.Count == 0)
			{
				Console.WriteLine("Pas d'auteurs.");
			}
			else
			{
				foreach (var item in liste)
				{
					Console.WriteLine(item);
				}
			}

			Menu(new Option("Retour", Auteurs));
		}

		// SELECT par id
		private void AuteursGet()
		{
			Title("Lire un auteur par id");

			int id = -1;

			while (id < 0)
			{
				Console.Write("Entrer un id : ");
				int userId = id;
				if(!int.TryParse(Console.ReadLine(), out userId))
				{
					Console.WriteLine($"\tEntrer un nombre entier.");
					continue;
				}

				if(userId < 0)
				{
					Console.WriteLine($"\tEntrée invalide.");
					continue;
				}

				id = userId;
				break;
			}

			// v1 : Standard
			/* Auteur? auteur = _context.Auteurs
				.Include(x => x.Livres)
				.FirstOrDefault(x => x.Id == id); // "?" juste pour éviter le soulignement
			*/

			// v2 : Repository Pattern
			Auteur? auteur = _repositoryAuteur.Get(id);

			if(auteur == null)
			{
				Console.WriteLine("Pas d'auteur à cet id.");
			}
			else
			{
				Console.WriteLine(auteur);
				if (auteur.Livres.Count == 0)
				{
					Console.WriteLine("\tPas de livres.");
				}
				else
				{
					Console.WriteLine($"\tNombre de livres : {auteur.Livres.Count}");
					foreach (var item in auteur.Livres)
					{
						Console.WriteLine($"\t{item}");
					}
				}
			}

			Menu(new Option("Retour", Auteurs));
		}

		// INSERT
		private void AuteursAdd()
		{
			Title("Ajouter un auteur");

			Auteur newAuteur = new();

			Console.Write("Entrer un prénom : ");
			newAuteur.Prenom = Console.ReadLine();

			Console.Write("Entrer un nom : ");
			newAuteur.Nom = Console.ReadLine();

			while (true)
			{
				Console.Write("Entrer une date de naissance : ");
				DateOnly dateOnlyTemp;
				if(!DateOnly.TryParse(Console.ReadLine(),out dateOnlyTemp))
				{
					Console.WriteLine($"\tEntrer une date valide.");
					continue;
				}

				newAuteur.Date_Naissance = dateOnlyTemp.ToDateTime(new TimeOnly(0,0));
				break;
			}

			Menu(new Option[]
			{
				new("Annuler", Auteurs),
				new("Ajouter", ()=> AuteursAddOk(newAuteur))
			}, ref _userChoice);
		}

		// ... fin
		private void AuteursAddOk(Auteur newAuteur)
		{
			Title("Ajout d'un nouveaul auteur");

			/*_context.Auteurs.Add(newAuteur);

			if (_context.SaveChanges() == 1)
			{
				Console.WriteLine("Auteur ajouté.");
			}
			else
			{
				Console.WriteLine("Erreur, auteur non ajouté.");
			}*/

			// Avec Repository Pattern

			if (_repositoryAuteur.Add(newAuteur))
			{
				Console.WriteLine("Auteur ajouté.");
			}
			else
			{
				Console.WriteLine("Erreur, auteur non ajouté.");
			}

			Menu(new Option("Retour", Auteurs));
		}

		// UPDATE
		private void AuteursUpdate()
		{
			Title("Modifier un auteur");

			int id = -1;

			while (id < 0)
			{
				Console.Write("Entrer un id : ");
				int userId = id;
				if (!int.TryParse(Console.ReadLine(), out userId))
				{
					Console.WriteLine($"\tEntrer un nombre entier.");
					continue;
				}

				if (userId < 0)
				{
					Console.WriteLine($"\tEntrée invalide.");
					continue;
				}

				id = userId;
				break;
			}

			// v1 : standard
			/*
			Auteur? auteur = _context.Auteurs
				.Include(x => x.Livres)
				.FirstOrDefault(x => x.Id == id);
			*/

			// v2 : Repository Pattern
			Auteur? auteur =_repositoryAuteur.Get(id);

			if (auteur == null)
			{
				Console.WriteLine("Pas d'auteur à cet id.");
				Menu(new Option("Retour", Auteurs));
			}
			else
			{
				Console.WriteLine(auteur);

				Menu(new Option[]
				{
				new("Annuler", Auteurs),
				new("Modifier", ()=> AuteursUpdateOk(auteur))
				}, ref _userChoice);
			}
		}

		// ... fin
		private void AuteursUpdateOk(Auteur auteur)
		{
			Title("Modification de l'auteur");

			Console.Write($"Entrer un prénom ({auteur.Prenom}) : ");
			auteur.Prenom = Console.ReadLine();

			Console.Write($"Entrer un prénom ({auteur.Nom}) : ");
			auteur.Nom = Console.ReadLine();

			while (true)
			{
				Console.Write($"Entrer une date de naissance ({auteur.Date_Naissance.ToString("d")}): ");
				DateOnly dateOnlyTemp;
				if (!DateOnly.TryParse(Console.ReadLine(), out dateOnlyTemp))
				{
					Console.WriteLine($"\tEntrer une date valide.");
					continue;
				}

				auteur.Date_Naissance = dateOnlyTemp.ToDateTime(new TimeOnly(0, 0));
				break;
			}

			// v1 : standard
			/*_context.Auteurs.Update(auteur);

			if(_context.SaveChanges() > 0)
			{
				Console.WriteLine("Auteur modifié.");
			}
			else
			{
				Console.WriteLine("Erreur, auteur non modifié.");
			}
			*/

			// v2 : Repository Pattern
			if(_repositoryAuteur.Update(auteur))
			{
				Console.WriteLine("Auteur modifié.");
			}
			else
			{
				Console.WriteLine("Erreur, auteur non modifié.");
			}

			Menu(new Option("Retour", Auteurs));
		}

		// DELETE
		private void AuteursDelete()
		{
			Title("Supprimer un auteur par id");

			int id = -1;

			while (id < 0)
			{
				Console.Write("Entrer un id : ");
				int userId = id;
				if (!int.TryParse(Console.ReadLine(), out userId))
				{
					Console.WriteLine($"\tEntrer un nombre entier.");
					continue;
				}

				if (userId < 0)
				{
					Console.WriteLine($"\tEntrée invalide.");
					continue;
				}

				id = userId;
				break;
			}

			// v1 : standard
			/*Auteur? auteur = _context.Auteurs
				.Include(x => x.Livres)
				.FirstOrDefault(x => x.Id == id);*/

			// v2 : Repository Pattern
			Auteur? auteur = _repositoryAuteur.Get(id);

			if (auteur == null)
			{
				Console.WriteLine("Pas d'auteur à cet id.");
				Menu(new Option("Retour", Auteurs));
			}
			else
			{
				Console.WriteLine(auteur);

				Menu(new Option[]
				{
				new("Annuler", Auteurs),
				new("Supprimer", ()=> AuteursDeleteOk(auteur))
				}, ref _userChoice);
			}
		}

		// ... fin
		private void AuteursDeleteOk(Auteur auteur)
		{
			Title("Suppression de l'auteur");

			/*
			_context.Auteurs.Remove(auteur);

			if(_context.SaveChanges() == 1)
			{
				Console.WriteLine("L'auteur a été supprimé.");
			}
			else
			{
				Console.WriteLine("Erreur de suppression");
			}
			*/

			// Avec Repository Pattern

			if(_repositoryAuteur.Delete(auteur))
			{
				Console.WriteLine("L'auteur a été supprimé.");
			}
			else
			{
				Console.WriteLine("Erreur de suppression.");
			}

			Menu(new Option("Retour", Auteurs));
		}

		#endregion



		#region Programme : éditeurs

		// SELECT tout
		private void EditeursGetAll()
		{
			Title("Lire tous les éditeurs");

			ICollection<Editeur> liste = _repositoryEditeur.GetAll();

			if (liste.Count == 0)
			{
				Console.WriteLine("Pas d'éditeur.");
			}
			else
			{
				foreach (var item in liste)
				{
					Console.WriteLine(item);
				}
			}

			Menu(new Option("Retour", Editeurs));
		}

		// SELECT par id
		private void EditeursGet()
		{
			Title("Lire un éditeur par id");

			int id = -1;

			while (id < 0)
			{
				Console.Write("Entrer un id : ");
				int userId = id;
				if (!int.TryParse(Console.ReadLine(), out userId))
				{
					Console.WriteLine($"\tEntrer un nombre entier.");
					continue;
				}

				if (userId < 0)
				{
					Console.WriteLine($"\tEntrée invalide.");
					continue;
				}

				id = userId;
				break;
			}

			Editeur? editeur = _repositoryEditeur.Get(id);

			if (editeur == null)
			{
				Console.WriteLine("Pas d'éditeur à cet id.");
			}
			else
			{
				Console.WriteLine(editeur);
				if (editeur.Livres.Count == 0)
				{
					Console.WriteLine("\tPas de livres.");
				}
				else
				{
					foreach (var livre in editeur.Livres)
					{
						Console.WriteLine($"\t{livre}, par {livre.Auteur.Prenom} {livre.Auteur.Nom}");
					}
				}
			}

			Menu(new Option("Retour", Editeurs));
		}

		// INSERT
		private void EditeursAdd()
		{
			Title("Ajouter un éditeur");

			Editeur newEd = new();

			Console.Write("Entrer un nom : ");
			newEd.Nom = Console.ReadLine();

			Menu(new Option[]
			{
				new("Annuler", Editeurs),
				new("Ajouter", ()=> EditeursAddOk(newEd))
			}, ref _userChoice);
		}

		// ... fin
		private void EditeursAddOk(Editeur newEd)
		{
			Title("Ajout d'un nouvel éditeur");

			if (_repositoryEditeur.Add(newEd))
			{
				Console.WriteLine("Éditeur ajouté.");
			}
			else
			{
				Console.WriteLine("Erreur, éditeur non ajouté.");
			}

			Menu(new Option("Retour", Editeurs));
		}

		// UPDATE
		private void EditeursUpdate()
		{
			Title("Modifier un éditeur");

			int id = -1;

			while (id < 0)
			{
				Console.Write("Entrer un id : ");
				int userId = id;
				if (!int.TryParse(Console.ReadLine(), out userId))
				{
					Console.WriteLine($"\tEntrer un nombre entier.");
					continue;
				}

				if (userId < 0)
				{
					Console.WriteLine($"\tEntrée invalide.");
					continue;
				}

				id = userId;
				break;
			}

			Editeur? editeur = _repositoryEditeur.Get(id);

			if (editeur == null)
			{
				Console.WriteLine("Pas d'éditeur à cet id.");
				Menu(new Option("Retour", Editeurs));
			}
			else
			{
				Console.WriteLine(editeur);
				Console.WriteLine("\nModifier ?");

				Menu(new Option[]
				{
				new("Annuler", Editeurs),
				new("Modifier", ()=> EditeursUpdateOk(editeur))
				}, ref _userChoice);
			}
		}

		// ... fin
		private void EditeursUpdateOk(Editeur editeur)
		{
			Title("Modification de l'éditeur");

			Console.Write($"Entrer un nom ({editeur.Nom}) : ");
			editeur.Nom = Console.ReadLine();

			if (_repositoryEditeur.Update(editeur))
			{
				Console.WriteLine("Éditeur modifié.");
			}
			else
			{
				Console.WriteLine("Erreur, éditeur non modifié.");
			}

			Menu(new Option("Retour", Editeurs));
		}

		// DELETE
		private void EditeursDelete()
		{
			Title("Supprimer un éditeur par id");

			int id = -1;

			while (id < 0)
			{
				Console.Write("Entrer un id : ");
				int userId = id;
				if (!int.TryParse(Console.ReadLine(), out userId))
				{
					Console.WriteLine($"\tEntrer un nombre entier.");
					continue;
				}

				if (userId < 0)
				{
					Console.WriteLine($"\tEntrée invalide.");
					continue;
				}

				id = userId;
				break;
			}

			Editeur? editeur = _repositoryEditeur.Get(id);

			if (editeur == null)
			{
				Console.WriteLine("Pas d'éditeur à cet id.");
				Menu(new Option("Retour", Editeurs));
			}
			else
			{
				Console.WriteLine(editeur);

				Menu(new Option[]
				{
				new("Annuler", Editeurs),
				new("Supprimer", ()=> EditeursDeleteOk(editeur))
				}, ref _userChoice);
			}
		}

		// ... fin
		private void EditeursDeleteOk(Editeur editeur)
		{
			Title("Suppression de l'éditeur");

			if (_repositoryEditeur.Delete(editeur))
			{
				Console.WriteLine("L'éditeur a été supprimé.");
			}
			else
			{
				Console.WriteLine("Erreur de suppression.");
			}

			Menu(new Option("Retour", Editeurs));
		}

		#endregion



		#region Livres

		// SELECT tout
		private void LivresGetAll()
		{
			Title("Lire tous les livres");

			ICollection<Livre> liste = _repositoryLivre.GetAll();

			if (liste.Count == 0)
			{
				Console.WriteLine("Pas de livre.");
			}
			else
			{
				foreach (var item in liste)
				{
					Console.WriteLine(item);
				}
			}

			Menu(new Option("Retour", Livres));
		}

		// SELECT par id
		private void LivresGet()
		{
			Title("Lire un livre par id");

			int id = -1;

			while (id < 0)
			{
				Console.Write("Entrer un id : ");
				int userId = id;
				if (!int.TryParse(Console.ReadLine(), out userId))
				{
					Console.WriteLine($"\tEntrer un nombre entier.");
					continue;
				}

				if (userId < 0)
				{
					Console.WriteLine($"\tEntrée invalide.");
					continue;
				}

				id = userId;
				break;
			}

			Livre? livre = _repositoryLivre.Get(id);

			if (livre == null)
			{
				Console.WriteLine("Pas de livre à cet id.");
			}
			else
			{
				Console.WriteLine(livre);
				Console.WriteLine($"\tAuteur : {livre.Auteur}");
				Console.WriteLine($"\tÉditeur : {livre.Editeur}");
			}

			Menu(new Option("Retour", Livres));
		}

		// INSERT / 1 : chercher l'auteur
		private void LivresAddAuteur()
		{
			Title("Ajouter un livre : choisir l'auteur");

			int id = -1;

			while (id < 0)
			{
				Console.Write("Entrer un id d'auteur : ");
				int userId = id;
				if (!int.TryParse(Console.ReadLine(), out userId))
				{
					Console.WriteLine($"\tEntrer un nombre entier.");
					continue;
				}

				if (userId < 0)
				{
					Console.WriteLine($"\tEntrée invalide.");
					continue;
				}

				id = userId;
				break;
			}

			Auteur? auteur = _repositoryAuteur.Get(id);

			if (auteur == null)
			{
				Console.WriteLine("Pas d'auteur à cet id.");

				Menu(new Option[]
				{
					new("Annuler", Livres),
					new("Recommencer", LivresAddAuteur)
				}, ref _userChoice);
			}
			else
			{
				Console.WriteLine(auteur);

				Menu(new Option[]
				{
					new("Annuler", Livres),
					new("Recommencer", LivresAddAuteur),
					new("Sélectionner", ()=> LivresAddEditeur(auteur))
				}, ref _userChoice);
			}
		}

		// INSERT / 2 : chercher l'éditeur
		private void LivresAddEditeur(Auteur auteur)
		{
			Title("Ajouter un livre : choisir l'éditeur");

			int id = -1;

			while (id < 0)
			{
				Console.Write("Entrer un id d'éditeur : ");
				int userId = id;
				if (!int.TryParse(Console.ReadLine(), out userId))
				{
					Console.WriteLine($"\tEntrer un nombre entier.");
					continue;
				}

				if (userId < 0)
				{
					Console.WriteLine($"\tEntrée invalide.");
					continue;
				}

				id = userId;
				break;
			}

			Editeur? editeur = _repositoryEditeur.Get(id);

			if (editeur == null)
			{
				Console.WriteLine("Pas d'éditeur à cet id.");

				Menu(new Option[]
				{
					new("Annuler", Livres),
					new("Recommencer", ()=> LivresAddEditeur(auteur))
				}, ref _userChoice);
			}
			else
			{
				Console.WriteLine(editeur);

				Menu(new Option[]
				{
					new("Annuler", Livres),
					new("Recommencer", ()=> LivresAddEditeur(auteur)),
					new("Sélectionner", ()=> LivresAddLivre(auteur, editeur))
				}, ref _userChoice);
			}
		}

		// INSERT / 3 : créer le livre
		private void LivresAddLivre(Auteur auteur, Editeur editeur)
		{
			Title("Ajouter un livre : créer le livre");

			Livre newLivre = new();

			while (true)
			{
				Console.Write("Entrer une catégorie (A, B ou C) : ");
				
				string userStr = Console.ReadLine();
				if(String.IsNullOrEmpty(userStr))
				{
					Console.WriteLine("\tEntrer une valeur.");
					continue;
				}

				userStr = userStr.ToUpper();
				if (!Enum.GetNames(typeof(Categorie)).Contains(userStr))
				{
					Console.WriteLine("\tEntrer A, B ou C.");
					continue;
				}

				Categorie categorie;
				if(!Enum.TryParse(userStr, out categorie))
				{
					Console.WriteLine($"\tEntrer une catégorie valide.");
					continue;
				}

				newLivre.Categorie = categorie;
				break;
			}

			Console.Write("Entrer un titre : ");
			newLivre.Titre = Console.ReadLine();

			while (true)
			{
				Console.Write("Entrer une date de publication : ");
				DateOnly dateOnlyTemp;
				if (!DateOnly.TryParse(Console.ReadLine(), out dateOnlyTemp))
				{
					Console.WriteLine($"\tEntrer une date valide.");
					continue;
				}

				newLivre.Date_Publication = dateOnlyTemp.ToDateTime(new TimeOnly(0, 0));
				break;
			}

			Console.Write("Entrer un ISBN 10 : ");
			newLivre.Isbn_10 = Console.ReadLine();

			Console.Write("Entrer un ISBN 13 : ");
			newLivre.Isbn_13 = Console.ReadLine();

			newLivre.AuteurId = auteur.Id;
			newLivre.EditeurId = editeur.Id;

			Menu(new Option[]
			{
				new("Annuler", Livres),
				new("Recommencer", ()=> LivresAddLivre(auteur, editeur)),
				new("Ajouter", ()=> LivresAddOk(newLivre))
			}, ref _userChoice);


		}

		// ... fin
		private void LivresAddOk(Livre newLivre)
		{
			Title("Ajout d'un nouveau livre");

			if (_repositoryLivre.Add(newLivre))
			{
				Console.WriteLine("Livre ajouté.");
			}
			else
			{
				Console.WriteLine("Erreur, livre non ajouté.");
			}

			Menu(new Option("Retour", Livres));
		}

		// UPDATE / 1 : chercher
		private void LivresUpdate()
		{
			Title("Modifier un livre : chercher");

			int id = -1;

			while (id < 0)
			{
				Console.Write("Entrer un id : ");
				int userId = id;
				if (!int.TryParse(Console.ReadLine(), out userId))
				{
					Console.WriteLine($"\tEntrer un nombre entier.");
					continue;
				}

				if (userId < 0)
				{
					Console.WriteLine($"\tEntrée invalide.");
					continue;
				}

				id = userId;
				break;
			}

			Livre? livre = _repositoryLivre.Get(id);

			if (livre == null)
			{
				Console.WriteLine("Pas de livre à cet id.");
				Menu(new Option("Retour", Livres));
			}
			else
			{
				/*Console.WriteLine(livre);

				Menu(new Option[]
				{
				new("Annuler", Livres),
				new("Modifier", ()=> LivresUpdateMenu(livre))
				}, ref _userChoice);*/

				Console.Clear();
				LivresUpdateMenu(livre);
			}
		}

		// UPDATE / 2 : menu de sélection
		private void LivresUpdateMenu(Livre livre)
		{
			Title("Modifier un livre : propriétés");

			Console.WriteLine(livre);
			Console.WriteLine($"\tAuteur : {livre.Auteur.Prenom} {livre.Auteur.Nom}");
			Console.WriteLine($"\tÉditeur : {livre.Editeur.Nom}");

			Console.WriteLine("\nChoisir la propriété à modifier.");

			Menu(new Option[]
			{
				new("Catégorie", ()=> LivresModifierCategorie(livre)),
				new("Titre", ()=> LivresModifierTitre(livre)),
				new("Date publication", ()=> LivresModifierDate(livre)),
				new("ISBN 10", ()=> LivresModifierISBN10(livre)),
				new("ISBN 13", ()=> LivresModifierISBN13(livre)),
				new("Auteur", ()=> LivresModifierAuteur(livre)),
				new("Editeur", ()=> LivresModifierEditeur(livre)),
				new("Recommencer", LivresUpdate),
				new("Annuler", Livres)
			}, ref _userChoice);
		}

		// UPDATE / modifier chaque propriété 
		private void LivresModifierCategorie(Livre livre)
		{
			Title("Modifier un livre : catégorie");

			while (true)
			{
				Console.Write("Entrer une catégorie (A, B ou C) : ");

				string userStr = Console.ReadLine();
				if (String.IsNullOrEmpty(userStr))
				{
					Console.WriteLine("\tEntrer une valeur.");
					continue;
				}

				userStr = userStr.ToUpper();
				if (!Enum.GetNames(typeof(Categorie)).Contains(userStr))
				{
					Console.WriteLine("\tEntrer A, B ou C.");
					continue;
				}

				Categorie categorie;
				if (!Enum.TryParse(userStr, out categorie))
				{
					Console.WriteLine($"\tEntrer une catégorie valide.");
					continue;
				}

				livre.Categorie = categorie;
				break;
			}


			if (_repositoryLivre.Update(livre))
			{
				Console.WriteLine("Livre modifié.");
			}
			else
			{
				Console.WriteLine("Erreur, livre non modifié.");
			}

			Menu(new Option("Retour", () => LivresUpdateMenu(livre)));
		}

		private void LivresModifierTitre(Livre livre)
		{
			Title("Modifier un livre : titre");

			Console.Write($"Entrer un titre ({livre.Titre}) : ");
			livre.Titre = Console.ReadLine();

			if (_repositoryLivre.Update(livre))
			{
				Console.WriteLine("Livre modifié.");
			}
			else
			{
				Console.WriteLine("Erreur, livre non modifié.");
			}

			Menu(new Option("Retour", ()=> LivresUpdateMenu(livre)));
		}
		
		private void LivresModifierDate(Livre livre)
		{
			Title("Modifier un livre : date de publication");

			while (true)
			{
				Console.Write($"Entrer une date ({livre.Date_Publication.ToString("d")}) : ");
				DateOnly dateOnlyTemp;
				if (!DateOnly.TryParse(Console.ReadLine(), out dateOnlyTemp))
				{
					Console.WriteLine($"\tEntrer une date valide.");
					continue;
				}

				livre.Date_Publication = dateOnlyTemp.ToDateTime(new TimeOnly(0, 0));
				break;
			}

			if (_repositoryLivre.Update(livre))
			{
				Console.WriteLine("Livre modifié.");
			}
			else
			{
				Console.WriteLine("Erreur, livre non modifié.");
			}

			Menu(new Option("Retour", () => LivresUpdateMenu(livre)));
		}

		private void LivresModifierISBN10(Livre livre)
		{
			Title("Modifier un livre : ISBN 10");

			Console.Write($"Entrer un ISBN10 ({livre.Isbn_10}) : ");
			livre.Isbn_10 = Console.ReadLine();

			if (_repositoryLivre.Update(livre))
			{
				Console.WriteLine("Livre modifié.");
			}
			else
			{
				Console.WriteLine("Erreur, livre non modifié.");
			}

			Menu(new Option("Retour", () => LivresUpdateMenu(livre)));
		}

		private void LivresModifierISBN13(Livre livre)
		{
			Title("Modifier un livre : ISBN 13");

			Console.Write($"Entrer un ISBN13 ({livre.Isbn_13}) : ");
			livre.Isbn_13 = Console.ReadLine();

			if (_repositoryLivre.Update(livre))
			{
				Console.WriteLine("Livre modifié.");
			}
			else
			{
				Console.WriteLine("Erreur, livre non modifié.");
			}

			Menu(new Option("Retour", () => LivresUpdateMenu(livre)));
		}

		private void LivresModifierAuteur(Livre livre)
		{
			Title("Modifier un livre : auteur");

			int id = -1;

			while (id < 0)
			{
				Console.Write("Entrer un id d'auteur : ");
				int userId = id;
				if (!int.TryParse(Console.ReadLine(), out userId))
				{
					Console.WriteLine($"\tEntrer un nombre entier.");
					continue;
				}

				if (userId < 0)
				{
					Console.WriteLine($"\tEntrée invalide.");
					continue;
				}

				id = userId;
				break;
			}

			Auteur? auteur = _repositoryAuteur.Get(id);

			if (auteur == null)
			{
				Console.WriteLine("Pas d'auteur à cet id.");

				Menu(new Option[]
				{
					new("Annuler", ()=> LivresUpdateMenu(livre)),
					new("Recommencer", ()=> LivresModifierAuteur(livre))
				}, ref _userChoice);
			}
			else
			{
				Console.WriteLine(auteur);

				Menu(new Option[]
				{
					new("Annuler", ()=> LivresUpdateMenu(livre)),
					new("Recommencer", ()=> LivresModifierAuteur(livre)),
					new("Sélectionner", ()=> LivresModifierAuteurOk(livre, id))
				}, ref _userChoice);
			}
		}

		private void LivresModifierAuteurOk(Livre livre, int auteurId)
		{
			Title("Modifier l'auteur d'un livre");

			livre.AuteurId = auteurId;

			bool okUpdate = _repositoryLivre.Update(livre);

			if (okUpdate)
			{
				Console.WriteLine("Livre modifié.");
			}
			else
			{
				Console.WriteLine("Erreur, livre non modifié.");
			}

			if(okUpdate)
			{
				Livre? livreOk = _repositoryLivre.Get(livre.Id);

				if (livreOk == null)
				{
					Console.WriteLine("Erreur sur livre.");

					Menu(new Option("Retour", () => LivresUpdateMenu(livre)));
				}
				else
				{
					Menu(new Option("Retour", () => LivresUpdateMenu(livreOk)));
				}
			}
		}

		private void LivresModifierEditeur(Livre livre)
		{
			Title("Modifier un livre : éditeur");

			int id = -1;

			while (id < 0)
			{
				Console.Write("Entrer un id d'éditeur : ");
				int userId = id;
				if (!int.TryParse(Console.ReadLine(), out userId))
				{
					Console.WriteLine($"\tEntrer un nombre entier.");
					continue;
				}

				if (userId < 0)
				{
					Console.WriteLine($"\tEntrée invalide.");
					continue;
				}

				id = userId;
				break;
			}

			Editeur? editeur = _repositoryEditeur.Get(id);

			if (editeur == null)
			{
				Console.WriteLine("Pas d'éditeur à cet id.");

				Menu(new Option[]
				{
					new("Annuler", ()=> LivresUpdateMenu(livre)),
					new("Recommencer", ()=> LivresModifierEditeur(livre))
				}, ref _userChoice);
			}
			else
			{
				Console.WriteLine(editeur);

				Menu(new Option[]
				{
					new("Annuler", ()=> LivresUpdateMenu(livre)),
					new("Recommencer", ()=> LivresModifierEditeur(livre)),
					new("Sélectionner", ()=> LivresModifierEditeurOk(livre, id))
				}, ref _userChoice);
			}
		}

		private void LivresModifierEditeurOk(Livre livre, int editeurId)
		{
			Title("Modifier l'éditeur d'un livre");

			livre.EditeurId = editeurId;

			bool okUpdate = _repositoryLivre.Update(livre);

			if (okUpdate)
			{
				Console.WriteLine("Livre modifié.");
			}
			else
			{
				Console.WriteLine("Erreur, livre non modifié.");
			}

			if (okUpdate)
			{
				Livre? livreOk = _repositoryLivre.Get(livre.Id);

				if (livreOk == null)
				{
					Console.WriteLine("Erreur sur livre.");

					Menu(new Option("Retour", () => LivresUpdateMenu(livre)));
				}
				else
				{
					Menu(new Option("Retour", () => LivresUpdateMenu(livreOk)));
				}
			}
		}

		// DELETE
		private void LivresDelete()
		{
			Title("Supprimer un livre par id");

			int id = -1;

			while (id < 0)
			{
				Console.Write("Entrer un id : ");
				int userId = id;
				if (!int.TryParse(Console.ReadLine(), out userId))
				{
					Console.WriteLine($"\tEntrer un nombre entier.");
					continue;
				}

				if (userId < 0)
				{
					Console.WriteLine($"\tEntrée invalide.");
					continue;
				}

				id = userId;
				break;
			}

			Livre? livre = _repositoryLivre.Get(id);

			if (livre == null)
			{
				Console.WriteLine("Pas de livre à cet id.");
				Menu(new Option("Retour", Livres));
			}
			else
			{
				Console.WriteLine(livre);

				Menu(new Option[]
				{
				new("Annuler", Livres),
				new("Supprimer", ()=> LivresDeleteOk(livre))
				}, ref _userChoice);
			}
		}

		// ... fin
		private void LivresDeleteOk(Livre livre)
		{
			Title("Suppression de livre");

			if (_repositoryLivre.Delete(livre))
			{
				Console.WriteLine("Le livre a été supprimé.");
			}
			else
			{
				Console.WriteLine("Erreur de suppression.");
			}

			Menu(new Option("Retour", Livres));
		}

		#endregion



		#region Menu controls

		/// <summary>
		/// Créer un menu avec une seule entrée.
		/// </summary>
		/// <param name="option">L'option pour cette entrée.</param>
		private void Menu(Option option)
		{
			// Afficher le menu

			Console.BackgroundColor = ConsoleColor.White;
			Console.ForegroundColor = ConsoleColor.Black;
			Console.WriteLine($"\n  {option.p_title} ");
			Console.ResetColor();
			Console.WriteLine("\n[ENTREE valider]");

			ConsoleKey key = default;

			while (key != ConsoleKey.Enter)
			{
				key = Console.ReadKey(true).Key;
			}

			// Ici, la key est Enter.

			Console.Clear(); // Avant ce qui suit
			option.p_action();
		}

		/// <summary>
		/// Créer un menu à partir d'un tableau d'options.
		/// <br/>La navigation réécrit le menu à la position du curseur à chaque fois.
		/// </summary>
		/// <param name="menuOptions">Tableau d'options.</param>
		/// <param name="currentChoice">Variable (en référence), conservant l'index de l'option choisie.</param>
		private void Menu(Option[] menuOptions, ref int currentChoice)
		{
			while (true)
			{
				// Connaître la position du curseur (tuple)

				(int Left, int Top, int Size) cursorStart = (Console.CursorLeft, Console.CursorTop, Console.CursorSize);

				// Afficher le menu

				Console.WriteLine();

				for (int i = 0; i < menuOptions.Length; i++)
				{
					if (i == currentChoice)
					{
						Console.BackgroundColor = ConsoleColor.White;
						Console.ForegroundColor = ConsoleColor.Black;
					}
					else
					{
						Console.BackgroundColor = ConsoleColor.Black;
						Console.ForegroundColor = ConsoleColor.White;
					}
					Console.WriteLine($"  {menuOptions[i].p_title} ");
					Console.ResetColor();
				}

				Console.WriteLine("\n[HAUT/BAS naviguer] [ENTREE valider]");

				// Choix utilisateur

				ConsoleKey key = default;

				while (key != ConsoleKey.Enter && key != ConsoleKey.UpArrow && key != ConsoleKey.DownArrow)
				{
					key = Console.ReadKey(true).Key;
				}

				// Ici, la key est l'une de celles autorisées.

				if (key == ConsoleKey.DownArrow)
				{
					currentChoice++;
				}
				else if (key == ConsoleKey.UpArrow)
				{
					currentChoice--;
				}
				else if (key == ConsoleKey.Enter)
				{
					// Effacer (ici et pas après sinon non considéré)
					Console.Clear();

					// Conserver le choix utilisateur et réinitialiser ce dernier
					int savedChoice = currentChoice;
					currentChoice = 0;

					// Lancer l'action avec le choix sauvegardé
					menuOptions[savedChoice].p_action();

					// Arrêter ici
					break;
				}

				if (currentChoice < 0)
				{
					currentChoice = menuOptions.Length - 1;
				}
				else if (currentChoice > menuOptions.Length - 1)
				{
					currentChoice = 0;
				}

				// Réécriture à la position du curseur (et non Console.Clear())

				Console.SetCursorPosition(cursorStart.Left, cursorStart.Top);
			}
		}

		#endregion



		#region UI

		/// <summary>
		/// Afficher un titre entre deux traits.
		/// </summary>
		/// <param name="str">Le titre.</param>
		private void Title(string str)
		{
			Line();
			Console.WriteLine(str);
			Line();
			Console.WriteLine();
		}

		/// <summary>
		/// Afficher un trait sur 50 caractères.
		/// </summary>
		private void Line()
		{
			Console.WriteLine(new String('═', 50));
		}

		#endregion
	}
}
