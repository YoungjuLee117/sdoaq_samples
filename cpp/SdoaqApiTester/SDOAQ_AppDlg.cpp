#include "stdafx.h"

#include "SDOAQ_App.h"
#include "SDOAQ_AppDlg.h"
#include "afxdialogex.h"
#include "SDOAQ_Callback.h"

//----------------------------------------------------------------------------
CSDOAQ_Dlg::CSDOAQ_Dlg(CWnd* pParent /*=NULL*/)
	: CDialogEx(IDD_SDOAQ_APP_DIALOG, pParent)
	, m_nRingBufferSize(3)
	, m_nContiStack(0)
	, m_nContiEdof(0)
	, m_nContiAF(0)
	, dxRangeStart()
	, dxRangeEnd()
	, dyRangeStart()
	, dyRangeEnd()
	, dzRangeStart()
	, dzRangeEnd()
	, m_hLogFile(INVALID_HANDLE_VALUE)
	, m_tickLastLog(0)
	, m_hwnd3D(NULL)
{
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);

	m_vFocusSet.push_back(DFLT_FOCUS_STEP);
	m_vSnapFocusSet.push_back(DFLT_FOCUS_STEP);
}

//----------------------------------------------------------------------------
CSDOAQ_Dlg::~CSDOAQ_Dlg()
{
}

//----------------------------------------------------------------------------
BEGIN_MESSAGE_MAP(CSDOAQ_Dlg, CDialogEx)
	ON_WM_SYSCOMMAND()
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	ON_WM_SIZE()
	ON_WM_CLOSE()
	ON_WM_DESTROY()
	ON_WM_TIMER()
	ON_MESSAGE(EUM_LOG, OnUmLog)
	ON_MESSAGE(EUM_ERROR, OnUmError)
	ON_MESSAGE(EUM_INITDONE, OnInitDone)
	ON_MESSAGE(EUM_RECEIVE_ZSTACK, OnReceiveZstack)
	ON_MESSAGE(EUM_RECEIVE_SINGLE_FOCUS, OnReceiveSingleFocus)
	ON_MESSAGE(EUM_RECEIVE_EDOF, OnReceiveEdof)
	ON_MESSAGE(EUM_RECEIVE_AF, OnReceiveAF)
	ON_MESSAGE(EUM_RECEIVE_SNAP, OnReceiveSnap)
	ON_BN_CLICKED(IDC_CHECK_SIMUL_MODE, OnCheckSimulMode)
	ON_BN_CLICKED(IDC_INITIALIZE, OnSdoaqInitialize)
	ON_BN_CLICKED(IDC_FINALIZE, OnSdoaqFinalize)
	ON_CBN_SELENDOK(IDC_COMBO_PARAMETER, OnSelectedCombobox)
	ON_BN_CLICKED(IDC_SET_PARAMETER, OnSdoaqSetParameter)
	ON_BN_CLICKED(IDC_SET_ROI, OnSdoaqSetROI)
	ON_BN_CLICKED(IDC_SET_AFROI, OnSdoaqSetAFROI)
	ON_BN_CLICKED(IDC_SET_RING_BUF_SIZE, OnSdoaqSetRingBufSize)
	ON_BN_CLICKED(IDC_SET_FOCUS_SET, OnSdoaqSetFocusSet)
	ON_BN_CLICKED(IDC_SET_SNAPFOCUS_SET, OnSdoaqSetSnapFocusSet)
	ON_BN_CLICKED(IDC_ACQ_STACK, OnSdoaqSingleShotStack)
	ON_BN_CLICKED(IDC_CONTI_STACK, OnSdoaqPlayStack)
	ON_BN_CLICKED(IDC_STOP_STACK, OnSdoaqStopStack)
	ON_BN_CLICKED(IDC_ACQ_EDOF, OnSdoaqSingleShotEdof)
	ON_BN_CLICKED(IDC_CONTI_EDOF, OnSdoaqPlayEdof)
	ON_BN_CLICKED(IDC_STOP_EDOF, OnSdoaqStopEdof)
	ON_BN_CLICKED(IDC_ACQ_AF, OnSdoaqSingleShotAF)
	ON_BN_CLICKED(IDC_CONTI_AF, OnSdoaqPlayAF)
	ON_BN_CLICKED(IDC_STOP_AF, OnSdoaqStopAF)
	ON_BN_CLICKED(IDC_SNAP, OnSdoaqSnap)
	ON_BN_CLICKED(IDC_CONTI_SINGLE_FOCUS, OnSdoaqPlaySingleFocus)
	ON_BN_CLICKED(IDC_STOP_SINGLE_FOCUS, OnSdoaqStopSingleFocus)
	ON_BN_CLICKED(IDC_SET_CALIBRATION, OnSdoaqSetCalibrationFile)
	ON_CBN_SELENDOK(IDC_COMBO_OBJECTIVE, OnSdoaqComboObjective)
END_MESSAGE_MAP()

//============================================================================
BOOL CSDOAQ_Dlg::OnInitDialog()
{
	CDialogEx::OnInitDialog();

	// Add "About..." menu item to system menu.

	// IDM_ABOUTBOX must be in the system command range.
	ASSERT((IDM_ABOUTBOX & 0xFFF0) == IDM_ABOUTBOX);
	ASSERT(IDM_ABOUTBOX < 0xF000);

	CMenu* pSysMenu = GetSystemMenu(FALSE);
	if (pSysMenu != NULL)
	{
		BOOL bNameValid;
		CString strAboutMenu;
		bNameValid = strAboutMenu.LoadString(IDS_ABOUTBOX);
		ASSERT(bNameValid);
		if (!strAboutMenu.IsEmpty())
		{
			pSysMenu->AppendMenu(MF_SEPARATOR);
			pSysMenu->AppendMenu(MF_STRING, IDM_ABOUTBOX, strAboutMenu);
		}
	}

	// Set the icon for this dialog.  The framework does this automatically
	//  when the application's main window is not a dialog
	SetIcon(m_hIcon, TRUE);			// Set big icon
	SetIcon(m_hIcon, FALSE);		// Set small icon

	// TODO: Add extra initialization here
	BuildEnvironment();

	BuildParameterID_Combobox();

	((CButton*)GetDlgItem(IDC_CHECK_STEPMAP))->SetCheck(1);
	((CButton*)GetDlgItem(IDC_CHECK_EDOF))->SetCheck(1);
	//((CButton*)GetDlgItem(IDC_CHECK_QUALITYMAP))->SetCheck(1);
	((CButton*)GetDlgItem(IDC_CHECK_HEIGHTMAP))->SetCheck(1);
	((CButton*)GetDlgItem(IDC_CHECK_POINTCLOUD))->SetCheck(1);

	//BuildCalibrationFile_Combobox();

	//// CREATE IMAGE VIEWER
	m_vhwndIV.resize(3, NULL);
	for (auto it = m_vhwndIV.begin(); it != m_vhwndIV.end(); it++)
	{
		if (WSIORV_SUCCESS <= ::WSUT_IV_CreateImageViewer((WSIOCSTR)_T("Main viewer")
			, (WSIOVOID)(this->m_hWnd), &(*it), 0
			, WSUTIVOPMODE_VISION | WSUTIVOPMODE_TOPTITLE | WSUTIVOPMODE_FRAMEOUTER
		))
		{
			(void)WSUT_IV_SetColor(*it, WSUTIVRESOURCE_OUTERFRAME, RGB(70, 130, 180));
		}
		else
		{
			*it = NULL;
			print_wsio_last_error();
		}
	}

	auto rv_sdoaq = ::WSGL_Initialize(m_hWnd, &m_hwnd3D);
	if (WSIORV_SUCCESS <= rv_sdoaq)
	{
		(void)::WSGL_SetDisplayAttributes(m_hwnd3D, EDA_SHOW_GUIDER_OBJECTS
			| EDA_SHOW_SCALE_OBJECTS
			| EDA_SHOW_COLORMAPBAR_OBJECTS
		);
		(void)::WSGL_Display_BG(m_hwnd3D);
	}
	else
	{
		Log(FString(_T("[WSGL Error] >> returns error(%d)."), rv_sdoaq));
		print_wsgl_last_error();
	}

	ShowViewer();

	ShowWindow(SW_MAXIMIZE);

	return TRUE;  // return TRUE  unless you set the focus to a control
}

