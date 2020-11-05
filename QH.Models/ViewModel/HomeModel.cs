using System;
using System.Collections.Generic;
using System.Text;

namespace QH.Models
{
	public class HomeModel
	{
		public UserModel user { get; set; } = new UserModel();
		public List<MenusModel> menus { get; set; } = new List<MenusModel>();
		public List<string> permissions { get; set; } = new List<string>();

	}
    public class UserModel
    {
        public long Id { get; set; }
        public string NickName { get; set; }
        public string UserName { get; set; }
        public string Avatar { get; set; }
    }

    public class MenusModel
    {
        public long Id { get; set; }
        public long? ParentId { get; set; }
        public string Label { get; set; }
        public string Path { get; set; }
        public string Icon { get; set; }
		/// <summary>
		/// 隐藏
		/// </summary>
		public Boolean? Hidden { get; set; }

		/// <summary>
		/// 启用
		/// </summary>
		public Boolean? Enabled { get; set; }

		/// <summary>
		/// 可关闭
		/// </summary>
		public Boolean? Closable { get; set; }

		/// <summary>
		/// 打开组
		/// </summary>
		public Boolean? Opened { get; set; }

		/// <summary>
		/// 打开新窗口
		/// </summary>
		public Boolean? NewWindow { get; set; }

		/// <summary>
		/// 链接外显
		/// </summary>
		public Boolean? External { get; set; }
	}
}
