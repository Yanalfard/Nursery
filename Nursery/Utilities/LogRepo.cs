using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nursery.Utilities
{
    public static class LogRepo
    {
        public static string UserLogin(string username)
        {
            return
                $"User: {username} logged";
        }

        public static string DeleteUserRole(string username, string userRoleId, string identificationNo)
        {
            return
                $"User: {username} Deleted UserRole: {userRoleId} From Identification :{identificationNo}";
        } 
       
        public static string AddUser(string username, string AdduserName)
        {
            return
                $"User: {username} Add User: {AdduserName}";
        } 
        public static string AddRole(string username, string AdduserName)
        {
            return
                $"User: {username} Add Role: {AdduserName}";
        }
        public static string EditRole(string username, string AdduserName)
        {
            return
                $"User: {username} Edit Role: {AdduserName}";
        }
        public static string EditUser(string username, string AdduserName)
        {
            return
                $"User: {username} Edit User: {AdduserName}";
        }
        public static string EditUserPassword(string username, string AdduserName)
        {
            return
                $"User: {username} Edit Password User: {AdduserName}";
        }
        public static string DeleteUser(string username, string AdduserName)
        {
            return
                $"User: {username} Delete User: {AdduserName}";
        } 
        public static string AddUserRoleRel(string username, string roleName,string roleusername)
        {
            return
                 $"User: {username} Add Role : {roleName} in shift From Identification :{roleusername}";
        }
        public static string EditUserRoleRel(string username, string roleName,string roleusername)
        {
            return
                 $"User: {username} Edit Role : {roleName} in shift From Identification :{roleusername}";
        }

        public static string AddKid(string username, string createUser, string nickname)
        {
            return
                $"کاربر {username} کودک {createUser}  با نام مستعار {nickname}را ایجاد کرد";
        }
        public static string EditKid(string username, string createUser, string nickname)
        {
            return
                $"کاربر {username} کودک {createUser}  با نام مستعار {nickname}را ویرایش کرد";
        }
        public static string DeleteKid(string username, string createUser, string nickname)
        {
            return
                $"کاربر {username} کودک {createUser}  با نام مستعار {nickname}را حذف کرد";
        }
        public static string AddPage(string username, string namePage)
        {
            return
                $"کاربر {username} بخش {namePage} را ایجاد کرد";
        }
        public static string EditPage(string username, string namePage)
        {
            return
                $"کاربر {username} بخش {namePage} را ویرایش کرد";
        }
        public static string DeletePage(string username, string namePage)
        {
            return
                $"کاربر {username} بخش {namePage} را حذف  کرد";
        }
        public static string AddRolePageRel(string username,string nameRole, string namePage)
        {
            return
                $"کاربر {username} بخش {namePage} برای شیفت {nameRole}    تعریف کرد    ";
        }
        public static string EditRolePageRel(string username, string nameRole, string namePage)
        {
            return
                $"کاربر {username} بخش {namePage} برای شیفت {nameRole}    ویرایش کرد    ";
        }
        public static string DeleteRolePageRel(string username, string nameRole, string namePage)
        {
            return
                $"کاربر {username} بخش  {namePage}  برای شیفت  {nameRole}  حذف کرد    ";
        }
    }
}
