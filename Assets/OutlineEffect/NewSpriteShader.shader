Shader "Custom/NewSpriteShader"
{
    Properties
    {
        _MainTex("Main Texture", 2D) = "white" {}
        _OutlineColor("Outline Color", Color) = (0, 0, 1, 1)
        _OutlineWidth("Outline Width", Range(0, 1)) = 0
        _SpriteColor("Sprite Color", Color) = (1, 1, 1, 1)
    }

        SubShader
        {
            Cull Off
            Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
            LOD 100

            ZWrite Off
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

                float4 _OutlineColor;
                float _OutlineWidth;
                float4 _SpriteColor;

                sampler2D _MainTex;

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    fixed4 col = tex2D(_MainTex, i.uv);
                    fixed4 outlineCol = _OutlineColor;

                    // Calculate the outline based on the distance from the edge
                    fixed2 d = ddx(i.uv) + ddy(i.uv);
                    float outline = fwidth(col.a) * _OutlineWidth;
                    float alpha = saturate((outline - length(d)) / outline);

                    // Apply the sprite color to the color value
                    col *= _SpriteColor;

                    // Return the color with the outline effect
                    return lerp(col, outlineCol, alpha);
                }
                ENDCG
            }
        }
}