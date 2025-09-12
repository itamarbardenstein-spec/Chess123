using System.Windows.Input;

namespace Chess.ViewModel
{
    class MainPageVm
    {
        public ICommand ChangeLightCommand { get;}
        public MainPageVm() 
        {
            ChangeLightCommand = new Command(ChangeLight);
        }

        private void ChangeLight()
        {
           
        }
    }
}
