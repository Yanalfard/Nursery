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
                $"کاربر: {username} وارد شد";
        }

        public static string DeleteUserRole(string username, string userRoleId, string identificationNo)
        {
            return
                $"کاربر: {username} شیفت: {userRoleId} از کابر  :{identificationNo} حذف کرد";
        }

        public static string AddUser(string username, string AdduserName)
        {
            return
                $"کاربر: {username} کاربر: {AdduserName}  رو اضافه کرد";
        }
        public static string AddRole(string username, string AdduserName)
        {
            return
                $"کاربر : {username} شیفت : {AdduserName} ایجاد کرد";
        }
        public static string EditRole(string username, string AdduserName)
        {
            return
               $"کاربر : {username} شیفت : {AdduserName} ویرایش کرد";
        }
        public static string EditUser(string username, string AdduserName)
        {
            return
                  $"کاربر: {username} کاربر: {AdduserName}  رو ویرایش کرد";
        }
        public static string EditUserPassword(string username, string AdduserName)
        {
            return
                 $"کاربر: {username} رمز کاربر: {AdduserName}  رو ویرایش کرد";
        }
        public static string DeleteUser(string usernameAdmin, string deleteUsername)
        {
            return
                $"کاربر: {usernameAdmin} کاربر : {deleteUsername} را حذف کرد";
        }
        public static string AddUserRoleRel(string usernameAdmin, string roleName, string roleusername)
        {
            return
                $"کاربر :{usernameAdmin} شیفت{roleName} رو به کاربر {roleusername}  اضافه کرد";

        }
        public static string EditUserRoleRel(string usernameAdmin, string roleName, string roleusername)
        {
            return
                 $"کاربر :{usernameAdmin} شیفت{roleName} رو به کاربر {roleusername}  ویرایش کرد";
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
        public static string AddRolePageRel(string username, string nameRole, string namePage)
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

        public static string AddPageFormRel(string username, string nameForm, string namePage)
        {
            return
                $"کاربر {username} بخش {namePage} برای فرم {nameForm}    تعریف کرد    ";
        }
        public static string DeletePageFormRel(string username, string nameForm, string namePage)
        {
            return
                $"کاربر {username} بخش {namePage} برای فرم {nameForm}    حذف  کرد    ";
        }


        public static string AddFormPageRel(string username, string nameForm, string namePage)
        {
            return
                $"کاربر {username} فرم  {nameForm}  را به بخش {namePage} اضافه کرد ";
        }
        public static string EditFormPageRel(string username, string nameForm, string namePage)
        {
            return
               $"کاربر {username} فرم  {nameForm} را از بخش  {namePage} ویرایش کرد ";
        }
        public static string DeleteFormPageRel(string username, string nameForm, string namePage)
        {
            return
               $"کاربر {username} فرم  {nameForm} را از بخش {namePage} حدف کرد ";
        }



        public static string AddForm(string username, string formName)
        {
            return
               $"کاربر: {username} فرم:  {formName} را ایجاد کرد";
        }
        public static string EditForm(string username, string formName)
        {
            return
               $"کاربر: {username} فرم:  {formName} را ویرایش کرد";
        }
        public static string DeleteForm(string username, string formName)
        {
            return
                $"کاربر: {username} فرم:  {formName} را حذف کرد";
        }




        public static string AddFormKid(string username, string formName,string kidName)
        {
            return
               $"کاربر: {username}  فرم   {formName} برای کودک {kidName} پر کرد";
        }
        public static string UpdateIsAcceptedFormKid(string username, string KidName, string userAdd)
        {
            return
               $"کاربر:  فرم: {username} فرم {KidName} که {userAdd} اضافه کرده بود رو تائید کرد ";
        }
        public static string UpdateOffAcceptedFormKid(string username, string KidName, string userAdd)
        {
            return
               $"کاربر:  فرم: {username} فرم {KidName} که {userAdd} اضافه کرده بود رو رد کرد ";
        }
        public static string DeleteFormKid(string username, string KidName, string userAdd)
        {
            return
                $" کاربر{username} فرم کودک  {KidName} که توسط کاربر  {userAdd}  اضافه شده بود رو حذف کرد ";
        }

    }
}
