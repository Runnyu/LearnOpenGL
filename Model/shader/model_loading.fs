#version 330 core
out vec4 FragColor;  
in vec3 ourColor;
in vec2 TexCoords;

uniform sampler2D texture_diffuse1;
uniform sampler2D texture_diffuse2;
uniform sampler2D texture_diffuse3;
uniform sampler2D texture_specular1;
uniform sampler2D texture_specular2;

void main()
{
    FragColor = vec4(vec3(texture(texture_diffuse1, TexCoords)), 1.0);
}