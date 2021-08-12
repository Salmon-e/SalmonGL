#version 330 core

out vec4 FragColor;
in vec4 color;
in vec2 aTexCoord;
uniform sampler2D texture0;
uniform sampler2D texture1;
void main()
{
    FragColor = texture(texture0, aTexCoord)*texture(texture1, aTexCoord)*color;
}