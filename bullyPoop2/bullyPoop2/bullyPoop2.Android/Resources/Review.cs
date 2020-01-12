using System;
namespace bullyPoop2.Droid.Resources
{
    public class Review
    {
        public string building;
        public Bathroom bathroom;
        public int rating;
        public string review;
        // TODO: Add user

        public Review(string building, Bathroom bathroom, int rating, string review)
        {
            this.building = building;
            this.bathroom = bathroom;
            this.review = review;
            this.rating = rating;

            // TODO: Add User
            
        }
}
}
