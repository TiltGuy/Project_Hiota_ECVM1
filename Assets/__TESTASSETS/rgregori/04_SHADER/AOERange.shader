// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "AOERange"
{
	Properties
	{
		_Offset("Offset", Range( -1 , 1)) = 0.6
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_Vector2("Vector 2", Vector) = (2,2,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#pragma target 3.0
		#pragma surface surf Standard alpha:fade keepalpha noshadow exclude_path:deferred 
		struct Input
		{
			float3 worldPos;
			float2 uv_texcoord;
			float4 screenPos;
		};

		uniform sampler2D _TextureSample0;
		uniform float2 _Vector2;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _Offset;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 _Vector0 = float3(1.36,0,0);
			o.Albedo = _Vector0;
			o.Emission = _Vector0;
			float3 ase_worldPos = i.worldPos;
			float3 temp_output_5_0_g1 = ( ( ase_worldPos - float3(3,3,3) ) / 6.668048 );
			float dotResult8_g1 = dot( temp_output_5_0_g1 , temp_output_5_0_g1 );
			float2 temp_cast_0 = (_CosTime.w).xx;
			float2 uv_TexCoord18 = i.uv_texcoord + temp_cast_0;
			float2 panner40 = ( _CosTime.w * _Vector2 + uv_TexCoord18);
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float eyeDepth3 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float smoothstepResult14 = smoothstep( 0.0 , 1.0 , ( 1.0 - ( eyeDepth3 - ( ase_screenPos.w - _Offset ) ) ));
			o.Alpha = ( ( pow( saturate( dotResult8_g1 ) , 1.0 ) + tex2D( _TextureSample0, panner40 ) ) * smoothstepResult14 ).r;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18703
2141;138;1429;669;2639.681;699.9357;2.266971;True;True
Node;AmplifyShaderEditor.RangedFloatNode;11;-1227.336,1327.093;Inherit;False;Property;_Offset;Offset;0;0;Create;True;0;0;False;0;False;0.6;1;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenPosInputsNode;1;-1279.488,1067.163;Float;False;1;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CosTime;17;-2415.922,-33.87205;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScreenDepthNode;3;-1088.588,871.6747;Inherit;False;0;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;5;-924.843,1137.844;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;18;-2071.269,-96.32628;Inherit;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;41;-1995.006,294.994;Inherit;False;Property;_Vector2;Vector 2;2;0;Create;True;0;0;False;0;False;2,2;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;32;-1274.929,326.1553;Inherit;False;Constant;_Float0;Float 0;2;0;Create;True;0;0;False;0;False;6.668048;0;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;31;-1278.128,110.9555;Inherit;False;Constant;_Vector1;Vector 1;2;0;Create;True;0;0;False;0;False;3,3,3;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.PannerNode;40;-1610.195,-12.71632;Inherit;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;33;-1292.528,473.3553;Inherit;False;Constant;_Float1;Float 1;2;0;Create;True;0;0;False;0;False;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;4;-802.0258,949.4741;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;15;-1232.419,-268.6623;Inherit;True;Property;_TextureSample0;Texture Sample 0;1;0;Create;True;0;0;False;0;False;-1;da8f1c2ca73fa7849812d48c57b95295;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;12;-615.0143,1005.751;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;34;-950.6788,275.9613;Inherit;True;SphereMask;-1;;1;988803ee12caf5f4690caee3c8c4a5bb;0;3;15;FLOAT3;0,0,0;False;14;FLOAT;0;False;12;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;14;-374.1906,993.1678;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;42;-455.7571,280.5591;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;-519.4557,-358.4974;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.Vector3Node;7;-368.978,-362.973;Inherit;True;Constant;_Vector0;Vector 0;1;0;Create;True;0;0;False;0;False;1.36,0,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.BillboardNode;26;-702.1688,-266.1478;Inherit;False;Cylindrical;False;0;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;35;-162.6982,339.1321;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TimeNode;47;-1724.782,-251.7224;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;10;0,0;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;AOERange;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;ForwardOnly;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;1;3;False;-1;1;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;1;0;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;5;0;1;4
WireConnection;5;1;11;0
WireConnection;18;1;17;4
WireConnection;40;0;18;0
WireConnection;40;2;41;0
WireConnection;40;1;17;4
WireConnection;4;0;3;0
WireConnection;4;1;5;0
WireConnection;15;1;40;0
WireConnection;12;0;4;0
WireConnection;34;15;31;0
WireConnection;34;14;32;0
WireConnection;34;12;33;0
WireConnection;14;0;12;0
WireConnection;42;0;34;0
WireConnection;42;1;15;0
WireConnection;30;1;26;0
WireConnection;35;0;42;0
WireConnection;35;1;14;0
WireConnection;10;0;7;0
WireConnection;10;2;7;0
WireConnection;10;9;35;0
ASEEND*/
//CHKSM=22A6BEFB7E423478395D22AA7781D5DAA568D43C