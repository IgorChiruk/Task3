using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Web_Chat.Models;

namespace Web_Chat.Hubs
{
    public class ChatHub : Hub
    {
        static List<User> Users = new List<User>();

        // Отправка сообщений
        public void Send(string name, string incomingMessage, string UserId)
        {
            using (MessageContext context = new MessageContext())
            {
                using (UserContext userContext = new UserContext())
                {
                    User newuser = new User();
                    int i = Int32.Parse(UserId);
                    var alluser = userContext.Users.ToList();
                   foreach(User u in alluser)
                    {
                        if (u.Id == i)
                        {
                            newuser.Name = u.Name;
                            break;
                        }
                    }
                    Message message = new Message { MessageDate = DateTime.Now, FromUser = UserId, MessageText = incomingMessage };
                    context.Messages.Add(message);
                    context.SaveChanges();
                    Clients.Caller.addMyMessage(newuser.Name, incomingMessage, UserId);
                    Clients.Others.addOtherMessage(newuser.Name, incomingMessage, UserId);
                }
            }
        }

        public void DisplayMessage(string UserId)
        {
            using (MessageContext context = new MessageContext())
            {
                var tempMessages = context.Messages.ToList();
                int i = tempMessages.Count - 10;
                while (i < 0)
                {
                    i++;
                }
                for (; i < tempMessages.Count; i++)
                {
                    if (tempMessages[i].FromUser==UserId)
                    {
                        Clients.Caller.addMyMessage(GetNameById(tempMessages[i].FromUser), tempMessages[i].MessageText, tempMessages[i].FromUser);
                        Clients.Others.addOtherMessage(GetNameById(tempMessages[i].FromUser), tempMessages[i].MessageText, tempMessages[i].FromUser);
                    }
                    else
                    {
                        Clients.Caller.addOtherMessage(GetNameById(tempMessages[i].FromUser), tempMessages[i].MessageText, tempMessages[i].FromUser);
                        Clients.Others.addMyMessage(GetNameById(tempMessages[i].FromUser), tempMessages[i].MessageText, tempMessages[i].FromUser);
                    }              
                }
            }
        }

        private string GetNameById(string id)
        {
            string name = null;
            using (UserContext userContext = new UserContext())
            {
                var allusers = userContext.Users.ToList();
                int i = Int32.Parse(id);
                foreach(User u in allusers)
                {
                    if (u.Id == i)
                    {
                        name = u.Name;
                        break;
                    }
                }
            }
            return name;
        }

            // Подключение нового пользователя
            public void Connect(string UserId)
        {
            var id = Context.ConnectionId;
            int i = Int32.Parse(UserId);


            if (!Users.Any(x => x.ConnectionId == id))
            {
                using (UserContext context = new UserContext())
                {
                    User newUser = new User();
                    var users = context.Users.ToList();
                    foreach (User tempUser in users)
                    {
                        if (tempUser.Id == i)
                        {
                            newUser.Id = i;
                            newUser.Name = tempUser.Name;
                            newUser.ConnectionId = id;
                            break;
                        }
                    }

                    Users.Add(newUser);

                    // Посылаем сообщение текущему пользователю
                    Clients.Caller.onConnected(id, newUser.Name, newUser.Id, Users);

                    // Посылаем сообщение всем пользователям, кроме текущего
                    Clients.AllExcept(id).onNewUserConnected(id, newUser.Name);
                }

            }
        }

        // Отключение пользователя
        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            var item = Users.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            if (item != null)
            {
                Users.Remove(item);
                var id = Context.ConnectionId;
                Clients.All.onUserDisconnected(id, item.Name);
            }

            return base.OnDisconnected(stopCalled);
        }

        public override System.Threading.Tasks.Task OnConnected()
        {
           var id = this.Context.Request.Cookies["UserId"].Value;
            DisplayMessage(id);
            return base.OnConnected();
        }
    }
}




//public void Send(string name, string incomingMessage, string UserId)
//{
//    using (MessageContext context = new MessageContext())
//    {
//        using (UserContext userContext = new UserContext())
//        {
//            User newuser = new User();
//            int i = Int32.Parse(UserId);
//            var alluser = userContext.Users.ToList();
//            foreach (User u in alluser)
//            {
//                if (u.Id == i)
//                {
//                    newuser.Name = u.Name;
//                    break;
//                }
//            }
//            Message message = new Message { MessageDate = DateTime.Now, FromUser = UserId, MessageText = incomingMessage };
//            context.Messages.Add(message);
//            context.SaveChanges();
//            Clients.All.addMessage(newuser.Name, incomingMessage, UserId);
//        }
//    }
//}

//public void DisplayMessage(string UserId)
//{
//    using (MessageContext context = new MessageContext())
//    {
//        var tempMessages = context.Messages.ToList();
//        int i = tempMessages.Count - 10;
//        while (i < 0)
//        {
//            i++;
//        }
//        for (; i < tempMessages.Count; i++)
//        {
//            //Clients.All.addMessage(GetNameById(tempMessages[i].FromUser), tempMessages[i].MessageText, tempMessages[i].FromUser);
//            Clients.Caller.addMyMessage(GetNameById(tempMessages[i].FromUser), tempMessages[i].MessageText, tempMessages[i].FromUser);
//            //Clients.Others.addOtherMessage(GetNameById(tempMessages[i].FromUser), tempMessages[i].MessageText, tempMessages[i].FromUser);
//        }
//    }
//}