// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Portal"
{
	Properties
	{
		_Speed_Rotation("Speed_Rotation", Float) = 0.2
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_Rotate_Power_Upper("Rotate_Power_Upper", Float) = -2
		_Rotating_Power_Lower("Rotating_Power_Lower", Float) = 5
		_Color_Upper("Color_Upper", Color) = (1,0,0.50281,1)
		_Color_Low("Color_Low", Color) = (0,1,0.2262168,0)
		_Emissive_Upper("Emissive_Upper", Float) = 2
		_TextureSampleUpper("Texture Sample Upper", 2D) = "white" {}
		_Scale_Vor_Upper("Scale_Vor_Upper", Float) = 3
		_Angle_Vor_Upper("Angle_Vor_Upper", Float) = 0
		_TextureSampleLower("Texture Sample Lower", 2D) = "white" {}
		_Scale_Voronoi_Low("Scale_Voronoi_Low", Float) = 5
		_Angle_Voronoi_Low("Angle_Voronoi_Low", Float) = 0
		_Emissive_Lower("Emissive_Lower", Float) = 2.5
		_GradiantValues("GradiantValues", Vector) = (0,0,0,0)
		_Color_Fond("Color_Fond", Color) = (0.05098041,0.2247859,0.7372549,1)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "AlphaTest+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 uv_texcoord;
			float3 viewDir;
		};

		uniform float4 _Color_Low;
		uniform sampler2D _TextureSampleLower;
		uniform float _Scale_Voronoi_Low;
		uniform float _Angle_Voronoi_Low;
		uniform float _Rotating_Power_Lower;
		uniform float _Speed_Rotation;
		uniform float _Emissive_Lower;
		uniform float4 _Color_Fond;
		uniform float4 _Color_Upper;
		uniform sampler2D _TextureSampleUpper;
		uniform float _Rotate_Power_Upper;
		uniform float _Scale_Vor_Upper;
		uniform float _Angle_Vor_Upper;
		uniform float _Emissive_Upper;
		SamplerState sampler_TextureSampleUpper;
		uniform float2 _GradiantValues;
		uniform float _Cutoff = 0.5;


		float2 voronoihash18( float2 p )
		{
			
			p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
			return frac( sin( p ) *43758.5453);
		}


		float voronoi18( float2 v, float time, inout float2 id, inout float2 mr, float smoothness )
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
			 		float2 o = voronoihash18( n + g );
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


		float2 voronoihash64( float2 p )
		{
			
			p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
			return frac( sin( p ) *43758.5453);
		}


		float voronoi64( float2 v, float time, inout float2 id, inout float2 mr, float smoothness )
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
			 		float2 o = voronoihash64( n + g );
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


		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_TexCoord37 = i.uv_texcoord * float2( 6,1 );
			float2 Offset40 = ( ( 0.2 - 1 ) * i.viewDir.xy * 1.0 ) + uv_TexCoord37;
			float4 tex2DNode21 = tex2D( _TextureSampleLower, Offset40 );
			float time18 = _Angle_Voronoi_Low;
			float2 center45_g1 = float2( 0.5,0.5 );
			float2 delta6_g1 = ( i.uv_texcoord - center45_g1 );
			float angle10_g1 = ( length( delta6_g1 ) * _Rotating_Power_Lower );
			float x23_g1 = ( ( cos( angle10_g1 ) * delta6_g1.x ) - ( sin( angle10_g1 ) * delta6_g1.y ) );
			float2 break40_g1 = center45_g1;
			float mulTime9 = _Time.y * _Speed_Rotation;
			float2 temp_cast_0 = (mulTime9).xx;
			float2 break41_g1 = temp_cast_0;
			float y35_g1 = ( ( sin( angle10_g1 ) * delta6_g1.x ) + ( cos( angle10_g1 ) * delta6_g1.y ) );
			float2 appendResult44_g1 = (float2(( x23_g1 + break40_g1.x + break41_g1.x ) , ( break40_g1.y + break41_g1.y + y35_g1 )));
			float2 coords18 = appendResult44_g1 * _Scale_Voronoi_Low;
			float2 id18 = 0;
			float2 uv18 = 0;
			float voroi18 = voronoi18( coords18, time18, id18, uv18, 0 );
			float4 temp_output_31_0 = ( ( _Color_Low * ( tex2DNode21 * voroi18 ) ) * ( _Emissive_Lower + _SinTime.w ) );
			float4 lerpResult80 = lerp( temp_output_31_0 , _Color_Fond , tex2DNode21);
			o.Albedo = lerpResult80.rgb;
			float2 Offset83 = ( ( 0.5 - 1 ) * i.viewDir.xy * 2.0 ) + uv_TexCoord37;
			float2 center45_g2 = float2( 0.5,0.5 );
			float2 delta6_g2 = ( i.uv_texcoord - center45_g2 );
			float angle10_g2 = ( length( delta6_g2 ) * _Rotate_Power_Upper );
			float x23_g2 = ( ( cos( angle10_g2 ) * delta6_g2.x ) - ( sin( angle10_g2 ) * delta6_g2.y ) );
			float2 break40_g2 = center45_g2;
			float2 temp_cast_2 = (mulTime9).xx;
			float2 break41_g2 = temp_cast_2;
			float y35_g2 = ( ( sin( angle10_g2 ) * delta6_g2.x ) + ( cos( angle10_g2 ) * delta6_g2.y ) );
			float2 appendResult44_g2 = (float2(( x23_g2 + break40_g2.x + break41_g2.x ) , ( break40_g2.y + break41_g2.y + y35_g2 )));
			float2 temp_output_63_0 = appendResult44_g2;
			float4 tex2DNode65 = tex2D( _TextureSampleUpper, ( Offset83 + temp_output_63_0 ) );
			float time64 = _Angle_Vor_Upper;
			float2 coords64 = temp_output_63_0 * _Scale_Vor_Upper;
			float2 id64 = 0;
			float2 uv64 = 0;
			float voroi64 = voronoi64( coords64, time64, id64, uv64, 0 );
			float4 lerpResult79 = lerp( ( ( _Color_Upper * ( ( 1.0 - tex2DNode65 ) * voroi64 ) ) * ( _Emissive_Upper + _SinTime.w ) ) , temp_output_31_0 , tex2DNode65.a);
			o.Emission = lerpResult79.rgb;
			o.Alpha = 1;
			float2 uv_TexCoord50 = i.uv_texcoord * float2( 1,2.5 );
			float smoothstepResult55 = smoothstep( _GradiantValues.x , _GradiantValues.y , uv_TexCoord50.y);
			clip( smoothstepResult55 - _Cutoff );
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
0;0;1920;1019;1915.113;579.7894;1.765122;True;True
Node;AmplifyShaderEditor.RangedFloatNode;10;-2298.014,379.3234;Inherit;False;Property;_Speed_Rotation;Speed_Rotation;0;0;Create;True;0;0;False;0;False;0.2;0.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;38;-2791.666,502.7771;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.TextureCoordinatesNode;37;-2821.97,312.7492;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;6,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;78;-2085.467,931.945;Inherit;False;Property;_Rotate_Power_Upper;Rotate_Power_Upper;2;0;Create;True;0;0;False;0;False;-2;-2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;9;-2067.95,404.3253;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;63;-1777.482,854.0443;Inherit;True;Twirl;-1;;2;90936742ac32db8449cd21ab6dd337c8;0;4;1;FLOAT2;0,0;False;2;FLOAT2;0,0;False;3;FLOAT;0;False;4;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ParallaxMappingNode;83;-2163.631,586.538;Inherit;False;Normal;4;0;FLOAT2;0,0;False;1;FLOAT;0.5;False;2;FLOAT;2;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;102;-1460.305,713.5936;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;17;-2075.972,72.29086;Inherit;False;Property;_Rotating_Power_Lower;Rotating_Power_Lower;3;0;Create;True;0;0;False;0;False;5;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;65;-1323.91,686.1537;Inherit;True;Property;_TextureSampleUpper;Texture Sample Upper;7;0;Create;True;0;0;False;0;False;-1;None;0000000000000000f000000000000000;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ParallaxMappingNode;40;-2368.859,-32.179;Inherit;False;Normal;4;0;FLOAT2;0,0;False;1;FLOAT;0.2;False;2;FLOAT;1;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;20;-1570.752,331.1661;Inherit;False;Property;_Scale_Voronoi_Low;Scale_Voronoi_Low;11;0;Create;True;0;0;False;0;False;5;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;62;-1624.436,1255.721;Inherit;False;Property;_Scale_Vor_Upper;Scale_Vor_Upper;8;0;Create;True;0;0;False;0;False;3;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;61;-1643.572,1143.32;Inherit;False;Property;_Angle_Vor_Upper;Angle_Vor_Upper;9;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;16;-1757.876,29.07684;Inherit;True;Twirl;-1;;1;90936742ac32db8449cd21ab6dd337c8;0;4;1;FLOAT2;0,0;False;2;FLOAT2;0,0;False;3;FLOAT;0;False;4;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;19;-1557.752,242.166;Inherit;False;Property;_Angle_Voronoi_Low;Angle_Voronoi_Low;12;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.VoronoiNode;64;-1353.268,1114.326;Inherit;True;0;0;1;0;1;False;1;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;0.8;False;2;FLOAT;3.12;False;3;FLOAT;0;False;3;FLOAT;0;FLOAT2;1;FLOAT2;2
Node;AmplifyShaderEditor.SamplerNode;21;-1319.571,-88.12006;Inherit;True;Property;_TextureSampleLower;Texture Sample Lower;10;0;Create;True;0;0;False;0;False;-1;None;0000000000000000f000000000000000;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VoronoiNode;18;-1313.387,235.2909;Inherit;True;0;0;1;0;1;False;1;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;0.8;False;2;FLOAT;3.12;False;3;FLOAT;0;False;3;FLOAT;0;FLOAT2;1;FLOAT2;2
Node;AmplifyShaderEditor.OneMinusNode;101;-987.1108,785.3177;Inherit;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;68;-764.6699,1020.701;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;12;-819.2356,-241.2789;Inherit;False;Property;_Color_Low;Color_Low;5;0;Create;True;0;0;False;0;False;0,1,0.2262168,0;0,1,0.7064571,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SinTimeNode;36;-517.144,361.3165;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;69;-735.4052,753.6082;Inherit;False;Property;_Color_Upper;Color_Upper;4;0;Create;True;0;0;False;0;False;1,0,0.50281,1;0.3005765,0.2180046,0.6698113,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;66;-300.9046,646.9246;Inherit;False;Property;_Emissive_Upper;Emissive_Upper;6;0;Create;True;0;0;False;0;False;2;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;-885.5715,108.88;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;30;-488.8496,248.9293;Inherit;False;Property;_Emissive_Lower;Emissive_Lower;13;0;Create;True;0;0;False;0;False;2.5;7;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;23;-479.6056,-44.23511;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;70;-12.41002,641.1172;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;73;-422.9824,774.6716;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;35;-202.9075,210.7757;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;45;353.6295,1004.419;Inherit;False;634.7377;420.1881;Opacity;3;55;51;50;;1,1,1,1;0;0
Node;AmplifyShaderEditor.ColorNode;100;63.98793,268.3783;Inherit;False;Property;_Color_Fond;Color_Fond;15;0;Create;True;0;0;False;0;False;0.05098041,0.2247859,0.7372549,1;0,0.09055519,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;112.2177,-100.0924;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;74;224.3316,498.4089;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;50;403.6295,1054.42;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,2.5;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;51;408.3672,1250.606;Inherit;False;Property;_GradiantValues;GradiantValues;14;0;Create;True;0;0;False;0;False;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;91;-103.2661,-928.0886;Inherit;False;Constant;_Float3;Float 3;15;0;Create;True;0;0;False;0;False;2.5;2.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.VoronoiNode;87;-1171.23,-1013.411;Inherit;True;0;0;1;0;1;False;1;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;0.8;False;2;FLOAT;3.12;False;3;FLOAT;0;False;3;FLOAT;0;FLOAT2;1;FLOAT2;2
Node;AmplifyShaderEditor.SmoothstepOpNode;55;734.3663,1170.607;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;2.56;False;2;FLOAT;1.87;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;99;58.68018,-1399.603;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;90;-693.1306,-839.9779;Inherit;False;Constant;_Color0;Color 0;14;0;Create;True;0;0;False;0;False;0.3182054,0,1,0;0.3182054,0,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;95;314.8306,-1015.176;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;85;-1428.595,-917.5358;Inherit;False;Constant;_Float2;Float 2;12;0;Create;True;0;0;False;0;False;5;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;93;81.18695,-925.8027;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;88;-743.415,-1139.822;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;89;-425.2159,-1084.452;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;86;-1177.415,-1336.822;Inherit;True;Property;_TextureSampleLower1;Texture Sample Lower;11;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Instance;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;84;-1415.595,-1006.536;Inherit;False;Constant;_Float1;Float 1;13;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;98;-449.0962,-1493.543;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;80;519.3304,54.5245;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SinTimeNode;92;-201.7511,-845.5696;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;96;596.2385,-1180.352;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;94;-193.187,-1060.745;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;97;-744.679,-1499.081;Inherit;False;Constant;_Color1;Color 1;5;0;Create;True;0;0;False;0;False;0,1,0.2262168,0;0,1,0.2262168,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;79;601.9045,352.7407;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1024.32,136.3689;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Portal;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;Opaque;;AlphaTest;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;1;False;-1;1;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;9;0;10;0
WireConnection;63;3;78;0
WireConnection;63;4;9;0
WireConnection;83;0;37;0
WireConnection;83;3;38;0
WireConnection;102;0;83;0
WireConnection;102;1;63;0
WireConnection;65;1;102;0
WireConnection;40;0;37;0
WireConnection;40;3;38;0
WireConnection;16;3;17;0
WireConnection;16;4;9;0
WireConnection;64;0;63;0
WireConnection;64;1;61;0
WireConnection;64;2;62;0
WireConnection;21;1;40;0
WireConnection;18;0;16;0
WireConnection;18;1;19;0
WireConnection;18;2;20;0
WireConnection;101;0;65;0
WireConnection;68;0;101;0
WireConnection;68;1;64;0
WireConnection;22;0;21;0
WireConnection;22;1;18;0
WireConnection;23;0;12;0
WireConnection;23;1;22;0
WireConnection;70;0;66;0
WireConnection;70;1;36;4
WireConnection;73;0;69;0
WireConnection;73;1;68;0
WireConnection;35;0;30;0
WireConnection;35;1;36;4
WireConnection;31;0;23;0
WireConnection;31;1;35;0
WireConnection;74;0;73;0
WireConnection;74;1;70;0
WireConnection;87;1;84;0
WireConnection;87;2;85;0
WireConnection;55;0;50;2
WireConnection;55;1;51;1
WireConnection;55;2;51;2
WireConnection;99;0;98;0
WireConnection;99;1;94;0
WireConnection;95;0;98;0
WireConnection;95;1;93;0
WireConnection;93;0;91;0
WireConnection;93;1;92;4
WireConnection;88;0;86;0
WireConnection;88;1;87;0
WireConnection;89;0;88;0
WireConnection;98;0;97;0
WireConnection;98;1;88;0
WireConnection;80;0;31;0
WireConnection;80;1;100;0
WireConnection;80;2;21;0
WireConnection;96;0;99;0
WireConnection;96;1;95;0
WireConnection;96;2;86;1
WireConnection;94;0;89;0
WireConnection;94;1;90;0
WireConnection;79;0;74;0
WireConnection;79;1;31;0
WireConnection;79;2;65;4
WireConnection;0;0;80;0
WireConnection;0;2;79;0
WireConnection;0;10;55;0
ASEEND*/
//CHKSM=053FBC80EA51A3C10C79F738E820E59D089D9824