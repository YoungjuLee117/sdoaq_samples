
// SdoaqMultiWsCameraFrameCallbackDlg.cpp : implementation file
//

#include "pch.h"
#include "framework.h"
#include "SdoaqMultiCameraFrameCallback.h"
#include "SdoaqMultiCameraFrameCallbackDlg.h"
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
#include "..\..\Include\SDOAQ\SDOAQ_MULTIWS.h"
#include "..\..\Include\SDOAQ\SDOAQ_LLAPI.h"
#include "..\..\Include\WSIO\WSIO_UTIL.h"


//----------------------------------------------------------------------------
const int NUMS_OF_WS = 2;
static WSIOVOID g_hViewer[NUMS_OF_WS];
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

CSdoaqMultiCameraFrameCallbackDlg::CSdoaqMultiCameraFrameCallbackDlg(CWnd* pParent /*=nullptr*/)
	: CDialogEx(IDD_SDOAQMULTICAMERAFRAMECALLBACK_DIALOG, pParent)
{
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
}

void CSdoaqMultiCameraFrameCallbackDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
}

//----------------------------------------------------------------------------
BEGIN_MESSAGE_MAP(CSdoaqMultiCameraFrameCallbackDlg, CDialogEx)
	ON_WM_CLOSE()
	ON_WM_SIZE()
	ON_COMMAND(IDC_SW_TRIGGER_WS1, &OnBnClickedSwTrigger_ws1)
	ON_BN_CLICKED(IDC_BTN_SET_FOV_WS1, OnSetFov_ws1)
	ON_BN_CLICKED(IDC_BTN_SET_EXPOSURE_WS1, OnSetExposureTime_ws1)
	ON_BN_CLICKED(IDC_BTN_SET_GAIN_WS1, OnSetGain_ws1)
	ON_BN_CLICKED(IDC_BTN_SET_WB_WS1, OnSetWhitebalance_ws1)

	ON_COMMAND(IDC_SW_TRIGGER_WS2, &OnBnClickedSwTrigger_ws2)
	ON_BN_CLICKED(IDC_BTN_SET_FOV_WS2, OnSetFov_ws2)
	ON_BN_CLICKED(IDC_BTN_SET_EXPOSURE_WS2, OnSetExposureTime_ws2)
	ON_BN_CLICKED(IDC_BTN_SET_GAIN_WS2, OnSetGain_ws2)
	ON_BN_CLICKED(IDC_BTN_SET_WB_WS2, OnSetWhitebalance_ws2)
END_MESSAGE_MAP()


