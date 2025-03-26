namespace Tankathon.API;

public class TankStats : ITankStats
{
    //Gotta jump through some hoops to not expose the private setters to the tanks, but still be able to make use of them internally int he API
    //TODO: There's probably a better way of doing this?
    public float _rotation { get { return rotation; } set { rotation = value; } }
    public float _xPos { get { return xPos; } set { xPos = value; } }
    public float _yPos { get { return yPos; } set { yPos = value; } }
    public int _healthCurrent { get { return healthCurrent; } set { healthCurrent = value; } }
    public int _score { get { return score; } set { score = value; } }

    public string name { get; set; }
    public float rotation { get; private set; }
    public float xPos { get; private set; }
	public float yPos { get; private set; }
	public int healthCurrent { get; private set; }
	public int score { get; private set; }
}