using System;
using MassTransit;
using System.Collections.Generic;
using bullyPoop2.Droid.Resources;

namespace bullyPoop2.Droid.Resources
{
    public class Bathroom
    {
        public string building;
        public int floor;
        public int number;
        public float avgRating;
        public bool handicap;
        public float frequency;
        public float latLong;
        public char size;
        public string sex;
        public string UID;
        public int stalls;
        public int urinals;

        public Bathroom(string building, int floor, int number, bool handicap, int stalls, int urinals, string sex)
        {
            this.building = building;
            this.floor = floor;
            this.number = number;
            this.handicap = handicap;
            this.stalls = stalls;
            this.urinals = urinals;
            this.UID = NewId.Next().ToString();
            this.avgRating = 0;
            this.sex = sex;
        }

        public int getAvgRating(List<Review> reviews)
        {
            int count = 0;
            int total = 0;

            for (var i = 0; i < reviews.Count; i++)
            {
                if (reviews[i].bathroomId == this.UID)
                {
                    count++;
                    total += reviews[i].rating;
                }
            }

            if (count == 0) return 0;
            return Convert.ToInt32(total / count);
        }
    }
}
