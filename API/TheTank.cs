using Godot;
using Godot.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Tankathon.API.Internal;

public partial class TheTank : CharacterBody2D, IEntity
{
	[Export]
	public string TankName = "TankName";

	public EntityType eType => EntityType.Tank; 

	public bool col = false;
    public Vector2 _velocity = Vector2.Zero;
	private int health = 10;
	private int points = 0;
	
    public ITank thisTank;
	Actions actions;
	private IActions _passedActions;
	private TankSetup _tankSetup;
	private Scoreboard _scoreboard;
	private BoxContainer _tankScoreContainer;
    private ProgressBar _healthBar;

	private Node scorePanel;


    //Shooty things
    CollisionShape2D _collisionShape;
	PackedScene bullet;
    Marker2D turret;
    private List<Bullet> bulletsFired = new List<Bullet>();


    //For the raycasting
    private PhysicsDirectSpaceState2D spaceState;
	private PhysicsRayQueryParameters2D query_m;
	private PhysicsRayQueryParameters2D query_l;
	private PhysicsRayQueryParameters2D query_r;
    private Dictionary rayQueryResult;
	private System.Collections.Generic.Dictionary<Side, Entity> hitResults = new System.Collections.Generic.Dictionary<Side, Entity>();


    public override void _Ready()
	{
		_passedActions = GetNode<Actions>("Actions");
		_healthBar = GetNode<ProgressBar>("HealthBar");
        _scoreboard = GetNode<Scoreboard>("%Scoreboard");
        _tankScoreContainer = GetNode<BoxContainer>("%TanksScoreContainer");
		_tankSetup = new TankSetup();

        //get the turret object
        turret = GetNode<Marker2D>("Turret");

        //get the bullet preloaded
        bullet = GD.Load<PackedScene>("res://Scenes/Bullet.tscn");

		//get sel references
		_collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");

        base._Ready();
    }

	public override void _Process(double delta)
	{
		base._Process(delta);
	}

	public override void _PhysicsProcess(double delta)
	{
		thisTank.Do(_passedActions, _scoreboard);
		var k2d = MoveAndCollide(_velocity);
		if (k2d != null)
			col = true;
		else 
			col = false;

		base._PhysicsProcess(delta);
	}

	internal void Init()
	{
        _healthBar.Value = health;
        thisTank.Setup(_tankSetup);

        //setup Scoreboard object for this tank
        SetupScoreboard(_tankSetup);

    }

    internal void Shoot()
	{
		Bullet bulletInstance = (Bullet)bullet.Instantiate();
		bulletInstance.Position = turret.GlobalPosition;
		bulletInstance.Rotation = this.Rotation;
		bulletInstance.initializer = this;
		GetParent().AddChild(bulletInstance);
		bulletsFired.Add(bulletInstance);
    }

	internal void PopBullet(Bullet bullet)
	{
		bulletsFired.Remove(bullet);
	}

	internal System.Collections.Generic.Dictionary<Side, Entity> LookAt()
	{
		hitResults.Clear();

        spaceState = GetWorld2D().DirectSpaceState;
		//Middle Raycast
        // use global coordinates, not local to node
        query_m = PhysicsRayQueryParameters2D.Create(GlobalPosition, ToGlobal(new Vector2(0, -1500)));
		query_m.CollideWithAreas = true;
        query_m.Exclude = [GetRid(), .. bulletsFired.Select(b => b.GetRid()).ToArray()];
        rayQueryResult = spaceState.IntersectRay(query_m);

		if(rayQueryResult.Count > 0)
		{
			var entity = rayQueryResult["collider"].As<CollisionObject2D>();
            hitResults.Add(Side.Middle, GetEntityInPath(entity));
		}

        //Left Raycast
        query_l = PhysicsRayQueryParameters2D.Create(GlobalPosition, ToGlobal(new Vector2(-150, -1500)));
        query_l.CollideWithAreas = true;
        query_l.Exclude = [GetRid(), .. bulletsFired.Select(b => b.GetRid()).ToArray()];
        rayQueryResult = spaceState.IntersectRay(query_l);

        if (rayQueryResult.Count > 0)
        {
            var entity = rayQueryResult["collider"].As<CollisionObject2D>();
            hitResults.Add(Side.Left, GetEntityInPath(entity));
        }

        //Right Raycast
        query_r = PhysicsRayQueryParameters2D.Create(GlobalPosition, ToGlobal(new Vector2(150, -1500)));
        query_r.CollideWithAreas = true;
        query_r.Exclude = [GetRid(), .. bulletsFired.Select(b => b.GetRid()).ToArray()];
        rayQueryResult = spaceState.IntersectRay(query_r);

        if (rayQueryResult.Count > 0)
        {
            var entity = rayQueryResult["collider"].As<CollisionObject2D>();
            hitResults.Add(Side.Right, GetEntityInPath(entity));
        }

        return hitResults;
    }

	internal Entity GetEntityInPath(CollisionObject2D entity)
	{
        Entity entityInPath = new Entity();
        entityInPath.eType = (entity as IEntity).eType;
        entityInPath.globalPosition = entity.GlobalPosition;
        entityInPath.rotation = entity.Rotation;
        entityInPath.distanceTo = ((Vector2)rayQueryResult["position"]).DistanceTo(_collisionShape.GlobalPosition) - (_collisionShape.Shape.GetRect().Size.Y / 2);
		return entityInPath;
    }

    public override void _Draw()
    {
		DrawLine(new Vector2(0, 0), new Vector2(0, -1500), Colors.Green, 2); //Middle
		DrawLine(new Vector2(0, 0), new Vector2(150, -1500), Colors.Red, 2); //Right
		DrawLine(new Vector2(0, 0), new Vector2(-150, -1500), Colors.Blue, 2); //Left
    }

	internal void SetupScoreboard(TankSetup setup)
	{
		scorePanel = GD.Load<PackedScene>("res://Scenes/Score_Panel.tscn").Instantiate<Node>();
		scorePanel.Set("tank_health", health);
		scorePanel.Set("tank_name", setup.name.Length > 12 ? setup.name.Substring(0, 10) + "..." : setup.name);

        _tankScoreContainer.AddChild(scorePanel);
		scorePanel.Call("change_health", health);
		scorePanel.Call("change_panel_color", setup.primaryColor, setup.secondaryColor);

    }

    internal void Hurt()
	{
		health--;
        //_scoreboard.ScoreChanged(team);

        _healthBar.Value = health;
        scorePanel.Call("change_health", health);

        if (health <= 0)
			this.QueueFree();
    }

	internal void Score()
	{
		points++;
		scorePanel.Call("change_points", points);
    }
}

