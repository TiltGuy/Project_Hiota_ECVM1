// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Platforms"
{
	Properties
	{
		_EdgeLength ( "Edge length", Range( 2, 50 ) ) = 15
		_PlatformArenaLOW_DefaultMaterial_Albedo("Platform-Arena-LOW_DefaultMaterial_Albedo", 2D) = "white" {}
		_PlatformArenaLOW_DefaultMaterial_AO("Platform-Arena-LOW_DefaultMaterial_AO", 2D) = "white" {}
		_PlatformArenaLOW_DefaultMaterial_Metallic("Platform-Arena-LOW_DefaultMaterial_Metallic", 2D) = "white" {}
		_PlatformArenaLOW_DefaultMaterial_Normal("Platform-Arena-LOW_DefaultMaterial_Normal", 2D) = "bump" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#include "Tessellation.cginc"
		#pragma target 4.6
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc tessellate:tessFunction 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _PlatformArenaLOW_DefaultMaterial_Normal;
		uniform float4 _PlatformArenaLOW_DefaultMaterial_Normal_ST;
		uniform sampler2D _PlatformArenaLOW_DefaultMaterial_Albedo;
		uniform float4 _PlatformArenaLOW_DefaultMaterial_Albedo_ST;
		uniform sampler2D _PlatformArenaLOW_DefaultMaterial_Metallic;
		uniform float4 _PlatformArenaLOW_DefaultMaterial_Metallic_ST;
		uniform sampler2D _PlatformArenaLOW_DefaultMaterial_AO;
		uniform float4 _PlatformArenaLOW_DefaultMaterial_AO_ST;
		uniform float _EdgeLength;

		float4 tessFunction( appdata_full v0, appdata_full v1, appdata_full v2 )
		{
			return UnityEdgeLengthBasedTess (v0.vertex, v1.vertex, v2.vertex, _EdgeLength);
		}

		void vertexDataFunc( inout appdata_full v )
		{
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_PlatformArenaLOW_DefaultMaterial_Normal = i.uv_texcoord * _PlatformArenaLOW_DefaultMaterial_Normal_ST.xy + _PlatformArenaLOW_DefaultMaterial_Normal_ST.zw;
			o.Normal = UnpackNormal( tex2D( _PlatformArenaLOW_DefaultMaterial_Normal, uv_PlatformArenaLOW_DefaultMaterial_Normal ) );
			float2 uv_PlatformArenaLOW_DefaultMaterial_Albedo = i.uv_texcoord * _PlatformArenaLOW_DefaultMaterial_Albedo_ST.xy + _PlatformArenaLOW_DefaultMaterial_Albedo_ST.zw;
			o.Albedo = tex2D( _PlatformArenaLOW_DefaultMaterial_Albedo, uv_PlatformArenaLOW_DefaultMaterial_Albedo ).rgb;
			float2 uv_PlatformArenaLOW_DefaultMaterial_Metallic = i.uv_texcoord * _PlatformArenaLOW_DefaultMaterial_Metallic_ST.xy + _PlatformArenaLOW_DefaultMaterial_Metallic_ST.zw;
			o.Metallic = tex2D( _PlatformArenaLOW_DefaultMaterial_Metallic, uv_PlatformArenaLOW_DefaultMaterial_Metallic ).r;
			float2 uv_PlatformArenaLOW_DefaultMaterial_AO = i.uv_texcoord * _PlatformArenaLOW_DefaultMaterial_AO_ST.xy + _PlatformArenaLOW_DefaultMaterial_AO_ST.zw;
			o.Occlusion = tex2D( _PlatformArenaLOW_DefaultMaterial_AO, uv_PlatformArenaLOW_DefaultMaterial_AO ).r;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18703
-1920;2;1920;1019;1210.804;607.7936;1.3;True;True
Node;AmplifyShaderEditor.SamplerNode;1;-129.2024,-430.9944;Inherit;True;Property;_PlatformArenaLOW_DefaultMaterial_Albedo;Platform-Arena-LOW_DefaultMaterial_Albedo;5;0;Create;True;0;0;False;0;False;-1;280976332a6e2d34fb9223adfed868c8;280976332a6e2d34fb9223adfed868c8;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;7;-181.2029,-191.7945;Inherit;True;Property;_PlatformArenaLOW_DefaultMaterial_Normal;Platform-Arena-LOW_DefaultMaterial_Normal;9;0;Create;True;0;0;False;0;False;-1;c4fa3f05701b9d349aecd4885b138a37;c4fa3f05701b9d349aecd4885b138a37;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;6;-538.703,-5.894461;Inherit;True;Property;_PlatformArenaLOW_DefaultMaterial_Metallic;Platform-Arena-LOW_DefaultMaterial_Metallic;8;0;Create;True;0;0;False;0;False;-1;e8523e5dc38c82349b19688975a2491f;e8523e5dc38c82349b19688975a2491f;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;2;-597.2025,295.7056;Inherit;True;Property;_PlatformArenaLOW_DefaultMaterial_AO;Platform-Arena-LOW_DefaultMaterial_AO;6;0;Create;True;0;0;False;0;False;-1;b18a3f9fb45bd844c820b9ceaef012c2;b18a3f9fb45bd844c820b9ceaef012c2;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;3;-111.0024,380.2059;Inherit;True;Property;_PlatformArenaLOW_DefaultMaterial_Height;Platform-Arena-LOW_DefaultMaterial_Height;7;0;Create;True;0;0;False;0;False;-1;83ee6dfd2a65faa4cb5e9e840364cc83;83ee6dfd2a65faa4cb5e9e840364cc83;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;5;313.3,18.2;Float;False;True;-1;6;ASEMaterialInspector;0;0;Standard;Platforms;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;0;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;5;0;1;0
WireConnection;5;1;7;0
WireConnection;5;3;6;0
WireConnection;5;5;2;0
ASEEND*/
//CHKSM=A6A225869AF7D095CB2C691C535BC2F02F056F81