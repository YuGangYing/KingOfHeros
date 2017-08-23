// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

//应彧刚(yingyugang@gmail.com)
Shader "Custom/OverlayBlendAlpha" 
{
    Properties {
		_MainTex ("Base Texture ", 2D) = "white" {} 
        _OverlayTex("Mask",2D) = ""{}//R：定义变色的区域;G：高光强度，范围;B：反射强度，范围;CubeMap A：透明通道;
        _Color ("Mask Color", Color) = (0.2, 0.3, 1 ,1)
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
            sampler2D _OverlayTex;
            uniform half4 _Color;          
			uniform half _Emission;
			
            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.color = v.color;
               // o.texcoord = TRANSFORM_TEX(v.texcoord,_HighLightTex);
                o.texcoord = v.texcoord;
                return o;
            }
            half4 frag (v2f i) : COLOR
            {
				half4 baseColor = tex2D(_MainTex, i.texcoord);
				half4 overlayColor = tex2D(_OverlayTex, i.texcoord);
                half4 texColor = overlayColor.r * _Color;   
                half aCol = 0;
               	if(baseColor.x < 0.5)
               	{
               		aCol = 2*baseColor.x*texColor.x;
               	}
               	else
               	{
               		aCol = 1-2*(1-baseColor.x)*(1-texColor.x);
               	}
               	half bCol = 0;
               	if(baseColor.y < 0.5)
               	{
               		bCol = 2*baseColor.y*texColor.y;
               	}
               	else
               	{
               		bCol = 1-2*(1-baseColor.y)*(1-texColor.y);
               	} 
                half cCol = 0;
               	if(baseColor.z < 0.5)
               	{
               		cCol = 2*baseColor.z*texColor.z;
               	}
               	else
               	{
               		cCol = 1-2*(1-baseColor.z)*(1-texColor.z);
               	}
            	//return lerp(float4(aCol,bCol,cCol,1),baseColor,1 - gray)*_Emission;
            	return lerp(half4(aCol,bCol,cCol,1),baseColor,1 - overlayColor.r) * (1 + overlayColor.g);
            }
            ENDCG 
        }
    }   
    Fallback "VertexLit" 
}