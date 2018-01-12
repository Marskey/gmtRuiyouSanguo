using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;

namespace GMService
{
	/// <summary>
	/// 项目安装
	/// </summary>
	[RunInstaller(true)]
	public partial class ProjectInstaller : System.Configuration.Install.Installer
	{
		/// <summary>
		/// 构造方法
		/// </summary>
		public ProjectInstaller()
		{
			InitializeComponent();
		}
	}
}