using PM.Model.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace PM.API.DAL.DAL
{
    public interface IUserProfileDbManager
    {
        bool Create(UserProfile userProfile,int? id=0);
    }
}
