
// SdoaqAutoFocusDlg.cpp : implementation file
//

#include "pch.h"
#include "framework.h"
#include "SdoaqAutoFocus.h"
#include "SdoaqAutoFocusDlg.h"
#include "afxdialogex.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


//----------------------------------------------------------------------------
static WSIOVOID g_hViewer = NULL;
//----------------------------------------------------------------------------
static void g_SDOAQ_InitDoneCallback(eErrorCode errorCode, char* pErrorMessage);
static void g_PlayAFCallback(eErrorCode errorCode, int lastFilledRingBufferEntry, double dbBestFocusStep, double dbBestScore, double dbMatchedFocusStep);
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

CSdoaqAutoFocusDlg::CSdoaqAutoFocusDlg(CWnd* pParent /*=nullptr*/)
	: CDialogEx(IDD_SDOAQAUTOFOCUS_DIALOG, pParent)
{
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
}

void CSdoaqAutoFocusDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
}

//----------------------------------------------------------------------------
BEGIN_MESSAGE_MAP(CSdoaqAutoFocusDlg, CDialogEx)
	ON_WM_CLOSE()
	ON_WM_SIZE()
	ON_MESSAGE(EUM_INITDONE, OnInitDone)
	ON_MESSAGE(EUM_RECEIVE_AF, OnReceiveAF)
	ON_BN_CLICKED(IDC_SET_AFROI, OnSdoaqSetAFRoi)
	ON_BN_CLICKED(IDC_ACQ_AF, OnSdoaqSingleShotAF)
	ON_BN_CLICKED(IDC_CONTI_AF, OnSdoaqPlayAF)
	ON_BN_CLICKED(IDC_STOP_AF, OnSdoaqStopAF)
END_MESSAGE_MAP()


//============================================================================
// WINDOWS MESSAGE HANDLER
//----------------------------------------------------------------------------
BOOL CSdoaqAutoFocusDlg::OnInitDialog()
{
	CDialogEx::OnInitDialog();

	SetIcon(m_hIcon, TRUE);
	SetIcon(m_hIcon, FALSE);

	g_LogLine(_T("================================================"));
	g_LogLine(_T(" SDOAQ Auto Focus Sample"));
	g_LogLine(_T("================================================"));
	
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
BOOL CSdoaqAutoFocusDlg::PreTranslateMessage(MSG* pMsg)
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
void CSdoaqAutoFocusDlg::OnClose()
{
	(void)::SDOAQ_Finalize();
	(void)::WSUT_IV_DestroyImageViewer(g_hViewer);

	CDialogEx::OnClose();
}

//----------------------------------------------------------------------------
void CSdoaqAutoFocusDlg::OnSize(UINT nType, int cx, int cy)
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
LRESULT CSdoaqAutoFocusDlg::OnInitDone(WPARAM wErrorCode, LPARAM lpMessage)
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
		eErrorCode rv;
		rv = ::SDOAQ_GetIntParameterRange(piCameraFullFrameSizeX, &nDummy, &nWidth);
		rv = ::SDOAQ_GetIntParameterRange(piCameraFullFrameSizeY, &nDummy, &nHeight);

		AcquisitionFixedParameters AFP;
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
		rv = ::SDOAQ_GetIntParameterRange(piFocusPosition, &nLowFocus, &nHighFocus);

		// nLowFocus: Low dof of the image to be scanned.
		// nHighFocus: High dof of the image to be scanned.
		// nInterval: Adjust the gap between images to be scanned. If you narrow the gap, you can get a more accurate image.
		int nInterval = 10;
		for (int nFocus = nLowFocus; nFocus <= nHighFocus; nFocus += nInterval)
		{
			m_vFocusSet.push_back(nFocus);
		}

		//----------------------------------------------------------------------------
		// SET AUTO-FOCUS AREA: left, top, width, height
		//----------------------------------------------------------------------------
		SetDlgItemText(IDC_EDIT_AFROI, _T("956,479,128,128"));
		OnSdoaqSetAFRoi();
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

//----------------------------------------------------------------------------
LRESULT CSdoaqAutoFocusDlg::OnReceiveAF(WPARAM wErrorCode, LPARAM lMsgParaReceiveAf)
{
	tMsgParaReceiveAf ParaAF;
	RetrievePointerBlock(ParaAF, lMsgParaReceiveAf);

	if (ecNoError != wErrorCode)
	{
		g_LogLine(_T("SDOAQ_PlayAfCallback() returns error(%d)."), (int)wErrorCode);
	}
	else if (SET.rb.active)
	{
		auto vRemovedMsg = UpdateLastMessage(m_hWnd, EUM_RECEIVE_AF, wErrorCode, lMsgParaReceiveAf);
		for (auto& each_msg : vRemovedMsg)
		{
			RetrievePointerBlock(ParaAF, each_msg.lParam);
		}

		const int base_order = (ParaAF.lastFilledRingBufferEntry % (int)SET.rb.numsBuf);
		++m_nContiAF;

		ImageViewer("AF", m_nContiAF, SET, (BYTE*)SET.rb.ppBuf[base_order + 0]);

		g_LogLine(_T(">> best focus: %d, best focus score: %.4lf, current focus: %d"), (int)(round(ParaAF.dbBestFocusStep)), ParaAF.dbBestScore, (int)ParaAF.dbMatchedFocusStep);
	}

	return 0;
}

//----------------------------------------------------------------------------
void CSdoaqAutoFocusDlg::OnSdoaqSetAFRoi()
{
	CString sROI;
	GetDlgItemText(IDC_EDIT_AFROI, sROI);

	CString sLeft, sTop, sWidth, sHeight;
	AfxExtractSubString(sLeft, sROI, 0, ',');
	AfxExtractSubString(sTop, sROI, 1, ',');
	AfxExtractSubString(sWidth, sROI, 2, ',');
	AfxExtractSubString(sHeight, sROI, 3, ',');

	CRect recAF(_ttoi(sLeft), _ttoi(sTop), _ttoi(sLeft) + _ttoi(sWidth), _ttoi(sTop) + _ttoi(sHeight));
	const auto rv1 = ::SDOAQ_SetIntParameterValue(piFocusLeftTop, ((recAF.left & 0x0000FFFF) << 16) | (recAF.top & 0x0000FFFF) << 0);
	const auto rv2 = ::SDOAQ_SetIntParameterValue(piFocusRightBottom, ((recAF.right & 0x0000FFFF) << 16) | (recAF.bottom & 0x0000FFFF) << 0);
	if (rv1 != ecNoError)
	{
		g_LogLine(_T("Set AF roi (piFocusLeftTop) returns error(%d)."), rv1);
	}
	if (rv2 != ecNoError)
	{
		g_LogLine(_T("Set AF roi (piFocusRightBottom) returns error(%d)."), rv2);
	}
}

//----------------------------------------------------------------------------
void CSdoaqAutoFocusDlg::OnSdoaqSingleShotAF()
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

	size_t AFImageBufferSize = SET.ImgSize();
	unsigned char* pAFImageBuffer = new unsigned char[AFImageBufferSize];
	double dbBestFocusStep;
	double dbBestScore;
	double dbMatchedFocusStep;

	eErrorCode rv = ::SDOAQ_SingleShotAF_Ex(
		&AFP,
		pPositions, (int)FOCUS.numsFocus,
		pAFImageBuffer,
		AFImageBufferSize,
		&dbBestFocusStep,
		&dbBestScore,
		&dbMatchedFocusStep
	);
	if (ecNoError == rv)
	{
		++m_nContiAF;

		if (pAFImageBuffer && AFImageBufferSize)
		{
			ImageViewer("AF", m_nContiAF, SET, pAFImageBuffer);

			g_LogLine(_T(">> best focus: %d, best focus score: %.4lf, current focus: %d"), (int)(round(dbBestFocusStep)), dbBestScore, (int)dbMatchedFocusStep);
		}
		else
		{
			ImageViewer("AF", m_nContiAF);
		}
	}
	else
	{
		g_LogLine(_T("SDOAQ_SingleShotAF_Ex() returns error(%d)."), rv);
	}

	delete[] pAFImageBuffer;
	delete[] pPositions;
}

