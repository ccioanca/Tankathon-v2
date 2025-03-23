class_name ScorePanel
extends MarginContainer

@export
var tank_name := ""
@export
var tank_health = 0

var tank_name_panel
var tank_health_panel

func _ready():
	tank_name_panel = get_node("HBox/TankName") as Label;
	tank_name_panel.text = tank_name
	
	tank_health_panel = get_node("HBox/TankHealth") as Label;
	change_health(tank_health)
	pass

func change_health(health):
	tank_health = health
	tank_health_panel.text = "%s/10" % tank_health
	
