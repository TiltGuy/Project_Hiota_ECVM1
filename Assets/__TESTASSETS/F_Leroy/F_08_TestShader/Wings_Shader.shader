// Made with Amplify Shader Editor v1.9.1.3
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Wings_Shader"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.45
		[HighDynamicRange]_HighDynamicRange("High Dynamic Range", Range( 1 , 4)) = 1
		_Texture("Texture", 2D) = "white" {}
		_ColorPearls("ColorPearls", Color) = (0,0,0,0)
		_Emissive("Emissive", Range( 1 , 10)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Transparent+0" "IsEmissive" = "true"  }
		Cull Off
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Unlit keepalpha 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float _HighDynamicRange;
		uniform sampler2D _Texture;
		uniform float4 _Texture_ST;
		uniform float4 _ColorPearls;
		uniform float _Emissive;
		uniform float _Cutoff = 0.45;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv_Texture = i.uv_texcoord * _Texture_ST.xy + _Texture_ST.zw;
			float4 tex2DNode14 = tex2D( _Texture, uv_Texture );
			float4 appendResult27 = (float4(tex2DNode14.r , tex2DNode14.g , tex2DNode14.b , 0.0));
			o.Emission = ( ( ( _HighDynamicRange * appendResult27 ) * _ColorPearls ) * _Emissive ).xyz;
			o.Alpha = 1;
			clip( ( _ColorPearls.a * tex2DNode14.a ) - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=19103
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;1;-859.7523,-275.5142;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;-359.4849,-133.1263;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ColorNode;12;-778.8742,-165.1544;Inherit;False;Property;_ColorPearls;ColorPearls;3;0;Create;True;0;0;0;False;0;False;0,0,0,0;0.3286244,1,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;23;-1138.191,-383.958;Inherit;False;Property;_HighDynamicRange;High Dynamic Range;1;0;Create;True;0;0;0;False;1;HighDynamicRange;False;1;1;1;4;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;14;-1477.786,-95.44663;Inherit;True;Property;_Texture;Texture;2;0;Create;True;0;0;0;False;0;False;-1;faad2c967939e544babd3a55b333df15;f2e0dbc7724247a478aad173d897eed1;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;-399.1195,207.7644;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;-178.8931,-18.83459;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;Wings_Shader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;;0;False;;False;0;False;;0;False;;False;0;Custom;0.45;True;False;0;True;TransparentCutout;;Transparent;All;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;True;0;0;False;;0;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;0;-1;-1;-1;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.RangedFloatNode;25;-468.8931,-2.834595;Inherit;False;Property;_Emissive;Emissive;5;0;Create;True;0;0;0;False;0;False;0;10;1;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;13;-363.3899,124.2888;Inherit;False;Property;_Opacity;Opacity;4;0;Create;True;0;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;15;-854.5694,21.86779;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BreakToComponentsNode;2;-643.2184,40.66003;Inherit;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.DynamicAppendNode;27;-1100.889,-128.4146;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
WireConnection;1;0;23;0
WireConnection;1;1;27;0
WireConnection;4;0;1;0
WireConnection;4;1;12;0
WireConnection;24;0;12;4
WireConnection;24;1;14;4
WireConnection;26;0;4;0
WireConnection;26;1;25;0
WireConnection;0;2;26;0
WireConnection;0;10;24;0
WireConnection;2;0;15;0
WireConnection;27;0;14;1
WireConnection;27;1;14;2
WireConnection;27;2;14;3
ASEEND*/
//CHKSM=B495A0D53FDEF808F4393BE78D684EF8021B6EE2