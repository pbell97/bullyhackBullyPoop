using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace bullyPoop2.Droid.Resources
{
    public class User
    {
        public string username;
        public string sex;
        public string email;
        public int poopPoints;
        public string favBathroom;

        public User(string username, string sex, string email, int poopPoints, string favBathroom)
        {
            this.username = username;
            this.sex = sex;
            this.email = email;
            this.poopPoints = poopPoints;
            this.favBathroom = favBathroom;
        }
    }
}