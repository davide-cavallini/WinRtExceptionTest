
using System.Diagnostics;

namespace WinRtExceptionTest
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            BtnNavigate.Clicked += BtnNavigate_Clicked;
        }

        protected override void OnDisappearing()
        {
            BtnNavigate.Clicked -= BtnNavigate_Clicked;
            base.OnDisappearing();
        }

        private async void BtnNavigate_Clicked(object? sender, EventArgs e)
        {
            Debug.WriteLine($"{nameof(MainPage)} into {nameof(Page2)}");
            await Shell.Current.GoToAsync(nameof(Page2));
        }
    }

}
