using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
            //TODO: CHANGE THIS TO REFLECTION ASSEMBLY.GETTYPES
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage>();
            containerRegistry.RegisterForNavigation<GamePage>();
            containerRegistry.RegisterForNavigation<InfoPage>();
            containerRegistry.RegisterForNavigation<SettingsPage>();
            containerRegistry.RegisterForNavigation<AboutPage>();

            containerRegistry.RegisterSingleton<IContext, DiceContext>();
        }

        private void EnsureDbCreated(IContext ctx)
        {
            ctx.CreateTable<DiceWall>();
            ctx.CreateTable<Dice>();
            ctx.CreateTable<Game>();
            ctx.CreateTable<Config>();
        }

        private IDictionary<Type, Func<Entity[]>> _seedDict = new Dictionary<Type, Func<Entity[]>>();


        private void EnsureDbSeeded(IContext ctx)
        {
            _seedDict[typeof(Game)] = () => Seed.GetGames();
            _seedDict[typeof(Dice)] = () => Seed.GetDice();
            _seedDict[typeof(DiceWall)] = () => Seed.GetWalls();
            _seedDict[typeof(Config)] = () => Seed.GetConfigs();

            var info = GetType().GetMethod("InsertNotExisting", BindingFlags.Instance | BindingFlags.NonPublic);
            
            _seedDict.ForEach(pair =>
            {
                var generic = info?.MakeGenericMethod(pair.Key);
                generic?.Invoke(this, new object[] {ctx});
            });

        }

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
