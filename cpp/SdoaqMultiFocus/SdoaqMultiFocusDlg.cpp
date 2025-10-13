
// SdoaqMultiFocusDlg.cpp : implementation file
//

#include "pch.h"
#include "framework.h"
#include "SdoaqMultiFocus.h"
#include "SdoaqMultiFocusDlg.h"
#include "afxdialogex.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


//----------------------------------------------------------------------------
static WSIOVOID g_hViewer = NULL;
//----------------------------------------------------------------------------
static void g_SDOAQ_InitDoneCallback(eErrorCode errorCode, char* pErrorMessage);
static void g_PlayMFCallbackEx(eErrorCode errorCode, int lastFilledRingBufferEntry, void* callbackUserData, int countRects, int* pRectIdArray, int* pRectStepArray);
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
//============================================================================

CSdoaqMultiFocusDlg::CSdoaqMultiFocusDlg(CWnd* pParent /*=nullptr*/)
	: CDialogEx(IDD_SDOAQMULTIFOCUS_DIALOG, pParent)
{
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
}

void CSdoaqMultiFocusDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
}

//----------------------------------------------------------------------------
BEGIN_MESSAGE_MAP(CSdoaqMultiFocusDlg, CDialogEx)
	ON_WM_CLOSE()
	ON_WM_SIZE()
	ON_MESSAGE(EUM_INITDONE, OnInitDone)
	ON_MESSAGE(EUM_RECEIVE_MF, OnReceiveMF)
	ON_BN_CLICKED(IDC_ADD_SCRIPT, OnSdoaqAddMFScript)
	ON_BN_CLICKED(IDC_SET_BLANK_STEP, OnSdoaqSetOutsideFocusStep)
	ON_BN_CLICKED(IDC_PLAY_MF, OnSdoaqPlayMF)
	ON_BN_CLICKED(IDC_UPDATE_SCRIPT, OnSdoaqUpdateScript)
	ON_BN_CLICKED(IDC_CLEAR_SCRIPT, OnSdoaqClearScript)
	ON_BN_CLICKED(IDC_STOP_MF, OnSdoaqStopMF)
END_MESSAGE_MAP()


