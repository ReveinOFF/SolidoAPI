using Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface
{
    public interface IMainRepository
    {
        Task<MainGetDTO> Get(int id);
        Task Create();
        Task Update(MainDTO mainDTO);
        Task Delete(string name);
    }
}