//----------------------------------------------------------------------------
void CSDOAQ_Dlg::BuildParameterID_Combobox(void)
{
	auto p_combo = (CComboBox*)GetDlgItem(IDC_COMBO_PARAMETER);
	if (p_combo)
	{
		p_combo->ResetContent();
		auto ADD_PI = [&](LPCTSTR name, eParameterId value)
		{
			p_combo->SetItemData(p_combo->AddString(name), value);
		};

		// camera
		ADD_PI(_T("CameraExposureTime"), piCameraExposureTime);
		ADD_PI(_T("DataExposureTime"), piDataExposureTime);
		ADD_PI(_T("CameraFullFrameSizeX"), piCameraFullFrameSizeX);
		ADD_PI(_T("CameraFullFrameSizeY"), piCameraFullFrameSizeY);
		ADD_PI(_T("CameraPixelSizeX"), piCameraPixelSizeX);
		ADD_PI(_T("CameraPixelSizeY"), piCameraPixelSizeY);
		ADD_PI(_T("CameraBinning"), piCameraBinning);
		ADD_PI(_T("CameraGain"), piCameraGain);
		ADD_PI(_T("DataGain"), piDataGain);
		ADD_PI(_T("WhiteBalanceRed"), piWhiteBalanceRed);
		ADD_PI(_T("WhiteBalanceGreen"), piWhiteBalanceGreen);
		ADD_PI(_T("WhiteBalanceBlue"), piWhiteBalanceBlue);
		ADD_PI(_T("CameraFfcId"), piCameraFfcId);
		ADD_PI(_T("DataCamFfcId"), piDataCamFfcId);
		ADD_PI(_T("SoftwareFfcId"), piSoftwareFfcId);
		ADD_PI(_T("DataSoftwareFfcId"), piDataSoftwareFfcId);
		ADD_PI(_T("CameraColor"), piCameraColor);
		//
		ADD_PI(_T("FocusPosition"), piFocusPosition);
		ADD_PI(_T("SingleFocus"), piSingleFocus);
		ADD_PI(_T("ReflexCorrectionAlgorithm"), piReflexCorrectionAlgorithm);
		ADD_PI(_T("ReflexCorrectionPattern"), piReflexCorrectionPattern);
		ADD_PI(_T("FocusMeasureMethod"), piFocusMeasureMethod);
		ADD_PI(_T("MaxStacksPerSecond"), piMaxStacksPerSecond);
		ADD_PI(_T("ObjectiveId"), piObjectiveId);
		ADD_PI(_T("FocusLeftTop"), piFocusLeftTop);
		ADD_PI(_T("FocusRightBottom"), piFocusRightBottom);
		ADD_PI(_T("SaveFileFormat"), piSaveFileFormat);
		ADD_PI(_T("SaveOnlyResult"), piSaveOnlyResult);
		ADD_PI(_T("VpsReportCycleSeconds"), piVpsReportCycleSeconds);
		ADD_PI(_T("VpsReportTimeSeconds"), piVpsReportTimeSeconds);
		ADD_PI(_T("SimulMalsHighestStep"), piSimulMalsHighestStep);
		ADD_PI(_T("SimulMalsLowestStep"), piSimulMalsLowestStep);
		// light
		ADD_PI(_T("LightingList"), piLightingList);
		ADD_PI(_T("ActiveLightingList"), piActiveLightingList);
		ADD_PI(_T("SelectSettingLighting"), piSelectSettingLighting);
		if ("sdzeiss lighting device")
		{
			ADD_PI(_T("InnerRingIntensity"), piInnerRingIntensity);
			ADD_PI(_T("MiddleRingIntensity"), piMiddleRingIntensity);
			ADD_PI(_T("OuterRingIntensity"), piOuterRingIntensity);
			ADD_PI(_T("CoaxIntensity"), piCoaxIntensity);
		}
		if ("lcbpwm lighting device")
		{
			ADD_PI(_T("Channel 1 Intensity"), piIntensityGeneralChannel_1);
			ADD_PI(_T("Channel 2 Intensity"), piIntensityGeneralChannel_2);
			ADD_PI(_T("Channel 3 Intensity"), piIntensityGeneralChannel_3);
			ADD_PI(_T("Channel 4 Intensity"), piIntensityGeneralChannel_4);
			ADD_PI(_T("Channel 5 Intensity"), piIntensityGeneralChannel_5);
			ADD_PI(_T("Channel 6 Intensity"), piIntensityGeneralChannel_6);
			ADD_PI(_T("Channel 7 Intensity"), piIntensityGeneralChannel_7);
			ADD_PI(_T("Channel 8 Intensity"), piIntensityGeneralChannel_8);
		}
		// edof algorithm
		ADD_PI(_T("edof_algorithm_method"), pi_edof_algorithm_method);
		ADD_PI(_T("edof_calc_resize_ratio"), pi_edof_calc_resize_ratio);
		ADD_PI(_T("edof_calc_pixelwise_kernel_size"), pi_edof_calc_pixelwise_kernel_size);
		ADD_PI(_T("edof_calc_pixelwise_iteration"), pi_edof_calc_pixelwise_iteration);
		ADD_PI(_T("edof_depthwise_kernel_size"), pi_edof_depthwise_kernel_size);
		ADD_PI(_T("edof_depth_quality_th"), pi_edof_depth_quality_th);
		ADD_PI(_T("edof_bilateral_sigma_color"), pi_edof_bilateral_sigma_color);
		ADD_PI(_T("edof_bilateral_sigma_space"), pi_edof_bilateral_sigma_space);
		ADD_PI(_T("edof_num_thread"), pi_edof_num_thread);
		ADD_PI(_T("edof_is_scale_correction_enabled"), pi_edof_is_scale_correction_enabled);
		ADD_PI(_T("edof_scale_correction_dst_step"), pi_edof_scale_correction_dst_step);
		// af algorithm
		ADD_PI(_T("af_sharpness_measure_method"), pi_af_sharpness_measure_method);
		ADD_PI(_T("af_resampling_method"), pi_af_resampling_method);
		ADD_PI(_T("af_stability_method"), pi_af_stability_method);
		ADD_PI(_T("af_stability_debounce_count"), pi_af_stability_debounce_count);
		// auto function
		ADD_PI(_T("FeatureAutoExposure"), piFeatureAutoExposure);
		ADD_PI(_T("FeatureAutoWhiteBalance"), piFeatureAutoWhiteBalance);
		ADD_PI(_T("FeatureAutoIlluminate"), piFeatureAutoIlluminate);
		ADD_PI(_T("LogLevel"), piLogLevel);
		//p_combo->SetCurSel(0);
	}
}

//----------------------------------------------------------------------------
eParameterId CSDOAQ_Dlg::GetCurrentParameterID(void)
{
	auto p_combo = (CComboBox*)GetDlgItem(IDC_COMBO_PARAMETER);
	if (p_combo)
	{
		auto cur_sel = p_combo->GetCurSel();
		if (cur_sel >= 0)
		{
			return (eParameterId)p_combo->GetItemData(cur_sel);
		}
	}
	return (eParameterId)piInvalidParameter;
}

//----------------------------------------------------------------------------
void CSDOAQ_Dlg::BuildCalibrationFile_Combobox()
{
	// 1. make calib file list from current directory
	std::vector<CString> vsCalibList;
	m_calFile.GetCalibList(GetCurrentDir(), vsCalibList);
	//m_calFile.GetCalibList(FString(_T("%s\\%s"), GetCurrentDir(), _T("Calib")), vsCalibList);

	// 2. parse file
	m_calFile.calibData.clear();
	for (auto it = vsCalibList.begin(); it != vsCalibList.end(); it++)
	{
		m_calFile.BuildCalibData(*it);
	}

	auto p_combo = (CComboBox*)GetDlgItem(IDC_COMBO_OBJECTIVE);
	p_combo->ResetContent();
	for (auto it = m_calFile.calibData.begin(); it != m_calFile.calibData.end(); it++)
	{
		p_combo->AddString(it->objective);
	}
}

//----------------------------------------------------------------------------
void CSDOAQ_Dlg::ShowViewer(void)
{
	if (m_vhwndIV.size())
	{
		CRect rc;
		GetDlgItem(IDC_IMAGE)->GetWindowRect(rc);
		ScreenToClient(rc);
		const int cx = rc.Width() / 2;
		const int cy = rc.Height() / 2;

		std::vector<CRect> vrc;
		vrc.push_back(CRect(rc.left + 0, rc.top + 0, rc.left + cx, rc.top + cy));
		vrc.push_back(CRect(rc.left + cx, rc.top + 0, rc.right, rc.top + cy));
		vrc.push_back(CRect(rc.left + 0, rc.top + cy, rc.left + cx, rc.bottom));
		CRect rc3d(rc.left + cx, rc.top + cy, rc.right, rc.bottom);

		for (size_t uid = 0; uid < m_vhwndIV.size(); uid++)
		{
			::WSUT_IV_ShowWindow(m_vhwndIV[uid], (WSIOINT)true, vrc[uid].left, vrc[uid].top, vrc[uid].right, vrc[uid].bottom);
		}
		(void)::WSGL_ShowWindow(m_hwnd3D, (WSIOINT)true, rc3d.left, rc3d.top, rc3d.right, rc3d.bottom);
	}
}

//----------------------------------------------------------------------------
// If you add a minimize button to your dialog, you will need the code below
//  to draw the icon.  For MFC applications using the document/view model,
//  this is automatically done for you by the framework.
//----------------------------------------------------------------------------
void CSDOAQ_Dlg::OnPaint()
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
		CRect Rect;
		GetWindowRect(Rect);
		ScreenToClient(&Rect);
		InvalidateRect(Rect);

		CDialogEx::OnPaint();
	}
}

//----------------------------------------------------------------------------
void CSDOAQ_Dlg::OnSize(UINT nType, int cx, int cy)
{
	CDialogEx::OnSize(nType, cx, cy);

	ShowViewer();
}

//----------------------------------------------------------------------------
void CSDOAQ_Dlg::OnClose()
{
	(void)::SDOAQ_Finalize();

	for (auto it = m_vhwndIV.begin(); it != m_vhwndIV.end(); it++)
	{
		(void)::WSUT_IV_DestroyImageViewer(*it);
	}
	(void)::WSGL_Finalize(m_hwnd3D);

	CDialogEx::OnClose();
}

