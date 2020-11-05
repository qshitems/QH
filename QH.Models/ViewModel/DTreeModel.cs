using System;
using System.Collections.Generic;
using System.Text;

namespace QH.Models
{
  public  class DTreeModel
    {
        public StatusModel Status { get; set; } = new StatusModel();
        public List<PermissionModel> Data { get; set; } = new List<PermissionModel>();
    }

    public class StatusModel
    {
        public int Code { get; set; } = 200;

        public string Message { get; set; } = "成功";
    }
    public class PermissionModel
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Label { get; set; }
        public int? Type { get; set; }
        public string Path { get; set; }
        public string Icon { get; set; }
        public int? Sort { get; set; }
        public string CheckArr { get; set; } = "0";
    }
}
