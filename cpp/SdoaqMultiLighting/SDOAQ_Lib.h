#pragma once

//============================================================================

//============================================================================
// SDOAQ , WSIO LIBRARY & HEADER
//----------------------------------------------------------------------------
#pragma comment(lib, "../../Include/SDOAQ/SDOAQ.lib")
#pragma comment(lib, "../../Include/WSIO/WSIODLL_R64.lib")

#include "..\..\Include\SDOAQ\SDOAQ_WS.h"
#include "..\..\Include\SDOAQ\SDOAQ_MULTIWS.h"
#include "..\..\Include\WSIO\WSIO_UTIL.h"
#include "..\..\Include\WSIO\WSIO_GL.h"


//============================================================================
// SDOAQ DEFINITION
//----------------------------------------------------------------------------
#define MONOBYTES			1
#define COLORBYTES			3
#define XYZNUMS				3
enum ERingBufEdofV2Offset
{// ref. SDOAQ_WS.h/SDOAQ_PlayEdofEx
	RBE2O_EDOF = 0,
	RBE2O_SMAP = 1,
	RBE2O_QMAP = 2,
	RBE2O_HMAP = 3,
	RBE2O_PCLOUD = 4,
	RBE2O_NUMS = 5 // edof + stepmap + qualitymap + heightmap + pointcloud
};

//============================================================================
// SDOAQ Functions
//----------------------------------------------------------------------------
inline bool IsMonoCameraInstalled()
{
	int cameraColor;
	(void)::SDOAQ_GetIntParameterValue(piCameraColor, &cameraColor);
	return (cameraColor == ccMono);
}

//----------------------------------------------------------------------------
inline bool GetIntParameterValue(eParameterId pi, int* pvalue)
{
	bool available;
	return true
		&& ecNoError == ::SDOAQ_IsParameterAvailable(pi, &available)
		&& available
		&& (pvalue == NULL || ecNoError == ::SDOAQ_GetIntParameterValue(pi, pvalue))
		;
}

inline bool IsSupportAutoExposure(void)
{
	return GetIntParameterValue(piFeatureAutoExposure, NULL);
}

inline bool IsSupportAutoWB(void)
{
	return GetIntParameterValue(piFeatureAutoWhiteBalance, NULL);
}

inline bool IsSupportAutoIlluminate(void)
{
	return GetIntParameterValue(piFeatureAutoIlluminate, NULL);
}

inline bool IsSupportBinning(void)
{
	return GetIntParameterValue(piFeatureBinning, NULL);
}

//----------------------------------------------------------------------------
inline eErrorCode SetSdoaqFocusRect(const CRect& rc)
{
	const auto rv1 = ::SDOAQ_SetIntParameterValue(piFocusLeftTop, ((rc.left & 0x0000FFFF) << 16) | (rc.top & 0x0000FFFF) << 0);
	const auto rv2 = ::SDOAQ_SetIntParameterValue(piFocusRightBottom, ((rc.right & 0x0000FFFF) << 16) | (rc.bottom & 0x0000FFFF) << 0);
	if (rv1 != ecNoError)
	{
		return rv1;
	}
	if (rv2 != ecNoError)
	{
		return rv2;
	}
	return ecNoError;
}

//----------------------------------------------------------------------------
inline LPCTSTR GetSdoaqErrorString(int eCode)
{
	switch (eCode)
	{
	case ecNoError: return _T("ecNoError");
	case ecUnknownError: return _T("ecUnknownError");
	case ecLowMemory: return _T("ecLowMemory");
	case ecFileNotFound: return _T("ecFileNotFound");
	case ecNotInitialized: return _T("ecNotInitialized");
	case ecInvalidParameter: return _T("ecInvalidParameter");
	case ecTimeoutOccurred: return _T("ecTimeoutOccurred");
	case ecParameterIsNotWritable: return _T("ecParameterIsNotWritable");
	case ecParameterIsNotSet: return _T("ecParameterIsNotSet");
	case ecAutoAdjustTargetNotReached: return _T("ecAutoAdjustTargetNotReached");
	case ecNotImplemented: return _T("ecNotImplemented");
	case ecNotSupported: return _T("ecNotSupported");
	case ecNoAuthorization: return _T("ecNoAuthorization");
	case ecNoWisescope: return _T("ecNoWisescope");
	case ecNoLighting: return _T("ecNoLighting");

	default: return _T("Invalid");
	}
}

//----------------------------------------------------------------------------
inline bool IS_HIGHER_or_EQUAL_VERSION(int major_version, int minor_version, int patch_version)
{
	const auto read_major = ::SDOAQ_GetMajorVersion();
	const auto read_minor = ::SDOAQ_GetMinorVersion();
	const auto read_patch = ::SDOAQ_GetPatchVersion();
	if (read_major != major_version) return read_major > major_version;
	if (read_minor != minor_version) return read_minor > minor_version;
	return read_patch >= patch_version;
}

