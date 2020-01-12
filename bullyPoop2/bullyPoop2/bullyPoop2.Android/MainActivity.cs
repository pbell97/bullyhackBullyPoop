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
        List<Bathroom> allBathrooms;
        List<string> buildings;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            allBathrooms = new List<Bathroom>();
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

            addBathroomButton.Click += (sender, e) =>
            {
                //Change Content View
                Console.WriteLine("Clicked Add Bathroom");
                registerBathroom();
            };

            var bathroomNames = new List<String>();

            for (var i = 0; i < allBathrooms.Count; i++)
            {
                var name = allBathrooms[i].building + " - Floor " + allBathrooms[i].floor;
                if (allBathrooms[i].number > 1) name += " - #" + allBathrooms[i].number;
                bathroomNames.Add(name);
            }

            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, bathroomNames);
            bathroomListView.Adapter = adapter;
        }


        public void registerBathroom()
        {
            SetContentView(Resource.Layout.registerBathroom);
            var registerButton = FindViewById<Button>(Resource.Id.buttonRegisterBathroom);

            registerButton.Click += (sender, e) =>
            {
                var building = FindViewById<EditText>(Resource.Id.editTextRegisterBuilding).Text;
                var floor = FindViewById<EditText>(Resource.Id.editTextRegisterFloor).Text;
                var number = FindViewById<EditText>(Resource.Id.editTextRegisterNumber).Text;
                var stallsNumber = FindViewById<EditText>(Resource.Id.editTextRegisterNumberStalls).Text;
                var urinalsNumber = FindViewById<EditText>(Resource.Id.editTextRegisterNumberUrinals).Text;
                var handicap = FindViewById<CheckBox>(Resource.Id.checkBoxHandicap).Checked;

                if (building == "" || floor == "" || number == "" || stallsNumber == "" || urinalsNumber == "")
                {
                    Console.WriteLine("A field is empty");
                }
                else
                {
                    Console.WriteLine("All fields are completed");
                    var newBathroom = new Bathroom(building, Int32.Parse(floor), Int32.Parse(number), handicap, Int32.Parse(stallsNumber), Int32.Parse(urinalsNumber));
                    allBathrooms.Add(newBathroom);

                    // TODO On successful submit...
                    HomePage();
                }
            };
        }

        public void addReview()
        {
            SetContentView(Resource.Layout.addReview);
        }

    }
}