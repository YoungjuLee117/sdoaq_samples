using System;
using System.Runtime.InteropServices;
using System.Text;

namespace SDOWSIO
{
    partial class WSIO
    {
        //====================================================================================================================================
        // WSIO Graphic Library Definition & API
        //------------------------------------------------------------------------------------------------------------------------------------
        //	이 파일(WSIO_GL.h)에는 서비스로 제공되는 3D 그래픽 라이브러리 기능을 추가한다.
        //
        //------------------------------------------------------------------------------------------------------------------------------------
        public static partial class GL
        {
            //====================================================================================================================================
            // WSIO Graphic Library API
            //------------------------------------------------------------------------------------------------------------------------------------
            public static string WSGL_GetVersion()
            {
                try
                {
                    int major = WSGL_GetMajorVersion();
                    int minor = WSGL_GetMinorVersion();
                    int pathch = WSGL_GetPatchVersion();

                    return $"{major}.{minor}.{pathch}";
                }
                catch
                {
                    return "None";
                }
            }

            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern int WSGL_GetMajorVersion();

            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern int WSGL_GetMinorVersion();

            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern int WSGL_GetPatchVersion();

            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSGL_Initialize(IntPtr parent_hwnd, out IntPtr hd3v);

            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSGL_Finalize(IntPtr hd3v);

            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool WSGL_IsOnRun(IntPtr hd3v);

            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSGL_ShowWindow(IntPtr hd3v, bool show, int left, int top, int right, int bottom);

            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSGL_ResetDisplay(IntPtr hd3v);

            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSGL_Display_BG(IntPtr hd3v);

            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public static extern WSIORV WSGL_Display_25D(
                        IntPtr hd3v,
                        string sz_group,
                        string sz_name,
                        [In] float[] vertex,
                        uint nums_vertex_data,
                        [In] byte[] image,
                        uint nums_image_data,
                        int mode,
                        float unit_microns,
                        [In] ref tPara_Display25D para
                        );

            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public static extern WSIORV WSGL_Display_3D(
                        IntPtr hd3v,
                        string sz_group,
                        string sz_name,
                        [In] float[] vertex,
                        uint nums_vertex_data,
                        [In] byte[] image,
                        uint nums_image_data,
                        [In] uint[] face,
                        uint nums_face,
                        int mode,
                        float unit_microns
                        );

            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public static extern WSIORV WSGL_Display_3D(IntPtr hd3v, string sz_group, string sz_name, int mode, float unit_microns);

            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public static extern WSIORV WSGL_Save_3D(IntPtr hd3v, string sz_group, float unit_microns);

            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public static extern WSIORV WSGL_ClearObject(IntPtr hd3v, string sz_group);

            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSGL_DoRendering(IntPtr hd3v);

            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSGL_ClearScreen(IntPtr hd3v);

            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSGL_SetActivation(IntPtr hd3v, int ekey, int activation);

            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSGL_GetActivation(IntPtr hd3v, int ekey,
                out int activation);

            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSGL_SetDisplayAttributes(IntPtr hd3v, int attributes);

            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSGL_GetDisplayAttributes(IntPtr hd3v,
                out int attributes);

            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSGL_SetMode(
                IntPtr hd3v,
                int mode_group,
                int mode,
                [In] int[] modeList,
                int mode_nums
                );

            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSGL_RegisterModeCallback(IntPtr hd3v, int mode_group, IntPtr hwnd, uint msg);

            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSGL_RotationType(IntPtr hd3v,
                out int rotation_type);

            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSGL_SetFunctionMode(
                IntPtr hd3v,
                int function_mode,
                [In] int[] modeList,
                int mode_nums
                );

            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSGL_RegisterFunctionModeCallback(IntPtr hd3v, IntPtr hwnd, uint msg);

            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSGL_SetProjectionMode(IntPtr hd3v, int rojection_mode);

            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSGL_RegisterProjectionModeCallback(IntPtr hd3v, IntPtr hwnd, uint msg);

            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSGL_SetSurfaceMode(IntPtr hd3v, int surface_mode);

            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSGL_RegisterSurfaceModeCallback(IntPtr hd3v, IntPtr hwnd, uint msg);

            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSGL_SetImageColorRatio(IntPtr hd3v, float image_ratio);

            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSGL_GetImageColorRatio(IntPtr hd3v,
                out float image_ratio);

            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSGL_SetZscaleRatio(IntPtr hd3v, float zscale_ratio);

            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSGL_GetZscaleRatio(IntPtr hd3v,
                out float zscale_ratio);

            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public static extern WSIORV WSGL_GetMeasureData(
                IntPtr hd3v,
                [Out] StringBuilder buffer,
                int buf_size,
                string sz_delimiter,
                out int written_size);

            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSGL_SetMeasureDataUnit(IntPtr hd3v, bool flagRendering);

            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSGL_ClearAllMeasureData(IntPtr hd3v, bool flagRendering);

            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSGL_ClearLastMeasureData(IntPtr hd3v, bool flagRendering);

            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSGL_SetProfileValue(IntPtr hd3v, int type, double value, bool o_rendering);

            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSGL_GetProfileValue(IntPtr hd3v, int type,
                out double value);

            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public static extern WSIORV WSGL_RegisterProfileValueUpdateCallback(IntPtr hd3v, IntPtr hwnd, uint msg);

            //----------------------------------------------------------------------------
            // 'WSGL_SetGradient' api currently has no functionality.
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSGL_SetProfileValue(IntPtr hd3v, double dx, double dy, double dz);

            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSGL_SetColor(IntPtr hd3v, int resource_id, uint rgb_color);

            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public static extern WSIORV WSGL_SetInfoString(IntPtr hd3v, string sz_info);

            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public static extern string WSGL_GetLastErrorString(IntPtr hd3v);

            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public static extern WSIORV WSGL_SetUserString(int ekey, string sz_str);

            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public static extern WSIORV WSGL_GetUserString(int ekey,
                [Out] StringBuilder buf,
                int size);

            //----------------------------------------------------------------------------
            // USEFUL FUNCTION
            //----------------------------------------------------------------------------
            // Create triangle indices for all (or jumped by mag) points in the x, y matrix.
            // 'indices_nums' must be 'width' x 'height' x NUMOF_TRIANGLE_INDICES_OF_PLANE_XY_PER_PIXEL or bigger.

            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSGL_GenerateTriangleIndicesOfPlaneXY(
                out uint written_nums,
                [Out] uint[] indices,
                uint indices_nums,
                uint width,
                uint height,
                int mag,
                [Out] float[] vertex
                );
            //----------------------------------------------------------------------------
            // 'buf_nums' must be 'width' x 'height' x NUMOF_TEXCOORD_BUF_NUMBER_PER_PIXEL or bigger.
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSGL_GenerateFixedXY25D_Texcoord(
                uint width,
                uint height,
                int dirs,
                [Out] float[] buf,
                uint buf_nums
                );
            //====================================================================================================================================
        }
    }
}
