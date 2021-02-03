using Autofac;
using System;
using static System.Console;

namespace DependencyInjectionAutoFac
{
    public interface INotificationService
    {
        void NotifyUsernameChanged(User user);
    }

    public class ConsoleNotification : INotificationService
    {
        public void NotifyUsernameChanged(User user)
        {
            WriteLine($"Username has been changed to {user.Username}");
        }
    }

    public class User
    {
        public string Username { get; set; }

        public User(string username)
        {
            Username = username;
        }
    }

    public class UserService
    {
        private INotificationService _notificationService;

        public UserService(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public void ChangeUsername(User user, string name)
        {
            user.Username = name;
            _notificationService.NotifyUsernameChanged(user);
        }
    }
    
    public class ProgramModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<ConsoleNotification>().As<INotificationService>();
            containerBuilder.RegisterType<UserService>().AsSelf();
        }
    }

    internal class Program
    {
        public static void Main(string[] args)
        {
            var containerBuilder = new ContainerBuilder();
            // containerBuilder.RegisterType<ConsoleNotification>().As<INotificationService>();
            // containerBuilder.RegisterType<UserService>().AsSelf();
            containerBuilder.RegisterModule<ProgramModule>();
            
            var container = containerBuilder.Build();
            // var consoleNotification = container.Resolve<INotificationService>();
            var userService = container.Resolve<UserService>();

            var user = new User("Tim");
            userService.ChangeUsername(user, "Robert");
        }
    }
}