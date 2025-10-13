
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
static void g_PlayAFCallbackEx(eErrorCode errorCode, int lastFilledRingBufferEntry, void* callbackUserData, double dbBestFocusStep, double dbBestScore, double dbMatchedFocusStep);
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
	ON_BN_CLICKED(IDC_SET_SMM, OnSdoaqSetSharpnessMeasureMethod)
	ON_BN_CLICKED(IDC_SET_RM, OnSdoaqSetResamplingMethod)
	ON_BN_CLICKED(IDC_SET_SM, OnSdoaqSetStabilityMethod)
	ON_BN_CLICKED(IDC_SET_SDC, OnSdoaqSetStabilityDebounceCount)
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
				int nInterval = 10;
				for (int nFocus = nLowFocus; nFocus <= nHighFocus; nFocus += nInterval)
				{
					m_vFocusSet.push_back(nFocus);
				}

				//----------------------------------------------------------------------------
				// SET AUTO-FOCUS AREA: left, top, width, height
				//----------------------------------------------------------------------------
				SetDlgItemText(IDC_EDIT_AFROI, FString(_T("%d,%d,128,128"), nWidth / 2, nHeight / 2));
				OnSdoaqSetAFRoi();

				SetDlgItemText(IDC_EDIT_SMM, _T("0"));
				OnSdoaqSetSharpnessMeasureMethod();

				SetDlgItemText(IDC_EDIT_RM, _T("1"));
				OnSdoaqSetResamplingMethod();

				SetDlgItemText(IDC_EDIT_SM, _T("1"));
				OnSdoaqSetStabilityMethod();

				SetDlgItemText(IDC_EDIT_SDC, _T("4"));
				OnSdoaqSetStabilityDebounceCount();
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

		g_LogLine(_T(">> best focus: %.4lf, best focus score: %.4lf, current focus: %d"), ParaAF.dbBestFocusStep, ParaAF.dbBestScore, (int)ParaAF.dbMatchedFocusStep);
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
	eErrorCode rv_sdoaq = SetSdoaqFocusRect(recAF);
	if (ecNoError != rv_sdoaq)
	{
		g_LogLine(_T("SDOAQ_SetIntParameterValue[ParamID-%d,%d] returns error(%d)."), piFocusLeftTop, piFocusRightBottom, rv_sdoaq);
	}
}

//----------------------------------------------------------------------------
eErrorCode CSdoaqAutoFocusDlg::SetIntTypeParaValue(eParameterId paraID, int nValue)
{
	int nMin, nMax;
	eErrorCode rv_sdoaq = ::SDOAQ_GetIntParameterRange(paraID, &nMin, &nMax);
	if (ecNoError == rv_sdoaq)
	{
		if (nValue >= nMin && nValue <= nMax)
		{
			return ::SDOAQ_SetIntParameterValue(paraID, nValue);
		}
		else
		{
			g_LogLine(_T("[ParamID-%d] : value is out of range[%d ~ %d]"), paraID, nMin, nMax);
			return ecInvalidParameter;
		}
	}
	else
	{
		g_LogLine(_T("SDOAQ_GetIntParameterRange[ParamID-%d] returns error(%d)."), paraID, rv_sdoaq);
		return rv_sdoaq;
	}
}

//----------------------------------------------------------------------------
void CSdoaqAutoFocusDlg::OnSdoaqSetSharpnessMeasureMethod()
{
	CString sValue;
	GetDlgItemText(IDC_EDIT_SMM, sValue);

	// focus measure (sharpness measure) method (0: Modified Laplacian, 1: Gradient(Sobel), 2: Graylevel local variance)
	if (ecNoError != SetIntTypeParaValue(pi_af_sharpness_measure_method, _ttoi(sValue)))
		SetDlgItemText(IDC_EDIT_SMM, _T("0"));
}

//----------------------------------------------------------------------------
void CSdoaqAutoFocusDlg::OnSdoaqSetResamplingMethod()
{
	CString sValue;
	GetDlgItemText(IDC_EDIT_RM, sValue);

	// image processing resolution(0: full, 1: half, 2: quarter)
	if (ecNoError != SetIntTypeParaValue(pi_af_resampling_method, _ttoi(sValue)))
		SetDlgItemText(IDC_EDIT_RM, _T("1"));
}

