#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

struct Light
{
	float3 dir;				// world space direction
	float3 pos;				// world space position
	float4 ambient;
	float4 diffuse;
	float4 specular;
	float spotInnerCone;	// spot light inner cone (theta) angle
	float spotOuterCone;	// spot light outer cone (phi) angle
	float radius;           // applies to point and spot lights only
};

struct Material
{
	float4 ambient;
	float4 diffuse;
	float4 emissive;
	float4 specular;
	float shininess;
};

//-----------------------------------------------------------------------------
// Globals.
//-----------------------------------------------------------------------------

float4x4 worldMatrix;
float4x4 worldInverseTransposeMatrix;
float4x4 worldViewProjectionMatrix;

float3 cameraPos;
float4 globalAmbient;

Light light;
Material material;

//-----------------------------------------------------------------------------
// Textures.
//-----------------------------------------------------------------------------

texture colorMapTexture;
texture normalMapTexture;

sampler2D colorMap = sampler_state
{
	Texture = <colorMapTexture>;
	MagFilter = Linear;
	MinFilter = Anisotropic;
	MipFilter = Linear;
	MaxAnisotropy = 16;
};

sampler2D normalMap = sampler_state
{
	Texture = <normalMapTexture>;
	MagFilter = Linear;
	MinFilter = Anisotropic;
	MipFilter = Linear;
	MaxAnisotropy = 16;
};

//-----------------------------------------------------------------------------
// Vertex Shaders.
//-----------------------------------------------------------------------------

struct VS_INPUT
{
	float3 position : POSITION;
	float2 texCoord : TEXCOORD0;
	float3 normal : NORMAL;
	float4 tangent : TANGENT;
};

struct VS_OUTPUT_DIR
{
	float4 position : POSITION;
	float2 texCoord : TEXCOORD0;
	float3 halfVector : TEXCOORD1;
	float3 lightDir : TEXCOORD2;
	float4 diffuse : COLOR0;
	float4 specular : COLOR1;
};

struct VS_OUTPUT_POINT
{
	float4 position : POSITION;
	float2 texCoord : TEXCOORD0;
	float3 viewDir : TEXCOORD1;
	float3 lightDir : TEXCOORD2;
	float4 diffuse : COLOR0;
	float4 specular : COLOR1;
};

struct VS_OUTPUT_SPOT
{
	float4 position : POSITION;
	float2 texCoord : TEXCOORD0;
	float3 viewDir : TEXCOORD1;
	float3 lightDir : TEXCOORD2;
	float3 spotDir : TEXCOORD3;
	float4 diffuse : COLOR0;
	float4 specular : COLOR1;
};

VS_OUTPUT_DIR VS_DirLighting(VS_INPUT IN)
{
	VS_OUTPUT_DIR OUT;

	float3 worldPos = mul(float4(IN.position, 1.0f), worldMatrix).xyz;
	float3 lightDir = -light.dir;
	float3 viewDir = cameraPos - worldPos;
	float3 halfVector = normalize(normalize(lightDir) + normalize(viewDir));

	float3 n = mul(IN.normal, (float3x3)worldInverseTransposeMatrix);
	float3 t = mul(IN.tangent.xyz, (float3x3)worldInverseTransposeMatrix);
	float3 b = cross(n, t) * IN.tangent.w;
	float3x3 tbnMatrix = float3x3(t.x, b.x, n.x,
		t.y, b.y, n.y,
		t.z, b.z, n.z);

	OUT.position = mul(float4(IN.position, 1.0f), worldViewProjectionMatrix);
	OUT.texCoord = IN.texCoord;
	OUT.halfVector = mul(halfVector, tbnMatrix);
	OUT.lightDir = mul(lightDir, tbnMatrix);
	OUT.diffuse = material.diffuse * light.diffuse;
	OUT.specular = material.specular * light.specular;

	return OUT;
}

VS_OUTPUT_POINT VS_PointLighting(VS_INPUT IN)
{
	VS_OUTPUT_POINT OUT;

	float3 worldPos = mul(float4(IN.position, 1.0f), worldMatrix).xyz;
	float3 viewDir = cameraPos - worldPos;
	float3 lightDir = (light.pos - worldPos) / light.radius;

	float3 n = mul(IN.normal, (float3x3)worldInverseTransposeMatrix);
	float3 t = mul(IN.tangent.xyz, (float3x3)worldInverseTransposeMatrix);
	float3 b = cross(n, t) * IN.tangent.w;
	float3x3 tbnMatrix = float3x3(t.x, b.x, n.x,
		t.y, b.y, n.y,
		t.z, b.z, n.z);

	OUT.position = mul(float4(IN.position, 1.0f), worldViewProjectionMatrix);
	OUT.texCoord = IN.texCoord;
	OUT.viewDir = mul(viewDir, tbnMatrix);
	OUT.lightDir = mul(lightDir, tbnMatrix);
	OUT.diffuse = material.diffuse * light.diffuse;
	OUT.specular = material.specular * light.specular;

	return OUT;
}

