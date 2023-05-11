// Made with Amplify Shader Editor v1.9.1.3
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Destroy_Barrier"
{
	Properties
	{
		_MainTex("MainTex", 2D) = "white" {}
		_Cutoff( "Mask Clip Value", Float ) = 0.02
		_Grow("Grow", Range( 0 , 1)) = 0.07630336
		_PushPull("PushPull", Float) = -0.1
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_Contour("Contour", Color) = (0.4822135,0,1,0)
		_RealSmoothStep("RealSmoothStep", Range( 0 , 2)) = 0.88
		_TextureSample1("Texture Sample 1", 2D) = "white" {}
		_Colored_Waves("Colored_Waves", Color) = (0,0,0,0)
		_Speed_Of_Waves("Speed_Of_Waves", Vector) = (0,3,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 4.6
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows exclude_path:deferred vertex:vertexDataFunc 
		#undef TRANSFORM_TEX
		#define TRANSFORM_TEX(tex,name) float4(tex.xy * name##_ST.xy + name##_ST.zw, tex.z, tex.w)
		struct Input
		{
			float4 uv_texcoord;
		};

		uniform float _Grow;
		uniform float _PushPull;
		uniform sampler2D _MainTex;
		uniform float4 _MainTex_ST;
		uniform float _RealSmoothStep;
		uniform float4 _Contour;
		uniform sampler2D _TextureSample1;
		uniform float2 _Speed_Of_Waves;
		uniform float4 _Colored_Waves;
		uniform sampler2D _TextureSample0;
		uniform float4 _TextureSample0_ST;
		uniform float _Cutoff = 0.02;


		float2 voronoihash103( float2 p )
		{
			
			p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
			return frac( sin( p ) *43758.5453);
		}


		float voronoi103( float2 v, float time, inout float2 id, inout float2 mr, float smoothness, inout float2 smoothId )
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
			 		float2 o = voronoihash103( n + g );
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
			
F1 = 8.0;
for ( int j = -2; j <= 2; j++ )
{
for ( int i = -2; i <= 2; i++ )
{
float2 g = mg + float2( i, j );
float2 o = voronoihash103( n + g );
		o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = f - g - o;
float d = dot( 0.5 * ( r + mr ), normalize( r - mr ) );
F1 = min( F1, d );
}
}
return F1;
		}


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float temp_output_5_0 = ( ( v.texcoord.xy.y - (-0.2 + (_Grow - 0.0) * (1.0 - -0.2) / (1.0 - 0.0)) ) - 0.0 );
			float3 ase_vertexNormal = v.normal.xyz;
			v.vertex.xyz += ( ( temp_output_5_0 * ase_vertexNormal ) * _PushPull );
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_MainTex = i.uv_texcoord * _MainTex_ST.xy + _MainTex_ST.zw;
			float4 tex2DNode13 = tex2D( _MainTex, uv_MainTex );
			float4 appendResult108 = (float4(tex2DNode13.r , tex2DNode13.g , tex2DNode13.b , 0.0));
			o.Albedo = appendResult108.xyz;
			float smoothstepResult57 = smoothstep( 0.0 , 1.0 , ( i.uv_texcoord.y * _RealSmoothStep ));
			float2 panner92 = ( _Time.y * _Speed_Of_Waves + i.uv_texcoord.xy);
			o.Emission = ( ( smoothstepResult57 * _Contour ) + ( tex2D( _TextureSample1, panner92 ) * ( smoothstepResult57 * _Colored_Waves ) ) ).rgb;
			o.Alpha = 1;
			float2 uv_TextureSample0 = i.uv_texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			float temp_output_5_0 = ( ( i.uv_texcoord.xy.y - (-0.2 + (_Grow - 0.0) * (1.0 - -0.2) / (1.0 - 0.0)) ) - 0.0 );
			float time103 = 2.46;
			float2 voronoiSmoothId103 = 0;
			float2 coords103 = i.uv_texcoord.xy * 2.18;
			float2 id103 = 0;
			float2 uv103 = 0;
			float fade103 = 0.5;
			float voroi103 = 0;
			float rest103 = 0;
			for( int it103 = 0; it103 <2; it103++ ){
			voroi103 += fade103 * voronoi103( coords103, time103, id103, uv103, 0,voronoiSmoothId103 );
			rest103 += fade103;
			coords103 *= 2;
			fade103 *= 0.5;
			}//Voronoi103
			voroi103 /= rest103;
			clip( ( ( 1.0 - smoothstepResult57 ) * ( ( tex2D( _TextureSample0, uv_TextureSample0 ) * ( 1.0 - temp_output_5_0 ) ) * voroi103 ) ).r - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=19103
Node;AmplifyShaderEditor.RangedFloatNode;1;-1956.039,-629.8352;Inherit;True;Property;_Grow;Grow;3;0;Create;True;0;0;0;False;0;False;0.07630336;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;53;-672.17,-975.9412;Inherit;True;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;2;-1662.836,-366.5355;Inherit;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;3;-1607.849,-626.0174;Inherit;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-0.2;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;55;-679.2903,-734.8102;Inherit;False;Property;_RealSmoothStep;RealSmoothStep;7;0;Create;True;0;0;0;False;0;False;0.88;0.445;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;54;-325.8321,-972.6849;Inherit;False;FLOAT4;1;0;FLOAT4;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleSubtractOpNode;4;-1312.438,-323.3354;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;56;-89.17439,-864.2274;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;93;276.4078,-1067.16;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;59;-168.935,-638.6724;Inherit;False;Constant;_Smooth;Smooth;9;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;57;221.785,-719.222;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;92;604.9361,-1185.096;Inherit;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;5;-1074.739,-322.819;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;99;483.7179,-961.835;Inherit;False;Property;_Colored_Waves;Colored_Waves;9;0;Create;True;0;0;0;False;0;False;0,0,0,0;1,0.4444933,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;9;-795.5435,-328.4815;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;29;207.4538,-977.0301;Inherit;False;Property;_Contour;Contour;6;0;Create;True;0;0;0;False;0;False;0.4822135,0,1,0;1,0.8313726,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;10;-1149.331,-523.5341;Inherit;True;Property;_TextureSample0;Texture Sample 0;5;0;Create;True;0;0;0;False;0;False;-1;8d864b597abecaa42a89b6f0fe3e57ab;8d864b597abecaa42a89b6f0fe3e57ab;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NormalVertexDataNode;6;-1143.424,-8.907511;Inherit;True;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;91;857.3263,-980.1428;Inherit;True;Property;_TextureSample1;Texture Sample 1;8;0;Create;True;0;0;0;False;0;False;-1;None;d1c24466dc92d3f46966d94b2a2526bc;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;100;726.1364,-808.0245;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;-453.2474,-340.2309;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;63;487.7157,-386.4547;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;96;1134.066,-734.464;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;58;626.2157,-659.7203;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-659.4373,176.6149;Inherit;False;Property;_PushPull;PushPull;4;0;Create;True;0;0;0;False;0;False;-0.1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-821.9225,-31.84186;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;98;1358.096,-513.7786;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;62;731.5711,-202.8543;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;85;191.7767,-479.3946;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GradientNode;11;-1953.938,-777.8353;Inherit;False;0;2;2;1,1,1,0;0,0,0,1;1,0;1,1;0;1;OBJECT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;-335.5512,14.58409;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;86;1095.204,-372.5918;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;95;288.1108,-1306.235;Inherit;False;Property;_Speed_Of_Waves;Speed_Of_Waves;10;0;Create;True;0;0;0;False;0;False;0,3;0,-1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;104;9.551914,-229.6625;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.VoronoiNode;103;-236.199,-174.7606;Inherit;True;0;0;1;4;2;False;1;False;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;2.46;False;2;FLOAT;2.18;False;3;FLOAT;0;False;3;FLOAT;0;FLOAT2;1;FLOAT2;2
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1803.322,-337.3245;Float;False;True;-1;6;ASEMaterialInspector;0;0;Standard;Destroy_Barrier;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;;0;False;;False;0;False;;0;False;;False;0;Custom;0.02;True;True;0;True;TransparentCutout;;Geometry;ForwardOnly;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;True;0.5;True;0;0;False;;0;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0.01;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;2;-1;-1;-1;0;False;0;0;False;;-1;0;False;_Mask_Clip_Value;0;0;0;False;0.1;False;;0;False;;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.SimpleAddOpNode;107;1680.459,-704.3683;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SamplerNode;13;1207.486,-1083.213;Inherit;True;Property;_MainTex;MainTex;0;0;Create;True;0;0;0;False;0;False;-1;None;41ce5aa5451d3f345890d5fcc52dd317;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;108;1526.685,-939.3473;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ColorNode;109;1348.874,-818.9583;Inherit;False;Property;_Base_ColorIguess;Base_ColorIguess;1;0;Create;True;0;0;0;False;0;False;0,0,0,0;1,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;105;1856.223,-1109.703;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
WireConnection;3;0;1;0
WireConnection;54;0;53;0
WireConnection;4;0;2;2
WireConnection;4;1;3;0
WireConnection;56;0;54;1
WireConnection;56;1;55;0
WireConnection;57;0;56;0
WireConnection;57;1;59;0
WireConnection;92;0;53;0
WireConnection;92;2;95;0
WireConnection;92;1;93;0
WireConnection;5;0;4;0
WireConnection;9;0;5;0
WireConnection;91;1;92;0
WireConnection;100;0;57;0
WireConnection;100;1;99;0
WireConnection;14;0;10;0
WireConnection;14;1;9;0
WireConnection;63;0;57;0
WireConnection;96;0;91;0
WireConnection;96;1;100;0
WireConnection;58;0;57;0
WireConnection;58;1;29;0
WireConnection;7;0;5;0
WireConnection;7;1;6;0
WireConnection;98;0;58;0
WireConnection;98;1;96;0
WireConnection;62;0;63;0
WireConnection;62;1;104;0
WireConnection;85;0;53;4
WireConnection;85;1;55;0
WireConnection;12;0;7;0
WireConnection;12;1;8;0
WireConnection;86;0;85;0
WireConnection;104;0;14;0
WireConnection;104;1;103;0
WireConnection;0;0;108;0
WireConnection;0;2;98;0
WireConnection;0;10;62;0
WireConnection;0;11;12;0
WireConnection;107;0;108;0
WireConnection;107;1;109;0
WireConnection;108;0;13;1
WireConnection;108;1;13;2
WireConnection;108;2;13;3
ASEEND*/
//CHKSM=06CC9CE31D43ED2063FCF90FA578B5A1581B79F4