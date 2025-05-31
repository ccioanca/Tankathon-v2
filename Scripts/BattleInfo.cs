using Godot;

namespace Tankathon.Scripts;

[GlobalClass]
public partial class BattleInfo : Resource
{
    [Export]
    public Godot.Collections.Array<TeamData> teams;

    public BattleInfo()
    {
        teams = new Godot.Collections.Array<TeamData>();
    }
}
