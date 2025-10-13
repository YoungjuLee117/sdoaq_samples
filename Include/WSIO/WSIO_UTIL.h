/*	WSIO_UTIL.h

	Copyright (c) SD Optics,Inc. All rights reserved.

	========================================================================================================================================================
	Revision history
	========================================================================================================================================================
	Version     date      Author         Descriptions
	--------------------------------------------------------------------------------------------------------------------------------------------------------
*/

#pragma once

#include "WSIO.h"

//====================================================================================================================================
// WSIO UTILities Definition & API
//------------------------------------------------------------------------------------------------------------------------------------
//	이 파일(WSIO_UTIL.h)에는 서비스로 제공되는 유틸리티 성격의 기능을 추가한다.
//	여기에 추가되는 내용은 WSIO 본연의 기능은 아니다.
//
//	1. 샘플 코드에서 사용되는 기능
//
//	2. 사용자의 편의를 위해 제공되는 기능
//
//------------------------------------------------------------------------------------------------------------------------------------

//============================================================================
// ENUMERATED DEFINITION
//----------------------------------------------------------------------------

typedef int WSUTIVOPMODE;
enum WSUTIVOPMODE_Enum
{
	WSUTIVOPMODE_NULL					= 0,
	WSUTIVOPMODE_INFOIMAGE				= (1 << 0),
	WSUTIVOPMODE_INFOPOS				= (1 << 1),
	WSUTIVOPMODE_INFORULLER				= (1 << 2),
	WSUTIVOPMODE_INFOOSD				= (1 << 4),
	WSUTIVOPMODE_IMGPROCESSING			= (1 << 3),
	WSUTIVOPMODE_TOPTITLE				= (1 << 5),
	WSUTIVOPMODE_TOPWINSIZE				= (1 << 6),
	WSUTIVOPMODE_RESERVED_07			= (1 << 7),
	WSUTIVOPMODE_RESERVED_08			= (1 << 8),
	WSUTIVOPMODE_RESERVED_09			= (1 << 9),
	WSUTIVOPMODE_ENLARGEMENT			= (1 << 10),
	WSUTIVOPMODE_FRAMEOUTER				= (1 << 11),
	WSUTIVOPMODE_FRAMEINNER				= (1 << 12),
	WSUTIVOPMODE_CREATE_WITH_BR_FUNC	= (1 << 20),
	WSUTIVOPMODE_RESERVED_22			= (1 << 22),
	WSUTIVOPMODE_RESERVED_23			= (1 << 23),
	WSUTIVOPMODE_RESERVED_24			= (1 << 24),
	WSUTIVOPMODE_RESERVED_25			= (1 << 25),
	WSUTIVOPMODE_RESERVED_26			= (1 << 26),
	WSUTIVOPMODE_RESERVED_27			= (1 << 27),
	WSUTIVOPMODE_VISION					= (WSUTIVOPMODE_INFOIMAGE | WSUTIVOPMODE_INFOPOS | WSUTIVOPMODE_INFORULLER
												| WSUTIVOPMODE_ENLARGEMENT | WSUTIVOPMODE_CREATE_WITH_BR_FUNC),
};

#define WSUTIVOPMODE_MARKRECTRESIZE WSUTIVOPMODE_CREATE_WITH_BR_FUNC	// FOR OLD VERSION - Reserved for backward compatibility.
#define WSUTIVOPMODE_MARKBRIGHTNESS WSUTIVOPMODE_CREATE_WITH_BR_FUNC	// FOR OLD VERSION - Reserved for backward compatibility.

