//====================================================================================================================================
// WSIF (WSIO WiseScope-InterFace) Definition & API
//------------------------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace SDOWSIO
{
    partial class WSIO
    {
        public static class WSIF
        {
            #region Enum
            //----------------------------------------------------------------------------
            // WSIF DATA
            //----------------------------------------------------------------------------
            public enum WSIFDATAGROUP
            {
                WSIFDATAGROUP_NULL = 0,
                WSIFDATAGROUP_TCODE = 0x0001,
                WSIFDATAGROUP_ALL = -1,
            }

            //----------------------------------------------------------------------------
            // WSIF CLASS
            //----------------------------------------------------------------------------
            public enum WSIFCLASS
            {
                WSIFCLASS_NULL = 0,
                WSIFCLASS_GENERAL = 1,      // Default
            };

            //----------------------------------------------------------------------------
            // WSIF NETWORK TYPE
            //----------------------------------------------------------------------------
            public enum WSIFNETTYPE
            {
                WSIFNETTYPE_NULL = 0,
                WSIFNETTYPE_UDP = 1,            // Default, User datagram Protocol
                WSIFNETTYPE_TCP_DUAL = 2,       // Transport Control Protocol, 2 half-way TCP connection
                WSIFNETTYPE_TCP_SERVER = 3,     // DO NOT USE in WSIF API. WSM use this type.
                WSIFNETTYPE_TCP_CLIENT = 4,		// Tranport Control Protocol, 1 both-way TCP connection
            }

            //----------------------------------------------------------------------------
            // WSIF NETWORK STATE
            //----------------------------------------------------------------------------
            public enum WSIFNETSTATE
            {
                WSIFNETSTATE_NONE = 0,              // No network connection - Not opened
                WSIFNETSTATE_WAIT = 1,              // Our side's network is READY, but protocol is not established. - Opened but not initialized.
                WSIFNETSTATE_WAIT_ONEWAY = 3,       // Our side's network is READY, but other side's network is not READY. - Opened but other is not opened.
                WSIFNETSTATE_WAIT_BOTHWAY = 4,      // Our side's network and other side's network are READY, but protocol is not established - Opened but not initialized.
                WSIFNETSTATE_ESTABLISHED = 2,		// All connection is OK. Message send/receive are enabled. - Ready to all kind of send/receive message.
            }

            //----------------------------------------------------------------------------
            // WSIF READY STATE
            //----------------------------------------------------------------------------
            public enum WSIFREADYSTATUS
            {
                WSIFREADYSTATUS_FAIL = 0,           // Not Ready - Initialization failed
                WSIFREADYSTATUS_OK = 1,             // Ready - Initialization Success
                WSIFREADYSTATUS_WAITING = 2,		// Waiting to Ready - Initialization progressed
            }

            //----------------------------------------------------------------------------
            // WSIF RAW INSPECT RESULT
            //----------------------------------------------------------------------------
            public enum WSIFRAWINSPECTRESULT
            {
                WSIFRAWINSPECTRESULT_SUCCESS = 0,
                WSIFRAWINSPECTRESULT_ERROR = 1,
                WSIFRAWINSPECTRESULT_CAMERAERROR = 2,
            }

            //----------------------------------------------------------------------------
            // REQUEST_MOVE_PARALLEL_LIGHTING
            //----------------------------------------------------------------------------
            public enum WSIFMOVELIGHTINGRESULT
            {
                WSIFMOVELIGHTINGRESULT_SUCCESS = 0,
                WSIFMOVELIGHTINGRESULT_ERROR = 1,
            }

            //----------------------------------------------------------------------------
            // WSIF ALGORITHM METHOD
            //----------------------------------------------------------------------------
            public enum WSIFALGOMETHOD
            {
                WSIFALGOMETHOD_NONE = 0,        // No algorithm
                WSIFALGOMETHOD_SIMUL = 1,       // Simulation
                ////
                // REFER TO WSIO_ALGORITHM file.
                ////
                WSIFALGOMETHOD_WSM = 0xFFFF,	// Use WSM default configuration
            }

            //----------------------------------------------------------------------------
            // WSIF CAMERA OPERATION MODE
            //----------------------------------------------------------------------------
            public enum WSIFCAMOPMODE
            {
                WSIFCAMOPMODE_READY = 0,    // normal ready mode for inspection
                WSIFCAMOPMODE_LIVE = 1,     // live
                WSIFCAMOPMODE_EXTSNAP = 2,  // external triggering
            };

            //----------------------------------------------------------------------------
            // WSIF USER STRING
            //----------------------------------------------------------------------------
            public const int WSIFNUMOF_USERSTRING = 16;

            public enum WSIFUSERSTRING
            {
                WSIFUSERSTRING_WSMNAME = 0,
                WSIFUSERSTRING_START = 1,
                WSIFUSERSTRING_END = WSIFNUMOF_USERSTRING - 1,
            };

            //----------------------------------------------------------------------------
            // WSIO LANGUAGE CODE
            //----------------------------------------------------------------------------
            public const int WSIOLANGCODE_CHINESE = (int)WSIOLANGCODE.WSIOLANGCODE_CHINESE_SIMPLIFIED;

            public enum WSIOLANGCODE
            {
                WSIOLANGCODE_BASIC = 0,
                WSIOLANGCODE_KOREAN = 1,
                WSIOLANGCODE_ENGLISH = 2,
                WSIOLANGCODE_CHINESE_SIMPLIFIED = 3,
            };

            //----------------------------------------------------------------------------
            // WSIF SEIZE REQUEST
            //----------------------------------------------------------------------------
            public enum WSIF_SEIZINGMODE
            {
                WSIF_SEIZINGMODE_RELEASE = 0,
                WSIF_SEIZINGMODE_SEIZE = 1,
                WSIF_SEIZINGMODE_SEIZEBYFORCE = 2,
            };

            //----------------------------------------------------------------------------
            // WSIF SEIZED-BY STATUS
            //----------------------------------------------------------------------------
            public enum WSIF_SEIZEDBYSTATUS
            {
                WSIF_SEIZEDBYSTATUS_FREE = 0,
                WSIF_SEIZEDBYSTATUS_BYOTHERS = 1,
                WSIF_SEIZEDBYSTATUS_BYYOU = 2,
            };

            //----------------------------------------------------------------------------
            // WSIF MALS DEPTH TABLE TYPE
            //----------------------------------------------------------------------------
            public enum WSIF_MALSDEPTHTBL
            {
                WSIF_MALSDEPTHTBL_UNKNOWN = 0,
                WSIF_MALSDEPTHTBL_NONE = 1,
                WSIF_MALSDEPTHTBL_LINEAR = 2,
                WSIF_MALSDEPTHTBL_TABLE = 3,
                WSIF_MALS_FULL_CAL = 4,
            };

            //----------------------------------------------------------------------------
            // WSIF ALERT CODE
            //----------------------------------------------------------------------------
            public enum WSIFALERTCODE
            {
                WSIFALERTCODE_DEVICE_CLOSED = 1,
                WSIFALERTCODE_DEVICE_NO_ANSWER = 2,
                WSIFALERTCODE_DEVICE_RETURN_ERROR = 3,
                WSIFALERTCODE_DEVICE_INIT_ERROR = 4,
            }

            //----------------------------------------------------------------------------
            // WSIF ALARM LEVEL
            //----------------------------------------------------------------------------
            public enum WSIFALERTLEVEL
            {
                WSIFALERTLEVEL_LOG = 1,
                WSIFALERTLEVEL_WARNING = 2,
                WSIFALERTLEVEL_ERROR = 3,
            };

            //----------------------------------------------------------------------------
            // WSIF ALERT 
            //----------------------------------------------------------------------------
            public enum WSIFALERTDEVICETYPE
            {
                WSIFALERTDEVICETYPE_CAMERA = 0x410,
                WSIFALERTDEVICETYPE_LIGHTING = 0x420,
                WSIFALERTDEVICETYPE_MALS = 0x430,
            };
            #endregion

            #region Constant
            //============================================================================
            // CONSTANT DEFINITION
            //----------------------------------------------------------------------------
            // LENGTH OF STRING
            //----------------------------------------------------------------------------
            public const int WSIFSIZEOF_TID = 255; // This definition is no longer used. Reserved for backward compatibility.
            public const int WSIFSIZEOF_MAXPATH = WSIOSIZEOF_MAXPATH;
            //----------------------------------------------------------------------------
            // SIZE OF SOMETHING
            //----------------------------------------------------------------------------
            public const int WSIFNUMOF_USERRECORD = 256;
            //----------------------------------------------------------------------------
            // DEFAULT MOVEOK DELAY TIME (MILLI SECOND)
            //----------------------------------------------------------------------------
            public const int WSIFDEFMOVEOKDELAYTIME = 50 * 1000;

            //============================================================================
            // C-LANGAUAGE STYLE TYPE DEFINITION
            //----------------------------------------------------------------------------
            public static readonly IntPtr WSIFALLHANDLE = new IntPtr(-1);
            public const int WSIFALLALGORITHM = 0;
            #endregion

            //============================================================================
            // C-LANGAUAGE STYLE FUNCTION DEFINITION
            //----------------------------------------------------------------------------
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate void WSIFFUNC_HANDLE(IntPtr wsif_handle);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public delegate void WSIFFUNC_REPLYPARAMS(IntPtr wsif_handle, string wsif_params_str);

            //============================================================================
            // COMMON DATA MANAGEMENT
            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_ResetCommonData(WSIFDATAGROUP data_groups);

            //============================================================================
            // CREATE / DESTROY
            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_CreateWsi(out IntPtr wsif_handle,
                WSIFCLASS wsif_class, uint user_id);
            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_DestroyWsi(IntPtr wsif_handle);

            //====================================================================================================================================
            // WSIO WiseScope-InterFace API
            //------------------------------------------------------------------------------------------------------------------------------------

            //============================================================================
            // WSIO PARAMETER : SET BEFORE OPENNING.
            //----------------------------------------------------------------------------

            //============================================================================
            // WSIO PARAMETER : READ ANYTIMES.
            //----------------------------------------------------------------------------
            // USER ID : Unique identifier
            //----------------------------------------------------------------------------
            //			WSIO_SetUserID function is not supported. Set user_id function is in WSIF_CreateWsi().
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_GetUserId(IntPtr wsif_handle,
                out int user_id);

            //============================================================================
            // OPEN / CLOSE WSI
            //----------------------------------------------------------------------------
            // WSIF_OpenWsi_IP : 수행 이후에 다른 WSIO 호출 없이 핸들러가 종료되어야 한다.
            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public static extern WSIORV WSIF_OpenWsi_IP(IntPtr wsif_handle, WSIFNETTYPE if_mode, string my_ip_str, int my_port, string other_ip_str, int other_port, string net_drive_str);
            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_CloseWsi(IntPtr wsif_handle);

            //============================================================================
            // WSIO PARAMETER : READ AFTER OPENNING.
            //----------------------------------------------------------------------------
            // OPEN PATH : The path of Open folder
            // OPEN IMAGE PATH : The path of Open image folder
            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public static extern WSIORV WSIF_GetOpenPath(IntPtr wsif_handle,
                [Out] StringBuilder open_path_buffer,
                uint size_of_open_path_buffer);
            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public static extern WSIORV WSIF_GetOpenImagePath(IntPtr wsif_handle,
                [Out] StringBuilder open_image_path_buffer,
                uint size_of_open_image_path_buffer);

            //============================================================================
            // STATE
            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_GetNetState(IntPtr wsif_handle,
                out WSIFNETSTATE net_state);

            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_GetNetState_V2(IntPtr wsif_handle,
                out WSIFNETSTATE net_state);

            //============================================================================
            // REQUEST/REPLY MESSAGE
            //----------------------------------------------------------------------------
            // REQUEST_INITIALIZE
            // REPLY_READY
            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_Request_Initialize(IntPtr wsif_handle);
            //----------------------------------------------------------------------------
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate void WSIFFUNC_READY(IntPtr wsif_handle, WSIFREADYSTATUS ready_status);

            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_RegiCbf_Ready(WSIFFUNC_READY cbf);

            //----------------------------------------------------------------------------
            // REQUEST_SEIZE
            // REPLY_SEIZE
            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_Seize(int timeout, IntPtr wsif_handle, WSIF_SEIZINGMODE seizing_mode,
                out WSIF_SEIZINGMODE result_seizing_mode);  // Blocked function

            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_Request_Seize(IntPtr wsif_handle, WSIF_SEIZINGMODE seizing_mode);
            //----------------------------------------------------------------------------
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate void WSIFFUNC_SEIZE(IntPtr wsif_handle, WSIF_SEIZINGMODE result_seizing_mode);

            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_RegiCbf_Seize(WSIFFUNC_SEIZE cbf);


            //----------------------------------------------------------------------------            
            // REQUEST_INSPECT
            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public static extern WSIORV WSIF_Request_UInspect(IntPtr wsif_handle, string wsif_params_str);

            //LAB Version Inspect
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public static extern WSIORV WSIF_Request_VTProfile(IntPtr wsif_handle, string wsif_profile_str);

            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_Request_VTBestFocusStep(IntPtr wsif_handle, int best_focus_step);

            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public static extern WSIORV WSIF_Request_VTCommand(IntPtr wsif_handle, string wsif_tid_str, string wsif_profile_str, string wsif_command_str);

            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public static extern WSIORV WSIF_Request_VTTrigger(IntPtr wsif_handle, string wsif_trigger_str);

            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public static extern WSIORV WSIF_Request_RawVTCommand(IntPtr wsif_handle, string wsif_tid_str, string wsif_profile_str, string wsif_command_str);

            //General Version Inspect
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public static extern WSIORV WSIF_Request_RawInspect(IntPtr wsif_handle, string wsif_tid_str);

            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public static extern WSIORV WSIF_Request_RawCaptureWait(IntPtr wsif_handle, string wsif_tid_str);

            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public static extern WSIORV WSIF_Request_ReadyRawInspect(IntPtr wsif_handle, string wsif_tid_str);

            //----------------------------------------------------------------------------            
            // Replay Inspect
            //----------------------------------------------------------------------------
            [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public delegate void WSIFFUNC_MOVEOK(IntPtr wsif_handle, string wsio_tid_str);

            //Capture Done
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_RegiCbf_Moveok(WSIFFUNC_MOVEOK cbf);

            //Algo Done
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_RegiCbf_UInspect(WSIFFUNC_REPLYPARAMS cbf);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public delegate void WSIFFUNC_REPLYUINSPECT_BESTFOCUS(IntPtr wsif_handle, string wsif_tid_str, string bestfocus_str, double bestfocus_double);
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_RegiCbf_UInspect_BESTFOCUS(WSIFFUNC_REPLYUINSPECT_BESTFOCUS cbf);

            //Raw Inspect
            [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public delegate void WSIFFUNC_RAWREPLYINSP(IntPtr wsif_handle, string wsif_tid_str, WSIFRAWINSPECTRESULT raw_result_type, int num_of_array_result, IntPtr array_result_filename, IntPtr array_result_contents);

            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_RegiCbf_RawInspect(WSIFFUNC_RAWREPLYINSP cbf);

            //Algo Index 별로 결과 처리가 필요 한 경우
            [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public delegate void WSIFFUNC_RAWREPLYINSP_PARTIAL(IntPtr wsif_handle, string wsif_tid_str, uint algo_id, WSIFRAWINSPECTRESULT raw_result_type, int num_of_array_result, IntPtr array_result_filename, IntPtr array_result_contents);

            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_RegiCbf_RawInspect_Partial(WSIFFUNC_RAWREPLYINSP_PARTIAL cbf);

            public static (string fileName, string contents)[] Get_InspectResultFileList(int numofResult, IntPtr resultFilenames, IntPtr resultContents)
            {
                var items = new List<(string fileName, string contents)>();

                if (resultFilenames == IntPtr.Zero || resultContents == IntPtr.Zero)
                {
                    return items.ToArray();
                }

                var ptrFileNams = new IntPtr[numofResult];
                var ptrContents = new IntPtr[numofResult];

                Marshal.Copy(resultFilenames, ptrFileNams, 0, numofResult);
                Marshal.Copy(resultContents, ptrContents, 0, numofResult);
                for (int i = 0; i < numofResult; i++)
                {
                    items.Add((Marshal.PtrToStringAnsi(ptrFileNams[i]), Marshal.PtrToStringAnsi(ptrContents[i])));
                }
                return items.ToArray();
            }
            //----------------------------------------------------------------------------            
            // TCode
            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public static extern WSIORV WSIF_Set_TCODE(string tid, string tcode_str);

            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public static extern WSIORV WSIF_Get_TCODE(string tid,
                [Out] StringBuilder tcode_str, int size_of_tcode_buffer);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public delegate void WSIFFUNC_NOTIFY_TCODE(IntPtr wsif_handle, string tcode_str);
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_RegiCbf_TCODE(WSIFFUNC_NOTIFY_TCODE cbf);

            //----------------------------------------------------------------------------            
            // Cavity
            //----------------------------------------------------------------------------
            public const int WSIF_DEFECTCODE_CAVITY = -2;
            public const int WSIF_DEFECTCODE_NOCHANGE = -1;
            public const int WSIF_DEFECTCODE_PASS = 0;

            [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public delegate void WSIFFUNC_NOTIFY_DETECTEDCAVITY(string wsif_tid_str, string detected_cavity_str);
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_RegiCbf_DetectedCavity(WSIFFUNC_NOTIFY_DETECTEDCAVITY cbf);

            //----------------------------------------------------------------------------            
            // Set Inspect Result
            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public static extern WSIORV WSIF_Set_DefectCode(IntPtr wsif_handle, string wsif_tid_str, int defect_code);

            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public static extern WSIORV WSIF_Set_DefectResultName(IntPtr wsif_handle, string wsif_tid_str, string defect_filenames_str);

            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public static extern WSIORV WSIF_Finalize_TID(string wsif_tid_str);


            //----------------------------------------------------------------------------
            // REQUEST_STOP
            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_Request_Stop(IntPtr sif_handle);

            //----------------------------------------------------------------------------
            // REQUEST_DOWN
            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_Request_Down(IntPtr sif_handle);

            //----------------------------------------------------------------------------
            // REQUEST_ALL_INSPECT_DONE
            // REPLY_ALL_INSPECT_DONE
            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_Request_IsAllInspectDone(IntPtr wsif_handle);
            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_RegiCbf_IsAllInspectDone(WSIFFUNC_HANDLE cbf);

            //----------------------------------------------------------------------------
            // REQUEST_LIVE_MODE
            // ITEM_CAMERA_MODE
            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_Request_CameraMode(IntPtr wsif_handle, WSIFCAMOPMODE cam_op_mode);    // NOT_ZERO:live, ZERO:not live

            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_Request_LiveMode(IntPtr wsif_handle, int live_mode);    // NOT_ZERO:live, ZERO:not live

            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_SetItem_ExtSnapMode(IntPtr wsif_handle, int on_off);    // NOT_ZERO:ext-snap-mode, ZERO:not ext-snap-mode

            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_GetItem_ExtSnapMode(IntPtr wsif_handle, out int on_off);    // NOT_ZERO:ext-snap-mode, ZERO:not ext-snap-mode

            //----------------------------------------------------------------------------
            // WSIMSG_REQUEST_EXTSNAP_TEST
            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_Request_ExtSnapModeTest(IntPtr wsif_handle);

            //----------------------------------------------------------------------------
            // REQUEST_MAKE_PLATE_INFO
            // Image Save Folder 에 특정 정보를 남기고 싶은 경우 사용 (TID 상위 폴더에 정보 생성)
            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public static extern WSIORV WSIF_Request_MakePlateInfo(IntPtr wsif_handle, string plate_info_str); // max length is WSIFSIZEOF_MAXPATH

            //----------------------------------------------------------------------------
            // REQUEST_ADD_CELL_INFO
            // Image Save Folder 에 특정 정보를 남기고 싶은 경우 사용 (TID 폴더 CellInfo에 정보 생성)
            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public static extern WSIORV WSIF_WSIF_Request_AddCellInfo(IntPtr wsif_handle, string add_cell_info_str);

            //----------------------------------------------------------------------------
            // REQUEST_STILL_DISPLAY
            // 이미지 리뷰 요청
            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public static extern WSIORV WSIF_Request_StillDisplay(IntPtr wsif_handle, uint reserved, string wsif_tid_str, string image_path);

            //----------------------------------------------------------------------------
            // REQUEST_SAVING_TID
            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public static extern WSIORV WSIF_Request_SavingTid(IntPtr wsif_handle, string image_path, string wsif_tid_str, string new_text_str);

            //----------------------------------------------------------------------------
            // REQUEST_MOVE_PARALLEL_LIGHTING
            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public static extern WSIORV WSIF_Request_MoveParallelLighting(IntPtr wsif_handle, string parallel_lighting_pos_str); // Motion 별 포지션 값은 ',' 구분자를 통해 전달 ex) 200,1000,30,100

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate void WSIFFUNC_MOVEPARALLELLIGHTING(IntPtr wsif_handle, int result_type);

            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_RegiCbf_MoveParallelLighting(WSIFFUNC_MOVEPARALLELLIGHTING cbf); //0 -> Success, 1 -> Error

            //============================================================================
            // NOTIFY MESSAGE
            //----------------------------------------------------------------------------
            // NOTIFY_RTC_CHANGED
            //----------------------------------------------------------------------------
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate void WSIFFUNC_RTC(
                                       IntPtr wsif_handle,
                                       int year,               // 2016
                                       int month,              // 1~12
                                       int day,                // 1~31
                                       int hour,               // 0~23
                                       int minute,             // 0~59
                                       int second,             // 0~59
                                       int milliseconds        // 0~999
                                       );

            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_RegiCbf_NotifyRtcChanged(WSIFFUNC_RTC cbf);

            //============================================================================
            // CONTROL MESSAGE
            //----------------------------------------------------------------------------
            // CONTROL OPEN / CLOSE
            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_RegiCbf_ControlOpen(WSIFFUNC_HANDLE cbf);

            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_RegiCbf_ControlClose(WSIFFUNC_HANDLE cbf);

            //============================================================================
            // REQUEST CONSTANT ITEM GET MESSAGE
            //----------------------------------------------------------------------------
            // ITEM_SW_VERSION
            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public static extern WSIORV WSIF_GetItem_SwVersion(IntPtr wisf_handle,
                [Out] StringBuilder sw_version_buffer, int size_of_sw_version_buffer);

            //----------------------------------------------------------------------------
            // ITEM_SHARED_NAME
            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public static extern WSIORV WSIF_GetItem_SharedName(IntPtr wsif_handle,
                [Out] StringBuilder shared_name_buffer, int size_of_shared_name_buffer);

            //----------------------------------------------------------------------------
            // ITEM_STEP_TO_UM
            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_GetItem_MalsStepMetric(IntPtr wsif_handle,
                out double mals_step_metric);

            //----------------------------------------------------------------------------
            // ITEM_PIXEL_TO_UM
            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_GetItem_ImagePixelMetric(IntPtr wsif_handle,
                out double image_pixel_metric);

            //----------------------------------------------------------------------------
            // ITEM_LIGHTING_NUMS
            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_GetItem_LightingNums(IntPtr wsif_handle,
                out int lighting_nums);

            //----------------------------------------------------------------------------
            // ITEM_ALGORITHM_NUMS
            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_GetItem_AlgorithmNums(IntPtr wsif_handle,
                out int algorithm_nums);

            //----------------------------------------------------------------------------
            // ITEM_MALS_DEPTH_TABLE_TYPE
            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_GetItem_MalsDepthTableType(IntPtr wsif_handle,
                out int mals_depth_table_type);

            //============================================================================
            // REQUEST ITEM SET/GET MESSAGE
            //----------------------------------------------------------------------------
            // ITEM_IMAGEPATH	
            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public static extern WSIORV WSIF_SetItem_CurrentImagePath(IntPtr wsif_handle, string current_image_path);

            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_ReqItem_CurrentImagePath(IntPtr wsif_handle);

            //----------------------------------------------------------------------------
            [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public delegate void WSIFFUNC_CURRENTIMAGEPATH(IntPtr wsif_handle, string current_image_path);

            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_RegiCbf_CurrentImagePath(WSIFFUNC_CURRENTIMAGEPATH cbf);

            //----------------------------------------------------------------------------
            // ITEM_UPLOAD_STORAGE
            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public static extern WSIORV WSIF_SetItem_UploadStorage(IntPtr wsif_handle, string pair_info_str);

            //----------------------------------------------------------------------------
            // ITEM_SAVE_FORMAT	
            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_SetItem_CapturedImageSaveFormat(IntPtr wsif_handle, WSIOIMGFORMAT captured_image_save_format);

            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_ReqItem_CapturedImageSaveFormat(IntPtr wsif_handle);

            //----------------------------------------------------------------------------
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate void WSIFFUNC_CAPTUREDIMAGESAVEFORMAT(IntPtr wsif_handle, WSIOIMGFORMAT captured_image_save_format);

            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_RegiCbf_CapturedImageSaveFormat(WSIFFUNC_CAPTUREDIMAGESAVEFORMAT cbf);

            //----------------------------------------------------------------------------
            // ITEM_CAMERA_ROI / ITEM_CAMERA_ROI_WITH_SHIFT
            //----------------------------------------------------------------------------
            //V1
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_SetItem_CameraROI(IntPtr wsif_handle, int camera_roi_X, int camera_roi_Y);

            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_ReqItem_CameraROI(IntPtr wsif_handle);

            //----------------------------------------------------------------------------
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate void WSIFFUNC_CAMERAROI(IntPtr wsif_handle, int camera_roi_X, int camera_roi_Y);

            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_RegiCbf_CameraROI(WSIFFUNC_CAMERAROI cbf);

            //V2
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_SetItem_CameraROI_V2(IntPtr wsif_handle, int camera_roi_X, int camera_roi_Y, int shift_roi_X, int shift_roi_y);

            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_ReqItem_CameraROI_V2(IntPtr wsif_handle);

            //----------------------------------------------------------------------------
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate void WSIFFUNC_CAMERAROI_V2(IntPtr wsif_handle, int camera_roi_X, int camera_roi_Y, int shift_roi_X, int shift_roi_y);

            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_RegiCbf_CameraROI_V2(WSIFFUNC_CAMERAROI_V2 cbf);

            //V3
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_SetItem_CameraROI_V3(IntPtr wsif_handle, int camera_roi_X, int camera_roi_Y, int shift_roi_X, int shift_roi_y, int binning_value);

            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_ReqItem_CameraROI_V3(IntPtr wsif_handle);

            //----------------------------------------------------------------------------
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate void WSIFFUNC_CAMERAROI_V3(IntPtr wsif_handle, int camera_roi_X, int camera_roi_Y, int shift_roi_X, int shift_roi_y, int binning_value);

            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_RegiCbf_CameraROI_V3(WSIFFUNC_CAMERAROI_V3 cbf);

            //----------------------------------------------------------------------------
            // ITEM_UPDATE_LIGHTING_DB
            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_SetItem_UpdateLightingData(IntPtr wsif_handle, string lighting_data_str, int always_send_flag);

            //----------------------------------------------------------------------------
            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_ReqItem_UpdateLightingData(IntPtr wsif_handle);

            //----------------------------------------------------------------------------
            [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public delegate void WSIFFUNC_UPDATELIGHTINGDATA(IntPtr wsif_handle, string lighting_data_str);

            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_RegiCbf_UpdateLightingData(WSIFFUNC_UPDATELIGHTINGDATA cbf);

            //----------------------------------------------------------------------------
            // ITEM_LIGHTING_INTENSITY
            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public static extern WSIORV WSIF_SetItem_LightingIntensity(IntPtr wsif_handle, string lighting_intensity_str);

            //----------------------------------------------------------------------------
            // ITEM_LIGHTING_PATTERN
            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public static extern WSIORV WSIF_SetItem_LightingPattern(IntPtr wsif_handle, string lighting_pattern_str);

            //----------------------------------------------------------------------------
            // ITEM_LIGHTING_LINKED_MOVER_POS
            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public static extern WSIORV WSIF_SetItem_LightingLinkedMoverPos(IntPtr wsif_handle, string lighting_linked_mover_pos_str, int always_send_flag);

            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_ReqItem_LightingLinkedMoverPos(IntPtr wsif_handle);

            //----------------------------------------------------------------------------
            [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public delegate void WSIFFUNC_LIGHTINGLINKEDMOVERPOS(IntPtr wsif_handle, string lighting_linked_mover_pos_str);

            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_RegiCbf_LightingLinkedMoverPos(WSIFFUNC_LIGHTINGLINKEDMOVERPOS cbf);

            //----------------------------------------------------------------------------
            // ITEM_MALS_STEP_TABLE
            // ITEM_MALS_STEP_LHU
            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_SetItem_MalsStepTable(IntPtr wsif_handle, int num_of_step, int[] step_array);

            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_SetItem_MalsStepLHU(IntPtr wsif_handle, int lowest_step, int highest_step, int step_unit);

            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_ReqItem_MalsSteps(IntPtr wsif_handle);

            //----------------------------------------------------------------------------
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate void WSIFFUNC_MALSSTEPTABLE(IntPtr wsif_handle, int num_of_step, IntPtr step_array);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate void WSIFFUNC_MALSSTEPLHU(IntPtr wsif_handle, int lowest_step, int highest_step, int step_unit);

            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_RegiCbf_MalsSteps(WSIFFUNC_MALSSTEPTABLE cbf_table, WSIFFUNC_MALSSTEPLHU cbf_LHU);

            //----------------------------------------------------------------------------
            // ITEM_MALS_TABLE_MASK_V2
            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public static extern WSIORV WSIF_SetItem_MalsTableMask(IntPtr wsif_handle, string mals_table_mask_str);

            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_ReqItem_MalsTableMask(IntPtr wsif_handle);

            //----------------------------------------------------------------------------
            [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public delegate void WSIFFUNC_MALSTABLEMASK(IntPtr wsif_handle, string mals_table_mask_str);

            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_RegiCbf_MalsTableMask(WSIFFUNC_MALSTABLEMASK cbf);

            //----------------------------------------------------------------------------
            // ITEM_MALS_STEP_ADJUST
            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_SetItem_MalsStepAdjust(IntPtr wsif_handle, int mals_step_adjust);

            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_SetItem_MalsStepAdjust_MM(IntPtr wsif_handle, double mals_step_adjust_millimeter);

            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_ReqItem_MalsStepAdjust(IntPtr wsif_handle);

            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_ReqItem_MalsStepAdjust_MM(IntPtr wsif_handle);

            //----------------------------------------------------------------------------
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate void WSIFFUNC_MALSSTEPADJUST(IntPtr wsif_handle, int mals_step_adjust);

            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_RegiCbf_MalsStepAdjust(WSIFFUNC_MALSSTEPADJUST cbf);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate void WSIFFUNC_MALSSTEPADJUSTMM(IntPtr wsif_handle, double mals_step_adjust_millimeter);

            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_RegiCbf_MalsStepAdjust_MM(WSIFFUNC_MALSSTEPADJUSTMM cbf);

            //----------------------------------------------------------------------------
            // ITEM_ALGORITHM_METHOD	
            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_SetItem_AlgorithmMethod(IntPtr wsif_handle, int algo_method);     // WSIOALLHANDLE or wsio handle

            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_ReqItem_AlgorithmMethod(IntPtr wsif_handle);      // WSIFALLHANDLE or wsif handle 

            //----------------------------------------------------------------------------
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate void WSIFFUNC_ALGORITHMMETHOD(IntPtr wsif_handle, int algo_method);

            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_RegiCbf_AlgorithmMethod(WSIFFUNC_ALGORITHMMETHOD cbf);

            //----------------------------------------------------------------------------
            // MULTI ITEM_ALGORITHM_METHOD	
            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_SetItem_MultiAlgorithmMethod(IntPtr wsif_handle, uint algo_index, int algo_method);    // WSIFALLALGORITHM or index(1 ~ )

            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_ReqItem_MultiAlgorithmMethod(IntPtr wsif_handle, uint algo_index);      // WSIFALLALGORITHM or index(1 ~ ) 

            //----------------------------------------------------------------------------
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate void WSIFFUNC_MULTIALGORITHMMETHOD(IntPtr wsif_handle, uint algo_index, int algo_method);

            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_RegiCbf_MultiAlgorithmMethod(WSIFFUNC_MULTIALGORITHMMETHOD cbf);

            //----------------------------------------------------------------------------
            // ITEM_ALGORITHM_MODULE
            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public static extern WSIORV WSIF_SetItem_AlgorithmModule(IntPtr wsif_handle, string algo_module_str);

            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_ReqItem_AlgorithmModule(IntPtr wsif_handle);

            //----------------------------------------------------------------------------
            [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public delegate void WSIFFUNC_ALGORITHMMODULE(IntPtr wsif_handle, string algo_module_str);

            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_RegiCbf_AlgorithmModule(WSIFFUNC_ALGORITHMMODULE cbf);

            //----------------------------------------------------------------------------
            // Item_MultiAlgorithmModule
            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public static extern WSIORV WSIF_SetItem_MultiAlgorithmModule(IntPtr wsif_handle, uint algo_index, string algo_module_str);    // WSIFALLALGORITHM or index(1 ~ )

            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_ReqItem_MultiAlgorithmModule(IntPtr wsif_handle, uint algo_index);    // WSIFALLALGORITHM or index(1 ~ )

            //----------------------------------------------------------------------------
            [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public delegate void WSIFFUNC_MULTIALGORITHMMODULE(IntPtr wsif_handle, uint algo_index, string algo_module_str);

            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_RegiCbf_MultiAlgorithmModule(WSIFFUNC_MULTIALGORITHMMODULE cbf);

            //----------------------------------------------------------------------------
            // ITEM_ALGORITHM_CODE	
            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public static extern WSIORV WSIF_SetItem_AlgorithmCode(IntPtr wsif_handle, string algo_code_str);  // WSIFALLHANDLE or wsif handle

            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_ReqItem_AlgorithmCode(IntPtr wsif_handle);    // WSIFALLHANDLE or wsif handle

            //----------------------------------------------------------------------------
            [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public delegate void WSIFFUNC_ALGORITHMCODE(IntPtr wsif_handle, string algo_code_str);

            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_RegiCbf_AlgorithmCode(WSIFFUNC_ALGORITHMCODE cbf);

            //----------------------------------------------------------------------------
            // MULTI ITEM_ALGORITHM_CODE	
            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public static extern WSIORV WSIF_SetItem_MultiAlgorithmCode(IntPtr wsif_handle, uint algo_index, string algo_code_str);

            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_ReqItem_MultiAlgorithmCode(IntPtr wsif_handle, uint algo_index);

            //----------------------------------------------------------------------------
            [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public delegate void WSIFFUNC_MULTIALGORITHMCODE(IntPtr wsif_handle, uint algo_index, string algo_code_str);

            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_RegiCbf_MultiAlgorithmCode(WSIFFUNC_MULTIALGORITHMCODE cbf);

            //----------------------------------------------------------------------------
            // MULTI ITEM_ALGORITHM_LIGHTING_SET	
            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public static extern WSIORV WSIF_SetItem_AlgorithmLightingSet(IntPtr wsif_handle, uint algo_index, string light_name);

            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_ReqItem_AlgorithmLightingSet(IntPtr wsif_handle, uint algo_index);

            //----------------------------------------------------------------------------
            [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public delegate void WSIFFUNC_ALGORITHMLIGHTINGSET(IntPtr wsif_handle, uint algo_index, string light_name);

            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_RegiCbf_AlgorithmLightingSet(WSIFFUNC_ALGORITHMLIGHTINGSET cbf);

            //----------------------------------------------------------------------------
            // MULTI ITEM_ALGORITHM_SECTIONID	
            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public static extern WSIORV WSIF_SetItem_AlgorithmSectionID(IntPtr wsif_handle, uint algo_index, string algo_sectionid_str);

            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_ReqItem_AlgorithmSectionID(IntPtr wsif_handle, uint algo_index);

            //----------------------------------------------------------------------------
            [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public delegate void WSIFFUNC_ALGORITHMSECTIONID(IntPtr wsif_handle, uint algo_index, string algo_sectionid_str);

            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_RegiCbf_AlgorithmSectionID(WSIFFUNC_ALGORITHMSECTIONID cbf);

            //----------------------------------------------------------------------------
            // ITEM_ALIGN_PARAMETER
            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_SetItem_AlignParameter(IntPtr wsif_handle, double target_angle, double target_x, double target_y);

            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_ReqItem_AlignParameter(IntPtr wsif_handle);

            //----------------------------------------------------------------------------
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate void WSIFFUNC_ALIGNPARAMETER(IntPtr wsif_handle, double target_angle, double target_x, double target_y);

            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_RegiCbf_AlignParameter(WSIFFUNC_ALIGNPARAMETER cbf);

            //----------------------------------------------------------------------------
            // ITEM_CELL_MASK_TYPE	
            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_SetItem_CellMaskType(IntPtr wsif_handle, int cell_mask_type);

            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_ReqItem_CellMaskType(IntPtr wsif_handle);

            //----------------------------------------------------------------------------
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate void WSIFFUNC_CELLMASKTYPE(IntPtr wsif_handle, int cell_mask_type);

            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_RegiCbf_CellMaskType(WSIFFUNC_CELLMASKTYPE cbf);

            //----------------------------------------------------------------------------
            // ITEM_DEFECT_PRIORITY
            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public static extern WSIORV WSIF_SetItem_DefectPriority_SD(IntPtr wsif_handle, string defect_priority_str);

            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_ReqItem_DefectPriority_SD(IntPtr wsif_handle);

            //----------------------------------------------------------------------------
            [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public delegate void WSIFFUNC_DEFECTPRIORITYSD(IntPtr wsif_handle, string defect_priority_str);

            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_RegiCbf_DefectPriority_SD(WSIFFUNC_DEFECTPRIORITYSD cbf);

            //----------------------------------------------------------------------------
            // WSI_ITEM_EXPECTED_CAVITY
            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public static extern int WSIF_SetItem_ExpectedCavity(IntPtr wsif_handle, string expected_cavity_str);

            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_ReqItem_ExpectedCavity(IntPtr wsif_handle);

            //----------------------------------------------------------------------------
            [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public delegate void WSIFFUNC_EXPECTEDCAVITY(IntPtr wsif_handle, string expected_cavity_str);

            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_RegiCbf_ExpectedCavity(WSIFFUNC_EXPECTEDCAVITY cbf);

            //----------------------------------------------------------------------------
            // ITEM_IMAGE_INFORMATION
            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_SetItem_ReportImageInformation(IntPtr wsif_handle, int report_image_information_type); // 1(report), 0(Not report)

            [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public delegate void WSIFFUNC_IMAGEINFO(IntPtr wsif_handle, string wsif_tid_str, IntPtr array_raw_image_str, int num_of_array_raw_image_str, IntPtr array_result_image_str, int num_of_array_result_image_str);
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_RegiCbf_ImageInformation(WSIFFUNC_IMAGEINFO cbf);

            //----------------------------------------------------------------------------
            // ITEM_CURRENT_MALS_STEP
            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_SetItem_CurrentMalsStep(IntPtr wsif_handle, int current_mals_step);

            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_ReqItem_CurrentMalsStep(IntPtr wsif_handle);

            //----------------------------------------------------------------------------
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate void WSIFFUNC_CURMALSSTEP(IntPtr wsif_handle, int cur_mals_step);

            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_RegiCbf_CurrentMalsStep(WSIFFUNC_CURMALSSTEP cbf);

            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_SetItem_LiveMalsStep(IntPtr wsif_handle, int live_mals_step);

            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_SetItem_ExtSnapMalsStep(IntPtr wsif_handle, int ext_snap_mals_step);

            //----------------------------------------------------------------------------
            // ITEM_LIVE_LIGHTING
            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public static extern WSIORV WSIF_SetItem_LiveLighting(IntPtr wsif_handle, string live_lighting_name);

            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public static extern WSIORV WSIF_SetItem_ExtSnapLighting(IntPtr wsif_handle, string ext_snap_lighting_name);

            //----------------------------------------------------------------------------
            // ITEM_FULL_SIMUL_MODE	
            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_SetItem_FullSimulMode(IntPtr wsif_handle, int full_simul_mode_type);// 1(Full simul), 0(Not-Full-Simul)

            //----------------------------------------------------------------------------
            // Item_RTC
            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_SetItem_RTC(
                                 IntPtr wsif_handle,     // WSIOALLHANDLE or wsio handle
                                 int year,               // 2016
                                 int month,              // 1~12
                                 int day,                // 1~31
                                 int hour,               // 0~23
                                 int minute,             // 0~59
                                 int second,             // 0~59
                                 int milliseconds,       // 0~999
                                 int only_ohter_host     // NOT_ZERO:set if peer is not same host computer. //ZERO: always set
                );

            //----------------------------------------------------------------------------
            // Item_LanguageCode
            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_SetItem_LanguageCode(IntPtr wsif_handle, WSIOLANGCODE lang_code);      // WSIOALLHANDLE or wsio handle

            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_ReqItem_LanguageCode(IntPtr wsif_handle);     // WSIFALLHANDLE or wsif handle 

            //----------------------------------------------------------------------------
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate void WSIFFUNC_LANGUAGECODE(IntPtr wsif_handle, WSIOLANGCODE lang_code);

            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_RegiCbf_LanguageCode(WSIFFUNC_LANGUAGECODE cbf);

            //----------------------------------------------------------------------------
            // ITEM_LGIT_LAS_INFO
            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public static extern WSIORV WSIF_SetItem_LAS_Info(IntPtr wsif_handle, string pair_info_str);


            //============================================================================
            // HOOK MALS RAW DATA
            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public static extern WSIORV WSIF_SendMalsFrameData(IntPtr wsif_handle, string frame_str);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public delegate void WSIFFUNC_MALSFRAMEDATA(IntPtr wsif_handle, string frame_str);
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_RegiCbf_MalsFrameData(WSIFFUNC_MALSFRAMEDATA cbf);

            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_EnableReceiveMalsFrameData(IntPtr wsif_handle, int on_off);

            //============================================================================
            // HOOK SERIAL FRAME
            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public static extern WSIORV WSIF_SendSerialFrameData(IntPtr wsif_handle, string device_port, string frame_str);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public delegate void WSIFFUNC_SERIALFRAME(IntPtr wsif_handle, string device_port, string frame_str);
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_RegiCbf_SerialFrameData(WSIFFUNC_SERIALFRAME cbf);

            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public static extern WSIORV WSIF_EnableReceiveSerialFrameData(IntPtr wsif_handle, string device_port, int on_off);

            //============================================================================
            // ALERT SYSTEM
            //----------------------------------------------------------------------------
            [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public delegate void WSIFFUNC_ALERT(IntPtr wsif_handle, WSIFALERTCODE alert_code, WSIFALERTLEVEL alert_level, WSIFALERTDEVICETYPE alert_type, string alert_model, string alert_id);

            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_RegiCbf_Alert(WSIFFUNC_ALERT cbf);

            //============================================================================
            // WSIO PARAMETER : USER DEFINED DATA.
            //----------------------------------------------------------------------------
            // USER RECORD : 4BYTES (unsinged int type) MEMORY
            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_SetUserRecord(IntPtr wsif_handle, uint user_record_index, uint user_record_value);
            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_GetUserRecord(IntPtr wsif_handle, uint user_record_index,
                out uint user_record_value);

            //----------------------------------------------------------------------------
            // USER STRING : WSIOUSERSTR(WSIOSIZEOF_USERSTRING bytes) string
            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public static extern WSIORV WSIF_SetUserString(IntPtr wsif_handle, uint user_string_index, string user_string, uint size_of_user_string);
            //----------------------------------------------------------------------------
            [DllImport(WSIO_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern WSIORV WSIF_GetUserString(IntPtr wsif_handle, uint user_string_index,
                [Out] StringBuilder user_string_buffer, uint size_of_user_string_buffer);

        }
    }
}