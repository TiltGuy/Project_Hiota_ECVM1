// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/Parallax BackWall"
{
	Properties
	{
		_Back02("Back02", 2D) = "white" {}
		_Back01("Back01", 2D) = "white" {}
		_Back03("Back03", 2D) = "white" {}
		_Back01_Mask("Back01_Mask", 2D) = "white" {}
		_Back02_Mask("Back02_Mask", 2D) = "white" {}
		_GradiantValues("GradiantValues", Vector) = (0,0,0,0)
		_Back03_Mask("Back03_Mask", 2D) = "white" {}
		_Back04("Back04", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float2 uv_texcoord;
			float3 viewDir;
			INTERNAL_DATA
		};

		uniform sampler2D _Back04;
		uniform sampler2D _Back03;
		uniform sampler2D _Back03_Mask;
		SamplerState sampler_Back03_Mask;
		uniform float4 _Back03_Mask_ST;
		uniform sampler2D _Back02;
		uniform sampler2D _Back02_Mask;
		SamplerState sampler_Back02_Mask;
		uniform sampler2D _Back01;
		uniform sampler2D _Back01_Mask;
		SamplerState sampler_Back01_Mask;
		uniform float2 _GradiantValues;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Normal = float3(0,0,1);
			float2 uv_TexCoord49 = i.uv_texcoord * float2( 6,1 );
			float2 Offset115 = ( ( 1.0 - 1 ) * i.viewDir.xy * 1.0 ) + uv_TexCoord49;
			float2 Offset50 = ( ( 0.4 - 1 ) * i.viewDir.xy * 1.0 ) + uv_TexCoord49;
			float2 uv_Back03_Mask = i.uv_texcoord * _Back03_Mask_ST.xy + _Back03_Mask_ST.zw;
			float4 lerpResult113 = lerp( tex2D( _Back04, Offset115 ) , tex2D( _Back03, Offset50 ) , tex2D( _Back03_Mask, uv_Back03_Mask ).r);
			float2 Offset53 = ( ( 0.4 - 1 ) * i.viewDir.xy * 1.0 ) + uv_TexCoord49;
			float4 lerpResult58 = lerp( lerpResult113 , tex2D( _Back02, Offset53 ) , tex2D( _Back02_Mask, Offset53 ).r);
			float2 Offset54 = ( ( 0.6 - 1 ) * i.viewDir.xy * 1.0 ) + uv_TexCoord49;
			float4 lerpResult59 = lerp( lerpResult58 , tex2D( _Back01, Offset54 ) , tex2D( _Back01_Mask, Offset54 ).r);
			o.Emission = lerpResult59.rgb;
			float2 uv_TexCoord99 = i.uv_texcoord * float2( 1,2.5 );
			float smoothstepResult105 = smoothstep( _GradiantValues.x , _GradiantValues.y , uv_TexCoord99.y);
			o.Alpha = smoothstepResult105;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows 

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
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float4 tSpace0 : TEXCOORD2;
				float4 tSpace1 : TEXCOORD3;
				float4 tSpace2 : TEXCOORD4;
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
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
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
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.viewDir = IN.tSpace0.xyz * worldViewDir.x + IN.tSpace1.xyz * worldViewDir.y + IN.tSpace2.xyz * worldViewDir.z;
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
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
-1466;217;1332;594;2265.479;-832.8057;1.879576;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;49;-1689.823,676.201;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;6,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;57;-1675.814,1261.459;Inherit;False;Tangent;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ParallaxMappingNode;115;-1233.113,423.0185;Inherit;False;Normal;4;0;FLOAT2;0,0;False;1;FLOAT;1;False;2;FLOAT;1;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ParallaxMappingNode;50;-1210.883,783.2891;Inherit;False;Normal;4;0;FLOAT2;0,0;False;1;FLOAT;0.4;False;2;FLOAT;1;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;114;-555.8807,68.58895;Inherit;True;Property;_Back04;Back04;20;0;Create;True;0;0;False;0;False;-1;d877830f488945b4789a10c0efda1caf;d126f348f0b8afb4cbfd199269a716d3;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ParallaxMappingNode;53;-1200.708,1181.888;Inherit;False;Normal;4;0;FLOAT2;0,0;False;1;FLOAT;0.4;False;2;FLOAT;1;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;107;-560.7224,596.2294;Inherit;True;Property;_Back03_Mask;Back03_Mask;19;0;Create;True;0;0;False;0;False;-1;13ee0f927009e5943bcf1fa923c4b865;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;74;-561.5524,388.1378;Inherit;True;Property;_Back03;Back03;14;0;Create;True;0;0;False;0;False;-1;6e2e42fe541a5e541bbb93149a59d382;d126f348f0b8afb4cbfd199269a716d3;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;20;-3522.197,-434.12;Inherit;False;1783.557;1624.035;Example;7;12;11;5;4;17;9;15;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;72;-528.7255,834.5437;Inherit;True;Property;_Back02;Back02;12;0;Create;True;0;0;False;0;False;-1;c52e93fd5b1568a439dd280bcb1711e6;6f96fc6cd89e6aa4e8051cc1e75966d7;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ParallaxMappingNode;54;-1198.563,1548.75;Inherit;False;Normal;4;0;FLOAT2;0,0;False;1;FLOAT;0.6;False;2;FLOAT;1;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;127;-380.8896,1977.254;Inherit;False;634.7377;420.1881;Opacity;3;99;106;105;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;73;-527.6519,1079.272;Inherit;True;Property;_Back02_Mask;Back02_Mask;16;0;Create;True;0;0;False;0;False;-1;e348e8dabdcd98548a9a8d2f70ba8510;8bc5370acb3f01f46a05a50390482128;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;113;-171.3738,394.5734;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;70;-524.069,1404.997;Inherit;True;Property;_Back01;Back01;13;0;Create;True;0;0;False;0;False;-1;9bc740ef2808f874294cd4d89bd0dc59;6f96fc6cd89e6aa4e8051cc1e75966d7;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;99;-330.8896,2027.254;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,2.5;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;71;-532.5305,1645.539;Inherit;True;Property;_Back01_Mask;Back01_Mask;15;0;Create;True;0;0;False;0;False;-1;f2329e7d51358b2499acb03e3ef041a5;8bc5370acb3f01f46a05a50390482128;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;11;-3120.087,-384.12;Inherit;False;761;372;Back;2;2;1;;1,1,1,1;0;0
Node;AmplifyShaderEditor.LerpOp;58;-55.54207,967.3553;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;17;-3104.808,63.3984;Inherit;False;760.4933;495.533;middle?;3;13;14;18;;1,1,1,1;0;0
Node;AmplifyShaderEditor.Vector2Node;106;-326.1519,2223.442;Inherit;False;Property;_GradiantValues;GradiantValues;17;0;Create;True;0;0;False;0;False;0,0;1.55,1.48;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.CommentaryNode;12;-3089.703,654.2173;Inherit;False;743.1398;520.6328;InFront;3;8;6;19;;1,1,1,1;0;0
Node;AmplifyShaderEditor.ParallaxMappingNode;29;-2973.832,2839.103;Inherit;False;Normal;4;0;FLOAT2;1,1;False;1;FLOAT;0.94;False;2;FLOAT;1;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;32;-1951.502,1968.802;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;38;-2621.619,2812.498;Inherit;True;Property;_04;04;11;0;Create;True;0;0;False;0;False;-1;0a4607fe21262a848a00723c46ba5141;0a4607fe21262a848a00723c46ba5141;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;37;-2676.77,1659.302;Inherit;True;Property;_02;02;9;0;Create;True;0;0;False;0;False;-1;45ba13e82c20e3d44b7ea864b6395803;45ba13e82c20e3d44b7ea864b6395803;True;0;False;white;Auto;False;Object;-1;Auto;Cube;8;0;SAMPLERCUBE;;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;35;-2624.4,2450.684;Inherit;True;Property;_03_mask;03_mask;8;0;Create;True;0;0;False;0;False;-1;2c5f59f2bac4c9c4d8d48c50157a8586;2c5f59f2bac4c9c4d8d48c50157a8586;True;0;False;white;Auto;False;Object;-1;Auto;Cube;8;0;SAMPLERCUBE;;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;22;-3469.503,1754.983;Inherit;False;Tangent;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SamplerNode;19;-2682.955,726.1613;Inherit;True;Property;_ed84ef97b620c03e1e238e706d6ba7a2c;ed84ef97b620c03e1e238e706d6ba7a2c;3;0;Create;True;0;0;False;0;False;-1;2c70a655c5418344c861d5fdf8afa65c;2c70a655c5418344c861d5fdf8afa65c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ParallaxMappingNode;6;-3039.703,760.6503;Inherit;False;Normal;4;0;FLOAT2;1,1;False;1;FLOAT;1;False;2;FLOAT;1;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;34;-2639.92,3048.717;Inherit;True;Property;_04_Mask;04_Mask;7;0;Create;True;0;0;False;0;False;-1;f57652a3e6fc83145b888f09cc5da2c7;f57652a3e6fc83145b888f09cc5da2c7;True;0;False;white;Auto;False;Object;-1;Auto;Cube;8;0;SAMPLERCUBE;;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ParallaxMappingNode;25;-2988.727,1681.963;Inherit;False;Normal;4;0;FLOAT2;1,1;False;1;FLOAT;0.98;False;2;FLOAT;1;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;59;-68.24522,1480.986;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;8;-2713.479,959.9149;Inherit;True;Property;_PlatformArenaLOW_DefaultMaterial_Mask2;Platform-Arena-LOW_DefaultMaterial_Mask2;1;0;Create;True;0;0;False;0;False;-1;aa46518e4e6161b48b71af83422eadb3;e2294b3836a24e942b731ea6cf60e79f;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;14;-2721.314,113.3984;Inherit;True;Property;_ed84ef97b620c03e1e238e706d6ba7a2d;ed84ef97b620c03e1e238e706d6ba7a2d;2;0;Create;True;0;0;False;0;False;-1;f382cae24ca64c34695e8bd88140c0a4;f382cae24ca64c34695e8bd88140c0a4;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ParallaxMappingNode;2;-3070.088,-334.1201;Inherit;False;Normal;4;0;FLOAT2;1,1;False;1;FLOAT;0.9;False;2;FLOAT;1;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;15;-2003.64,104.3086;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;31;-2169.454,1680.166;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;1;-2736.087,-242.1202;Inherit;True;Property;_ed84ef97b620c03e1e238e706d6ba7a2b;ed84ef97b620c03e1e238e706d6ba7a2b;0;0;Create;True;0;0;False;0;False;-1;3c92c5242c29ba4449999021f925aec1;3c92c5242c29ba4449999021f925aec1;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;36;-2654.199,1904.946;Inherit;True;Property;_02_mask;02_mask;10;0;Create;True;0;0;False;0;False;-1;a963d8112a1211c4980aa5b81a1f4d22;a963d8112a1211c4980aa5b81a1f4d22;True;0;False;white;Auto;False;Object;-1;Auto;Cube;8;0;SAMPLERCUBE;;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ParallaxMappingNode;13;-3054.808,140.7057;Inherit;False;Normal;4;0;FLOAT2;1,1;False;1;FLOAT;0.96;False;2;FLOAT;1;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ParallaxMappingNode;27;-2987.809,2254.86;Inherit;False;Normal;4;0;FLOAT2;1,1;False;1;FLOAT;0.96;False;2;FLOAT;1;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;126;2648.544,1460.355;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,2.5;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;28;-2627.514,2231.271;Inherit;True;Property;_03;03;6;0;Create;True;0;0;False;0;False;-1;e2f6f6101ecef3c4da1059d03ae8e991;e2f6f6101ecef3c4da1059d03ae8e991;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;33;-1709.303,2240.688;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.Vector2Node;125;2653.282,1656.544;Inherit;False;Property;_Vector0;Vector 0;18;0;Create;True;0;0;False;0;False;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SmoothstepOpNode;124;2979.282,1576.544;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;2.56;False;2;FLOAT;1.87;False;1;FLOAT;0
Node;AmplifyShaderEditor.ParallaxMappingNode;24;-2998.26,1305.756;Inherit;False;Normal;4;0;FLOAT2;1,1;False;1;FLOAT;0.99;False;2;FLOAT;1;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;4;-3472.197,-44.36562;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;21;-3506.522,1492.552;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;18;-2695.236,313.5577;Inherit;True;Property;_Masktets;Mask tets;4;0;Create;True;0;0;False;0;False;-1;2062205d6f9358046ae7677b130c621c;2062205d6f9358046ae7677b130c621c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SmoothstepOpNode;105;-0.1519227,2143.442;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;2.56;False;2;FLOAT;1.87;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;9;-2269.946,-253.4296;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0.2765664,0.3138022,0.5188679,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;5;-3425.197,180.6345;Inherit;False;Tangent;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SamplerNode;23;-2678.845,1281.433;Inherit;True;Property;_01;01;5;0;Create;True;0;0;False;0;False;-1;7dc4371147da39841a61c173a31e879f;7dc4371147da39841a61c173a31e879f;True;0;False;white;Auto;False;Object;-1;Auto;Cube;8;0;SAMPLERCUBE;;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;361.8502,1468.721;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Custom/Parallax BackWall;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;115;0;49;0
WireConnection;115;3;57;0
WireConnection;50;0;49;0
WireConnection;50;3;57;0
WireConnection;114;1;115;0
WireConnection;53;0;49;0
WireConnection;53;3;57;0
WireConnection;74;1;50;0
WireConnection;72;1;53;0
WireConnection;54;0;49;0
WireConnection;54;3;57;0
WireConnection;73;1;53;0
WireConnection;113;0;114;0
WireConnection;113;1;74;0
WireConnection;113;2;107;1
WireConnection;70;1;54;0
WireConnection;71;1;54;0
WireConnection;58;0;113;0
WireConnection;58;1;72;0
WireConnection;58;2;73;1
WireConnection;29;0;21;0
WireConnection;29;3;22;0
WireConnection;32;0;31;0
WireConnection;32;1;28;0
WireConnection;32;2;35;1
WireConnection;38;1;29;0
WireConnection;37;1;25;0
WireConnection;35;1;27;0
WireConnection;19;1;6;0
WireConnection;6;0;4;0
WireConnection;6;3;5;0
WireConnection;34;1;29;0
WireConnection;25;0;21;0
WireConnection;25;3;22;0
WireConnection;59;0;58;0
WireConnection;59;1;70;0
WireConnection;59;2;71;1
WireConnection;8;1;6;0
WireConnection;14;1;13;0
WireConnection;2;0;4;0
WireConnection;2;3;5;0
WireConnection;15;0;9;0
WireConnection;15;1;14;0
WireConnection;15;2;18;1
WireConnection;31;0;23;0
WireConnection;31;1;37;0
WireConnection;31;2;36;1
WireConnection;1;1;2;0
WireConnection;36;1;25;0
WireConnection;13;0;4;0
WireConnection;27;0;21;0
WireConnection;27;3;22;0
WireConnection;28;1;27;0
WireConnection;33;0;32;0
WireConnection;33;1;38;0
WireConnection;33;2;34;1
WireConnection;124;0;126;2
WireConnection;124;1;125;1
WireConnection;124;2;125;2
WireConnection;24;0;21;0
WireConnection;24;3;22;0
WireConnection;18;1;13;0
WireConnection;105;0;99;2
WireConnection;105;1;106;1
WireConnection;105;2;106;2
WireConnection;9;0;1;0
WireConnection;9;1;19;0
WireConnection;9;2;8;1
WireConnection;23;1;24;0
WireConnection;0;2;59;0
WireConnection;0;9;105;0
ASEEND*/
//CHKSM=8F2F3D8A65BEB408D79D6D0BEB06292FC52BB927