//----------------------------------------------------------------------------
void CSDOAQ_Dlg::OnDestroy()
{
	t_vmsgid vmsgid;
	vmsgid.push_back(EUM_LOG);
	vmsgid.push_back(EUM_ERROR);
	DeleteAllWinmsgWithWStringPtrInLparam(m_hWnd, vmsgid);

	__super::OnDestroy();
}

//----------------------------------------------------------------------------
// The system calls this function to obtain the cursor to display while the user drags
//  the minimized window.
//----------------------------------------------------------------------------
HCURSOR CSDOAQ_Dlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}

//---------------------------------------------------------------------------
void CSDOAQ_Dlg::OnTimer(UINT_PTR nIDEvent)
{
	switch (nIDEvent)
	{
	case TIMER_LOG:
		PrintLog();
		break;
	}

	__super::OnTimer(nIDEvent);
}

//----------------------------------------------------------------------------
BOOL CSDOAQ_Dlg::PreTranslateMessage(MSG* pMsg)
{
	// TODO: Add your specialized code here and/or call the base class
	if (pMsg->message == WM_KEYDOWN)
	{
		if (pMsg->wParam == VK_RETURN)
			return TRUE;
		else if (pMsg->wParam == VK_ESCAPE)
			return TRUE;

		else if (pMsg->wParam == VK_BACK)
		{
			if (::WSGL_IsOnRun(m_hwnd3D))
			{
				(void)::WSGL_ClearLastMeasureData(m_hwnd3D, true);
			}
		}
		else if (pMsg->wParam == VK_DELETE)
		{
			if (::WSGL_IsOnRun(m_hwnd3D))
			{
				(void)::WSGL_ClearAllMeasureData(m_hwnd3D, true);
			}
		}
		else if (pMsg->wParam == VK_HOME)
		{
			if (::WSGL_IsOnRun(m_hwnd3D))
			{
				(void)::WSGL_ResetDisplay(m_hwnd3D);
			}
		}
	}

	return CDialogEx::PreTranslateMessage(pMsg);
}

//----------------------------------------------------------------------------
void CSDOAQ_Dlg::Log(LPCTSTR p_log_str)
{
	CString sLogFileName = FString(_T("%s\\%s.log"), m_sLogPath, CTime::GetCurrentTime().Format("%Y.%m.%d"));
	DWORD dwDummy;
	if (m_sLogFileName != sLogFileName)
	{
		if (m_hLogFile != INVALID_HANDLE_VALUE)
		{
			CloseHandle(m_hLogFile);
		}

		m_hLogFile = CreateFile(sLogFileName, GENERIC_WRITE, FILE_SHARE_READ, NULL, OPEN_ALWAYS, FILE_ATTRIBUTE_NORMAL, NULL);
		m_sLogFileName = sLogFileName;
	}
	if (m_hLogFile != INVALID_HANDLE_VALUE)
	{
		SetFilePointer(m_hLogFile, 0, NULL, FILE_END);
		CString sLog = p_log_str + CString("\r\n");
		WriteFile(m_hLogFile, CStringA(sLog), CStringA(sLog).GetLength(), &dwDummy, NULL);
	}

	m_sLog += p_log_str;
	m_sLog += "\r\n";

	if (m_sLog.GetLength() > 4 * 64 * 1024)
	{
		m_sLog = m_sLog.Right(3 * 64 * 1024);
	}

	const DWORD LOG_PERIOD = 100;
	DWORD tickDiff = GetTickCount() - m_tickLastLog;
	if (tickDiff < LOG_PERIOD)
	{
		SetTimer(TIMER_LOG, LOG_PERIOD - tickDiff, NULL);
	}
	else
	{
		PrintLog();
	}
}

//----------------------------------------------------------------------------
void CSDOAQ_Dlg::PrintLog(void)
{
	KillTimer(TIMER_LOG);
	m_tickLastLog = GetTickCount();

	CEdit* const pWnd = (CEdit*)AfxGetMainWnd()->GetDlgItem(IDC_LOG);
	if (pWnd)
	{
		pWnd->SetWindowText(m_sLog);
		const int nLen = pWnd->GetWindowTextLength();
		pWnd->SetSel(nLen, nLen);
	}
}

//----------------------------------------------------------------------------
void CSDOAQ_Dlg::ApiError(LPCTSTR sApi, int eCode)
{
	Log(FString(_T("[API Error] >> %s() returns error %d (=%s)."), sApi, eCode, GetSdoaqErrorString(eCode)));
}

//----------------------------------------------------------------------------
LRESULT CSDOAQ_Dlg::OnUmLog(WPARAM wSeverity, LPARAM lpMessage)
{
	CString* pMessage = (CString*)lpMessage;
	if (pMessage)
	{
		LPCTSTR sSeverity;
		switch ((eLogSeverity)wSeverity)
		{
		case lsError:	sSeverity = _T("Error"); break;
		case lsWarning:	sSeverity = _T("Warning"); break;
		case lsInfo:	sSeverity = _T("Info"); break;
		case lsVerbose:	sSeverity = _T("Verbose"); break;
		case lsMeasure:	sSeverity = _T("Measure"); break;
		default:		sSeverity = _T("Unknown"); break;
		}
		Log(FString(_T("[LogCallback : %s] %s"), sSeverity, *pMessage));
		delete pMessage;
	}
	return 0;
}

//----------------------------------------------------------------------------
LRESULT CSDOAQ_Dlg::OnUmError(WPARAM wErrorCode, LPARAM lpMessage)
{
	CString* pMessage = (CString*)lpMessage;

	Log(FString(_T("[ErrorCallback : ErrorCode:%d] %s"), wErrorCode, pMessage ? *pMessage : _T("")));

	if (pMessage)
	{
		delete pMessage;
	}
	return 0;
}

//----------------------------------------------------------------------------
void CSDOAQ_Dlg::BuildEnvironment(void)
{
	m_sScriptFile = FString(_T("%s\\wisescope.script.txt"), GetCurrentDir());
	m_sLogPath = FString(_T("%s\\Log"), GetCurrentDir());

	(void)::CreateDirectory(m_sLogPath, NULL);

	const int nMajorVersion = ::SDOAQ_GetMajorVersion();
	const int nMinorVersion = ::SDOAQ_GetMinorVersion();
	const int nPatchVersion = ::SDOAQ_GetPatchVersion();

	SetWindowText(FString(_T("SDOAQ API TESTER (dll %d.%d.%d)"), nMajorVersion, nMinorVersion, nPatchVersion));

	Log(_T(">> =================================================="));
	Log(_T(">> Welcome to SDOAQ API Tester"));
	Log(_T(">> =================================================="));

	Log(FString(_T(">> SDOAQ dll version: %d.%d.%d"), nMajorVersion, nMinorVersion, nPatchVersion));

	if (m_sScriptFile.GetLength())
	{
		::SDOAQ_SetSystemScriptFilename((CStringA)m_sScriptFile);
		Log(FString(_T(">> Script file: %s"), m_sScriptFile));
	}
	Log(FString(_T(">> Log path: %s"), m_sLogPath));

	// set the cam files folder path
	char buf[1024];
	if (WSIORV_SUCCESS <= WSUT_GetInstalledPath_TypeA("SDOAQ", buf, 1024))
	{
		::SDOAQ_SetCamfilePath(FStringA("%s\\RUN\\CamFiles", (CStringA)buf));
	}
	else
	{
		::SDOAQ_SetCamfilePath(FStringA("%s\\..\\..\\Include\\SDOAQ\\CamFiles", (CStringA)GetCurrentDir()));
	}
}

//----------------------------------------------------------------------------
void CSDOAQ_Dlg::ReadySdoaqDll(void)
{
	BuildCalibrationFile_Combobox();
}

//----------------------------------------------------------------------------
LRESULT CSDOAQ_Dlg::OnInitDone(WPARAM wErrorCode, LPARAM lpMessage)
{
	CString* pMessage = (CString*)lpMessage;

	Log(FString(_T("[InitDoneCallback : ErrorCode:%d] %s"), wErrorCode, pMessage ? *pMessage : _T("")));

	if (ecNoError == wErrorCode)
	{
		const int nAlgoVersion = ::SDOAQ_GetAlgorithmVersion();
		Log(FString(_T(">> sdedof dll version: %d.%d"), nAlgoVersion / 1000, nAlgoVersion % 1000));

		GetDlgItem(IDC_INITIALIZE)->SetWindowText(_T("Initialized"));
		GetDlgItem(IDC_INITIALIZE)->EnableWindow(FALSE);

		ReadySdoaqDll();

		SET.m_nColorByte = IsMonoCameraInstalled() ? MONOBYTES : COLORBYTES;

		// It is appropriate that some of the items in the initial settings below are actually read from SDOAQ
		GetDlgItem(IDC_EDIT_ROI)->SetWindowText(_T("0,0,2040,1086"));
		GetDlgItem(IDC_EDIT_AFROI)->SetWindowText(_T("956,479,128,128"));
		GetDlgItem(IDC_EDIT_RING_BUF_SIZE)->SetWindowText(_T("3"));
		GetDlgItem(IDC_EDIT_FOCUS_SET)->SetWindowText(_T("0-319-36"));
		GetDlgItem(IDC_EDIT_SNAPFOCUS_SET)->SetWindowText(_T("0-319-18"));

		OnSdoaqSetROI();
		OnSdoaqSetAFROI();
		OnSdoaqSetRingBufSize();
		OnSdoaqSetFocusSet();
		OnSdoaqSetSnapFocusSet();
	}
	else
	{
		Log(_T("[WARNING] Abnormal operation may occur if initialization is executed again after initialization is completed."
			"Finalize first and then re-initialize."));
	}

	if (pMessage)
	{
		delete pMessage;
	}

	return 0;
}


