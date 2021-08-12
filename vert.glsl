#version 330

in vec2 position;
in vec4 inColor;
out vec4 color;
uniform vec2 resolution;
void main()
{	
	gl_Position = vec4(position/resolution, 0.0, 1.0);
	color = inColor;
}