//============================================================================
// Utilities
//----------------------------------------------------------------------------
#include <vector>
struct t_amsg
{
	t_amsg(UINT message, WPARAM wParam, LPARAM lParam) : message(message), wParam(wParam), lParam(lParam) { ; }
	UINT message;
	WPARAM wParam;
	LPARAM lParam;
};
typedef std::vector<t_amsg> t_vmsg;

//----------------------------------------------------------------------------
inline t_vmsg RetrieveMessages(HWND hwnd, UINT wMsgFilter)
{
	t_vmsg v_msg;
	MSG msg;
	while (::PeekMessage(&msg, hwnd, wMsgFilter, wMsgFilter, PM_REMOVE))
	{
		v_msg.push_back(t_amsg(msg.message, msg.wParam, msg.lParam));
	}
	return v_msg;
}

//----------------------------------------------------------------------------
inline t_vmsg RetrieveMessages(HWND hwnd, UINT wMsgFilter, WPARAM wParam, LPARAM lParam)
{
	t_vmsg v_msg;
	v_msg.push_back(t_amsg(wMsgFilter, wParam, lParam));
	MSG msg;
	while (::PeekMessage(&msg, hwnd, wMsgFilter, wMsgFilter, PM_REMOVE))
	{
		v_msg.push_back(t_amsg(msg.message, msg.wParam, msg.lParam));
	}
	return v_msg;
}

//----------------------------------------------------------------------------
inline bool UpdateLastMessage(HWND hwnd, UINT wMsgFilter, WPARAM& wParam, LPARAM& lParam)
{
	auto v_msg = RetrieveMessages(hwnd, wMsgFilter);
	if (v_msg.size())
	{
		wParam = v_msg.rbegin()->wParam;
		lParam = v_msg.rbegin()->lParam;
		return true;
	}
	return false;
}

//----------------------------------------------------------------------------
inline void MsgWait(DWORD dwMillisecond)
{
	MSG msg;
	const auto tick_begin = GetTickCount64();
	auto tick_current = tick_begin;

	while (tick_current - tick_begin <= dwMillisecond)
	{
		while (::PeekMessage(&msg, NULL, 0, 0, PM_REMOVE))
		{
			TranslateMessage(&msg);
			DispatchMessage(&msg);
		}
		tick_current = GetTickCount64();
	}
}

//------------------------------------------------------------------------------------------------
inline void ProcessWindowMessage()
{
	MSG msg;
	while (::PeekMessage(&msg, NULL, NULL, NULL, PM_REMOVE))
	{
		::SendMessageW(msg.hwnd, msg.message, msg.wParam, msg.lParam);
	}
}

//----------------------------------------------------------------------------
// Multibyte string -> new unicode string
inline void SetMultibytes2WString(CStringW& sWS, const char* szMB)
{
	if (szMB)
	{
		const int nLen = ::MultiByteToWideChar(CP_ACP, MB_PRECOMPOSED, szMB, -1, NULL, NULL);
		if (nLen > 0)
		{
			const auto p_dest = sWS.GetBufferSetLength(nLen);
			::MultiByteToWideChar(CP_ACP, 0, (LPCCH)szMB, -1, p_dest, nLen);
			return;
		}
	}

	sWS.Empty();
}

//----------------------------------------------------------------------------
// Multibyte string -> new unicode string
inline CString* NewWString(const char* szMB)
{
	auto pWString = new CString;
	SetMultibytes2WString(*pWString, szMB);
	return pWString;
}

//----------------------------------------------------------------------------
typedef std::vector<UINT> t_vmsgid;
inline void DeleteAllWinmsgWithWStringPtrInLparam(HWND hwnd, t_vmsgid v_msgid)
{
	if (hwnd)
	{
		for (auto it = v_msgid.begin(); it != v_msgid.end(); it++)
		{
			MSG msg;
			while (::PeekMessage(&msg, hwnd, *it, *it, PM_REMOVE))
			{
				if (msg.lParam)
				{
					delete (CString*)msg.lParam;
				}
			}
		}
	}
}

//----------------------------------------------------------------------------
inline CString FString(LPCTSTR sFormat, ...)
{
	va_list args;
	va_start(args, sFormat);
	CString s;
	s.FormatV(sFormat, args);
	return s;
}

//----------------------------------------------------------------------------
inline CStringA FStringA(char* sFormat, ...)
{
	va_list args;
	va_start(args, sFormat);
	CStringA s;
	s.FormatV(sFormat, args);
	return s;
}

//----------------------------------------------------------------------------
inline CString GetCurrentDir()
{
	TCHAR currentDir[MAX_PATH];
	::GetCurrentDirectory(MAX_PATH, currentDir);
	return currentDir;
}