//----------------------------------------------------------------------------
void CSDOAQ_Dlg::OnCheckSimulMode()
{
	bool flagSimulationMode = ((CButton*)GetDlgItem(IDC_CHECK_SIMUL_MODE))->GetCheck() == BST_CHECKED ? true : false;
	::SDOAQ_SetIntParameterValue(piFullSimulationMode, flagSimulationMode);

	// To apply simulation mode, SDOAQ must be re-initialized.
	OnSdoaqFinalize();
	OnSdoaqInitialize();
}

//----------------------------------------------------------------------------
void CSDOAQ_Dlg::OnSdoaqInitialize()
{
	SET.rb.active = false;

	eErrorCode rv_sdoaq = ::SDOAQ_Initialize(g_LogCallback, g_ErrorCallback, g_InitDoneCallback);
	if (ecNoError != rv_sdoaq)
	{
		ApiError(_T("SDOAQ_Initialize"), rv_sdoaq);
		return;
	}

	rv_sdoaq = ::SDOAQ_RegisterMoveokCallback(g_MoveokCallback);
	if (ecNoError != rv_sdoaq)
	{
		ApiError(_T("SDOAQ_RegisterMoveokCallback"), rv_sdoaq);
		return;
	}
}

//----------------------------------------------------------------------------
void CSDOAQ_Dlg::OnSdoaqFinalize()
{
	eErrorCode rv_sdoaq = ::SDOAQ_Finalize();
	if (ecNoError != rv_sdoaq)
	{
		ApiError(_T("SDOAQ_Finalize"), rv_sdoaq);
	}

	GetDlgItem(IDC_INITIALIZE)->SetWindowText(_T("Initialize"));
	GetDlgItem(IDC_INITIALIZE)->EnableWindow(TRUE);

	SET.rb.active = false;
	SET.ClearBuffer();
}

//----------------------------------------------------------------------------
void CSDOAQ_Dlg::OnSelectedCombobox()
{
	// The parameter id selected in the combo box
	eParameterId paraID = GetCurrentParameterID();

	bool flagAvailable = false;
	eErrorCode rv_sdoaq = ::SDOAQ_IsParameterAvailable(paraID, &flagAvailable);
	if (ecNoError == rv_sdoaq)
	{
		CString sValue;
		if (flagAvailable)
		{
			auto ERROR_STR = [&](eErrorCode rv_sdoaq) -> auto
			{
				switch (rv_sdoaq)
				{
				case ecNoError: return "";
				case ecNoWisescope: return "No camera";
				case ecNoLighting: return "No lighting";
				case ecParameterIsNotSet: return "Not set yet";
				default: return "Not supported";
				}
			};

			eParameterType eType;
			rv_sdoaq = ::SDOAQ_GetParameterType(paraID, &eType);
			if (ecNoError == rv_sdoaq)
			{
				if (eType == ptInt)
				{
					int nValue;
					eErrorCode rv_sdoaq = ::SDOAQ_GetIntParameterValue(paraID, &nValue);
					if (ecNoError == rv_sdoaq)
					{
						if (paraID == piReflexCorrectionPattern)
						{
							sValue.Format(_T("0x%x"), nValue);
						}
						else
						{
							sValue.Format(_T("%d"), nValue);
						}
					}
					else
					{
						sValue = ERROR_STR(rv_sdoaq);
						ApiError(FString(_T("SDOAQ_GetIntParameterValue[ParamID-%d]"), paraID), rv_sdoaq);
					}
				}
				else if (eType == ptDouble)
				{
					double dbValue;
					eErrorCode rv_sdoaq = ::SDOAQ_GetDblParameterValue(paraID, &dbValue);
					if (ecNoError == rv_sdoaq)
					{
						sValue.Format(_T("%.3lf"), dbValue);
					}
					else
					{
						sValue = ERROR_STR(rv_sdoaq);
						ApiError(FString(_T("SDOAQ_GetDblParameterValue[ParamID-%d]"), paraID), rv_sdoaq);
					}
				}
				else if (eType == ptString)
				{
					char buf[256];
					int size = sizeof(buf);
					eErrorCode rv_sdoaq = ::SDOAQ_GetStringParameterValue(paraID, buf, &size);
					if (ecNoError == rv_sdoaq)
					{
						sValue = buf;
					}
					else
					{
						sValue = ERROR_STR(rv_sdoaq);
						ApiError(FString(_T("SDOAQ_GetStringParameterValue[ParamID-%d]"), paraID), rv_sdoaq);
					}
				}
			}
			else
			{
				sValue = ERROR_STR(rv_sdoaq);
				ApiError(FString(_T("SDOAQ_GetParameterType[ParamID-%d]"), paraID), rv_sdoaq);
			}
		}
		else
		{
			sValue = _T("Not available");
		}
		GetDlgItem(IDC_EDIT_VALUE)->SetWindowText(sValue);
	}
	else
	{
		ApiError(FString(_T("SDOAQ_IsParameterAvailable[ParamID-%d]"), paraID), rv_sdoaq);
	}

	// Disable items that are not allowed to be written
	bool flagWritable = false;
	rv_sdoaq = ::SDOAQ_IsParameterWritable(paraID, &flagWritable);
	if (ecNoError == rv_sdoaq)
	{
		const BOOL WRITABLE = flagWritable ? TRUE : FALSE;
		GetDlgItem(IDC_EDIT_VALUE)->EnableWindow(WRITABLE);
		GetDlgItem(IDC_SET_PARAMETER)->EnableWindow(WRITABLE);
	}
	else
	{
		ApiError(FString(_T("SDOAQ_IsParameterWritable[ParamID-%d]"), paraID), rv_sdoaq);
	}
}

//----------------------------------------------------------------------------
void CSDOAQ_Dlg::OnSdoaqSetParameter()
{
	// The parameter id selected in the combo box
	eParameterId paraID = GetCurrentParameterID();

	bool flagAvailable = false;
	eErrorCode rv_sdoaq = ::SDOAQ_IsParameterAvailable(paraID, &flagAvailable);
	if (ecNoError == rv_sdoaq)
	{
		bool flagWritable = false;
		rv_sdoaq = ::SDOAQ_IsParameterWritable(paraID, &flagWritable);
		if (ecNoError == rv_sdoaq)
		{
			if (flagAvailable && flagWritable)
			{
				eParameterType eType;
				rv_sdoaq = ::SDOAQ_GetParameterType(paraID, &eType);
				if (ecNoError == rv_sdoaq)
				{
					CString sParameters;
					GetDlgItemText(IDC_EDIT_VALUE, sParameters);

					if (eType == ptInt)
					{
						sParameters.MakeLower();

						int nValue = 0;
						if (sParameters.Find(_T("x")) != -1
							&& paraID == piReflexCorrectionPattern
							)
						{
							swscanf_s(sParameters, L"%x", &nValue);
						}
						else
						{
							nValue = _ttoi(sParameters);
						}

						int nMin, nMax;
						rv_sdoaq = ::SDOAQ_GetIntParameterRange(paraID, &nMin, &nMax);
						if (ecNoError == rv_sdoaq)
						{
							if (nValue >= nMin && nValue <= nMax)
							{
								(void)::SDOAQ_SetIntParameterValue(paraID, nValue);
							}
							else
							{
								ApiError(FString(_T("SDOAQ_SetIntParameterValue[ParamID-%d] : value is out of range[%d ~ %d]"), paraID, nMin, nMax), ecUnknownError);
							}
						}
						else
						{
							ApiError(FString(_T("SDOAQ_GetIntParameterRange[ParamID-%d]"), paraID), rv_sdoaq);
						}
					}
					else if (eType == ptDouble)
					{
						double dbValue = _ttof(sParameters);
						double dbMin, dbMax;

						rv_sdoaq = ::SDOAQ_GetDblParameterRange(paraID, &dbMin, &dbMax);
						if (ecNoError == rv_sdoaq)
						{
							if (dbValue >= dbMin && dbValue <= dbMax)
							{
								(void)::SDOAQ_SetDblParameterValue(paraID, dbValue);
							}
							else
							{
								ApiError(FString(_T("SDOAQ_SetDblParameterValue[ParamID-%d] : value is out of range[%.1lf ~ %.1lf]"), paraID, dbMin, dbMax), ecUnknownError);
							}
						}
						else
						{
							ApiError(FString(_T("SDOAQ_GetDblParameterRange[ParamID-%d]"), paraID), rv_sdoaq);
						}
					}
					else if (eType == ptString)
					{
						(void)::SDOAQ_SetStringParameterValue(paraID, (CStringA)sParameters.GetBuffer());
					}
				}
				else
				{
					ApiError(FString(_T("SDOAQ_GetParameterType[ParamID-%d]"), paraID), rv_sdoaq);
				}
			}
		}
		else
		{
			ApiError(FString(_T("SDOAQ_IsParameterWritable[ParamID-%d]"), paraID), rv_sdoaq);
		}
	}
	else
	{
		ApiError(FString(_T("SDOAQ_IsParameterAvailable[ParamID-%d]"), paraID), rv_sdoaq);
	}
}

