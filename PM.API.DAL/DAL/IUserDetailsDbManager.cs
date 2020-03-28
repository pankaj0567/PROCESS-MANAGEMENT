using PM.Model.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace PM.API.DAL.DAL
{
    public interface IUserDetailsDbManager
    {
        bool Create(UserDetails userDetails);
        IEnumerable<UserDetails> GetAll();
    }
}