typedef int WSUTIVRESOURCE;
enum WSUTIVRESOURCE_Enum
{
	WSUTIVRESOURCE_UNUSEDAREA = 0,
	WSUTIVRESOURCE_OUTERFRAME = 1,
	WSUTIVRESOURCE_INNERFRAME = 2,
	WSUTIVRESOURCE_STATICPAINT = 3,
	WSUTIVRESOURCE_NOIMAGECOLOR = 4,
	WSUTIVRESOURCE_MRBRIGHTNESS = 11,
	WSUTIVRESOURCE_MRAUTOFOCUS = 12,
	WSUTIVRESOURCE_MRDSLOP = 14,
	WSUTIVRESOURCE_MRMULTIFOCUS = 13,
	WSUTIVRESOURCE_MRMULTILINE = 15,
	WSUTIVRESOURCE_MRMULTIRECT = 18,
	WSUTIVRESOURCE_MRMULTIELLIPSE = 19,
	WSUTIVRESOURCE_MRMULTIPOLYGON = 24,
	WSUTIVRESOURCE_MRMULTIMEASURE = 20, // MULTI line, rect, ellipse, polygon
	WSUTIVRESOURCE_OSDTEXTCOLOR = 21,
	WSUTIVRESOURCE_OSDBACKCOLOR = 22,
	WSUTIVRESOURCE_OSDLINECOLOR = 23,
	WSUTIVRESOURCE_GUIEFFECT = 16,
	WSUTIVRESOURCE_OSDTYPE = 17,
};

enum WSUTIVGUISHAPE_Enum
{
	WSUTIVGUISHAPE_EQUAL = (1 << 15),
	WSUTIVGUISHAPE_INNER = (1 << 16),
	WSUTIVGUISHAPE_4D = (1 << 9),
	WSUTIVGUISHAPE_8D = (1 << 10),
	WSUTIVGUISHAPE_PERPENDICULAR = (1 << 11),
	WSUTIVGUISHAPE_BOUNDARY4 = (1 << 12),
	WSUTIVGUISHAPE_LINECROSS = (1 << 13),
	WSUTIVGUISHAPE_DRAW_ASSISTANT = (1 << 14),
};

enum WSUTIVOSDTYPE_Enum
{
	WSUTIVOSDTYPE_TEXT_BOUNDARY = (1 << 0),
	WSUTIVOSDTYPE_TEXT_ID = (1 << 1),
	WSUTIVOSDTYPE_TEXT_TYPE = (1 << 2),
	WSUTIVOSDTYPE_TEXT_VALUE = (1 << 3),
};

typedef WSIOUINT16 WSUTIVOBJFUNC;
enum WSUTIVOBJFUNC_Enum
{
	WSUTIVOBJFUNC_NULL					= 0,
	WSUTIVOBJFUNC_SELF					= (1 << 0),
	WSUTIVOBJFUNC_SCRIPT				= (1 << 1),
	WSUTIVOBJFUNC_UPDATERECT			= (1 << 2),
	WSUTIVOBJFUNC_UPDATELINE			= (1 << 3),
	WSUTIVOBJFUNC_SPOT					= (1 << 4),
	WSUTIVOBJFUNC_UPDATELINES			= (1 << 5),
	WSUTIVOBJFUNC_BIT_ONDRAWING			= (1 << 15),
	WSUTIVOBJFUNC_ALL					= (WSUTIVOBJFUNC)-1
};

enum WSUTIVOBJTYPE_Enum
{
	WSUTIVOBJTPYE_NULL = NULL,
	WSUTIVOBJTPYE_AUTO_FOCUS = 1,
	WSUTIVOBJTPYE_FIXED_FOCUS = 2,
	WSUTIVOBJTPYE_EDOF = 3,
	WSUTIVOBJTPYE_MEASURE_LINE = 4,
	WSUTIVOBJTPYE_MEASURE_RECT = 5,
	WSUTIVOBJTPYE_MEASURE_ELLIPSE = 6,
	WSUTIVOBJTPYE_MEASURE_POLYGON = 7,
};

typedef int WSUTIVIMAGEMODE;
enum WSUTIVIMAGEMODE_Enum
{
	WSUTIVIMAGEMODE_NORMAL				= 0,	// default value
	WSUTIVIMAGEMODE_STATICPAINT			= 1,
};