//----------------------------------------------------------------------------
void CSdoaqAutoFocusDlg::OnSdoaqSetStabilityMethod()
{
	CString sValue;
	GetDlgItemText(IDC_EDIT_SM, sValue);

	// range = { 1(None) , 2(stability-fullstep), 3(stability-halfstep), 4(stability-onestep) }
	if (ecNoError != SetIntTypeParaValue(pi_af_stability_method, _ttoi(sValue)))
		SetDlgItemText(IDC_EDIT_SM, _T("1"));
}

//----------------------------------------------------------------------------
void CSdoaqAutoFocusDlg::OnSdoaqSetStabilityDebounceCount()
{
	CString sValue;
	GetDlgItemText(IDC_EDIT_SDC, sValue);

	// range = {0 ~ 10}
	if (ecNoError != SetIntTypeParaValue(pi_af_stability_debounce_count, _ttoi(sValue)))
		SetDlgItemText(IDC_EDIT_SDC, _T("4"));
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

	AFP.callbackUserData = (void*)::GetTickCount64();
	eErrorCode rv_sdoaq = ::SDOAQ_SingleShotAFEx(
		&AFP,
		pPositions, (int)FOCUS.numsFocus,
		pAFImageBuffer,
		AFImageBufferSize,
		&dbBestFocusStep,
		&dbBestScore,
		&dbMatchedFocusStep
	);
	if (ecNoError == rv_sdoaq)
	{
		++m_nContiAF;

		if (pAFImageBuffer && AFImageBufferSize)
		{
			ImageViewer("AF", m_nContiAF, SET, pAFImageBuffer);

			g_LogLine(_T(">> best focus: %.4lf, best focus score: %.4lf, current focus: %d"), dbBestFocusStep, dbBestScore, (int)dbMatchedFocusStep);
		}
		else
		{
			ImageViewer("AF", m_nContiAF);
		}
	}
	else
	{
		g_LogLine(_T("SDOAQ_SingleShotAFEx() returns error(%d)."), rv_sdoaq);
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

	AFP.callbackUserData = (void*)::GetTickCount64();
	eErrorCode rv_sdoaq = ::SDOAQ_PlayAFEx(
		&AFP,
		g_PlayAFCallbackEx,
		pPositions, (int)FOCUS.numsFocus,
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
		g_LogLine(_T("SDOAQ_PlayAFEx() returns error(%d)."), rv_sdoaq);
	}

	delete[] pPositions;
}

//----------------------------------------------------------------------------
void CSdoaqAutoFocusDlg::OnSdoaqStopAF()
{
	SET.rb.active = false;

	const eErrorCode rv_sdoaq = ::SDOAQ_StopAF();
	if (ecNoError != rv_sdoaq)
	{
		g_LogLine(_T("SDOAQ_StopAF() returns error(%d)."), rv_sdoaq);
	}

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
static void g_PlayAFCallbackEx(eErrorCode errorCode, int lastFilledRingBufferEntry, void* callbackUserData, double dbBestFocusStep, double dbBestScore, double dbMatchedFocusStep)
{
	if (theApp.m_pMainWnd)
	{
		auto pcPara = new tMsgParaReceiveAf;
		pcPara->lastFilledRingBufferEntry = lastFilledRingBufferEntry;
		pcPara->dbBestFocusStep = dbBestFocusStep;
		pcPara->dbBestScore = dbBestScore;
		pcPara->dbMatchedFocusStep = dbMatchedFocusStep;
		theApp.m_pMainWnd->PostMessageW(EUM_RECEIVE_AF, (WPARAM)errorCode, (LPARAM)pcPara);

		static void* g_prev = NULL; if (g_prev != callbackUserData) { g_prev = callbackUserData; g_LogLine(_T("AF callback 0x%I64X"), (unsigned long long)callbackUserData); }
	}
}
