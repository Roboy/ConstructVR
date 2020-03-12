Shader "Outlined/Standard"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
	_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0

		[Header(Emission)]
	_EmissionColor("Color", Color) = (0,0,0,0)
		_EmissionIntensity("Intensity", Range(0.0, 1.0)) = 0


		[Header(Outline)]
	_FirstOutlineColor("Color", Color) = (1,0,0,0.5)
		_FirstOutlineWidth("Width", Range(0.0, 2.0)) = 0.15

		_Angle("Switch shader on angle", Range(0.0, 180.0)) = 89

	}

		CGINCLUDE
#include "UnityCG.cginc"

		struct appdata {
		float4 vertex : POSITION;
		float4 normal : NORMAL;
	};

	struct Input {
		float2 uv_MainTex;
	};

	sampler2D _MainTex;
	half _Glossiness;
	half _Metallic;
	fixed4 _Color;

	uniform float4 _EmissionColor;
	uniform float _EmissionIntensity;

	uniform float4 _FirstOutlineColor;
	uniform float _FirstOutlineWidth;

	uniform float4 _SecondOutlineColor;
	uniform float _SecondOutlineWidth;

	uniform float _Angle;
	ENDCG

		SubShader
	{
		Tags{ "RenderType" = "Transparent" "Queue" = "Transparent" }
		LOD 200
		Pass{
		ColorMask 0
	}

		Pass{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite Off
		Cull Back
		CGPROGRAM

		struct v2f {
		float4 pos : SV_POSITION;
	};

#pragma vertex vert
#pragma fragment frag

	v2f vert(appdata v) {
		appdata original = v;

		float3 scaleDir = normalize(v.vertex.xyz - float4(0,0,0,1));
		//This shader consists of 2 ways of generating outline that are dynamically switched based on demiliter angle
		//If vertex normal is pointed away from object origin then custom outline generation is used (based on scaling along the origin-vertex vector)
		//Otherwise the old-school normal vector scaling is used
		//This way prevents weird artifacts from being created when using either of the methods
		if (degrees(acos(dot(scaleDir.xyz, v.normal.xyz))) > _Angle) {
			v.vertex.xyz += normalize(v.normal.xyz) * _FirstOutlineWidth;
		}
		else {
			v.vertex.xyz += scaleDir * _FirstOutlineWidth;
		}

		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);
		return o;
	}

	half4 frag(v2f i) : COLOR{
		return _FirstOutlineColor;
	}

		ENDCG
	}

		// Render normally
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask RGB


		CGPROGRAM

#pragma surface surf Standard fullforwardshadows alpha:fade
#pragma target 3.0

		void surf(Input IN, inout SurfaceOutputStandard o)
	{
		fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
		o.Albedo = c.rgb;
		o.Metallic = _Metallic;
		o.Smoothness = _Glossiness;
		o.Alpha = c.a;
		o.Emission = _EmissionColor.rgb * _EmissionIntensity;
	}
	ENDCG


	}



		FallBack "Standard"
}
