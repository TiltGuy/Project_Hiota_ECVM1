// Made with Amplify Shader Editor v1.9.1.3
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Wind_Up_Sword"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_TextureSample1("Texture Sample 1", 2D) = "white" {}
		_Colored_Waves("Colored_Waves", Color) = (0.9755855,0,1,0)
		_Speed_Of_Waves("Speed_Of_Waves", Vector) = (0,3,0,0)
		_Emission_Power("Emission_Power", Range( 0 , 10)) = 1
		_Opacity("Opacity", Range( 0 , 1)) = 1
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
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows exclude_path:deferred 
		#undef TRANSFORM_TEX
		#define TRANSFORM_TEX(tex,name) float4(tex.xy * name##_ST.xy + name##_ST.zw, tex.z, tex.w)
		struct Input
		{
			float4 uv_texcoord;
		};

		uniform float4 _Colored_Waves;
		uniform float _Emission_Power;
		uniform sampler2D _TextureSample1;
		uniform float2 _Speed_Of_Waves;
		uniform float _Opacity;
		uniform float _Cutoff = 0.5;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 temp_cast_0 = (( _Colored_Waves.a * _Emission_Power )).xxx;
			o.Albedo = temp_cast_0;
			float2 panner5 = ( _Time.y * _Speed_Of_Waves + i.uv_texcoord.xy);
			float4 tex2DNode7 = tex2D( _TextureSample1, panner5 );
			o.Emission = ( tex2DNode7 * _Colored_Waves ).rgb;
			o.Alpha = 1;
			clip( ( tex2DNode7.a * _Opacity ) - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=19103
Node;AmplifyShaderEditor.BreakToComponentsNode;1;-1935.383,-189.6673;Inherit;False;FLOAT4;1;0;FLOAT4;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.PannerNode;5;-1004.615,-402.0785;Inherit;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;10;-2225.293,-310.6156;Inherit;True;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;3;-1333.143,-284.1425;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;2;-1695.516,38.60757;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;4;-1387.766,63.79568;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;6;-871.0511,104.7308;Inherit;False;Property;_Colored_Waves;Colored_Waves;2;0;Create;True;0;0;0;False;0;False;0.9755855,0,1,0;0.3679244,0,0.3305867,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;9;-1321.44,-523.2175;Inherit;False;Property;_Speed_Of_Waves;Speed_Of_Waves;3;0;Create;True;0;0;0;False;0;False;0,3;0,-2;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SamplerNode;7;-723.1882,-185.8267;Inherit;True;Property;_TextureSample1;Texture Sample 1;1;0;Create;True;0;0;0;False;0;False;-1;3604126eac39c9a4787cf83e5bea9a96;3604126eac39c9a4787cf83e5bea9a96;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;12;-808.4948,287.7896;Inherit;False;Property;_Emission_Power;Emission_Power;4;0;Create;True;0;0;0;False;0;False;1;0.96;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;97.50002,10.4;Float;False;True;-1;6;ASEMaterialInspector;0;0;Standard;Wind_Up_Sword;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;;0;False;;False;0;False;;0;False;;False;0;Custom;0.5;True;True;0;True;TransparentCutout;;Geometry;ForwardOnly;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;True;0;0;False;;0;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;0;-1;-1;-1;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;-368.8333,11.8054;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-427.2255,250.3682;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;-164.6254,295.8684;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;15;-594.9254,402.4683;Inherit;False;Property;_Opacity;Opacity;5;0;Create;True;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
WireConnection;1;0;10;0
WireConnection;5;0;10;0
WireConnection;5;2;9;0
WireConnection;5;1;3;0
WireConnection;2;0;1;1
WireConnection;4;0;1;1
WireConnection;7;1;5;0
WireConnection;0;0;13;0
WireConnection;0;2;11;0
WireConnection;0;10;14;0
WireConnection;11;0;7;0
WireConnection;11;1;6;0
WireConnection;13;0;6;4
WireConnection;13;1;12;0
WireConnection;14;0;7;4
WireConnection;14;1;15;0
ASEEND*/
//CHKSM=931575C38210CDC270F783CB6D3E00E345420A91