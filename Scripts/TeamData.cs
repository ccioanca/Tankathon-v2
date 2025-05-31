using Godot;

namespace Tankathon.Scripts;

[GlobalClass]
public partial class TeamData : Resource
{
    [Export]
    public AudioStream shootSound;
    [Export]
    public AudioStream deathSound;
    [Export]
    public Texture logo;
    [Export]
    public string tankType;

    public TeamData()
    {
        shootSound = null;
        deathSound = null;
        logo = null;
        tankType = null;
    }
}
