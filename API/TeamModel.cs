using System;

namespace Tankathon.API;

public partial class TeamModel
{
    public Type tankType;
    public Godot.Resource tankInfo;

    public TeamModel(Type type, Godot.Resource info)
    {
        tankType = type;
        tankInfo = info;
    }
}
