
// SDOAQ_AppDlg.h : header file
//

#pragma once

#include "SDOAQ_Lib.h"
#include "SDOAQ_CalibrationFile.h"

//----------------------------------------------------------------------------
enum EUserMessage
{
	EUM_LOG = 0xA000, // LPARAM is a pointer of CString
	EUM_ERROR, // LPARAM is a pointer of CString
	EUM_INITDONE,
	EUM_RECEIVE_ZSTACK,
	EUM_RECEIVE_EDOF,
	EUM_RECEIVE_AF,
	EUM_RECEIVE_SNAP,
};

struct tMsgParaReceiveAf
{
	int lastFilledRingBufferEntry;
	double dbBestFocusStep;
	double dbScore;
	double dbMatchedStep;
};

//============================================================================
class CSDOAQ_Dlg : public CDialogEx
{
public:
	CSDOAQ_Dlg(CWnd* pParent = nullptr);	// standard constructor
	virtual ~CSDOAQ_Dlg();

protected:
	virtual BOOL PreTranslateMessage(MSG* pMsg);

protected:
	HICON m_hIcon;

	// Generated message map functions
	virtual BOOL OnInitDialog();

	DECLARE_MESSAGE_MAP()
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();

	afx_msg void OnTimer(UINT_PTR nIDEvent);
	afx_msg void OnClose();
	afx_msg void OnDestroy();
	afx_msg void OnSize(UINT nType, int cx, int cy);

	afx_msg LRESULT OnUmLog(WPARAM wSeverity, LPARAM lpMessage);
	afx_msg LRESULT OnUmError(WPARAM wErrorCode, LPARAM lpMessage);
	afx_msg LRESULT OnInitDone(WPARAM wErrorCode, LPARAM lpMessage);
	afx_msg LRESULT OnReceiveZstack(WPARAM wErrorCode, LPARAM lLastFilledRingBufferEntry);
	afx_msg LRESULT OnReceiveEdof(WPARAM wErrorCode, LPARAM lLastFilledRingBufferEntry);
	afx_msg LRESULT OnReceiveAF(WPARAM wErrorCode, LPARAM lMsgParaReceiveAf);
	afx_msg LRESULT OnReceiveSnap(WPARAM wErrorCode, LPARAM lLastFilledRingBufferEntry);

	afx_msg void OnCheckSimulMode();
	afx_msg void OnSdoaqInitialize();
	afx_msg void OnSdoaqFinalize();
	afx_msg void OnSelectedCombobox();
	afx_msg void OnSdoaqSetParameter();
	afx_msg void OnSdoaqSetROI();
	afx_msg void OnSdoaqSetAFROI();
	afx_msg void OnSdoaqSetRingBufSize();
	afx_msg void OnSdoaqSetFocusSet();
	afx_msg void OnSdoaqSetSnapFocusSet();
	afx_msg void OnSdoaqSingleShotStack();
	afx_msg void OnSdoaqPlayStack();
	afx_msg void OnSdoaqStopStack();
	afx_msg void OnSdoaqSingleShotEdof();
	afx_msg void OnSdoaqPlayEdof();
	afx_msg void OnSdoaqStopEdof();
	afx_msg void OnSdoaqSingleShotAF();
	afx_msg void OnSdoaqPlayAF();
	afx_msg void OnSdoaqStopAF();
	afx_msg void OnSdoaqSnap();
	afx_msg void OnSdoaqSetCalibrationFile();
	afx_msg void OnSdoaqComboObjective();

	void BuildEnvironment(void);
	void ReadySdoaqDll(void);
	void ShowViewer(void);

private:
	void BuildParameterID_Combobox(void);
	eParameterId GetCurrentParameterID(void);
	void BuildCalibrationFile_Combobox(void);

	//----------------------------------------------------------------------------
private:
	CString m_sScriptFile;
	CString m_sLogPath;

public:
	enum
	{
		TIMER_LOG = 0
	};

