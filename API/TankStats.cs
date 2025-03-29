namespace Tankathon.API;

public class TankStats : ITankStats
{
    public string name { get; set; }
    public float rotation { get; set; }
    public float xPos { get; set; }
	public float yPos { get; set; }
	public int healthCurrent { get; set; }
	public int score { get; set; }
}