VS_OUTPUT_SPOT VS_SpotLighting(VS_INPUT IN)
{
	VS_OUTPUT_SPOT OUT;

	float3 worldPos = mul(float4(IN.position, 1.0f), worldMatrix).xyz;
	float3 viewDir = cameraPos - worldPos;
	float3 lightDir = (light.pos - worldPos) / light.radius;

	float3 n = mul(IN.normal, (float3x3)worldInverseTransposeMatrix);
	float3 t = mul(IN.tangent.xyz, (float3x3)worldInverseTransposeMatrix);
	float3 b = cross(n, t) * IN.tangent.w;
	float3x3 tbnMatrix = float3x3(t.x, b.x, n.x,
		t.y, b.y, n.y,
		t.z, b.z, n.z);

	OUT.position = mul(float4(IN.position, 1.0f), worldViewProjectionMatrix);
	OUT.texCoord = IN.texCoord;
	OUT.viewDir = mul(viewDir, tbnMatrix);
	OUT.lightDir = mul(lightDir, tbnMatrix);
	OUT.spotDir = mul(light.dir, tbnMatrix);
	OUT.diffuse = material.diffuse * light.diffuse;
	OUT.specular = material.specular * light.specular;

	return OUT;
}

//-----------------------------------------------------------------------------
// Pixel Shaders.
//-----------------------------------------------------------------------------

float4 PS_DirLighting(VS_OUTPUT_DIR IN) : COLOR
{
	float3 n = normalize(tex2D(normalMap, IN.texCoord).rgb * 2.0f - 1.0f);
	float3 h = normalize(IN.halfVector);
	float3 l = normalize(IN.lightDir);

	float nDotL = saturate(dot(n, l));
	float nDotH = saturate(dot(n, h));
	float power = (nDotL == 0.0f) ? 0.0f : pow(nDotH, material.shininess);

	float4 color = (material.ambient * (globalAmbient + light.ambient)) +
		(IN.diffuse * nDotL) + (IN.specular * power);

	return color * tex2D(colorMap, IN.texCoord);
}

float4 PS_PointLighting(VS_OUTPUT_POINT IN) : COLOR
{
	float atten = saturate(1.0f - dot(IN.lightDir, IN.lightDir));

float3 n = normalize(tex2D(normalMap, IN.texCoord).rgb * 2.0f - 1.0f);
float3 l = normalize(IN.lightDir);
float3 v = normalize(IN.viewDir);
float3 h = normalize(l + v);

float nDotL = saturate(dot(n, l));
float nDotH = saturate(dot(n, h));
float power = (nDotL == 0.0f) ? 0.0f : pow(nDotH, material.shininess);

float4 color = (material.ambient * (globalAmbient + (atten * light.ambient))) +
(IN.diffuse * nDotL * atten) + (IN.specular * power * atten);

return color * tex2D(colorMap, IN.texCoord);
}

float4 PS_SpotLighting(VS_OUTPUT_SPOT IN) : COLOR
{
	float atten = saturate(1.0f - dot(IN.lightDir, IN.lightDir));

float3 l = normalize(IN.lightDir);
float2 cosAngles = cos(float2(light.spotOuterCone, light.spotInnerCone) * 0.5f);
float spotDot = dot(-l, normalize(IN.spotDir));
float spotEffect = smoothstep(cosAngles[0], cosAngles[1], spotDot);

atten *= spotEffect;

float3 n = normalize(tex2D(normalMap, IN.texCoord).rgb * 2.0f - 1.0f);
float3 v = normalize(IN.viewDir);
float3 h = normalize(l + v);

float nDotL = saturate(dot(n, l));
float nDotH = saturate(dot(n, h));
float power = (nDotL == 0.0f) ? 0.0f : pow(nDotH, material.shininess);

float4 color = (material.ambient * (globalAmbient + (atten * light.ambient))) +
(IN.diffuse * nDotL * atten) + (IN.specular * power * atten);

return color * tex2D(colorMap, IN.texCoord);
}

//-----------------------------------------------------------------------------
// Techniques.
//-----------------------------------------------------------------------------

technique NormalMappingDirectionalLighting
{
	pass
	{
		VertexShader = compile VS_SHADERMODEL VS_DirLighting();
		PixelShader = compile PS_SHADERMODEL PS_DirLighting();
	}
}

technique NormalMappingPointLighting
{
	pass
	{
		VertexShader = compile VS_SHADERMODEL VS_PointLighting();
		PixelShader = compile PS_SHADERMODEL PS_PointLighting();
	}
}

technique NormalMappingSpotLighting
{
	pass
	{
		VertexShader = compile VS_SHADERMODEL VS_SpotLighting();
		PixelShader = compile PS_SHADERMODEL PS_SpotLighting();
	}
}
