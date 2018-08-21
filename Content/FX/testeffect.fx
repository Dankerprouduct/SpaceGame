#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

sampler mainSampler : register(s0);
sampler lightSampler = sampler_state{Texture = <lightMask>;};
float4x4 World;
float4x4 View;
float4x4 Projection;

struct vs2ps
{
    float4 Pos : POSITION0;
    float4 TexCd : TEXCOORD0;
    float3 PosW : TEXCOORD1;
};

vs2ps VS(float4 Pos : POSITION0,float4 TexCd : TEXCOORD0)
{
    vs2ps Out;
float4 worldPosition = mul(Pos, World);
float4 viewPosition = mul(worldPosition, View);
    Out.Pos = mul(viewPosition, Projection);
    Out.TexCd = TexCd;
    Out.PosW = mul(Pos,World);
    return Out;
    }

float4 PixelShaderFunction(vs2ps input) : COLOR0
{   float2 texCoord = input.TexCd;
    float2 screenPosition = input.PosW;
    float4 lightColor = tex2D(lightSampler, texCoord);
    float4 mainColor = tex2D(mainSampler, texCoord);
    if(screenPosition.x < 3500)
    {
         return (mainColor * lightColor);
    }
         else return mainColor;
}

technique Technique1  
    {  
        pass Pass1  
        {  
            PixelShader = compile ps_5_0 PixelShaderFunction();  
        }  
    } 
