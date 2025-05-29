using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Tankathon.API;
using Tankathon.EvilTank;

namespace Tankathon.API.Internal;
public partial class GameManager : Node2D
{
	//TODO: Actually name all these resources based on the team names
	[ExportGroup("R1 Fight")]
	[Export]
	Resource r1Battle;

	// [ExportGroup("R2 Fight")]
	// [Export]
	// Resource trTankRes;

	// [ExportGroup("R3 Fight")]
	// [Export]
	// Resource blTankRes;

	// [ExportGroup("R4 Fight")]
	// [Export]
	// Resource brTankRes;

	TheTank tlTank = null;
	TheTank trTank = null;
	TheTank blTank = null;
	TheTank brTank = null;

	//list of combatants
	private List<TeamModel> _tankTypes;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//initialize the fights here
		//Test Fight
		_tankTypes = new List<TeamModel>
		{
			new TeamModel(typeof(DumTank), r1Battle.Get("teams").As<Godot.Collections.Array>()[0].As<Godot.Resource>()),
			new TeamModel(typeof(DumTank), r1Battle.Get("teams").As<Godot.Collections.Array>()[1].As<Godot.Resource>()),
			new TeamModel(typeof(DumTank), r1Battle.Get("teams").As<Godot.Collections.Array>()[2].As<Godot.Resource>()),
			new TeamModel(typeof(DumTank), r1Battle.Get("teams").As<Godot.Collections.Array>()[3].As<Godot.Resource>())
		};

		tlTank = GetNode<TheTank>("TopLeftTank");
		brTank = GetNode<TheTank>("BottomRightTank");
		trTank = GetNodeOrNull<TheTank>("TopRightTank");
		blTank = GetNodeOrNull<TheTank>("BottomLeftTank");


		tlTank.thisTank = Activator.CreateInstance(_tankTypes[0].tankType) as ITank;
		brTank.thisTank = Activator.CreateInstance(_tankTypes[1].tankType) as ITank;
		if(trTank != null)
			trTank.thisTank = Activator.CreateInstance(_tankTypes[2].tankType) as ITank;
		if(blTank != null)
			blTank.thisTank = Activator.CreateInstance(_tankTypes[3].tankType) as ITank;

		tlTank.Init(_tankTypes[0].tankInfo);
		brTank.Init(_tankTypes[1].tankInfo);
		if(trTank != null)
			trTank.Init(_tankTypes[2].tankInfo);
		if(blTank != null)
			blTank.Init(_tankTypes[3].tankInfo);
	}
}
