using System;
namespace OnTrackWebService.Data
{
    public class UserInRole
    {
        public static bool Role(string Role, string Roles)
        {
            try
            {
                string[] roleList = Roles.Split(',');

                foreach (var _role in roleList)
                {
                    if (_role == Role)
                        return true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message, "User In Role");
            }

            return false;
        }
    }
}
