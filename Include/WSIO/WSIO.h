/*	WSIO.h

	Copyright (c) SD Optics,Inc. All rights reserved.

	========================================================================================================================================================
	Revision history
	========================================================================================================================================================
	Version     date      Author         Descriptions
	--------------------------------------------------------------------------------------------------------------------------------------------------------
*/

// The following 14 functions are not exposed in a C-style manner.
// To provide C-style versions, additional functions have been included.
// The names of these additional functions are derived from the original function names, with the suffix "_cs" appended.
//		{ WSIF_GetItem_MalsDepthTableType , WSIF_GetItem_MalsDepthTableType_cs }
//		{ WSIF_ReqItem_MalsStepAdjust_MM , WSIF_ReqItem_MalsStepAdjust_MM_cs }
//		{ WSIF_SendMalsFrameData , WSIF_SendMalsFrameData_cs }
//		{ WSIF_RegiCbf_MalsFrameData , WSIF_RegiCbf_MalsFrameData_cs }
//		{ WSIF_EnableReceiveMalsFrameData , WSIF_EnableReceiveMalsFrameData_cs }
//		{ WSUT_IV_AttachRawImgData_Z , WSUT_IV_AttachRawImgData_Z_cs }
//		{ WSUT_MultiStringFromLineScript , WSUT_MultiStringFromLineScript_cs }
//		{ WSUT_GetRainbowBGR , WSUT_GetRainbowBGR_cs }
//		{ WSUT_GenerateRainbowColorMap , WSUT_GenerateRainbowColorMap_cs }
//		{ WSGL_SetActivation , WSGL_SetActivation_cs }
//		{ WSGL_GetActivation , WSGL_GetActivation_cs }
//		{ WSGL_SetMode , WSGL_SetMode_cs }
//		{ WSGL_RegisterModeCallback , WSGL_RegisterModeCallback_cs }
//		{ WSGL_SetMeasureDataUnit , WSGL_SetMeasureDataUnit_cs }

#pragma once

//====================================================================================================================================
// WSIO (WiseScope integrated Objects) Common Definitioin & API
//------------------------------------------------------------------------------------------------------------------------------------
//============================================================================
// WSIODLL_EXPORTS
//----------------------------------------------------------------------------
#if defined(WSIODLL_EXPORTS)
#define WSIODLL_API	__declspec(dllexport)	// for DLL source
#define WSIO_CS_API	extern "C" __declspec(dllexport)
#else
#define WSIODLL_API extern //	__declspec(dllimport)	// for USER source
#define WSIO_CS_API extern "C" __declspec(dllimport)
#endif

//============================================================================
// ENUMERATED DEFINITION
//----------------------------------------------------------------------------
// WSIO RETURN VALUE
// - Zero(0) or Positive number : Positive meaning (Success, Pass, OK, ...)
// - Negative number : Negative meaning (Error, ...)
//----------------------------------------------------------------------------

typedef int	WSIORV;
enum WSIORV_Enum
{
	//==========================================
	WSIORV_SUCCESS						=  0,
	WSIORV_SUCCESS_TRUE					=  1,
	WSIORV_SUCCESS_FALSE				=  2,
	//==========================================
	WSIORV_ERROR_BEGIN					= -2000,
	//------------------------------------------
	WSIORV_FAIL							= -2001,
	WSIORV_NODLL						= -2038,		// Failed to load dll module
	WSIORV_NO_DLL_API					= -2040,		// Failed to load dll api
	WSIORV_DEPRECATED_API				= -2037,		// This api is deprecated.
	WSIORV_NO_FUNCTIONALITY				= -2041,		// No functionality

