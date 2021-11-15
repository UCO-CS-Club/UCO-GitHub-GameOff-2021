// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "ddShaders/dd_Invert" {
Properties 
	{;
		_Color ("Tint Color", Color) = (1,1,1,1);
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_NoiseTex("Noise", 2D) = "white" {}
		_MaskTex("Mask (Alpha Only)", 2D) = "white" {}
		_Brightness("Brightness", Float) = 1;
		_Contrast("Contrast", Float) = 1;
		_Test("Test1", Float) = 1;
		_MoveDirection("Move Direction", Vector) = (1.0, 1.0, 1.0, 1.0);
	}
	
	// UV is like a coordinate plane. For mapping textures
	
	SubShader 
	{
		Tags { "Queue"="Transparent" }

		Pass
		{
		   ZWrite On
		   ColorMask 0
		}
        Blend OneMinusDstColor OneMinusSrcAlpha //invert blending, so long as FG color is 1,1,1,1
        BlendOp Add
        
        Pass
		{ 
		
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag 
			#include "UnityCG.cginc"

			uniform float4 _Color;
			sampler2D _NoiseTex;
			sampler2D _MaskTex

			// can use to adjust tiling size
			float4 _NoiseTex_ST;
			float4 _MaskTex_ST;
			float4 _MoveDirection;

			float _Brightness;
			float _Contrast;
			

			struct vertexInput
			{
				float4 vertex: POSITION;
				float4 color : COLOR;	
				float4 texcoord : TEXCOORD;
			};

			struct fragmentInput
			{
				float4 pos : SV_POSITION;
				float4 color : COLOR0; 
				float2 uv : TEXCOORD0;
				float2 uv2 : TEXCOORD1;
			};

			// per vertex
			fragmentInput vert( vertexInput i )
			{
				fragmentInput o;
				o.pos = UnityObjectToClipPos(i.vertex);

				o.uv = TRANSFORM_TEX(i.texcoord, _NoiseTex);
				o.uv2 = TRANSFORM_TEX(i.texcoord, _MaskTex);

				return o;
			}


			// R G B A
			// 0.0 no color, 1.0 maxed color 
			// 
			// 
			// per pixel
			// pixel shader = better
			half4 frag(fragmentInput i) : COLOR
			{
				// float float float
				float4 mainColor = _Color;
				float2 coords = float2(i.uv.x + (_Time.y + _MoveDirection.x), i.uv.y + (_Time.y + _MoveDirection.y));
				float4 noiseTex = tex2D(_NoiseTex, coords * cos(_Time.y * _Test));
				float maskAlpha = tex2D(_MaskTex, i.uv).a;

				noiseTex *= _Brightness;
				noiseTex -= _Contrast; // subract from 

				// this clamps the value back down to range
				noiseTex = saturate(noiseTex);



				// mainColor.xyzw or mainColor.rgba
				//mainColor.r

				return float4(noiseText.r, noiseText.r, noiseText.r, maskAlpha);
			}

			ENDCG
		}
	}
}