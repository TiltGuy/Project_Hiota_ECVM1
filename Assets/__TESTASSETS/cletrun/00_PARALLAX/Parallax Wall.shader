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
		uniform sampler2D _Pics_Back;
		uniform sampler2D _WallsDestroyed;
		uniform sampler2D _TextureSample2;
		uniform sampler2D _TextureSample4;
		uniform sampler2D _TextureSample5;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Normal = float3(0,0,1);
			float2 uv_TexCoord145 = i.uv_texcoord * float2( 7,1 );
			float2 Offset140 = ( ( 0.0 - 1 ) * i.viewDir.xy * 1.0 ) + uv_TexCoord145;
			float2 Offset128 = ( ( 0.0 - 1 ) * i.viewDir.xy * 1.0 ) + uv_TexCoord145;
			float4 tex2DNode129 = tex2D( _Pics_Back, Offset128 );
			float4 lerpResult133 = lerp( tex2D( _Filled, Offset140 ) , tex2DNode129 , tex2DNode129.a);
			float2 uv_TexCoord49 = i.uv_texcoord * float2( 5,1 );
			float2 Offset115 = ( ( 0.5 - 1 ) * i.viewDir.xy * 1.0 ) + uv_TexCoord49;
			float4 tex2DNode137 = tex2D( _WallsDestroyed, Offset115 );
			float4 lerpResult135 = lerp( lerpResult133 , tex2DNode137 , tex2DNode137.a);
			float2 Offset50 = ( ( 0.5 - 1 ) * i.viewDir.xy * 1.0 ) + uv_TexCoord49;
			float4 tex2DNode136 = tex2D( _TextureSample2, Offset50 );
			float4 lerpResult113 = lerp( lerpResult135 , tex2DNode136 , tex2DNode136.a);
			float2 Offset53 = ( ( 0.8 - 1 ) * i.viewDir.xy * 1.0 ) + uv_TexCoord145;
			float4 tex2DNode138 = tex2D( _TextureSample4, Offset53 );
			float4 lerpResult58 = lerp( lerpResult113 , tex2DNode138 , tex2DNode138.a);
			float2 uv_TexCoord151 = i.uv_texcoord * float2( 4,1 );
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
Node;AmplifyShaderEditor.CommentaryNode;20;-3522.197,-434.12;Inherit;False;1783.557;1624.035;Example;7;12;11;5;4;17;9;15;;1,1,1,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;99;-937.2631,1846.582;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,2.5;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;12;-3089.703,654.2173;Inherit;False;743.1398;520.6328;InFront;3;8;6;19;;1,1,1,1;0;0
Node;AmplifyShaderEditor.Vector2Node;106;-932.5255,2042.77;Inherit;False;Property;_GradiantValues;GradiantValues;13;0;Create;True;0;0;0;False;0;False;2.03,1.65;1.55,1.48;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.CommentaryNode;11;-3120.087,-384.12;Inherit;False;761;372;Back;2;2;1;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;17;-3104.808,63.3984;Inherit;False;760.4933;495.533;middle?;3;13;14;18;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;38;-2621.619,2812.498;Inherit;True;Property;_04;04;12;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;19;-2682.955,726.1613;Inherit;True;Property;_ed84ef97b620c03e1e238e706d6ba7a2c;ed84ef97b620c03e1e238e706d6ba7a2c;4;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SmoothstepOpNode;105;-606.525,1962.77;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;2.56;False;2;FLOAT;1.87;False;1;FLOAT;0
Node;AmplifyShaderEditor.ParallaxMappingNode;29;-2973.832,2839.103;Inherit;False;Normal;4;0;FLOAT2;1,1;False;1;FLOAT;0.94;False;2;FLOAT;1;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;32;-1951.502,1968.802;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;23;-2678.845,1281.433;Inherit;True;Property;_01;01;6;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Cube;8;0;SAMPLERCUBE;;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-2736.087,-242.1202;Inherit;True;Property;_ed84ef97b620c03e1e238e706d6ba7a2b;ed84ef97b620c03e1e238e706d6ba7a2b;1;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SmoothstepOpNode;124;2979.282,1576.544;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;2.56;False;2;FLOAT;1.87;False;1;FLOAT;0
Node;AmplifyShaderEditor.ParallaxMappingNode;13;-3054.808,140.7057;Inherit;False;Normal;4;0;FLOAT2;1,1;False;1;FLOAT;0.96;False;2;FLOAT;1;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;22;-3469.503,1754.983;Inherit;False;Tangent;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ParallaxMappingNode;27;-2987.809,2254.86;Inherit;False;Normal;4;0;FLOAT2;1,1;False;1;FLOAT;0.96;False;2;FLOAT;1;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ParallaxMappingNode;6;-3039.703,760.6503;Inherit;False;Normal;4;0;FLOAT2;1,1;False;1;FLOAT;1;False;2;FLOAT;1;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ParallaxMappingNode;24;-2998.26,1305.756;Inherit;False;Normal;4;0;FLOAT2;1,1;False;1;FLOAT;0.99;False;2;FLOAT;1;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;33;-1709.303,2240.688;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;15;-2003.64,104.3086;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;9;-2269.946,-253.4296;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0.2765664,0.3138022,0.5188679,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ParallaxMappingNode;2;-3070.088,-334.1201;Inherit;False;Normal;4;0;FLOAT2;1,1;False;1;FLOAT;0.9;False;2;FLOAT;1;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ParallaxMappingNode;25;-2988.727,1681.963;Inherit;False;Normal;4;0;FLOAT2;1,1;False;1;FLOAT;0.98;False;2;FLOAT;1;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;34;-2639.92,3048.717;Inherit;True;Property;_04_Mask;04_Mask;8;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Cube;8;0;SAMPLERCUBE;;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;14;-2721.314,113.3984;Inherit;True;Property;_ed84ef97b620c03e1e238e706d6ba7a2d;ed84ef97b620c03e1e238e706d6ba7a2d;3;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;4;-3472.197,-44.36562;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;8;-2713.479,959.9149;Inherit;True;Property;_PlatformArenaLOW_DefaultMaterial_Mask2;Platform-Arena-LOW_DefaultMaterial_Mask2;2;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;37;-2676.77,1659.302;Inherit;True;Property;_02;02;10;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Cube;8;0;SAMPLERCUBE;;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;31;-2169.454,1680.166;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;126;2648.544,1460.355;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,2.5;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;125;2653.282,1656.544;Inherit;False;Property;_Vector0;Vector 0;14;0;Create;True;0;0;0;False;0;False;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SamplerNode;18;-2695.236,313.5577;Inherit;True;Property;_Masktets;Mask tets;5;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;5;-3425.197,180.6345;Inherit;False;Tangent;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SamplerNode;36;-2654.199,1904.946;Inherit;True;Property;_02_mask;02_mask;11;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Cube;8;0;SAMPLERCUBE;;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;35;-2624.4,2450.684;Inherit;True;Property;_03_mask;03_mask;9;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Cube;8;0;SAMPLERCUBE;;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;28;-2627.514,2231.271;Inherit;True;Property;_03;03;7;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;21;-3506.522,1492.552;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;139;-849.4467,933.1866;Inherit;True;Property;_TextureSample5;Texture Sample 5;19;0;Create;True;0;0;0;False;0;False;-1;478e345c0026efe45be0e8b51456d191;478e345c0026efe45be0e8b51456d191;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;58;-471.4811,803.078;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;133;-451.1611,-207.4102;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;135;-430.2852,168.7215;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;59;-467.1551,1047.796;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;129;-871.1249,-133.2413;Inherit;True;Property;_Pics_Back;Pics_Back;15;0;Create;True;0;0;0;False;0;False;-1;86045c29db684424498f483089e8a3f9;86045c29db684424498f483089e8a3f9;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;137;-849.2241,112.6422;Inherit;True;Property;_WallsDestroyed;Walls Destroyed;17;0;Create;True;0;0;0;False;0;False;-1;49caf2115c173474a8b74f2c0a5f812d;49caf2115c173474a8b74f2c0a5f812d;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;141;-904.8343,-492.7616;Inherit;True;Property;_Filled;Filled;20;0;Create;True;0;0;0;False;0;False;-1;ed0e844f9c467904c9231f41b6b74d17;67eba19dcdaca9742a345494fb33c6f6;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;138;-850.7231,732.4713;Inherit;True;Property;_TextureSample4;Texture Sample 4;18;0;Create;True;0;0;0;False;0;False;-1;123415b7c2d1f124b916a5d7b7a18e03;702afd956f9064d468d43fadc2136989;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ParallaxMappingNode;54;-1223.46,872.6854;Inherit;False;Normal;4;0;FLOAT2;0,0;False;1;FLOAT;1;False;2;FLOAT;1;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ParallaxMappingNode;53;-1221.945,698.7319;Inherit;False;Normal;4;0;FLOAT2;0,0;False;1;FLOAT;0.8;False;2;FLOAT;1;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;145;-1698.589,367.3108;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;7,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;151;-1595.307,1034.001;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;4,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ParallaxMappingNode;115;-1222.791,262.0748;Inherit;False;Normal;4;0;FLOAT2;0,0;False;1;FLOAT;0.5;False;2;FLOAT;1;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ParallaxMappingNode;50;-1212.653,526.668;Inherit;False;Normal;4;0;FLOAT2;0,0;False;1;FLOAT;0.5;False;2;FLOAT;1;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ParallaxMappingNode;128;-1219.799,-34.82867;Inherit;False;Normal;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ParallaxMappingNode;140;-1267.914,-437.9044;Inherit;False;Normal;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;113;-465.2411,539.0422;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;136;-851.3806,448.0792;Inherit;True;Property;_TextureSample2;Texture Sample 2;16;0;Create;True;0;0;0;False;0;False;-1;123415b7c2d1f124b916a5d7b7a18e03;123415b7c2d1f124b916a5d7b7a18e03;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;57;-1765.325,738.9677;Inherit;False;Tangent;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.TextureCoordinatesNode;49;-1780.129,576.0519;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;5,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;82.95158,1076.644;Float;False;True;-1;7;ASEMaterialInspector;0;0;Standard;Custom/Parallax BackWall;False;False;False;False;True;False;True;False;True;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;;0;False;;False;0;False;;0;False;;False;0;Masked;0.5;True;False;0;False;TransparentCutout;;AlphaTest;All;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;False;0;5;False;;10;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;0;-1;-1;-1;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;38;1;29;0
WireConnection;19;1;6;0
WireConnection;105;0;99;2
WireConnection;105;1;106;1
WireConnection;105;2;106;2
WireConnection;29;0;21;0
WireConnection;29;3;22;0
WireConnection;32;0;31;0
WireConnection;32;1;28;0
WireConnection;32;2;35;1
WireConnection;23;1;24;0
WireConnection;1;1;2;0
WireConnection;124;0;126;2
WireConnection;124;1;125;1
WireConnection;124;2;125;2
WireConnection;13;0;4;0
WireConnection;27;0;21;0
WireConnection;27;3;22;0
WireConnection;6;0;4;0
WireConnection;6;3;5;0
WireConnection;24;0;21;0
WireConnection;24;3;22;0
WireConnection;33;0;32;0
WireConnection;33;1;38;0
WireConnection;33;2;34;1
WireConnection;15;0;9;0
WireConnection;15;1;14;0
WireConnection;15;2;18;1
WireConnection;9;0;1;0
WireConnection;9;1;19;0
WireConnection;9;2;8;1
WireConnection;2;0;4;0
WireConnection;2;3;5;0
WireConnection;25;0;21;0
WireConnection;25;3;22;0
WireConnection;34;1;29;0
WireConnection;14;1;13;0
WireConnection;8;1;6;0
WireConnection;37;1;25;0
WireConnection;31;0;23;0
WireConnection;31;1;37;0
WireConnection;31;2;36;1
WireConnection;18;1;13;0
WireConnection;36;1;25;0
WireConnection;35;1;27;0
WireConnection;28;1;27;0
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
WireConnection;54;0;151;0
WireConnection;54;3;57;0
WireConnection;53;0;145;0
WireConnection;53;3;57;0
WireConnection;115;0;49;0
WireConnection;115;3;57;0
WireConnection;50;0;49;0
WireConnection;50;3;57;0
WireConnection;128;0;145;0
WireConnection;128;3;57;0
WireConnection;140;0;145;0
WireConnection;140;3;57;0
WireConnection;113;0;135;0
WireConnection;113;1;136;0
WireConnection;113;2;136;4
WireConnection;136;1;50;0
WireConnection;0;2;59;0
ASEEND*/
//CHKSM=A1199DCF90DFE5EB280229366F74D1C6152FD001