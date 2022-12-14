// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Hiota_Slash"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.25
		_Texture1("Texture 1", 2D) = "white" {}
		[HighDynamicRange]_HighDynamicRange("High Dynamic Range", Range( 1 , 4)) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] _tex4coord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "AlphaTest+0" "IsEmissive" = "true"  }
		Cull Off
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Unlit keepalpha addshadow fullforwardshadows 
		#undef TRANSFORM_TEX
		#define TRANSFORM_TEX(tex,name) float4(tex.xy * name##_ST.xy + name##_ST.zw, tex.z, tex.w)
		struct Input
		{
			float2 uv_texcoord;
			float4 uv_tex4coord;
			float4 vertexColor : COLOR;
		};

		uniform float _HighDynamicRange;
		uniform sampler2D _Texture1;
		uniform float _Cutoff = 0.25;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float4 break2 = i.uv_tex4coord;
			float2 appendResult3 = (float2(break2.x , break2.z));
			float4 tex2DNode7 = tex2D( _Texture1, (i.uv_texcoord*appendResult3 + 0.0) );
			o.Emission = ( ( _HighDynamicRange * tex2DNode7 ) * i.vertexColor ).rgb;
			o.Alpha = 1;
			clip( ( i.vertexColor.a * tex2DNode7.a ) - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18703
2007;136;1318;477;2337.419;396.6115;1;True;True
Node;AmplifyShaderEditor.TexCoordVertexDataNode;1;-2343.1,244.1465;Inherit;True;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BreakToComponentsNode;2;-2007.701,262.3464;Inherit;False;FLOAT4;1;0;FLOAT4;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.DynamicAppendNode;3;-1823.915,347.7828;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;4;-2099.382,-106.604;Inherit;True;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;5;-1822.558,-240.9771;Inherit;True;Property;_Texture1;Texture 1;1;0;Create;True;0;0;False;0;False;4167678f01c7abf47ac183532d823345;4167678f01c7abf47ac183532d823345;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.ScaleAndOffsetNode;6;-1609.391,156.479;Inherit;True;3;0;FLOAT2;1,0;False;1;FLOAT2;1,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.VertexColorNode;9;-815.7431,29.36914;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;8;-1048.665,-373.1725;Inherit;False;Property;_HighDynamicRange;High Dynamic Range;2;0;Create;True;0;0;False;1;HighDynamicRange;False;1;1;1;4;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;7;-1404.65,-232.2107;Inherit;True;Property;_Coupe_FX_1001_BaseMap_ACESACEScg;Coupe_FX_1001_BaseMap_ACES - ACEScg;2;0;Create;True;0;0;False;0;False;-1;faad2c967939e544babd3a55b333df15;faad2c967939e544babd3a55b333df15;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;-774.9259,-174.7285;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;12;-842.1439,267.2974;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;10;-491.7556,123.2328;Inherit;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;-308.8011,252.0551;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-274.6586,-32.34084;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;13;-415.7333,420.8267;Inherit;False;Property;_AlphaSlashModifier;AlphaSlashModifier;3;0;Create;True;0;0;False;0;False;0.5;0.293;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;16;-2022.694,114.7622;Inherit;False;Property;_Vector0;Vector 0;4;0;Create;True;0;0;False;0;False;0,0;1,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-15.6,5.2;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;Hiota_Slash;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.25;True;True;0;True;Opaque;;AlphaTest;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;2;0;1;0
WireConnection;3;0;2;0
WireConnection;3;1;2;2
WireConnection;6;0;4;0
WireConnection;6;1;3;0
WireConnection;7;0;5;0
WireConnection;7;1;6;0
WireConnection;11;0;8;0
WireConnection;11;1;7;0
WireConnection;12;0;7;4
WireConnection;10;0;9;0
WireConnection;14;0;10;3
WireConnection;14;1;12;0
WireConnection;15;0;11;0
WireConnection;15;1;9;0
WireConnection;0;2;15;0
WireConnection;0;10;14;0
ASEEND*/
//CHKSM=F7E0C9075725487576FFC2842DDFD0C106A6DF8B