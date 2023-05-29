// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Glowing_Body"
{
	Properties
	{
		_Metallic("Metallic", Range( 0 , 1)) = 0
		_Smoothness("Smoothness", Range( 0 , 1)) = 0
		_Base_Color("Base_Color", Color) = (0,0,0,0)
		_Top_Color("Top_Color", Color) = (0,0,0,0)
		_BottomLine("BottomLine", Range( 0 , 1)) = 0
		_TopLine("TopLine", Range( 0 , 1)) = 0
		_Bottom_Color("Bottom_Color", Color) = (0,0,0,0)
		_Scale_Fresnel("Scale_Fresnel", Float) = 1
		_Power_Fresnel("Power_Fresnel", Float) = 0
		_Bias_Fresnel("Bias_Fresnel", Float) = 0
		[HideInInspector] _tex4coord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#undef TRANSFORM_TEX
		#define TRANSFORM_TEX(tex,name) float4(tex.xy * name##_ST.xy + name##_ST.zw, tex.z, tex.w)
		struct Input
		{
			float4 uv_tex4coord;
			float3 worldPos;
			float3 worldNormal;
		};

		uniform float4 _Base_Color;
		uniform float _TopLine;
		uniform float4 _Top_Color;
		uniform float _Bias_Fresnel;
		uniform float _Scale_Fresnel;
		uniform float _Power_Fresnel;
		uniform float _BottomLine;
		uniform float4 _Bottom_Color;
		uniform float _Metallic;
		uniform float _Smoothness;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Albedo = _Base_Color.rgb;
			float temp_output_46_0 = ( i.uv_tex4coord.y * 0.88 );
			float smoothstepResult48 = smoothstep( _TopLine , 1.0 , temp_output_46_0);
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV41 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode41 = ( _Bias_Fresnel + _Scale_Fresnel * pow( 1.0 - fresnelNdotV41, _Power_Fresnel ) );
			float smoothstepResult55 = smoothstep( _BottomLine , 1.0 , ( 1.0 - temp_output_46_0 ));
			o.Emission = ( ( smoothstepResult48 * ( _Top_Color * fresnelNode41 ) ) + ( smoothstepResult55 * ( fresnelNode41 * _Bottom_Color ) ) ).rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Smoothness;
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows exclude_path:deferred 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
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
				float4 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				float3 worldNormal : TEXCOORD3;
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
				o.worldNormal = worldNormal;
				o.customPack1.xyzw = customInputData.uv_tex4coord;
				o.customPack1.xyzw = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
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
				surfIN.uv_tex4coord = IN.customPack1.xyzw;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = IN.worldNormal;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
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
Version=18703
324;95;1318;471;-22.55985;52.79777;1.839453;True;True
Node;AmplifyShaderEditor.TexCoordVertexDataNode;44;-888.8273,-248.943;Inherit;True;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BreakToComponentsNode;45;-542.4893,-245.6867;Inherit;False;FLOAT4;1;0;FLOAT4;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.RangedFloatNode;47;-550.8822,-65.14833;Inherit;False;Constant;_Float0;Float 0;7;0;Create;True;0;0;False;0;False;0.88;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;65;-666.4259,531.6514;Inherit;False;Property;_Power_Fresnel;Power_Fresnel;10;0;Create;True;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;64;-667.7982,466.8908;Inherit;False;Property;_Scale_Fresnel;Scale_Fresnel;9;0;Create;True;0;0;False;0;False;1;3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;66;-629.0807,386.3188;Inherit;False;Property;_Bias_Fresnel;Bias_Fresnel;11;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;46;-305.8317,-137.229;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;54;-87.52341,-72.57692;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;41;-393.4968,407.0194;Inherit;False;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;1;False;2;FLOAT;1;False;3;FLOAT;2.29;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;56;-373.8472,6.720528;Inherit;False;Property;_BottomLine;BottomLine;6;0;Create;True;0;0;False;0;False;0;0.2;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;59;6.095682,423.7624;Inherit;False;Property;_Bottom_Color;Bottom_Color;8;0;Create;True;0;0;False;0;False;0,0,0,0;0,0.3187318,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;42;-159.0227,-512.3511;Inherit;False;Property;_Top_Color;Top_Color;5;0;Create;True;0;0;False;0;False;0,0,0,0;1,0.9657439,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;57;-281.5871,-806.4127;Inherit;False;Property;_TopLine;TopLine;7;0;Create;True;0;0;False;0;False;0;0.2;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;55;148.3955,-67.89015;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0.52;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;43;155.5378,-433.1162;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;61;240.636,220.8611;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SmoothstepOpNode;48;127.649,-802.7507;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0.37;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;62;489.3204,24.45465;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;58;472.9797,-567.0654;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;39;952.2891,337.9336;Inherit;False;Property;_Smoothness;Smoothness;3;0;Create;True;0;0;False;0;False;0;0.514;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;40;1141.547,-300.5863;Inherit;False;Property;_Base_Color;Base_Color;4;0;Create;True;0;0;False;0;False;0,0,0,0;1,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;63;853.5321,-300.1192;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;38;946.3129,259.7666;Inherit;False;Property;_Metallic;Metallic;2;0;Create;True;0;0;False;0;False;0;0.53;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;30;1386.071,136.1234;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Glowing_Body;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;ForwardOnly;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;5;False;-1;10;False;-1;0;5;False;-1;10;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;45;0;44;0
WireConnection;46;0;45;1
WireConnection;46;1;47;0
WireConnection;54;0;46;0
WireConnection;41;1;66;0
WireConnection;41;2;64;0
WireConnection;41;3;65;0
WireConnection;55;0;54;0
WireConnection;55;1;56;0
WireConnection;43;0;42;0
WireConnection;43;1;41;0
WireConnection;61;0;41;0
WireConnection;61;1;59;0
WireConnection;48;0;46;0
WireConnection;48;1;57;0
WireConnection;62;0;55;0
WireConnection;62;1;61;0
WireConnection;58;0;48;0
WireConnection;58;1;43;0
WireConnection;63;0;58;0
WireConnection;63;1;62;0
WireConnection;30;0;40;0
WireConnection;30;2;63;0
WireConnection;30;3;38;0
WireConnection;30;4;39;0
ASEEND*/
//CHKSM=6886C5675765D0B0CA0BDB9C358647E7945D3C65