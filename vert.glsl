#version 330

in vec2 position;
out vec4 color;
uniform vec2 resolution;
void main()
{	
	gl_Position = vec4(position/resolution, 0.0, 1.0);
	color = vec4(position/resolution, 1.0, 1.0);
}