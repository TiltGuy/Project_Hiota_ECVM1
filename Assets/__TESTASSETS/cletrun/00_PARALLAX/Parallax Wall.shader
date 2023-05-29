// Made with Amplify Shader Editor v1.9.1.3
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/Parallax BackWall"
{
	Properties
	{
		_Pics_Back("Pics_Back", 2D) = "white" {}
		_TextureSample2("Texture Sample 2", 2D) = "white" {}
		_WallsDestroyed("Walls Destroyed", 2D) = "white" {}
		_TextureSample4("Texture Sample 4", 2D) = "white" {}
		_TextureSample5("Texture Sample 5", 2D) = "white" {}
		_Filled("Filled", 2D) = "white" {}
		_Tiling_Walls("Tiling_Walls", Vector) = (0,0,0,0)
		_Tiling_WallsDestroy("Tiling_WallsDestroy", Vector) = (0,0,0,0)
		_Tiling_BackAndPics("Tiling_BackAndPics", Vector) = (7,1,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "AlphaTest+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#pragma target 5.0
		#pragma surface surf Standard keepalpha noshadow noambient nolightmap  nodirlightmap 
		struct Input
		{
			float2 uv_texcoord;
			float3 viewDir;
			INTERNAL_DATA
		};

		uniform sampler2D _Filled;
		uniform float2 _Tiling_BackAndPics;
		uniform sampler2D _Pics_Back;
		uniform sampler2D _WallsDestroyed;
		uniform float2 _Tiling_WallsDestroy;
		uniform sampler2D _TextureSample2;
		uniform sampler2D _TextureSample4;
		uniform sampler2D _TextureSample5;
		uniform float2 _Tiling_Walls;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Normal = float3(0,0,1);
			float2 uv_TexCoord145 = i.uv_texcoord * _Tiling_BackAndPics;
			float2 Offset140 = ( ( 0.0 - 1 ) * i.viewDir.xy * 1.0 ) + uv_TexCoord145;
			float2 Offset128 = ( ( 0.0 - 1 ) * i.viewDir.xy * 1.0 ) + uv_TexCoord145;
			float4 tex2DNode129 = tex2D( _Pics_Back, Offset128 );
			float4 lerpResult133 = lerp( tex2D( _Filled, Offset140 ) , tex2DNode129 , tex2DNode129.a);
			float2 uv_TexCoord49 = i.uv_texcoord * _Tiling_WallsDestroy;
			float2 Offset115 = ( ( 0.5 - 1 ) * i.viewDir.xy * 1.0 ) + uv_TexCoord49;
			float4 tex2DNode137 = tex2D( _WallsDestroyed, Offset115 );
			float4 lerpResult135 = lerp( lerpResult133 , tex2DNode137 , tex2DNode137.a);
			float2 Offset50 = ( ( 0.6 - 1 ) * i.viewDir.xy * 1.0 ) + uv_TexCoord49;
			float4 tex2DNode136 = tex2D( _TextureSample2, Offset50 );
			float4 lerpResult113 = lerp( lerpResult135 , tex2DNode136 , tex2DNode136.a);
			float2 Offset53 = ( ( 0.8 - 1 ) * i.viewDir.xy * 1.0 ) + uv_TexCoord145;
			float4 tex2DNode138 = tex2D( _TextureSample4, Offset53 );
			float4 lerpResult58 = lerp( lerpResult113 , tex2DNode138 , tex2DNode138.a);
			float2 uv_TexCoord151 = i.uv_texcoord * _Tiling_Walls;
			float2 Offset54 = ( ( 1.0 - 1 ) * i.viewDir.xy * 1.0 ) + uv_TexCoord151;
			float4 tex2DNode139 = tex2D( _TextureSample5, Offset54 );
			float4 lerpResult59 = lerp( lerpResult58 , tex2DNode139 , tex2DNode139.a);
			o.Emission = lerpResult59.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=19103
Node;AmplifyShaderEditor.CommentaryNode;127;-987.2633,1796.582;Inherit;False;634.7377;420.1881;Opacity;3;99;106;105;;1,1,1,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;99;-937.2631,1846.582;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,2.5;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;106;-932.5255,2042.77;Inherit;False;Property;_GradiantValues;GradiantValues;1;0;Create;True;0;0;0;False;0;False;2.03,1.65;1.55,1.48;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SmoothstepOpNode;105;-606.525,1962.77;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;2.56;False;2;FLOAT;1.87;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;124;2979.282,1576.544;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;2.56;False;2;FLOAT;1.87;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;126;2648.544,1460.355;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,2.5;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;125;2653.282,1656.544;Inherit;False;Property;_Vector0;Vector 0;2;0;Create;True;0;0;0;False;0;False;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SamplerNode;139;-849.4467,933.1866;Inherit;True;Property;_TextureSample5;Texture Sample 5;7;0;Create;True;0;0;0;False;0;False;-1;478e345c0026efe45be0e8b51456d191;478e345c0026efe45be0e8b51456d191;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;58;-471.4811,803.078;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;133;-451.1611,-207.4102;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;135;-430.2852,168.7215;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;59;-467.1551,1047.796;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;129;-871.1249,-133.2413;Inherit;True;Property;_Pics_Back;Pics_Back;3;0;Create;True;0;0;0;False;0;False;-1;86045c29db684424498f483089e8a3f9;86045c29db684424498f483089e8a3f9;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;137;-849.2241,112.6422;Inherit;True;Property;_WallsDestroyed;Walls Destroyed;5;0;Create;True;0;0;0;False;0;False;-1;49caf2115c173474a8b74f2c0a5f812d;49caf2115c173474a8b74f2c0a5f812d;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;141;-904.8343,-492.7616;Inherit;True;Property;_Filled;Filled;8;0;Create;True;0;0;0;False;0;False;-1;ed0e844f9c467904c9231f41b6b74d17;67eba19dcdaca9742a345494fb33c6f6;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;138;-850.7231,732.4713;Inherit;True;Property;_TextureSample4;Texture Sample 4;6;0;Create;True;0;0;0;False;0;False;-1;123415b7c2d1f124b916a5d7b7a18e03;702afd956f9064d468d43fadc2136989;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ParallaxMappingNode;128;-1219.799,-34.82867;Inherit;False;Normal;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ParallaxMappingNode;140;-1267.914,-437.9044;Inherit;False;Normal;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;113;-465.2411,539.0422;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;136;-851.3806,448.0792;Inherit;True;Property;_TextureSample2;Texture Sample 2;4;0;Create;True;0;0;0;False;0;False;-1;123415b7c2d1f124b916a5d7b7a18e03;123415b7c2d1f124b916a5d7b7a18e03;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;57;-1765.325,738.9677;Inherit;False;Tangent;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;301.177,1076.644;Float;False;True;-1;7;ASEMaterialInspector;0;0;Standard;Custom/Parallax BackWall;False;False;False;False;True;False;True;False;True;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;;0;False;;False;0;False;;0;False;;False;0;Masked;0.5;True;False;0;False;TransparentCutout;;AlphaTest;All;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;False;0;5;False;;10;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;0;-1;-1;-1;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.SimpleAddOpNode;152;85.91887,976.5104;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;154;-157.2323,806.7656;Inherit;False;Constant;_Color0;Color 0;21;0;Create;True;0;0;0;False;0;False;0.1792453,0.132743,0.132743,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ParallaxMappingNode;54;-1223.46,872.6854;Inherit;False;Normal;4;0;FLOAT2;0,0;False;1;FLOAT;1;False;2;FLOAT;1;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ParallaxMappingNode;53;-1221.945,698.7319;Inherit;False;Normal;4;0;FLOAT2;0,0;False;1;FLOAT;0.8;False;2;FLOAT;1;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ParallaxMappingNode;50;-1211.653,526.668;Inherit;False;Normal;4;0;FLOAT2;0,0;False;1;FLOAT;0.6;False;2;FLOAT;1;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ParallaxMappingNode;115;-1222.791,260.0748;Inherit;False;Normal;4;0;FLOAT2;0,0;False;1;FLOAT;0.5;False;2;FLOAT;1;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;49;-1750.737,576.0519;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;5,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;151;-1595.307,1034.001;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;4,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;145;-1753.453,373.1891;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;7,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;157;-1793.096,1061.608;Inherit;False;Property;_Tiling_Walls;Tiling_Walls;9;0;Create;True;0;0;0;False;0;False;0,0;4,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;155;-1980.618,598.7709;Inherit;False;Property;_Tiling_WallsDestroy;Tiling_WallsDestroy;10;0;Create;True;0;0;0;False;0;False;0,0;7,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;156;-1979.423,386.3689;Inherit;False;Property;_Tiling_BackAndPics;Tiling_BackAndPics;11;0;Create;True;0;0;0;False;0;False;7,1;7,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
WireConnection;105;0;99;2
WireConnection;105;1;106;1
WireConnection;105;2;106;2
WireConnection;124;0;126;2
WireConnection;124;1;125;1
WireConnection;124;2;125;2
WireConnection;139;1;54;0
WireConnection;58;0;113;0
WireConnection;58;1;138;0
WireConnection;58;2;138;4
WireConnection;133;0;141;0
WireConnection;133;1;129;0
WireConnection;133;2;129;4
WireConnection;135;0;133;0
WireConnection;135;1;137;0
WireConnection;135;2;137;4
WireConnection;59;0;58;0
WireConnection;59;1;139;0
WireConnection;59;2;139;4
WireConnection;129;1;128;0
WireConnection;137;1;115;0
WireConnection;141;1;140;0
WireConnection;138;1;53;0
WireConnection;128;0;145;0
WireConnection;128;3;57;0
WireConnection;140;0;145;0
WireConnection;140;3;57;0
WireConnection;113;0;135;0
WireConnection;113;1;136;0
WireConnection;113;2;136;4
WireConnection;136;1;50;0
WireConnection;0;2;59;0
WireConnection;152;0;154;0
WireConnection;152;1;59;0
WireConnection;54;0;151;0
WireConnection;54;3;57;0
WireConnection;53;0;145;0
WireConnection;53;3;57;0
WireConnection;50;0;49;0
WireConnection;50;3;57;0
WireConnection;115;0;49;0
WireConnection;115;3;57;0
WireConnection;49;0;155;0
WireConnection;151;0;157;0
WireConnection;145;0;156;0
ASEEND*/
//CHKSM=C1F427AA42834390C08990A6129CC6F841CEC66E