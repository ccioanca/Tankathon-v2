extends Sprite2D

var aliveDelta:float = 0.0;

@export var color:Color;

func _process(delta: float) -> void:
	aliveDelta += delta;
	set("modulate", Color(color, (3-aliveDelta)/3))
