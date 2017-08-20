// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

//(yingyugang@gmail.com)
Shader "Custom/SimpleTextureTrans" {
	Properties {
		_MainTex ("Base Texture ", 2D) = "white" {} 
        _Color ("Color", Color) = (0.2, 0.3, 1 ,1)
      	//_Emission("Emission",Range (0.5, 2)) = 1
    } 
    SubShader {
		Tags { "Queue" = "Transparent" } 
		 // draw after all opaque geometry has been drawn
		Pass {
		ZWrite Off // don't write to depth buffer 
		// in order not to occlude other objects

		Blend SrcAlpha OneMinusSrcAlpha // use alpha blending

		CGPROGRAM 

		#pragma vertex vert 
		#pragma fragment frag
		#include "UnityCG.cginc"
		struct appdata_t {
       	    half4 vertex : POSITION;
       	    half4 color : COLOR;
       		half2 texcoord : TEXCOORD0;
	    };
	    struct v2f {
	        half4 vertex : POSITION;
	        half4 color : COLOR;
	        half2 texcoord : TEXCOORD0;
	    };
     	sampler2D _MainTex;
    	uniform half4 _Color; 
		v2f vert(appdata_t v)
		{
			v2f o;
	        o.vertex = UnityObjectToClipPos(v.vertex);
	        o.texcoord = v.texcoord;
	        return o;
		}

		float4 frag(v2f i) : COLOR 
		{
			half4 baseColor = tex2D(_MainTex, i.texcoord);
	    	
	    	
	    	baseColor.a = baseColor.g;
	    	baseColor.xyz = baseColor.xyz * _Color.xyz;
			return baseColor; 
		}

		ENDCG  
      }
   }
	FallBack "Diffuse"
}
