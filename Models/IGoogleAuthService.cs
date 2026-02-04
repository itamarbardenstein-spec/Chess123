using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Models
{
    public interface IGoogleAuthService
    {
        Task<string> AuthenticateAsync();
    }
}
