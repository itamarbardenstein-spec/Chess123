using Chess.Models;
using Chess.ModelsLogic;
using Chess.Views;
using System.Windows.Input;

namespace Chess.ViewModel
{
    public class HomePageVM
    {
        private readonly User user = new();
        public ICommand PlayCommand { get; }
        public ICommand InstructionsCommand { get; private set; }
        public string UserName
        {
            get => user.UserName;
            set
            {
                user.UserName = value;             
            }
        }
        public HomePageVM()
        {
            PlayCommand = new Command(Play);
            InstructionsCommand = new Command(ShowInstructionsPrompt);
        }       
        public static void ShowInstructionsPrompt(object obj)
        {
            Application.Current!.MainPage!.DisplayAlert(Strings.Instructions, Strings.InstructionsTxt, Strings.Ok);
        }
        private void Play(object? sender)
        {
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                if (Application.Current != null)
                {
                    Application.Current.MainPage = new AppShell();
                }
            });
        }
    }
}
