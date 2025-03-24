using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace KringeShopWpf.ViewModel
{
    public class AdminAuthPageVM:BaseVM
    {
		private string _password;
		public string Password
		{
			get { return _password; }
			set 
			{
				_password = value;
				Signal();
			}
		}

		private string _username;
		public string Username
		{
			get { return _username; }
			set 
			{
				_username = value;
				Signal();
			}
		}

		public CommandVM Enter {  get; set; }

		private PasswordBox _pwdbox;

        public AdminAuthPageVM()
        {
			Password = _pwdbox.Password;
			Enter = new CommandVM(() =>
			{

			});
		}

		internal void SetPassBox(PasswordBox pwd_box)
		{
			_pwdbox = pwd_box;
		}
    }
}
