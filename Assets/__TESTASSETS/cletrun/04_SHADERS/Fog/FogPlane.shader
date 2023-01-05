// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "FogPlane"
{
	Properties
	{
		_EdgeLength ( "Edge length", Range( 2, 50 ) ) = 15.7
		_DepthFade("Depth Fade", Range( 0 , 200)) = 0
		_Color("Color", Color) = (1,0,0.8365736,0)
		_HeightIntensity("Height Intensity", Range( 0 , 1)) = 0
		_Texture0("Texture 0", 2D) = "white" {}
		_Disto("Disto", Range( 0 , 1)) = 0
		_Speed("Speed", Vector) = (0.05,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#include "Tessellation.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 4.6
		struct Input
		{
			float2 uv_texcoord;
			float4 screenPos;
		};

		uniform sampler2D _Texture0;
		uniform float2 _Speed;
		uniform float4 _Texture0_ST;
		uniform float _Disto;
		uniform float _HeightIntensity;
		uniform float4 _Color;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _DepthFade;
		uniform float _EdgeLength;

		float4 tessFunction( appdata_full v0, appdata_full v1, appdata_full v2 )
		{
			return UnityEdgeLengthBasedTess (v0.vertex, v1.vertex, v2.vertex, _EdgeLength);
		}

		void vertexDataFunc( inout appdata_full v )
		{
			float2 panner3 = ( 1.0 * _Time.y * _Speed + v.texcoord.xy);
			float2 uv_Texture0 = v.texcoord * _Texture0_ST.xy + _Texture0_ST.zw;
			float2 lerpResult39 = lerp( panner3 , ( panner3 * tex2Dlod( _Texture0, float4( uv_Texture0, 0, 0.0) ).r ) , (0.0 + (_Disto - 0.0) * (0.04 - 0.0) / (1.0 - 0.0)));
			float4 tex2DNode1 = tex2Dlod( _Texture0, float4( lerpResult39, 0, 0.0) );
			float3 ase_worldNormal = UnityObjectToWorldNormal( v.normal );
			v.vertex.xyz += ( tex2DNode1.r * ase_worldNormal * _HeightIntensity );
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 panner3 = ( 1.0 * _Time.y * _Speed + i.uv_texcoord);
			float2 uv_Texture0 = i.uv_texcoord * _Texture0_ST.xy + _Texture0_ST.zw;
			float2 lerpResult39 = lerp( panner3 , ( panner3 * tex2D( _Texture0, uv_Texture0 ).r ) , (0.0 + (_Disto - 0.0) * (0.04 - 0.0) / (1.0 - 0.0)));
			float4 tex2DNode1 = tex2D( _Texture0, lerpResult39 );
			o.Albedo = ( tex2DNode1 * _Color ).rgb;
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth16 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float distanceDepth16 = abs( ( screenDepth16 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _DepthFade ) );
			float3 temp_cast_1 = (distanceDepth16).xxx;
			o.Emission = temp_cast_1;
			float clampResult19 = clamp( distanceDepth16 , 0.0 , 1.0 );
			o.Alpha = clampResult19;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows vertex:vertexDataFunc tessellate:tessFunction 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 4.6
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
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				float4 screenPos : TEXCOORD3;
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
				vertexDataFunc( v );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				o.screenPos = ComputeScreenPos( o.pos );
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
				surfIN.screenPos = IN.screenPos;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
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
-1860;175;1463;754;517.7285;782.3026;1.952401;True;False
Node;AmplifyShaderEditor.CommentaryNode;13;-987.7502,-1289.028;Inherit;False;1646.808;738.0056;Cloud is moving;14;35;1;46;39;40;44;43;38;3;42;5;2;41;37;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;41;-221.3157,-693.5309;Inherit;False;Property;_Disto;Disto;15;0;Create;True;0;0;False;0;False;0;0.061;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;37;-369.8995,-1242.794;Inherit;True;Property;_Texture0;Texture 0;13;0;Create;True;0;0;False;0;False;None;322e9c3a8466cf74bb7b16519c2a19eb;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.TextureCoordinatesNode;2;-937.7502,-1239.027;Inherit;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;5;-896.6512,-939.1281;Inherit;False;Property;_Speed;Speed;16;0;Create;True;0;0;False;0;False;0.05,0;0.003,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TFHCRemapNode;42;65.68446,-893.5309;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;0.04;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;43;207.6844,-932.5309;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;3;-628.3501,-1183.127;Inherit;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;38;-394.3157,-925.5309;Inherit;True;Property;_TextureSample3;Texture Sample 3;9;0;Create;True;0;0;False;0;False;-1;322e9c3a8466cf74bb7b16519c2a19eb;322e9c3a8466cf74bb7b16519c2a19eb;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;44;-30.31558,-984.5309;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;40;-186.3157,-1088.531;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;51;-472.4144,96.37752;Inherit;False;1373.705;502.4927;FogDepth;6;18;32;33;16;31;19;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;18;-422.4144,316.5024;Inherit;False;Property;_DepthFade;Depth Fade;7;0;Create;True;0;0;False;0;False;0;16;0;200;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;39;-13.80906,-1168.196;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;52;-353.0215,-436.2976;Inherit;False;853.105;462.0044;Emissif;4;47;48;49;50;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;33;469.5579,482.8702;Inherit;False;Property;_HeightIntensity;Height Intensity;10;0;Create;True;0;0;False;0;False;0;0.256;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;356.9495,-1006.468;Inherit;True;Property;_Cloud;Cloud;11;0;Create;True;0;0;False;0;False;-1;322e9c3a8466cf74bb7b16519c2a19eb;322e9c3a8466cf74bb7b16519c2a19eb;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldNormalVector;32;485.2962,146.3775;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ColorNode;46;413.6855,-810.3959;Inherit;False;Property;_Color;Color;9;0;Create;True;0;0;False;0;False;1,0,0.8365736,0;0,0.4922409,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DepthFade;16;-13.41431,279.5024;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;19;287.7361,278.4204;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;12;367.9407,-1517.33;Inherit;True;NormalCreate;0;;1;e12f7ae19d416b942820e3932b56220f;0;4;1;SAMPLER2D;;False;2;FLOAT2;0,0;False;3;FLOAT;0.5;False;4;FLOAT;2;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;45;760.9078,-418.9967;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;35;348.6991,-1236.915;Inherit;True;Property;_Cloud1;Cloud 1;8;0;Create;True;0;0;False;0;False;-1;c7fb9361860a6144e8d152da34174b19;c7fb9361860a6144e8d152da34174b19;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;739.291,163.9607;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TexturePropertyNode;47;-303.0216,-386.298;Inherit;True;Property;_Texture1;Texture 1;14;0;Create;True;0;0;False;0;False;None;322e9c3a8466cf74bb7b16519c2a19eb;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SamplerNode;54;1091.7,-654.1746;Inherit;True;Property;_TextureSample0;Texture Sample 0;12;0;Create;True;0;0;False;0;False;-1;None;1b980fcef937e144da26d659ca3a516a;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;49;-36.31568,-186.2936;Inherit;False;Property;_Emissif;Emissif;11;0;Create;True;0;0;False;0;False;0,0,0,0;0,0.8509804,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;48;-85.71651,-383.8935;Inherit;True;Property;_TextureSample4;Texture Sample 4;10;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;55;1451.026,-510.475;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;50;338.0844,-334.4936;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;30;2132.364,-384.6235;Float;False;True;-1;6;ASEMaterialInspector;0;0;Standard;FogPlane;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;2;15.7;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;2;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;42;0;41;0
WireConnection;43;0;42;0
WireConnection;3;0;2;0
WireConnection;3;2;5;0
WireConnection;38;0;37;0
WireConnection;44;0;43;0
WireConnection;40;0;3;0
WireConnection;40;1;38;1
WireConnection;39;0;3;0
WireConnection;39;1;40;0
WireConnection;39;2;44;0
WireConnection;1;0;37;0
WireConnection;1;1;39;0
WireConnection;16;0;18;0
WireConnection;19;0;16;0
WireConnection;45;0;1;0
WireConnection;45;1;46;0
WireConnection;35;1;39;0
WireConnection;31;0;1;1
WireConnection;31;1;32;0
WireConnection;31;2;33;0
WireConnection;48;0;47;0
WireConnection;55;0;54;0
WireConnection;50;0;48;0
WireConnection;50;1;49;0
WireConnection;30;0;45;0
WireConnection;30;2;16;0
WireConnection;30;9;19;0
WireConnection;30;11;31;0
ASEEND*/
//CHKSM=21E38CB9C025A0BB9236AFBC51DC2EB489B67F99