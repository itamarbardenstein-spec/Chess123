using Chess.Models;

namespace Chess.ModelsLogic
{
    public class MainPageML:MainPageModel
    {          
        public override void ShowInstructionsPrompt(object obj)
        {
            Application.Current!.MainPage!.DisplayAlert(Strings.Instructions, Strings.InsructionsTxt, Strings.Ok);
        }
    }
}