typedef int WSUTIVFLIPBITS;
enum WSUTIVFLIPBITS_Enum
{	// If the FLIP attributes and the ROTATE attributes are given at the same time, The ROTATE is perfromed first.
	WSUTIVFLIPBITS_NORMAL				= 0,	// default value
	WSUTIVFLIPBITS_VERT					= (1<<0),
	WSUTIVFLIPBITS_HORZ					= (1<<1),
	WSUTIVFLIPBITS_ROTATE_R180			= WSUTIVFLIPBITS_VERT | WSUTIVFLIPBITS_HORZ,
	WSUTIVFLIPBITS_ROTATE_R90			= (1<<2),
	WSUTIVFLIPBITS_ROTATE_R270			= (1<<3),
};

//----------------------------------------------------------------------------
// WSIO WINDOW EVENT CODE
// : WPARAM(LOWORD:child-id, HIWORD:WSUTWEVTCODE_Enum), LPARAM(data)
//----------------------------------------------------------------------------

typedef int WSUTWEVTCODE;
enum WSUTWEVTCODE_Enum
{
	WSUTWEVTCODE_BTNCLK				= 0x0011,	// Generated additionally when WSUTWEVTCODE_LBTNDN or WSUTWEVTCODE_RBTNDN events occur.
	WSUTWEVTCODE_LBTNDN				= 0x0021,
	WSUTWEVTCODE_LBTNUP				= 0x0022,
	WSUTWEVTCODE_LBTNDCLK			= 0x0023,
	WSUTWEVTCODE_RBTNDN				= 0x0031,
	WSUTWEVTCODE_RBTNUP				= 0x0032,
	WSUTWEVTCODE_RBTNDCLK			= 0x0033,

	WSUTWEVTCODE_EDITCHECK			= 0x1011,	// Generated additionally when WSUTWEVTCODE_ENTERDOWN or WSUTWEVTCODE_FOCUSOUT events occur
	WSUTWEVTCODE_FOCUSOUT			= 0x1112,
	WSUTWEVTCODE_ENTERDOWN			= 0x1221,

	WSUTWEVTCODE_REQLAYOUT			= 0x4101,
	WSUTWEVTCODE_VALUEUPDATED		= 0x4201,

	WSUTWEVTCODE_TITLECLK			= 0x8111,
	WSUTWEVTCODE_VMCAMCLK			= 0x8211,	// bool(Button state) , The view camera button click
	WSUTWEVTCODE_VMCAMLBTNDBLCLK	= 0x8223,	// bool(Button state) , The view camera button double-click
	WSUTWEVTCODE_VMCAPCLK			= 0x8311,	// bool(Button state) , The view captured button click
	WSUTWEVTCODE_VMCAPLBTNDBLCLK	= 0x8323,	// bool(Button state) , The view captured button double-click
	WSUTWEVTCODE_VMRESCLK			= 0x8411,	// bool(Button state) , The view result button click
	WSUTWEVTCODE_VMRESLBTNDBLCLK	= 0x8423,	// bool(Button state) , The view result button double-click
	WSUTWEVTCODE_LIVECLK			= 0x8511,	// bool(Button state) , The live button click
	WSUTWEVTCODE_LIVELBTNDBLCLK		= 0x8523,	// bool(Button state) , The live button double-click
	WSUTWEVTCODE_OFFREVLBTNDBLCLK	= 0x8623,	// The off-review button double-click
};

//====================================================================================================================================
// WSIO UTILities API
//------------------------------------------------------------------------------------------------------------------------------------

//============================================================================
// IMAGE VIEWER
//----------------------------------------------------------------------------
WSIODLL_API	WSIORV			WSUT_IV_CreateImageViewer(
			WSIOCSTR				profile_str,
			WSIOVOID				parent_hwnd,
			WSIOVOID*				ptr_viewer_hwnd,
			WSIOUINT				viewer_control_id,
			WSUTIVOPMODE			operation_mode
			);
WSIODLL_API	WSIORV			WSUT_CreateImageViewer(		// OLD VERSION
			WSIOCSTR				profile_str,
			WSIOVOID				parent_hwnd,
			WSIOVOID*				ptr_viewer_hwnd,
			WSIOUINT				viewer_control_id
			);

