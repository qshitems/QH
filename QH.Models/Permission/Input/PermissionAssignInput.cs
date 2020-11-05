using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QH.Models
{
    public class PermissionAssignInput
    {
        [Required(ErrorMessage = "角色不能为空！")]
        public int RoleId { get; set; }

        [Required(ErrorMessage = "权限不能为空！")]
        public List<int> PermissionIds { get; set; }
    }
}
