using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DiceRoller.DataAccess.Context;
using DiceRoller.DataAccess.Models;
using DiceRoller.Helpers;
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
	    public static byte[] CroppedImage;
		public static MasterDetailPage MasterDetail { get; set; }
        public static IContainerProvider GlobalContainer;
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();
            GlobalContainer = Container;

            var ctx = Container.Resolve<IContext>();
            EnsureDbCreated(ctx);
            EnsureDbSeeded(ctx);

             await NavigationService.NavigateAsync("MasterDetailsPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage>();
            containerRegistry.RegisterForNavigation<GamePage>();
            containerRegistry.RegisterForNavigation<SettingsPage>();
            containerRegistry.RegisterForNavigation<MasterDetailsPage>();
            containerRegistry.RegisterForNavigation<GameCreatorPage>();

            containerRegistry.RegisterSingleton<IContext, DiceContext>();
            containerRegistry.RegisterSingleton<IEventAgregator, EventAggregator>();
        }

        private void EnsureDbCreated(IContext ctx)
        {
            ctx.CreateTable<DiceWall>();
            ctx.CreateTable<Dice>();
            ctx.CreateTable<Game>();
            ctx.CreateTable<Config>();
        }

        private readonly IDictionary<Type, Func<Entity[]>> _seedDict = new Dictionary<Type, Func<Entity[]>>();


        private void EnsureDbSeeded(IContext ctx)
        {
            _seedDict[typeof(Game)] = Seed.GetGames;
            _seedDict[typeof(Dice)] = Seed.GetDice;
            _seedDict[typeof(DiceWall)] = Seed.GetWalls;
            _seedDict[typeof(Config)] = Seed.GetConfigs;

            var info = GetType().GetMethod("InsertNotExisting", BindingFlags.Instance | BindingFlags.NonPublic);
            
            _seedDict.ForEach(pair =>
            {
                var generic = info?.MakeGenericMethod(pair.Key);
                generic?.Invoke(this, new object[] {ctx});
            });
        }

        //used by reflection
        private void InsertNotExisting<T>(IContext ctx) where T : Entity, new()
        {
            var all = ctx.GetAll<T>();
            var arr = _seedDict[typeof(T)].Invoke();
            arr.ForEach(o =>
            {
                if (all.All(x => x.Id != o.Id))
                    ctx.InsertOrReplace(o);
            });
        }
    }
}
