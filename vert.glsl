#version 330

in vec2 position;
in vec4 inColor;
in vec2 texCoord;
out vec4 color;
out vec2 aTexCoord;
uniform vec2 resolution;
void main()
{	
	gl_Position = vec4(position/resolution, 0.0, 1.0);
	color = inColor;
	aTexCoord = texCoord;
}