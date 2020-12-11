using ExtCore.Data.Abstractions;
using SlimeWeb.Core.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlimeWeb.Core.Data.Repository
{
    public class BannedUsersRepository : IRepository  //<BannedUsers>
    {
        void IRepository.SetStorageContext(IStorageContext storageContext)
        {
            throw new NotImplementedException();
        }
    }
}
