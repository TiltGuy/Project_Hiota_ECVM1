// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/Parallax Wall"
{
	Properties
	{
		_01("01", 2D) = "white" {}
		_03("03", 2D) = "white" {}
		_04_Mask("04_Mask", 2D) = "white" {}
		_03_mask("03_mask", 2D) = "white" {}
		_02("02", 2D) = "white" {}
		_02_mask("02_mask", 2D) = "white" {}
		_04("04", 2D) = "white" {}
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

		uniform sampler2D _01;
		uniform sampler2D _02;
		uniform sampler2D _02_mask;
		SamplerState sampler_02_mask;
		uniform sampler2D _03;
		uniform sampler2D _03_mask;
		SamplerState sampler_03_mask;
		uniform sampler2D _04;
		uniform sampler2D _04_Mask;
		SamplerState sampler_04_Mask;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Normal = float3(0,0,1);
			float2 Offset24 = ( ( 0.9 - 1 ) * i.viewDir.xy * 1.0 ) + i.uv_texcoord;
			float2 Offset25 = ( ( 0.85 - 1 ) * i.viewDir.xy * 1.0 ) + i.uv_texcoord;
			float4 lerpResult31 = lerp( tex2D( _01, Offset24 ) , tex2D( _02, Offset25 ) , tex2D( _02_mask, Offset25 ).r);
			float2 Offset27 = ( ( 0.83 - 1 ) * i.viewDir.xy * 1.0 ) + i.uv_texcoord;
			float4 lerpResult32 = lerp( lerpResult31 , tex2D( _03, Offset27 ) , tex2D( _03_mask, Offset27 ).r);
			float2 Offset29 = ( ( 0.8 - 1 ) * i.viewDir.xy * 1.0 ) + i.uv_texcoord;
			float4 lerpResult33 = lerp( lerpResult32 , tex2D( _04, Offset29 ) , tex2D( _04_Mask, Offset29 ).r);
			o.Emission = lerpResult33.rgb;
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
-1920;2;1920;1019;2824.815;303.1245;2.164302;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;21;-1546.331,17.08419;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;22;-1509.312,279.5157;Inherit;False;Tangent;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ParallaxMappingNode;25;-1028.536,206.4952;Inherit;False;Normal;4;0;FLOAT2;1,1;False;1;FLOAT;0.85;False;2;FLOAT;1;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ParallaxMappingNode;24;-1038.069,-171.5759;Inherit;False;Normal;4;0;FLOAT2;1,1;False;1;FLOAT;0.9;False;2;FLOAT;1;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;23;-718.6545,-194.0347;Inherit;True;Property;_01;01;5;0;Create;True;0;0;False;0;False;-1;7dc4371147da39841a61c173a31e879f;7dc4371147da39841a61c173a31e879f;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;37;-716.5797,183.8344;Inherit;True;Property;_02;02;10;0;Create;True;0;0;False;0;False;-1;45ba13e82c20e3d44b7ea864b6395803;45ba13e82c20e3d44b7ea864b6395803;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ParallaxMappingNode;27;-1027.618,779.392;Inherit;False;Normal;4;0;FLOAT2;1,1;False;1;FLOAT;0.83;False;2;FLOAT;1;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;36;-694.0085,429.4785;Inherit;True;Property;_02_mask;02_mask;11;0;Create;True;0;0;False;0;False;-1;a963d8112a1211c4980aa5b81a1f4d22;a963d8112a1211c4980aa5b81a1f4d22;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ParallaxMappingNode;29;-1013.641,1363.635;Inherit;False;Normal;4;0;FLOAT2;1,1;False;1;FLOAT;0.8;False;2;FLOAT;1;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;20;-3522.197,-434.12;Inherit;False;1783.557;1624.035;Example;7;12;11;5;4;17;9;15;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;28;-667.324,755.803;Inherit;True;Property;_03;03;6;0;Create;True;0;0;False;0;False;-1;e2f6f6101ecef3c4da1059d03ae8e991;e2f6f6101ecef3c4da1059d03ae8e991;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;31;-209.2638,204.6983;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;35;-664.2098,975.2157;Inherit;True;Property;_03_mask;03_mask;9;0;Create;True;0;0;False;0;False;-1;2c5f59f2bac4c9c4d8d48c50157a8586;2c5f59f2bac4c9c4d8d48c50157a8586;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;11;-3120.087,-384.12;Inherit;False;761;372;Back;2;2;1;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;17;-3104.808,63.3984;Inherit;False;760.4933;495.533;middle?;3;13;14;18;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;12;-3089.703,654.2173;Inherit;False;743.1398;520.6328;InFront;3;8;6;19;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;34;-679.7299,1573.248;Inherit;True;Property;_04_Mask;04_Mask;8;0;Create;True;0;0;False;0;False;-1;f57652a3e6fc83145b888f09cc5da2c7;f57652a3e6fc83145b888f09cc5da2c7;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;38;-661.4281,1337.03;Inherit;True;Property;_04;04;11;0;Create;True;0;0;False;0;False;-1;0a4607fe21262a848a00723c46ba5141;0a4607fe21262a848a00723c46ba5141;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;32;8.687996,493.3344;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;4;-3472.197,-44.36562;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;14;-2721.314,113.3984;Inherit;True;Property;_ed84ef97b620c03e1e238e706d6ba7a2d;ed84ef97b620c03e1e238e706d6ba7a2d;2;0;Create;True;0;0;False;0;False;-1;f382cae24ca64c34695e8bd88140c0a4;f382cae24ca64c34695e8bd88140c0a4;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;15;-2003.64,104.3086;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ParallaxMappingNode;13;-3054.808,140.7057;Inherit;False;Normal;4;0;FLOAT2;1,1;False;1;FLOAT;0.96;False;2;FLOAT;1;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ParallaxMappingNode;6;-3039.703,760.6503;Inherit;False;Normal;4;0;FLOAT2;1,1;False;1;FLOAT;1;False;2;FLOAT;1;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;18;-2695.236,313.5577;Inherit;True;Property;_Masktets;Mask tets;4;0;Create;True;0;0;False;0;False;-1;2062205d6f9358046ae7677b130c621c;2062205d6f9358046ae7677b130c621c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;33;250.8864,765.2206;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ParallaxMappingNode;2;-3070.088,-334.1201;Inherit;False;Normal;4;0;FLOAT2;1,1;False;1;FLOAT;0.9;False;2;FLOAT;1;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;9;-2269.946,-253.4296;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0.2765664,0.3138022,0.5188679,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;5;-3425.197,180.6345;Inherit;False;Tangent;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SamplerNode;19;-2682.955,726.1613;Inherit;True;Property;_ed84ef97b620c03e1e238e706d6ba7a2c;ed84ef97b620c03e1e238e706d6ba7a2c;3;0;Create;True;0;0;False;0;False;-1;2c70a655c5418344c861d5fdf8afa65c;2c70a655c5418344c861d5fdf8afa65c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;8;-2713.479,959.9149;Inherit;True;Property;_PlatformArenaLOW_DefaultMaterial_Mask2;Platform-Arena-LOW_DefaultMaterial_Mask2;1;0;Create;True;0;0;False;0;False;-1;aa46518e4e6161b48b71af83422eadb3;e2294b3836a24e942b731ea6cf60e79f;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-2736.087,-242.1202;Inherit;True;Property;_ed84ef97b620c03e1e238e706d6ba7a2b;ed84ef97b620c03e1e238e706d6ba7a2b;0;0;Create;True;0;0;False;0;False;-1;3c92c5242c29ba4449999021f925aec1;3c92c5242c29ba4449999021f925aec1;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;608.8163,720.0249;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Custom/Parallax Wall;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;25;0;21;0
WireConnection;25;3;22;0
WireConnection;24;0;21;0
WireConnection;24;3;22;0
WireConnection;23;1;24;0
WireConnection;37;1;25;0
WireConnection;27;0;21;0
WireConnection;27;3;22;0
WireConnection;36;1;25;0
WireConnection;29;0;21;0
WireConnection;29;3;22;0
WireConnection;28;1;27;0
WireConnection;31;0;23;0
WireConnection;31;1;37;0
WireConnection;31;2;36;1
WireConnection;35;1;27;0
WireConnection;34;1;29;0
WireConnection;38;1;29;0
WireConnection;32;0;31;0
WireConnection;32;1;28;0
WireConnection;32;2;35;1
WireConnection;14;1;13;0
WireConnection;15;0;9;0
WireConnection;15;1;14;0
WireConnection;15;2;18;1
WireConnection;13;0;4;0
WireConnection;6;0;4;0
WireConnection;6;3;5;0
WireConnection;18;1;13;0
WireConnection;33;0;32;0
WireConnection;33;1;38;0
WireConnection;33;2;34;1
WireConnection;2;0;4;0
WireConnection;2;3;5;0
WireConnection;9;0;1;0
WireConnection;9;1;19;0
WireConnection;9;2;8;1
WireConnection;19;1;6;0
WireConnection;8;1;6;0
WireConnection;1;1;2;0
WireConnection;0;2;33;0
ASEEND*/
//CHKSM=C9BD7B85B9858076E99DC2689C71928B361FCFCF