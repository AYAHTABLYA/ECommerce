using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.DTOS.IdentityDtos
{
    public record UserDTO(string Email, string DisplayName, string Token);

}
