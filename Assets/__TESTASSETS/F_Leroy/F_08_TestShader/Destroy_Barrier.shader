// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Destroy_Barrier"
{
	Properties
	{
		_MainTex("MainTex", 2D) = "white" {}
		_Cutoff( "Mask Clip Value", Float ) = 0.4
		_Grow("Grow", Range( 0 , 1)) = 0.07630336
		_PushPull("PushPull", Float) = -0.1
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_Contour("Contour", Color) = (0.4822135,0,1,0)
		_Edge_Width("Edge_Width", Float) = 0
		_Mask_Clip_Value("Mask_Clip_Value", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#pragma target 4.6
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows exclude_path:deferred vertex:vertexDataFunc 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float _Grow;
		uniform float _PushPull;
		uniform sampler2D _MainTex;
		uniform float4 _MainTex_ST;
		uniform sampler2D _TextureSample0;
		uniform float4 _TextureSample0_ST;
		uniform float _Edge_Width;
		uniform float _Mask_Clip_Value;
		uniform float4 _Contour;
		uniform float _Cutoff = 0.4;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float temp_output_5_0 = ( ( v.texcoord.xy.y - (-0.2 + (_Grow - 0.0) * (1.0 - -0.2) / (1.0 - 0.0)) ) - 0.0 );
			float3 ase_vertexNormal = v.normal.xyz;
			float3 temp_output_12_0 = ( ( temp_output_5_0 * ase_vertexNormal ) * _PushPull );
			v.vertex.xyz += temp_output_12_0;
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_MainTex = i.uv_texcoord * _MainTex_ST.xy + _MainTex_ST.zw;
			o.Albedo = tex2D( _MainTex, uv_MainTex ).rgb;
			float2 uv_TextureSample0 = i.uv_texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			float temp_output_5_0 = ( ( i.uv_texcoord.y - (-0.2 + (_Grow - 0.0) * (1.0 - -0.2) / (1.0 - 0.0)) ) - 0.0 );
			float4 temp_output_14_0 = ( tex2D( _TextureSample0, uv_TextureSample0 ) * ( 1.0 - temp_output_5_0 ) );
			float4 temp_cast_1 = (( _Edge_Width + _Mask_Clip_Value )).xxxx;
			o.Emission = ( step( temp_output_14_0 , temp_cast_1 ) * _Contour ).rgb;
			o.Alpha = 1;
			clip( temp_output_14_0.r - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18703
364;307;1318;477;1617.823;-274.6787;1.495189;True;True
Node;AmplifyShaderEditor.RangedFloatNode;1;-1956.039,-629.8352;Inherit;True;Property;_Grow;Grow;2;0;Create;True;0;0;False;0;False;0.07630336;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;2;-1662.836,-366.5355;Inherit;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;3;-1607.849,-626.0174;Inherit;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-0.2;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;4;-1312.438,-323.3354;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;5;-1074.739,-322.819;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;31;-392.3095,-606.1531;Inherit;False;Property;_Edge_Width;Edge_Width;6;0;Create;True;0;0;False;0;False;0;0.49;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;33;-414.4377,-501.4164;Inherit;False;Property;_Mask_Clip_Value;Mask_Clip_Value;7;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;9;-795.5435,-328.4815;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;10;-904.4246,-603.5995;Inherit;True;Property;_TextureSample0;Texture Sample 0;4;0;Create;True;0;0;False;0;False;-1;8d864b597abecaa42a89b6f0fe3e57ab;8d864b597abecaa42a89b6f0fe3e57ab;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;32;-8.808594,-500.7963;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;-446.0419,-404.1051;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.NormalVertexDataNode;6;-1143.424,-8.907511;Inherit;True;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StepOpNode;26;134.2792,-635.8307;Inherit;True;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;29;227.5023,-434.1028;Inherit;False;Property;_Contour;Contour;5;0;Create;True;0;0;False;0;False;0.4822135,0,1,0;0,0.23313,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;8;-659.4373,176.6149;Inherit;False;Property;_PushPull;PushPull;3;0;Create;True;0;0;False;0;False;-0.1;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-821.9225,-31.84186;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;-335.5512,14.58409;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;462.7341,-494.9587;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GradientNode;11;-1953.938,-777.8353;Inherit;False;0;2;2;1,1,1,0;0,0,0,1;1,0;1,1;0;1;OBJECT;0
Node;AmplifyShaderEditor.SamplerNode;13;439.2087,-800.2947;Inherit;True;Property;_MainTex;MainTex;0;0;Create;True;0;0;False;0;False;-1;None;c893e328c1326064cbace00da708c4c7;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;35;-572.2959,491.2575;Inherit;True;34;Destruction;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;34;-18.73911,191.7167;Inherit;True;Destruction;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;973.339,-346.1986;Float;False;True;-1;6;ASEMaterialInspector;0;0;Standard;Destroy_Barrier;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.4;True;True;0;True;TransparentCutout;;Geometry;ForwardOnly;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;True;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0.01;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;1;-1;-1;1;0;False;0;0;False;-1;-1;0;False;33;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;3;0;1;0
WireConnection;4;0;2;2
WireConnection;4;1;3;0
WireConnection;5;0;4;0
WireConnection;9;0;5;0
WireConnection;32;0;31;0
WireConnection;32;1;33;0
WireConnection;14;0;10;0
WireConnection;14;1;9;0
WireConnection;26;0;14;0
WireConnection;26;1;32;0
WireConnection;7;0;5;0
WireConnection;7;1;6;0
WireConnection;12;0;7;0
WireConnection;12;1;8;0
WireConnection;30;0;26;0
WireConnection;30;1;29;0
WireConnection;34;0;12;0
WireConnection;0;0;13;0
WireConnection;0;2;30;0
WireConnection;0;10;14;0
WireConnection;0;11;12;0
ASEEND*/
//CHKSM=040541B2DF3031FABA5F673086D069B1A24A724D