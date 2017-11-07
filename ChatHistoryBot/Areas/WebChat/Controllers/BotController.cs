using ChatHistoryBot.Areas.WebChat.Models;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;

namespace ChatHistoryBot.Areas.WebChat.Controllers
{
    [Authorize]
    public class BotController : Controller
    {
        // GET: WebChat/Bot
        public ActionResult Index()
        {
            #region 
            //            var encryptedID = CryptoHelpers.EncryptString(System.Web.HttpContext.Current.User.Identity.GetUserId(), "1234", false);
            //            using (var context = new ConversationDataContext())
            //            {
            //                var record = (from each in context.UserKeys
            //                    where each.UserId == encryptedID
            //                    select each).FirstOrDefault();
            //
            //                if (record == null || (record.Timestamp < DateTime.UtcNow.AddSeconds(-20)))
            //                {
            //                    RedirectToAction("Register", "Account");
            //                }
            //                else
            //                {
            //                    context.UserKeys.Remove(record);
            //                    context.SaveChanges();
            //                    
            //                }
            //            }
#endregion
            var model = new BotModel(System.Web.HttpContext.Current.User.Identity.GetUserId());
                    return View(model);
        }
    }
   
}