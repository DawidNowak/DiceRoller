using DiceRoller.DataAccess.Context;
using DiceRoller.DataAccess.Models;
using Prism;
using Prism.Ioc;
using DiceRoller.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Prism.Autofac;
using Xamarin.Forms.Internals;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace DiceRoller
{
    public partial class App : PrismApplication
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            var ctx = Container.Resolve<IContext>();
            EnsureDbCreated(ctx);
            EnsureDbSeeded(ctx);

             await NavigationService.NavigateAsync("NavigationPage/MainPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage>();
            containerRegistry.RegisterForNavigation<GamePage>();

            containerRegistry.RegisterSingleton<IContext, DiceContext>();
        }

        private void EnsureDbCreated(IContext ctx)
        {
            ctx.CreateTable<DiceWall>();
            ctx.CreateTable<Dice>();
            ctx.CreateTable<Game>();
        }

        private void EnsureDbSeeded(IContext ctx)
        {
            //I know it's awful but insert with children does not work(?)
            //syntax error in sql

            var games = Seed.GetGames();
            var dice = Seed.GetDice();
            var walls = Seed.GetWalls();

            games.ForEach(ctx.InsertOrReplace);
            dice.ForEach(ctx.InsertOrReplace);
            walls.ForEach(ctx.InsertOrReplace);
        }
    }
}
