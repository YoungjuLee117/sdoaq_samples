
// SdoaqMultiLightingDlg.cpp : implementation file
//

#include "pch.h"
#include "framework.h"
#include "SdoaqMultiLighting.h"
#include "SdoaqMultiLightingDlg.h"
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

CSdoaqMultiLightingDlg::CSdoaqMultiLightingDlg(CWnd* pParent /*=nullptr*/)
	: CDialogEx(IDD_SDOAQMULTILIGHTING_DIALOG, pParent)
{
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
}

void CSdoaqMultiLightingDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
}

BEGIN_MESSAGE_MAP(CSdoaqMultiLightingDlg, CDialogEx)
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	ON_WM_CLOSE()
	ON_WM_SIZE()
	ON_MESSAGE(EUM_INITDONE, OnInitDone)
	ON_BN_CLICKED(IDC_INITIALIZE, &CSdoaqMultiLightingDlg::OnBtnInitialize)
	ON_BN_CLICKED(IDC_FINALIZE, &CSdoaqMultiLightingDlg::OnBtnFinalize)
	ON_CBN_SELCHANGE(IDC_COMBO_LIGHTING, &CSdoaqMultiLightingDlg::OnCbnSelLighting)
	ON_BN_CLICKED(IDC_BTN_SET, &CSdoaqMultiLightingDlg::OnBtnSet)
	ON_CBN_SELCHANGE(IDC_COMBO_ACTIVE_LIGHTING, &CSdoaqMultiLightingDlg::OnCbnSelActiveLighting)
	ON_BN_CLICKED(IDC_ACQ_STACK, &CSdoaqMultiLightingDlg::OnBtnAcqStack)
END_MESSAGE_MAP()


// CSdoaqMultiLightingDlg message handlers

BOOL CSdoaqMultiLightingDlg::OnInitDialog()
{
	CDialogEx::OnInitDialog();

	// Set the icon for this dialog.  The framework does this automatically
	//  when the application's main window is not a dialog
	SetIcon(m_hIcon, TRUE);			// Set big icon
	SetIcon(m_hIcon, FALSE);		// Set small icon

	// TODO: Add extra initialization here
	const int ver_major = ::SDOAQ_GetMajorVersion();
	const int ver_minor = ::SDOAQ_GetMinorVersion();
	const int ver_patch = ::SDOAQ_GetPatchVersion();
	g_LogLine(_T("SDOAQ DLL version is \"%d.%d.%d\""), ver_major, ver_minor, ver_patch);

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

// If you add a minimize button to your dialog, you will need the code below
//  to draw the icon.  For MFC applications using the document/view model,
//  this is automatically done for you by the framework.

void CSdoaqMultiLightingDlg::OnPaint()
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

// The system calls this function to obtain the cursor to display while the user drags
//  the minimized window.
HCURSOR CSdoaqMultiLightingDlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}

BOOL CSdoaqMultiLightingDlg::PreTranslateMessage(MSG* pMsg)
{
	if (pMsg->message == WM_KEYDOWN)
	{
		if (pMsg->wParam == VK_RETURN)
			return TRUE;
		else if (pMsg->wParam == VK_ESCAPE)
			return TRUE;
	}

	return CDialogEx::PreTranslateMessage(pMsg);
}


void CSdoaqMultiLightingDlg::OnClose()
{
	(void)::SDOAQ_Finalize();
	(void)::WSUT_IV_DestroyImageViewer(g_hViewer);

	CDialogEx::OnClose();
}


