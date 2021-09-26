Shader "Shader games/SandSimulation"
{
    Properties
    {
    }

    SubShader
    {
       Lighting Off
       Blend One Zero

       Pass
       {
           CGPROGRAM
           #include "UnityCustomRenderTexture.cginc"
           #pragma vertex CustomRenderTextureVertexShader
           #pragma fragment frag
           #pragma target 3.0

           float4 _drawPos;
           float4 _drawColor;
           float _drawRadius;

           float4 get(v2f_customrendertexture IN, int x, int y) : COLOR
           {
                fixed2 uv = IN.localTexcoord.xy + fixed2(x / _CustomRenderTextureWidth, -y / _CustomRenderTextureHeight);
                
                if (uv.y < 0) return float4(1, 1, 1, 1);
                else if (uv.y > 1) return float4(0, 0, 0, 0);

                return tex2D(_SelfTexture2D, uv);
           }

           bool notEmpty(v2f_customrendertexture IN, int x, int y) : bool
           {
               if (get(IN, x, y).a >= 0.9) return true;
               return false;
           }

           float4 frag(v2f_customrendertexture IN) : COLOR
           {
               float4 color = get(IN, 0, 0);

               if (distance(_drawPos, IN.localTexcoord.xy) < _drawRadius / 1000)
               {
                   color = _drawColor;
               }
               else if (color.a <= 0.1)
               {
                   if (notEmpty(IN, 0, -1))  color = get(IN, 0, -1);
                   else if (notEmpty(IN, 1, -1) && notEmpty(IN, 1, 0) && notEmpty(IN, 2, 0) &&
                       !(notEmpty(IN, -1, -1) && notEmpty(IN, -1, 0) && notEmpty(IN, -2, 0)))    color = get(IN, 1, -1);
                   else if (notEmpty(IN, -1, -1) && notEmpty(IN, -1, 0) && notEmpty(IN, -2, 0))  color = get(IN, -1, -1);
               }
               else if (
                   !notEmpty(IN, 0, 1) ||
                       (!notEmpty(IN, -1, 1) && notEmpty(IN, 1, 1) && !notEmpty(IN, -1, 0) && !(notEmpty(IN, -2, 0) && notEmpty(IN, -3, 1) && notEmpty(IN, -2, 1))) ||
                       (!notEmpty(IN, 1, 1) && notEmpty(IN, -1, 1) && !notEmpty(IN, 1, 0)))
               {
                   color = float4(0, 0, 0, 0);
               }

               return color;
           }
           ENDCG
       }
    }
}
