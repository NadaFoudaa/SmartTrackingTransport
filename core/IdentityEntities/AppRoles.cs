using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IdentityEntities
{
	public static class AppRoles
	{
		public const string Admin = "Admin";
		public const string Driver = "Driver";
		public const string User = "User";

		public static readonly string[] AllRoles = new string[]
		{
			Admin,
			Driver,
			User
		};
	}
}
