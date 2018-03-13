// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Wasabi/EffectShader" {
	Properties {
		_MainTex ("MainTexture", 2D) = "white" {}
		_EffectTex("EffectTexture", 2D) = "white" {}
		_DefaultShadowColor("ShadowColor", Color) = (0.6, 0.6, 0.6, 1.0)

			_Color0("EffectColor_0", Color) = (0.0, 0.0, 0.0, 1.0)
			[MaterialToggle] _Eff0("On_Eff0", Float) = 0
			_Color1("EffectColor_1", Color) = (0.0, 0.0, 0.0, 1.0)
			[MaterialToggle] _Eff1("On_Eff1", Float) = 0
			_Color2("EffectColor_2", Color) = (0.0, 0.0, 0.0, 1.0)
			[MaterialToggle] _Eff2("On_Eff2", Float) = 0
			_Color3("EffectColor_3", Color) = (0.0, 0.0, 0.0, 1.0)
			[MaterialToggle] _Eff3("On_Eff3", Float) = 0
			_Color4("EffectColor_4", Color) = (0.0, 0.0, 0.0, 1.0)
			[MaterialToggle] _Eff4("On_Eff4", Float) = 0

		/*	[MaterialToggle] _Strike("On_Strike", Float) = 0
			_StrikeVal("StrikeValue", Float) = 0*/
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		Lighting Off
		
		CGPROGRAM
		#pragma surface surf NoLighting noforwardadd //vertex:vert 

		#pragma target 3.0
		#include "UnityCG.cginc" 

		sampler2D _MainTex;
		sampler2D _EffectTex;

		struct Input {
			float2 uv_MainTex;
			float2 uv2_EffectTex;
		};

		fixed4 _DefaultShadowColor;

		fixed4 _Color0;
		fixed4 _Color1;
		fixed4 _Color2;
		fixed4 _Color3;
		fixed4 _Color4;

		float _Eff0;
		float _Eff1;
		float _Eff2;
		float _Eff3;
		float _Eff4;
		/*float _Strike;
		float _StrikeVal;*/

		fixed4 LightingNoLighting(SurfaceOutput s, fixed3 lightDir, fixed atten)
		{
			fixed4 c;
			c.rgb = s.Albedo * 0.5;
			c.a = s.Alpha;
			return c;
		}

		/*void vert(inout appdata_full v)
		{
			if (_Strike == 1)
			{
				if ((v.vertex.x > 0.0 && v.vertex.x < 0.5) && (v.normal.x > 0.05 && v.normal.x < 0.15))
				{
					v.vertex.x += _StrikeVal;
				}
			}
		}*/

		void surf(Input IN, inout SurfaceOutput o) 
		{
			fixed4 col = tex2D(_MainTex, IN.uv_MainTex);
			fixed4 eff = tex2D(_EffectTex, IN.uv2_EffectTex);

			if (eff.a == 0)
			{
				if (_Eff0 == 1)
				{
					col = _Color0;
				}
				//else
				//{
				//	col *= _DefaultShadowColor;
				//}
			}
			else if ((eff.a >= 0.15 && eff.a <= 0.35))
			{
				if (_Eff1 == 1)
				{
					col = _Color1;
				}
				else
				{
					col *= _DefaultShadowColor;
				}
			}
			else if ((eff.a >= 0.4 && eff.a <= 0.6))
			{
				if (_Eff2 == 1)
				{
					col = _Color2;
				}
				else
				{
					col *= _DefaultShadowColor;
				}
			}
			else if ((eff.a >= 0.65 && eff.a <= 0.85))
			{
				if (_Eff3 == 1)
				{
					col = _Color3;
				}
				else
				{
					col *= _DefaultShadowColor;
				}
			}
			else if (eff.a == 1)
			{
				if (_Eff4 == 1)
				{
					col = _Color4;
				}
				//else
				//{
				//	col *= _DefaultShadowColor;
				//}
			}

			o.Albedo = col.rgb;
			o.Alpha = col.a;
		}

		
		ENDCG
	}
	FallBack "Unlit"
}
