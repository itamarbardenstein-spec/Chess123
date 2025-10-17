﻿using Chess.Models;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Firebase.Auth;

namespace Chess.ModelsLogic
{
    internal class User : UserModels
    {
        public override void Register()
        {
            fbd.CreateUserWithEmailAndPasswordAsync(Email, Password, UserName, OnComplete);
        }
        

        private void OnComplete(Task task)
        {
            if (task.IsCompletedSuccessfully)
            {
                SaveToPreferences();
                OnAuthCompleted?.Invoke(this, EventArgs.Empty);
            }
            else if (task.Exception != null)
            {
                
                string msg = task.Exception.Message;
                ShowAlert(GetFirebaseErrorMessage(msg));
            }
            else
                ShowAlert(Strings.RegistrationFailed);
        }
        public override string GetFirebaseErrorMessage(string msg)
        {
            if (msg.Contains(Strings.ErrMessageReason))
            {
                if (msg.Contains(Strings.EmailExists))
                    return Strings.EmailExistsErrMsg;
                if (msg.Contains(Strings.InvalidEmailAddress))
                    return Strings.InvalidEmailErrMessage;
                if (msg.Contains(Strings.WeakPassword))
                    return Strings.WeakPasswordErrMessage;
            }                                        
            return Strings.UnknownErrorMessage;
        }

        private static void ShowAlert(string msg)
        {
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                Toast.Make(msg, ToastDuration.Long).Show();
            });
        }
        private void SaveToPreferences()
        {
            Preferences.Set(Keys.UserNameKey, UserName);
            Preferences.Set(Keys.PasswordKey, Password);
            Preferences.Set(Keys.EmailKey, Email);
            Preferences.Set(Keys.AgeKey, Age);          
        }

        public override void Login()
        {         
            Preferences.Set(Keys.UserNameKey, UserName);
            Preferences.Set(Keys.PasswordKey, Password);
            Preferences.Set(Keys.EmailKey, Email);         
        }
        public override bool CanLogin()
        {
            return (!string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password) && !string.IsNullOrWhiteSpace(Email));
        }
        public override bool CanRegister()
        {
            return (!string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password) && !string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(Age));
        }
        public User()
        {           
            UserName = Preferences.Get(Keys.UserNameKey, string.Empty);
            Password = Preferences.Get(Keys.PasswordKey, string.Empty);
            Email = Preferences.Get(Keys.EmailKey, string.Empty);
            Age = Preferences.Get(Keys.AgeKey, string.Empty);
        }
    }
}
