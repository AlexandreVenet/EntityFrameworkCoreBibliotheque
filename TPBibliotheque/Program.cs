using TPBibliotheque.ClassesIHM;

namespace TPBibliotheque
{
	internal class Program
	{
		static void Main(string[] args)
		{
			// Paramètres de l'application Console
			Console.OutputEncoding = System.Text.Encoding.UTF8;
			Console.Title = "TP Bibliothèque numérique";

			new IHM();
		}
	}
}