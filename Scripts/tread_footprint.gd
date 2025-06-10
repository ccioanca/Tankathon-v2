extends Marker2D

# The scene for a single piece of the track marks.
@export var segment_scene: PackedScene
# The length of one texture segment. (match the segment texture width - after any scaling)
@export var segment_length: float = 24.0
# The maximum number of segments to keep on screen.
@export var max_segments: int = 100

# A queue to keep track of the segments in order.
var _segments: Array[Node2D] = []
# The position where the last segment was laid down.
var _last_spawn_position: Vector2

# A flag to ensure we have a starting point.
var _is_started: bool = false

func _process(_delta):
	
	# If the target node isn't set, do nothing.
	if not is_instance_valid(self):
		return

	var current_position = global_position

	# On the first frame, set the starting position and exit.
	if not _is_started:
		_last_spawn_position = current_position
		_is_started = true
		return
	
	# --- Core Logic ---
	# Continuously check the distance between the current position and the last spawn position.
	while _last_spawn_position.distance_to(current_position) > segment_length:
		# --- 1. Calculate Position and Rotation ---
		# Get the direction from the last spawn point to the current point.
		var direction = _last_spawn_position.direction_to(current_position)
		# Determine the position for the new segment. It starts where the last one ended.
		var spawn_position = _last_spawn_position + direction * segment_length
		
		# --- 2. Instantiate and Place Segment ---
		var new_segment = segment_scene.instantiate()
		new_segment.set_as_top_level(true);
		add_child(new_segment)
		
		new_segment.global_position = _last_spawn_position
		# The rotation is the angle of the direction vector.
		new_segment.rotation = direction.angle()
		
		# Add the new segment to our queue.
		_segments.append(new_segment)
		
		# --- 3. Update State for Next Loop Iteration ---
		# The next segment will start from the end of the one we just placed.
		_last_spawn_position = spawn_position

		# --- 4. Enforce Max Length ---
		# If we have too many segments, remove the oldest one.
		if _segments.size() > max_segments:
			var old_segment = _segments.pop_front()
			old_segment.queue_free()
