using System;
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

        public Bathroom(string building, int floor, int number, bool handicap, int stalls, int urinals)
        {
            this.building = building;
            this.floor = floor;
            this.number = number;
            this.handicap = handicap;
            this.stalls = stalls;
            this.urinals = urinals;
        }
    }
}