//----------------------------------------------------------------------------
void CSdoaqAutoFocusDlg::OnSdoaqPlayAF()
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

	eErrorCode rv = ::SDOAQ_PlayAF_Ex(
		&AFP,
		g_PlayAFCallback,
		pPositions, (int)FOCUS.numsFocus,
		m_nRingBufferSize,
		SET.rb.ppBuf,
		SET.rb.pSizes
	);
	if (ecNoError == rv)
	{
		SET.rb.active = true;
	}
	else
	{
		g_LogLine(_T("SDOAQ_PlayAF_Ex() returns error(%d)."), rv);
	}

	delete[] pPositions;
}

//----------------------------------------------------------------------------
void CSdoaqAutoFocusDlg::OnSdoaqStopAF()
{
	SET.rb.active = false;
	(void)::SDOAQ_StopAF();

	SET.ClearBuffer();
}

//----------------------------------------------------------------------------
void CSdoaqAutoFocusDlg::ImageViewer(const char* title, int title_no, const tTestSet& SET, void* data)
{
	ImageViewer(title, title_no, SET.afp.cameraRoiWidth, SET.afp.cameraRoiHeight, SET.m_nColorByte, data);
}

//----------------------------------------------------------------------------
void CSdoaqAutoFocusDlg::ImageViewer(const char* title, int title_no, int width, int height, int colorbytes, void* data)
{
	WSIOCHAR full_title[256] = { 0 };
	if (title)
	{
		sprintf_s(full_title, sizeof full_title, "%s %d", title, title_no);
	}

	const unsigned size = (data ? width * height * colorbytes : 0);
	if (WSIORV_SUCCESS > ::WSUT_IV_AttachRawImgData_V2(g_hViewer, width, height, width*colorbytes, colorbytes, data, size, full_title))
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
static void g_PlayAFCallback(eErrorCode errorCode, int lastFilledRingBufferEntry, double dbBestFocusStep, double dbBestScore, double dbMatchedFocusStep)
{
	if (theApp.m_pMainWnd)
	{
		auto pcPara = new tMsgParaReceiveAf;
		pcPara->lastFilledRingBufferEntry = lastFilledRingBufferEntry;
		pcPara->dbBestFocusStep = dbBestFocusStep;
		pcPara->dbBestScore = dbBestScore;
		pcPara->dbMatchedFocusStep = dbMatchedFocusStep;
		theApp.m_pMainWnd->PostMessageW(EUM_RECEIVE_AF, (WPARAM)errorCode, (LPARAM)pcPara);
	}
}
