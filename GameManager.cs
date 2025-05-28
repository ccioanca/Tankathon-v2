using Godot;
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
	Resource tlTankResR1;
	Resource trTankResR1;
	Resource blTankResR1;
	Resource brTankResR1;

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
			new TeamModel(typeof(DumTank), tlTankResR1),
			new TeamModel(typeof(DumTank), tlTankResR1),
			new TeamModel(typeof(DumTank), tlTankResR1),
			new TeamModel(typeof(DumTank), tlTankResR1)
		};

		//Fight 1
		// _tankTypes = new List<TeamModel>
		// {
		// 	new TeamModel(typeof(DumTank), tlTankResR1),
		// 	new TeamModel(typeof(DumTank), trTankResR1),
		// 	new TeamModel(typeof(DumTank), blTankResR1),
		// 	new TeamModel(typeof(DumTank), brTankResR1)
		// };

		//Fight 2
		// _tankTypes = new List<TeamModel>
		// {
		// 	new TeamModel(typeof(DumTank), tlTankRes),
		// 	new TeamModel(typeof(DumTank), tlTankRes),
		// 	new TeamModel(typeof(DumTank), tlTankRes),
		// 	new TeamModel(typeof(DumTank), tlTankRes)
		// };

		//Fight 3
		// _tankTypes = new List<TeamModel>
		// {
		// 	new TeamModel(typeof(DumTank), tlTankRes),
		// 	new TeamModel(typeof(DumTank), tlTankRes),
		// 	new TeamModel(typeof(DumTank), tlTankRes),
		// 	new TeamModel(typeof(DumTank), tlTankRes)
		// };

		//Fight 4
		// _tankTypes = new List<TeamModel>
		// {
		// 	new TeamModel(typeof(DumTank), tlTankRes),
		// 	new TeamModel(typeof(DumTank), tlTankRes),
		// 	new TeamModel(typeof(DumTank), tlTankRes),
		// 	new TeamModel(typeof(DumTank), tlTankRes)
		// };

		tlTank = GetNode<TheTank>("TopLeftTank");
		trTank = GetNode<TheTank>("TopRightTank");
		blTank = GetNode<TheTank>("BottomLeftTank");
		brTank = GetNode<TheTank>("BottomRightTank");


		tlTank.thisTank = Activator.CreateInstance(_tankTypes[0].tankType) as ITank;
		trTank.thisTank = Activator.CreateInstance(_tankTypes[1].tankType) as ITank;
		blTank.thisTank = Activator.CreateInstance(_tankTypes[2].tankType) as ITank;
		brTank.thisTank = Activator.CreateInstance(_tankTypes[3].tankType) as ITank;

		tlTank.Init(_tankTypes[0].tankInfo.Get("shootSound").As<AudioStream>(), _tankTypes[0].tankInfo.Get("deathSound").As<AudioStream>());
		trTank.Init(_tankTypes[1].tankInfo.Get("shootSound").As<AudioStream>(), _tankTypes[0].tankInfo.Get("deathSound").As<AudioStream>());
		blTank.Init(_tankTypes[2].tankInfo.Get("shootSound").As<AudioStream>(), _tankTypes[0].tankInfo.Get("deathSound").As<AudioStream>());
		brTank.Init(_tankTypes[3].tankInfo.Get("shootSound").As<AudioStream>(), _tankTypes[0].tankInfo.Get("deathSound").As<AudioStream>());
	}
}
