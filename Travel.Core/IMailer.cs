using Mvc.Mailer;
using Travel.Data;
namespace Travel.Core
{
    public interface IMailer
    {
		MvcMailMessage Welcome(User model);
		MvcMailMessage NotifyDriver(Message message, string cabinetLink);
		MvcMailMessage NotifyPassenger(Message message, string cabinetLink);
		MvcMailMessage SendMail(Message message);
		MvcMailMessage SendCallBack(Message message);
		
    }
}
