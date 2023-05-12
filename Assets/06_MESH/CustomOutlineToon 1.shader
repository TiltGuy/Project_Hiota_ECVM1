// Made with Amplify Shader Editor v1.9.1.3
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/Toon"
{
	Properties
	{
		_ASEOutlineColor( "Outline Color", Color ) = (0,0,0,0)
		[NoScaleOffset]_BaseColorRGBOutlineWidthA("Base Color (RGB) Outline Width (A)", 2D) = "gray" {}
		[HDR]_BaseTint("Base Tint", Color) = (1,1,1,0)
		_BaseCellSharpness("Base Cell Sharpness", Range( 0.01 , 1)) = 0.01
		_BaseCellOffset("Base Cell Offset", Range( -1 , 1)) = 0
		_IndirectDiffuseContribution("Indirect Diffuse Contribution", Range( 0 , 1)) = 1
		_ShadowContribution("Shadow Contribution", Range( 0 , 1)) = 0.5
		[NoScaleOffset]_SpecularMap("Specular Map", 2D) = "white" {}
		[HDR]_SpecularColor("Specular Color", Color) = (1,1,1,1)
		_specularhighlightsize("specular highlight size", Range( -1 , -0.5)) = -0.95
		_HighlightCellSharpness("Highlight Cell Sharpness", Range( 0.001 , 1)) = 0.01
		_IndirectSpecularContribution("Indirect Specular Contribution", Range( 0 , 1)) = 1
		[Toggle(_STATICHIGHLIGHTS_ON)] _StaticHighLights("Static HighLights", Float) = 0
		[Normal][NoScaleOffset]_NormalMap("Normal Map", 2D) = "bump" {}
		_NormalScale("Normal Scale", Range( 0 , 1)) = 1
		_HiotaV3_Material001_Emissive("HiotaV3_Material.001_Emissive", 2D) = "white" {}
		_Emissiveintensity("Emissive intensity", Range( 0 , 20)) = 0
		[HDR]_EmissiveColorA("Emissive Color A", Color) = (0,0,0,0)
		[HDR]_EmissiveColorB("Emissive Color B", Color) = (0,0,0,0)
		_OutlineWidth("Outline Width", Range( 0 , 10)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ }
		Cull Front
		CGPROGRAM
		#pragma target 3.0
		#pragma surface outlineSurf Outline nofog  keepalpha noshadow noambient novertexlights nolightmap nodynlightmap nodirlightmap nometa noforwardadd vertex:outlineVertexDataFunc 
		
		float4 _ASEOutlineColor;
		void outlineVertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_vertexNormal = v.normal.xyz;
			float3 outlineVar = ( ase_vertexNormal * _OutlineWidth );
			v.vertex.xyz += outlineVar;
		}
		inline half4 LightingOutline( SurfaceOutput s, half3 lightDir, half atten ) { return half4 ( 0,0,0, s.Alpha); }
		void outlineSurf( Input i, inout SurfaceOutput o )
		{
			o.Emission = _ASEOutlineColor.rgb;
			o.Alpha = 1;
		}
		ENDCG
		

		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Off
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "UnityStandardUtils.cginc"
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#include "Lighting.cginc"
		#pragma target 4.6
		#pragma shader_feature_local _STATICHIGHLIGHTS_ON
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
			float3 worldNormal;
			INTERNAL_DATA
			float2 uv_texcoord;
			float3 worldPos;
		};

		struct SurfaceOutputCustomLightingCustom
		{
			half3 Albedo;
			half3 Normal;
			half3 Emission;
			half Metallic;
			half Smoothness;
			half Occlusion;
			half Alpha;
			Input SurfInput;
			UnityGIInput GIData;
		};

		uniform sampler2D _NormalMap;
		uniform float _NormalScale;
		uniform float4 _SpecularColor;
		uniform sampler2D _SpecularMap;
		uniform float _IndirectSpecularContribution;
		uniform float _specularhighlightsize;
		uniform float _HighlightCellSharpness;
		uniform float _IndirectDiffuseContribution;
		uniform float _BaseCellOffset;
		uniform float _BaseCellSharpness;
		uniform float _ShadowContribution;
		uniform sampler2D _BaseColorRGBOutlineWidthA;
		uniform float4 _BaseTint;
		uniform float4 _EmissiveColorA;
		uniform float4 _EmissiveColorB;
		uniform float _Emissiveintensity;
		uniform sampler2D _HiotaV3_Material001_Emissive;
		uniform float4 _HiotaV3_Material001_Emissive_ST;
		uniform float _OutlineWidth;


		struct Gradient
		{
			int type;
			int colorsLength;
			int alphasLength;
			float4 colors[8];
			float2 alphas[8];
		};


		Gradient NewGradient(int type, int colorsLength, int alphasLength, 
		float4 colors0, float4 colors1, float4 colors2, float4 colors3, float4 colors4, float4 colors5, float4 colors6, float4 colors7,
		float2 alphas0, float2 alphas1, float2 alphas2, float2 alphas3, float2 alphas4, float2 alphas5, float2 alphas6, float2 alphas7)
		{
			Gradient g;
			g.type = type;
			g.colorsLength = colorsLength;
			g.alphasLength = alphasLength;
			g.colors[ 0 ] = colors0;
			g.colors[ 1 ] = colors1;
			g.colors[ 2 ] = colors2;
			g.colors[ 3 ] = colors3;
			g.colors[ 4 ] = colors4;
			g.colors[ 5 ] = colors5;
			g.colors[ 6 ] = colors6;
			g.colors[ 7 ] = colors7;
			g.alphas[ 0 ] = alphas0;
			g.alphas[ 1 ] = alphas1;
			g.alphas[ 2 ] = alphas2;
			g.alphas[ 3 ] = alphas3;
			g.alphas[ 4 ] = alphas4;
			g.alphas[ 5 ] = alphas5;
			g.alphas[ 6 ] = alphas6;
			g.alphas[ 7 ] = alphas7;
			return g;
		}


		float4 SampleGradient( Gradient gradient, float time )
		{
			float3 color = gradient.colors[0].rgb;
			UNITY_UNROLL
			for (int c = 1; c < 8; c++)
			{
			float colorPos = saturate((time - gradient.colors[c-1].w) / ( 0.00001 + (gradient.colors[c].w - gradient.colors[c-1].w)) * step(c, (float)gradient.colorsLength-1));
			color = lerp(color, gradient.colors[c].rgb, lerp(colorPos, step(0.01, colorPos), gradient.type));
			}
			#ifndef UNITY_COLORSPACE_GAMMA
			color = half3(GammaToLinearSpaceExact(color.r), GammaToLinearSpaceExact(color.g), GammaToLinearSpaceExact(color.b));
			#endif
			float alpha = gradient.alphas[0].x;
			UNITY_UNROLL
			for (int a = 1; a < 8; a++)
			{
			float alphaPos = saturate((time - gradient.alphas[a-1].y) / ( 0.00001 + (gradient.alphas[a].y - gradient.alphas[a-1].y)) * step(a, (float)gradient.alphasLength-1));
			alpha = lerp(alpha, gradient.alphas[a].x, lerp(alphaPos, step(0.01, alphaPos), gradient.type));
			}
			return float4(color, alpha);
		}


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			v.vertex.xyz += 0;
			v.vertex.w = 1;
		}

		inline half4 LightingStandardCustomLighting( inout SurfaceOutputCustomLightingCustom s, half3 viewDir, UnityGI gi )
		{
			UnityGIInput data = s.GIData;
			Input i = s.SurfInput;
			half4 c = 0;
			#ifdef UNITY_PASS_FORWARDBASE
			float ase_lightAtten = data.atten;
			if( _LightColor0.a == 0)
			ase_lightAtten = 0;
			#else
			float3 ase_lightAttenRGB = gi.light.color / ( ( _LightColor0.rgb ) + 0.000001 );
			float ase_lightAtten = max( max( ase_lightAttenRGB.r, ase_lightAttenRGB.g ), ase_lightAttenRGB.b );
			#endif
			#if defined(HANDLE_SHADOWS_BLENDING_IN_GI)
			half bakedAtten = UnitySampleBakedOcclusion(data.lightmapUV.xy, data.worldPos);
			float zDist = dot(_WorldSpaceCameraPos - data.worldPos, UNITY_MATRIX_V[2].xyz);
			float fadeDist = UnityComputeShadowFadeDistance(data.worldPos, zDist);
			ase_lightAtten = UnityMixRealtimeAndBakedShadows(data.atten, bakedAtten, UnityComputeShadowFade(fadeDist));
			#endif
			float3 temp_cast_0 = (1.0).xxx;
			float2 uv_NormalMap82 = i.uv_texcoord;
			float3 normalizeResult170 = normalize( (WorldNormalVector( i , UnpackScaleNormal( tex2D( _NormalMap, uv_NormalMap82 ), _NormalScale ) )) );
			float3 NewNormals220 = normalizeResult170;
			float3 indirectNormal106 = NewNormals220;
			float2 uv_SpecularMap81 = i.uv_texcoord;
			float4 temp_output_184_0 = ( _SpecularColor * tex2D( _SpecularMap, uv_SpecularMap81 ) );
			float temp_output_189_0 = (temp_output_184_0).a;
			Unity_GlossyEnvironmentData g106 = UnityGlossyEnvironmentSetup( temp_output_189_0, data.worldViewDir, indirectNormal106, float3(0,0,0));
			float3 indirectSpecular106 = UnityGI_IndirectSpecular( data, 1.0, indirectNormal106, g106 );
			float3 lerpResult187 = lerp( temp_cast_0 , indirectSpecular106 , _IndirectSpecularContribution);
			float3 HighlightColor249 = (temp_output_184_0).rgb;
			#if defined(LIGHTMAP_ON) && ( UNITY_VERSION < 560 || ( defined(LIGHTMAP_SHADOW_MIXING) && !defined(SHADOWS_SHADOWMASK) && defined(SHADOWS_SCREEN) ) )//aselc
			float4 ase_lightColor = 0;
			#else //aselc
			float4 ase_lightColor = _LightColor0;
			#endif //aselc
			float3 LightColorFalloff227 = ( ase_lightColor.rgb * ase_lightAtten );
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float3 normalizeResult4_g3 = normalize( ( ase_worldViewDir + ase_worldlightDir ) );
			float dotResult62 = dot( normalizeResult4_g3 , NewNormals220 );
			float dotResult54 = dot( NewNormals220 , ase_worldlightDir );
			float NdotL236 = dotResult54;
			#ifdef _STATICHIGHLIGHTS_ON
				float staticSwitch195 = NdotL236;
			#else
				float staticSwitch195 = dotResult62;
			#endif
			float3 temp_cast_2 = (1.0).xxx;
			UnityGI gi115 = gi;
			float3 diffNorm115 = NewNormals220;
			gi115 = UnityGI_Base( data, 1, diffNorm115 );
			float3 indirectDiffuse115 = gi115.indirect.diffuse + diffNorm115 * 0.0001;
			float3 lerpResult117 = lerp( temp_cast_2 , indirectDiffuse115 , _IndirectDiffuseContribution);
			float temp_output_214_0 = ( 1.0 - ( ( 1.0 - ase_lightAtten ) * _WorldSpaceLightPos0.w ) );
			Gradient gradient265 = NewGradient( 1, 2, 2, float4( 0.3207547, 0.3207547, 0.3207547, 0.4470588 ), float4( 1, 1, 1, 0.5823606 ), 0, 0, 0, 0, 0, 0, float2( 1, 0 ), float2( 1, 1 ), 0, 0, 0, 0, 0, 0 );
			float lerpResult159 = lerp( temp_output_214_0 , ( saturate( ( ( NdotL236 + _BaseCellOffset ) / _BaseCellSharpness ) ) * ase_lightAtten ) , _ShadowContribution);
			float2 uv_BaseColorRGBOutlineWidthA76 = i.uv_texcoord;
			float4 tex2DNode76 = tex2D( _BaseColorRGBOutlineWidthA, uv_BaseColorRGBOutlineWidthA76 );
			float3 BaseColor253 = ( ( ( lerpResult117 * ase_lightColor.a * temp_output_214_0 ) + ( ase_lightColor.rgb * SampleGradient( gradient265, lerpResult159 ).r ) ) * (( tex2DNode76 * _BaseTint )).rgb );
			float fresnelNdotV280 = dot( NewNormals220, ase_worldViewDir );
			float fresnelNode280 = ( 0.0 + 1.66 * pow( 1.0 - fresnelNdotV280, 1.46 ) );
			float clampResult282 = clamp( fresnelNode280 , 0.0 , 1.0 );
			float4 lerpResult277 = lerp( _EmissiveColorA , _EmissiveColorB , clampResult282);
			float2 uv_HiotaV3_Material001_Emissive = i.uv_texcoord * _HiotaV3_Material001_Emissive_ST.xy + _HiotaV3_Material001_Emissive_ST.zw;
			float4 tex2DNode267 = tex2D( _HiotaV3_Material001_Emissive, uv_HiotaV3_Material001_Emissive );
			c.rgb = ( float4( ( lerpResult187 * HighlightColor249 * LightColorFalloff227 * pow( temp_output_189_0 , 1.5 ) * saturate( ( ( staticSwitch195 + _specularhighlightsize ) / ( ( 1.0 - temp_output_189_0 ) * _HighlightCellSharpness ) ) ) ) , 0.0 ) + float4( BaseColor253 , 0.0 ) + ( lerpResult277 * _Emissiveintensity * tex2DNode267.r ) ).rgb;
			c.a = 1;
			return c;
		}

		inline void LightingStandardCustomLighting_GI( inout SurfaceOutputCustomLightingCustom s, UnityGIInput data, inout UnityGI gi )
		{
			s.GIData = data;
		}

		void surf( Input i , inout SurfaceOutputCustomLightingCustom o )
		{
			o.SurfInput = i;
			o.Normal = float3(0,0,1);
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf StandardCustomLighting keepalpha fullforwardshadows vertex:vertexDataFunc 

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
				vertexDataFunc( v, customInputData );
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
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				SurfaceOutputCustomLightingCustom o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputCustomLightingCustom, o )
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
Node;AmplifyShaderEditor.CommentaryNode;226;-803.833,-214.5792;Inherit;False;1370.182;280;Comment;5;82;52;170;220;109;Normals;0.5220588,0.6044625,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;109;-753.8331,-138.0697;Float;False;Property;_NormalScale;Normal Scale;13;0;Create;True;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;82;-422.226,-164.5792;Inherit;True;Property;_NormalMap;Normal Map;12;2;[Normal];[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;5c6f1267243da21439d09aa2ee3a3f38;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldNormalVector;52;-103.4431,-159.3391;Inherit;False;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.NormalizeNode;170;138.1924,-160.2827;Inherit;False;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;241;-676.3679,366.5508;Inherit;False;835.6508;341.2334;Comment;4;53;223;54;236;N dot L;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;223;-599.3095,416.5508;Inherit;False;220;NewNormals;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;220;323.3487,-158.8317;Float;False;NewNormals;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;53;-626.368,528.7842;Inherit;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.CommentaryNode;242;1324.039,459.9366;Inherit;False;2744.931;803.0454;Comment;26;253;158;130;235;73;182;76;133;107;159;162;160;214;74;213;215;207;57;60;58;127;59;237;256;265;264;Base Color;1,1,1,1;0;0
Node;AmplifyShaderEditor.DotProductOpNode;54;-302.0764,453.3497;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;59;1374.039,641.3355;Float;False;Property;_BaseCellOffset;Base Cell Offset;3;0;Create;True;0;0;0;False;0;False;0;-0.13;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;237;1374.247,533.2394;Inherit;False;236;NdotL;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;236;-83.71747,456.6653;Float;False;NdotL;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;60;1676.114,631.0218;Float;False;Property;_BaseCellSharpness;Base Cell Sharpness;2;0;Create;True;0;0;0;False;0;False;0.01;0.01;0.01;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LightAttenuation;127;1631.055,790.9418;Inherit;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;58;1657.487,534.5102;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;57;1956.552,537.3538;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0.01;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;215;1944.082,816.0775;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;251;648.5698,-1229.416;Inherit;False;2234.221;738.9581;Comment;18;184;180;249;246;181;177;81;172;175;61;222;239;62;195;174;173;171;261;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;233;1355.396,-187.3015;Inherit;False;828.4254;361.0605;Comment;5;115;118;117;119;225;Indirect Diffuse;1,1,1,1;0;0
Node;AmplifyShaderEditor.WorldSpaceLightPos;207;1626.013,899.7625;Inherit;False;0;3;FLOAT4;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.CommentaryNode;243;1727.822,1561.833;Inherit;False;1926.522;520.1537;Comment;17;98;244;231;193;96;250;94;103;93;92;238;91;90;88;89;221;86;Rim Color;1,1,1,1;0;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;86;1824.853,1773.611;Float;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;213;2134.626,851.7131;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;74;2114.005,542.3831;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;225;1405.396,-42.92128;Inherit;False;220;NewNormals;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;119;1785.008,-137.3016;Float;False;Constant;_Float4;Float 4;20;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;88;2129.955,1708.719;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;61;698.5698,-733.562;Inherit;False;Blinn-Phong Half Vector;-1;;3;91a149ac9d615be429126c95e20753ce;0;0;1;FLOAT3;0
Node;AmplifyShaderEditor.OneMinusNode;214;2324.502,854.6982;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;248;1736.271,-1575.858;Inherit;False;287;165;Comment;1;189;Spec/Smooth;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;184;1659.207,-1057.273;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;89;2072.556,1825.936;Float;False;Property;_RimOffset;Rim Offset;16;0;Create;True;0;0;0;False;0;False;0.6;0.65;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;118;1659.391,58.75889;Float;False;Property;_IndirectDiffuseContribution;Indirect Diffuse Contribution;4;0;Create;True;0;0;0;False;0;False;1;0.183;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;222;762.1332,-605.4583;Inherit;False;220;NewNormals;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;160;2351.156,541.2684;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;162;2155.415,983.4974;Float;False;Property;_ShadowContribution;Shadow Contribution;5;0;Create;True;0;0;0;False;0;False;0.5;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;62;1043.79,-679.1187;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;117;1999.821,-60.9863;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ComponentMaskNode;189;1786.271,-1525.858;Inherit;False;False;False;False;True;1;0;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LightColorNode;107;2626.905,541.7966;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.GetLocalVarNode;239;1003.499,-845.3307;Inherit;False;236;NdotL;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;90;2353.555,1712.936;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;195;1271.122,-784.4333;Float;False;Property;_StaticHighLights;Static HighLights;11;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;All;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;240;-635.8909,943.4141;Inherit;False;717.6841;295.7439;Comment;4;229;228;230;227;Light Falloff;0.9947262,1,0.6176471,1;0;0
Node;AmplifyShaderEditor.OneMinusNode;261;2021.558,-885.707;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;133;3050.939,734.3177;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;256;3276.777,970.2915;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;91;2513.555,1712.936;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;174;1799.042,-648.0834;Float;False;Property;_HighlightCellSharpness;Highlight Cell Sharpness;9;0;Create;True;0;0;0;False;0;False;0.01;0.001;0.001;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;182;3048.637,509.9366;Inherit;False;3;3;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;247;1687.587,-2100.743;Inherit;False;1008.755;365.3326;Comment;5;224;106;188;186;187;Indirect Specular;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;238;2644.923,1611.833;Inherit;False;236;NdotL;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;93;2689.555,1712.936;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LightAttenuation;229;-585.8909,1129.158;Inherit;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;224;1737.587,-1980.103;Inherit;False;220;NewNormals;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;171;1760.558,-788.2134;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LightColorNode;228;-540.1871,993.4141;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;260;2116.472,-654.7482;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;92;2577.555,1840.936;Float;False;Property;_RimPower;Rim Power;15;0;Create;True;0;0;0;False;0;False;0.4;0.01;0.01;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;235;3423.945,954.0895;Inherit;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;172;2108.795,-795.9062;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;180;1898.907,-1053.456;Inherit;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SaturateNode;103;2870.778,1633.693;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;230;-351.8918,1059.575;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;98;2890.947,1874.34;Float;False;Property;_RimColor;Rim Color;14;1;[HDR];Create;True;0;0;0;False;0;False;1,1,1,0;1.292,1.938,2.584,1;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;94;2881.555,1712.936;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;249;2211.476,-1055.947;Float;False;HighlightColor;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;227;-177.207,1056.042;Float;False;LightColorFalloff;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;96;3165.683,1641.71;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;244;3154.779,1926.118;Inherit;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SaturateNode;175;2324.68,-792.1935;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;250;3159.324,1739.908;Inherit;False;249;HighlightColor;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;253;3801.573,744.8395;Float;False;BaseColor;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;231;3153.879,1829.875;Inherit;False;227;LightColorFalloff;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;246;2204.838,-950.8322;Inherit;False;227;LightColorFalloff;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;187;2512.345,-1997.898;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PowerNode;263;2277.378,-1363.768;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;181;2713.791,-1059.016;Inherit;False;5;5;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;173;1405.781,-660.3005;Float;False;Property;_specularhighlightsize;specular highlight size;8;0;Create;True;0;0;0;False;0;False;-0.95;-0.971;-1;-0.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;73;3033.755,1064.272;Float;False;Property;_BaseTint;Base Tint;1;1;[HDR];Create;True;0;0;0;False;0;False;1,1,1,0;1.358491,0.7871431,0.7183642,1;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;188;2149.974,-1822.665;Float;False;Property;_IndirectSpecularContribution;Indirect Specular Contribution;10;0;Create;True;0;0;0;False;0;False;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;130;3289.122,606.0974;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;158;3617.323,789.9739;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;159;2584.489,858.3273;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GradientSampleNode;264;2581.068,1068.534;Inherit;True;2;0;OBJECT;;False;1;FLOAT;0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;186;2188.282,-2050.743;Float;False;Constant;_Float5;Float 5;20;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.IndirectSpecularLight;106;2167.959,-1964.78;Inherit;False;World;3;0;FLOAT3;0,0,0;False;1;FLOAT;0.5;False;2;FLOAT;1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;177;1329.485,-1179.416;Float;False;Property;_SpecularColor;Specular Color;7;1;[HDR];Create;True;0;0;0;False;0;False;1,1,1,1;2.670157,2.657562,2.663224,0.427451;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;81;1225.452,-1011.302;Inherit;True;Property;_SpecularMap;Specular Map;6;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;69bbefecb1732ca41b520544f27a72b1;69bbefecb1732ca41b520544f27a72b1;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;193;3522.963,1638.638;Inherit;False;4;4;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;267;4139.006,1026.176;Inherit;True;Property;_HiotaV3_Material001_Emissive;HiotaV3_Material.001_Emissive;17;0;Create;True;0;0;0;False;0;False;-1;7507b7fd0dc854649911eea706901461;77f8a10a4b79fbf4ca09c438b13dd72f;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;163;4819.006,620.3811;Inherit;False;3;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;269;4708.016,980.3923;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;268;4426.169,842.5921;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;275;3439.228,1310.014;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;76;2907.31,861.5428;Inherit;True;Property;_BaseColorRGBOutlineWidthA;Base Color (RGB) Outline Width (A);0;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;bd4468b9952b99649a5843d3c8eb4750;bd20c59567505064e95b921c42f93f55;True;0;False;gray;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleDivideOpNode;276;3692.228,1349.014;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;3;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;277;4606.199,1464.118;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;270;4165.903,1250.256;Inherit;False;Property;_Emissiveintensity;Emissive intensity;18;0;Create;True;0;0;0;False;0;False;0;1.4;0;20;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;273;4177.095,1433.461;Inherit;False;Property;_EmissiveColorA;Emissive Color A;19;1;[HDR];Create;True;0;0;0;False;0;False;0,0,0,0;0.3372549,1.733333,1.756863,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;278;4210.809,1752.473;Inherit;False;Property;_EmissiveColorB;Emissive Color B;20;1;[HDR];Create;True;0;0;0;False;0;False;0,0,0,0;0.172549,0.7215686,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;221;1777.822,1662.402;Inherit;False;220;NewNormals;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;279;3956.201,1492.53;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;280;3017.425,2168.645;Inherit;True;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1.66;False;3;FLOAT;1.46;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;282;3430.346,2171.886;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;281;2747.426,2281.826;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.GradientNode;265;2242.422,1136.883;Inherit;False;1;2;2;0.3207547,0.3207547,0.3207547,0.4470588;1,1,1,0.5823606;1,0;1,1;0;1;OBJECT;0
Node;AmplifyShaderEditor.IndirectDiffuseLighting;115;1688.455,-41.32622;Inherit;False;World;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;5309.991,551.4377;Float;False;True;-1;6;ASEMaterialInspector;0;0;CustomLighting;Custom/Toon;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;;0;False;;False;0;False;;0;False;;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;True;0;0;False;;0;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.OutlineNode;283;4889.24,312.5876;Inherit;False;2;True;None;0;0;Front;True;True;True;True;0;False;;3;0;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.NormalVertexDataNode;285;4460.24,127.5876;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;286;4466.24,290.5876;Inherit;False;Property;_OutlineWidth;Outline Width;21;0;Create;True;0;0;0;False;0;False;0;0;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;284;4721.24,195.5876;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
WireConnection;82;5;109;0
WireConnection;52;0;82;0
WireConnection;170;0;52;0
WireConnection;220;0;170;0
WireConnection;54;0;223;0
WireConnection;54;1;53;0
WireConnection;236;0;54;0
WireConnection;58;0;237;0
WireConnection;58;1;59;0
WireConnection;57;0;58;0
WireConnection;57;1;60;0
WireConnection;215;0;127;0
WireConnection;213;0;215;0
WireConnection;213;1;207;2
WireConnection;74;0;57;0
WireConnection;88;0;221;0
WireConnection;88;1;86;0
WireConnection;214;0;213;0
WireConnection;184;0;177;0
WireConnection;184;1;81;0
WireConnection;160;0;74;0
WireConnection;160;1;127;0
WireConnection;62;0;61;0
WireConnection;62;1;222;0
WireConnection;117;0;119;0
WireConnection;117;1;115;0
WireConnection;117;2;118;0
WireConnection;189;0;184;0
WireConnection;90;0;88;0
WireConnection;90;1;89;0
WireConnection;195;1;62;0
WireConnection;195;0;239;0
WireConnection;261;0;189;0
WireConnection;133;0;107;1
WireConnection;133;1;264;1
WireConnection;256;0;76;0
WireConnection;256;1;73;0
WireConnection;91;0;90;0
WireConnection;182;0;117;0
WireConnection;182;1;107;2
WireConnection;182;2;214;0
WireConnection;93;0;91;0
WireConnection;171;0;195;0
WireConnection;171;1;173;0
WireConnection;260;0;261;0
WireConnection;260;1;174;0
WireConnection;235;0;256;0
WireConnection;172;0;171;0
WireConnection;172;1;260;0
WireConnection;180;0;184;0
WireConnection;103;0;238;0
WireConnection;230;0;228;1
WireConnection;230;1;229;0
WireConnection;94;0;93;0
WireConnection;94;1;92;0
WireConnection;249;0;180;0
WireConnection;227;0;230;0
WireConnection;96;0;103;0
WireConnection;96;1;94;0
WireConnection;244;0;98;0
WireConnection;175;0;172;0
WireConnection;253;0;158;0
WireConnection;187;0;186;0
WireConnection;187;1;106;0
WireConnection;187;2;188;0
WireConnection;263;0;189;0
WireConnection;181;0;187;0
WireConnection;181;1;249;0
WireConnection;181;2;246;0
WireConnection;181;3;263;0
WireConnection;181;4;175;0
WireConnection;130;0;182;0
WireConnection;130;1;133;0
WireConnection;158;0;130;0
WireConnection;158;1;235;0
WireConnection;159;0;214;0
WireConnection;159;1;160;0
WireConnection;159;2;162;0
WireConnection;264;0;265;0
WireConnection;264;1;159;0
WireConnection;106;0;224;0
WireConnection;106;1;189;0
WireConnection;193;0;96;0
WireConnection;193;1;250;0
WireConnection;193;2;231;0
WireConnection;193;3;244;0
WireConnection;163;0;181;0
WireConnection;163;1;253;0
WireConnection;163;2;269;0
WireConnection;269;0;277;0
WireConnection;269;1;270;0
WireConnection;269;2;267;1
WireConnection;268;1;76;0
WireConnection;268;2;267;1
WireConnection;275;0;76;1
WireConnection;275;1;76;2
WireConnection;275;2;76;3
WireConnection;276;0;275;0
WireConnection;277;0;273;0
WireConnection;277;1;278;0
WireConnection;277;2;282;0
WireConnection;279;0;276;0
WireConnection;280;0;220;0
WireConnection;280;4;281;0
WireConnection;282;0;280;0
WireConnection;115;0;225;0
WireConnection;0;13;163;0
WireConnection;0;11;283;0
WireConnection;283;1;284;0
WireConnection;284;0;285;0
WireConnection;284;1;286;0
ASEEND*/
//CHKSM=CB20D8DE3441D95D33B4C89F12395D33138600AF