// Made with Amplify Shader Editor v1.9.1.3
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Grass"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_Grass("Grass", 2D) = "white" {}
		_Amplitude("Amplitude", Float) = 1
		_Speed("Speed", Float) = 1
		_Float1("Float 1", Float) = 0.1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Geometry+0" "IgnoreProjector" = "True" }
		Cull Off
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			float3 worldPos;
			float2 uv_texcoord;
		};

		uniform float _Float1;
		uniform float _Speed;
		uniform float _Amplitude;
		uniform sampler2D _Grass;
		uniform float4 _Grass_ST;
		uniform float _Cutoff = 0.5;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float2 appendResult34 = (float2(ase_worldPos.x , ase_worldPos.y));
			float2 panner38 = ( 1.0 * _Time.y * float2( 0,0 ) + ( appendResult34 * _Float1 ));
			float3 ase_vertex3Pos = v.vertex.xyz;
			float4 appendResult15 = (float4(0.0 , ( panner38 + ( v.texcoord.xy.y * sin( ( ( ase_vertex3Pos.x + 0.0 + ( _Time.y * _Speed ) ) / 0.1324087 ) ) ) ) , 0.0));
			v.vertex.xyz += ( appendResult15 * _Amplitude ).xyz;
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Grass = i.uv_texcoord * _Grass_ST.xy + _Grass_ST.zw;
			float4 tex2DNode1 = tex2D( _Grass, uv_Grass );
			o.Albedo = tex2DNode1.rgb;
			o.Alpha = 1;
			clip( tex2DNode1.a - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=19103
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;321.7583,-39.77074;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Grass;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;;0;False;;False;0;False;;0;False;;False;0;Custom;0.5;True;True;0;True;TransparentCutout;;Geometry;All;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;True;2;5;False;;10;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;0;-1;-1;-1;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.SimpleAddOpNode;3;-1117.141,669.292;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;-1258.186,788.2985;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;6;-1495.097,769.5662;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;8;-945.2426,688.0248;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-1105.021,876.4516;Inherit;False;Constant;_Float0;Float 0;2;0;Create;True;0;0;0;False;0;False;0.1324087;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-1497.3,863.2287;Inherit;False;Property;_Speed;Speed;3;0;Create;True;0;0;0;False;0;False;1;0.3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-135.9233,-279.744;Inherit;True;Property;_Grass;Grass;1;0;Create;True;0;0;0;False;0;False;-1;89b5f192b18e1a84bbf7458a138d028c;8ea973a08f4f3344c812ca7bff8051b8;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PosVertexDataNode;2;-1359.562,606.483;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SinOpNode;11;-798.3103,747.0414;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;15;-394.6322,491.5406;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;16;-463.222,722.7261;Inherit;False;Property;_Amplitude;Amplitude;2;0;Create;True;0;0;0;False;0;False;1;0.4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;1.662541,459.1935;Inherit;True;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;13;-1010.593,451.4189;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;-690.1978,544.6319;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;39;-543.1544,343.9184;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;35;-1193.462,-103.9213;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;38;-871.7648,11.32666;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;34;-1480.309,-121.962;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.WorldPosInputsNode;33;-1691.386,-132.7865;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;36;-1432.518,21.99215;Inherit;False;Property;_Float1;Float 1;4;0;Create;True;0;0;0;False;0;False;0.1;-1.98;0;0;0;1;FLOAT;0
WireConnection;0;0;1;0
WireConnection;0;10;1;4
WireConnection;0;11;31;0
WireConnection;3;0;2;1
WireConnection;3;2;4;0
WireConnection;4;0;6;0
WireConnection;4;1;7;0
WireConnection;8;0;3;0
WireConnection;8;1;9;0
WireConnection;11;0;8;0
WireConnection;15;1;39;0
WireConnection;31;0;15;0
WireConnection;31;1;16;0
WireConnection;14;0;13;2
WireConnection;14;1;11;0
WireConnection;39;0;38;0
WireConnection;39;1;14;0
WireConnection;35;0;34;0
WireConnection;35;1;36;0
WireConnection;38;0;35;0
WireConnection;34;0;33;1
WireConnection;34;1;33;2
ASEEND*/
//CHKSM=62D635D23B1C2C7BE2AB7029ABCBCAD9A16A9897