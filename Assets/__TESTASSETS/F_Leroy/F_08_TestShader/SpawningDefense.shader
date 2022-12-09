// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SpawningDefense"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_ColorVariety("ColorVariety", Range( 0 , 1)) = 1
		[Header(Refraction)]
		_ChromaticAberration("Chromatic Aberration", Range( 0 , 0.3)) = 0.1
		_OpacityValue("OpacityValue", Range( 0 , 1)) = 0.4057682
		_Smoothness("Smoothness", Range( 0 , 1)) = 1
		_PushPull("PushPull", Float) = -0.1
		_Refraction_Value("Refraction_Value", Range( 0 , 1)) = 0.9208435
		_TextureScale("TextureScale", Range( 0 , 100)) = 48.02579
		_TestEmissive("TestEmissive", Range( 0 , 2)) = 0
		_DefenceField_New_Texture_1001_BaseMap_ACESACEScg("DefenceField_New_Texture_1001_BaseMap_ACES - ACEScg", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		GrabPass{ }
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#pragma multi_compile _ALPHAPREMULTIPLY_ON
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
			float3 worldPos;
			float2 uv_texcoord;
			float4 screenPos;
		};

		uniform float _PushPull;
		uniform float _TextureScale;
		uniform float _ColorVariety;
		uniform float _TestEmissive;
		uniform float _Smoothness;
		uniform float _OpacityValue;
		uniform sampler2D _DefenceField_New_Texture_1001_BaseMap_ACESACEScg;
		uniform float4 _DefenceField_New_Texture_1001_BaseMap_ACESACEScg_ST;
		uniform sampler2D _GrabTexture;
		uniform float _ChromaticAberration;
		uniform float _Refraction_Value;
		uniform float _Cutoff = 0.5;


		float2 voronoihash3( float2 p )
		{
			
			p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
			return frac( sin( p ) *43758.5453);
		}


		float voronoi3( float2 v, float time, inout float2 id, inout float2 mr, float smoothness )
		{
			float2 n = floor( v );
			float2 f = frac( v );
			float F1 = 8.0;
			float F2 = 8.0; float2 mg = 0;
			for ( int j = -1; j <= 1; j++ )
			{
				for ( int i = -1; i <= 1; i++ )
			 	{
			 		float2 g = float2( i, j );
			 		float2 o = voronoihash3( n + g );
					o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = f - g - o;
					float d = 0.5 * dot( r, r );
			 		if( d<F1 ) {
			 			F2 = F1;
			 			F1 = d; mg = g; mr = r; id = o;
			 		} else if( d<F2 ) {
			 			F2 = d;
			 		}
			 	}
			}
			return F1;
		}


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
			float temp_output_20_0 = ( v.texcoord.xy.y - (-0.2 + (_SinTime.w - 0.0) * (1.0 - -0.2) / (1.0 - 0.0)) );
			float temp_output_21_0 = ( temp_output_20_0 - -0.6 );
			float3 ase_vertexNormal = v.normal.xyz;
			v.vertex.xyz += ( ( temp_output_21_0 * ase_vertexNormal ) * _PushPull );
			v.vertex.w = 1;
		}

		inline float4 Refraction( Input i, SurfaceOutputStandard o, float indexOfRefraction, float chomaticAberration ) {
			float3 worldNormal = o.Normal;
			float4 screenPos = i.screenPos;
			#if UNITY_UV_STARTS_AT_TOP
				float scale = -1.0;
			#else
				float scale = 1.0;
			#endif
			float halfPosW = screenPos.w * 0.5;
			screenPos.y = ( screenPos.y - halfPosW ) * _ProjectionParams.x * scale + halfPosW;
			#if SHADER_API_D3D9 || SHADER_API_D3D11
				screenPos.w += 0.00000000001;
			#endif
			float2 projScreenPos = ( screenPos / screenPos.w ).xy;
			float3 worldViewDir = normalize( UnityWorldSpaceViewDir( i.worldPos ) );
			float3 refractionOffset = ( indexOfRefraction - 1.0 ) * mul( UNITY_MATRIX_V, float4( worldNormal, 0.0 ) ) * ( 1.0 - dot( worldNormal, worldViewDir ) );
			float2 cameraRefraction = float2( refractionOffset.x, refractionOffset.y );
			float4 redAlpha = tex2D( _GrabTexture, ( projScreenPos + cameraRefraction ) );
			float green = tex2D( _GrabTexture, ( projScreenPos + ( cameraRefraction * ( 1.0 - chomaticAberration ) ) ) ).g;
			float blue = tex2D( _GrabTexture, ( projScreenPos + ( cameraRefraction * ( 1.0 + chomaticAberration ) ) ) ).b;
			return float4( redAlpha.r, green, blue, redAlpha.a );
		}

		void RefractionF( Input i, SurfaceOutputStandard o, inout half4 color )
		{
			#ifdef UNITY_PASS_FORWARDBASE
			color.rgb = color.rgb + Refraction( i, o, _Refraction_Value, _ChromaticAberration ) * ( 1 - color.a );
			color.a = 1;
			#endif
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Normal = float3(0,0,1);
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 ase_worldPos = i.worldPos;
			float3 temp_output_16_0_g3 = ( ase_worldPos * 100.0 );
			float3 crossY18_g3 = cross( ase_worldNormal , ddy( temp_output_16_0_g3 ) );
			float3 worldDerivativeX2_g3 = ddx( temp_output_16_0_g3 );
			float dotResult6_g3 = dot( crossY18_g3 , worldDerivativeX2_g3 );
			float crossYDotWorldDerivX34_g3 = abs( dotResult6_g3 );
			float time3 = 5.0;
			float2 coords3 = i.uv_texcoord * _TextureScale;
			float2 id3 = 0;
			float2 uv3 = 0;
			float voroi3 = voronoi3( coords3, time3, id3, uv3, 0 );
			float2 temp_cast_0 = (_ColorVariety).xx;
			float2 temp_output_5_0 = pow( id3 , temp_cast_0 );
			float temp_output_20_0_g3 = temp_output_5_0.x;
			float3 crossX19_g3 = cross( ase_worldNormal , worldDerivativeX2_g3 );
			float3 break29_g3 = ( sign( crossYDotWorldDerivX34_g3 ) * ( ( ddx( temp_output_20_0_g3 ) * crossY18_g3 ) + ( ddy( temp_output_20_0_g3 ) * crossX19_g3 ) ) );
			float3 appendResult30_g3 = (float3(break29_g3.x , -break29_g3.y , break29_g3.z));
			float3 normalizeResult39_g3 = normalize( ( ( crossYDotWorldDerivX34_g3 * ase_worldNormal ) - appendResult30_g3 ) );
			float3 ase_worldTangent = WorldNormalVector( i, float3( 1, 0, 0 ) );
			float3 ase_worldBitangent = WorldNormalVector( i, float3( 0, 1, 0 ) );
			float3x3 ase_worldToTangent = float3x3( ase_worldTangent, ase_worldBitangent, ase_worldNormal );
			float3 worldToTangentDir42_g3 = mul( ase_worldToTangent, normalizeResult39_g3);
			o.Normal = worldToTangentDir42_g3;
			Gradient gradient4 = NewGradient( 1, 8, 2, float4( 0.7923233, 0.514151, 1, 0.1250019 ), float4( 0.6714391, 0.495283, 1, 0.2500038 ), float4( 0.5789307, 0.514151, 1, 0.3750057 ), float4( 0.4858491, 0.506012, 1, 0.5000076 ), float4( 0.495283, 0.6397706, 1, 0.6249943 ), float4( 0.504717, 0.7921752, 1, 0.7499962 ), float4( 0.4392157, 0.7952045, 1, 0.8749981 ), float4( 0.4666667, 0.9163182, 1, 1 ), float2( 0.5882353, 0 ), float2( 0.5882353, 1 ), 0, 0, 0, 0, 0, 0 );
			o.Albedo = ( SampleGradient( gradient4, temp_output_5_0.x ) * float4( 0,0,0,0 ) ).rgb;
			o.Emission = ( SampleGradient( gradient4, temp_output_5_0.x ) * _TestEmissive ).rgb;
			o.Smoothness = _Smoothness;
			o.Alpha = _OpacityValue;
			float2 uv_DefenceField_New_Texture_1001_BaseMap_ACESACEScg = i.uv_texcoord * _DefenceField_New_Texture_1001_BaseMap_ACESACEScg_ST.xy + _DefenceField_New_Texture_1001_BaseMap_ACESACEScg_ST.zw;
			float temp_output_20_0 = ( i.uv_texcoord.y - (-0.2 + (_SinTime.w - 0.0) * (1.0 - -0.2) / (1.0 - 0.0)) );
			float temp_output_21_0 = ( temp_output_20_0 - -0.6 );
			clip( ( tex2D( _DefenceField_New_Texture_1001_BaseMap_ACESACEScg, uv_DefenceField_New_Texture_1001_BaseMap_ACESACEScg ) * ( 1.0 - temp_output_21_0 ) ).r - _Cutoff );
			o.Normal = o.Normal + 0.00001 * i.screenPos * i.worldPos;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha finalcolor:RefractionF fullforwardshadows exclude_path:deferred vertex:vertexDataFunc 

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
				float4 screenPos : TEXCOORD2;
				float4 tSpace0 : TEXCOORD3;
				float4 tSpace1 : TEXCOORD4;
				float4 tSpace2 : TEXCOORD5;
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
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
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
1289;214;1318;479;3690.563;-68.09979;1.344509;True;True
Node;AmplifyShaderEditor.SinTimeNode;44;-2494.251,250.0522;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;1;-1818.856,-458.6397;Inherit;False;Property;_TextureScale;TextureScale;8;0;Create;True;0;0;False;0;False;48.02579;21.4;0;100;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;19;-2250.798,453.0268;Inherit;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-0.2;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;18;-2312.285,708.6089;Inherit;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;2;-1749.063,-226.7926;Inherit;False;Property;_ColorVariety;ColorVariety;1;0;Create;True;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.VoronoiNode;3;-1466.977,-500.4621;Inherit;True;0;0;1;0;1;False;1;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;5;False;2;FLOAT;15;False;3;FLOAT;0;False;3;FLOAT;0;FLOAT2;1;FLOAT2;2
Node;AmplifyShaderEditor.SimpleSubtractOpNode;20;-1955.386,755.7089;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;22;-1786.371,1070.137;Inherit;True;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GradientNode;4;-1097.473,-413.3276;Inherit;False;1;8;2;0.7923233,0.514151,1,0.1250019;0.6714391,0.495283,1,0.2500038;0.5789307,0.514151,1,0.3750057;0.4858491,0.506012,1,0.5000076;0.495283,0.6397706,1,0.6249943;0.504717,0.7921752,1,0.7499962;0.4392157,0.7952045,1,0.8749981;0.4666667,0.9163182,1,1;0.5882353,0;0.5882353,1;0;1;OBJECT;0
Node;AmplifyShaderEditor.PowerNode;5;-1159.303,-287.017;Inherit;False;False;2;0;FLOAT2;0,0;False;1;FLOAT;1.21;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;21;-1716.386,756.2253;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;-0.6;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;29;-1438.491,750.5628;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;23;-1464.869,1047.202;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;24;-1411.666,1283.235;Inherit;False;Property;_PushPull;PushPull;6;0;Create;True;0;0;False;0;False;-0.1;-0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GradientSampleNode;6;-855.6723,-409.4277;Inherit;True;2;0;OBJECT;;False;1;FLOAT;0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;7;-982.9085,-75.03521;Inherit;False;Property;_TestEmissive;TestEmissive;9;0;Create;True;0;0;False;0;False;0;1;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;33;-1365.744,390.2212;Inherit;True;Property;_DefenceField_New_Texture_1001_BaseMap_ACESACEScg;DefenceField_New_Texture_1001_BaseMap_ACES - ACEScg;10;0;Create;True;0;0;False;0;False;-1;b524ef01a4ef7e540a5ee973ca3149d3;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;13;-669.7767,121.347;Inherit;False;Property;_Refraction_Value;Refraction_Value;7;0;Create;True;0;0;False;0;False;0.9208435;0.922;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;32;-217.6365,-176.3459;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;-1069.093,1051.567;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-671.2801,39.8535;Inherit;False;Property;_Smoothness;Smoothness;5;0;Create;True;0;0;False;0;False;1;0.809;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;14;-671.217,197.6033;Inherit;False;Property;_OpacityValue;OpacityValue;4;0;Create;True;0;0;False;0;False;0.4057682;0.823;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-513.3848,-118.1081;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;16;-878.6512,-215.4203;Inherit;False;Normal From Height;-1;;3;1942fe2c5f1a1f94881a33d532e4afeb;0;1;20;FLOAT;0;False;2;FLOAT3;40;FLOAT3;0
Node;AmplifyShaderEditor.SaturateNode;31;-1699.273,658.9616;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;-849.6048,596.8793;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;SpawningDefense;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;TransparentCutout;;Geometry;ForwardOnly;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;2;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.3;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;19;0;44;4
WireConnection;3;2;1;0
WireConnection;20;0;18;2
WireConnection;20;1;19;0
WireConnection;5;0;3;1
WireConnection;5;1;2;0
WireConnection;21;0;20;0
WireConnection;29;0;21;0
WireConnection;23;0;21;0
WireConnection;23;1;22;0
WireConnection;6;0;4;0
WireConnection;6;1;5;0
WireConnection;32;0;6;0
WireConnection;26;0;23;0
WireConnection;26;1;24;0
WireConnection;10;0;6;0
WireConnection;10;1;7;0
WireConnection;16;20;5;0
WireConnection;31;0;20;0
WireConnection;25;0;33;0
WireConnection;25;1;29;0
WireConnection;0;0;32;0
WireConnection;0;1;16;40
WireConnection;0;2;10;0
WireConnection;0;4;9;0
WireConnection;0;8;13;0
WireConnection;0;9;14;0
WireConnection;0;10;25;0
WireConnection;0;11;26;0
ASEEND*/
//CHKSM=E90A4D3FB6ECF1ACE2FF3594FC23D8668BB8F396