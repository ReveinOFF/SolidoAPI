using Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface
{
    public interface IAuthenticationRepository
    {
        Task AdminCreateAsync();
        Task<AuthorizationDTO> LoginAsync(string login, string password);
        Task<AuthorizationDTO> ChangeProfile(string login, string currentPassword, string newPassword);
        Task LogoutAsync();
    }
}
