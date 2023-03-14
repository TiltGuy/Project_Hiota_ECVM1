// Made with Amplify Shader Editor v1.9.1.3
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Shield_Effect"
{
	Properties
	{
		_Shield_Slider("Shield_Slider", Range( 0 , 1)) = 1
		_Cutoff( "Mask Clip Value", Float ) = 0.15
		[HighDynamicRange]_HighDynamicRange("High Dynamic Range", Range( 1 , 4)) = 1
		_Texture("Texture", 2D) = "white" {}
		_Speed("Speed", Float) = 0
		_ColorPearls("ColorPearls", Color) = (0,0,0,0)
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_Shine_Of_Aura("Shine_Of_Aura", Float) = 2.55
		_Size_Of_Aura("Size_Of_Aura", Range( 0 , 2)) = 0
		_Shield_Min("Shield_Min", Float) = 0
		_Shield_Max("Shield_Max", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Reverse"  "Queue" = "AlphaTest+0" "IsEmissive" = "true"  }
		Cull Off
		AlphaToMask On
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
		};

		uniform float _HighDynamicRange;
		uniform sampler2D _Texture;
		uniform float _Speed;
		uniform float _Shine_Of_Aura;
		uniform sampler2D _TextureSample0;
		uniform float4 _ColorPearls;
		uniform float _Shield_Min;
		uniform float _Shield_Max;
		uniform float _Shield_Slider;
		uniform float _Size_Of_Aura;
		uniform float _Cutoff = 0.15;

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
			float4 tex2DNode17 = tex2D( _Texture, rotator1 );
			float4 tex2DNode49 = tex2D( _TextureSample0, rotator1 );
			o.Emission = ( ( _HighDynamicRange * ( tex2DNode17 + ( _Shine_Of_Aura * tex2DNode49 ) ) ) * _ColorPearls ).rgb;
			o.Alpha = 1;
			float lerpResult74 = lerp( _Shield_Min , _Shield_Max , _Shield_Slider);
			clip( ( ( ( i.vertexColor.a * ( tex2DNode17.a + ( tex2DNode49.a * lerpResult74 ) ) ) * lerpResult74 ) + ( tex2DNode49.a * _Size_Of_Aura ) ) - _Cutoff );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Unlit keepalpha fullforwardshadows noshadow 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			AlphaToMask Off
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				half4 color : COLOR0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				o.color = v.color;
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.vertexColor = IN.color;
				SurfaceOutput o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutput, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=19103
Node;AmplifyShaderEditor.BreakToComponentsNode;20;-1010.29,531.1648;Inherit;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;-793.1937,375.591;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleTimeNode;7;-2257.227,609.631;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RotatorNode;1;-2047.771,410.1766;Inherit;True;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;36;-1808.722,270.0338;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;41;-2157.789,43.55888;Inherit;True;Property;_Texture0;Texture 0;3;0;Create;True;0;0;0;False;0;False;None;faad2c967939e544babd3a55b333df15;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.Vector2Node;43;-2296.363,453.7375;Inherit;False;Constant;_Vector0;Vector 0;4;0;Create;True;0;0;0;False;0;False;0.5,0.5;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;44;-2432.863,300.3376;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;45;-2421.581,602.5545;Inherit;False;Property;_Speed;Speed;5;0;Create;True;0;0;0;False;0;False;0;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;17;-1744.347,407.08;Inherit;True;Property;_Texture;Texture;4;0;Create;True;0;0;0;False;0;False;-1;faad2c967939e544babd3a55b333df15;194244f8eb1165a4494a0200f25f86f1;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;52;-1292.105,738.804;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;16;-1571.9,124.7593;Inherit;False;Property;_HighDynamicRange;High Dynamic Range;2;0;Create;True;0;0;0;False;1;HighDynamicRange;False;1;1.45;1;4;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-348.7399,307.0103;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;Shield_Effect;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;;0;False;;False;0;False;;0;False;;False;0;Custom;0.15;True;True;0;True;Custom;Reverse;AlphaTest;All;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;5;False;0;0;False;;0;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;1;-1;-1;-1;0;True;0;0;False;;-1;0;False;_Mask_Clip;0;0;0;False;1;False;;0;False;;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;55;-1435.837,789.7865;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;53;-2056.157,683.386;Inherit;False;Property;_Shine_Of_Aura;Shine_Of_Aura;8;0;Create;True;0;0;0;False;0;False;2.55;3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;54;-1555.79,675.7476;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;49;-1869.803,769.6906;Inherit;True;Property;_TextureSample0;Texture Sample 0;7;0;Create;True;0;0;0;False;0;False;-1;None;d6c00c5481c7d604da3a11ae3affc0e6;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;73;-833.8459,929.934;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-846.6778,637.67;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;70;-625.2695,748.0777;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;71;-482.489,796.7557;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;-1203.76,228.0031;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;51;-1403.087,463.6362;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;46;-1155.382,350.0628;Inherit;False;Property;_ColorPearls;ColorPearls;6;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0.827451,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;15;-1176.455,535.3787;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;56;-1164.346,1057.35;Inherit;False;Property;_Size_Of_Aura;Size_Of_Aura;9;0;Create;True;0;0;0;False;0;False;0;0.5;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;66;-1910.15,1117.889;Float;False;Property;_Shield_Slider;Shield_Slider;0;0;Create;True;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;74;-1506.182,975.8524;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;75;-1857.375,978.6107;Inherit;False;Property;_Shield_Min;Shield_Min;10;0;Create;True;0;0;0;False;0;False;0;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;76;-1845.675,1044.911;Inherit;False;Property;_Shield_Max;Shield_Max;11;0;Create;True;0;0;0;False;0;False;0;0.75;0;0;0;1;FLOAT;0
WireConnection;20;0;15;0
WireConnection;22;0;18;0
WireConnection;22;1;46;0
WireConnection;7;0;45;0
WireConnection;1;0;44;0
WireConnection;1;1;43;0
WireConnection;1;2;7;0
WireConnection;36;2;41;0
WireConnection;17;1;1;0
WireConnection;52;0;17;4
WireConnection;52;1;55;0
WireConnection;0;2;22;0
WireConnection;0;10;71;0
WireConnection;55;0;49;4
WireConnection;55;1;74;0
WireConnection;54;0;53;0
WireConnection;54;1;49;0
WireConnection;49;1;1;0
WireConnection;73;0;49;4
WireConnection;73;1;56;0
WireConnection;21;0;20;3
WireConnection;21;1;52;0
WireConnection;70;0;21;0
WireConnection;70;1;74;0
WireConnection;71;0;70;0
WireConnection;71;1;73;0
WireConnection;18;0;16;0
WireConnection;18;1;51;0
WireConnection;51;0;17;0
WireConnection;51;1;54;0
WireConnection;74;0;75;0
WireConnection;74;1;76;0
WireConnection;74;2;66;0
ASEEND*/
//CHKSM=43CB3637B31329DCF59E4B5412821E7EE1174FD9