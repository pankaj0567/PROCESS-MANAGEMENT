using PM.Model.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace PM.API.DAL.DAL
{
    public interface IUserDetailsDbManager
    {
        bool Create(UserDetails userDetails);
        IEnumerable<UserViewModel> GetAll(int role);
        UserDetails ValidateUser(UserDetails userDetails);

        bool BulkInsertFile(Model.Model.File file);
    }
}
