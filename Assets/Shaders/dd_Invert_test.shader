//Our main shader block
Shader "ddShaders/dd_Invert_test" //This defines where in the material shader menu the shader exists, and also what name it has 
{
	//ShaderLab Property Block
	Properties 
	{
		_TintColor("Tint Color", Color) = (1,1,1,1) //Color tint for the final effect

		[Header(Textures)] //Property block title text
		_NoiseTex("Noise", 2D) = "white" {} //Noise texture (for the glitch)
		_MaskTex("Mask (Alpha Only)", 2D) = "white" {} //Mask texture (where the noise shows up)
		_RandomNoiseTex("Random Noise Tex", 2d) = "white" {}

		[Header(Glitch Effect)] //Property block title text
		_NoiseBrightness("Noise Brightness", Float) = 4 //How bright the noise texture is
		_NoiseContrast("Noise Contrast", Float) = 2 //How contrasty/crunchy the noise texture is. This also visually controls how present the glitch effect is
		_NoiseMoveDirection("Noise Move Direction", Vector) = (1.0, 1.0, 1.0, 1.0) //The direction in which the noise texture is moved in (the higher the values, the faster it also goes)

		//material toggle basically turns this into a checkbox, there is no actual boolean type in the unity shaderlab code (but they do exist in HLSL code)
		[MaterialToggle] _NoiseGrowingEnabled("Enable Noise Scale Growth", Float) = 0 //Enables/Disables the noise shrinking/growth
		_NoiseGrowFrequency("Noise Growing Frequency", Float) = 8 //How fast the noise grows and shrinks in size
		_NoiseGrowAmount("Noise Grow Amount", Float) = 3 //How much the noise grows
		_OffsetFrequencyDampener("Offset Frequency Dampener", Float) = 20 //How much the noise grows
	}
	
	//ShaderLab SubShader Block (you can have mutliple sub shaders in a shader) - https://docs.unity3d.com/Manual/SL-SubShader.html
	SubShader 
	{
		//The render queue 
		Tags { "Queue"="Transparent" }

		Pass
		{
		   ZWrite On //Write to the depth buffer
		   ColorMask 0 //Render to render target 0, which is the main one (refer to here if you just want more info on what the heck this is - https://docs.unity3d.com/Manual/SL-ColorMask.html)
		}

		//THIS IS WHAT DOES THE INVERTED COLOR EFFECT
        Blend OneMinusDstColor OneMinusSrcAlpha //inverted blending, so long as the foreground color is 1,1,1,1 or white with no alpha - https://docs.unity3d.com/Manual/SL-Blend.html
        BlendOp Add //We will use Add to blend, which means we are adding our computed colors onto the scene - https://docs.unity3d.com/Manual/SL-BlendOp.html
        
		//MAIN SHADER PASS - https://docs.unity3d.com/Manual/SL-Pass.html
        Pass
		{
			CGPROGRAM //CG SHADER CODE START
			#pragma vertex vert //vertex function
			#pragma fragment frag //fragment function
			#include "UnityCG.cginc" //to use TRANSFORM_TEX

			//Define our variables from the property block in here in the main shader code
			sampler2D _MaskTex;         //Mask texture (where the noise shows up)
			sampler2D _NoiseTex;        //Noise texture (for the glitch)
			sampler2D _RandNoiseTex;
			float4 _MaskTex_ST;         //Texture Tiling and Offsets (the 4 values in the unity inspector next to the texture)
			float4 _NoiseTex_ST;        //Texture Tiling and Offsets (the 4 values in the unity inspector next to the texture)
			sampler2D _RandNoiseTex_ST;

			float4 _TintColor;          //Color tint for the final effect
			float _NoiseBrightness;     //How bright the noise texture is
			float _NoiseContrast;       //How contrasty/crunchy the noise texture is. This also visually controls how present the glitch effect is
			float4 _NoiseMoveDirection; //The direction in which the noise texture is moved in (the higher the values, the faster it also goes)

			float _NoiseGrowingEnabled; //Enables/Disables the noise shrinking/growth
			float _NoiseGrowFrequency;  //How fast the noise grows and shrinks in size
			float _NoiseGrowAmount;     //How much the noise grows
			float _OffsetFrequencyDampener;     //How much the noise grows

			//need to define some vertex input values for the shader (input because unity will assign these specific values from the mesh/polygon itself)
			struct vertexInput
			{
				float4 vertex: POSITION;    //Vertex position
				float4 color : COLOR;       //Vertex color
				float4 texcoord : TEXCOORD; //UV Coordinates (how a texture is mapped on a polygon)
			};

			//need to also define some fragment (pixel) input values for the shader (we have to assign these values ourselves)
			struct fragmentInput
			{
				float4 pos : SV_POSITION; //VERTEX POSITION
				float4 color : COLOR0;    //VERTEX COLOR
				float2 uv : TEXCOORD0;    //UV Coordinates 1
				float2 uv2 : TEXCOORD1;   //UV Coordinates 2
			};

			//vertex function (per vertex)
			fragmentInput vert( vertexInput i )
			{
				//bring in the fragment struct since we will assign values to it
				fragmentInput o;

				//assign the vertex position values (using a unity built in function to do that)
				o.pos = UnityObjectToClipPos(i.vertex);

				//assign our uv coordinate values
				o.uv = TRANSFORM_TEX(i.texcoord, _NoiseTex); //uv coordinates specific to the noise texture map
				o.uv2 = TRANSFORM_TEX(i.texcoord, _MaskTex); //uv coordinates specific to the mask texture map

				//return the fragment struct after now assigning values to each of its fields
				return o;
			}

			//fragment/pixel shader (per pixel)
			half4 frag(fragmentInput i) : COLOR
			{

				float2 noiseCoordinates = i.uv; //lets use our first UV Coordinates 1 set
				//noiseCoordinates = float2(i.uv.x + (_Time.y * _NoiseMoveDirection.x), i.uv.y + (_Time.y * _NoiseMoveDirection.y)); //modify the coordinates so we move the texture around and use the time value to change the values over time
				noiseCoordinates = float2(i.uv.x + (_Time.y * _NoiseMoveDirection.x), i.uv.y + (_Time.y * _NoiseMoveDirection.y));

				float2 offset = float2(
					//tex2D(_RandNoiseTex, float2(0, _OffsetFrequencyDampener)).r, // read a color from the Random Noise Texture
					10 * _Time.y / 10 * _OffsetFrequencyDampener,
					0
					//tex2D(_RandNoiseTex, float2(0, sin(i.pos.y) * _Time.y)).r
				);
				//offset = float2(i.uv.x, i.uv.y);

				noiseCoordinates += offset;

				//if(_NoiseGrowingEnabled > 0) //if the user toggled the option to have the noise grow and shrink (material toggle, if checked = 1, if not checked = 0)
					//noiseCoordinates *= sin(_Time.y * _NoiseGrowFrequency) * _NoiseGrowAmount; //MULTIPLY the coordinates so we can grow or shrink the texture map based on time using a sin wave

				//float4 noiseTexture = tex2D(_NoiseTex, noiseCoordinates); //sample our main noise texture with the modified noise coordinates
				float4 noiseTexture = tex2D(_NoiseTex, noiseCoordinates); //sample our main noise texture with the modified noise coordinates
				float maskAlphaTexture = tex2D(_MaskTex, i.uv2).a; //sample our mask texture, in this case we are just sampling a single channel, the alpha channel in this instance USING a second set of unmodified coordinates

				noiseTexture *= _NoiseBrightness; //intensify the color values (making the image brighter)
				noiseTexture -= _NoiseContrast; //subtract from the color values (adding contrast to the image)
				noiseTexture = saturate(noiseTexture); //clamp the colors into a 0 - 1 range (so we don't get wierd unintended artifacts with values less than 0 making whatever is behind the shader darker)
				//noiseTexture *= maskAlphaTexture; //multiply the noise texture by the alpha channel of our mask texture (in the alpha channel, transparent pixels are 0, opaque pixels are 1, so it will make whatever we don't want 0 so the noise effect doesn't show up))

				float4 finalColor = float4(noiseTexture.r, noiseTexture.r, noiseTexture.r, noiseTexture.r); //lets get our final color (just sampling the red channel of the noise texture in every color channel to make it monochromatic)
				finalColor *= _TintColor; //tint our final effect using the tint color

				return finalColor; //return the final computed color value
			}
			ENDCG //CG SHADER CODE END
		}
	}
}

