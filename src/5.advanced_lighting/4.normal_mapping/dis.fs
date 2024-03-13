#version 430 core

out vec4 FragColor;

uniform sampler2D normalMap;
uniform sampler2D shadowMap;

uniform vec3 lightPos;
uniform vec3 viewPos;

in vec2 teTexCoord;
in vec3 tePosition;
in float Height;
in mat3 TBN;
in mat3 normalMat;
in vec3 mNormal;

in TES_OUT {
    vec3 FragPos;
    vec4 FragPosLightSpace;
} fs_in;

float ShadowCalculation(vec4 fragPosLightSpace, vec3 Normal)
{
    vec3 projCoords = fragPosLightSpace.xyz / fragPosLightSpace.w;
    projCoords = projCoords * 0.5 + 0.5;
    float closestDepth = texture(shadowMap, projCoords.xy).r;
    float currentDepth = projCoords.z;
    vec3 normal = normalize(normalMat * Normal);
    vec3 lightDir = normalize(lightPos - fs_in.FragPos);
    float bias = max(0.05 * (1.0 - dot(normal, lightDir)), 0.005);
    float shadow = 0.0;
    vec2 texelSize = 1.0 / textureSize(shadowMap, 0);
    for(int x = -1; x <= 1; ++x)
    {
        for(int y = -1; y <= 1; ++y)
        {
            float pcfDepth = texture(shadowMap, projCoords.xy + vec2(x, y) * texelSize).r; 
            shadow += currentDepth - bias > pcfDepth ? 1.0 : 0.0;        
        }    
    }
    shadow /= 9.0;
    if(projCoords.z > 1.0)
        shadow = 0.0;
    return shadow;
}

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

    // calculate shadow
    float shadow = ShadowCalculation(fs_in.FragPosLightSpace, mNormal);

    vec3 specular = vec3(0.2) * spec;
    FragColor = vec4(ambient + (1.0 - shadow)*(diffuse + specular), 1.0);
    //FragColor = vec4(vec3(1.0 - shadow), 1.0);
}