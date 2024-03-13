#version 430 core
out vec4 FragColor;

uniform vec3 lightPos;
uniform vec3 viewPos;

void main()
{           
    FragColor = vec4(1.0, 1.0, 0.0, 1.0);
}