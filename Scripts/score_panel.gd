class_name pointsPanel
extends MarginContainer

@export
var tank_name := ""
@export
var tank_health = 0
@export
var tank_points := 0

var tank_name_panel
var tank_health_panel
var tank_points_panel

func _ready():
	tank_name_panel = get_node("HBox/TankName") as Label;
	tank_name_panel.text = tank_name
	
	tank_health_panel = get_node("HBox/TankHealth") as Label;
	change_health(tank_health)
	
	tank_points_panel = get_node("HBox/Panel/TankPoints") as Label;
	change_points(tank_points)
	pass

func change_health(health):
	tank_health = health
	tank_health_panel.text = "%s/10" % tank_health
	
func change_points(points):
	tank_points = points
	tank_points_panel.text = "%s" % tank_points
	
