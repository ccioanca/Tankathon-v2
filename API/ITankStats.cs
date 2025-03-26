namespace Tankathon.API
{
    public interface ITankStats
    {
        public string name { get; set; }
        public float rotation { get; }
        public float xPos { get; }
        public float yPos { get; }
        public int healthCurrent { get; }
        public int score { get; }
    }
}