//----------------------------------------------------------------------------
WSIODLL_API	WSIORV			WSUT_IV_DestroyImageViewer(
			WSIOVOID				viewer_hwnd
			);
WSIODLL_API	WSIORV			WSUT_DestroyImageViewer(	// OLD VERSION
			WSIOVOID				viewer_hwnd
			);

//----------------------------------------------------------------------------
WSIODLL_API	WSIORV			WSUT_IV_SetEventMessage(
			WSIOVOID				viewer_hwnd,
			WSIOUINT				event_message_id
			);

//----------------------------------------------------------------------------
typedef void(*WSUTFUNC_IV_ONPAINT)(
			WSIOUINT				viewer_control_id,
			WSIOVOID				mem_cdc_ptr,
			WSIOINT					position_left,
			WSIOINT					position_top,
			WSIOINT					position_right,
			WSIOINT					position_bottom
			);
WSIODLL_API	WSIORV			WSUT_IV_RegiCbf_OnPaintPost(
			WSIOVOID				viewer_hwnd,
			WSUTFUNC_IV_ONPAINT		cbf
			);

//----------------------------------------------------------------------------
WSIODLL_API	WSIORV			WSUT_IV_ShowWindow(
			WSIOVOID				viewer_hwnd,
			WSIOINT					flag_show,
			WSIOINT					position_left,
			WSIOINT					position_top,
			WSIOINT					position_right,
			WSIOINT					position_bottom
			);
WSIODLL_API	WSIORV			WSUT_ShowWindow(	// OLD VERSION
			WSIOVOID				viewer_hwnd,
			WSIOINT					flag_show,
			WSIOINT					position_left,
			WSIOINT					position_top,
			WSIOINT					position_right,
			WSIOINT					position_bottom
			);

//----------------------------------------------------------------------------
typedef void(*WSUTFUNC_IV_STRING)(
			WSIOVOID				viewer_hwnd,
			WSUTIVOBJFUNC			obj_func,
			WSIOUINT16				cb_data,
			WSIOCSTR				script_str
			);

WSIODLL_API WSIOUINT64		WSUT_IV_EncodeWparam(
			WSUTIVOBJFUNC			obj_func,
			WSIOUINT16				cb_data,
			WSIOUINT				data32
			);

WSIODLL_API WSIORV			WSUT_IV_DecodeWparam(
			WSIOUINT64				wparam,
			WSUTIVOBJFUNC*			p_obj_func,
			WSIOUINT16*				p_cb_data,
			WSIOUINT*				p_data32
			);

WSIODLL_API WSIOUINT64		WSUT_IV_EncodeRect(
			WSIOUINT16				left,
			WSIOUINT16				top,
			WSIOUINT16				right,
			WSIOUINT16				bottom
			);

WSIODLL_API WSIORV			WSUT_IV_DecodeRect(
			WSIOUINT64				u64,
			WSIOUINT16*				p_left,
			WSIOUINT16*				p_top,
			WSIOUINT16*				p_right,
			WSIOUINT16*				p_bottom
			);

WSIODLL_API WSIOUINT64		WSUT_IV_EncodePoint(
			WSIOUINT				point_x,
			WSIOUINT				point_y
			);

WSIODLL_API WSIORV			WSUT_IV_DecodePoint(
			WSIOUINT64				u64,
			WSIOUINT*				p_point_x,
			WSIOUINT*				p_point_y
			);

WSIODLL_API WSIOUINT64		WSUT_IV_EncodePoint2(
			WSIOUINT16				point1_x,
			WSIOUINT16				point1_y,
			WSIOUINT16				point2_x,
			WSIOUINT16				point2_y
			);

WSIODLL_API WSIORV			WSUT_IV_DecodePoint2(
			WSIOUINT64				u64,
			WSIOUINT16*				p_point1_x,
			WSIOUINT16*				p_point1_y,
			WSIOUINT16*				p_point2_x,
			WSIOUINT16*				p_point2_y
			);

