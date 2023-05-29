// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Fog_Nuage"
{
	Properties
	{
		_EdgeLength ( "Edge length", Range( 2, 50 ) ) = 2
		_TessPhongStrength( "Phong Tess Strength", Range( 0, 1 ) ) = 1
		_Distance("Distance", Float) = 0
		_displacemenbtamout("displacemenbt amout", Float) = 0
		[HDR]_Color1("Color 1", Color) = (0,0,0,0)
		[HDR]_Color2("Color 2", Color) = (0,0,0,0)
		_NoiseDisplacementTxt("NoiseDisplacementTxt", 2D) = "white" {}
		_DisplacementMapscale("Displacement Map scale", Float) = 0
		_VoronoiTxt("VoronoiTxt", 2D) = "white" {}
		_VoronoidTilling("VoronoidTilling", Float) = 0
		_VoronoidSpeed("VoronoidSpeed", Vector) = (0,0,0,0)
		_Speed("Speed", Vector) = (0,0,0,0)
		_RenderTexture("RenderTexture", 2D) = "white" {}
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

		uniform sampler2D _NoiseDisplacementTxt;
		uniform float2 _Speed;
		uniform float _DisplacementMapscale;
		uniform sampler2D _VoronoiTxt;
		uniform float2 _VoronoidSpeed;
		uniform float _VoronoidTilling;
		uniform sampler2D _RenderTexture;
		uniform float4 _RenderTexture_ST;
		uniform float _displacemenbtamout;
		uniform float4 _Color1;
		uniform float4 _Color2;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _Distance;
		uniform float _EdgeLength;
		uniform float _TessPhongStrength;

		float4 tessFunction( appdata_full v0, appdata_full v1, appdata_full v2 )
		{
			return UnityEdgeLengthBasedTess (v0.vertex, v1.vertex, v2.vertex, _EdgeLength);
		}

		void vertexDataFunc( inout appdata_full v )
		{
			float2 temp_cast_0 = (_DisplacementMapscale).xx;
			float2 uv_TexCoord13 = v.texcoord.xy * temp_cast_0;
			float2 panner15 = ( 1.0 * _Time.y * ( _Speed * 0.01 ) + uv_TexCoord13);
			float2 temp_cast_1 = (_VoronoidTilling).xx;
			float2 uv_TexCoord57 = v.texcoord.xy * temp_cast_1;
			float2 panner59 = ( 1.0 * _Time.y * ( _VoronoidSpeed * 0.1 ) + uv_TexCoord57);
			float4 temp_output_58_0 = ( 1.0 - tex2Dlod( _VoronoiTxt, float4( panner59, 0, 0.0) ) );
			float2 uv_RenderTexture = v.texcoord * _RenderTexture_ST.xy + _RenderTexture_ST.zw;
			float4 temp_output_16_0 = ( ( tex2Dlod( _NoiseDisplacementTxt, float4( panner15, 0, 0.0) ) * temp_output_58_0 * temp_output_58_0 ) * ( 1.0 - tex2Dlod( _RenderTexture, float4( uv_RenderTexture, 0, 0.0) ) ) );
			v.vertex.xyz += ( temp_output_16_0 * float4( float3(0,0,1) , 0.0 ) * _displacemenbtamout ).rgb;
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 temp_cast_0 = (_DisplacementMapscale).xx;
			float2 uv_TexCoord13 = i.uv_texcoord * temp_cast_0;
			float2 panner15 = ( 1.0 * _Time.y * ( _Speed * 0.01 ) + uv_TexCoord13);
			float2 temp_cast_1 = (_VoronoidTilling).xx;
			float2 uv_TexCoord57 = i.uv_texcoord * temp_cast_1;
			float2 panner59 = ( 1.0 * _Time.y * ( _VoronoidSpeed * 0.1 ) + uv_TexCoord57);
			float4 temp_output_58_0 = ( 1.0 - tex2D( _VoronoiTxt, panner59 ) );
			float2 uv_RenderTexture = i.uv_texcoord * _RenderTexture_ST.xy + _RenderTexture_ST.zw;
			float4 temp_output_16_0 = ( ( tex2D( _NoiseDisplacementTxt, panner15 ) * temp_output_58_0 * temp_output_58_0 ) * ( 1.0 - tex2D( _RenderTexture, uv_RenderTexture ) ) );
			float4 lerpResult47 = lerp( _Color1 , _Color2 , temp_output_16_0);
			o.Emission = lerpResult47.rgb;
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth1 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float distanceDepth1 = abs( ( screenDepth1 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _Distance ) );
			o.Alpha = saturate( distanceDepth1 );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows vertex:vertexDataFunc tessellate:tessFunction tessphong:_TessPhongStrength 

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
-1920;2;1920;1019;568.1277;-301.7526;1;True;False
Node;AmplifyShaderEditor.CommentaryNode;31;-1463.629,648.7042;Inherit;False;2159.928;535.5914;;3;30;29;27;vertex offset;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;62;-1562.385,235.268;Inherit;False;1107.999;394;Panning Texture 2 with different speed;6;56;60;61;57;59;55;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;27;-1413.629,823.5648;Inherit;False;765.0779;343.7308;panning texture;5;13;26;18;21;15;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;56;-1512.385,290.2681;Inherit;False;Property;_VoronoidTilling;VoronoidTilling;12;0;Create;True;0;0;False;0;False;0;3.97;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;60;-1317.385,454.2682;Inherit;False;Property;_VoronoidSpeed;VoronoidSpeed;13;0;Create;True;0;0;False;0;False;0,0;1.34,0.43;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.ScaleNode;61;-1088.385,481.2682;Inherit;False;0.1;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;57;-1278.385,285.2681;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;26;-1363.629,885.1858;Inherit;False;Property;_DisplacementMapscale;Displacement Map scale;10;0;Create;True;0;0;False;0;False;0;3.65;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;18;-1321.5,1003.296;Inherit;False;Property;_Speed;Speed;15;0;Create;True;0;0;False;0;False;0,0;0.1,0.1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;13;-1109.635,873.5648;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;59;-964.3858,369.2681;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ScaleNode;21;-1103.08,1003.558;Inherit;False;0.01;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;37;-791.0128,1213.73;Inherit;False;904.313;312.6136;RenderTexture Painted with Particule system in black on the texture;3;35;36;33;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;55;-774.3859,399.2681;Inherit;True;Property;_VoronoiTxt;VoronoiTxt;11;0;Create;True;0;0;False;0;False;-1;c773c030703f55247ae062c1029bc681;c773c030703f55247ae062c1029bc681;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;29;-607.7405,698.7042;Inherit;False;664.665;459.3485;multipling mask and texture for dispalcement. Square blurred for edges smooth;3;25;22;28;;1,1,1,1;0;0
Node;AmplifyShaderEditor.PannerNode;15;-853.5523,936.134;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0.16;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;35;-741.0128,1263.73;Inherit;True;Property;_RenderTexture;RenderTexture;16;0;Create;True;0;0;False;0;False;None;da053593210106347ba93a31537d2664;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SamplerNode;36;-410.0128,1264.73;Inherit;True;Property;_TextureSample1;Texture Sample 1;12;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;32;-438.5583,325.7089;Inherit;False;764.2364;188.7541;depth fade;2;1;2;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;25;-559.6956,933.0527;Inherit;True;Property;_NoiseDisplacementTxt;NoiseDisplacementTxt;9;0;Create;True;0;0;False;0;False;-1;None;8d1efb6804f894441a74456065561c01;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;58;-446.4859,570.5679;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;-178.0754,846.8604;Inherit;True;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;2;-388.5584,398.463;Inherit;False;Property;_Distance;Distance;5;0;Create;True;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;33;-84.69968,1272.344;Inherit;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;30;79.05725,713.0983;Inherit;False;566.2411;441.965;displacement force multiping vector by Z axis;3;24;23;16;;1,1,1,1;0;0
Node;AmplifyShaderEditor.ColorNode;48;499.3108,126.3285;Inherit;False;Property;_Color2;Color 2;8;1;[HDR];Create;True;0;0;False;0;False;0,0,0,0;0,0.04749833,0.4881921,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;132.1986,820.298;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DepthFade;1;-92.59818,375.7089;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;20;529.7088,-68.25777;Inherit;False;Property;_Color1;Color 1;7;1;[HDR];Create;True;0;0;False;0;False;0,0,0,0;0.07075471,0.710365,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector3Node;23;324.398,933.7632;Inherit;False;Constant;_Vector0;Vector 0;8;0;Create;True;0;0;False;0;False;0,0,1;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;24;272.3454,1082.844;Inherit;False;Property;_displacemenbtamout;displacemenbt amout;6;0;Create;True;0;0;False;0;False;0;20.9;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;47;849.81,93.1285;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;46;702.3851,951.8546;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.PosVertexDataNode;63;-319.1176,166.805;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;8;773.9779,374.2408;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;22;-557.7405,748.7042;Inherit;True;Property;_displacementmask;displacement mask;14;0;Create;True;0;0;False;0;False;-1;None;9404eb36b5f16d847bd14d71ae6dcb19;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1253.027,109.2108;Float;False;True;-1;6;ASEMaterialInspector;0;0;Standard;Fog_Nuage;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;2;2;10;25;True;1;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;0;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;61;0;60;0
WireConnection;57;0;56;0
WireConnection;13;0;26;0
WireConnection;59;0;57;0
WireConnection;59;2;61;0
WireConnection;21;0;18;0
WireConnection;55;1;59;0
WireConnection;15;0;13;0
WireConnection;15;2;21;0
WireConnection;36;0;35;0
WireConnection;25;1;15;0
WireConnection;58;0;55;0
WireConnection;28;0;25;0
WireConnection;28;1;58;0
WireConnection;28;2;58;0
WireConnection;33;0;36;0
WireConnection;16;0;28;0
WireConnection;16;1;33;0
WireConnection;1;0;2;0
WireConnection;47;0;20;0
WireConnection;47;1;48;0
WireConnection;47;2;16;0
WireConnection;46;0;16;0
WireConnection;46;1;23;0
WireConnection;46;2;24;0
WireConnection;8;0;1;0
WireConnection;0;2;47;0
WireConnection;0;9;8;0
WireConnection;0;11;46;0
ASEEND*/
//CHKSM=0EF5AB4BD1AEB1E96F78F3C0E4F24B5B13827F53