//----------------------------------------------------------------------------
void CSDOAQ_Dlg::OnSdoaqSetROI()
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

	int nDummy;
	auto rv_sdoaq = ::SDOAQ_GetIntParameterRange(piCameraFullFrameSizeX, &nDummy, &nMaxWidth);
	if (ecNoError == rv_sdoaq)
	{
		if (AFP.cameraRoiLeft < 0 || AFP.cameraRoiLeft > nMaxWidth)
		{
			ApiError(FString(_T("Set cameraRoiLeft : value is out of range[ ~ %d]"), nMaxWidth), ecUnknownError);
			return;
		}
		if (AFP.cameraRoiWidth < 1 || AFP.cameraRoiWidth > nMaxWidth)
		{
			ApiError(FString(_T("Set cameraRoiWidth : value is out of range[ ~ %d]"), nMaxWidth), ecUnknownError);
			return;
		}
	}
	else
	{
		ApiError(_T("SDOAQ_GetIntParameterRange[piCameraFullFrameSizeX]"), rv_sdoaq);
		return;
	}

	rv_sdoaq = ::SDOAQ_GetIntParameterRange(piCameraFullFrameSizeY, &nDummy, &nMaxHeight);
	if (ecNoError == rv_sdoaq)
	{
		if (AFP.cameraRoiTop < 0 || AFP.cameraRoiTop > nMaxHeight)
		{
			ApiError(FString(_T("Set cameraRoiTop : value is out of range[ ~ %d]"), nMaxHeight), ecUnknownError);
			return;
		}
		if (AFP.cameraRoiHeight < 1 || AFP.cameraRoiHeight > nMaxHeight)
		{
			ApiError(FString(_T("Set cameraRoiHeight : value is out of range[ ~ %d]"), nMaxHeight), ecUnknownError);
			return;
		}
	}
	else
	{
		ApiError(_T("SDOAQ_GetIntParameterRange[piCameraFullFrameSizeY]"), rv_sdoaq);
		return;
	}

	if (!SET.rb.active)
	{
		SET.afp = AFP;
	}
}

//----------------------------------------------------------------------------
void CSDOAQ_Dlg::OnSdoaqSetAFROI()
{
	CString sParameters;
	GetDlgItemText(IDC_EDIT_AFROI, sParameters);

	CString sLeft, sTop, sWidth, sHeight;
	AfxExtractSubString(sLeft, sParameters, 0, ',');
	AfxExtractSubString(sTop, sParameters, 1, ',');
	AfxExtractSubString(sWidth, sParameters, 2, ',');
	AfxExtractSubString(sHeight, sParameters, 3, ',');

	CRect recAF(_ttoi(sLeft), _ttoi(sTop), _ttoi(sLeft) + _ttoi(sWidth), _ttoi(sTop) + _ttoi(sHeight));
	eErrorCode rv_sdoaq = SetSdoaqFocusRect(recAF);
	if (ecNoError != rv_sdoaq)
	{
		ApiError(FString(_T("SDOAQ_SetIntParameterValue[ParamID-%d,%d]"), piFocusLeftTop, piFocusRightBottom), rv_sdoaq);
	}
}

//----------------------------------------------------------------------------
void CSDOAQ_Dlg::OnSdoaqSetRingBufSize()
{
	CString sSize;
	GetDlgItemText(IDC_EDIT_RING_BUF_SIZE, sSize);

	m_nRingBufferSize = max(1, _ttoi(sSize));
}

//----------------------------------------------------------------------------
void CSDOAQ_Dlg::OnSdoaqSetFocusSet()
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

	if (m_vFocusSet.size() == 0)
	{
		m_vFocusSet.push_back(DFLT_FOCUS_STEP);
	}
}

//----------------------------------------------------------------------------
void CSDOAQ_Dlg::OnSdoaqSetSnapFocusSet()
{
	CString sSnapFocusSet;
	GetDlgItemText(IDC_EDIT_SNAPFOCUS_SET, sSnapFocusSet);

	m_vSnapFocusSet.clear();

	if (sSnapFocusSet.Find(_T("-")) != -1)
	{
		int nLow, nHigh, nUnit;
		if (3 <= swscanf_s((LPCTSTR)sSnapFocusSet, _T("%d-%d-%d"), &nLow, &nHigh, &nUnit))
		{
			for (int nFocus = nLow; nFocus <= nHigh; nFocus += nUnit)
			{
				m_vSnapFocusSet.push_back(nFocus);
			}
		}
	}
	else
	{
		int posSeek = 0;
		do
		{
			CString sSeek = sSnapFocusSet.Mid(posSeek);
			int nFocus;
			if (1 <= swscanf_s((LPCTSTR)sSeek, _T("%d"), &nFocus))
			{
				m_vSnapFocusSet.push_back(nFocus);
			}
		} while (-1 != (posSeek = sSnapFocusSet.Find(_T(" "), posSeek + 1)));
	}

	if (m_vSnapFocusSet.size() == 0)
	{
		m_vSnapFocusSet.push_back(DFLT_FOCUS_STEP);
	}
}

