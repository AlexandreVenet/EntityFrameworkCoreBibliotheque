using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPBibliotheque.ClassesIHM
{
	/// <summary>
	/// Option de menu.
	/// </summary>
	internal class Option
	{
		#region Fields

		/// <summary>
		/// Titre de l'option.
		/// </summary>
		private string _title;

		/// <summary>
		/// Action de l'option (delegate).
		/// </summary>
		private Action _action;

		#endregion



		#region Properties

		/// <summary>
		/// Titre de l'option.
		/// </summary>
		public string p_title { get => _title; set => _title = value; }

		/// <summary>
		/// Action de l'option (delegate).
		/// </summary>
		public Action p_action { get => _action; set => _action = value; }

		#endregion



		#region Constructors

		/// <summary>
		/// Définir une option de menu : un titre et une callback (delegate).
		/// </summary>
		/// <param name="title">Titre de l'option.</param>
		/// <param name="action">Callback de l'option (delegate).</param>
		public Option(string title, Action action)
		{
			p_title = title;
			_action = action;
		}

		#endregion
	}
}