WSIODLL_API	WSIORV			WSUT_IV_ActivateFunction(
			WSIOVOID				viewer_hwnd,
			WSUTIVRESOURCE			resource_id,
			WSUTIVOBJFUNC			obj_func,
			WSIOUINT16				cb_data,
			WSUTFUNC_IV_STRING		cbf,
			WSIOVOID				receiver_hwnd,
			WSIOUINT				window_msg_id
			);

// 'WSUT_IV_SetFunctionActivation' is a deprecated API.
WSIODLL_API	WSIORV WSUT_IV_SetFunctionActivation(WSIOVOID, WSUTIVRESOURCE, WSIOINT, WSIOVOID, WSIOVOID, WSIOUINT);
// 'WSUT_IV_RegiCbf_FunctionScript' is a deprecated API.
WSIODLL_API	WSIORV WSUT_IV_RegiCbf_FunctionScript(WSUTFUNC_IV_STRING);

//----------------------------------------------------------------------------
WSIODLL_API	WSIORV			WSUT_IV_SetFunctionScript(
			WSIOVOID				viewer_hwnd,
			WSUTIVRESOURCE			resource_id,
			WSIOCSTR				script_str
			);

//----------------------------------------------------------------------------
WSIODLL_API	WSIORV			WSUT_IV_SetFunctionRectData(
			WSIOVOID				viewer_hwnd,
			WSUTIVRESOURCE			resource_id,
			WSIOINT					position_left,
			WSIOINT					position_top,
			WSIOINT					position_right,
			WSIOINT					position_bottom
			);

//----------------------------------------------------------------------------
WSIODLL_API	WSIORV			WSUT_IV_GetFunctionRectData(
	WSIOVOID				viewer_hwnd,
	WSUTIVRESOURCE			resource_id,
	WSIOINT*				ptr_position_left,
	WSIOINT*				ptr_position_top,
	WSIOINT*				ptr_position_right,
	WSIOINT*				ptr_position_bottom
);

//----------------------------------------------------------------------------
WSIODLL_API	WSIORV			WSUT_IV_SetFunctionLineData(
			WSIOVOID				viewer_hwnd,
			WSUTIVRESOURCE			resource_id,
			WSIOINT					begin_point_x,
			WSIOINT					begin_point_y,
			WSIOINT					end_point_x,
			WSIOINT					end_point_y
			);

//----------------------------------------------------------------------------
WSIODLL_API	WSIORV			WSUT_IV_UpdateManualFocus(
			WSIOVOID				viewer_hwnd,
			WSIOUINT				manual_focus
			);

//----------------------------------------------------------------------------
// CALLBACK WINDOW MESSAGE STRUCTURE
// WSUTIVRESOURCE_MRAUTOFOCUS:
//		WPARAM(ID)
//		LPARAM(8 bytes rectangular position data): B7B6(left), B5B4(top), B3B2(right), B1B0(bottom)
//		=> WSUT_IV_Decoding8BytesToRect

WSIODLL_API WSIORV			WSUT_IV_Decoding8BytesToRect(
			unsigned long long		lParam,
			WSIOINT*				ptr_left,
			WSIOINT*				ptr_top,
			WSIOINT*				ptr_right,
			WSIOINT*				ptr_bottom
			);

//----------------------------------------------------------------------------
WSIODLL_API	WSIORV			WSUT_IV_SetResource(
			WSIOVOID				viewer_hwnd,
			WSUTIVRESOURCE			resource_id,
			WSIOUINT				value
);

//----------------------------------------------------------------------------
WSIODLL_API	WSIORV			WSUT_IV_SetColor(
			WSIOVOID				viewer_hwnd,
			WSUTIVRESOURCE			resource_id,
			WSIORGB					rgb_color
			);
  
//----------------------------------------------------------------------------
WSIODLL_API	WSIORV			WSUT_IV_SetImageMode(
			WSIOVOID				viewer_hwnd,
			WSUTIVIMAGEMODE			image_mode
			);

