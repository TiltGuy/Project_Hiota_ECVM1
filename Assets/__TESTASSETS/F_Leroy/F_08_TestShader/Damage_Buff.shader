// Made with Amplify Shader Editor v1.9.1.3
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Damage_Buff"
{
	Properties
	{
		_Color0("Color 0", Color) = (0,0,0,0)
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_Speed("Speed", Vector) = (0,2,0,0)
		_Amplitude("Amplitude", Float) = 1
		_Grow("Grow", Range( 0 , 1)) = 0
		_Speed1("Speed", Float) = 1
		_New_High("New_High", Range( 0 , 1)) = 0.2622461
		_New_Low("New_Low", Range( 0 , 1)) = 1
		_Emission_Intensity("Emission_Intensity", Range( 1 , 10)) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows exclude_path:deferred vertex:vertexDataFunc 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _TextureSample0;
		uniform float2 _Speed;
		uniform float _Grow;
		uniform float _New_Low;
		uniform float _New_High;
		uniform float _Speed1;
		uniform float _Amplitude;
		uniform float4 _Color0;
		uniform float _Emission_Intensity;
		uniform float _Cutoff = 0.5;


		float2 voronoihash1( float2 p )
		{
			
			p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
			return frac( sin( p ) *43758.5453);
		}


		float voronoi1( float2 v, float time, inout float2 id, inout float2 mr, float smoothness, inout float2 smoothId )
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
			 		float2 o = voronoihash1( n + g );
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


		float2 voronoihash44( float2 p )
		{
			
			p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
			return frac( sin( p ) *43758.5453);
		}


		float voronoi44( float2 v, float time, inout float2 id, inout float2 mr, float smoothness, inout float2 smoothId )
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
			 		float2 o = voronoihash44( n + g );
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


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float time1 = 0.0;
			float2 voronoiSmoothId1 = 0;
			float2 panner3 = ( 1.0 * _Time.y * _Speed + v.texcoord.xy);
			float2 temp_cast_0 = (tex2Dlod( _TextureSample0, float4( panner3, 0, 0.0) ).a).xx;
			float2 coords1 = temp_cast_0 * 1.0;
			float2 id1 = 0;
			float2 uv1 = 0;
			float voroi1 = voronoi1( coords1, time1, id1, uv1, 0, voronoiSmoothId1 );
			float3 ase_vertexNormal = v.normal.xyz;
			float temp_output_11_0 = ( ( v.texcoord.xy.y - (_New_Low + (_Grow - 0.0) * (_New_High - _New_Low) / (1.0 - 0.0)) ) - 0.0 );
			float time44 = 0.0;
			float2 voronoiSmoothId44 = 0;
			float2 coords44 = v.texcoord.xy * 3.0;
			float2 id44 = 0;
			float2 uv44 = 0;
			float voroi44 = voronoi44( coords44, time44, id44, uv44, 0, voronoiSmoothId44 );
			float3 ase_vertex3Pos = v.vertex.xyz;
			float4 appendResult39 = (float4(( voroi44 * sin( ( ( ase_vertex3Pos.x + 0.0 + ( _Time.y * _Speed1 ) ) / 0.1324087 ) ) ) , 0.0 , 0.0 , 0.0));
			v.vertex.xyz += ( saturate( voroi1 ) * float4( ( ase_vertexNormal * saturate( temp_output_11_0 ) ) , 0.0 ) * ( appendResult39 * _Amplitude ) ).xyz;
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 appendResult24 = (float3(_Color0.r , _Color0.g , _Color0.b));
			float time1 = 0.0;
			float2 voronoiSmoothId1 = 0;
			float2 panner3 = ( 1.0 * _Time.y * _Speed + i.uv_texcoord);
			float2 temp_cast_0 = (tex2D( _TextureSample0, panner3 ).a).xx;
			float2 coords1 = temp_cast_0 * 1.0;
			float2 id1 = 0;
			float2 uv1 = 0;
			float voroi1 = voronoi1( coords1, time1, id1, uv1, 0, voronoiSmoothId1 );
			float temp_output_11_0 = ( ( i.uv_texcoord.y - (_New_Low + (_Grow - 0.0) * (_New_High - _New_Low) / (1.0 - 0.0)) ) - 0.0 );
			float3 temp_output_22_0 = ( appendResult24 * ( voroi1 * temp_output_11_0 ) * _Emission_Intensity );
			o.Emission = temp_output_22_0;
			o.Alpha = 1;
			clip( temp_output_22_0.x - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=19103
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;-487.7863,509.2275;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;2;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;12;-1893.072,551.3097;Inherit;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;13;-1542.673,594.5098;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;14;-2234.909,331.9272;Inherit;False;Property;_New_Low;New_Low;8;0;Create;True;0;0;0;False;0;False;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;15;-2238.511,108.8107;Inherit;True;Property;_Grow;Grow;5;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;-746.0687,612.119;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TFHCRemapNode;19;-1838.085,291.8282;Inherit;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0.2;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;20;-1004.745,642.7252;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;21;-1247.689,403.647;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;3;-1698.567,-63.17433;Inherit;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0.5,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;4;-2014.661,-237.3477;Inherit;True;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;24;-838.988,-350.4913;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;-319.0993,21.3207;Inherit;True;3;3;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;25;-750.2543,279.9039;Inherit;False;Property;_Emission_Intensity;Emission_Intensity;9;0;Create;True;0;0;0;False;0;False;1;1;1;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;23;-1144.018,-433.6811;Inherit;False;Property;_Color0;Color 0;0;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;17;-2227.306,416.0092;Inherit;False;Property;_New_High;New_High;7;0;Create;True;0;0;0;False;0;False;0.2622461;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;49,-63;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Damage_Buff;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;;0;False;;False;0;False;;0;False;;False;0;Custom;0.5;True;True;0;False;TransparentCutout;;Geometry;ForwardOnly;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;True;0;0;False;;0;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;1;-1;-1;-1;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.VoronoiNode;1;-1041.928,-111.3427;Inherit;True;0;0;1;0;1;False;1;False;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;3;FLOAT;0;FLOAT2;1;FLOAT2;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;-689.5676,9.675232;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;8;-630.4745,407.3193;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;7;-1989.661,-45.34772;Inherit;False;Property;_Speed;Speed;3;0;Create;True;0;0;0;False;0;False;0,2;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;-894.0917,893.851;Inherit;True;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;28;-1833.762,1086.036;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;29;-1974.807,1205.044;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;30;-2211.72,1186.311;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;31;-1661.864,1104.769;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;32;-1821.642,1293.198;Inherit;False;Constant;_Float0;Float 0;2;0;Create;True;0;0;0;False;0;False;0.1324087;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;33;-2213.922,1279.974;Inherit;False;Property;_Speed1;Speed;6;0;Create;True;0;0;0;False;0;False;1;0.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;34;-1514.932,1163.786;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;38;-1202.853,1161.385;Inherit;False;Property;_Amplitude;Amplitude;4;0;Create;True;0;0;0;False;0;False;1;0.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;39;-1178.092,954.3046;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;36;-1357.419,979.5762;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;35;-1848.114,803.1637;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PosVertexDataNode;37;-2266.546,1054.076;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;11;-1288.976,580.0262;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;43;-1584.402,961.7186;Inherit;False;Noise Sine Wave;-1;;2;a6eff29f739ced848846e3b648af87bd;0;2;1;FLOAT2;0,0;False;2;FLOAT2;-0.5,1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.VoronoiNode;44;-1574.402,814.7186;Inherit;False;0;0;1;0;1;False;1;False;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;3;False;3;FLOAT;0;False;3;FLOAT;0;FLOAT2;1;FLOAT2;2
Node;AmplifyShaderEditor.SamplerNode;2;-1360.799,-166.7008;Inherit;True;Property;_TextureSample0;Texture Sample 0;2;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
WireConnection;9;0;8;0
WireConnection;9;1;16;0
WireConnection;9;2;27;0
WireConnection;13;0;12;2
WireConnection;13;1;19;0
WireConnection;16;0;21;0
WireConnection;16;1;20;0
WireConnection;19;0;15;0
WireConnection;19;3;14;0
WireConnection;19;4;17;0
WireConnection;20;0;11;0
WireConnection;3;0;4;0
WireConnection;3;2;7;0
WireConnection;24;0;23;1
WireConnection;24;1;23;2
WireConnection;24;2;23;3
WireConnection;22;0;24;0
WireConnection;22;1;26;0
WireConnection;22;2;25;0
WireConnection;0;2;22;0
WireConnection;0;10;22;0
WireConnection;0;11;9;0
WireConnection;1;0;2;4
WireConnection;26;0;1;0
WireConnection;26;1;11;0
WireConnection;8;0;1;0
WireConnection;27;0;39;0
WireConnection;27;1;38;0
WireConnection;28;0;37;1
WireConnection;28;2;29;0
WireConnection;29;0;30;0
WireConnection;29;1;33;0
WireConnection;31;0;28;0
WireConnection;31;1;32;0
WireConnection;34;0;31;0
WireConnection;39;0;36;0
WireConnection;36;0;44;0
WireConnection;36;1;34;0
WireConnection;11;0;13;0
WireConnection;43;1;35;0
WireConnection;44;0;35;0
WireConnection;2;1;3;0
ASEEND*/
//CHKSM=F5042D063BD3598689047AF3E0A8721F933B40D5