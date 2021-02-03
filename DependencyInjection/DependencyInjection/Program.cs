using System;

namespace DependencyInjection
{

    public interface INotificationService
    {
        void NotifyUsernameChanged(User user);
    }
    
    public class ConsoleNotification : INotificationService
    {
        public void NotifyUsernameChanged(User user)
        {
            Console.WriteLine($"Username has been changed to {user.Username}");
        }
    }
    
    public class User
    {
        private INotificationService _notificationService;
        public string Username { get; private set; }

        public User(string username, INotificationService notificationService)
        {
            Username = username;
            _notificationService = notificationService;
        }
        
        public void ChangeUsername(string name)
        {
            Username = name;
            _notificationService.NotifyUsernameChanged(this);
        }
    }
    
    internal class Program
    {
        public static void Main(string[] args)
        {
            var consoleNotification = new ConsoleNotification();
            var user = new User("Tim",consoleNotification);
            user.ChangeUsername("Robert");
        }
    }
}