//----------------------------------------------------------------------------
WSIODLL_API	WSIORV			WSUT_IV_SetFlipMode(
			WSIOVOID				viewer_hwnd,
			WSUTIVFLIPBITS			flip_bits
			);

//----------------------------------------------------------------------------
WSIODLL_API	WSIORV			WSUT_IV_AttachFile(
			WSIOVOID				viewer_hwnd,
			WSIOCSTR				file_name_str
			);
WSIODLL_API	WSIORV			WSUT_AttachFile(	// OLD VERSION
			WSIOVOID				viewer_hwnd,
			WSIOCSTR				file_name_str
			);

//----------------------------------------------------------------------------
WSIODLL_API	WSIORV			WSUT_IV_AttachRawImgData(
			WSIOVOID				viewer_hwnd,
			WSIOUINT				width,
			WSIOUINT				height,
			WSIOUINT				line_bytes,
			WSIOUINT				pixel_bytes,
			WSIOVOID				data,
			WSIOUINT				data_size
			);

//----------------------------------------------------------------------------
WSIODLL_API	WSIORV			WSUT_IV_AttachRawImgData_V2(
			WSIOVOID				viewer_hwnd,
			WSIOUINT				width,
			WSIOUINT				height,
			WSIOUINT				line_bytes,
			WSIOUINT				pixel_bytes,
			WSIOVOID				data,
			WSIOUINT				data_size,
			WSIOCSTR				path_name_str
			);

//----------------------------------------------------------------------------
WSIODLL_API	WSIORV			WSUT_IV_AttachRawImgData_F3(
	WSIOVOID				viewer_hwnd,
	WSIOUINT				width,
	WSIOUINT				height,
	WSIOUINT				line_bytes,
	WSIOUINT				pixel_bytes,
	WSIOVOID				data_surface,
	WSIOUINT				data_surface_size,
	WSIOCSTR				path_name_str,
	WSIOVOID				data_f3,
	WSIOUINT				data_f3_size
	);

//----------------------------------------------------------------------------
WSIODLL_API	WSIORV			WSUT_IV_AttachRawImgData_Z(
	WSIOVOID				viewer_hwnd,
	WSIOUINT				width,
	WSIOUINT				height,
	WSIOUINT				line_bytes,
	WSIOUINT				pixel_bytes,
	WSIOVOID				data_surface,
	WSIOUINT				data_surface_size,
	WSIOCSTR				path_name_str,
	WSIOVOID				data_z,
	WSIOUINT				data_z_size
);
WSIO_CS_API	WSIORV WSUT_IV_AttachRawImgData_Z_cs(WSIOVOID viewer_hwnd, WSIOUINT width, WSIOUINT height, WSIOUINT line_bytes, WSIOUINT pixel_bytes, WSIOVOID data_surface, WSIOUINT data_surface_size, WSIOCSTR path_name_str, WSIOVOID data_z, WSIOUINT data_z_size);

//----------------------------------------------------------------------------
WSIODLL_API	WSIORV			WSUT_IV_SetPixelWidth(
			WSIOVOID				viewer_hwnd,
			WSIODOUBLE				pixel_width_mm
			);

//============================================================================
// COMMAND LINE PARSING
//----------------------------------------------------------------------------
#define WSUTSERVERIP "serverip"
#define WSUTCLIENTIP "clientip"
#define WSUTPARENTHWND "parenthwnd"
//----------------------------------------------------------------------------
WSIODLL_API WSIORV			WSUT_IpFromCmdLine(
			WSIOCSTR				command_line_str,
			WSIOCSTR				token_str,
			WSIOPSTR				ip_buffer,
			WSIOUINT				size_of_ip_buffer,
			WSIOINT*				ptr_port
			);

//----------------------------------------------------------------------------
WSIODLL_API WSIORV			WSUT_IntFromCmdLine(
			WSIOCSTR				command_line_str,
			WSIOCSTR				token_str,
			WSIOINT*				ptr_int
			);

