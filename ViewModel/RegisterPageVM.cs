using Chess.ModelsLogic;
using System.Windows.Input;

namespace Chess.ViewModel
{
    internal class RegisterPageVM
    {
        public ICommand RegisterCommand { get; }
        private readonly User user = new();
        public bool CanRegister()
        {
            return (!string.IsNullOrWhiteSpace(user.UserName) && !string.IsNullOrWhiteSpace(user.Password) && !string.IsNullOrWhiteSpace(user.Email) && !string.IsNullOrWhiteSpace(user.Age));
        }
        public RegisterPageVM()
        {
            RegisterCommand=new Command(Register, CanRegister);
        }
        private void Register()
        {
            user.Register();
        }        
        public string UserName
        {
            get => user.UserName;
            set
            {                              
                 user.UserName = value;
                 (RegisterCommand as Command)?.ChangeCanExecute();                            
            }
            
        }
        public string Password
        {
            get => user.Password;
            set
            {
                user.Password = value;
                (RegisterCommand as Command)?.ChangeCanExecute();
            }

        }
        public string Email
        {
            get => user.Email;
            set
            {
                user.Email = value;
                (RegisterCommand as Command)?.ChangeCanExecute();
            }

        }
        public string Age
        {
            get => user.Age;
            set
            {
                user.Age = value;
                (RegisterCommand as Command)?.ChangeCanExecute();
            }

        }
        
    }
}
