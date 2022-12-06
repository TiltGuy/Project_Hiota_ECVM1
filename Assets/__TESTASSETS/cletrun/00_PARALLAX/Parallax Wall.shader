// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/Parallax Wall"
{
	Properties
	{
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_TextureSample1("Texture Sample 1", 2D) = "white" {}
		_05("05", 2D) = "white" {}
		_06("06", 2D) = "white" {}
		_07("07", 2D) = "white" {}
		_Mask_03("Mask_03", 2D) = "white" {}
		_Mask_04("Mask_04", 2D) = "white" {}
		_Mask_05("Mask_05", 2D) = "white" {}
		_Mask_06("Mask_06", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 uv_texcoord;
			float3 viewDir;
		};

		uniform sampler2D _07;
		uniform sampler2D _06;
		uniform sampler2D _Mask_06;
		SamplerState sampler_Mask_06;
		uniform sampler2D _05;
		uniform sampler2D _Mask_05;
		SamplerState sampler_Mask_05;
		uniform sampler2D _TextureSample1;
		uniform sampler2D _Mask_04;
		SamplerState sampler_Mask_04;
		uniform sampler2D _TextureSample0;
		uniform sampler2D _Mask_03;
		SamplerState sampler_Mask_03;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_TexCoord49 = i.uv_texcoord * float2( 4,1 );
			float2 Offset50 = ( ( 0.7 - 1 ) * i.viewDir.xy * 2.0 ) + uv_TexCoord49;
			float2 Offset53 = ( ( 0.8 - 1 ) * i.viewDir.xy * 2.0 ) + uv_TexCoord49;
			float4 lerpResult58 = lerp( tex2D( _07, Offset50 ) , tex2D( _06, Offset53 ) , tex2D( _Mask_06, Offset53 ).r);
			float2 Offset54 = ( ( 0.84 - 1 ) * i.viewDir.xy * 1.0 ) + uv_TexCoord49;
			float4 lerpResult59 = lerp( lerpResult58 , tex2D( _05, Offset54 ) , tex2D( _Mask_05, Offset54 ).r);
			float2 Offset55 = ( ( 0.92 - 1 ) * i.viewDir.xy * 1.0 ) + uv_TexCoord49;
			float4 lerpResult60 = lerp( lerpResult59 , tex2D( _TextureSample1, Offset55 ) , tex2D( _Mask_04, Offset55 ).r);
			float2 Offset56 = ( ( 1.0 - 1 ) * i.viewDir.xy * 1.0 ) + uv_TexCoord49;
			float4 lerpResult61 = lerp( lerpResult60 , tex2D( _TextureSample0, Offset56 ) , tex2D( _Mask_03, Offset56 ).r);
			o.Emission = lerpResult61.rgb;
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows 

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
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
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
				surfIN.viewDir = worldViewDir;
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
-862;83;809;879;2980.592;546.4859;3.512478;True;False
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;57;-1675.814,1261.459;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.TextureCoordinatesNode;49;-1689.823,678.101;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;4,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ParallaxMappingNode;50;-1183.966,69.06467;Inherit;False;Normal;4;0;FLOAT2;0,0;False;1;FLOAT;0.7;False;2;FLOAT;2;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ParallaxMappingNode;53;-1173.791,467.6631;Inherit;False;Normal;4;0;FLOAT2;0,0;False;1;FLOAT;0.8;False;2;FLOAT;2;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;42;-503.1292,13.39744;Inherit;True;Property;_06;06;15;0;Create;True;0;0;False;0;False;-1;78ea672b31f99b24c85271f1825d763a;78ea672b31f99b24c85271f1825d763a;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ParallaxMappingNode;54;-1171.646,834.5258;Inherit;False;Normal;4;0;FLOAT2;0,0;False;1;FLOAT;0.84;False;2;FLOAT;1;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;43;-507.0606,-534.4853;Inherit;True;Property;_07;07;16;0;Create;True;0;0;False;0;False;-1;75c0ff680066bee489cf2d470fb9fc8d;75c0ff680066bee489cf2d470fb9fc8d;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;47;-510.145,251.9172;Inherit;True;Property;_Mask_06;Mask_06;20;0;Create;True;0;0;False;0;False;-1;4a419e4ebb3ead541a0bb0d40c3531c9;4a419e4ebb3ead541a0bb0d40c3531c9;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ParallaxMappingNode;55;-1167.356,1358.002;Inherit;False;Normal;4;0;FLOAT2;0,0;False;1;FLOAT;0.92;False;2;FLOAT;1;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;46;-487.2147,865.1512;Inherit;True;Property;_Mask_05;Mask_05;19;0;Create;True;0;0;False;0;False;-1;9cdd1042398c650429e07dfb1d73fc00;9cdd1042398c650429e07dfb1d73fc00;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;58;-28.62488,253.1308;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;41;-503.6959,630.3035;Inherit;True;Property;_05;05;14;0;Create;True;0;0;False;0;False;-1;11c883c93375b9a4abd346b50d07c28d;11c883c93375b9a4abd346b50d07c28d;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;45;-484.9839,1481.869;Inherit;True;Property;_Mask_04;Mask_04;18;0;Create;True;0;0;False;0;False;-1;382cf2a2f7cf5bf4a852912759481642;382cf2a2f7cf5bf4a852912759481642;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ParallaxMappingNode;56;-1120.157,1913.66;Inherit;False;Normal;4;0;FLOAT2;0,0;False;1;FLOAT;1;False;2;FLOAT;1;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;59;-41.32803,766.7616;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;20;-3522.197,-434.12;Inherit;False;1783.557;1624.035;Example;7;12;11;5;4;17;9;15;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;40;-480.8646,1267.621;Inherit;True;Property;_TextureSample1;Texture Sample 1;13;0;Create;True;0;0;False;0;False;-1;184302da9522d0e45840e1c4ed62ce70;184302da9522d0e45840e1c4ed62ce70;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;39;-486.4959,1868.272;Inherit;True;Property;_TextureSample0;Texture Sample 0;12;0;Create;True;0;0;False;0;False;-1;40db8cc80b1cfd742a047acb1933824b;40db8cc80b1cfd742a047acb1933824b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;44;-494.7357,2086.64;Inherit;True;Property;_Mask_03;Mask_03;17;0;Create;True;0;0;False;0;False;-1;cba9ea077dc910648a00230b88846072;cba9ea077dc910648a00230b88846072;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;60;-33.63139,1385.593;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;17;-3104.808,63.3984;Inherit;False;760.4933;495.533;middle?;3;13;14;18;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;12;-3089.703,654.2173;Inherit;False;743.1398;520.6328;InFront;3;8;6;19;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;11;-3120.087,-384.12;Inherit;False;761;372;Back;2;2;1;;1,1,1,1;0;0
Node;AmplifyShaderEditor.ParallaxMappingNode;29;-2973.832,2839.103;Inherit;False;Normal;4;0;FLOAT2;1,1;False;1;FLOAT;0.94;False;2;FLOAT;1;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ParallaxMappingNode;6;-3039.703,760.6503;Inherit;False;Normal;4;0;FLOAT2;1,1;False;1;FLOAT;1;False;2;FLOAT;1;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ParallaxMappingNode;24;-2998.26,1305.756;Inherit;False;Normal;4;0;FLOAT2;1,1;False;1;FLOAT;0.99;False;2;FLOAT;1;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ParallaxMappingNode;13;-3054.808,140.7057;Inherit;False;Normal;4;0;FLOAT2;1,1;False;1;FLOAT;0.96;False;2;FLOAT;1;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;35;-2624.4,2450.684;Inherit;True;Property;_03_mask;03_mask;8;0;Create;True;0;0;False;0;False;-1;2c5f59f2bac4c9c4d8d48c50157a8586;2c5f59f2bac4c9c4d8d48c50157a8586;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;38;-2621.619,2812.498;Inherit;True;Property;_04;04;11;0;Create;True;0;0;False;0;False;-1;0a4607fe21262a848a00723c46ba5141;0a4607fe21262a848a00723c46ba5141;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;33;-1709.303,2240.688;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;9;-2269.946,-253.4296;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0.2765664,0.3138022,0.5188679,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;61;-19.32577,1948.287;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;32;-1951.502,1968.802;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;34;-2639.92,3048.717;Inherit;True;Property;_04_Mask;04_Mask;7;0;Create;True;0;0;False;0;False;-1;f57652a3e6fc83145b888f09cc5da2c7;f57652a3e6fc83145b888f09cc5da2c7;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;14;-2721.314,113.3984;Inherit;True;Property;_ed84ef97b620c03e1e238e706d6ba7a2d;ed84ef97b620c03e1e238e706d6ba7a2d;2;0;Create;True;0;0;False;0;False;-1;f382cae24ca64c34695e8bd88140c0a4;f382cae24ca64c34695e8bd88140c0a4;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-2736.087,-242.1202;Inherit;True;Property;_ed84ef97b620c03e1e238e706d6ba7a2b;ed84ef97b620c03e1e238e706d6ba7a2b;0;0;Create;True;0;0;False;0;False;-1;3c92c5242c29ba4449999021f925aec1;3c92c5242c29ba4449999021f925aec1;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;37;-2676.77,1659.302;Inherit;True;Property;_02;02;9;0;Create;True;0;0;False;0;False;-1;45ba13e82c20e3d44b7ea864b6395803;45ba13e82c20e3d44b7ea864b6395803;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;18;-2695.236,313.5577;Inherit;True;Property;_Masktets;Mask tets;4;0;Create;True;0;0;False;0;False;-1;2062205d6f9358046ae7677b130c621c;2062205d6f9358046ae7677b130c621c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;5;-3425.197,180.6345;Inherit;False;Tangent;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ParallaxMappingNode;2;-3070.088,-334.1201;Inherit;False;Normal;4;0;FLOAT2;1,1;False;1;FLOAT;0.9;False;2;FLOAT;1;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;21;-3506.522,1492.552;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;23;-2678.845,1281.433;Inherit;True;Property;_01;01;5;0;Create;True;0;0;False;0;False;-1;7dc4371147da39841a61c173a31e879f;7dc4371147da39841a61c173a31e879f;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;31;-2169.454,1680.166;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;19;-2682.955,726.1613;Inherit;True;Property;_ed84ef97b620c03e1e238e706d6ba7a2c;ed84ef97b620c03e1e238e706d6ba7a2c;3;0;Create;True;0;0;False;0;False;-1;2c70a655c5418344c861d5fdf8afa65c;2c70a655c5418344c861d5fdf8afa65c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;15;-2003.64,104.3086;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;28;-2627.514,2231.271;Inherit;True;Property;_03;03;6;0;Create;True;0;0;False;0;False;-1;e2f6f6101ecef3c4da1059d03ae8e991;e2f6f6101ecef3c4da1059d03ae8e991;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ParallaxMappingNode;25;-2988.727,1681.963;Inherit;False;Normal;4;0;FLOAT2;1,1;False;1;FLOAT;0.98;False;2;FLOAT;1;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;22;-3469.503,1754.983;Inherit;False;Tangent;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ParallaxMappingNode;27;-2987.809,2254.86;Inherit;False;Normal;4;0;FLOAT2;1,1;False;1;FLOAT;0.96;False;2;FLOAT;1;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;4;-3472.197,-44.36562;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;8;-2713.479,959.9149;Inherit;True;Property;_PlatformArenaLOW_DefaultMaterial_Mask2;Platform-Arena-LOW_DefaultMaterial_Mask2;1;0;Create;True;0;0;False;0;False;-1;aa46518e4e6161b48b71af83422eadb3;e2294b3836a24e942b731ea6cf60e79f;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;48;-503.3238,-283.8997;Inherit;True;Property;_Mask_07;Mask_07;21;0;Create;True;0;0;False;0;False;-1;a724af61fda66114daccc1b399da48d6;a724af61fda66114daccc1b399da48d6;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;36;-2654.199,1904.946;Inherit;True;Property;_02_mask;02_mask;10;0;Create;True;0;0;False;0;False;-1;a963d8112a1211c4980aa5b81a1f4d22;a963d8112a1211c4980aa5b81a1f4d22;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;301.5915,1896.065;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Custom/Parallax Wall;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;50;0;49;0
WireConnection;50;3;57;0
WireConnection;53;0;49;0
WireConnection;53;3;57;0
WireConnection;42;1;53;0
WireConnection;54;0;49;0
WireConnection;54;3;57;0
WireConnection;43;1;50;0
WireConnection;47;1;53;0
WireConnection;55;0;49;0
WireConnection;55;3;57;0
WireConnection;46;1;54;0
WireConnection;58;0;43;0
WireConnection;58;1;42;0
WireConnection;58;2;47;1
WireConnection;41;1;54;0
WireConnection;45;1;55;0
WireConnection;56;0;49;0
WireConnection;56;3;57;0
WireConnection;59;0;58;0
WireConnection;59;1;41;0
WireConnection;59;2;46;1
WireConnection;40;1;55;0
WireConnection;39;1;56;0
WireConnection;44;1;56;0
WireConnection;60;0;59;0
WireConnection;60;1;40;0
WireConnection;60;2;45;1
WireConnection;29;0;21;0
WireConnection;29;3;22;0
WireConnection;6;0;4;0
WireConnection;6;3;5;0
WireConnection;24;0;21;0
WireConnection;24;3;22;0
WireConnection;13;0;4;0
WireConnection;35;1;27;0
WireConnection;38;1;29;0
WireConnection;33;0;32;0
WireConnection;33;1;38;0
WireConnection;33;2;34;1
WireConnection;9;0;1;0
WireConnection;9;1;19;0
WireConnection;9;2;8;1
WireConnection;61;0;60;0
WireConnection;61;1;39;0
WireConnection;61;2;44;1
WireConnection;32;0;31;0
WireConnection;32;1;28;0
WireConnection;32;2;35;1
WireConnection;34;1;29;0
WireConnection;14;1;13;0
WireConnection;1;1;2;0
WireConnection;37;1;25;0
WireConnection;18;1;13;0
WireConnection;2;0;4;0
WireConnection;2;3;5;0
WireConnection;23;1;24;0
WireConnection;31;0;23;0
WireConnection;31;1;37;0
WireConnection;31;2;36;1
WireConnection;19;1;6;0
WireConnection;15;0;9;0
WireConnection;15;1;14;0
WireConnection;15;2;18;1
WireConnection;28;1;27;0
WireConnection;25;0;21;0
WireConnection;25;3;22;0
WireConnection;27;0;21;0
WireConnection;27;3;22;0
WireConnection;8;1;6;0
WireConnection;48;1;50;0
WireConnection;36;1;25;0
WireConnection;0;2;61;0
ASEEND*/
//CHKSM=A491F615AF6C23E07CAEFD52CDA1B45F105715C9