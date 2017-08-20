// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

//(yingyugang@gmail.com)
Shader "Custom/SimpleTexture" {
	Properties {
		_MainTex ("Base Texture ", 2D) = "white" {} 
        _Color ("Color", Color) = (0.2, 0.3, 1 ,1)
      	//_Emission("Emission",Range (0.5, 2)) = 1
    } 
    SubShader {
        Lighting Off 
        //Blend SrcAlpha OneMinusSrcAlpha
       // ZWrite Off 
        Fog { Mode Off } 
        Pass { 
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
            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				//o.color = v.color;
                // o.texcoord = TRANSFORM_TEX(v.texcoord,_HighLightTex);
                o.texcoord = v.texcoord;
                return o;
            }
            
            half4 frag (v2f i) : COLOR
            {
				half4 baseColor = tex2D(_MainTex, i.texcoord);
            	if(baseColor.a>0)
            	{
            		baseColor = lerp(baseColor,baseColor * _Color,baseColor.a);
            	}
            	return baseColor;
            }
			ENDCG
		}
	} 
	FallBack "Diffuse"
}
