using Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface
{
    public interface IContactRepository
    {
        Task<ContactDTO> Get(int id);
        Task Create();
        Task Update(ContactDTO roofDTO);
    }
}
