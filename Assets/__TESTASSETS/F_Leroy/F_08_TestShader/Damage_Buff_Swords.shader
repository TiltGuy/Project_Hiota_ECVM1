// Made with Amplify Shader Editor v1.9.1.3
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Damage_Buff_Swords"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = -0.1
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_Waves_Colors("Waves_Colors", Color) = (1,0,0,0)
		_Emission_Intensity("Emission_Intensity", Range( 0 , 10)) = 1
		_Speed("Speed", Vector) = (0,-2,0,0)
		_Grow("Grow", Range( 0 , 1)) = 0
		_New_High("New_High", Range( 0 , 1)) = 0.2622461
		_New_Low("New_Low", Range( 0 , 1)) = 1
		_Float0("Float 0", Float) = 0
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
		uniform float _Float0;
		uniform float _Emission_Intensity;
		uniform float4 _Waves_Colors;
		uniform float _Cutoff = -0.1;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float2 panner2 = ( 1.0 * _Time.y * _Speed + v.texcoord.xy);
			float4 tex2DNode3 = tex2Dlod( _TextureSample0, float4( panner2, 0, 0.0) );
			float3 appendResult9 = (float3(tex2DNode3.r , tex2DNode3.g , tex2DNode3.b));
			float3 ase_vertexNormal = v.normal.xyz;
			float temp_output_17_0 = ( ( v.texcoord.xy.y - (_New_Low + (_Grow - 0.0) * (_New_High - _New_Low) / (1.0 - 0.0)) ) - 0.0 );
			v.vertex.xyz += ( ( saturate( appendResult9 ) * ( ase_vertexNormal * saturate( temp_output_17_0 ) ) ) * _Float0 );
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 panner2 = ( 1.0 * _Time.y * _Speed + i.uv_texcoord);
			float4 tex2DNode3 = tex2D( _TextureSample0, panner2 );
			float3 appendResult9 = (float3(tex2DNode3.r , tex2DNode3.g , tex2DNode3.b));
			float3 appendResult12 = (float3(_Waves_Colors.r , _Waves_Colors.g , _Waves_Colors.b));
			float temp_output_17_0 = ( ( i.uv_texcoord.y - (_New_Low + (_Grow - 0.0) * (_New_High - _New_Low) / (1.0 - 0.0)) ) - 0.0 );
			float3 temp_output_42_0 = ( ( appendResult9 * _Emission_Intensity * appendResult12 ) * temp_output_17_0 );
			o.Emission = temp_output_42_0;
			o.Alpha = 1;
			clip( temp_output_42_0.x - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=19103
Node;AmplifyShaderEditor.TexCoordVertexDataNode;1;-1268.009,-251.9934;Inherit;True;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;2;-869.6484,-113.0152;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,-1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;7;-1168.768,-32.38956;Inherit;False;Property;_Speed;Speed;4;0;Create;True;0;0;0;False;0;False;0,-2;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.DynamicAppendNode;9;-233.8718,-24.94705;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;3;-601.9813,-38.3559;Inherit;True;Property;_TextureSample0;Texture Sample 0;1;0;Create;True;0;0;0;False;0;False;-1;cc3c706b1403c2a4fb7c46c5a5e0c841;cc3c706b1403c2a4fb7c46c5a5e0c841;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;37;-176.0472,341.8295;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;36;21.09686,483.1401;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;38;88.72146,778.6613;Inherit;False;Property;_Float0;Float 0;8;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;17;-796.0931,568.9388;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;14;-1384.189,525.2222;Inherit;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;16;-1033.79,568.4223;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;24;-1726.025,305.8398;Inherit;False;Property;_New_Low;New_Low;7;0;Create;True;0;0;0;False;0;False;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;13;-1729.627,82.72327;Inherit;True;Property;_Grow;Grow;5;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;40;-237.1855,586.0316;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;25;-1794.216,545.2097;Inherit;False;Property;_New_High;New_High;6;0;Create;True;0;0;0;False;0;False;0.2622461;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;39;266.5004,501.9961;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TFHCRemapNode;15;-1329.202,265.7408;Inherit;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0.2;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;41;-495.8617,616.6378;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;35;-738.8054,377.5596;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;10;-471.3658,168.6238;Inherit;False;Property;_Emission_Intensity;Emission_Intensity;3;0;Create;True;0;0;0;False;0;False;1;0;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;755.3063,41.23268;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Damage_Buff_Swords;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;;0;False;;False;0;False;;0;False;;False;0;Custom;-0.1;True;True;0;False;TransparentCutout;;Geometry;ForwardOnly;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;True;0;0;False;;0;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;0;-1;-1;-1;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;42;419.8311,128.6618;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;107.3041,-6.472128;Inherit;False;3;3;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;5;-792.7073,-341.5302;Inherit;False;Property;_Waves_Colors;Waves_Colors;2;0;Create;True;0;0;0;False;0;False;1,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;12;-459.4547,-301.6133;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;43;-13.19323,-421.3962;Inherit;False;Constant;_Color0;Color 0;9;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
WireConnection;2;0;1;0
WireConnection;2;2;7;0
WireConnection;9;0;3;1
WireConnection;9;1;3;2
WireConnection;9;2;3;3
WireConnection;3;1;2;0
WireConnection;37;0;9;0
WireConnection;36;0;37;0
WireConnection;36;1;40;0
WireConnection;17;0;16;0
WireConnection;16;0;14;2
WireConnection;16;1;15;0
WireConnection;40;0;35;0
WireConnection;40;1;41;0
WireConnection;39;0;36;0
WireConnection;39;1;38;0
WireConnection;15;0;13;0
WireConnection;15;3;24;0
WireConnection;15;4;25;0
WireConnection;41;0;17;0
WireConnection;0;2;42;0
WireConnection;0;10;42;0
WireConnection;0;11;39;0
WireConnection;42;0;11;0
WireConnection;42;1;17;0
WireConnection;11;0;9;0
WireConnection;11;1;10;0
WireConnection;11;2;12;0
WireConnection;12;0;5;1
WireConnection;12;1;5;2
WireConnection;12;2;5;3
ASEEND*/
//CHKSM=5A9F9D42D2ECE86BFDEA79197436D1DAC105D836