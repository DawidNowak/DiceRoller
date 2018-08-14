using System;
using Android.App;
using Android.Content.PM;
using Android.Content.Res;
using Android.OS;
using Android.Views;
using DiceRoller.DataAccess.Context;
using DiceRoller.Droid.Helpers;
using Plugin.CurrentActivity;
using Plugin.Permissions;
using Prism;
using Prism.Ioc;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace DiceRoller.Droid
{
    [Activity(Label = "DiceRoller", Icon = "@mipmap/ic_launcher", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                // Kill status bar underlay added by FormsAppCompatActivity
                // Must be done before calling FormsAppCompatActivity.OnCreate()
                var statusBarHeightInfo = typeof(FormsAppCompatActivity).GetField("statusBarHeight", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                if (statusBarHeightInfo == null)
                {
                    statusBarHeightInfo = typeof(FormsAppCompatActivity).GetField("_statusBarHeight", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                }
                statusBarHeightInfo?.SetValue(this, 0);
            }

			base.OnCreate(bundle);
			CrossCurrentActivity.Current.Init(this, bundle);

			global::Xamarin.Forms.Forms.Init(this, bundle);
            try
            {
                LoadApplication(new App(new AndroidInitializer()));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            this.Window.AddFlags(WindowManagerFlags.Fullscreen | WindowManagerFlags.TurnScreenOn);
        }

	    public override void OnConfigurationChanged(Configuration newConfig)
	    {
		    base.OnConfigurationChanged(newConfig);

		    switch (newConfig.Orientation)
		    {
			    case Orientation.Landscape:
				    switch (Device.Idiom)
				    {
					    case TargetIdiom.Phone:
						    LockRotation(Orientation.Portrait);
						    break;
					    case TargetIdiom.Tablet:
						    LockRotation(Orientation.Landscape);
						    break;
				    }
				    break;
			    case Orientation.Portrait:
				    switch (Device.Idiom)
				    {
					    case TargetIdiom.Phone:
						    LockRotation(Orientation.Portrait);
						    break;
					    case TargetIdiom.Tablet:
						    LockRotation(Orientation.Landscape);
						    break;
				    }
				    break;
		    }
		}

	    private void LockRotation(Orientation orientation)
	    {
		    switch (orientation)
		    {
			    case Orientation.Portrait:
				    RequestedOrientation = ScreenOrientation.Portrait;
				    break;
			    case Orientation.Landscape:
				    RequestedOrientation = ScreenOrientation.Landscape;
				    break;
		    }
	    }

		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry container)
        {
            container.Register<IDbPathHelper, DbPathHelperDroid>();
        }
    }
}

