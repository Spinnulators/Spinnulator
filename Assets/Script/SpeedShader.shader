Shader "Custom/SpeedShader" {
	Properties {
		_MainTex ("", 2D) = "white" {}
	}

	SubShader {

	ZTest Always Cull Off ZWrite Off Fog { Mode Off } //Rendering settings

		Pass {
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#include "UnityCG.cginc"

			uniform float _Momentum;

			// Data structure to pass information from vertex shader to fragment shader
			struct v2f {
				float4 position : POSITION;
				float2 texCoord : TEXCOORD0;
			};

			v2f vert(appdata_base v) : POSITION {
				v2f output;
				output.position = mul (UNITY_MATRIX_MVP, v.vertex);
				output.texCoord = MultiplyUV (UNITY_MATRIX_TEXTURE0, v.texcoord.xy);
				return output;
			}
			
			sampler2D _MainTex; //Reference in Pass is necessary to let us use this variable in shaders
			
			// Takes in a v2f struct, and returns the colour for the texel with chromatic abberation
			fixed4 chromaticAbberation(v2f input) {
				fixed4 color;
				
			    float2 texel = 1.0 / _ScreenParams.xy;
			    
			    // The amount of chromatic abberation is dependent on momentum
			    float ChromaticAberration = _Momentum * 100.0;
			    
			    // Convert "(0, 0) to (1, 1)" texture coordinates into "(-1, -1) to (1, 1)" coordinates
			    float2 convertedCoords = (input.texCoord - 0.5) * 2.0;

			    // Calculate distorted texture coordinates with an offset
			    float2 texCoordRed = input.texCoord - texel.xy * convertedCoords * ChromaticAberration;
			    float2 texCoordBlue = input.texCoord + texel.xy * convertedCoords * ChromaticAberration ;
			    
			    // Get colours from distorted texture coordinates
			    color.r = tex2D(_MainTex, texCoordRed).r;
			    color.g = tex2D(_MainTex, input.texCoord).g;
			    color.b = tex2D(_MainTex, texCoordBlue).b;
			    
			    return color;
			}
			
			// Takes in a v2f struct, distorts the texture coordinates and returns it back
			v2f fisheye(v2f input) {
			
				// The distortion is inversely proportional to the momentum
				float distortionExponent = 1.2f / (_Momentum + 0.1f);

				// We calculate the center position of the screen
				const float2 centerPosition = float2(0.5f, 0.5f);
				
				// The center difference vector is the difference to the center
				float2 centerDistanceVector = centerPosition - input.texCoord;
				float centerDistance = length(centerDistanceVector);

				centerDistanceVector = normalize(centerDistanceVector);
				
				// Distort the texture coordinates 
				float distortionFactor = pow(centerDistance, distortionExponent);
				input.texCoord += centerDistanceVector * distortionFactor;

				return input;
			}

			//Our Fragment Shader
			fixed4 frag (v2f input) : COLOR{
				fixed4 color;
	
			    input = fisheye(input);
				color = chromaticAbberation(input);
			    
				return color;
			}

			ENDCG
		}
	}
}