//============================================================================
// WINDOWS MESSAGE HANDLER
//----------------------------------------------------------------------------
BOOL CSdoaqMultiCameraFrameCallbackDlg::OnInitDialog()
{
	CDialogEx::OnInitDialog();

	SetIcon(m_hIcon, TRUE);
	SetIcon(m_hIcon, FALSE);

	GetDlgItem(IDC_SW_TRIGGER_WS1)->EnableWindow(FALSE);
	GetDlgItem(IDC_EDIT_FOV_WIDTH_WS1)->EnableWindow(FALSE);
	GetDlgItem(IDC_EDIT_FOV_HEIGHT_WS1)->EnableWindow(FALSE);
	GetDlgItem(IDC_EDIT_FOV_OFFSETX_WS1)->EnableWindow(FALSE);
	GetDlgItem(IDC_EDIT_FOV_OFFSETY_WS1)->EnableWindow(FALSE);
	GetDlgItem(IDC_EDIT_EXPOSURE_WS1)->EnableWindow(FALSE);
	GetDlgItem(IDC_EDIT_GAIN_WS1)->EnableWindow(FALSE);
	GetDlgItem(IDC_EDIT_WB_R_WS1)->EnableWindow(FALSE);
	GetDlgItem(IDC_EDIT_WB_G_WS1)->EnableWindow(FALSE);
	GetDlgItem(IDC_EDIT_WB_B_WS1)->EnableWindow(FALSE);

	GetDlgItem(IDC_SW_TRIGGER_WS2)->EnableWindow(FALSE);
	GetDlgItem(IDC_EDIT_FOV_WIDTH_WS2)->EnableWindow(FALSE);
	GetDlgItem(IDC_EDIT_FOV_HEIGHT_WS2)->EnableWindow(FALSE);
	GetDlgItem(IDC_EDIT_FOV_OFFSETX_WS2)->EnableWindow(FALSE);
	GetDlgItem(IDC_EDIT_FOV_OFFSETY_WS2)->EnableWindow(FALSE);
	GetDlgItem(IDC_EDIT_EXPOSURE_WS2)->EnableWindow(FALSE);
	GetDlgItem(IDC_EDIT_GAIN_WS2)->EnableWindow(FALSE);
	GetDlgItem(IDC_EDIT_WB_R_WS2)->EnableWindow(FALSE);
	GetDlgItem(IDC_EDIT_WB_G_WS2)->EnableWindow(FALSE);
	GetDlgItem(IDC_EDIT_WB_B_WS2)->EnableWindow(FALSE);

	const int ver_major = ::SDOAQ_GetMajorVersion();
	const int ver_minor = ::SDOAQ_GetMinorVersion();
	const int ver_patch = ::SDOAQ_GetPatchVersion();
	g_LogLine(_T("SDOAQ DLL version is \"%d.%d.%d\""), ver_major, ver_minor, ver_patch);

	// register multiple wisescopes uses before initialization
	g_LogLine(_T("register SDOAQ Multiple WiseScope"));
	::SDOAQ_RegisterMultiWsApi();

	// set the cam files folder path
	::SDOAQ_SetCamfilePath(CT2A(FString(_T("%s\\..\\..\\Include\\SDOAQ\\CamFiles"), GetCurrentDir())));

	g_LogLine(_T("start initialization..."));
	const eErrorCode rv_sdoaq = ::SDOAQ_Initialize(NULL, NULL, g_SDOAQ_InitDoneCallback);
	if (ecNoError != rv_sdoaq)
	{
		g_LogLine(_T("SDOAQ_Initialize() returns error(%d)."), rv_sdoaq);
	}

	for (auto& each : g_hViewer)
	{
		const WSIORV rv_wsio = ::WSUT_IV_CreateImageViewer((WSIOCSTR)_T("VIEWER")
			, (WSIOVOID)(this->m_hWnd), &each, NULL
			, WSUTIVOPMODE_VISION | WSUTIVOPMODE_TOPTITLE | WSUTIVOPMODE_FRAMEOUTER);
		if (WSIORV_SUCCESS > rv_wsio)
		{
			g_LogLine(_T("WSUT_IV_CreateImageViewer() returns error(%d)."), rv_wsio);
		}
	}

	SendMessage(WM_SIZE); // invoke WSUT_IV_ShowWindow call with size.

	return TRUE;  // return TRUE  unless you set the focus to a control
}

//----------------------------------------------------------------------------
BOOL CSdoaqMultiCameraFrameCallbackDlg::PreTranslateMessage(MSG* pMsg)
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
void CSdoaqMultiCameraFrameCallbackDlg::OnClose()
{
	(void)::SDOAQ_Finalize();

	for (auto& each : g_hViewer)
	{
		(void)::WSUT_IV_DestroyImageViewer(each);
	}

	CDialogEx::OnClose();
}

//----------------------------------------------------------------------------
void CSdoaqMultiCameraFrameCallbackDlg::OnSize(UINT nType, int cx, int cy)
{
	CDialogEx::OnSize(nType, cx, cy);

	CWnd* p_wnd = GetDlgItem(IDC_VIEWER);
	if (p_wnd)
	{
		CRect rc_ws[2];
		p_wnd->GetWindowRect(rc_ws[0]);
		ScreenToClient(rc_ws[0]);
		rc_ws[1] = rc_ws[0];
		rc_ws[0].right = rc_ws[1].left = (rc_ws[0].left + rc_ws[0].right) / 2;

		for (size_t uid = 0; uid < NUMS_OF_WS; uid++)
		{
			WSIORV rv_wsio = ::WSUT_IV_ShowWindow(g_hViewer[uid], (WSIOINT)true, rc_ws[uid].left, rc_ws[uid].top, rc_ws[uid].right, rc_ws[uid].bottom);
			if (WSIORV_SUCCESS > rv_wsio)
			{
				g_LogLine(_T("Viewer[%d] WSUT_IV_ShowWindow() returns error(%d)."), uid, rv_wsio);
			}
		}
	}
	else
	{
		// before OnInitDialog()
	}
}

