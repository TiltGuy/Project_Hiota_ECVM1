// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Platforms"
{
	Properties
	{
		_Canvas_N("Canvas_N", 2D) = "white" {}
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

		uniform sampler2D _Canvas_N;
		uniform float4 _Canvas_N_ST;

		float4 tessFunction( appdata_full v0, appdata_full v1, appdata_full v2 )
		{
			return ( (-1) * 0.0 );
		}

		void vertexDataFunc( inout appdata_full v )
		{
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Canvas_N = i.uv_texcoord * _Canvas_N_ST.xy + _Canvas_N_ST.zw;
			o.Normal = ( tex2D( _Canvas_N, uv_Canvas_N ) * 0.4588235 ).rgb;
			float4 color18 = IsGammaSpace() ? float4(0.5206479,0.9433962,0.9433962,0) : float4(0.2336527,0.8760344,0.8760344,0);
			o.Albedo = color18.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18703
-1920;2;1920;1019;874.2036;252.3497;1;True;True
Node;AmplifyShaderEditor.SamplerNode;22;-347.1707,-5.318604;Inherit;True;Property;_Canvas_N;Canvas_N;0;0;Create;True;0;0;False;0;False;-1;7ad0e904548935f4980cafbfebf050ab;7ad0e904548935f4980cafbfebf050ab;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;16;-383.6597,204.4863;Inherit;False;Constant;_Float0;Float 0;8;0;Create;True;0;0;False;0;False;0.4588235;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;24;-304.1707,560.6814;Inherit;False;Constant;_Float1;Float 1;2;0;Create;True;0;0;False;0;False;0;0;0;60;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;9;-414.9368,339.4059;Inherit;True;Property;_CanvasText;CanvasText;1;0;Create;True;0;0;False;0;False;-1;fb3ad497f8a03f943abf387df36827f3;fb3ad497f8a03f943abf387df36827f3;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;18;-104.1023,-197.1263;Inherit;False;Constant;_Color0;Color 0;2;0;Create;True;0;0;False;0;False;0.5206479,0.9433962,0.9433962,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;23;20.82935,116.6814;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;10.82935,479.6814;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;5;265.2,6.500004;Float;False;True;-1;6;ASEMaterialInspector;0;0;Standard;Platforms;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;2;15;10;25;False;0.75;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;23;0;22;0
WireConnection;23;1;16;0
WireConnection;25;0;9;0
WireConnection;25;1;24;0
WireConnection;5;0;18;0
WireConnection;5;1;23;0
WireConnection;5;14;25;0
ASEEND*/
//CHKSM=F1BA9E3BC069B9431515B0A12CA5F3161EEF5E51