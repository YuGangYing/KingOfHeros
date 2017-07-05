Shader "Hidden/CameraAlphaEffect" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_Alpha("Alpha", float) = 0.5
}

SubShader {
	Pass {
		ZTest Always Cull Off ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		Fog { Mode off }
				
		CGPROGRAM
		#pragma vertex vert_img
		#pragma fragment frag
		#pragma fragmentoption ARB_precision_hint_fastest 
		#include "UnityCG.cginc"

		uniform sampler2D _MainTex;
		uniform half _Alpha;

		fixed4 frag (v2f_img i) : COLOR
		{	
			fixed4 original = tex2D(_MainTex, i.uv);
 			fixed4 output = original;
			output.a = _Alpha;
			return output;
		}
		ENDCG

	}
}

Fallback off

}