using Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface
{
    public interface IRoofRepository
    {
        Task<IEnumerable<RoofGetDTO>> GetAll();
        Task<RoofGetDTO> Get(int id);
        Task Create(RoofDTO roofDTO);
        Task Delete(int id);
    }
}