	WSIORV_WSIFCLASS_INVALID			= -2002,		// WSIFCLASS is invalid.
	WSIORV_WSIHANDLE_NOTEXIST			= -2003,		// WSIHANDLE does not exist.
	WSIORV_WSIHANDLE_NOTOPENED			= -2004,		// WSIHANDLE is not opened.
	WSIORV_WSIHANDLE_NOTESTABLISHED		= -2020,		// WSIHANDLE is not established.
	WSIORV_WSINETTYPE_INVALID			= -2005,		// WSIFNETTYPE is invalid.
	WSIORV_WSIO_OPENFAILED				= -2006,		// 
	WSIORV_WSIOLOGTYPE_INVALID			= -2007,		// 
	WSIORV_USERRECORDID_INVALID			= -2008,		// USER RECORD ID is invalid.
	WSIORV_USERSTRINGID_INVALID			= -2009,		// USER STRING ID is invalid.
	WSIORV_INTERNAL_ERROR				= -2010,
	//WSIORV_WSIFTIDSTR_TOOLONG			= -2011,		// WSIOTIDSTR is too long
	WSIORV_ARG_STRISTOOLONG				= -2012,		// String argument is too long
	WSIORV_ARG_BLOCKISTOOLONG			= -2021,		// Block argument is too long
	WSIORV_ARG_BUFISNOTSUFFICIENT		= -2013,		// String buffer argument is not sufficent.
	WSIORV_ARG_NULLPOINTER				= -2035,		// There is one or more null pointer and/or null string arguments.
	WSIORV_TIMEOUT						= -2014,
	//WSIORV_NOWINDOWENVIRONMENT			= -2015,
	WSIORV_MAINHWNDALREADYASSIGNED		= -2016,
	WSIORV_FAILEDTOCREATEWINDOW			= -2017,
	WSIORV_HWNDISNOTWINDOW				= -2018,
	WSIORV_IMAGEVIEWER_TOOMANY			= -2019,
	WSIORV_IMAGEVIEWER_NOTFOUND			= -2022,
	WSIORV_FAILEDTOSETWINDOWPOS			= -2023,
	WSIORV_FAILEDTOATTACHFILE			= -2024,
	WSIORV_FAILEDTOATTACHIMAGEDATA		= -2029,
	WSIORV_NOIMGVIEWRESOURCE			= -2025,		// The resource ID is not applicable.
	WSIORV_NOIMGVIEWIMAGEMODE			= -2026,
	WSIORV_NOTYETIMPLEMENTED			= -2027,
	WSIORV_ARG_INVALIDALGORITHMID		= -2028,
	WSIORV_NOTOKEN						= -2030,
	WSIORV_TOKENHASINVALIDDATA			= -2031,
	WSIORV_CAMERA_BUSY					= -2032,
	WSIORV_FAILTOOPENSHAREDMEMORY		= -2033,
	WSIORV_NOTIMPLEMENTED				= -2034,
	WSIORV_FAILEDTOWRITEFILE			= -2036,		// File writing error
	WSIORV_FAILEDTOREADFILE				= -2039,
	WSIORV_OVER_SPEC					= -2042,
	//------------------------------------------
	WSIORV_ERROR_NEXT					= -2043,
	//------------------------------------------
	WSIORV_ERROR_END					= -4999
	//==========================================
};

//----------------------------------------------------------------------------
// WSIO LOG TYPE
//----------------------------------------------------------------------------

typedef int WSIOLOGTYPE;
enum WSIOLOGTYPE_Enum
{
	WSIOLOGTYPE_PROGRESS				= 0x1000,
	WSIOLOGTYPE_TEST					= 0x2000,
	WSIOLOGTYPE_TEMP					= 0x2001,
	WSIOLOGTYPE_ERROR					= 0x4000,
	WSIOLOGTYPE_WARNING					= 0x5000,
	WSIOLOGTYPE_WSIMESSAGE				= 0x0010,
	WSIOLOGTYPE_LOG						= 0x6000,
	WSIOLOGTYPE_DATA					= 0x8000,
};

//----------------------------------------------------------------------------
// WSIO IMAGE FORMAT
//----------------------------------------------------------------------------

typedef int WSIOIMGFORMAT;
enum WSIOIMGFORMAT_Enum
{
	WSIOIMGFORMAT_BMP = 0,
	WSIOIMGFORMAT_JPG = 1,
	WSIOIMGFORMAT_NONE = 2,
	WSIOIMGFORMAT_BIN = 3,
	WSIOIMGFORMAT_CSV = 4,
	WSIOIMGFORMAT_UNDEFINED = 5,

	WSIOIMGFORMAT_CZI = 6,
	WSIOIMGFORMAT_TIFF = 7,
	WSIOIMGFORMAT_ZIP = 8,
	WSIOIMGFORMAT_TXT = 9,
};


