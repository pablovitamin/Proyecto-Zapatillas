Shader "Unlit/Desintegracion"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_NoiseTex ("Texture", 2D) = "white" {}
		_BurnTex ("Texture", 2D) = "white" {}
		_Color ("_Color", Color) = (1.0,1.0,1.0,1.0)
		_Desintegracion ("_Desintegracion", Range (0.0, 1)) = 0.0
		_BurnSize ("Burn Size", Range(0.0, 1.0)) = 0.01
	}
	SubShader  
	{
		Tags { "Queue" = "Transparent" }

		LOD 100

		ZWrite Off
		//blendea la transparencia, por lo tanto arregla el problema que tenia con la transparencia
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			
			#include "UnityCG.cginc"

	
			

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			sampler2D _NoiseTex;
			sampler2D _BurnTex;
			float _Desintegracion;
			float _BurnSize;
			float4 _Color;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{


				// sample the texture
				//fixed4 col = tex2D(_MainTex, i.uv);

				fixed4 col = tex2D(_MainTex, i.uv);

				clip(tex2D(_NoiseTex,i.uv).rgb - _Desintegracion);
				half test = tex2D(_NoiseTex,i.uv).rgb - _Desintegracion;
			 	if(test < _BurnSize && _Desintegracion > 0 && _Desintegracion < 1){
			 	float4 emi = tex2D(_BurnTex, float2(test *(1/_BurnSize), 0));
			 		//multiplico el color final por el degradado
			 		col *= emi;
				}
				 

				 
				// apply fog
				//UNITY_OPAQUE_ALPHA(col.a);
				return col * _Color;
			}
			ENDCG
		}
	}
}
