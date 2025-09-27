﻿using System.Windows.Input;
using Chess.Models;
using Chess.ModelsLogic;

namespace Chess.ViewModel
{
    internal partial class LoginPagVM : ObservableObject
    {
        public ICommand ToggleIsPasswordCommand { get; }
        public bool IsPassword { get; set; } = true;
        public ICommand LoginCommand { get; }
        private readonly User user = new();
        public bool CanLogin()
        {
            return user.CanLogin();
        }
        
        public LoginPagVM()
        {
            LoginCommand = new Command(Login, CanLogin);
            ToggleIsPasswordCommand = new Command(ToggleIsPassword);
        }
        private void Login()
        {
            user.Login();
        }
        private void ToggleIsPassword()
        {
            IsPassword = !IsPassword;
            OnPropertyChanged(nameof(IsPassword));
        }

       
        public string UserName
        {
            get => user.UserName;
            set
            {
                user.UserName = value;
                (LoginCommand as Command)?.ChangeCanExecute();
            }

        }
        public string Password
        {
            get => user.Password;
            set
            {
                user.Password = value;
                (LoginCommand as Command)?.ChangeCanExecute();
            }

        }
        public string Email
        {
            get => user.Email;
            set
            {
                user.Email = value;
                (LoginCommand as Command)?.ChangeCanExecute();
            }

        }
        
    }
}
