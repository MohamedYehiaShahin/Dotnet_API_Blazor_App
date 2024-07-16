using System;
using System.Collections.Generic;

namespace Application.DTOs.Request.Account
{
   public record ChangeUserRoleRequestDTO(string UserEmail,string RoleName);
}
