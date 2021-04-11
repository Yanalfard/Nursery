using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nursery.Utilities
{
    public class LogRepo
    {
        public string UserLogin(string username, string ip)
        {
            return
                $"User: {username} logged in IP: {ip}";
        }
    }
}