//============================================================================
// WINDOWS MESSAGE HANDLER
//----------------------------------------------------------------------------
BOOL CSdoaqMultiFocusDlg::OnInitDialog()
{
	CDialogEx::OnInitDialog();

	// Set the icon for this dialog.  The framework does this automatically
	//  when the application's main window is not a dialog
	SetIcon(m_hIcon, TRUE);			// Set big icon
	SetIcon(m_hIcon, FALSE);		// Set small icon

	SetDlgItemText(IDC_STATIC_GUIDE,
		_T("========================================================================\r\n"
			"* This is one of several ways to implement multi-focus.\r\n"
			"* You can freely IMPLEMENT your OWN Multi-Focus function.\r\n"
			"========================================================================\r\n"));

	// set the cam files folder path
	::SDOAQ_SetCamfilePath(FStringA("%s\\..\\..\\Include\\SDOAQ\\CamFiles", (CStringA)GetCurrentDir()));

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
BOOL CSdoaqMultiFocusDlg::PreTranslateMessage(MSG* pMsg)
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
void CSdoaqMultiFocusDlg::OnClose()
{
	(void)::SDOAQ_Finalize();
	(void)::WSUT_IV_DestroyImageViewer(g_hViewer);

	CDialogEx::OnClose();
}

//----------------------------------------------------------------------------
void CSdoaqMultiFocusDlg::OnSize(UINT nType, int cx, int cy)
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
LRESULT CSdoaqMultiFocusDlg::OnInitDone(WPARAM wErrorCode, LPARAM lpMessage)
{
	CString* pMessage = (CString*)lpMessage;

	if (ecNoError == wErrorCode)
	{
		g_LogLine(_T("InitDoneCallback() %s"), pMessage ? *pMessage : _T(""));

		const int ver_major = ::SDOAQ_GetMajorVersion();
		const int ver_minor = ::SDOAQ_GetMinorVersion();
		const int ver_patch = ::SDOAQ_GetPatchVersion();
		g_LogLine(_T("sdoaq dll version is \"%d.%d.%d\""), ver_major, ver_minor, ver_patch);

		SET.m_nColorByte = IsMonoCameraInstalled() ? MONOBYTES : COLORBYTES;

		//----------------------------------------------------------------------------
		// ROI: image size to capture
		//----------------------------------------------------------------------------
		int nWidth, nHeight, nDummy;
		eErrorCode rv1 = ::SDOAQ_GetIntParameterRange(piCameraFullFrameSizeX, &nDummy, &nWidth);
		eErrorCode rv2 = ::SDOAQ_GetIntParameterRange(piCameraFullFrameSizeY, &nDummy, &nHeight);
		if (ecNoError == rv1 && ecNoError == rv2)
		{
			AcquisitionFixedParametersEx AFP;
			AFP.cameraRoiTop = 0;
			AFP.cameraRoiLeft = 0;
			AFP.cameraRoiWidth = nWidth;
			AFP.cameraRoiHeight = nHeight;
			AFP.cameraBinning = 1;
			if (!SET.rb.active)
			{
				SET.afp = AFP;
			}

			//----------------------------------------------------------------------------
			// FOCUS SET: scan image scan range
			//----------------------------------------------------------------------------
			m_vFocusSet.clear();
			int nLowFocus, nHighFocus;
			eErrorCode rv_sdoaq = ::SDOAQ_GetIntParameterRange(piFocusPosition, &nLowFocus, &nHighFocus);
			if (ecNoError == rv_sdoaq)
			{
				// nLowFocus: Low dof of the image to be scanned.
				// nHighFocus: High dof of the image to be scanned.
				// nInterval: Adjust the gap between images to be scanned. If you narrow the gap, you can get a more accurate image.
				int nInterval = 32;
				for (int nFocus = nLowFocus; nFocus <= nHighFocus; nFocus += nInterval)
				{
					m_vFocusSet.push_back(nFocus);
				}

				SetDlgItemText(IDC_STATIC_SCRIPT, _T("Script (id, func, focus, left, top, width, height)"
					"\r   * id (unique number)"
					"\r   * func (1: auto-focus, 2: fixed-focus)"
					"\r   * focus (focus step for fixed-focus roi)"
					"\r   * left,top,width,height (multi-foucs roi)"));
				SetDlgItemText(IDC_EDIT_SCRIPT, _T("1,1,160,1700,700,256,256"));
				OnSdoaqAddMFScript();

				SetDlgItemText(IDC_EDIT_BLANK_STEP, _T("160"));
				OnSdoaqSetOutsideFocusStep();
			}
			else
			{
				g_LogLine(_T("SDOAQ_GetIntParameterRange(piFocusPosition) returns error(%d)."), rv_sdoaq);
			}
		}
		else
		{
			g_LogLine(_T("SDOAQ_GetIntParameterRange(piCameraFullFrameSizeX) returns error(%d)."), rv1);
			g_LogLine(_T("SDOAQ_GetIntParameterRange(piCameraFullFrameSizeY) returns error(%d)."), rv2);
		}
	}
	else
	{
		g_LogLine(_T("InitDoneCallback() returns error(%d:%s)."), wErrorCode, pMessage ? *pMessage : _T(""));
	}

	if (pMessage)
	{
		delete pMessage;
	}

	return 0;
}

//------------------------------------------------------------------------------------------------
LRESULT CSdoaqMultiFocusDlg::OnReceiveMF(WPARAM wErrorCode, LPARAM lMsgParaReceiveMf)
{
	tMsgParaReceiveMf ParaMF;
	RetrievePointerBlock(ParaMF, lMsgParaReceiveMf);

	if (ecNoError != wErrorCode)
	{
		g_LogLine(_T("SDOAQ_PlayMfCallback() returns error(%d)."), (int)wErrorCode);
	}
	else if (SET.rb.active)
	{
		auto vRemovedMsg = UpdateLastMessage(m_hWnd, EUM_RECEIVE_MF, wErrorCode, lMsgParaReceiveMf);
		for (auto& each_msg : vRemovedMsg)
		{
			RetrievePointerBlock(ParaMF, each_msg.lParam);
		}

		//CString sMultiRectInfo;
		//for (auto& each : ParaMF.vRectInfo)
		//{
		//	sMultiRectInfo += FString(_T("Rect[%d]: %d step / "), each.id, each.step);
		//}
		//g_LogLine(_T("MF result: %s"), sMultiRectInfo);

		const int base_order = (ParaMF.lastFilledRingBufferEntry % (int)SET.rb.numsBuf);
		++m_nContiMF;

		ImageViewer("MF", m_nContiMF, SET, (BYTE*)SET.rb.ppBuf[base_order + 0]);
	}

	return 0;
}

//----------------------------------------------------------------------------
void CSdoaqMultiFocusDlg::OnSdoaqAddMFScript()
{
	CString sScript;
	GetDlgItemText(IDC_EDIT_SCRIPT, sScript);

	CString sId, sFunc, sFocus, sLeft, sTop, sWidth, sHeight;
	AfxExtractSubString(sId, sScript, 0, ',');
	AfxExtractSubString(sFunc, sScript, 1, ',');
	AfxExtractSubString(sFocus, sScript, 2, ',');
	AfxExtractSubString(sLeft, sScript, 3, ',');
	AfxExtractSubString(sTop, sScript, 4, ',');
	AfxExtractSubString(sWidth, sScript, 5, ',');
	AfxExtractSubString(sHeight, sScript, 6, ',');

	auto nFuncCode = _ttoi(sFunc);
	if (!(1 == nFuncCode || 2 == nFuncCode))
	{
		g_LogLine(_T("func code for multi-focus must be 1 or 2."));
		return;
	}

	CRect recMF(_ttoi(sLeft), _ttoi(sTop), _ttoi(sLeft) + _ttoi(sWidth), _ttoi(sTop) + _ttoi(sHeight));

	/*	MF script format
	Number of MR MULTI = 4 \n
		MR MULTI 0 = {1,2,23,1159,400,1445,675} \n
		MR MULTI 1 = {2,1,23,879,707,1010,945} \n
		MR MULTI 2 = {3,1,23,697,1041,927,1385} \n
		MR MULTI 3 = {4,2,23,183,408,578,971}	*/


	CString sNew = FString(_T("\r\nMR MULTI %d = {%s, %s, %s, %d, %d, %d, %d}")
		, m_numsMR++, sId, sFunc, sFocus, recMF.left, recMF.top, recMF.right, recMF.bottom);

	g_LogLine(_T("added script: %s"), sNew);

	m_sMultiAreas += (CStringA)sNew;
}

//----------------------------------------------------------------------------
void CSdoaqMultiFocusDlg::OnSdoaqSetOutsideFocusStep()
{
	CString sValue;
	GetDlgItemText(IDC_EDIT_BLANK_STEP, sValue);

	int nMin, nMax;
	eErrorCode rv_sdoaq = ::SDOAQ_GetIntParameterRange(piSingleFocus, &nMin, &nMax);
	if (ecNoError == rv_sdoaq)
	{
		int nValue = _ttoi(sValue);
		if (nValue >= nMin && nValue <= nMax)
		{
			(void)::SDOAQ_SetIntParameterValue(piSingleFocus, nValue);
			g_LogLine(_T("set the outside focus value to %d."), nValue);
		}
		else
		{
			g_LogLine(_T("focus value is out of range[%d ~ %d]"), nMin, nMax);
		}
	}
	else
	{
		g_LogLine(_T("SDOAQ_GetIntParameterRange(piSingleFocus) returns error(%d)."), rv_sdoaq);
	}
}

//----------------------------------------------------------------------------
void CSdoaqMultiFocusDlg::OnSdoaqPlayMF()
{
	if (SET.rb.active)
	{
		return;
	}

	auto& AFP = SET.afp;
	auto& FOCUS = SET.focus;

	FOCUS.numsFocus = m_vFocusSet.size();
	FOCUS.vFocusSet.resize(FOCUS.numsFocus);
	copy(m_vFocusSet.begin(), m_vFocusSet.end(), FOCUS.vFocusSet.begin());

	int* pPositions = new int[FOCUS.numsFocus];
	for (int pos = 0; pos < FOCUS.numsFocus; pos++)
	{
		pPositions[pos] = m_vFocusSet[pos];
	}

	if (SET.rb.ppBuf)
	{
		SET.ClearBuffer();
	}

	SET.rb.numsBuf = m_nRingBufferSize;
	SET.rb.ppBuf = (void**)new unsigned char*[SET.rb.numsBuf];
	SET.rb.pSizes = new size_t[SET.rb.numsBuf];

	for (size_t uidx = 0; uidx < SET.rb.numsBuf;)
	{
		SET.rb.ppBuf[uidx] = (void*)new unsigned char[SET.ImgSize()];
		SET.rb.pSizes[uidx] = SET.ImgSize();
		uidx++;
	}

	AFP.callbackUserData = (void*)::GetTickCount64();
	eErrorCode rv_sdoaq = ::SDOAQ_PlayMFEx(
		&AFP,
		g_PlayMFCallbackEx,
		pPositions, (int)FOCUS.numsFocus,
		GetFunctionScript(),
		m_nRingBufferSize,
		SET.rb.ppBuf,
		SET.rb.pSizes
	);
	if (ecNoError == rv_sdoaq)
	{
		SET.rb.active = true;
	}
	else
	{
		g_LogLine(_T("SDOAQ_PlayMFEx() returns error(%d)."), rv_sdoaq);
	}

	delete[] pPositions;
}

//----------------------------------------------------------------------------
void CSdoaqMultiFocusDlg::OnSdoaqUpdateScript()
{
	g_LogLine(_T("UpdatePlayMF() script = %s"), (CString)GetFunctionScript());

	const eErrorCode rv_sdoaq = ::SDOAQ_UpdatePlayMF(GetFunctionScript());
	if (ecNoError != rv_sdoaq)
	{
		g_LogLine(_T("SDOAQ_UpdatePlayMF() returns error(%d)."), rv_sdoaq);
	}
}

//----------------------------------------------------------------------------
void CSdoaqMultiFocusDlg::OnSdoaqClearScript()
{
	m_numsMR = 0;
	m_sMultiAreas = "";

	g_LogLine(_T("UpdatePlayMF() script = %s"), (CString)GetFunctionScript());

	const eErrorCode rv_sdoaq = ::SDOAQ_UpdatePlayMF(GetFunctionScript());
	if (ecNoError != rv_sdoaq)
	{
		g_LogLine(_T("SDOAQ_UpdatePlayMF() returns error(%d)."), rv_sdoaq);
	}
}

//----------------------------------------------------------------------------
void CSdoaqMultiFocusDlg::OnSdoaqStopMF()
{
	SET.rb.active = false;

	const eErrorCode rv_sdoaq = ::SDOAQ_StopMF();
	if (ecNoError != rv_sdoaq)
	{
		g_LogLine(_T("SDOAQ_StopMF() returns error(%d)."), rv_sdoaq);
	}

	SET.ClearBuffer();
}

//----------------------------------------------------------------------------
void CSdoaqMultiFocusDlg::ImageViewer(const char* title, int title_no
	, const tTestSet& SET, void* data)
{
	ImageViewer(title, title_no, SET.afp.cameraRoiWidth, SET.afp.cameraRoiHeight, SET.m_nColorByte, data);
}

//----------------------------------------------------------------------------
void CSdoaqMultiFocusDlg::ImageViewer(const char* title, int title_no
	, int width, int height, int colorbytes, void* data)
{
	WSIOCHAR full_title[256] = { 0 };
	if (title)
	{
		sprintf_s(full_title, sizeof full_title, "%s %d", title, title_no);
	}

	const unsigned size = (data ? width * height * colorbytes : 0);
	if (WSIORV_SUCCESS > ::WSUT_IV_AttachRawImgData_V2(g_hViewer
		, width, height, width*colorbytes, colorbytes, data, size, full_title))
	{
		WSIOCHAR sLastError[4 * 1024];
		::WSIO_LastErrorString(sLastError, sizeof sLastError);
		g_LogLine(_T("WSIO returns error(%s)."), (CString)sLastError);
	}
}

//============================================================================
// CALLBACK FUNCTION:
//----------------------------------------------------------------------------
static void g_SDOAQ_InitDoneCallback(eErrorCode errorCode, char* pErrorMessage)
{
	theApp.m_pMainWnd->PostMessageW(EUM_INITDONE, (WPARAM)errorCode, (LPARAM)NewWString(pErrorMessage));
}

//----------------------------------------------------------------------------
static void g_PlayMFCallbackEx(eErrorCode errorCode, int lastFilledRingBufferEntry
	, void* callbackUserData, int countRects, int* pRectIdArray, int* pRectStepArray)
{
	if (theApp.m_pMainWnd)
	{
		auto pcPara = new tMsgParaReceiveMf;
		pcPara->lastFilledRingBufferEntry = lastFilledRingBufferEntry;
		for (int idx = 0; idx < countRects; idx++)
		{
			tMsgParaReceiveMf::t_rec arec;
			arec.id = *pRectIdArray++;
			arec.step = *pRectStepArray++;
			pcPara->vRectInfo.push_back(arec);
		}
		theApp.m_pMainWnd->PostMessageW(EUM_RECEIVE_MF, (WPARAM)errorCode, (LPARAM)pcPara);

		static void* g_prev = NULL;
		if (g_prev != callbackUserData)
		{
			g_prev = callbackUserData;
			g_LogLine(_T("MF callback 0x%I64X"), (unsigned long long)callbackUserData);
		}
	}
}