//============================================================================
// CONSTANT DEFINITION
//----------------------------------------------------------------------------
// LENGTH OF STRING
//----------------------------------------------------------------------------
//#define WSIOSIZEOF_TID	(255) //The size limit of TID has been removed. However, if you need this definition, you can remove the comment mark and use the value as 255.
#define WSIOSIZEOF_MAXPATH				(512)

//============================================================================
// C-LANGAUAGE STYLE TYPE DEFINITION
//----------------------------------------------------------------------------
typedef void*				WSIOVOID;
typedef char				WSIOCHAR;
typedef const char*			WSIOCSTR;	// constant string
typedef char*				WSIOPSTR;	// non-constant string
typedef unsigned char		WSIOBYTE;
typedef unsigned short		WSIOUINT16;
typedef int					WSIOINT;
typedef unsigned			WSIOUINT;
typedef unsigned long long	WSIOUINT64;
typedef unsigned long		WSIORGB;
typedef float				WSIOFLOAT;
typedef double				WSIODOUBLE;

typedef WSIOVOID			WSIHANDLE;

//============================================================================
// WSIO MAT STRUCTURE
//----------------------------------------------------------------------------
typedef unsigned char WSIOMATTYPE;
enum WSIOMATTYPE_Enum
{
	WSIOMATTYPE_UNDEFINED = 0,
	WSIOMATTYPE_INTEGER = 1,
	WSIOMATTYPE_REAL = 2,
	WSIOMATTYPE_GBR = 3,
	WSIOMATTYPE_RGB = 4,
};

//----------------------------------------------------------------------------
struct WSIOMAT
{
	WSIOCHAR description[16]; // [offset 0] null terminated string
	WSIOBYTE reserved_16;
	WSIOMATTYPE pixel_type; // [offset 17]
	WSIOBYTE reserved_18;
	WSIOBYTE pixel_bytes; // [offset 19]
	WSIOINT total_size; // [offset 20]
	WSIOINT cols; // [offset 24] width
	WSIOINT rows; // [offset 28] height
	WSIOINT line_bytes; // [offset 32] = cols * pixel_bytes + pad bytes
	WSIOBYTE reserved_36[220];
	//WSIOBYTE data_ptr[1]; // [offset 256] // data from here
};
#define WSIOMAT_DATA_OFFSET 256

//============================================================================
// C-LANGAUAGE STYLE FUNCTION DEFINITION
//----------------------------------------------------------------------------
typedef void(*WSIOFUNC_WSIHANDLE)(WSIHANDLE wsi_handle);
//----------------------------------------------------------------------------
typedef void(*WSIOFUNC_REPLYPARAMS)(WSIHANDLE wsif_handle, WSIOCSTR wsif_params_str);
//----------------------------------------------------------------------------

//====================================================================================================================================
// WSIO (WiseScope integrated Objects) Common API
//------------------------------------------------------------------------------------------------------------------------------------

//============================================================================
// WSIO BASIC FUNCTIONS
//----------------------------------------------------------------------------
// GET WSIO & SDC VERSION
//----------------------------------------------------------------------------
WSIODLL_API	WSIOCSTR		WSIO_GetVersion(
			WSIOINT					version_type
			);

//----------------------------------------------------------------------------
// SET MAIN WINDOW HANDLE
//----------------------------------------------------------------------------
WSIODLL_API	WSIORV			WSIO_SetMainHWND(
			WSIOVOID				main_hwnd
			);

//============================================================================
// LAST ERROR
//----------------------------------------------------------------------------
WSIODLL_API	WSIORV			WSIO_LastErrorCode(
			void
			);
//----------------------------------------------------------------------------
WSIODLL_API	WSIOINT			WSIO_LastErrorString(
			WSIOPSTR				dest_buffer,
			WSIOINT					size_of_dest_buffer
			);

//============================================================================
// LOG SYSTEM
//----------------------------------------------------------------------------
WSIODLL_API	WSIORV			WSIO_LogEnable(
			WSIOLOGTYPE				log_type,
			WSIOINT					log_enable	// NOT_ZERO:enable, ZERO:disable
			);
//----------------------------------------------------------------------------
typedef void (*WSIOFUNC_LOG)(
			WSIOLOGTYPE				log_type,
			WSIOCSTR				log_str
			);
WSIODLL_API	WSIORV			WSIO_RegiCbf_Log(WSIOFUNC_LOG cbf);

//====================================================================================================================================
