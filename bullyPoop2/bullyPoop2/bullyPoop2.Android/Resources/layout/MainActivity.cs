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
using System.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Xamarin.Essentials;

namespace bullyPoop2.Droid
{
    [Activity(Label = "bullyPoop", Icon = "@mipmap/bp_icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        List<Bathroom> allBathrooms;
        List<string> buildings;
        List<Review> reviews;
        SecureStorageHelper storageHelper;
        WebRequestHelper webHelper;
        User currentUser;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            storageHelper = new SecureStorageHelper();
            webHelper = new WebRequestHelper("http://35.196.91.227");

            //var result = webHelper.getRequest("/bathrooms");
            //result.Replace("\n", "");

            //JsonConvert.SerializeObject(blank);
            //JsonConvert.DeserializeObject<User>(currentUser);

            //JObject json = JObject.Parse(result);
            //Console.WriteLine(json[0]);


            currentUser = storageHelper.GetItem<User>("currentUser");




            allBathrooms = new List<Bathroom>();
            allBathrooms.Add(new Bathroom("Union", 1, 1, true, 4, 4, "M"));
            buildings = new List<string>();
            reviews = new List<Review>();

            //webHelper.postRequest("/bathrooms", JsonConvert.SerializeObject(allBathrooms[0]));

            if (currentUser == null) currentUser = new User("poopmaster69", "M", "mrpoopy@gmail.com", 420, "Allen Hall - 1st Floor");
            storageHelper.StoreItem<User>("currentUser", currentUser);

            // TODO: Should be populated by database...
            buildings.Add("Lee Hall"); buildings.Add("Union"); buildings.Add("Allen"); buildings.Add("Library"); buildings.Add("Carpenter"); buildings.Add("McCain");
            buildings.Add("Butler"); buildings.Add("Chapel of Memories"); buildings.Add("Simral"); buildings.Add("Barnes & Nobel");

            for (var i = 0; i < allBathrooms.Count; i++)
            {
                if (!buildings.Contains(allBathrooms[i].building))
                {
                    buildings.Add(allBathrooms[i].building);
                }
            }


            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());

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
                var name = createBathroomName(allBathrooms[i]);
                string stars = string.Join("", Enumerable.Repeat("🚽", allBathrooms[i].getAvgRating(reviews)));
                if (stars == "") stars = "💩";
                name += "\t\tRating: " + stars;
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

            // Navbar
            FindViewById<Button>(Resource.Id.homeButtonRegisterBathroom).Click += (sender, e) => { HomePage(); };
            FindViewById<Button>(Resource.Id.mapButtonRegisterBathroom).Click += (sender, e) => { mapPage(); };
            FindViewById<Button>(Resource.Id.accountButtonRegisterBathroom).Click += (sender, e) => { accountPage(); };

            var registerButton = FindViewById<Button>(Resource.Id.buttonRegisterBathroom);
            var buildingSpinner = FindViewById<Spinner>(Resource.Id.buildingSpinnerRegisterBathroom);
            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, this.buildings);
            buildingSpinner.Adapter = adapter;

            registerButton.Click += (sender, e) =>
            {
                var building = buildingSpinner.SelectedItem.ToString();
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
                    var newBathroom = new Bathroom(building, Int32.Parse(floor), Int32.Parse(number), handicap, Int32.Parse(stallsNumber), Int32.Parse(urinalsNumber), currentUser.sex);
                    allBathrooms.Add(newBathroom);

                    if (!buildings.Contains(newBathroom.building))
                    {
                        buildings.Add(newBathroom.building);
                    }

                    // TODO On successful submit...
                    HomePage();
                }
            };
        }

        public void addReview()
        {
            SetContentView(Resource.Layout.addReview);

            // Navbar
            FindViewById<Button>(Resource.Id.buttonHomeAddReview).Click += (sender, e) => { HomePage(); };
            FindViewById<Button>(Resource.Id.buttonMapAddReview).Click += (sender, e) => { mapPage(); };
            FindViewById<Button>(Resource.Id.buttonAccountAddReview).Click += (sender, e) => { accountPage(); };

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

                Review review = new Review(reviewedBathroom.building, reviewedBathroom.UID, rating, reviewText, currentUser);
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

            // Navbar
            FindViewById<Button>(Resource.Id.buttonHomeAccountPage).Click += (sender, e) => { HomePage(); };
            FindViewById<Button>(Resource.Id.buttonMapAccountPage).Click += (sender, e) => { mapPage(); };
            FindViewById<Button>(Resource.Id.buttonAccountAccountPage).Click += (sender, e) => { accountPage(); };


            var showUser = FindViewById<TextView>(Resource.Id.showUser);
            var showSex = FindViewById<TextView>(Resource.Id.showSex);
            var showEmail = FindViewById<TextView>(Resource.Id.showEmail);

            showUser.Text = currentUser.username;
            showSex.Text = currentUser.sex;
            showEmail.Text = currentUser.email;


            var listView = FindViewById<ListView>(Resource.Id.reviewsListAccountPage);

            var reviewsToAdd = new List<string>();
            string latestReview;
            for (var i = 0; i < reviews.Count; i++)
            {
                if (reviews[i].user.username == currentUser.username)
                {
                    latestReview = "Username: " + reviews[i].user.username + "\n" + "Rating: " + reviews[i].rating + "\n" + "Description: " + reviews[i].review + "\n";
                    reviewsToAdd.Add(latestReview);
                }
            }


            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, reviewsToAdd);
            listView.Adapter = adapter;

        }

        public int getBathroomIndexFromName(string name)
        {
            var selectedBathroom = name;

            var building = selectedBathroom.Split(" - Floor ")[0];
            var temp = selectedBathroom.Split(" - Floor ")[1].Split(" - #");
            var floor = Int32.Parse(selectedBathroom.Split(" - Floor ")[1].Split(" - #")[0].Split("\t")[0]);
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

            // Navbar
            FindViewById<Button>(Resource.Id.buttonHomeViewReview).Click += (sender, e) => { HomePage(); };
            FindViewById<Button>(Resource.Id.buttonMapViewReview).Click += (sender, e) => { mapPage(); };
            FindViewById<Button>(Resource.Id.buttonAccountViewReview).Click += (sender, e) => { accountPage(); };

            FindViewById<TextView>(Resource.Id.bathroomNameViewReviews).Text = createBathroomName(bathroom);
            string stars = string.Join("", Enumerable.Repeat("🚽", bathroom.getAvgRating(reviews)));
            if (stars == "") stars = "0 Toilets Stars";
            FindViewById<TextView>(Resource.Id.bathroomRating).Text = "Rating: " + stars;

            var reviewsListView = FindViewById<ListView>(Resource.Id.reviewsListViewReviews);
            var reviewsToAdd = new List<string>();
            string latestReview;
            for (var i = 0; i < reviews.Count; i++)
            {
                if (reviews[i].bathroomId == bathroom.UID)
                {
                    latestReview = "Username: " + reviews[i].user.username + "\n" + "Rating: " + reviews[i].rating + "\n" + "Description: " + reviews[i].review + "\n";
                    reviewsToAdd.Add(latestReview);
                }
            }


            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, reviewsToAdd);
            reviewsListView.Adapter = adapter;
        }

        public void mapPage() {
            Console.WriteLine("Would be going to map page");
        }

    }
}