	struct tTestSet
	{
		tTestSet()
			: m_nColorByte(COLORBYTES)
		{
			afp.cameraRoiTop = 0;
			afp.cameraRoiLeft = 0;
			afp.cameraRoiWidth = (2064 / 4) * 4;
			afp.cameraRoiHeight = 1544;
			afp.cameraBinning = 1;
		}
		~tTestSet()
		{
			ClearBuffer();
		}
		void ClearBuffer(void)
		{
			if (rb.ppBuf)
			{
				for (size_t uidx = 0; uidx < rb.numsBuf; uidx++)
				{
					delete[] rb.ppBuf[uidx];
				}
				delete[] rb.ppBuf; rb.ppBuf = NULL;
				delete[] rb.pSizes; rb.pSizes = NULL;
				rb.numsBuf = 0;
			}
		}

		AcquisitionFixedParametersEx afp;

		inline int PixelSize(void) const { return afp.cameraRoiWidth * afp.cameraRoiHeight; }
		inline int ImgSize(void) const { return PixelSize() * m_nColorByte; }
		inline int DataSize(void) const { return PixelSize() * sizeof(float); }
		inline int PixelWidth(void) const { return afp.cameraRoiWidth; }
		inline int PixelHeight(void) const { return afp.cameraRoiHeight; }

		struct tRingBuf
		{
			tRingBuf(void)
				: active(false)
				, ppBuf(NULL)
				, pSizes(NULL)
				, numsBuf(0)
			{ ; }
			bool active;
			void** ppBuf;	// pointer to the data (image) buffer
			size_t* pSizes;	// array of data sizes (image sizes) for each data buffer
			size_t numsBuf;	// number of actual data in the ring buffer
		} rb;

		struct tFocus
		{
			tFocus(void)
				: numsFocus(10)
			{ ; }
			size_t numsFocus;
			std::vector<int> vFocusSet;
		} focus;

		int m_nColorByte;
	} SET;

private:
	int m_nRingBufferSize;

	int m_nContiStack;
	int m_nContiEdof;
	int m_nContiAF;

public:
	enum { DFLT_FOCUS_STEP = 160 };
	std::vector<int> m_vFocusSet;
	std::vector<int> m_vSnapFocusSet;

	int nMaxWidth, nMaxHeight;

	SDOAQ_CalibrationFile m_calFile;
	std::vector<CString> m_vsCalibList;
	// x,y and z range of calibration table (unit um). used for 3D rendering.
	double dxRangeStart;
	double dxRangeEnd;
	double dyRangeStart;
	double dyRangeEnd;
	double dzRangeStart;
	double dzRangeEnd;

	//----------------------------------------------------------------------------
	// LOG WINDOW
	//----------------------------------------------------------------------------
	CString m_sLog;
	CString m_sLogFileName;
	HANDLE m_hLogFile;
	DWORD m_tickLastLog;

	void Log(LPCTSTR p_log_str);
	void PrintLog(void);
	void ApiError(LPCTSTR sApi, int eCode);
	CString GetCurrentDir()
	{
		TCHAR currentDir[MAX_PATH];
		::GetCurrentDirectory(MAX_PATH, currentDir);
		return currentDir;
	}

	//----------------------------------------------------------------------------
	// IMAGE VIEWER
	//----------------------------------------------------------------------------
private:
	std::vector<WSIOVOID> m_vhwndIV;
	WSIOVOID m_hwnd3D;
	void print_wsio_last_error(void);
	void print_wsgl_last_error(void);

public:
	void ImageViewer(size_t uViewer, const char* title = NULL, int title_no = 0
		, const tTestSet& SET = tTestSet(), void* data = NULL);
	void ImageViewer(size_t uViewer, const char* title, int title_no
		, int width, int height, int colorbytes, void* data = NULL);
	void FloatViewer(bool onoff, size_t uViewer, const char* title = NULL, int title_no = 0
		, const tTestSet& SET = tTestSet(), void* data = NULL);
	void Viewer3D(bool onoff, const tTestSet& SET, void* p_pointcloud, void* p_edof);
};