//============================================================================
// PLAY FUNCTIONs
//----------------------------------------------------------------------------
void CSDOAQ_Dlg::OnSdoaqSingleShotStack()
{
	(void)::WSGL_Display_BG(m_hwnd3D);

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
	const LPCTSTR sz_api = _T("SDOAQ_SingleShotFocusStackEx");
	const eErrorCode rv_sdoaq = ::SDOAQ_SingleShotFocusStackEx(
		&AFP,
		pPositions, (int)FOCUS.numsFocus,
		ppFocusImages, pFocusImageBufferSizes
	);

	if (ecNoError == rv_sdoaq)
	{
		const auto tick_end = GetTickCount64();
		//Log(FString(_T(">> %s() takes : %llu ms / %d imgs"), sz_api, tick_end - tick_begin, FOCUS.numsFocus));

		++m_nContiStack;
		for (size_t uid = 0; uid < m_vhwndIV.size(); uid++)
		{
			if (uid < FOCUS.numsFocus)
			{
				auto pos = uid;
				if (uid == m_vhwndIV.size() - 1)
				{
					// last window -> last position
					pos = FOCUS.numsFocus - 1;
				}
				WSIOCHAR title[256];
				sprintf_s(title, sizeof title, "Zstack(%d)", pPositions[pos]);
				ImageViewer(uid, title, m_nContiStack, SET, ppFocusImages[pos]);
			}
			else
			{
				ImageViewer(uid);
			}
		}
	}
	else
	{
		ApiError(sz_api, rv_sdoaq);
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
void CSDOAQ_Dlg::OnSdoaqPlayStack()
{
	(void)::WSGL_Display_BG(m_hwnd3D);

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
	for (size_t pos = 0; pos < FOCUS.numsFocus; pos++)
	{
		pPositions[pos] = FOCUS.vFocusSet[pos];
	}

	if (SET.rb.ppBuf)
	{
		SET.ClearBuffer();
	}
	SET.rb.numsBuf = FOCUS.numsFocus * m_nRingBufferSize;
	SET.rb.ppBuf = (void**)new unsigned char*[SET.rb.numsBuf];
	SET.rb.pSizes = new size_t[SET.rb.numsBuf];
	auto size = SET.ImgSize();

	for (size_t uidx = 0; uidx < SET.rb.numsBuf; uidx++)
	{
		SET.rb.ppBuf[uidx] = (void*)new unsigned char[size];
		SET.rb.pSizes[uidx] = size;
	}

	AFP.callbackUserData = (void*)::GetTickCount64();
	const LPCTSTR sz_api = _T("SDOAQ_PlayFocusStackEx");
	const eErrorCode rv_sdoaq = ::SDOAQ_PlayFocusStackEx(
		&AFP,
		g_PlayFocusStackCallbackEx,
		pPositions, (int)FOCUS.numsFocus,
		m_nRingBufferSize, (unsigned char**)SET.rb.ppBuf, SET.rb.pSizes);

	if (ecNoError == rv_sdoaq)
	{
		SET.rb.active = true;
	}
	else
	{
		ApiError(sz_api, rv_sdoaq);
	}

	delete[] pPositions;
}

//----------------------------------------------------------------------------
void CSDOAQ_Dlg::OnSdoaqStopStack()
{
	SET.rb.active = false;

	const LPCTSTR sz_api = _T("SDOAQ_StopFocusStack");
	const eErrorCode rv_sdoaq = ::SDOAQ_StopFocusStack();
	if (ecNoError != rv_sdoaq)
	{
		ApiError(sz_api, rv_sdoaq);
	}

	SET.ClearBuffer();
}

//----------------------------------------------------------------------------
LRESULT CSDOAQ_Dlg::OnReceiveZstack(WPARAM wErrorCode, LPARAM lLastFilledRingBufferEntry)
{
	(void)::WSGL_Display_BG(m_hwnd3D);

	if (ecNoError != wErrorCode)
	{
		ApiError(_T("SDOAQ_PlayCallbackEx"), (int)wErrorCode);
	}
	else if (SET.rb.active)
	{
		if (m_nRingBufferSize == 1)
		{
			SET.rb.active = false;
		}

		(void)UpdateLastMessage(m_hWnd, EUM_RECEIVE_ZSTACK, wErrorCode, lLastFilledRingBufferEntry);

		auto AFP = SET.afp;
		auto& FOCUS = SET.focus;

		auto nFocusNums = FOCUS.numsFocus;
		const int base_order = (lLastFilledRingBufferEntry % (int)SET.rb.numsBuf)*(int)nFocusNums; //m_nRingBufferSize		

		++m_nContiStack;
		for (size_t uid = 0; uid < m_vhwndIV.size(); uid++)
		{
			if (uid < nFocusNums)
			{
				auto pos = uid;
				if (uid == m_vhwndIV.size() - 1)
				{
					// last window -> last position
					pos = nFocusNums - 1;
				}
				WSIOCHAR title[256];
				sprintf_s(title, sizeof title, "Zstack(%d)", FOCUS.vFocusSet[pos]);
				ImageViewer(uid, title, m_nContiStack, SET, SET.rb.ppBuf[base_order + uid]);
			}
			else
			{
				ImageViewer(uid);
			}
		}
	}

	return 0;
}

//----------------------------------------------------------------------------
void CSDOAQ_Dlg::OnSdoaqPlaySingleFocus()
{
	(void)::WSGL_Display_BG(m_hwnd3D);

	if (SET.rb.active)
	{
		return;
	}

	auto& AFP = SET.afp;
	auto& FOCUS = SET.focus;

	if (SET.rb.ppBuf)
	{
		SET.ClearBuffer();
	}
	SET.rb.numsBuf = FOCUS.numsFocus * m_nRingBufferSize;
	SET.rb.ppBuf = (void**)new unsigned char*[SET.rb.numsBuf];
	SET.rb.pSizes = new size_t[SET.rb.numsBuf];
	auto size = SET.ImgSize();

	for (size_t uidx = 0; uidx < SET.rb.numsBuf; uidx++)
	{
		SET.rb.ppBuf[uidx] = (void*)new unsigned char[size];
		SET.rb.pSizes[uidx] = size;
	}

	AFP.callbackUserData = (void*)::GetTickCount64();
	const LPCTSTR sz_api = _T("SDOAQ_PlaySingleFocusEx");
	const eErrorCode rv_sdoaq = ::SDOAQ_PlaySingleFocusEx(&AFP, g_PlayFocusSingleFocusCallbackEx, m_nRingBufferSize, (unsigned char**)SET.rb.ppBuf, SET.rb.pSizes);

	if (ecNoError == rv_sdoaq)
	{
		SET.rb.active = true;
	}
	else
	{
		ApiError(sz_api, rv_sdoaq);
	}
}

//----------------------------------------------------------------------------
void CSDOAQ_Dlg::OnSdoaqStopSingleFocus()
{
	OnSdoaqStopStack();
}

//----------------------------------------------------------------------------
LRESULT CSDOAQ_Dlg::OnReceiveSingleFocus(WPARAM wErrorCode, LPARAM lLastFilledRingBufferEntry)
{
	(void)::WSGL_Display_BG(m_hwnd3D);

	if (ecNoError != wErrorCode)
	{
		ApiError(_T("SDOAQ_PlayCallbackEx"), (int)wErrorCode);
	}
	else if (SET.rb.active)
	{		
		(void)UpdateLastMessage(m_hWnd, EUM_RECEIVE_SINGLE_FOCUS, wErrorCode, lLastFilledRingBufferEntry);

		const int base_order = (lLastFilledRingBufferEntry % (int)SET.rb.numsBuf); //m_nRingBufferSize		

		++m_nContiStack;						
		WSIOCHAR title[256];
		int nFocus;
		(void)::SDOAQ_GetIntParameterValue(piSingleFocus, &nFocus);
		sprintf_s(title, sizeof title, "Single Focus(%d)", nFocus);
		ImageViewer(0, title, m_nContiStack, SET, SET.rb.ppBuf[base_order + 0]);
	}

	return 0;
}

//----------------------------------------------------------------------------
void CSDOAQ_Dlg::OnSdoaqSingleShotEdof()
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
	for (size_t pos = 0; pos < FOCUS.numsFocus; pos++)
	{
		pPositions[pos] = FOCUS.vFocusSet[pos];
	}

	float* pStepMapImageBuffer = new float[SET.PixelSize()];
	unsigned char* pEdofImageBuffer = new unsigned char[SET.ImgSize()];
	float* pQualityMapBuffer = new float[SET.PixelSize()];
	float* pHeightMapBuffer = new float[SET.PixelSize()];
	float* pPointCloudBuffer = new float[SET.PixelSize() * 3];

	size_t stepMapBufferSize = SET.DataSize();
	size_t edofImageBufferSize = SET.ImgSize();
	size_t qualityMapBufferSize = SET.DataSize();
	size_t heightMapBufferSize = SET.DataSize();
	size_t pointCloudBufferSize = SET.DataSize() * 3;

	if (((CButton*)GetDlgItem(IDC_CHECK_STEPMAP))->GetCheck() == BST_UNCHECKED)
	{
		stepMapBufferSize = 0;
	}
	if (((CButton*)GetDlgItem(IDC_CHECK_EDOF))->GetCheck() == BST_UNCHECKED)
	{
		edofImageBufferSize = 0;
	}
	if (((CButton*)GetDlgItem(IDC_CHECK_QUALITYMAP))->GetCheck() == BST_UNCHECKED)
	{
		qualityMapBufferSize = 0;
	}
	if (((CButton*)GetDlgItem(IDC_CHECK_HEIGHTMAP))->GetCheck() == BST_UNCHECKED)
	{
		heightMapBufferSize = 0;
	}
	if (((CButton*)GetDlgItem(IDC_CHECK_POINTCLOUD))->GetCheck() == BST_UNCHECKED)
	{
		pointCloudBufferSize = 0;
	}

	const auto tick_begin = GetTickCount64();
	AFP.callbackUserData = (void*)::GetTickCount64();
	const LPCTSTR sz_api = _T("SDOAQ_SingleShotEdofEx");
	const eErrorCode rv_sdoaq = ::SDOAQ_SingleShotEdofEx(
		&AFP,
		pPositions, (int)FOCUS.numsFocus,
		pStepMapImageBuffer, stepMapBufferSize,
		pEdofImageBuffer, edofImageBufferSize,
		pQualityMapBuffer, qualityMapBufferSize,
		pHeightMapBuffer, heightMapBufferSize,
		pPointCloudBuffer, pointCloudBufferSize
	);

	if (ecNoError == rv_sdoaq)
	{
		const auto tick_end = GetTickCount64();
		//Log(FString(_T(">> %s() takes : %llu ms / %d imgs"), sz_api, tick_end - tick_begin, FOCUS.numsFocus));

		++m_nContiEdof;
		if (pEdofImageBuffer && edofImageBufferSize)
		{
			ImageViewer(0, "EDoF", m_nContiEdof, SET, pEdofImageBuffer);
		}
		else
		{
			ImageViewer(0, "EDoF", m_nContiEdof);
		}

		FloatViewer(pStepMapImageBuffer && stepMapBufferSize, 1, "StepMAP (not provided when using CUDA)", m_nContiEdof, SET, pStepMapImageBuffer);
		//FloatViewer(pQualityMapBuffer && qualityMapBufferSize, 2, "QualityMAP", m_nContiEdof, SET, pQualityMapBuffer);
		FloatViewer(pHeightMapBuffer && heightMapBufferSize, 2, "HeightMAP", m_nContiEdof, SET, pHeightMapBuffer);
		Viewer3D(pPointCloudBuffer && pointCloudBufferSize, SET, pPointCloudBuffer, pEdofImageBuffer);
	}
	else
	{
		ApiError(sz_api, rv_sdoaq);
	}

	delete[] pStepMapImageBuffer;
	delete[] pEdofImageBuffer;
	delete[] pQualityMapBuffer;
	delete[] pHeightMapBuffer;
	delete[] pPointCloudBuffer;
	delete[] pPositions;
}

//----------------------------------------------------------------------------
void CSDOAQ_Dlg::OnSdoaqPlayEdof()
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
	for (size_t pos = 0; pos < FOCUS.numsFocus; pos++)
	{
		pPositions[pos] = FOCUS.vFocusSet[pos];
	}

	if (SET.rb.ppBuf)
	{
		SET.ClearBuffer();
	}

	size_t edofImageBufferSize = SET.ImgSize();
	size_t stepMapBufferSize = SET.DataSize();
	size_t qualityMapBufferSize = SET.DataSize();
	size_t heightMapBufferSize = SET.DataSize();
	size_t pointCloudBufferSize = SET.DataSize() * 3;

	if (((CButton*)GetDlgItem(IDC_CHECK_STEPMAP))->GetCheck() == BST_UNCHECKED)
	{
		stepMapBufferSize = 0;
	}
	if (((CButton*)GetDlgItem(IDC_CHECK_EDOF))->GetCheck() == BST_UNCHECKED)
	{
		edofImageBufferSize = 0;
	}
	if (((CButton*)GetDlgItem(IDC_CHECK_QUALITYMAP))->GetCheck() == BST_UNCHECKED)
	{
		qualityMapBufferSize = 0;
	}
	if (((CButton*)GetDlgItem(IDC_CHECK_HEIGHTMAP))->GetCheck() == BST_UNCHECKED)
	{
		heightMapBufferSize = 0;
	}
	if (((CButton*)GetDlgItem(IDC_CHECK_POINTCLOUD))->GetCheck() == BST_UNCHECKED)
	{
		pointCloudBufferSize = 0;
	}

	SET.rb.numsBuf = EDOFRECSIZE * m_nRingBufferSize;
	SET.rb.ppBuf = (void**)new unsigned char*[SET.rb.numsBuf];
	SET.rb.pSizes = new size_t[SET.rb.numsBuf];

	for (size_t uidx = 0; uidx + EDOFRECSIZE - 1 < SET.rb.numsBuf;)
	{
		SET.rb.ppBuf[uidx] = edofImageBufferSize ? (void*)new unsigned char[edofImageBufferSize] : NULL;
		SET.rb.pSizes[uidx] = edofImageBufferSize;
		uidx++; // EDOF

		SET.rb.ppBuf[uidx] = stepMapBufferSize ? (void*)new float[stepMapBufferSize / sizeof(float)] : NULL;
		SET.rb.pSizes[uidx] = stepMapBufferSize;
		uidx++; // StepMap

		SET.rb.ppBuf[uidx] = qualityMapBufferSize ? (void*)new float[qualityMapBufferSize / sizeof(float)] : NULL;
		SET.rb.pSizes[uidx] = qualityMapBufferSize;
		uidx++; // QualityMap

		SET.rb.ppBuf[uidx] = heightMapBufferSize ? (void*)new float[heightMapBufferSize / sizeof(float)] : NULL;
		SET.rb.pSizes[uidx] = heightMapBufferSize;
		uidx++; // HeightMap

		SET.rb.ppBuf[uidx] = pointCloudBufferSize ? (void*)new float[pointCloudBufferSize / sizeof(float)] : NULL;
		SET.rb.pSizes[uidx] = pointCloudBufferSize;
		uidx++; // PointCloud
	}

	AFP.callbackUserData = (void*)::GetTickCount64();
	const LPCTSTR sz_api = _T("SDOAQ_PlayEdofEx");
	const eErrorCode rv_sdoaq = ::SDOAQ_PlayEdofEx(
		&AFP,
		g_PlayEdofCallbackEx,
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
		ApiError(sz_api, rv_sdoaq);
	}

	delete[] pPositions;
}

