using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QH.Models
{
    public class PermissionAssignInput
    {
        [Required(ErrorMessage = "��ɫ����Ϊ�գ�")]
        public int RoleId { get; set; }

        [Required(ErrorMessage = "Ȩ�޲���Ϊ�գ�")]
        public List<int> PermissionIds { get; set; }
    }
}
