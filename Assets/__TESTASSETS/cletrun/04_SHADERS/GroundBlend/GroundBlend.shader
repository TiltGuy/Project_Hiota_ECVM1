// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "GroundBlend"
{
	Properties
	{
		_Dirt("Dirt", 2D) = "white" {}
		_Dalles("Dalles", 2D) = "white" {}
		_Alpha("Alpha", 2D) = "white" {}
		_DallesN("Dalles-N", 2D) = "bump" {}
		_RockTextureN("Rock-TextureN", 2D) = "bump" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#pragma target 4.6
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _RockTextureN;
		uniform sampler2D _DallesN;
		uniform sampler2D _Alpha;
		SamplerState sampler_Alpha;
		uniform float4 _Alpha_ST;
		uniform sampler2D _Dirt;
		uniform sampler2D _Dalles;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_TexCoord10 = i.uv_texcoord * float2( 7,7 );
			float2 uv_Alpha = i.uv_texcoord * _Alpha_ST.xy + _Alpha_ST.zw;
			float4 tex2DNode9 = tex2D( _Alpha, uv_Alpha );
			float3 lerpResult13 = lerp( UnpackNormal( tex2D( _RockTextureN, uv_TexCoord10 ) ) , UnpackNormal( tex2D( _DallesN, uv_TexCoord10 ) ) , tex2DNode9.r);
			o.Normal = lerpResult13;
			float4 color14 = IsGammaSpace() ? float4(0.6698113,0.6413759,0.6413759,1) : float4(0.4061945,0.3689938,0.3689938,1);
			float4 lerpResult3 = lerp( tex2D( _Dirt, uv_TexCoord10 ) , tex2D( _Dalles, uv_TexCoord10 ) , tex2DNode9.r);
			o.Albedo = ( color14 * lerpResult3 ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18703
-1860;175;1463;754;3074.509;618.299;2.950141;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;10;-2035.418,172.321;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;7,7;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;9;-1601.47,405.4743;Inherit;True;Property;_Alpha;Alpha;2;0;Create;True;0;0;False;0;False;-1;f0733349d34b79649a68a0e806fbca43;02fb26f540063f643a1c9b10e59d1dc9;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;8;-1605.37,189.6743;Inherit;True;Property;_Dalles;Dalles;1;0;Create;True;0;0;False;0;False;-1;6b842f0d5800a0e4ba5ec56285bbee12;62588a2fd90136b449d1225b5085c757;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;7;-1606.67,-31.32572;Inherit;True;Property;_Dirt;Dirt;0;0;Create;True;0;0;False;0;False;-1;8cc83bf69920f99439e1fcf5392e2cc5;6b842f0d5800a0e4ba5ec56285bbee12;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;11;-1529.005,845.745;Inherit;True;Property;_DallesN;Dalles-N;3;0;Create;True;0;0;False;0;False;-1;eac7860cc437fb146966a6989ecf3a19;eac7860cc437fb146966a6989ecf3a19;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;12;-1520.33,644.2851;Inherit;True;Property;_RockTextureN;Rock-TextureN;4;0;Create;True;0;0;False;0;False;-1;20397de662f772540bbdbca877ae569b;20397de662f772540bbdbca877ae569b;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;3;-1040.185,3.445304;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;14;-355.9232,-193.9107;Inherit;False;Constant;_Color0;Color 0;5;0;Create;True;0;0;False;0;False;0.6698113,0.6413759,0.6413759,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;18;-1531.552,1292.623;Inherit;True;Property;_DallesH;Dalles-H;6;0;Create;True;0;0;False;0;False;-1;d66126d20fcfa1c4da81105bccd5c574;d66126d20fcfa1c4da81105bccd5c574;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;17;-1521.297,1079.329;Inherit;True;Property;_RockTextureH;Rock-TextureH;5;0;Create;True;0;0;False;0;False;-1;56d823ccf70574e4e8b0b5e1287cb007;56d823ccf70574e4e8b0b5e1287cb007;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;13;-1004.375,635.129;Inherit;True;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;19;-979.6268,1111.935;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;38.87871,-36.5497;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;324.8997,26.6;Float;False;True;-1;6;ASEMaterialInspector;0;0;Standard;GroundBlend;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;16.1;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;8;1;10;0
WireConnection;7;1;10;0
WireConnection;11;1;10;0
WireConnection;12;1;10;0
WireConnection;3;0;7;0
WireConnection;3;1;8;0
WireConnection;3;2;9;1
WireConnection;18;1;10;0
WireConnection;17;1;10;0
WireConnection;13;0;12;0
WireConnection;13;1;11;0
WireConnection;13;2;9;1
WireConnection;19;0;17;0
WireConnection;19;1;18;0
WireConnection;19;2;9;1
WireConnection;21;0;14;0
WireConnection;21;1;3;0
WireConnection;0;0;21;0
WireConnection;0;1;13;0
ASEEND*/
//CHKSM=67AE0C9F70401EE773796688565962A1A59B5E4A