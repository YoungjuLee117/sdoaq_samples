
// SdoaqCameraFrameCallbackDlg.cpp : implementation file
//

#include "pch.h"
#include "framework.h"
#include "SdoaqCameraFrameCallback.h"
#include "SdoaqCameraFrameCallbackDlg.h"
#include "afxdialogex.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

//============================================================================
// SDOAQ , WSIO LIBRARY & HEADER
//----------------------------------------------------------------------------
#pragma comment(lib, "../../Include/SDOAQ/SDOAQ.lib")
#pragma comment(lib, "../../Include/WSIO/WSIODLL_R64.lib")

#include "..\..\Include\SDOAQ\SDOAQ_WS.h"
#include "..\..\Include\SDOAQ\SDOAQ_LLAPI.h"
#include "..\..\Include\WSIO\WSIO_UTIL.h"


//----------------------------------------------------------------------------
static WSIOVOID g_hViewer = NULL;
//----------------------------------------------------------------------------
static void g_SDOAQ_InitDoneCallback(eErrorCode errorCode, char* pErrorMessage);
static void g_SDOAQ_FrameCallback(eErrorCode errorCode, unsigned char* pBuffer, size_t BufferSize, FrameDescriptor* pFrameDescriptor);
static void g_SDOAQ_SetCameraTriggerMode(eCameraTriggerMode ctm);
//----------------------------------------------------------------------------
static void g_LogLine(LPCTSTR sFormat, ...)
{
	static CString g_sLog;
	va_list args; va_start(args, sFormat);
	CString add_log; add_log.FormatV(sFormat, args);
	g_sLog += add_log + _T("\r\n");
	if (g_sLog.GetLength() >= 1400 * 40) { g_sLog = g_sLog.Right(1000 * 40); }
	if (theApp.m_pMainWnd)
	{
		CEdit* p_wnd = (CEdit*)theApp.m_pMainWnd->GetDlgItem(IDC_LOG);
		if (p_wnd)
		{
			p_wnd->SetWindowText(g_sLog);
			const int nLen = p_wnd->GetWindowTextLength();
			p_wnd->SetSel(nLen, nLen);
			//theApp.m_pMainWnd->PostMessage(WM_VSCROLL, SB_BOTTOM);
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
CString GetCurrentDir()
{
	TCHAR currentDir[MAX_PATH];
	::GetCurrentDirectory(MAX_PATH, currentDir);
	return currentDir;
}

//============================================================================

CSdoaqCameraFrameCallbackDlg::CSdoaqCameraFrameCallbackDlg(CWnd* pParent /*=nullptr*/)
	: CDialogEx(IDD_SDOAQCAMERAFRAMECALLBACK_DIALOG, pParent)
{
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
}

void CSdoaqCameraFrameCallbackDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
}

//----------------------------------------------------------------------------
BEGIN_MESSAGE_MAP(CSdoaqCameraFrameCallbackDlg, CDialogEx)
	ON_WM_CLOSE()
	ON_WM_SIZE()
	ON_COMMAND(IDC_CHECK_FRAME_CALLBACK, &OnBnClickedCheckFrameCallback)
	ON_COMMAND(IDC_SW_TRIGGER, &OnBnClickedSwTrigger)
	ON_CONTROL_RANGE(BN_CLICKED, IDC_TRIGGER_CAMERA_FREERUN, IDC_TRIGGER_GRABBER_EXTERNAL, &OnBnClickedTriggerMode)
	ON_BN_CLICKED(IDC_BTN_SET_FOV, OnSetFov)
	ON_BN_CLICKED(IDC_BTN_SET_EXPOSURE, OnSetExposureTime)
	ON_BN_CLICKED(IDC_BTN_SET_GAIN, OnSetGain)
	ON_BN_CLICKED(IDC_BTN_SET_WB, OnSetWhitebalance)
	ON_BN_CLICKED(IDC_BTN_SET_STRING, OnSetStringRegister)
	ON_BN_CLICKED(IDC_BTN_SET_INT, OnSetIntegerRegister)
	ON_BN_CLICKED(IDC_BTN_SET_DOUBLE, OnSetDoubleRegister)
	ON_BN_CLICKED(IDC_BTN_SET_BOOL, OnSetBoolRegister)
END_MESSAGE_MAP()


//============================================================================
// WINDOWS MESSAGE HANDLER
//----------------------------------------------------------------------------
BOOL CSdoaqCameraFrameCallbackDlg::OnInitDialog()
{
	CDialogEx::OnInitDialog();

	SetIcon(m_hIcon, TRUE);
	SetIcon(m_hIcon, FALSE);

	GetDlgItem(IDC_CHECK_FRAME_CALLBACK)->EnableWindow(FALSE);
	GetDlgItem(IDC_SW_TRIGGER)->EnableWindow(FALSE);
	GetDlgItem(IDC_EDIT_FOV_WIDTH)->EnableWindow(FALSE);
	GetDlgItem(IDC_EDIT_FOV_HEIGHT)->EnableWindow(FALSE);
	GetDlgItem(IDC_EDIT_FOV_OFFSETX)->EnableWindow(FALSE);
	GetDlgItem(IDC_EDIT_FOV_OFFSETY)->EnableWindow(FALSE);
	GetDlgItem(IDC_EDIT_EXPOSURE)->EnableWindow(FALSE);
	GetDlgItem(IDC_EDIT_GAIN)->EnableWindow(FALSE);
	GetDlgItem(IDC_EDIT_WB_R)->EnableWindow(FALSE);
	GetDlgItem(IDC_EDIT_WB_G)->EnableWindow(FALSE);
	GetDlgItem(IDC_EDIT_WB_B)->EnableWindow(FALSE);

	g_LogLine(_T("================================================"));
	g_LogLine(_T(" SDOAQ CAMERA FRAME CALLBACK SAMPLE"));
	g_LogLine(_T("================================================"));

	const int ver_major = ::SDOAQ_GetMajorVersion();
	const int ver_minor = ::SDOAQ_GetMinorVersion();
	const int ver_patch = ::SDOAQ_GetPatchVersion();
	g_LogLine(_T("SDOAQ DLL version is \"%d.%d.%d\""), ver_major, ver_minor, ver_patch);

	// set the cam files folder path
	::SDOAQ_SetCamfilePath(CT2A(FString(_T("%s\\..\\..\\Include\\SDOAQ\\CamFiles"), GetCurrentDir())));

	g_LogLine(_T("start initialization..."));
	const eErrorCode rv_sdoaq = ::SDOAQ_Initialize(NULL, NULL, g_SDOAQ_InitDoneCallback);
	if (ecNoError != rv_sdoaq)
	{
		g_LogLine(_T("SDOAQ_Initialize() returns error(%d)."), rv_sdoaq);
	}

	const WSIORV rv_wsio = ::WSUT_IV_CreateImageViewer((WSIOCSTR)_T("VIEWER")
		, (WSIOVOID)(this->m_hWnd), &g_hViewer, NULL
		, WSUTIVOPMODE_VISION | WSUTIVOPMODE_TOPTITLE);
	if (WSIORV_SUCCESS > rv_wsio)
	{
		g_LogLine(_T("WSUT_IV_CreateImageViewer() returns error(%d)."), rv_wsio);
	}

	SendMessage(WM_SIZE); // invoke WSUT_IV_ShowWindow call with size.

	return TRUE;  // return TRUE  unless you set the focus to a control
}

//----------------------------------------------------------------------------
BOOL CSdoaqCameraFrameCallbackDlg::PreTranslateMessage(MSG* pMsg)
{
	// TODO: Add your specialized code here and/or call the base class
	if (pMsg->message == WM_KEYDOWN)
	{
		if (pMsg->wParam == VK_RETURN)
			return TRUE;
		else if (pMsg->wParam == VK_ESCAPE)
			return TRUE;
	}

	return CDialogEx::PreTranslateMessage(pMsg);
}

//----------------------------------------------------------------------------
void CSdoaqCameraFrameCallbackDlg::OnClose()
{
	(void)::SDOAQ_Finalize();
	(void)::WSUT_IV_DestroyImageViewer(g_hViewer);

	CDialogEx::OnClose();
}

//----------------------------------------------------------------------------
void CSdoaqCameraFrameCallbackDlg::OnSize(UINT nType, int cx, int cy)
{
	CDialogEx::OnSize(nType, cx, cy);

	CWnd* p_wnd = GetDlgItem(IDC_VIEWER);
	if (p_wnd)
	{
		CRect rc;
		GetDlgItem(IDC_VIEWER)->GetWindowRect(rc);
		ScreenToClient(rc);
		rc.top += 24;

		const WSIORV rv_wsio = ::WSUT_IV_ShowWindow(g_hViewer, (WSIOINT)true, rc.left, rc.top, rc.right, rc.bottom);
		if (WSIORV_SUCCESS > rv_wsio)
		{
			g_LogLine(_T("WSUT_IV_ShowWindow() returns error(%d)."), rv_wsio);
		}
	}
	else
	{
		// before OnInitDialog()
	}
}

//----------------------------------------------------------------------------
void CSdoaqCameraFrameCallbackDlg::OnBnClickedCheckFrameCallback()
{
	eErrorCode rv_sdoaq;
	if (((CButton*)GetDlgItem(IDC_CHECK_FRAME_CALLBACK))->GetCheck() == BST_CHECKED)
	{
		g_LogLine(_T("register frame callback function!"));
		rv_sdoaq = ::SDOAQ_SetFrameCallback(g_SDOAQ_FrameCallback);
	}
	else
	{
		g_LogLine(_T("stop frame callback function!"));
		rv_sdoaq = ::SDOAQ_SetFrameCallback(NULL);
	}
	if (ecNoError != rv_sdoaq)
	{
		g_LogLine(_T("SDOAQ_SetFrameCallback() returns error(%d)."), rv_sdoaq);
	}
}

//----------------------------------------------------------------------------
void CSdoaqCameraFrameCallbackDlg::OnBnClickedSwTrigger()
{
	if (((CButton*)theApp.m_pMainWnd->GetDlgItem(IDC_CHECK_FRAME_CALLBACK))->GetCheck() != BST_CHECKED)
	{
		g_LogLine(_T(">>> You must check \"camera frame callback\" to receive frames!"));
		return;
	}

	static int g_sw_trigger_no = 0;
	g_LogLine(_T("execute software trigger (%d)"), ++g_sw_trigger_no);
	const eErrorCode rv_sdoaq = ::SDOAQ_ExecCameraSoftwareTrigger();
	if (ecNoError != rv_sdoaq)
	{
		g_LogLine(_T("SDOAQ_ExecCameraSoftwareTrigger() returns error(%d)."), rv_sdoaq);
	}
}

//----------------------------------------------------------------------------
void CSdoaqCameraFrameCallbackDlg::OnBnClickedTriggerMode(UINT uID)
{
	switch (uID)
	{
	case IDC_TRIGGER_CAMERA_FREERUN:
		g_SDOAQ_SetCameraTriggerMode(ctmCameraFreerun);
		break;
	case IDC_TRIGGER_CAMERA_SOFTWARE:
		g_SDOAQ_SetCameraTriggerMode(ctmCameraSoftware);
		break;
	case IDC_TRIGGER_CAMERA_EXTERNAL:
		g_SDOAQ_SetCameraTriggerMode(ctmCameraExternal);
		break;
	case IDC_TRIGGER_GRABBER_FREERUN:
		g_SDOAQ_SetCameraTriggerMode(ctmGrabberFreerun);
		break;
	case IDC_TRIGGER_GRABBER_SOFTWARE:
		g_SDOAQ_SetCameraTriggerMode(ctmGrabberSoftware);
		break;
	case IDC_TRIGGER_GRABBER_EXTERNAL:
		g_SDOAQ_SetCameraTriggerMode(ctmGrabberExternal);
		break;
	}

	if (((CButton*)theApp.m_pMainWnd->GetDlgItem(IDC_CHECK_FRAME_CALLBACK))->GetCheck() != BST_CHECKED)
	{
		g_LogLine(_T(">>> You must check \"camera frame callback\" to receive frames!"));
	}
}

//----------------------------------------------------------------------------
void CSdoaqCameraFrameCallbackDlg::OnSetFov()
{
	CString sWidth, sHeight, sOffsetX, sOffsetY;
	GetDlgItemText(IDC_EDIT_FOV_WIDTH, sWidth);
	GetDlgItemText(IDC_EDIT_FOV_HEIGHT, sHeight);
	GetDlgItemText(IDC_EDIT_FOV_OFFSETX, sOffsetX);
	GetDlgItemText(IDC_EDIT_FOV_OFFSETY, sOffsetY);

	int nDummy, nMaxWidth, nMaxHeight, nWidth, nHeight, nOffsetX, nOffsetY;
	nWidth = _ttoi(sWidth);
	nHeight = _ttoi(sHeight);
	nOffsetX = _ttoi(sOffsetX);
	nOffsetY = _ttoi(sOffsetY);

	eErrorCode rv1 = ::SDOAQ_GetIntParameterRange(piCameraFullFrameSizeX, &nDummy, &nMaxWidth);
	eErrorCode rv2 = ::SDOAQ_GetIntParameterRange(piCameraFullFrameSizeY, &nDummy, &nMaxHeight);
	if (ecNoError == rv1 && ecNoError == rv2)
	{
		if (nWidth <= nMaxWidth && nHeight <= nMaxHeight)
		{
			eErrorCode rv_sdoaq = ::SDOAQ_SetCameraRoiParameter(nWidth, nHeight, nOffsetX, nOffsetY, 1);
			if (ecNoError == rv_sdoaq)
			{
				int nBinning;
				rv_sdoaq = ::SDOAQ_GetCameraRoiParameter(&nWidth, &nHeight, &nOffsetX, &nOffsetY, &nBinning);
				if (ecNoError == rv_sdoaq)
				{
					SetDlgItemText(IDC_EDIT_FOV_WIDTH, FString(_T("%d"), nWidth));
					SetDlgItemText(IDC_EDIT_FOV_HEIGHT, FString(_T("%d"), nHeight));
					SetDlgItemText(IDC_EDIT_FOV_OFFSETX, FString(_T("%d"), nOffsetX));
					SetDlgItemText(IDC_EDIT_FOV_OFFSETY, FString(_T("%d"), nOffsetY));
					g_LogLine(_T("Set ROI (l:%d, t:%d, w:%d, h:%d)"), nOffsetX, nOffsetY, nWidth, nHeight);
				}
				else
				{
					g_LogLine(_T("SDOAQ_GetCameraRoiParameter() returns error(%d)."), rv_sdoaq);
				}
			}
			else
			{
				g_LogLine(_T("SDOAQ_SetCameraRoiParameter() returns error(%d)."), rv_sdoaq);
			}
		}
		else
		{
			g_LogLine(_T("SDOAQ_SetCameraParameter(FOV) value is out of range."));
		}
	}
	else
	{
		g_LogLine(_T("SDOAQ_GetIntParameterRange(piCameraFullFrameSizeX) returns error(%d)."), rv1);
		g_LogLine(_T("SDOAQ_GetIntParameterRange(piCameraFullFrameSizeY) returns error(%d)."), rv2);
	}
}

//----------------------------------------------------------------------------
void CSdoaqCameraFrameCallbackDlg::OnSetExposureTime()
{
	CString sValue;
	GetDlgItemText(IDC_EDIT_EXPOSURE, sValue);

	int nMin, nMax, nValue;
	nValue = _ttoi(sValue);

	eErrorCode rv_sdoaq = ::SDOAQ_GetIntParameterRange(piCameraExposureTime, &nMin, &nMax);
	if (ecNoError == rv_sdoaq)
	{
		if (nMin <= nValue && nValue <= nMax)
		{
			rv_sdoaq = ::SDOAQ_SetIntParameterValue(piCameraExposureTime, nValue);
			if (ecNoError == rv_sdoaq)
			{
				g_LogLine(_T("Set camera exposure time: %d"), nValue);
			}
			else
			{
				g_LogLine(_T("SDOAQ_SetIntParameterValue(piCameraExposureTime) returns error(%d)."), rv_sdoaq);
			}
		}
		else
		{
			g_LogLine(_T("CameraExposureTime value is out of range (%d ~ %d)."), nMin, nMax);
		}
	}
	else
	{
		g_LogLine(_T("SDOAQ_GetIntParameterRange(piCameraExposureTime) returns error(%d)."), rv_sdoaq);
	}
}

//----------------------------------------------------------------------------
void CSdoaqCameraFrameCallbackDlg::OnSetGain()
{
	CString sValue;
	GetDlgItemText(IDC_EDIT_GAIN, sValue);

	double dbMin, dbMax, dbValue;
	dbValue = _ttof(sValue);
	::SDOAQ_GetDblParameterRange(piCameraGain, &dbMin, &dbMax);

	if (dbMin <= dbValue && dbValue <= dbMax)
	{
		eErrorCode rv_sdoaq = ::SDOAQ_SetDblParameterValue(piCameraGain, dbValue);
		if (ecNoError == rv_sdoaq)
		{
			g_LogLine(_T("Set camera gain: %.3lf"), dbValue);
		}
		else
		{
			g_LogLine(_T("SDOAQ_SetDblParameterValue(piCameraGain) returns error(%d)."), rv_sdoaq);
		}
	}
	else
	{
		g_LogLine(_T("CameraGain value is out of range (%.3lf ~ %.3lf)."), dbMin, dbMax);
	}
}

//----------------------------------------------------------------------------
void CSdoaqCameraFrameCallbackDlg::OnSetWhitebalance()
{
	CString sRed, sGreen, sBlue;
	GetDlgItemText(IDC_EDIT_WB_R, sRed);
	GetDlgItemText(IDC_EDIT_WB_G, sGreen);
	GetDlgItemText(IDC_EDIT_WB_B, sBlue);

	double dbValue = _ttof(sRed);
	eErrorCode rv_sdoaq = ::SDOAQ_SetDblParameterValue(piWhiteBalanceRed, dbValue);
	if (ecNoError == rv_sdoaq)
	{
		g_LogLine(_T("Set camera whitebalance red: %.3lf"), dbValue);
	}
	else
	{
		g_LogLine(_T("SDOAQ_SetDblParameterValue(piWhiteBalanceRed) returns error(%d)."), rv_sdoaq);
	}

	dbValue = _ttof(sGreen);
	rv_sdoaq = ::SDOAQ_SetDblParameterValue(piWhiteBalanceGreen, dbValue);
	if (ecNoError == rv_sdoaq)
	{
		g_LogLine(_T("Set camera whitebalance green: %.3lf"), dbValue);
	}
	else
	{
		g_LogLine(_T("SDOAQ_SetDblParameterValue(piWhiteBalanceGreen) returns error(%d)."), rv_sdoaq);
	}

	dbValue = _ttof(sBlue);
	rv_sdoaq = ::SDOAQ_SetDblParameterValue(piWhiteBalanceBlue, dbValue);
	if (ecNoError == rv_sdoaq)
	{
		g_LogLine(_T("Set camera whitebalance blue: %.3lf"), dbValue);
	}
	else
	{
		g_LogLine(_T("SDOAQ_SetDblParameterValue(piWhiteBalanceBlue) returns error(%d)."), rv_sdoaq);
	}
}

//----------------------------------------------------------------------------
void CSdoaqCameraFrameCallbackDlg::OnSetStringRegister()
{
	CString sRegister, sValue;
	GetDlgItemText(IDC_EDIT_REGISTER, sRegister);
	GetDlgItemText(IDC_EDIT_REG_STRING, sValue);
	eErrorCode rv_sdoaq = ::SDOAQ_SetCameraParameterString(CT2CA(sRegister), CT2CA(sValue));
	if (ecNoError != rv_sdoaq)
	{
		g_LogLine(_T("SDOAQ_SetCameraParameterString(%s:%s) returns error(%d)."), sRegister, sValue, rv_sdoaq);
	}
}

//----------------------------------------------------------------------------
void CSdoaqCameraFrameCallbackDlg::OnSetIntegerRegister()
{
	//----------------------------------------------------------------------------
	// Depending on the register type, the value cannot be changed in the grabbing on state.
	// In this case, you must set the value after grabbing off.
	//----------------------------------------------------------------------------
	g_LogLine(_T("check the camera grabbing status before making some register changes"));
	eCameraGrabbingStatus cgs;
	eErrorCode rv_sdoaq = ::SDOAQ_GetCameraGrabbingStatus(&cgs);
	if (ecNoError == rv_sdoaq)
	{
		if (cgsOnGrabbing == cgs)
		{
			rv_sdoaq = ::SDOAQ_SetCameraGrabbingStatus(cgsOffGrabbing);
			if (ecNoError != rv_sdoaq)
			{
				g_LogLine(_T("SDOAQ_SetCameraGrabbingStatus() returns error(%d)."), rv_sdoaq);
				return;
			}
		}

		CString sRegister, sValue;
		GetDlgItemText(IDC_EDIT_REGISTER, sRegister);
		GetDlgItemText(IDC_EDIT_REG_INT, sValue);
		eErrorCode rv_sdoaq = ::SDOAQ_SetCameraParameterInteger(CT2CA(sRegister), _ttoi(sValue));
		if (ecNoError != rv_sdoaq)
		{
			g_LogLine(_T("SDOAQ_SetCameraParameterInteger(%s:%s) returns error(%d)."), sRegister, sValue, rv_sdoaq);
		}

		rv_sdoaq = ::SDOAQ_SetCameraGrabbingStatus(cgs);
		if (ecNoError != rv_sdoaq)
		{
			g_LogLine(_T("SDOAQ_SetCameraGrabbingStatus() returns error(%d)."), rv_sdoaq);
		}
	}
	else
	{
		g_LogLine(_T("SDOAQ_GetCameraGrabbingStatus() returns error(%d)."), rv_sdoaq);
	}
}

//----------------------------------------------------------------------------
void CSdoaqCameraFrameCallbackDlg::OnSetDoubleRegister()
{
	CString sRegister, sValue;
	GetDlgItemText(IDC_EDIT_REGISTER, sRegister);
	GetDlgItemText(IDC_EDIT_REG_DOUBLE, sValue);
	eErrorCode rv_sdoaq = ::SDOAQ_SetCameraParameterDouble(CT2CA(sRegister), _ttof(sValue));
	if (ecNoError != rv_sdoaq)
	{
		g_LogLine(_T("SDOAQ_SetCameraParameterDouble(%s:%s) returns error(%d)."), sRegister, sValue, rv_sdoaq);
	}
}

//----------------------------------------------------------------------------
void CSdoaqCameraFrameCallbackDlg::OnSetBoolRegister()
{
	CString sRegister, sValue;
	GetDlgItemText(IDC_EDIT_REGISTER, sRegister);
	GetDlgItemText(IDC_EDIT_REG_BOOL, sValue);
	eErrorCode rv_sdoaq = ::SDOAQ_SetCameraParameterBool(CT2CA(sRegister), _ttoi(sValue));
	if (ecNoError != rv_sdoaq)
	{
		g_LogLine(_T("SDOAQ_SetCameraParameterBool(%s:%s) returns error(%d)."), sRegister, sValue, rv_sdoaq);
	}
}

//============================================================================
// CALLBACK FUNCTION:
//----------------------------------------------------------------------------
static void g_SDOAQ_InitDoneCallback(eErrorCode errorCode, char* pErrorMessage)
{
	if (errorCode == ecNoError)
	{
		g_LogLine(_T("Initialization OK!!!"));

		g_LogLine(_T("register SDOAQ low level authorization"));
		::SDOAQ_RegisterLowLevelAuthorization();

		if (theApp.m_pMainWnd)
		{
			theApp.m_pMainWnd->GetDlgItem(IDC_CHECK_FRAME_CALLBACK)->EnableWindow(TRUE);
			theApp.m_pMainWnd->GetDlgItem(IDC_SW_TRIGGER)->EnableWindow(TRUE);
			((CButton*)theApp.m_pMainWnd->GetDlgItem(IDC_TRIGGER_CAMERA_SOFTWARE))->SetCheck(TRUE);
			g_SDOAQ_SetCameraTriggerMode(ctmCameraSoftware);

			theApp.m_pMainWnd->GetDlgItem(IDC_EDIT_FOV_WIDTH)->EnableWindow(TRUE);
			theApp.m_pMainWnd->GetDlgItem(IDC_EDIT_FOV_HEIGHT)->EnableWindow(TRUE);
			theApp.m_pMainWnd->GetDlgItem(IDC_EDIT_FOV_OFFSETX)->EnableWindow(TRUE);
			theApp.m_pMainWnd->GetDlgItem(IDC_EDIT_FOV_OFFSETY)->EnableWindow(TRUE);
			theApp.m_pMainWnd->GetDlgItem(IDC_EDIT_EXPOSURE)->EnableWindow(TRUE);
			theApp.m_pMainWnd->GetDlgItem(IDC_EDIT_GAIN)->EnableWindow(TRUE);
			theApp.m_pMainWnd->GetDlgItem(IDC_EDIT_WB_R)->EnableWindow(TRUE);
			theApp.m_pMainWnd->GetDlgItem(IDC_EDIT_WB_G)->EnableWindow(TRUE);
			theApp.m_pMainWnd->GetDlgItem(IDC_EDIT_WB_B)->EnableWindow(TRUE);

			int nWidth, nHeight, nOffsetX, nOffsetY, nBinning;
			(void)::SDOAQ_GetCameraRoiParameter(&nWidth, &nHeight, &nOffsetX, &nOffsetY, &nBinning);
			theApp.m_pMainWnd->SetDlgItemText(IDC_EDIT_FOV_WIDTH, FString(_T("%d"), nWidth));
			theApp.m_pMainWnd->SetDlgItemText(IDC_EDIT_FOV_HEIGHT, FString(_T("%d"), nHeight));
			theApp.m_pMainWnd->SetDlgItemText(IDC_EDIT_FOV_OFFSETX, FString(_T("%d"), nOffsetX));
			theApp.m_pMainWnd->SetDlgItemText(IDC_EDIT_FOV_OFFSETY, FString(_T("%d"), nOffsetY));

			int nValue;
			(void)::SDOAQ_GetIntParameterValue(piCameraExposureTime, &nValue);
			theApp.m_pMainWnd->SetDlgItemText(IDC_EDIT_EXPOSURE, FString(_T("%d"), nValue));

			double dbValue;
			(void)::SDOAQ_GetDblParameterValue(piCameraGain, &dbValue);
			theApp.m_pMainWnd->SetDlgItemText(IDC_EDIT_GAIN, FString(_T("%.3lf"), dbValue));

			(void)::SDOAQ_GetDblParameterValue(piWhiteBalanceRed, &dbValue);
			theApp.m_pMainWnd->SetDlgItemText(IDC_EDIT_WB_R, FString(_T("%.3lf"), dbValue));
			(void)::SDOAQ_GetDblParameterValue(piWhiteBalanceGreen, &dbValue);
			theApp.m_pMainWnd->SetDlgItemText(IDC_EDIT_WB_G, FString(_T("%.3lf"), dbValue));
			(void)::SDOAQ_GetDblParameterValue(piWhiteBalanceBlue, &dbValue);
			theApp.m_pMainWnd->SetDlgItemText(IDC_EDIT_WB_B, FString(_T("%.3lf"), dbValue));
		}
	}
	else
	{
		g_LogLine(_T("g_SDOAQ_InitDoneCallback() returns error(%d:%s)."), errorCode, (LPCTSTR)CA2W(pErrorMessage));
	}
}

//----------------------------------------------------------------------------
static void g_SDOAQ_FrameCallback(eErrorCode errorCode, unsigned char* pBuffer, size_t BufferSize, FrameDescriptor* pFrameDescriptor)
{
	if (errorCode == ecNoError && pBuffer && BufferSize && pFrameDescriptor)
	{
		// RECEIVED VALID FRAME

		static int g_frame_id = 0;
		static int nums_skip_to_display = 0;
		static ULONGLONG g_tick_next = 0;

		++g_frame_id;
		if (g_tick_next < GetTickCount64())
		{
			char sz_frame_id[256];
			sprintf_s(sz_frame_id, sizeof sz_frame_id, "Received Frame = %d  (Displayed Frame = %d)", g_frame_id, g_frame_id - nums_skip_to_display);
			const WSIORV rv_wsio = ::WSUT_IV_AttachRawImgData_V2(g_hViewer
				, pFrameDescriptor->pixelsWidth, pFrameDescriptor->pixelsHeight, pFrameDescriptor->bytesLine, pFrameDescriptor->bytesPixel
				, pBuffer, (int)BufferSize, sz_frame_id);
			if (WSIORV_SUCCESS > rv_wsio)
			{
				g_LogLine(_T("WSUT_IV_AttachRawImgData_V2() returns error(%d)."), rv_wsio);
			}

			g_tick_next = GetTickCount64() + 50; // minimum 50 msec
		}
		else
		{
			++nums_skip_to_display;
		}
	}
}

//----------------------------------------------------------------------------
static void g_SDOAQ_SetCameraTriggerMode(eCameraTriggerMode ctm)
{
	LPCTSTR sz;
	switch (ctm)
	{
	// The three modes below are for backward compatibility and are not recommended for use.
	case ctmFreerun: sz = _T("Freerun"); break;
	case ctmSoftware: sz = _T("Software"); break;
	case ctmExternal: sz = _T("External"); break;
	// The six modes below may not work perfectly depending on the camera or grabber.
	case ctmCameraFreerun: sz = _T("Camera Freerun"); break;
	case ctmCameraSoftware: sz = _T("Camera Software"); break;
	case ctmCameraExternal: sz = _T("Camera External"); break;
	case ctmGrabberFreerun: sz = _T("Grabber Freerun"); break;
	case ctmGrabberSoftware: sz = _T("Grabber Software"); break;
	case ctmGrabberExternal: sz = _T("Grabber External"); break;
	default: sz = _T("invalid"); break;
	}

	g_LogLine(_T("set camera trigger mode -> %s"), sz);
	const eErrorCode rv_sdoaq = ::SDOAQ_SetCameraTriggerMode(ctm);
	if (ecNoError != rv_sdoaq)
	{
		g_LogLine(_T("SDOAQ_SetCameraTriggerMode() returns error(%d)."), rv_sdoaq);
	}
}
