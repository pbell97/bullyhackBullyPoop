using System;
namespace bullyPoop2.Droid.Resources
{
    public class Review
    {
        public string building;
        public string bathroomId;
        public int rating;
        public string review;
        public User user;

        public Review(string building, string bathroomId, int rating, string review, User user)
        {
            this.building = building;
            this.bathroomId = bathroomId;
            this.review = review;
            this.rating = rating;
            this.user = user;            
        }
}
}