//----------------------------------------------------------------------------
void CSDOAQ_Dlg::OnSdoaqStopEdof()
{
	SET.rb.active = false;

	const LPCTSTR sz_api = _T("SDOAQ_StopEdof");
	const eErrorCode rv_sdoaq = ::SDOAQ_StopEdof();
	if (ecNoError != rv_sdoaq)
	{
		ApiError(sz_api, rv_sdoaq);
	}

	SET.ClearBuffer();
}

//----------------------------------------------------------------------------
LRESULT CSDOAQ_Dlg::OnReceiveEdof(WPARAM wErrorCode, LPARAM lLastFilledRingBufferEntry)
{
	if (ecNoError != wErrorCode)
	{
		ApiError(_T("SDOAQ_PlayCallback"), (int)wErrorCode);
	}
	else if (SET.rb.active)
	{
		if (m_nRingBufferSize == 1)
		{
			SET.rb.active = false;
		}

		(void)UpdateLastMessage(m_hWnd, EUM_RECEIVE_EDOF, wErrorCode, lLastFilledRingBufferEntry);

		auto AFP = SET.afp;
		const int base_order = (lLastFilledRingBufferEntry % (int)SET.rb.numsBuf)*EDOFRECSIZE; //m_nRingBufferSize

		++m_nContiEdof;
		ImageViewer(0, "EDoF", m_nContiEdof, SET, SET.rb.ppBuf[base_order + 0]);
		FloatViewer(true, 1, "StepMAP (not provided when using CUDA)", m_nContiEdof, SET, SET.rb.ppBuf[base_order + 1]);
		//FloatViewer(true, 2, "QualityMAP", m_nContiEdof, SET, SET.rb.ppBuf[base_order + 2]);
		FloatViewer(true, 2, "HeightMAP", m_nContiEdof, SET, SET.rb.ppBuf[base_order + 3]);
		Viewer3D(true, SET, SET.rb.ppBuf[base_order + 4], SET.rb.ppBuf[base_order + 0]);
	}

	return 0;
}

//----------------------------------------------------------------------------
void CSDOAQ_Dlg::OnSdoaqSingleShotAF()
{
	(void)::WSGL_Display_BG(m_hwnd3D);

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
	for (size_t pos = 0; pos < FOCUS.numsFocus; pos++)
	{
		pPositions[pos] = m_vFocusSet[pos];
	}

	size_t AFImageBufferSize = SET.ImgSize();
	unsigned char* pAFImageBuffer = new unsigned char[AFImageBufferSize];
	double dbBestFocusStep;
	double dbScore;
	double dbMatchedFocusStep;

	AFP.callbackUserData = (void*)::GetTickCount64();
	const LPCTSTR sz_api = _T("SDOAQ_SingleShotAFEx");
	const eErrorCode rv_sdoaq = ::SDOAQ_SingleShotAFEx(
		&AFP,
		pPositions, (int)FOCUS.numsFocus,
		pAFImageBuffer,
		AFImageBufferSize,
		&dbBestFocusStep,
		&dbScore,
		&dbMatchedFocusStep
	);
	if (ecNoError == rv_sdoaq)
	{
		++m_nContiAF;

		if (pAFImageBuffer && AFImageBufferSize)
		{
			ImageViewer(0, "AF", m_nContiAF, SET, pAFImageBuffer);
			Log(FString(_T("\t>> Best focus step : %.4lf, Score : %.4lf, Matched step : %d"), dbBestFocusStep, dbScore, (int)(dbMatchedFocusStep + 0.5)));
		}
		else
		{
			ImageViewer(0, "AF", m_nContiAF);
		}
	}
	else
	{
		ApiError(sz_api, rv_sdoaq);
	}

	delete[] pAFImageBuffer;
	delete[] pPositions;
}

//----------------------------------------------------------------------------
void CSDOAQ_Dlg::OnSdoaqPlayAF()
{
	(void)::WSGL_Display_BG(m_hwnd3D);

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
	for (size_t pos = 0; pos < FOCUS.numsFocus; pos++)
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
	const LPCTSTR sz_api = _T("SDOAQ_PlayAFEx");
	const eErrorCode rv_sdoaq = ::SDOAQ_PlayAFEx(
		&AFP,
		g_PlayAFCallbackEx2,
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
		ApiError(sz_api, rv_sdoaq);
	}

	delete[] pPositions;
}

//----------------------------------------------------------------------------
void CSDOAQ_Dlg::OnSdoaqStopAF()
{
	SET.rb.active = false;

	const LPCTSTR sz_api = _T("SDOAQ_StopAF");
	const eErrorCode rv_sdoaq = ::SDOAQ_StopAF();
	if (ecNoError != rv_sdoaq)
	{
		ApiError(sz_api, rv_sdoaq);
	}

	SET.ClearBuffer();
}

//----------------------------------------------------------------------------
LRESULT CSDOAQ_Dlg::OnReceiveAF(WPARAM wErrorCode, LPARAM lMsgParaReceiveAf)
{
	(void)::WSGL_Display_BG(m_hwnd3D);

	tMsgParaReceiveAf ParaAF;
	RetrievePointerBlock(ParaAF, lMsgParaReceiveAf);

	if (ecNoError != wErrorCode)
	{
		ApiError(_T("SDOAQ_PlayAfCallbackEx2"), (int)wErrorCode);
	}
	else if (SET.rb.active)
	{
		if (m_nRingBufferSize == 1)
		{
			SET.rb.active = false;
		}

		auto vRemovedMsg = UpdateLastMessage(m_hWnd, EUM_RECEIVE_AF, wErrorCode, lMsgParaReceiveAf);
		for (auto it = vRemovedMsg.begin(); it != vRemovedMsg.end(); it++)
		{
			RetrievePointerBlock(ParaAF, it->lParam);
		}

		const int base_order = (ParaAF.lastFilledRingBufferEntry % (int)SET.rb.numsBuf);
		++m_nContiAF;

		ImageViewer(0, "AF", m_nContiAF, SET, (BYTE*)SET.rb.ppBuf[base_order + 0]);
		Log(FString(_T("[Best Focus Step : %.4lf,\tScore : %.4lf]"), ParaAF.dbBestFocusStep, ParaAF.dbScore));
	}

	return 0;
}

