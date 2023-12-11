using System.Collections.Generic;
using UnityEngine;
namespace CCKProcessTracer.Editor
{
    public class ButtonDrawer : MonoBehaviour
    {
        public static List<Button> buttons = new List<Button>();

        public static void Clear()
        {
            //ボタンを押した時にボタンの数を減らすとボタン描画用のforeach中にリストの中身が変わるせいでエラーになるのを避けるためにnewしている
            buttons = new List<Button>();
        }

        public static void Draw()
        {
            foreach (var button in buttons)
            {
                var scaledRect = new Rect();
                var pos = View.ProcessViewPosition(new Vector2(button.rect.x, button.rect.y));
                scaledRect.x = pos.x;
                scaledRect.y = pos.y;
                scaledRect.width = button.rect.width * View.scale;
                scaledRect.height = button.rect.height * View.scale;

                if (GUI.Button(scaledRect, button.text))
                {
                    button.OnPress();
                }
            }
        }
    }
}
