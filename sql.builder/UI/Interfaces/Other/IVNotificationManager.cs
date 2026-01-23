using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sql.builder.UI
{
	public interface IVNotificationManager
	{
		void ShowNotification(string title, string text, string notificationType);

	}
}
