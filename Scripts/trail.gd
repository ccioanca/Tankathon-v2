extends CPUParticles2D

var timer := Timer.new()

# Called when the node enters the scene tree for the first time.
func _ready():
	add_child(timer)
	timer.wait_time = 2.0
	timer.one_shot = true
	timer.timeout.connect(on_timer_timeout)
	
	#change angle dependent on parent.
	angle_min = -get_parent().rotation_degrees - 15;
	angle_max = -get_parent().rotation_degrees + 15;


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _physics_process(_delta):	
	if not emitting and timer.is_stopped():
		timer.start();

func on_timer_timeout():
	queue_free()
