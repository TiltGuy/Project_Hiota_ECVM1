// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/Parallax FrontWall"
{
	Properties
	{
		_Roots("Roots", 2D) = "white" {}
		_Rocks("Rocks", 2D) = "white" {}
		_Rocks_Mask("Rocks_Mask", 2D) = "white" {}
		_Roots_Mask("Roots_Mask", 2D) = "white" {}
		_Trees("Trees", 2D) = "white" {}
		_Tree_Mask("Tree_Mask", 2D) = "white" {}
		_GradiantValues("GradiantValues", Vector) = (2.3,0,0,0)
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

		uniform sampler2D _Roots;
		uniform sampler2D _Roots_Mask;
		SamplerState sampler_Roots_Mask;
		uniform sampler2D _Trees;
		uniform sampler2D _Tree_Mask;
		SamplerState sampler_Tree_Mask;
		uniform sampler2D _Rocks;
		uniform sampler2D _Rocks_Mask;
		SamplerState sampler_Rocks_Mask;
		uniform float2 _GradiantValues;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Normal = float3(0,0,1);
			float2 uv_TexCoord49 = i.uv_texcoord * float2( 5,1 );
			float2 Offset55 = ( ( 0.7 - 1 ) * i.viewDir.xy * 1.0 ) + uv_TexCoord49;
			float4 tex2DNode69 = tex2D( _Roots_Mask, Offset55 );
			float2 Offset75 = ( ( 0.82 - 1 ) * i.viewDir.xy * 1.0 ) + uv_TexCoord49;
			float4 tex2DNode77 = tex2D( _Tree_Mask, Offset75 );
			float4 lerpResult60 = lerp( ( tex2D( _Roots, Offset55 ) * tex2DNode69.r ) , tex2D( _Trees, Offset75 ) , tex2DNode77.r);
			float2 Offset56 = ( ( 0.9 - 1 ) * i.viewDir.xy * 1.0 ) + uv_TexCoord49;
			float4 tex2DNode67 = tex2D( _Rocks_Mask, Offset56 );
			float4 lerpResult61 = lerp( lerpResult60 , tex2D( _Rocks, Offset56 ) , tex2DNode67.r);
			o.Emission = lerpResult61.rgb;
			float temp_output_131_0 = ( 1.0 - tex2DNode67.r );
			float2 uv_TexCoord99 = i.uv_texcoord * float2( 1,2.5 );
			float smoothstepResult105 = smoothstep( _GradiantValues.x , _GradiantValues.y , uv_TexCoord99.y);
			o.Alpha = ( ( ( tex2DNode77.r * temp_output_131_0 ) + tex2DNode67.r + ( tex2DNode69.r * ( 1.0 - tex2DNode77.r ) * temp_output_131_0 ) ) * smoothstepResult105 );
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
-1920;2;1920;1019;2790.771;-189.471;1.997839;True;False
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;57;-1656.814,1272.859;Inherit;False;Tangent;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.TextureCoordinatesNode;49;-1693.623,1103.702;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;5,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ParallaxMappingNode;56;-1166.974,1879.952;Inherit;False;Normal;4;0;FLOAT2;0,0;False;1;FLOAT;0.9;False;2;FLOAT;1;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ParallaxMappingNode;75;-1159.565,1242.01;Inherit;False;Normal;4;0;FLOAT2;0,0;False;1;FLOAT;0.82;False;2;FLOAT;1;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ParallaxMappingNode;55;-1131.065,699.2812;Inherit;False;Normal;4;0;FLOAT2;0,0;False;1;FLOAT;0.7;False;2;FLOAT;1;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;67;-856.5965,1979.361;Inherit;True;Property;_Rocks_Mask;Rocks_Mask;14;0;Create;True;0;0;False;0;False;-1;ea171257982f8524ca326c0a9c9cbc65;19c036c36d12c3844b0de52f87d05d87;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;77;-865.2171,1336.217;Inherit;True;Property;_Tree_Mask;Tree_Mask;17;0;Create;True;0;0;False;0;False;-1;997790ddb241a4741bcae6fbca03905b;3e2dcdc8679ca604387dfd16ded44697;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;69;-873.2884,707.8722;Inherit;True;Property;_Roots_Mask;Roots_Mask;15;0;Create;True;0;0;False;0;False;-1;748cf26e448d05d4bbc5f96692c76003;48cbbbdef0c3328488488ed6b7965e48;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;131;-513.6525,2302.387;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;136;-509.7755,1442.75;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;68;-867.3585,512.1688;Inherit;True;Property;_Roots;Roots;12;0;Create;True;0;0;False;0;False;-1;7fe7d43a2b796a643a6ebcae9c6501a3;e6c212efda9655d4a8370592c34bd13e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;76;-883.4169,1123.017;Inherit;True;Property;_Trees;Trees;16;0;Create;True;0;0;False;0;False;-1;5f0c91822b4a608428df66c466e38837;404ee47210805fa43bc3d60b5676a67d;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;116;-445.8559,566.2096;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;20;-3522.197,-434.12;Inherit;False;1783.557;1624.035;Example;7;12;11;5;4;17;9;15;;1,1,1,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;99;-15.31471,2476.969;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,2.5;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;132;-291.6454,2270.526;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;106;-10.57698,2673.158;Inherit;False;Property;_GradiantValues;GradiantValues;18;0;Create;True;0;0;False;0;False;2.3,0;1.67,1.46;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;135;-326.9117,1407.157;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;130;81.3587,2092.489;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;105;315.423,2593.158;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;2.56;False;2;FLOAT;1.87;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;66;-820.2202,1727.156;Inherit;True;Property;_Rocks;Rocks;13;0;Create;True;0;0;False;0;False;-1;82ee700d2dfb2004fb15ad8cfe5df72d;e97676eb5096f0145926c22f57e49543;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;60;-246.4458,1151.804;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;11;-3120.087,-384.12;Inherit;False;761;372;Back;2;2;1;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;17;-3104.808,63.3984;Inherit;False;760.4933;495.533;middle?;3;13;14;18;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;12;-3089.703,654.2173;Inherit;False;743.1398;520.6328;InFront;3;8;6;19;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;38;-2621.619,2812.498;Inherit;True;Property;_04;04;11;0;Create;True;0;0;False;0;False;-1;0a4607fe21262a848a00723c46ba5141;0a4607fe21262a848a00723c46ba5141;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ParallaxMappingNode;2;-3070.088,-334.1201;Inherit;False;Normal;4;0;FLOAT2;1,1;False;1;FLOAT;0.9;False;2;FLOAT;1;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;32;-1951.502,1968.802;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;28;-2627.514,2231.271;Inherit;True;Property;_03;03;6;0;Create;True;0;0;False;0;False;-1;e2f6f6101ecef3c4da1059d03ae8e991;e2f6f6101ecef3c4da1059d03ae8e991;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;19;-2682.955,726.1613;Inherit;True;Property;_ed84ef97b620c03e1e238e706d6ba7a2c;ed84ef97b620c03e1e238e706d6ba7a2c;3;0;Create;True;0;0;False;0;False;-1;2c70a655c5418344c861d5fdf8afa65c;2c70a655c5418344c861d5fdf8afa65c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;137;419.1924,2227.093;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;22;-3469.503,1754.983;Inherit;False;Tangent;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.LerpOp;15;-2003.64,104.3086;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;35;-2624.4,2450.684;Inherit;True;Property;_03_mask;03_mask;8;0;Create;True;0;0;False;0;False;-1;2c5f59f2bac4c9c4d8d48c50157a8586;2c5f59f2bac4c9c4d8d48c50157a8586;True;0;False;white;Auto;False;Object;-1;Auto;Cube;8;0;SAMPLERCUBE;;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;14;-2721.314,113.3984;Inherit;True;Property;_ed84ef97b620c03e1e238e706d6ba7a2d;ed84ef97b620c03e1e238e706d6ba7a2d;2;0;Create;True;0;0;False;0;False;-1;f382cae24ca64c34695e8bd88140c0a4;f382cae24ca64c34695e8bd88140c0a4;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;23;-2678.845,1281.433;Inherit;True;Property;_01;01;5;0;Create;True;0;0;False;0;False;-1;7dc4371147da39841a61c173a31e879f;7dc4371147da39841a61c173a31e879f;True;0;False;white;Auto;False;Object;-1;Auto;Cube;8;0;SAMPLERCUBE;;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;5;-3425.197,180.6345;Inherit;False;Tangent;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SamplerNode;1;-2736.087,-242.1202;Inherit;True;Property;_ed84ef97b620c03e1e238e706d6ba7a2b;ed84ef97b620c03e1e238e706d6ba7a2b;0;0;Create;True;0;0;False;0;False;-1;3c92c5242c29ba4449999021f925aec1;3c92c5242c29ba4449999021f925aec1;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ParallaxMappingNode;13;-3054.808,140.7057;Inherit;False;Normal;4;0;FLOAT2;1,1;False;1;FLOAT;0.96;False;2;FLOAT;1;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ParallaxMappingNode;6;-3039.703,760.6503;Inherit;False;Normal;4;0;FLOAT2;1,1;False;1;FLOAT;1;False;2;FLOAT;1;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ParallaxMappingNode;25;-2988.727,1681.963;Inherit;False;Normal;4;0;FLOAT2;1,1;False;1;FLOAT;0.98;False;2;FLOAT;1;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;61;50.05423,1858.996;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;18;-2695.236,313.5577;Inherit;True;Property;_Masktets;Mask tets;4;0;Create;True;0;0;False;0;False;-1;2062205d6f9358046ae7677b130c621c;2062205d6f9358046ae7677b130c621c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;33;-1709.303,2240.688;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;37;-2676.77,1659.302;Inherit;True;Property;_02;02;9;0;Create;True;0;0;False;0;False;-1;45ba13e82c20e3d44b7ea864b6395803;45ba13e82c20e3d44b7ea864b6395803;True;0;False;white;Auto;False;Object;-1;Auto;Cube;8;0;SAMPLERCUBE;;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;9;-2269.946,-253.4296;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0.2765664,0.3138022,0.5188679,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ParallaxMappingNode;29;-2973.832,2839.103;Inherit;False;Normal;4;0;FLOAT2;1,1;False;1;FLOAT;0.94;False;2;FLOAT;1;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;8;-2713.479,959.9149;Inherit;True;Property;_PlatformArenaLOW_DefaultMaterial_Mask2;Platform-Arena-LOW_DefaultMaterial_Mask2;1;0;Create;True;0;0;False;0;False;-1;aa46518e4e6161b48b71af83422eadb3;e2294b3836a24e942b731ea6cf60e79f;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;36;-2654.199,1904.946;Inherit;True;Property;_02_mask;02_mask;10;0;Create;True;0;0;False;0;False;-1;a963d8112a1211c4980aa5b81a1f4d22;a963d8112a1211c4980aa5b81a1f4d22;True;0;False;white;Auto;False;Object;-1;Auto;Cube;8;0;SAMPLERCUBE;;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;34;-2639.92,3048.717;Inherit;True;Property;_04_Mask;04_Mask;7;0;Create;True;0;0;False;0;False;-1;f57652a3e6fc83145b888f09cc5da2c7;f57652a3e6fc83145b888f09cc5da2c7;True;0;False;white;Auto;False;Object;-1;Auto;Cube;8;0;SAMPLERCUBE;;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ParallaxMappingNode;24;-2998.26,1305.756;Inherit;False;Normal;4;0;FLOAT2;1,1;False;1;FLOAT;0.99;False;2;FLOAT;1;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ParallaxMappingNode;27;-2987.809,2254.86;Inherit;False;Normal;4;0;FLOAT2;1,1;False;1;FLOAT;0.96;False;2;FLOAT;1;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;21;-3506.522,1492.552;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;31;-2169.454,1680.166;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;4;-3472.197,-44.36562;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;700.9253,1878.031;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Custom/Parallax FrontWall;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;56;0;49;0
WireConnection;56;3;57;0
WireConnection;75;0;49;0
WireConnection;75;3;57;0
WireConnection;55;0;49;0
WireConnection;55;3;57;0
WireConnection;67;1;56;0
WireConnection;77;1;75;0
WireConnection;69;1;55;0
WireConnection;131;0;67;1
WireConnection;136;0;77;1
WireConnection;68;1;55;0
WireConnection;76;1;75;0
WireConnection;116;0;68;0
WireConnection;116;1;69;1
WireConnection;132;0;77;1
WireConnection;132;1;131;0
WireConnection;135;0;69;1
WireConnection;135;1;136;0
WireConnection;135;2;131;0
WireConnection;130;0;132;0
WireConnection;130;1;67;1
WireConnection;130;2;135;0
WireConnection;105;0;99;2
WireConnection;105;1;106;1
WireConnection;105;2;106;2
WireConnection;66;1;56;0
WireConnection;60;0;116;0
WireConnection;60;1;76;0
WireConnection;60;2;77;1
WireConnection;38;1;29;0
WireConnection;2;0;4;0
WireConnection;2;3;5;0
WireConnection;32;0;31;0
WireConnection;32;1;28;0
WireConnection;32;2;35;1
WireConnection;28;1;27;0
WireConnection;19;1;6;0
WireConnection;137;0;130;0
WireConnection;137;1;105;0
WireConnection;15;0;9;0
WireConnection;15;1;14;0
WireConnection;15;2;18;1
WireConnection;35;1;27;0
WireConnection;14;1;13;0
WireConnection;23;1;24;0
WireConnection;1;1;2;0
WireConnection;13;0;4;0
WireConnection;6;0;4;0
WireConnection;6;3;5;0
WireConnection;25;0;21;0
WireConnection;25;3;22;0
WireConnection;61;0;60;0
WireConnection;61;1;66;0
WireConnection;61;2;67;1
WireConnection;18;1;13;0
WireConnection;33;0;32;0
WireConnection;33;1;38;0
WireConnection;33;2;34;1
WireConnection;37;1;25;0
WireConnection;9;0;1;0
WireConnection;9;1;19;0
WireConnection;9;2;8;1
WireConnection;29;0;21;0
WireConnection;29;3;22;0
WireConnection;8;1;6;0
WireConnection;36;1;25;0
WireConnection;34;1;29;0
WireConnection;24;0;21;0
WireConnection;24;3;22;0
WireConnection;27;0;21;0
WireConnection;27;3;22;0
WireConnection;31;0;23;0
WireConnection;31;1;37;0
WireConnection;31;2;36;1
WireConnection;0;2;61;0
WireConnection;0;9;137;0
ASEEND*/
//CHKSM=BED5A1BB2371276DB9315D56E23EE95A17E0CB40