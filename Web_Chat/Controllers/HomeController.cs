using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_Chat.Models;
// <add name="DBConnection" connectionString="data source=(localdb)\MSSQLLocalDB;Initial Catalog=userstore;Integrated Security=True;"
namespace Web_Chat.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            User user;
            var UserId = GetIdFromCookies();
            if (UserId == null)
            {
                int userID = CreateUser();
                SetCookie(userID);
                user = new User { Name = "Anonimus",Id= userID };
                return View(user);
            }
            else
            {
                using (UserContext context = new UserContext())
                {
                    int i = Int32.Parse(UserId);
                    var users = context.Users.ToList();
                    User newUser = new User();
                    foreach(User tempUser in users)
                    {
                        if (tempUser.Id == i)
                        {
                            newUser.Id = i;
                            newUser.Name = tempUser.Name;
                            break;
                        }
                    }
                    return View(newUser);          
                }
               
            }
            
        }

        private void SetCookie(int ID)
        {
            var cookie = new HttpCookie("UserID")
            {
                Value = ID.ToString(),
                Expires = DateTime.Now.AddYears(10),
            };
            Response.SetCookie(cookie);
        }

        private string GetIdFromCookies()
        {
            string IdUser = null;
            if (HttpContext.Request.Cookies.Count != 0 && CheckID())
            {
                var cookie = HttpContext.Request.Cookies.Get("UserID");
                IdUser = cookie.Value;
            }
            return IdUser;
        }

        private bool CheckID()
        {
            var cookieList = HttpContext.Request.Cookies.AllKeys.ToList();
            if (cookieList.Contains("UserID"))
            {
                return true;
            }
            else { return false; }
        }

        private int CreateUser()
        {
            using (UserContext context = new UserContext())
            {
                User user = new User { Name = "Anonimus"};              
                context.Users.Add(user);
                context.SaveChanges();             
                return user.Id;
            }
            
        }
    }
}