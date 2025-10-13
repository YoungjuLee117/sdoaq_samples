
// SdoaqCudaEdofDlg.cpp : implementation file
//

#include "pch.h"
#include "framework.h"
#include "SdoaqRunCudaEdof.h"
#include "SdoaqRunCudaEdofDlg.h"
#include "afxdialogex.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


//----------------------------------------------------------------------------
static WSIOVOID g_hViewer = NULL;
//----------------------------------------------------------------------------
static void g_SDOAQ_InitDoneCallback(eErrorCode errorCode, char* pErrorMessage);
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

CSdoaqEdofDlg::CSdoaqEdofDlg(CWnd* pParent /*=nullptr*/)
	: CDialogEx(IDD_SDOAQCUDAEDOF_DIALOG, pParent)
{
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
}

void CSdoaqEdofDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
}

BEGIN_MESSAGE_MAP(CSdoaqEdofDlg, CDialogEx)
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	ON_WM_SIZE()
	ON_WM_CLOSE()
	ON_MESSAGE(EUM_INITDONE, OnInitDone)
	ON_BN_CLICKED(IDC_SET_CALIBRATION, OnSdoaqSetCalibrationFile)
	ON_BN_CLICKED(IDC_SET_ROI, OnSdoaqSetROI)
	ON_BN_CLICKED(IDC_SET_FOCUS_SET, OnSdoaqSetFocusSet)
	ON_BN_CLICKED(IDC_SET_EDOF_RESIZE_RATIO, OnSdoaqSetEdofResize)
	ON_BN_CLICKED(IDC_SET_EDOF_ITERATION, OnSdoaqSetEdofIteration)
	ON_BN_CLICKED(IDC_SET_EDOF_THRESHOLD, OnSdoaqSetEdofThreshold)
	ON_BN_CLICKED(IDC_SET_EDOF_SCALE_STEP, OnSdoaqSetEdofScaleStep)
	ON_BN_CLICKED(IDC_RUN_EDOF, OnSdoaqCaptureAndRunCudaEdof)
END_MESSAGE_MAP()


// CSdoaqEdofDlg message handlers
//----------------------------------------------------------------------------
BOOL CSdoaqEdofDlg::OnInitDialog()
{
	CDialogEx::OnInitDialog();

	// Set the icon for this dialog.  The framework does this automatically
	//  when the application's main window is not a dialog
	SetIcon(m_hIcon, TRUE);			// Set big icon
	SetIcon(m_hIcon, FALSE);		// Set small icon

	// TODO: Add extra initialization here
	g_LogLine(_T("================================================"));
	g_LogLine(_T(" SDOAQ Run CUDA EDoF Algorithm Sample"));
	g_LogLine(_T("================================================"));


	//================================================================================================================
	//
	// If you capture images directly without using SDOAQ library, you do not need to perform library initialization.
	//
	//================================================================================================================
	// set the cam files folder path
	::SDOAQ_SetCamfilePath(FStringA("%s\\..\\..\\Include\\SDOAQ\\CamFiles", (CStringA)GetCurrentDir()));

	g_LogLine(_T("start SDOAQ initialization..."));
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

	g_LogLine(_T("wsio dll version is \"%s\""), (CString)::WSIO_GetVersion(FALSE));

	SetDlgItemText(IDC_EDIT_ROI, _T("0,0,2040,1086"));
	SetDlgItemText(IDC_EDIT_FOCUS_SET, _T("0-319-35"));
	SetDlgItemText(IDC_EDIT_EDOF_RESIZE_RATIO, _T("0.5"));
	SetDlgItemText(IDC_EDIT_EDOF_ITERATION, _T("8"));
	SetDlgItemText(IDC_EDIT_EDOF_THRESHOLD, _T("1.0"));
	SetDlgItemText(IDC_EDIT_EDOF_SCALE_STEP, _T("160"));

	SendMessage(WM_SIZE); // invoke WSUT_IV_ShowWindow call with size.

	return TRUE;  // return TRUE  unless you set the focus to a control
}

//----------------------------------------------------------------------------
void CSdoaqEdofDlg::OnPaint()
{
	if (IsIconic())
	{
		CPaintDC dc(this); // device context for painting

		SendMessage(WM_ICONERASEBKGND, reinterpret_cast<WPARAM>(dc.GetSafeHdc()), 0);

		// Center icon in client rectangle
		int cxIcon = GetSystemMetrics(SM_CXICON);
		int cyIcon = GetSystemMetrics(SM_CYICON);
		CRect rect;
		GetClientRect(&rect);
		int x = (rect.Width() - cxIcon + 1) / 2;
		int y = (rect.Height() - cyIcon + 1) / 2;

		// Draw the icon
		dc.DrawIcon(x, y, m_hIcon);
	}
	else
	{
		CDialogEx::OnPaint();
	}
}

