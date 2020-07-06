using System.Collections.Generic;
using SteelX.Shared;
using System;

namespace SteelX.Server
{
	public class User
	{
		/// <summary>
		/// Unique user Id for this user.
		/// </summary>
		public uint Id { get; set; }
		
		/// <summary>
		/// Possible remnant of social system. 
		/// Can be used as Alias, for Forums.
		/// </summary>
		/// Not used in game so far...
		public string Nickname { get; set; }

		/// <summary>
		/// Username the user uses to sign into their account
		/// </summary>
		public string Username { get; set; }

		/// <summary>
		/// The users password
		/// </summary>
		public string Password { get; set; }
		
		/// <summary>
		/// </summary>
		public string Email { get; set; }
		public string PasswordQuestion { get; set; }
		public string PasswordAnswer { get; set; }
		public string IsApproved { get; set; }
		public string Comment { get; set; }
		public DateTime CreationDate { get; set; }
		public DateTime LastPasswordChangedDate { get; set; }
		public DateTime LastActivityDate { get; set; }
		//public string ApplicationName { get; set; }
		public bool IsLockedOut { get; set; }
		public DateTime LastLockedOutDate { get; set; }
		public int FailedPasswordAttemptCount { get; set; }
		public DateTime FailedPasswordAttemptWindowStart { get; set; }
		public int FailedPasswordAnswerAttemptCount { get; set; }
		public DateTime FailedPasswordAnswerAttemptWindowStart { get; set; }
	}
}