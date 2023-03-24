// Made with Amplify Shader Editor v1.9.1.3
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Shield_Effect"
{
	Properties
	{
		_Shield_Slider("Shield_Slider", Range( 0 , 1)) = 1
		_Opacity_Test("Opacity_Test", Range( 0 , 5)) = 1
		_Shield_Max("Shield_Max", Float) = 0
		_Shield_Min("Shield_Min", Float) = 0
		_Cutoff( "Mask Clip Value", Float ) = 0.18
		[HighDynamicRange]_HighDynamicRange("High Dynamic Range", Range( 1 , 4)) = 1
		_Pearls_Texture("Pearls_Texture", 2D) = "white" {}
		_ColorPearls("ColorPearls", Color) = (1,0,0,0)
		_Aura_Texture("Aura_Texture", 2D) = "white" {}
		_Speed("Speed", Float) = 0
		_Shine_Of_Aura("Shine_Of_Aura", Float) = 2.55
		_Size_Of_Aura("Size_Of_Aura", Range( 0 , 2)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "AlphaTest+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Unlit keepalpha noshadow 
		struct Input
		{
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
		};

		uniform float _HighDynamicRange;
		uniform sampler2D _Pearls_Texture;
		uniform float _Speed;
		uniform float _Shine_Of_Aura;
		uniform sampler2D _Aura_Texture;
		uniform float4 _ColorPearls;
		uniform float _Opacity_Test;
		uniform float _Shield_Slider;
		uniform float _Shield_Min;
		uniform float _Shield_Max;
		uniform float _Size_Of_Aura;
		uniform float _Cutoff = 0.18;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float mulTime7 = _Time.y * _Speed;
			float cos1 = cos( mulTime7 );
			float sin1 = sin( mulTime7 );
			float2 rotator1 = mul( i.uv_texcoord - float2( 0.5,0.5 ) , float2x2( cos1 , -sin1 , sin1 , cos1 )) + float2( 0.5,0.5 );
			float4 tex2DNode17 = tex2D( _Pearls_Texture, rotator1 );
			float4 tex2DNode49 = tex2D( _Aura_Texture, rotator1 );
			o.Emission = ( ( _HighDynamicRange * ( tex2DNode17 + ( _Shine_Of_Aura * tex2DNode49 ) ) ) * _ColorPearls ).rgb;
			float temp_output_94_0 = ( _Opacity_Test * _Shield_Slider );
			float lerpResult74 = lerp( _Shield_Min , _Shield_Max , _Shield_Slider);
			float temp_output_71_0 = ( ( ( i.vertexColor.a * ( tex2DNode17.a + ( tex2DNode49.a * lerpResult74 ) ) ) * lerpResult74 ) + ( tex2DNode49.a * _Size_Of_Aura ) );
			o.Alpha = ( temp_output_94_0 * temp_output_71_0 );
			clip( temp_output_71_0 - _Cutoff );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=19103
Node;AmplifyShaderEditor.RangedFloatNode;16;-1571.9,124.7593;Inherit;False;Property;_HighDynamicRange;High Dynamic Range;5;0;Create;True;0;0;0;False;1;HighDynamicRange;False;1;1.4;1;4;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;54;-1555.79,675.7476;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;-1203.76,228.0031;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;51;-1403.087,463.6362;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleTimeNode;7;-2547.128,618.731;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RotatorNode;1;-2337.672,419.2766;Inherit;True;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;43;-2586.264,462.8375;Inherit;False;Constant;_Vector0;Vector 0;4;0;Create;True;0;0;0;False;0;False;0.5,0.5;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;44;-2722.764,309.4376;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;45;-2711.482,611.6545;Inherit;False;Property;_Speed;Speed;9;0;Create;True;0;0;0;False;0;False;0;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;70;-272.0221,822.4458;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;73;-670.7305,1095.546;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;56;-1001.23,1222.962;Inherit;False;Property;_Size_Of_Aura;Size_Of_Aura;11;0;Create;True;0;0;0;False;0;False;0;0.2;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;15;-1171.178,530.5273;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;52;-951.8721,785.9276;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;20;-946.6395,538.7331;Inherit;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.RangedFloatNode;66;-1800.472,1223.797;Float;False;Property;_Shield_Slider;Shield_Slider;0;0;Create;True;0;0;0;False;0;False;1;0.5767176;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;75;-1747.698,1084.519;Inherit;False;Property;_Shield_Min;Shield_Min;3;0;Create;True;0;0;0;False;0;False;0;0.15;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;76;-1735.998,1150.819;Inherit;False;Property;_Shield_Max;Shield_Max;2;0;Create;True;0;0;0;False;0;False;0;0.75;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-679.3671,689.9632;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;53;-2012.058,644.486;Inherit;False;Property;_Shine_Of_Aura;Shine_Of_Aura;10;0;Create;True;0;0;0;False;0;False;2.55;3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;74;-1509.505,1108.96;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;85;-1269.831,829.0062;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;402.3042,379.8441;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;Shield_Effect;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;;0;False;;False;0;False;;0;False;;False;0;Custom;0.18;True;False;0;True;Transparent;Reverse;AlphaTest;All;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;5;False;2;5;False;;10;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;4;-1;-1;-1;0;False;0;0;False;;-1;0;False;_Mask_Clip;0;0;0;False;1;False;;0;False;;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;-342.1816,292.863;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;46;-1037.382,347.1429;Inherit;False;Property;_ColorPearls;ColorPearls;7;0;Create;True;0;0;0;False;0;False;1,0,0,0;0,0.827451,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;92;155.4247,581.358;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;49;-1982.546,730.2158;Inherit;True;Property;_Aura_Texture;Aura_Texture;8;0;Create;True;0;0;0;False;0;False;-1;d6c00c5481c7d604da3a11ae3affc0e6;d6c00c5481c7d604da3a11ae3affc0e6;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;17;-1816.857,467.7032;Inherit;True;Property;_Pearls_Texture;Pearls_Texture;6;0;Create;True;0;0;0;False;0;False;-1;194244f8eb1165a4494a0200f25f86f1;194244f8eb1165a4494a0200f25f86f1;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;93;-389.9119,559.9089;Inherit;False;Property;_Opacity_Test;Opacity_Test;1;0;Create;True;0;0;0;False;0;False;1;0;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;94;-44.36145,562.766;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;71;-28.79024,884.2032;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;95;121.6401,458.7089;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
WireConnection;54;0;53;0
WireConnection;54;1;49;0
WireConnection;18;0;16;0
WireConnection;18;1;51;0
WireConnection;51;0;17;0
WireConnection;51;1;54;0
WireConnection;7;0;45;0
WireConnection;1;0;44;0
WireConnection;1;1;43;0
WireConnection;1;2;7;0
WireConnection;70;0;21;0
WireConnection;70;1;74;0
WireConnection;73;0;49;4
WireConnection;73;1;56;0
WireConnection;52;0;17;4
WireConnection;52;1;85;0
WireConnection;20;0;15;0
WireConnection;21;0;20;3
WireConnection;21;1;52;0
WireConnection;74;0;75;0
WireConnection;74;1;76;0
WireConnection;74;2;66;0
WireConnection;85;0;49;4
WireConnection;85;1;74;0
WireConnection;0;2;22;0
WireConnection;0;9;92;0
WireConnection;0;10;71;0
WireConnection;22;0;18;0
WireConnection;22;1;46;0
WireConnection;92;0;94;0
WireConnection;92;1;71;0
WireConnection;49;1;1;0
WireConnection;17;1;1;0
WireConnection;94;0;93;0
WireConnection;94;1;66;0
WireConnection;71;0;70;0
WireConnection;71;1;73;0
WireConnection;95;1;94;0
ASEEND*/
//CHKSM=81B93E3DF5E278E058F30513E0B692C454D188B3