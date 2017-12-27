using System;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using OpenTK;

namespace RuneGear.Utilities
{
    public static class GeneralUtility
    {
        public static float Epsilon = 1e-5f;
        public struct IconInfo
        {
            public bool fIcon;
            public int xHotspot;
            public int yHotspot;
            public IntPtr hbmMask;
            public IntPtr hbmColor;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr CreateIconIndirect(ref IconInfo icon);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetIconInfo(IntPtr hIcon, ref IconInfo pIconInfo);

        private static int MakeLong(short lowPart, short highPart)
        {
            return (int)(((ushort)lowPart) | (uint)(highPart << 16));
        }

        public static void DoubleBuffered(this Control control, bool enable)
        {
            var doubleBufferPropertyInfo = control.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            doubleBufferPropertyInfo.SetValue(control, enable, null);
        }

        public static Cursor CreateCursor(Bitmap bmp, int xHotSpot, int yHotSpot)
        {
            IconInfo tmp = new IconInfo();
            GetIconInfo(bmp.GetHicon(), ref tmp);
            tmp.xHotspot = xHotSpot;
            tmp.yHotspot = yHotSpot;
            tmp.fIcon = false;
            return new Cursor(CreateIconIndirect(ref tmp));
        }

        public static void SetSpacing(ListView listview, short cx, short cy)
        {
            const int LVM_FIRST = 0x1000;
            const int LVM_SETICONSPACING = LVM_FIRST + 53;
            SendMessage(listview.Handle, LVM_SETICONSPACING,
            IntPtr.Zero, (IntPtr)MakeLong(cx, cy));
        }

        /// <summary>
        /// Snaps the given point to the grid, expects coordinates in x,y plane
        /// </summary>
        /// <param name="point"></param>
        public static void SnapToGrid(ref Vector3 point, int snapValue)
        {
            for (int i = 0; i < 3; i++)
            {
                point[i] = SnapNumber(point[i], snapValue);
            }
        }

        public static Vector3 SnapToGrid(Vector3 point, int snapValue)
        {
            SnapToGrid(ref point, snapValue);
            return point;
        }

        // C# always rounds to down to zero -121/4 = -120, 121/4 = 120
        public static float SnapNumber(float value, int snapValue)
        {
            int halfGridSize = snapValue/2;
            float modifier = value < 0 ? -1f : 1f;

            return (int) (value + modifier*halfGridSize)/snapValue*snapValue;
        }

        public static bool IsCloseEnough(float value, float near)
        {
            return Math.Abs((value - near) / ((near == 0.0f) ? 1.0f : near)) < Epsilon;
        }
    }
}
