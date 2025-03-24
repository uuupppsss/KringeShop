using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KringeShopLib.Model;

namespace KringeShopWpf.ViewModel
{
    public class MainWindowVM:BaseVM
    {
		private List<Order> _orders;

		public List<Order> Orders
		{
			get { return _orders; }
			set 
			{ 
				_orders = value;
				Signal();
			}
		}

		public CommandVM OrderInfo { get; set; }

        public MainWindowVM()
        {
            
        }
    }

}