//----------------------------------------------------------------------------
void CSdoaqMultiCameraFrameCallbackDlg::UpdateWsSelection(int newWSId)
{
	if (m_cur_ws != newWSId)
	{
		m_cur_ws = newWSId;
		eErrorCode rv_sdoaq = ::SDOAQ_SelectMultiWs(newWSId);
		if (ecNoError != rv_sdoaq)
		{
			g_LogLine(_T("SDOAQ_SelectMultiWs(%d) returns error(%d)."), newWSId, rv_sdoaq);
		}
	}
}

//----------------------------------------------------------------------------
void CSdoaqMultiCameraFrameCallbackDlg::OnBnClickedSwTrigger_ws1()
{
	UpdateWsSelection(1);

	static int g_sw_trigger_no = 0;
	g_LogLine(_T("execute %dWS software trigger (%d)"), m_cur_ws, ++g_sw_trigger_no);
	const eErrorCode rv_sdoaq = ::SDOAQ_ExecCameraSoftwareTrigger();
	if (ecNoError != rv_sdoaq)
	{
		g_LogLine(_T("SDOAQ_ExecCameraSoftwareTrigger() returns error(%d)."), rv_sdoaq);
	}
}

//----------------------------------------------------------------------------
void CSdoaqMultiCameraFrameCallbackDlg::OnSetFov_ws1()
{
	UpdateWsSelection(1);

	CString sWidth, sHeight, sOffsetX, sOffsetY;
	GetDlgItemText(IDC_EDIT_FOV_WIDTH_WS1, sWidth);
	GetDlgItemText(IDC_EDIT_FOV_HEIGHT_WS1, sHeight);
	GetDlgItemText(IDC_EDIT_FOV_OFFSETX_WS1, sOffsetX);
	GetDlgItemText(IDC_EDIT_FOV_OFFSETY_WS1, sOffsetY);

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
					SetDlgItemText(IDC_EDIT_FOV_WIDTH_WS1, FString(_T("%d"), nWidth));
					SetDlgItemText(IDC_EDIT_FOV_HEIGHT_WS1, FString(_T("%d"), nHeight));
					SetDlgItemText(IDC_EDIT_FOV_OFFSETX_WS1, FString(_T("%d"), nOffsetX));
					SetDlgItemText(IDC_EDIT_FOV_OFFSETY_WS1, FString(_T("%d"), nOffsetY));
					g_LogLine(_T("set %dWS ROI (l:%d, t:%d, w:%d, h:%d)"), m_cur_ws, nOffsetX, nOffsetY, nWidth, nHeight);
				}
				else
				{
					g_LogLine(_T("SDOAQ_GetCameraRoiParameter() returns error(%d)."), rv_sdoaq);
				}
			}
			else
			{
				g_LogLine(_T("SDOAQ_SetCameraParameter() returns error(%d)."), rv_sdoaq);
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
void CSdoaqMultiCameraFrameCallbackDlg::OnSetExposureTime_ws1()
{
	UpdateWsSelection(1);

	CString sValue;
	GetDlgItemText(IDC_EDIT_EXPOSURE_WS1, sValue);

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
				g_LogLine(_T("set %dWS camera exposure time: %d"), m_cur_ws, nValue);
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
void CSdoaqMultiCameraFrameCallbackDlg::OnSetGain_ws1()
{
	UpdateWsSelection(1);

	CString sValue;
	GetDlgItemText(IDC_EDIT_GAIN_WS1, sValue);

	double dbMin, dbMax, dbValue;
	dbValue = _ttof(sValue);
	::SDOAQ_GetDblParameterRange(piCameraGain, &dbMin, &dbMax);

	if (dbMin <= dbValue && dbValue <= dbMax)
	{
		eErrorCode rv_sdoaq = ::SDOAQ_SetDblParameterValue(piCameraGain, dbValue);
		if (ecNoError == rv_sdoaq)
		{
			g_LogLine(_T("set %dWS camera gain: %.3lf"), m_cur_ws, dbValue);
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
void CSdoaqMultiCameraFrameCallbackDlg::OnSetWhitebalance_ws1()
{
	UpdateWsSelection(1);

	CString sRed, sGreen, sBlue;
	GetDlgItemText(IDC_EDIT_WB_R_WS1, sRed);
	GetDlgItemText(IDC_EDIT_WB_G_WS1, sGreen);
	GetDlgItemText(IDC_EDIT_WB_B_WS1, sBlue);

	double dbValue = _ttof(sRed);
	eErrorCode rv_sdoaq = ::SDOAQ_SetDblParameterValue(piWhiteBalanceRed, dbValue);
	if (ecNoError == rv_sdoaq)
	{
		g_LogLine(_T("set %dWS camera whitebalance red: %.3lf"), m_cur_ws, dbValue);
	}
	else
	{
		g_LogLine(_T("SDOAQ_SetDblParameterValue(piWhiteBalanceRed) returns error(%d)."), rv_sdoaq);
	}

	dbValue = _ttof(sGreen);
	rv_sdoaq = ::SDOAQ_SetDblParameterValue(piWhiteBalanceGreen, dbValue);
	if (ecNoError == rv_sdoaq)
	{
		g_LogLine(_T("set %dWS camera whitebalance green: %.3lf"), m_cur_ws, dbValue);
	}
	else
	{
		g_LogLine(_T("SDOAQ_SetDblParameterValue(piWhiteBalanceGreen) returns error(%d)."), rv_sdoaq);
	}

	dbValue = _ttof(sBlue);
	rv_sdoaq = ::SDOAQ_SetDblParameterValue(piWhiteBalanceBlue, dbValue);
	if (ecNoError == rv_sdoaq)
	{
		g_LogLine(_T("set %dWS camera whitebalance blue: %.3lf"), m_cur_ws, dbValue);
	}
	else
	{
		g_LogLine(_T("SDOAQ_SetDblParameterValue(piWhiteBalanceBlue) returns error(%d)."), rv_sdoaq);
	}
}

//----------------------------------------------------------------------------
void CSdoaqMultiCameraFrameCallbackDlg::OnBnClickedSwTrigger_ws2()
{
	UpdateWsSelection(2);

	static int g_sw_trigger_no = 0;
	g_LogLine(_T("execute %dWS software trigger (%d)"), m_cur_ws, ++g_sw_trigger_no);
	const eErrorCode rv_sdoaq = ::SDOAQ_ExecCameraSoftwareTrigger();
	if (ecNoError != rv_sdoaq)
	{
		g_LogLine(_T("SDOAQ_ExecCameraSoftwareTrigger() returns error(%d)."), rv_sdoaq);
	}
}

//----------------------------------------------------------------------------
void CSdoaqMultiCameraFrameCallbackDlg::OnSetFov_ws2()
{
	UpdateWsSelection(2);

	CString sWidth, sHeight, sOffsetX, sOffsetY;
	GetDlgItemText(IDC_EDIT_FOV_WIDTH_WS2, sWidth);
	GetDlgItemText(IDC_EDIT_FOV_HEIGHT_WS2, sHeight);
	GetDlgItemText(IDC_EDIT_FOV_OFFSETX_WS2, sOffsetX);
	GetDlgItemText(IDC_EDIT_FOV_OFFSETY_WS2, sOffsetY);

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
					SetDlgItemText(IDC_EDIT_FOV_WIDTH_WS2, FString(_T("%d"), nWidth));
					SetDlgItemText(IDC_EDIT_FOV_HEIGHT_WS2, FString(_T("%d"), nHeight));
					SetDlgItemText(IDC_EDIT_FOV_OFFSETX_WS2, FString(_T("%d"), nOffsetX));
					SetDlgItemText(IDC_EDIT_FOV_OFFSETY_WS2, FString(_T("%d"), nOffsetY));
					g_LogLine(_T("set %dWS ROI (l:%d, t:%d, w:%d, h:%d)"), m_cur_ws, nOffsetX, nOffsetY, nWidth, nHeight);
				}
				else
				{
					g_LogLine(_T("SDOAQ_GetCameraRoiParameter() returns error(%d)."), rv_sdoaq);
				}
			}
			else
			{
				g_LogLine(_T("SDOAQ_SetCameraParameter() returns error(%d)."), rv_sdoaq);
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
void CSdoaqMultiCameraFrameCallbackDlg::OnSetExposureTime_ws2()
{
	UpdateWsSelection(2);

	CString sValue;
	GetDlgItemText(IDC_EDIT_EXPOSURE_WS2, sValue);

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
				g_LogLine(_T("set %dWS camera exposure time: %d"), m_cur_ws, nValue);
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
void CSdoaqMultiCameraFrameCallbackDlg::OnSetGain_ws2()
{
	UpdateWsSelection(2);

	CString sValue;
	GetDlgItemText(IDC_EDIT_GAIN_WS2, sValue);

	double dbMin, dbMax, dbValue;
	dbValue = _ttof(sValue);
	::SDOAQ_GetDblParameterRange(piCameraGain, &dbMin, &dbMax);

	if (dbMin <= dbValue && dbValue <= dbMax)
	{
		eErrorCode rv_sdoaq = ::SDOAQ_SetDblParameterValue(piCameraGain, dbValue);
		if (ecNoError == rv_sdoaq)
		{
			g_LogLine(_T("set %dWS camera gain: %.3lf"), m_cur_ws, dbValue);
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
void CSdoaqMultiCameraFrameCallbackDlg::OnSetWhitebalance_ws2()
{
	UpdateWsSelection(2);

	CString sRed, sGreen, sBlue;
	GetDlgItemText(IDC_EDIT_WB_R_WS2, sRed);
	GetDlgItemText(IDC_EDIT_WB_G_WS2, sGreen);
	GetDlgItemText(IDC_EDIT_WB_B_WS2, sBlue);

	double dbValue = _ttof(sRed);
	eErrorCode rv_sdoaq = ::SDOAQ_SetDblParameterValue(piWhiteBalanceRed, dbValue);
	if (ecNoError == rv_sdoaq)
	{
		g_LogLine(_T("set %dWS camera whitebalance red: %.3lf"), m_cur_ws, dbValue);
	}
	else
	{
		g_LogLine(_T("SDOAQ_SetDblParameterValue(piWhiteBalanceRed) returns error(%d)."), rv_sdoaq);
	}

	dbValue = _ttof(sGreen);
	rv_sdoaq = ::SDOAQ_SetDblParameterValue(piWhiteBalanceGreen, dbValue);
	if (ecNoError == rv_sdoaq)
	{
		g_LogLine(_T("set %dWS camera whitebalance green: %.3lf"), m_cur_ws, dbValue);
	}
	else
	{
		g_LogLine(_T("SDOAQ_SetDblParameterValue(piWhiteBalanceGreen) returns error(%d)."), rv_sdoaq);
	}

	dbValue = _ttof(sBlue);
	rv_sdoaq = ::SDOAQ_SetDblParameterValue(piWhiteBalanceBlue, dbValue);
	if (ecNoError == rv_sdoaq)
	{
		g_LogLine(_T("set %dWS camera whitebalance blue: %.3lf"), m_cur_ws, dbValue);
	}
	else
	{
		g_LogLine(_T("SDOAQ_SetDblParameterValue(piWhiteBalanceBlue) returns error(%d)."), rv_sdoaq);
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

		// You can also register a separate callback function for each wisescope.
		g_LogLine(_T("register the same frame callback func for multi wisescopes"));
		eErrorCode rv_sdoaq = ::SDOAQ_SelectMultiWs(MULTI_WS_ALL);
		if (ecNoError != rv_sdoaq)
		{
			g_LogLine(_T("SDOAQ_SelectMultiWs(%d) returns error(%d)."), MULTI_WS_ALL, rv_sdoaq);
			return;
		}

		rv_sdoaq = ::SDOAQ_SetFrameCallback(g_SDOAQ_FrameCallback);
		if (ecNoError != rv_sdoaq)
		{
			g_LogLine(_T("SDOAQ_SetFrameCallback() returns error(%d)."), rv_sdoaq);
			return;
		}

		if (theApp.m_pMainWnd)
		{
			//----------------------------------------------------------------------------
			// wisescope 1
			rv_sdoaq = ::SDOAQ_SelectMultiWs(1);
			if (ecNoError != rv_sdoaq)
			{
				g_LogLine(_T("SDOAQ_SelectMultiWs(1) returns error(%d)."), rv_sdoaq);
				return;
			}

			theApp.m_pMainWnd->GetDlgItem(IDC_SW_TRIGGER_WS1)->EnableWindow(TRUE);
			g_SDOAQ_SetCameraTriggerMode(ctmCameraSoftware);

			theApp.m_pMainWnd->GetDlgItem(IDC_EDIT_FOV_WIDTH_WS1)->EnableWindow(TRUE);
			theApp.m_pMainWnd->GetDlgItem(IDC_EDIT_FOV_HEIGHT_WS1)->EnableWindow(TRUE);
			theApp.m_pMainWnd->GetDlgItem(IDC_EDIT_FOV_OFFSETX_WS1)->EnableWindow(TRUE);
			theApp.m_pMainWnd->GetDlgItem(IDC_EDIT_FOV_OFFSETY_WS1)->EnableWindow(TRUE);
			theApp.m_pMainWnd->GetDlgItem(IDC_EDIT_EXPOSURE_WS1)->EnableWindow(TRUE);
			theApp.m_pMainWnd->GetDlgItem(IDC_EDIT_GAIN_WS1)->EnableWindow(TRUE);
			theApp.m_pMainWnd->GetDlgItem(IDC_EDIT_WB_R_WS1)->EnableWindow(TRUE);
			theApp.m_pMainWnd->GetDlgItem(IDC_EDIT_WB_G_WS1)->EnableWindow(TRUE);
			theApp.m_pMainWnd->GetDlgItem(IDC_EDIT_WB_B_WS1)->EnableWindow(TRUE);

			int nWidth, nHeight, nOffsetX, nOffsetY, nBinning;
			(void)::SDOAQ_GetCameraRoiParameter(&nWidth, &nHeight, &nOffsetX, &nOffsetY, &nBinning);
			theApp.m_pMainWnd->SetDlgItemText(IDC_EDIT_FOV_WIDTH_WS1, FString(_T("%d"), nWidth));
			theApp.m_pMainWnd->SetDlgItemText(IDC_EDIT_FOV_HEIGHT_WS1, FString(_T("%d"), nHeight));
			theApp.m_pMainWnd->SetDlgItemText(IDC_EDIT_FOV_OFFSETX_WS1, FString(_T("%d"), nOffsetX));
			theApp.m_pMainWnd->SetDlgItemText(IDC_EDIT_FOV_OFFSETY_WS1, FString(_T("%d"), nOffsetY));

			int nValue;
			(void)::SDOAQ_GetIntParameterValue(piCameraExposureTime, &nValue);
			theApp.m_pMainWnd->SetDlgItemText(IDC_EDIT_EXPOSURE_WS1, FString(_T("%d"), nValue));

			double dbValue;
			(void)::SDOAQ_GetDblParameterValue(piCameraGain, &dbValue);
			theApp.m_pMainWnd->SetDlgItemText(IDC_EDIT_GAIN_WS1, FString(_T("%.3lf"), dbValue));

			(void)::SDOAQ_GetDblParameterValue(piWhiteBalanceRed, &dbValue);
			theApp.m_pMainWnd->SetDlgItemText(IDC_EDIT_WB_R_WS1, FString(_T("%.3lf"), dbValue));
			(void)::SDOAQ_GetDblParameterValue(piWhiteBalanceGreen, &dbValue);
			theApp.m_pMainWnd->SetDlgItemText(IDC_EDIT_WB_G_WS1, FString(_T("%.3lf"), dbValue));
			(void)::SDOAQ_GetDblParameterValue(piWhiteBalanceBlue, &dbValue);
			theApp.m_pMainWnd->SetDlgItemText(IDC_EDIT_WB_B_WS1, FString(_T("%.3lf"), dbValue));

			//----------------------------------------------------------------------------
			// wisescope 2
			rv_sdoaq = ::SDOAQ_SelectMultiWs(2);
			if (ecNoError != rv_sdoaq)
			{
				g_LogLine(_T("SDOAQ_SelectMultiWs(2) returns error(%d)."), rv_sdoaq);
				return;
			}

			theApp.m_pMainWnd->GetDlgItem(IDC_SW_TRIGGER_WS2)->EnableWindow(TRUE);
			g_SDOAQ_SetCameraTriggerMode(ctmCameraSoftware);

			theApp.m_pMainWnd->GetDlgItem(IDC_EDIT_FOV_WIDTH_WS2)->EnableWindow(TRUE);
			theApp.m_pMainWnd->GetDlgItem(IDC_EDIT_FOV_HEIGHT_WS2)->EnableWindow(TRUE);
			theApp.m_pMainWnd->GetDlgItem(IDC_EDIT_FOV_OFFSETX_WS2)->EnableWindow(TRUE);
			theApp.m_pMainWnd->GetDlgItem(IDC_EDIT_FOV_OFFSETY_WS2)->EnableWindow(TRUE);
			theApp.m_pMainWnd->GetDlgItem(IDC_EDIT_EXPOSURE_WS2)->EnableWindow(TRUE);
			theApp.m_pMainWnd->GetDlgItem(IDC_EDIT_GAIN_WS2)->EnableWindow(TRUE);
			theApp.m_pMainWnd->GetDlgItem(IDC_EDIT_WB_R_WS2)->EnableWindow(TRUE);
			theApp.m_pMainWnd->GetDlgItem(IDC_EDIT_WB_G_WS2)->EnableWindow(TRUE);
			theApp.m_pMainWnd->GetDlgItem(IDC_EDIT_WB_B_WS2)->EnableWindow(TRUE);

			(void)::SDOAQ_GetCameraRoiParameter(&nWidth, &nHeight, &nOffsetX, &nOffsetY, &nBinning);
			theApp.m_pMainWnd->SetDlgItemText(IDC_EDIT_FOV_WIDTH_WS2, FString(_T("%d"), nWidth));
			theApp.m_pMainWnd->SetDlgItemText(IDC_EDIT_FOV_HEIGHT_WS2, FString(_T("%d"), nHeight));
			theApp.m_pMainWnd->SetDlgItemText(IDC_EDIT_FOV_OFFSETX_WS2, FString(_T("%d"), nOffsetX));
			theApp.m_pMainWnd->SetDlgItemText(IDC_EDIT_FOV_OFFSETY_WS2, FString(_T("%d"), nOffsetY));

			(void)::SDOAQ_GetIntParameterValue(piCameraExposureTime, &nValue);
			theApp.m_pMainWnd->SetDlgItemText(IDC_EDIT_EXPOSURE_WS2, FString(_T("%d"), nValue));

			(void)::SDOAQ_GetDblParameterValue(piCameraGain, &dbValue);
			theApp.m_pMainWnd->SetDlgItemText(IDC_EDIT_GAIN_WS2, FString(_T("%.3lf"), dbValue));

			(void)::SDOAQ_GetDblParameterValue(piWhiteBalanceRed, &dbValue);
			theApp.m_pMainWnd->SetDlgItemText(IDC_EDIT_WB_R_WS2, FString(_T("%.3lf"), dbValue));
			(void)::SDOAQ_GetDblParameterValue(piWhiteBalanceGreen, &dbValue);
			theApp.m_pMainWnd->SetDlgItemText(IDC_EDIT_WB_G_WS2, FString(_T("%.3lf"), dbValue));
			(void)::SDOAQ_GetDblParameterValue(piWhiteBalanceBlue, &dbValue);
			theApp.m_pMainWnd->SetDlgItemText(IDC_EDIT_WB_B_WS2, FString(_T("%.3lf"), dbValue));
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

		int wsid = ::SDOAQ_GetCallbackMultiWs();

		static int g_frame_1ws_id = 0;
		static int g_frame_2ws_id = 0;

		if (wsid == 1) ++g_frame_1ws_id;
		else if (wsid == 2) ++g_frame_2ws_id;

		char sz_frame_id[256];
		sprintf_s(sz_frame_id, sizeof sz_frame_id, "Received Frame = %d", wsid == 1 ? g_frame_1ws_id : g_frame_2ws_id);
		const WSIORV rv_wsio = ::WSUT_IV_AttachRawImgData_V2(g_hViewer[wsid - 1]
			, pFrameDescriptor->pixelsWidth, pFrameDescriptor->pixelsHeight, pFrameDescriptor->bytesLine, pFrameDescriptor->bytesPixel
			, pBuffer, (int)BufferSize, sz_frame_id);
		if (WSIORV_SUCCESS > rv_wsio)
		{
			g_LogLine(_T("WSUT_IV_AttachRawImgData_V2() returns error(%d)."), rv_wsio);
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
