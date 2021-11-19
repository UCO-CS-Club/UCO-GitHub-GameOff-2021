Shader "Glitch/GravGlitchWarning" {
	Properties {
	  _MainTex("Texture", 2D) = "white" {}
	  _waveLength("Wave Length", Float) = 500 
	  _waverHeight("Wave Height", Float) = 0.005
	  _speed("Speed", Float) = 20
	}

	SubShader {
		Pass {
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#include "UnityCG.cginc" // required for v2f_img

			// Properties
			sampler2D _MainTex;

			float _waveLength;  
			float _waveHeight;
			float _speed;

			float4 frag(v2f_img IN) : COLOR{
				//float x = sin(IN.uv.x / 0.005 + _Time[1] * _speed) / 200;
				float x = sin(IN.uv.x / _waveHeight + _Time[1] * _speed) / _waveLength;

				float4 base = tex2D(_MainTex, IN.uv + float2(0, x)); // we want to change this pixel s.t it's y position is the same but x position is shifted

				return base;
			}
			ENDCG
		}	
	}
}
