#version 330 core
out vec4 FragColor;

in vec2 TexCoords;
in vec3 Normal;
in vec3 Position;
in vec4 clipSpace;

uniform vec3 cameraPos;
uniform samplerCube skybox;
uniform sampler2D teapot;

void main()
{    
    vec3 I = normalize(Position - cameraPos);
    vec3 R = reflect(I, normalize(Normal));
    vec2 ndc = (clipSpace.xy/clipSpace.w)/2.0 + 0.5;
    vec2 reflectTexCoords = vec2(1.0 - ndc.x, ndc.y);
    vec4 teapotColor = texture(teapot, reflectTexCoords);
    vec4 envColor = vec4(texture(skybox, R).rgb, 1.0);
    vec4 result = teapotColor;
    if (teapotColor == vec4(0.f, 0.f, 0.f, 1.f))
    {
        result = envColor;
    }
    FragColor = result;
    
}