//----------------------------------------------------------------------------
// The system calls this function to obtain the cursor to display while the user drags
//  the minimized window.
HCURSOR CSdoaqEdofDlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}

//----------------------------------------------------------------------------
void CSdoaqEdofDlg::OnSize(UINT nType, int cx, int cy)
{
	CDialogEx::OnSize(nType, cx, cy);

	// TODO: Add your message handler code here
	CWnd* p_wnd = GetDlgItem(IDC_IMAGE);
	if (p_wnd)
	{
		CRect rc;
		GetDlgItem(IDC_IMAGE)->GetWindowRect(rc);
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
void CSdoaqEdofDlg::OnClose()
{
	// TODO: Add your message handler code here and/or call default
	(void)::SDOAQ_CUDAEDOF_FinalizeLibrary();
	(void)::SDOAQ_Finalize();
	(void)::WSUT_IV_DestroyImageViewer(g_hViewer);

	CDialogEx::OnClose();
}

//----------------------------------------------------------------------------
BOOL CSdoaqEdofDlg::PreTranslateMessage(MSG* pMsg)
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
LRESULT CSdoaqEdofDlg::OnInitDone(WPARAM wErrorCode, LPARAM lpMessage)
{
	CString* pMessage = (CString*)lpMessage;

	if (ecNoError == wErrorCode)
	{
		g_LogLine(_T("InitDoneCallback() %s"), pMessage ? *pMessage : _T(""));

		const int ver = ::SDOAQ_CUDAEDOF_GetVersion();
		g_LogLine(_T("sdoaq cuda edof dll version is \"%d\""), ver);

		m_nCudaAvailability = ::SDOAQ_CUDAEDOF_CheckAvailability();
		g_LogLine(_T("sdoaq cuda availability is \"%d\""), m_nCudaAvailability);

		if (m_nCudaAvailability < 0)
		{
			g_LogLine(_T("Your NVIDIA graphics card does not support CUDA-based EDoF processing. Please upgrade your graphics card to enable this feature."));
			return 0;
		}

		SET.m_nColorByte = IsMonoCameraInstalled() ? MONOBYTES : COLORBYTES;

		OnSdoaqSetROI();
		OnSdoaqSetFocusSet();
		OnSdoaqSetEdofResize();
		OnSdoaqSetEdofIteration();
		OnSdoaqSetEdofThreshold();
		OnSdoaqSetEdofScaleStep();
		//::SDOAQ_SetIntParameterValue(pi_edof_is_scale_correction_enabled, 1);
		//::SDOAQ_SetIntParameterValue(pi_edof_algorithm_method, 67);
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
void CSdoaqEdofDlg::OnSdoaqSetCalibrationFile(void)
{
	CString sFilter = _T("calibration file (*.csv)|*.csv|");
	CFileDialog dlg(TRUE, _T("cvs"), NULL, OFN_HIDEREADONLY | OFN_OVERWRITEPROMPT, sFilter);
	if (dlg.DoModal() == IDOK)
	{
		m_sCalibFileName = dlg.GetPathName().GetBuffer();
		g_LogLine(_T("calibration file (%s) is set"), dlg.GetFileName());
	}
}

//----------------------------------------------------------------------------
void CSdoaqEdofDlg::OnSdoaqSetROI()
{
	CString sParameters;
	GetDlgItemText(IDC_EDIT_ROI, sParameters);

	CString sLeft, sTop, sWidth, sHeight;
	AfxExtractSubString(sLeft, sParameters, 0, ',');
	AfxExtractSubString(sTop, sParameters, 1, ',');
	AfxExtractSubString(sWidth, sParameters, 2, ',');
	AfxExtractSubString(sHeight, sParameters, 3, ',');

	AcquisitionFixedParametersEx AFP;
	AFP.cameraRoiTop = _ttoi(sTop);
	AFP.cameraRoiLeft = _ttoi(sLeft);
	AFP.cameraRoiWidth = (_ttoi(sWidth) / 4) * 4;
	AFP.cameraRoiHeight = _ttoi(sHeight);
	AFP.cameraBinning = 1;
	AFP.callbackUserData = NULL;

	int nDummy, nMaxWidth, nMaxHeight;
	auto rv_sdoaq = ::SDOAQ_GetIntParameterRange(piCameraFullFrameSizeX, &nDummy, &nMaxWidth);
	if (ecNoError == rv_sdoaq)
	{
		if (AFP.cameraRoiLeft < 0 || AFP.cameraRoiLeft > nMaxWidth)
		{
			g_LogLine(_T("Set cameraRoiLeft : value is out of range( ~ %d)"), nMaxWidth);
			return;
		}
		if (AFP.cameraRoiWidth < 1 || AFP.cameraRoiWidth > nMaxWidth)
		{
			g_LogLine(_T("Set cameraRoiWidth : value is out of range( ~ %d)"), nMaxWidth);
			return;
		}
	}
	else
	{
		g_LogLine(_T("SDOAQ_GetIntParameterRange(piCameraFullFrameSizeX) returns error(%d)."), rv_sdoaq);
		return;
	}

	rv_sdoaq = ::SDOAQ_GetIntParameterRange(piCameraFullFrameSizeY, &nDummy, &nMaxHeight);
	if (ecNoError == rv_sdoaq)
	{
		if (AFP.cameraRoiTop < 0 || AFP.cameraRoiTop > nMaxHeight)
		{
			g_LogLine(_T("Set cameraRoiTop : value is out of range( ~ %d)"), nMaxHeight);
			return;
		}
		if (AFP.cameraRoiHeight < 1 || AFP.cameraRoiHeight > nMaxHeight)
		{
			g_LogLine(_T("Set cameraRoiHeight : value is out of range( ~ %d)"), nMaxHeight);
			return;
		}
	}
	else
	{
		g_LogLine(_T("SDOAQ_GetIntParameterRange(piCameraFullFrameSizeY) returns error(%d)."), rv_sdoaq);
		return;
	}

	if (!SET.rb.active)
	{
		g_LogLine(_T("set roi: (left:%d, top:%d, width:%d, height:%d)"), AFP.cameraRoiTop, AFP.cameraRoiLeft, AFP.cameraRoiWidth, AFP.cameraRoiHeight);
		SET.afp = AFP;
	}
}

//----------------------------------------------------------------------------
void CSdoaqEdofDlg::OnSdoaqSetFocusSet()
{
	CString sFocusSet;
	GetDlgItemText(IDC_EDIT_FOCUS_SET, sFocusSet);

	m_vFocusSet.clear();

	if (sFocusSet.Find(_T("-")) != -1)
	{
		int nLow, nHigh, nUnit;
		if (3 <= swscanf_s((LPCTSTR)sFocusSet, _T("%d-%d-%d"), &nLow, &nHigh, &nUnit))
		{
			for (int nFocus = nLow; nFocus <= nHigh; nFocus += nUnit)
			{
				m_vFocusSet.push_back(nFocus);
			}
		}
	}
	else
	{
		int posSeek = 0;
		do
		{
			CString sSeek = sFocusSet.Mid(posSeek);
			int nFocus;
			if (1 <= swscanf_s((LPCTSTR)sSeek, _T("%d"), &nFocus))
			{
				m_vFocusSet.push_back(nFocus);
			}
		} while (-1 != (posSeek = sFocusSet.Find(_T(" "), posSeek + 1)));
	}

	CString sFocuslist = _T("");
	CString sFocus;
	for (auto focus : m_vFocusSet)
	{
		sFocus.Format(_T("%d, "), focus);
		sFocuslist += sFocus;
	}
	g_LogLine(_T("set focus: %s"), sFocuslist);
}

//----------------------------------------------------------------------------
void CSdoaqEdofDlg::OnSdoaqSetEdofResize()
{
	CString sEdofResize;
	GetDlgItemText(IDC_EDIT_EDOF_RESIZE_RATIO, sEdofResize);

	m_resize_ratio = _ttof(sEdofResize);

	double dbMin, dbMax;
	auto rv_sdoaq = ::SDOAQ_GetDblParameterRange(pi_edof_calc_resize_ratio, &dbMin, &dbMax);
	if (ecNoError == rv_sdoaq)
	{
		if (m_resize_ratio >= dbMin && m_resize_ratio <= dbMax)
		{
			//::SDOAQ_SetDblParameterValue(pi_edof_calc_resize_ratio, m_resize_ratio);
		}
		else
		{
			g_LogLine(_T("set EDoF resize ratio: value is out of range(%.2lf ~ %.2lf)"), dbMin, dbMax);
		}
	}
	else
	{
		g_LogLine(_T("SDOAQ_GetIntParameterRange(pi_edof_calc_resize_ratio) returns error(%d)."), rv_sdoaq);
	}
}

//----------------------------------------------------------------------------
void CSdoaqEdofDlg::OnSdoaqSetEdofIteration()
{
	CString sIteration;
	GetDlgItemText(IDC_EDIT_EDOF_ITERATION, sIteration);

	m_pixelwise_iteration = _ttoi(sIteration);

	int nMin, nMax;
	auto rv_sdoaq = ::SDOAQ_GetIntParameterRange(pi_edof_calc_pixelwise_iteration, &nMin, &nMax);
	if (ecNoError == rv_sdoaq)
	{
		if (m_pixelwise_iteration >= nMin && m_pixelwise_iteration <= nMax)
		{
			//::SDOAQ_SetIntParameterValue(pi_edof_calc_pixelwise_iteration, m_pixelwise_iteration);
		}
		else
		{
			g_LogLine(_T("set EDoF pixelwise iteration: value is out of range(%d ~ %d)"), nMin, nMax);
		}
	}
	else
	{
		g_LogLine(_T("SDOAQ_GetIntParameterRange(pi_edof_calc_pixelwise_iteration) returns error(%d)."), rv_sdoaq);
	}
}

//----------------------------------------------------------------------------
void CSdoaqEdofDlg::OnSdoaqSetEdofThreshold()
{
	CString sEdofThreshold;
	GetDlgItemText(IDC_EDIT_EDOF_THRESHOLD, sEdofThreshold);

	m_depth_quality_threshold = _ttof(sEdofThreshold);

	double dbMin, dbMax;
	auto rv_sdoaq = SDOAQ_GetDblParameterRange(pi_edof_depth_quality_th, &dbMin, &dbMax);
	if (ecNoError == rv_sdoaq)
	{
		if (m_depth_quality_threshold >= dbMin && m_depth_quality_threshold <= dbMax)
		{
			//::SDOAQ_SetDblParameterValue(pi_edof_depth_quality_th, m_depth_quality_threshold);
		}
		else
		{
			g_LogLine(_T("set EDoF depth quality threshold: value is out of range(%.2lf ~ %.2lf)"), dbMin, dbMax);
		}
	}
}

//----------------------------------------------------------------------------
void CSdoaqEdofDlg::OnSdoaqSetEdofScaleStep()
{
	CString sEdofScaleStep;
	GetDlgItemText(IDC_EDIT_EDOF_SCALE_STEP, sEdofScaleStep);

	m_scale_ref_step = _ttoi(sEdofScaleStep);

	int nMin, nMax;
	auto rv_sdoaq = ::SDOAQ_GetIntParameterRange(pi_edof_scale_correction_dst_step, &nMin, &nMax);
	if (ecNoError == rv_sdoaq)
	{
		if (m_scale_ref_step >= nMin && m_scale_ref_step <= nMax)
		{
			//::SDOAQ_SetIntParameterValue(pi_edof_scale_correction_dst_step, m_scale_ref_step);
		}
		else
		{
			g_LogLine(_T("set EDoF scale correction dst step: value is out of range(%d ~ %d)"), nMin, nMax);
		}
	}
	else
	{
		g_LogLine(_T("SDOAQ_GetIntParameterRange(pi_edof_scale_correction_dst_step) returns error(%d)."), rv_sdoaq);
	}
}

//----------------------------------------------------------------------------
void CSdoaqEdofDlg::OnSdoaqCaptureAndRunCudaEdof()
{
	if (m_nCudaAvailability < 0)
	{
		return;
	}

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
	unsigned char** ppFocusImages = new unsigned char*[FOCUS.numsFocus];
	size_t* pFocusImageBufferSizes = new size_t[FOCUS.numsFocus];

	for (size_t pos = 0; pos < FOCUS.numsFocus; pos++)
	{
		pPositions[pos] = FOCUS.vFocusSet[pos];

		auto size = SET.ImgSize();
		ppFocusImages[pos] = new unsigned char[size];
		pFocusImageBufferSizes[pos] = size;
	}

	const auto tick_begin = GetTickCount64();
	AFP.callbackUserData = (void*)::GetTickCount64();
	const eErrorCode rv_sdoaq = ::SDOAQ_SingleShotFocusStackEx(
		&AFP,
		pPositions, (int)FOCUS.numsFocus,
		ppFocusImages, pFocusImageBufferSizes
	);

	//----------------------------------------------------------------------------
	//
	//		Starting point of the EDOF algorithm execution.
	//
	//		Make sure to set each parameter to a suitable value.
	//
	//		Don't forget to specify the calibration file before proceeding.
	//
	//----------------------------------------------------------------------------

	if (ecNoError == rv_sdoaq)
	{
		SDOAQ_CUDA_EDOF_Params edofParams;
		edofParams.numMalsStep = (int)FOCUS.numsFocus;
		edofParams.imageWidth = AFP.cameraRoiWidth;
		edofParams.imageHeight = AFP.cameraRoiHeight;
		edofParams.imageOffsetX = AFP.cameraRoiLeft;
		edofParams.imageOffsetY = AFP.cameraRoiTop;
		edofParams.pixelColorType = SET.m_nColorByte;
		edofParams.samplingMode = m_resize_ratio;
		edofParams.pixelwiseKernelIteration = m_pixelwise_iteration;
		edofParams.depthThreshold = m_depth_quality_threshold;
		edofParams.stepInterval = FOCUS.numsFocus > 1 ? FOCUS.vFocusSet[1] - FOCUS.vFocusSet[0] : 0;
		edofParams.firstStep = FOCUS.vFocusSet[0];
		edofParams.isScaleCorrectionEnabled = true; // true or false
		edofParams.scaleCorrectionDstStep = m_scale_ref_step;
		

		// 1. Initialize EDoF algorithm library with EDoF parameter and calibration file
		// Call SDOAQ_CUDA_InitializeEdofLibrary() when calibration data or core parameters are changed.
		// For updating certain tunable parameters at runtime, use SDOAQ_CUDA_UpdateParams() without reinitializing.
		auto edof_rv = ::SDOAQ_CUDAEDOF_InitializeLibrary(edofParams, CT2A(m_sCalibFileName));
		if (0 > edof_rv)
		{
			g_LogLine(_T("SDOAQ_CUDA_InitializeEdofLibrary() returns error(%d)."), edof_rv);
			//return;
		}
		

		// 2. Register memory with cudaHostRegister
		for (int i = 0; i < FOCUS.numsFocus; i++)
		{
			(void)::SDOAQ_CUDAEDOF_RegisterMemory(ppFocusImages[i], sizeof(unsigned char) * SET.ImgSize());
		}

		unsigned char* pEdofImageBuffer = new unsigned char[SET.ImgSize()];
		auto edofImageBufferSize = SET.ImgSize();
		(void)::SDOAQ_CUDAEDOF_RegisterMemory(pEdofImageBuffer, sizeof(unsigned char) * edofImageBufferSize);

		clock_t runStart = clock();

		// 3. Add focus stack image to the algorithm
		for (int i = 0; i < FOCUS.numsFocus; i++)
		{
			(void)::SDOAQ_CUDAEDOF_AddImage(ppFocusImages[i], i);
		}		
		

		// 4. Run EDoF algorithm and generate output image
		edof_rv = ::SDOAQ_CUDAEDOF_Run(pEdofImageBuffer);
		clock_t runEnd = clock();

		if (ecNoError <= edof_rv)
		{
			++m_nContiEdof;

			if (pEdofImageBuffer && edofImageBufferSize)
			{
				ImageViewer("CUDA EDoF", m_nContiEdof, SET, pEdofImageBuffer);
			}
			else
			{
				ImageViewer("CUDA EDoF", m_nContiEdof);
			}
			g_LogLine(_T("SDOAQ_CUDA_RunEdof() total takes %d ms"), runEnd - runStart);
		}
		else
		{
			g_LogLine(_T("SDOAQ_CUDA_RunEdof() returns error(%d)."), edof_rv);
		}


		// 5. Unregister memory with cudaHostUnregister
		(void)::SDOAQ_CUDAEDOF_UnregisterMemory(pEdofImageBuffer);
		for (int i = 0; i < FOCUS.numsFocus; i++)
		{
			(void)::SDOAQ_CUDAEDOF_UnregisterMemory(ppFocusImages[i]);
		}

		delete[] pEdofImageBuffer;
	}
	else
	{
		g_LogLine(_T("SDOAQ_SingleShotEdofEx() returns error(%d)."), rv_sdoaq);
	}
	
	delete[] pFocusImageBufferSizes;
	for (size_t pos = 0; pos < FOCUS.numsFocus; pos++)
	{
		delete[] ppFocusImages[pos];
	}
	delete[] ppFocusImages;
	delete[] pPositions;
}

//----------------------------------------------------------------------------
void CSdoaqEdofDlg::ImageViewer(const char* title, int title_no, const tTestSet& SET, void* data)
{
	ImageViewer(title, title_no, SET.afp.cameraRoiWidth, SET.afp.cameraRoiHeight, SET.m_nColorByte, data);
}

//----------------------------------------------------------------------------
void CSdoaqEdofDlg::ImageViewer(const char* title, int title_no, int width, int height, int colorbytes, void* data)
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
	if (theApp.m_pMainWnd)
	{
		theApp.m_pMainWnd->PostMessageW(EUM_INITDONE, (WPARAM)errorCode, (LPARAM)NewWString(pErrorMessage));
	}
}
