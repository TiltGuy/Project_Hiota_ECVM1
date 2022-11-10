// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Platforms"
{
	Properties
	{
		_EdgeLength ( "Edge length", Range( 2, 50 ) ) = 15
		_PlatformArenaLOW_DefaultMaterial_Normal("Platform-Arena-LOW_DefaultMaterial_Normal", 2D) = "bump" {}
		_PlatformArenaLOW_DefaultMaterial_Albedo("Platform-Arena-LOW_DefaultMaterial_Albedo", 2D) = "white" {}
		_PlatformArenaLOW_DefaultMaterial_AO("Platform-Arena-LOW_DefaultMaterial_AO", 2D) = "white" {}
		_Canvas_N("Canvas_N", 2D) = "bump" {}
		_PlatformArenaLOW_DefaultMaterial_Mask2("Platform-Arena-LOW_DefaultMaterial_Mask2", 2D) = "white" {}
		_PlatformArenaLOW_DefaultMaterial_Mask3("Platform-Arena-LOW_DefaultMaterial_Mask2", 2D) = "white" {}
		_NormalScale("NormalScale", Range( 0 , 1)) = 0.6285164
		_PlatformTop_Text("PlatformTop_Text", 2D) = "white" {}
		_NormalScale_Canvas("NormalScale_Canvas", Range( 0 , 1)) = 0.5497602
		_PlatformArenaLOW_DefaultMaterial_Metallic("Platform-Arena-LOW_DefaultMaterial_Metallic", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#include "UnityStandardUtils.cginc"
		#include "Tessellation.cginc"
		#pragma target 4.6
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc tessellate:tessFunction 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Canvas_N;
		uniform float4 _Canvas_N_ST;
		uniform float _NormalScale_Canvas;
		uniform sampler2D _PlatformArenaLOW_DefaultMaterial_Normal;
		uniform float4 _PlatformArenaLOW_DefaultMaterial_Normal_ST;
		uniform float _NormalScale;
		uniform sampler2D _PlatformArenaLOW_DefaultMaterial_Mask3;
		uniform float4 _PlatformArenaLOW_DefaultMaterial_Mask3_ST;
		uniform sampler2D _PlatformTop_Text;
		uniform sampler2D _PlatformArenaLOW_DefaultMaterial_Albedo;
		uniform float4 _PlatformArenaLOW_DefaultMaterial_Albedo_ST;
		uniform sampler2D _PlatformArenaLOW_DefaultMaterial_Mask2;
		uniform float4 _PlatformArenaLOW_DefaultMaterial_Mask2_ST;
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
			float2 uv_Canvas_N = i.uv_texcoord * _Canvas_N_ST.xy + _Canvas_N_ST.zw;
			float2 uv_PlatformArenaLOW_DefaultMaterial_Normal = i.uv_texcoord * _PlatformArenaLOW_DefaultMaterial_Normal_ST.xy + _PlatformArenaLOW_DefaultMaterial_Normal_ST.zw;
			float2 uv_PlatformArenaLOW_DefaultMaterial_Mask3 = i.uv_texcoord * _PlatformArenaLOW_DefaultMaterial_Mask3_ST.xy + _PlatformArenaLOW_DefaultMaterial_Mask3_ST.zw;
			float3 lerpResult59 = lerp( UnpackScaleNormal( tex2D( _Canvas_N, uv_Canvas_N ), _NormalScale_Canvas ) , UnpackScaleNormal( tex2D( _PlatformArenaLOW_DefaultMaterial_Normal, uv_PlatformArenaLOW_DefaultMaterial_Normal ), _NormalScale ) , tex2D( _PlatformArenaLOW_DefaultMaterial_Mask3, uv_PlatformArenaLOW_DefaultMaterial_Mask3 ).rgb);
			o.Normal = lerpResult59;
			float2 temp_cast_1 = (3.588235).xx;
			float2 uv_TexCoord54 = i.uv_texcoord * temp_cast_1;
			float4 tex2DNode26 = tex2D( _PlatformTop_Text, uv_TexCoord54 );
			float2 uv_PlatformArenaLOW_DefaultMaterial_Albedo = i.uv_texcoord * _PlatformArenaLOW_DefaultMaterial_Albedo_ST.xy + _PlatformArenaLOW_DefaultMaterial_Albedo_ST.zw;
			float2 uv_PlatformArenaLOW_DefaultMaterial_Mask2 = i.uv_texcoord * _PlatformArenaLOW_DefaultMaterial_Mask2_ST.xy + _PlatformArenaLOW_DefaultMaterial_Mask2_ST.zw;
			float4 lerpResult33 = lerp( tex2DNode26 , tex2D( _PlatformArenaLOW_DefaultMaterial_Albedo, uv_PlatformArenaLOW_DefaultMaterial_Albedo ) , tex2D( _PlatformArenaLOW_DefaultMaterial_Mask2, uv_PlatformArenaLOW_DefaultMaterial_Mask2 ));
			o.Albedo = lerpResult33.rgb;
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
-980;11;876;1004;1093.449;1177.301;2.466734;False;False
Node;AmplifyShaderEditor.RangedFloatNode;55;-1098.287,-540.3071;Inherit;False;Constant;_Float0;Float 0;10;0;Create;True;0;0;False;0;False;3.588235;0;0;20;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;54;-773.4557,-557.9603;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;43;-949.7682,419.1648;Inherit;False;Property;_NormalScale;NormalScale;11;0;Create;True;0;0;False;0;False;0.6285164;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;45;-870.2841,206.3534;Inherit;False;Property;_NormalScale_Canvas;NormalScale_Canvas;13;0;Create;True;0;0;False;0;False;0.5497602;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;34;-517.1747,-377.7336;Inherit;True;Property;_PlatformArenaLOW_DefaultMaterial_Albedo;Platform-Arena-LOW_DefaultMaterial_Albedo;6;0;Create;True;0;0;False;0;False;-1;280976332a6e2d34fb9223adfed868c8;280976332a6e2d34fb9223adfed868c8;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;26;-513.056,-585.642;Inherit;True;Property;_PlatformTop_Text;PlatformTop_Text;12;0;Create;True;0;0;False;0;False;-1;3b89c3cabecee0148a13f7378bb65303;3b89c3cabecee0148a13f7378bb65303;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;53;-511.7816,-183.0802;Inherit;True;Property;_PlatformArenaLOW_DefaultMaterial_Mask2;Platform-Arena-LOW_DefaultMaterial_Mask2;9;0;Create;True;0;0;False;0;False;-1;ec2c8da72c568cd4a8d4ca79569d6869;ec2c8da72c568cd4a8d4ca79569d6869;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;38;-572.9689,401.5644;Inherit;True;Property;_PlatformArenaLOW_DefaultMaterial_Normal;Platform-Arena-LOW_DefaultMaterial_Normal;5;0;Create;True;0;0;False;0;False;-1;c4fa3f05701b9d349aecd4885b138a37;c4fa3f05701b9d349aecd4885b138a37;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;46;-555.7158,158.6093;Inherit;True;Property;_Canvas_N;Canvas_N;8;0;Create;True;0;0;False;0;False;-1;7ad0e904548935f4980cafbfebf050ab;7ad0e904548935f4980cafbfebf050ab;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;58;-561.1097,624.4646;Inherit;True;Property;_PlatformArenaLOW_DefaultMaterial_Mask3;Platform-Arena-LOW_DefaultMaterial_Mask2;10;0;Create;True;0;0;False;0;False;-1;ec2c8da72c568cd4a8d4ca79569d6869;ec2c8da72c568cd4a8d4ca79569d6869;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;33;-15.38338,-309.8977;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;39;648.3846,476.729;Inherit;True;Property;_PlatformArenaLOW_DefaultMaterial_AO;Platform-Arena-LOW_DefaultMaterial_AO;7;0;Create;True;0;0;False;0;False;-1;b18a3f9fb45bd844c820b9ceaef012c2;b18a3f9fb45bd844c820b9ceaef012c2;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;40;640.0161,678.2212;Inherit;True;Property;_PlatformArenaLOW_DefaultMaterial_Metallic;Platform-Arena-LOW_DefaultMaterial_Metallic;14;0;Create;True;0;0;False;0;False;-1;e8523e5dc38c82349b19688975a2491f;e8523e5dc38c82349b19688975a2491f;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;59;9.252417,260.1142;Inherit;True;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;5;700.0667,35.69011;Float;False;True;-1;6;ASEMaterialInspector;0;0;Standard;Platforms;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;2;15;10;25;False;0.75;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;0;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;54;0;55;0
WireConnection;26;1;54;0
WireConnection;38;5;43;0
WireConnection;46;5;45;0
WireConnection;33;0;26;0
WireConnection;33;1;34;0
WireConnection;33;2;53;0
WireConnection;59;0;46;0
WireConnection;59;1;38;0
WireConnection;59;2;58;0
WireConnection;5;0;33;0
WireConnection;5;1;59;0
WireConnection;5;3;40;0
WireConnection;5;5;39;0
ASEEND*/
//CHKSM=C012965A3B5CFDD52D1581B1AB1CA0F1C4D87EAE