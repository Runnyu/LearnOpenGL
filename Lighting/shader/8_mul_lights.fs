#version 330 core
in vec3 Normal;
in vec3 FragPos;
in vec2 TexCoords;

out vec4 FragColor;

uniform vec3 viewPos;

struct Material {
    sampler2D diffuse;
    sampler2D specular;
    sampler2D emission;
    float shininess;
}; 

uniform Material material;

// 定向光
struct DirLight {
    vec3 direction;

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};
uniform DirLight dirLight;

vec3 CalcDirLight(DirLight dirLight, vec3 norm, vec3 viewDir)
{
    // 环境光
    vec3 ambient = dirLight.ambient * vec3(texture(material.diffuse, TexCoords));

    // 漫反射
    vec3 lightDir = normalize(-dirLight.direction);
	float diff = max(dot(norm, lightDir), 0.0);
	vec3 diffuse = dirLight.diffuse * diff * vec3(texture(material.diffuse, TexCoords));

	// 镜面反射
	vec3 reflectDir = reflect(-lightDir, norm);
	float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
	vec3 specular = dirLight.specular * spec * vec3(texture(material.specular, TexCoords));

	return (ambient + diffuse + specular);
}

// 点光源
struct PointLight {
    vec3 position;

    float constant;
    float linear;
    float quadratic;

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};
#define NR_POINT_LIGHTS 4
uniform PointLight pointLights[NR_POINT_LIGHTS];

vec3 CalcPointLight(PointLight light, vec3 normal, vec3 fragPos, vec3 viewDir)
{
    vec3 lightDir = normalize(light.position - fragPos);
    // 漫反射
    float diff = max(dot(normal, lightDir), 0.0);
    // 镜面反射
    vec3 reflectDir = reflect(-lightDir, normal);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    // 衰减
    float distance = length(light.position - fragPos);
    float attenuation = 1.0 / (light.constant + light.linear * distance + light.quadratic * (distance * distance));
    // 合并结果
    vec3 ambient = light.ambient * vec3(texture(material.diffuse, TexCoords));
    vec3 diffuse = light.diffuse * diff * vec3(texture(material.diffuse, TexCoords));
    vec3 specular= light.specular * spec * vec3(texture(material.diffuse, TexCoords));
    return (ambient + diffuse + specular) * attenuation;
}

struct SpotLight {
    vec3 position;
    vec3 direction;
    float cutOff;
    float outerCutOff;

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;

    float constant;
    float linear;
    float quadratic;
};
uniform SpotLight spotLight;

vec3 CalcSpotLight(SpotLight spotLight, vec3 norm, vec3 FragPos, vec3 viewDir)
{
    vec3 spotLightDir = normalize(spotLight.position - FragPos);
    float theta = dot(spotLightDir, normalize(-spotLight.direction));
    float epsilon = spotLight.cutOff - spotLight.outerCutOff;
    float intensity = clamp((theta - spotLight.outerCutOff) / epsilon, 0.0, 1.0);   

    // 环境光
    vec3 ambient = spotLight.ambient * vec3(texture(material.diffuse, TexCoords));

    // 漫反射
    float diff = max(dot(norm, spotLightDir), 0.0);
    vec3 diffuse = spotLight.diffuse * diff * vec3(texture(material.diffuse, TexCoords)) * intensity;

    // 镜面反射
    vec3 reflectDir = reflect(-spotLightDir, norm);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    vec3 specular = spotLight.specular * spec * vec3(texture(material.specular, TexCoords)) * intensity;

    float distance = length(spotLight.position - FragPos);
    float attenuation = 1.0 / (spotLight.constant + spotLight.linear * distance + spotLight.quadratic * (distance * distance));

    return (ambient + diffuse + specular) * attenuation;
}

void main()
{
    // 属性
    vec3 norm = normalize(Normal);
    vec3 viewDir = normalize(viewPos - FragPos);

    // 定向光
	vec3 lightResult = CalcDirLight(dirLight, norm, viewDir);

    // 点光源
    for (int i = 0; i < NR_POINT_LIGHTS; i++)
        lightResult += CalcPointLight(pointLights[i], norm, FragPos, viewDir); 

    // 聚光
    lightResult += CalcSpotLight(spotLight, norm, FragPos, viewDir);

    // 放射光
    vec3 emission = vec3(texture(material.emission, TexCoords));
    vec3 result = emission.x > 0.0 ? emission : lightResult;

	FragColor = vec4(result, 1.0);
}