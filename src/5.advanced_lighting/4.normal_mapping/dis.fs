#version 430 core

out vec4 FragColor;

uniform sampler2D normalMap;

uniform vec3 lightPos;
uniform vec3 viewPos;

in vec2 teTexCoord;
in vec3 tePosition;
in float Height;
in mat3 TBN;

void main()
{
    vec3 TangentLightPos = TBN * lightPos;
    vec3 TangentViewPos = TBN * viewPos;
    vec3 TangentFragPos = TBN * tePosition;

    vec3 normal = texture(normalMap, teTexCoord).rgb;
    normal = normalize(normal * 2.0 - 1.0);

    vec3 color = vec3(0.7);
    vec3 ambient = 0.1 * color;
    vec3 lightDir = normalize(TangentLightPos - TangentFragPos);
    float diff = max(dot(normal, lightDir), 0.0);
    vec3 diffuse = diff * color;

    vec3 viewDir = normalize(TangentViewPos - TangentFragPos);
    vec3 reflectDir = reflect(-lightDir, normal);
    vec3 halfwayDir = normalize(lightDir + viewDir);
    float spec = pow(max(dot(normal, halfwayDir), 0.0), 32.0);

    vec3 specular = vec3(0.2) * spec;
    FragColor = vec4(ambient + diffuse + specular, 1.0);
    //FragColor = vec4(normalize(specular), 1.0);
}