//============================================================================
// LINE SCRIPT
//----------------------------------------------------------------------------
WSIODLL_API WSIORV			WSUT_StringFromLineScript(
			WSIOCSTR				data_or_path_str,
			WSIOCSTR				token_str,
			WSIOPSTR				value_buffer,
			WSIOUINT				size_of_value_buffer
			);

//----------------------------------------------------------------------------
WSIODLL_API WSIORV			WSUT_MultiStringFromLineScript(
			WSIOCSTR				data_or_path_str,
			WSIOCSTR				token_str,
			WSIOPSTR*				ptr_value_buffers,
			WSIOUINT*				ptr_number_of_value_buffer,
			WSIOUINT				size_of_value_buffer
);
WSIO_CS_API WSIORV WSUT_MultiStringFromLineScript_cs(WSIOCSTR data_or_path_str, WSIOCSTR token_str, WSIOPSTR* ptr_value_buffers, WSIOUINT* ptr_number_of_value_buffer, WSIOUINT size_of_value_buffer);

//----------------------------------------------------------------------------
WSIODLL_API WSIORV			WSUT_IntFromLineScript(
			WSIOCSTR				data_or_path_str,
			WSIOCSTR				token_str,
			WSIOINT*				ptr_int
			);

//----------------------------------------------------------------------------
WSIODLL_API WSIORV			WSUT_Uint64FromLineScript(
			WSIOCSTR				data_or_path_str,
			WSIOCSTR				token_str,
			WSIOUINT64*				ptr_uint64
			);

//----------------------------------------------------------------------------
WSIODLL_API WSIORV			WSUT_AddressFromLineScript(
			WSIOCSTR				data_or_path_str,
			WSIOCSTR				token_str,
			WSIOVOID*				ptr_address
			);

//============================================================================
// SHARED MEMORY
//----------------------------------------------------------------------------
WSIODLL_API WSIORV			WSUT_CopyFromSharedMemory(
			WSIOCSTR				shared_memory_name_str,
			WSIOVOID*				ptr_to_memory,
			WSIOINT					offset_of_shared_memory,
			WSIOINT					size_to_copy
			);

//============================================================================
// IMAGE FILE
//----------------------------------------------------------------------------
WSIODLL_API WSIORV			WSUT_WriteImageFile(
			WSIOCSTR				file_str,
			WSIOVOID*				ptr_data,
			WSIOINT					width,
			WSIOINT					height,
			WSIOINT					pixel_bytes,
			// If 'data_size' is NULL, the actual data size is used as a calculated value.
			WSIOINT					data_size
			);

//----------------------------------------------------------------------------
WSIODLL_API WSIORV			WSUT_GetBmpAttributes(
	WSIOCSTR				file_str,
	WSIOINT*				ptr_width,
	WSIOINT*				ptr_height,
	WSIOINT*				ptr_pixel_bytes
);

//----------------------------------------------------------------------------
WSIODLL_API WSIORV WSUT_GetRainbowBGR(float v255, unsigned char* pbgr);
WSIO_CS_API WSIORV WSUT_GetRainbowBGR_cs(float v255, unsigned char* pbgr);
WSIODLL_API WSIORV WSUT_GenerateRainbowColorMap(float* pFloat, int stride, size_t pixels, unsigned char* pbgr_map, float lowest, float highest);
WSIO_CS_API WSIORV WSUT_GenerateRainbowColorMap_cs(float* pFloat, int stride, size_t pixels, unsigned char* pbgr_map, float lowest, float highest);

//============================================================================
// LICENSE
//----------------------------------------------------------------------------
WSIODLL_API WSIOINT			WSUT_CheckLicense_TypeA(
			void
			);

//============================================================================
// GET INSTALLER DATA
//----------------------------------------------------------------------------
WSIODLL_API WSIORV WSUT_GetInstalledPath_TypeA(
	WSIOCSTR target_name_str,
	WSIOPSTR path_buffer,
	WSIOUINT size_of_path_buffer
);

//====================================================================================================================================
