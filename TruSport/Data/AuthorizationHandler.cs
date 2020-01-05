using System;
using System.Threading.Tasks;
using TruSport.Services;
using Xamarin.Essentials;

namespace TruSport.Data
{
    public class AuthorizationHandler
    {
		UserService userService;

		public AuthorizationHandler()
		{
			userService = new UserService();
		}

		public async Task<bool> UserInRole(string role)
		{
			string userRole = await SecureStorage.GetAsync("UserRole");

			//Profile profile = await profileService.Get(profileID);

			if (userRole != null)
			{
				if (role.Contains(","))
				{
					string[] roleList = role.Split(',');

					foreach (var roleName in roleList)
					{
						if (roleName == userRole)
						{
							return true;
						}

					}
				}
				else
				{
					if (role == userRole)
						return true;
				}
			}

			return false;
		}

		//public async Task<bool> UserInRole(string role)
		//{
		//    string profileID = await SecureStorage.GetAsync("PlayerID");

		//    Profile profile = await profileService.Get(profileID);

		//    if (profile != null)
		//    {
		//        if (role.Contains(profile.Role.Name))
		//            return true;
		//    }

		//    return false;
		//}
	}
}
