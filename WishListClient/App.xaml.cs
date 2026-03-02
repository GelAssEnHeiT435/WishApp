using WishListClient.src.Services;

namespace WishListClient
{
    public partial class App : Application
    {
        private readonly IServiceProvider _provider;

        public App(IServiceProvider provider)
        {
            InitializeComponent();
            _provider = provider;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }

        protected override async void OnStart()
        {
            base.OnStart();
            var wishlistService = _provider.GetRequiredService<WishlistService>();

            try {
                await wishlistService.GetAllWishesAsync();
            }
            catch (Exception ex) {
                await Windows[0].Page!.DisplayAlert("Ошибка", $"Не удалось загрузить список желаний. Проверьте соединение. {ex.Message}", "OK");
            }
        }
    }
}