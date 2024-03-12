// vertex shader
#version 430 core

// vertex position
layout (location = 0) in vec3 aPos;

// normal
layout (location = 1) in vec3 aNormal;

// texture coordinate
layout (location = 2) in vec2 aTex;

out vec2 TexCoords;
out vec3 Normal;

void main()
{
    gl_Position = vec4(aPos, 1.0);
    TexCoords = aTex;
    Normal = aNormal;
}