using System;
using System.Collections.Generic;
using System.Text;
using DataLayer.Models;
using Services.Repositories;

namespace Services.Services
{
    public class Core : IDisposable
    {
        private NurseryContext _context = new NurseryContext();

        private MainRepo<TblConfig> _config;
        private MainRepo<TblField> _field;
        private MainRepo<TblForm> _form;
        private MainRepo<TblFormFieldRel> _formFieldRel;
        private MainRepo<TblPage> _page;
        private MainRepo<TblPageFormRel> _pageFormRel;
        private MainRepo<TblRegex> _regex;
        private MainRepo<TblRole> _role;
        private MainRepo<TblRolePageRel> _rolePageRel;
        private MainRepo<TblUser> _user;
        private MainRepo<TblUserLog> _userLog;
        private MainRepo<TblUserRoleRel> _userRoleRel;
        private MainRepo<TblValue> _value;

        public MainRepo<TblConfig> Config => _config ??= new MainRepo<TblConfig>(_context);
        public MainRepo<TblField> Field => _field ??= new MainRepo<TblField>(_context);
        public MainRepo<TblForm> Form => _form ??= new MainRepo<TblForm>(_context);
        public MainRepo<TblFormFieldRel> FormFieldRel => _formFieldRel ??= new MainRepo<TblFormFieldRel>(_context);
        public MainRepo<TblPage> Page => _page ??= new MainRepo<TblPage>(_context);
        public MainRepo<TblPageFormRel> PageFormRel => _pageFormRel ??= new MainRepo<TblPageFormRel>(_context);
        public MainRepo<TblRegex> Regex => _regex ??= new MainRepo<TblRegex>(_context);
        public MainRepo<TblRole> Role => _role ??= new MainRepo<TblRole>(_context);
        public MainRepo<TblRolePageRel> RolePageRel => _rolePageRel ??= new MainRepo<TblRolePageRel>(_context);
        public MainRepo<TblUser> User => _user ??= new MainRepo<TblUser>(_context);
        public MainRepo<TblUserLog> UserLog => _userLog ??= new MainRepo<TblUserLog>(_context);
        public MainRepo<TblUserRoleRel> UserRoleRel => _userRoleRel ??= new MainRepo<TblUserRoleRel>(_context);
        public MainRepo<TblValue> Value => _value ??= new MainRepo<TblValue>(_context);
        public virtual void Save() => _context.SaveChanges();


        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