//----------------------------------------------------------------------------
void CSDOAQ_Dlg::OnSdoaqSnap()
{
	if (SET.rb.active)
	{
		auto nFocusNums = m_vSnapFocusSet.size();
		int* pPositions = new int[nFocusNums];
		for (int pos = 0; pos < nFocusNums; pos++)
		{
			pPositions[pos] = m_vSnapFocusSet[pos];
		}

		TCHAR currentDir[MAX_PATH];
		::GetCurrentDirectory(MAX_PATH, currentDir);

		CString sCurrentTime;
		sCurrentTime.Format(CTime::GetCurrentTime().Format("%Y.%h.%d. %Hh %Mm %Ss"));
		CString sSnapPath;
		sSnapPath.Format(_T("%s\\Snap\\%s"), currentDir, sCurrentTime);

		CStringA sSnapPathA = (CStringA)sSnapPath;
		SnapParameters snap_para;
		snap_para.version = (void*)2;
		snap_para.v2.sSnapPath = sSnapPathA;
		snap_para.v2.sConfigFilename = NULL;
		snap_para.v2.sConfigData = NULL;

		const auto callbackUserData = (void*)::GetTickCount64();
		const eErrorCode rv_sdoaq = ::SDOAQ_PlaySnapEx(g_SnapCallbackEx, callbackUserData, pPositions, (int)nFocusNums, &snap_para);
		if (ecNoError != rv_sdoaq)
		{
			ApiError(_T("SDOAQ_PlaySnapEx"), rv_sdoaq);
		}

		delete[] pPositions;
	}
	else
	{
		Log(_T(">> Snap is only valid while acquisition is in progress!"));
	}
}

//------------------------------------------------------------------------------------------------
LRESULT CSDOAQ_Dlg::OnReceiveSnap(WPARAM wErrorCode, LPARAM lLastFilledRingBufferEntry)
{
	if (ecNoError != wErrorCode)
	{
		ApiError(_T("SDOAQ_SnapCallbackEx"), (int)wErrorCode);
	}
	else if (SET.rb.active)
	{
		(void)UpdateLastMessage(m_hWnd, EUM_RECEIVE_SNAP, wErrorCode, lLastFilledRingBufferEntry);
	}

	return 0;
}

//----------------------------------------------------------------------------
// Note: Set up the calibration file after initialization is complete
//----------------------------------------------------------------------------
void CSDOAQ_Dlg::OnSdoaqSetCalibrationFile()
{
	CString sFilter = _T("calibration file (*.csv)|*.csv|");
	CFileDialog dlg(TRUE, _T("cvs"), NULL, OFN_HIDEREADONLY | OFN_OVERWRITEPROMPT, sFilter);
	dlg.m_ofn.lpstrInitialDir = GetCurrentDir();
	if (dlg.DoModal() == IDOK)
	{
		eErrorCode rv_sdoaq = ::SDOAQ_SetCalibrationFile(CT2A(dlg.GetPathName().GetBuffer()));
		if (ecNoError != rv_sdoaq)
		{
			ApiError(_T("SDOAQ_SetCalibrationFile"), rv_sdoaq);
			return;
		}

		Log(FString(_T(">> Calibration file [%s] is set"), dlg.GetPathName()));
	}
}

//----------------------------------------------------------------------------
// Note: Set up the calibration file after initialization is complete
//----------------------------------------------------------------------------
void CSDOAQ_Dlg::OnSdoaqComboObjective()
{
	CString sObjective;
	((CComboBox*)GetDlgItem(IDC_COMBO_OBJECTIVE))->GetLBText(((CComboBox*)GetDlgItem(IDC_COMBO_OBJECTIVE))->GetCurSel(), sObjective);

	// Set calibration data of the selected objective lens
	for (auto it_data = m_calFile.calibData.begin(); it_data != m_calFile.calibData.end(); it_data++)
	{
		auto& list = *it_data;
		if (0 == sObjective.Compare(list.objective))
		{
			const auto size = (int)list.calibTable.size();

			if (size == 0)
			{
				continue;
			}
			auto height = new double[size];
			auto pitchX = new double[size];
			auto pitchY = new double[size];
			auto scaleX = new double[size];
			auto scaleY = new double[size];
			auto shiftX = new double[size];
			auto shitfY = new double[size];

			int index = 0;
			for (auto it_table = list.calibTable.begin(); it_table != list.calibTable.end(); it_table++)
			{
				height[index] = it_table->obj_height;
				pitchX[index] = it_table->pixel_pitch_x;
				pitchY[index] = it_table->pixel_pitch_y;
				scaleX[index] = it_table->scale_x;
				scaleY[index] = it_table->scale_y;
				shiftX[index] = it_table->shift_x;
				shitfY[index] = it_table->shift_y;
				index++;
			}

			eErrorCode rv_sdoaq = ::SDOAQ_SetExternalCalibrationTable(size,
				height, pitchX, pitchY,
				scaleX, scaleY, shiftX, shitfY,
				list.fieldCurvatureCoefs);
			if (ecNoError != rv_sdoaq)
			{
				ApiError(_T("SDOAQ_SetExternalCalibrationTable"), rv_sdoaq);
			}

			delete[] height;
			delete[] pitchX;
			delete[] pitchY;
			delete[] scaleX;
			delete[] scaleY;
			delete[] shiftX;
			delete[] shitfY;

			auto xRange = (list.calibTable[index / 2].pixel_pitch_x * nMaxWidth) / 2;
			dxRangeStart = -xRange; dxRangeEnd = xRange;
			auto yRange = (list.calibTable[index / 2].pixel_pitch_y * nMaxHeight) / 2;
			dyRangeStart = -yRange; dyRangeEnd = yRange;
			dzRangeStart = list.calibTable[index - 1].obj_height; dzRangeEnd = list.calibTable[0].obj_height;

			break;
		}
	}
}

//----------------------------------------------------------------------------
void CSDOAQ_Dlg::print_wsio_last_error(void)
{
	WSIOCHAR sLastError[4 * 1024];
	::WSIO_LastErrorString(sLastError, sizeof sLastError);
	Log(FString(_T("[WSIO ERROR] %s"), (CString)sLastError));
}

//----------------------------------------------------------------------------
void CSDOAQ_Dlg::print_wsgl_last_error(void)
{
	CString* sLastError = NewWString(::WSGL_GetLastErrorString(m_hwnd3D));
	Log(FString(_T("[WSGL ERROR] %s"), (CString)*sLastError));
	delete sLastError;
}

//----------------------------------------------------------------------------
void CSDOAQ_Dlg::ImageViewer(size_t uViewer, const char* title, int title_no
	, const tTestSet& SET, void* data)
{
	ImageViewer(uViewer, title, title_no
		, SET.afp.cameraRoiWidth, SET.afp.cameraRoiHeight, SET.m_nColorByte, data);
}

void CSDOAQ_Dlg::ImageViewer(size_t uViewer, const char* title, int title_no
	, int width, int height, int colorbytes, void* data)
{
	WSIOCHAR full_title[256] = { 0 };
	if (title)
	{
		sprintf_s(full_title, sizeof full_title, "%s %d", title, title_no);
	}

	const unsigned size = (data ? width * height * colorbytes : 0);
	if (WSIORV_SUCCESS > ::WSUT_IV_AttachRawImgData_V2(m_vhwndIV[uViewer], width, height, width*colorbytes, colorbytes, data, size, full_title))
	{
		print_wsio_last_error();
	}
}

//----------------------------------------------------------------------------
void CSDOAQ_Dlg::FloatViewer(bool onoff, size_t uViewer, const char* title, int title_no
	, const tTestSet& SET, void* data)
{
	if (onoff && data)
	{
		auto pSeek = (float*)data;
		auto low = *pSeek;
		auto high = *pSeek;

		const unsigned pixels = SET.PixelSize();
		for (unsigned uSampling = 0; uSampling < pixels; uSampling += 100)
		{
			float v = *(pSeek + uSampling);
			if (v < low)
			{
				low = v;
			}
			if (high < v)
			{
				high = v;
			}
		}
		const auto inc = (float)256 / (high - low);

		auto const pImg = new unsigned char[SET.PixelSize()];
		auto pDest = pImg;
		auto pSrc = (float*)data;
		for (unsigned loop = 0; loop < pixels; loop++)
		{
			*pDest++ = (const unsigned char)((*pSrc++ - low)*inc);
		}

		ImageViewer(uViewer, title, title_no, SET.afp.cameraRoiWidth, SET.afp.cameraRoiHeight, MONOBYTES, pImg);

		delete[] pImg;
	}
	else
	{
		ImageViewer(uViewer, title, title_no);
	}
}

//----------------------------------------------------------------------------
void CSDOAQ_Dlg::Viewer3D(bool onoff, const tTestSet& SET, void* p_pointcloud, void* p_edof)
{
	if (onoff && p_pointcloud && p_edof)
	{
		tPara_Display25D Para;
		Para.width = SET.PixelWidth();
		Para.height = SET.PixelHeight();
		Para.z_offset1 = dzRangeStart;
		Para.z_offset2 = dzRangeEnd;
		Para.z_slices = (unsigned)m_vFocusSet.size();
		Para.scx1 = dxRangeStart;
		Para.scx2 = dxRangeEnd;
		Para.scy1 = dyRangeStart;
		Para.scy2 = dyRangeEnd;
		Para.scz1 = dzRangeStart;
		Para.scz2 = dzRangeEnd;

		(void)::WSGL_Display_25D(m_hwnd3D, GL_MG_ONSTAGE, "main", (float*)p_pointcloud, SET.PixelSize() * XYZNUMS, p_edof, SET.ImgSize()
			, EDM_BGR_BYTE | EDM_DIMENSION_CALXY_25D | EDM_NDC_XY_ONLY, 1.0f, &Para);
	}
	else
	{
		(void)::WSGL_Display_BG(m_hwnd3D);
	}
}
