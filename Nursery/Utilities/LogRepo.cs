using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nursery.Utilities
{
    public static class LogRepo
    {
        public static string UserLogin(string username, string ip)
        {
            return
                $"User: {username} logged in IP: {ip}";
        }

        public static string DeleteUserRole(string username, string userRoleId, string identificationNo)
        {
            return
                $"User: {username} Deleted UserRole: {userRoleId} From Identification :{identificationNo}";
        }
    }
}
