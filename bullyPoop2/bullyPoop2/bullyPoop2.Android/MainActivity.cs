using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.ObjectModel;
using System.Collections.Generic;

using bullyPoop2.Droid.Resources;

namespace bullyPoop2.Droid
{
    [Activity(Label = "bullyPoop", Icon = "@mipmap/bp_icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());

            SetContentView(Resource.Layout.mainLayout);
            var button = FindViewById<Button>(Resource.Id.firstButton);
            var label = FindViewById<TextView>(Resource.Id.firstLabel);

            button.Click += (sender, e) =>
            {
                label.Text = "Hello there";
            };

            HomePage();





        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }


        public void HomePage()
        {
            SetContentView(Resource.Layout.homePage);
            var bathroomListView = FindViewById<ListView>(Resource.Id.homePageBathroomsList);
            var addBathroomButton = FindViewById<Button>(Resource.Id.buttonAddBathroom);
            var addAccountButton = FindViewById<Button>(Resource.Id.buttonAccount);

            addBathroomButton.Click += (sender, e) =>
            {
                //Change Content View
                Console.WriteLine("Clicked Add Bathroom");
            };

            addAccountButton.Click += (sender, e) =>
            {
                accountPage();
            };

            var bathrooms = new List<Bathroom>();
            var bathroomNames = new List<String>();

            bathrooms.Add(new Bathroom("Union", 1, 1, true, 4, 4));
            bathrooms.Add(new Bathroom("Union", 1, 2, true, 8, 8));


            for (var i = 0; i < bathrooms.Count; i++)
            {
                var name = bathrooms[i].building + " - Floor " + bathrooms[i].floor;
                if (bathrooms[i].number > 1) name += " - #" + bathrooms[i].number;
                bathroomNames.Add(name);
            }

            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, bathroomNames);
            bathroomListView.Adapter = adapter;
        }

        public void accountPage()
        {
            SetContentView(Resource.Layout.accountPage);

            var currentUser = new List<User>();
            currentUser.Add(new User("poopmaster69", "M", "mrpoopy@gmail.com", 420, "Allen Hall - 1st Floor"));

            var showUser = FindViewById<TextView>(Resource.Id.showUser);
            var showSex = FindViewById<TextView>(Resource.Id.showSex);
            var showEmail = FindViewById<TextView>(Resource.Id.showEmail);

            showUser.Text = currentUser[0].username;
            showSex.Text = currentUser[0].sex;
            showEmail.Text = currentUser[0].email;


        }
    }
}