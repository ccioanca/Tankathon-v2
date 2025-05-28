shader_type canvas_item;

uniform vec4 _oldcolor1 : source_color;
uniform vec4 _oldcolor2 : source_color;
uniform vec4 _newcolor1 : source_color;
uniform vec4 _newcolor2 : source_color;

void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	// Called for every pixel the material is visible on.
	vec4 cur_color = texture(TEXTURE, UV);
	
	if(cur_color == _oldcolor1)
	{
		COLOR = _newcolor1;
	}
	else if(cur_color == _oldcolor2)
	{
		COLOR = _newcolor2;		
	}
}

//void light() {
	// Called for every pixel for every light affecting the CanvasItem.
	// Uncomment to replace the default light processing function with this one.
//}
