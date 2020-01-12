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
        List<Review> reviews;

        User currentUser;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            allBathrooms = new List<Bathroom>();
            allBathrooms.Add(new Bathroom("Union", 1, 1, true, 4, 4));
            buildings = new List<string>();
            reviews = new List<Review>();

            currentUser = new User("poopmaster69", "M", "mrpoopy@gmail.com", 420, "Allen Hall - 1st Floor");

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


            this.buildings.Add("Union");
            this.buildings.Add("Butler");
            this.buildings.Add("Lee Hall");

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
            var addReviewButton = FindViewById<Button>(Resource.Id.buttonAddReviewHomePage);
            var addAccountButton = FindViewById<Button>(Resource.Id.buttonAccount);
            addBathroomButton.Click += (sender, e) =>
            {
                //Change Content View
                Console.WriteLine("Clicked Add Bathroom");
                registerBathroom();
            };

            addReviewButton.Click += (sender, e) =>
            {
                addReview();
            };
            addAccountButton.Click += (sender, e) =>
            {
                accountPage();
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

            bathroomListView.ItemClick += (sender, e) =>
            {
                string selectedBathroom = ((ListView)sender).GetItemAtPosition(e.Position).ToString();
                viewReviews(allBathrooms[getBathroomIndexFromName(selectedBathroom)]);
            };
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
                    Toast.MakeText(this, "Please fill out all fields", ToastLength.Long).Show();
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
            var spinner = FindViewById<Spinner>(Resource.Id.spinnerBuildingAddReview);
            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, this.buildings);
            spinner.Adapter = adapter;

            spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(this.spinnerAddReviewBuildingSelected);



            var submitButton = FindViewById<Button>(Resource.Id.buttonSubmitAddReview);
            submitButton.Click += (sender, e) =>
            {
                //Validation here
                string reviewText = FindViewById<EditText>(Resource.Id.inputReviewAddReview).Text;
                if (FindViewById<EditText>(Resource.Id.inputRatingAddReview).Text == "" || reviewText == "")
                {
                    Toast.MakeText(this, "Please fill out all fields", ToastLength.Long).Show();
                    return;
                }
                int rating = Int32.Parse(FindViewById<EditText>(Resource.Id.inputRatingAddReview).Text);


                var bathroomSpinner = FindViewById<Spinner>(Resource.Id.spinnerBathroomAddReview);
                Spinner spinner2 = (Spinner)bathroomSpinner;
                var selectedBathroom = spinner2.SelectedItem.ToString();
                Bathroom reviewedBathroom = null;
                reviewedBathroom = allBathrooms[getBathroomIndexFromName(selectedBathroom)];

                Review review = new Review(reviewedBathroom.building, reviewedBathroom, rating, reviewText, currentUser);
                reviews.Add(review);
                HomePage();

            };

        }

        private void spinnerAddReviewBuildingSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            var selectedBuilding = spinner.GetItemAtPosition(e.Position);
            Console.WriteLine("User selected " + selectedBuilding);

            // Updates list of bathrooms based on building
            var bathroomNames = new List<string>();
            for (var i = 0; i < allBathrooms.Count; i++)
            {
                if (allBathrooms[i].building == selectedBuilding.ToString())
                {
                    var name = allBathrooms[i].building + " - Floor " + allBathrooms[i].floor;
                    if (allBathrooms[i].number > 1) name += " - #" + allBathrooms[i].number;
                    bathroomNames.Add(name);
                }
            }

            var spinner2 = FindViewById<Spinner>(Resource.Id.spinnerBathroomAddReview);
            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, bathroomNames);
            spinner2.Adapter = adapter;
        }

        public void accountPage()
        {
            SetContentView(Resource.Layout.accountPage);

            var showUser = FindViewById<TextView>(Resource.Id.showUser);
            var showSex = FindViewById<TextView>(Resource.Id.showSex);
            var showEmail = FindViewById<TextView>(Resource.Id.showEmail);

            showUser.Text = currentUser.username;
            showSex.Text = currentUser.sex;
            showEmail.Text = currentUser.email;
        }

        public int getBathroomIndexFromName(string name)
        {
            var selectedBathroom = name;

            var building = selectedBathroom.Split(" - Floor ")[0];
            var temp = selectedBathroom.Split(" - Floor ")[1].Split(" - #");
            var floor = Int32.Parse(selectedBathroom.Split(" - Floor ")[1].Split(" - #")[0]);
            var number = 1;
            if (selectedBathroom.Split(" - Floor ")[1].Split(" - #").Length > 1)
            {
                number = Int32.Parse(selectedBathroom.Split(" - Floor ")[1].Split(" - #")[1]);
            }
            for (var i = 0; i < allBathrooms.Count; i++)
            {
                if (allBathrooms[i].building == building && allBathrooms[i].floor == floor && allBathrooms[i].number == number)
                {
                    return i;
                }
            }

            return -1000;
        }

        public string createBathroomName(Bathroom bathroom){
            var name = bathroom.building + " - Floor " + bathroom.floor;
            if (bathroom.number > 1) name += " - #" + bathroom.number;
            return name;
        }

        public void viewReviews(Bathroom bathroom)
        {
            SetContentView(Resource.Layout.viewReviews);
            FindViewById<TextView>(Resource.Id.bathroomNameViewReviews).Text = createBathroomName(bathroom);
            FindViewById<TextView>(Resource.Id.bathroomRating).Text = "Rating: " + "5" + " stars";

            var reviewsListView = FindViewById<ListView>(Resource.Id.reviewsListViewReviews);
            var reviewsToAdd = new List<string>();
            string latestReview;
            for (var i = 0; i < reviews.Count; i++)
            {
                if (reviews[i].bathroom.UID == bathroom.UID)
                {
                    latestReview = "Username: " + reviews[i].user.username + "\n" + "Rating: " + reviews[i].rating + "\n" + "Description: " + reviews[i].review + "\n";
                    reviewsToAdd.Add(latestReview);
                }
            }


            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, reviewsToAdd);
            reviewsListView.Adapter = adapter;


        }

    }
}