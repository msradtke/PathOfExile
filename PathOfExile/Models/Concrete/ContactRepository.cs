using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PathOfExile.Models.Entities;
using PathOfExile.Models.Services;

namespace PathOfExile.Models.Concrete
{
    public class ContactRepository : RepositoryBase<Contact>
    {
        public ContactRepository()
        {
            base._path = LoadDataService.GetPath("Contacts");
        }
    }
}
