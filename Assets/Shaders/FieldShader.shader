// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Editor/FieldShader"
{
	SubShader
	{
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha, One One
			ZWrite Off
			ZTest Always
			Cull Back

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			struct appdata_t {
				float4 vertex : POSITION;
				float4 color : COLOR;
			};
			struct v2f {
				fixed4 color : COLOR;
				float4 vertex : SV_POSITION;
			};
			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.color = v.color;
				return o;
			}
			fixed4 frag (v2f i) : SV_Target
			{
				return i.color;
			}
			ENDCG  
		}  
	}
    FallBack "Diffuse"
}