void CSdoaqMultiLightingDlg::OnSize(UINT nType, int cx, int cy)
{
	CDialogEx::OnSize(nType, cx, cy);

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
LRESULT CSdoaqMultiLightingDlg::OnInitDone(WPARAM wErrorCode, LPARAM lpMessage)
{
	CString* pMessage = (CString*)lpMessage;

	if (ecNoError == wErrorCode)
	{
		g_LogLine(_T("InitDoneCallback() %s"), pMessage ? *pMessage : _T(""));

		GetDlgItem(IDC_INITIALIZE)->SetWindowText(_T("Initialized"));
		GetDlgItem(IDC_INITIALIZE)->EnableWindow(FALSE);

		GetDlgItem(IDC_COMBO_LIGHTING)->EnableWindow(TRUE);
		GetDlgItem(IDC_BTN_SET)->EnableWindow(TRUE);
		GetDlgItem(IDC_COMBO_ACTIVE_LIGHTING)->EnableWindow(TRUE);
		GetDlgItem(IDC_ACQ_STACK)->EnableWindow(TRUE);

		BuildLightingList();
		BuildAcquisitionSet();
	}
	else
	{
		g_LogLine(_T("InitDoneCallback() returns error(%d:%s)."), wErrorCode, pMessage ? *pMessage : _T(""));
		g_LogLine(_T("[WARNING] Abnormal operation may occur if initialization is executed again after initialization is completed."
			"Finalize first and then re-initialize."));
	}

	if (pMessage)
	{
		delete pMessage;
	}

	return 0;
}


//----------------------------------------------------------------------------
void CSdoaqMultiLightingDlg::OnBtnInitialize()
{
	// set the cam files folder path
	::SDOAQ_SetCamfilePath(FStringA("%s\\..\\..\\Include\\SDOAQ\\CamFiles", (CStringA)GetCurrentDir()));

	g_LogLine(_T("start sdoaq initialization..."));

	const eErrorCode rv_sdoaq = ::SDOAQ_Initialize(NULL, NULL, g_SDOAQ_InitDoneCallback);
	if (ecNoError != rv_sdoaq)
	{
		g_LogLine(_T("SDOAQ_Initialize() returns error(%d)."), rv_sdoaq);
	}
}


//----------------------------------------------------------------------------
void CSdoaqMultiLightingDlg::OnBtnFinalize()
{
	g_LogLine(_T("sdoaq finalize."));

	eErrorCode rv_sdoaq = ::SDOAQ_Finalize();
	if (ecNoError != rv_sdoaq)
	{
		g_LogLine(_T("SDOAQ_Finalize() returns error(%d)."), rv_sdoaq);
	}

	GetDlgItem(IDC_INITIALIZE)->SetWindowText(_T("Initialize"));
	GetDlgItem(IDC_INITIALIZE)->EnableWindow(TRUE);

	GetDlgItem(IDC_COMBO_LIGHTING)->EnableWindow(FALSE);
	GetDlgItem(IDC_BTN_SET)->EnableWindow(FALSE);
	GetDlgItem(IDC_COMBO_ACTIVE_LIGHTING)->EnableWindow(FALSE);
	GetDlgItem(IDC_ACQ_STACK)->EnableWindow(FALSE);
}


//----------------------------------------------------------------------------
void CSdoaqMultiLightingDlg::OnCbnSelLighting()
{
	CString sSelectedLight;
	auto p_combo = (CComboBox*)GetDlgItem(IDC_COMBO_LIGHTING);
	p_combo->GetLBText(p_combo->GetCurSel(), sSelectedLight);

	::SDOAQ_SetStringParameterValue(piSelectSettingLighting, (CStringA)sSelectedLight.GetBuffer());

	// read data for selected lighting
	int nValue;
	(void)::SDOAQ_GetIntParameterValue(piDataExposureTime, &nValue);
	SetDlgItemText(IDC_EDIT_EXPOSURE, FString(_T("%d"), nValue));

	double dbValue;
	(void)::SDOAQ_GetDblParameterValue(piDataGain, &dbValue);
	SetDlgItemText(IDC_EDIT_GAIN, FString(_T("%.1lf"), dbValue));
}


//----------------------------------------------------------------------------
void CSdoaqMultiLightingDlg::OnBtnSet()
{
	CString sSelectedLight;
	auto p_combo = (CComboBox*)GetDlgItem(IDC_COMBO_LIGHTING);
	auto cur_sel = p_combo->GetCurSel();
	if (cur_sel >= 0)
	{
		p_combo->GetLBText(cur_sel, sSelectedLight);
	}
	else
	{
		g_LogLine(_T("first, select the light you want to specify data to."));
		return;
	}

	eParameterId pid;
	CString sValue;

	//--------------------------------------------------------
	// set data exposure time for the selected lighting
	//--------------------------------------------------------
	GetDlgItemText(IDC_EDIT_EXPOSURE, sValue);
	int nMin, nMax, nValue;
	nValue = _ttoi(sValue);

	pid = piDataExposureTime;
	eErrorCode rv_sdoaq = ::SDOAQ_GetIntParameterRange(pid, &nMin, &nMax);
	if (ecNoError == rv_sdoaq)
	{
		if (nMin <= nValue && nValue <= nMax)
		{
			rv_sdoaq = ::SDOAQ_SetIntParameterValue(pid, nValue);
			if (ecNoError == rv_sdoaq)
			{
				g_LogLine(_T("set data exposure time for %s: %d"), sSelectedLight, nValue);
			}
			else
			{
				g_LogLine(_T("SDOAQ_SetIntParameterValue(%d) returns error(%d)."), pid, rv_sdoaq);
			}
		}
		else
		{
			g_LogLine(_T("piDataExposureTime value is out of range (%d ~ %d)."), nMin, nMax);
		}
	}
	else
	{
		g_LogLine(_T("SDOAQ_GetIntParameterRange(%d) returns error(%d)."), pid, rv_sdoaq);
	}

	//--------------------------------------------------------
	// set data gain for the selected lighting
	//--------------------------------------------------------
	GetDlgItemText(IDC_EDIT_GAIN, sValue);
	double dbMin, dbMax, dbValue;
	dbValue = _ttof(sValue);

	pid = piDataGain;
	::SDOAQ_GetDblParameterRange(pid, &dbMin, &dbMax);

	if (dbMin <= dbValue && dbValue <= dbMax)
	{
		eErrorCode rv_sdoaq = ::SDOAQ_SetDblParameterValue(pid, dbValue);
		if (ecNoError == rv_sdoaq)
		{
			g_LogLine(_T("set data gain for %s: %.3lf"), sSelectedLight, dbValue);
		}
		else
		{
			g_LogLine(_T("SDOAQ_SetDblParameterValue(%d) returns error(%d)."), pid, rv_sdoaq);
		}
	}
	else
	{
		g_LogLine(_T("piDataGain value is out of range (%.3lf ~ %.3lf)."), dbMin, dbMax);
	}
}


//----------------------------------------------------------------------------
void CSdoaqMultiLightingDlg::OnCbnSelActiveLighting()
{
	CString sActivatedLight;
	auto p_combo = (CComboBox*)GetDlgItem(IDC_COMBO_ACTIVE_LIGHTING);
	p_combo->GetLBText(p_combo->GetCurSel(), sActivatedLight);

	::SDOAQ_SetStringParameterValue(piActiveLightingList, (CStringA)sActivatedLight.GetBuffer());
}


//----------------------------------------------------------------------------
void CSdoaqMultiLightingDlg::OnBtnAcqStack()
{
	static int m_nContiStack = 0;

	auto& AFP = SET.afp;
	auto& FOCUS = SET.focus;

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
		ppFocusImages, pFocusImageBufferSizes);

	if (ecNoError == rv_sdoaq)
	{
		const auto tick_end = GetTickCount64();
		//g_LogLine(_T("SDOAQ_SingleShotFocusStackEx() takes : %llu ms / %d imgs"), tick_end - tick_begin, FOCUS.numsFocus);

		++m_nContiStack;
		ImageViewer("Zstack", m_nContiStack, SET, ppFocusImages[0]);
	}
	else
	{
		g_LogLine(_T("SDOAQ_SingleShotFocusStackEx() returns error(%d)."), rv_sdoaq);
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
void CSdoaqMultiLightingDlg::BuildLightingList()
{
	bool flagAvailable = false;
	eParameterId pid = piLightingList;
	eErrorCode rv_sdoaq = ::SDOAQ_IsParameterAvailable(pid, &flagAvailable);

	((CComboBox*)GetDlgItem(IDC_COMBO_LIGHTING))->ResetContent();
	((CComboBox*)GetDlgItem(IDC_COMBO_ACTIVE_LIGHTING))->ResetContent();

	if (ecNoError == rv_sdoaq)
	{
		if (flagAvailable)
		{
			eParameterType eType;
			rv_sdoaq = ::SDOAQ_GetParameterType(pid, &eType);
			if (ecNoError == rv_sdoaq)
			{
				if (ptString == eType)
				{
					char buf[256];
					int size = sizeof(buf);
					eErrorCode rv_sdoaq = ::SDOAQ_GetStringParameterValue(pid, buf, &size);
					if (ecNoError == rv_sdoaq)
					{
						CString sLight;
						int i = 0; // substring index to extract
						while (AfxExtractSubString(sLight, (CString)buf, i++, _T(' ')))
						{
							((CComboBox*)GetDlgItem(IDC_COMBO_LIGHTING))->AddString(sLight);
							((CComboBox*)GetDlgItem(IDC_COMBO_ACTIVE_LIGHTING))->AddString(sLight);
						}
					}
					else
					{
						g_LogLine(_T("SDOAQ_GetStringParameterValue(pid=%d) returns error(%d)."), pid, rv_sdoaq);
					}
				}
				else
				{
					g_LogLine(_T("type of pid(%d) should be string."));
				}
			}
			else
			{
				g_LogLine(_T("SDOAQ_GetParameterType(pid=%d) returns error(%d)."), pid, rv_sdoaq);
			}
		}
		else
		{
			g_LogLine(_T("pid(%d) is not available."));
		}
	}
	else
	{
		g_LogLine(_T("SDOAQ_IsParameterAvailable(pid=%d) returns error(%d)."), pid, rv_sdoaq);
	}
}


//----------------------------------------------------------------------------
void CSdoaqMultiLightingDlg::BuildAcquisitionSet()
{
	SET.m_nColorByte = IsMonoCameraInstalled() ? MONOBYTES : COLORBYTES;

	auto& AFP = SET.afp;
	int nDummy, nMaxWidth, nMaxHeight;
	if (ecNoError == ::SDOAQ_GetIntParameterRange(piCameraFullFrameSizeX, &nDummy, &nMaxWidth))
		AFP.cameraRoiWidth = nMaxWidth;
	if (ecNoError == ::SDOAQ_GetIntParameterRange(piCameraFullFrameSizeY, &nDummy, &nMaxHeight))
		AFP.cameraRoiHeight = nMaxHeight;

	auto& FOCUS = SET.focus;
	FOCUS.numsFocus = 1;
	int nFocus;
	if (ecNoError == ::SDOAQ_GetIntParameterValue(piFocusPosition, &nFocus))
		FOCUS.vFocusSet.push_back(nFocus);
	else
		FOCUS.vFocusSet.push_back(160);
}


//----------------------------------------------------------------------------
void CSdoaqMultiLightingDlg::ImageViewer(const char* title, int title_no, const tTestSet& SET, void* data)
{
	ImageViewer(title, title_no, SET.afp.cameraRoiWidth, SET.afp.cameraRoiHeight, SET.m_nColorByte, data);
}


//----------------------------------------------------------------------------
void CSdoaqMultiLightingDlg::ImageViewer(const char* title, int title_no, int width, int height, int colorbytes, void* data)
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
