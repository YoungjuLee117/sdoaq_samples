#include "stdafx.h"
#include "SDOAQ_Callback.h"
#include "SDOAQ_App.h"
#include "SDOAQ_AppDlg.h"

//----------------------------------------------------------------------------
void __stdcall g_LogCallback(eLogSeverity severity, char* pMessage)
{
	if (theApp.m_pMainWnd)
	{
		theApp.m_pMainWnd->PostMessageW(EUM_LOG, (WPARAM)severity, (LPARAM)NewWString(pMessage));
	}
}

//----------------------------------------------------------------------------
void __stdcall g_ErrorCallback(eErrorCode errorCode, char* pErrorMessage)
{
	if (theApp.m_pMainWnd)
	{
		theApp.m_pMainWnd->PostMessageW(EUM_ERROR, (WPARAM)errorCode, (LPARAM)NewWString(pErrorMessage));
	}
}

//----------------------------------------------------------------------------
void __stdcall g_ObjectiveChanged(eObjectiveId newObjectiveId)
{
	if (theApp.m_pMainWnd)
	{
		const char* pName;
		switch (newObjectiveId)
		{
		case oi00_35x:
			pName = ">> 0.35x objective found.";
			break;
		case oi01_30x:
			pName = ">> 1.30x objective found.";
			break;
		case oi02_50x:
			pName = ">> 2.50x objective found.";
			break;
		case oi00_75x:
			pName = ">> 0.75x objective found.";
			break;
		case oi01_20x:
			pName = ">> 1.20x objective found.";
			break;
		case oi01_80x:
			pName = ">> 1.80x objective found.";
			break;
		case oi00_50x:
			pName = ">> 0.50x objective found.";
			break;
		case oi01_00x:
			pName = ">> 1.00x objective found.";
			break;
		}
		theApp.m_pMainWnd->PostMessageW(EUM_LOG, (WPARAM)lsInfo, (LPARAM)NewWString(pName));
	}
}

//----------------------------------------------------------------------------
void __stdcall g_InitDoneCallback(eErrorCode errorCode, char* pErrorMessage)
{
	if (theApp.m_pMainWnd)
	{
		theApp.m_pMainWnd->PostMessageW(EUM_INITDONE, (WPARAM)errorCode, (LPARAM)NewWString(pErrorMessage));
	}
}

//----------------------------------------------------------------------------
static void callback_test_log(LPCTSTR sz_title, eErrorCode errorCode, void* callbackUserData)
{
	auto pString = new CString;
	pString->Format(_T("%s CALLBACK <- 0x%I64X"), sz_title ? sz_title : _T(""), (unsigned long long)callbackUserData);
	theApp.m_pMainWnd->PostMessageW(EUM_LOG, ecNoError != errorCode ? (WPARAM)lsError : (WPARAM)lsInfo, (LPARAM)pString);
}

//----------------------------------------------------------------------------
void __stdcall g_MoveokCallback(eErrorCode errorCode, void* callbackUserData)
{
	if (theApp.m_pMainWnd)
	{
		static void* g_prev = NULL;
		if (g_prev != callbackUserData)
		{
			g_prev = callbackUserData;			
		}

		callback_test_log(_T("MoveOK"), errorCode, callbackUserData);

		if (ecNoError != errorCode)		
		{
			// Handle error:
			// - retry image capture
			// - skip this cycle
			// - log and continue
			return;
		}
		
		// Motion trigger logic here
	}
}

//----------------------------------------------------------------------------
void __stdcall g_PlayFocusStackCallbackEx(eErrorCode errorCode, int lastFilledRingBufferEntry, void* callbackUserData)
{
	if (theApp.m_pMainWnd)
	{
		theApp.m_pMainWnd->PostMessageW(EUM_RECEIVE_ZSTACK, (WPARAM)errorCode, (LPARAM)lastFilledRingBufferEntry);

		static void* g_prev = NULL; if (g_prev != callbackUserData) { g_prev = callbackUserData; callback_test_log(_T("FOCUS"), errorCode, callbackUserData); }
	}
}

void __stdcall g_PlayFocusStackCallback(eErrorCode errorCode, int lastFilledRingBufferEntry)
{
	g_PlayFocusStackCallbackEx(errorCode, lastFilledRingBufferEntry, NULL);
}

//----------------------------------------------------------------------------
void __stdcall g_PlayEdofCallbackEx(eErrorCode errorCode, int lastFilledRingBufferEntry, void* callbackUserData)
{
	if (theApp.m_pMainWnd)
	{
		theApp.m_pMainWnd->PostMessageW(EUM_RECEIVE_EDOF, (WPARAM)errorCode, (LPARAM)lastFilledRingBufferEntry);

		static void* g_prev = NULL; if (g_prev != callbackUserData) { g_prev = callbackUserData; callback_test_log(_T("EDOF"), errorCode, callbackUserData); }
	}
}

void __stdcall g_PlayEdofCallback(eErrorCode errorCode, int lastFilledRingBufferEntry)
{
	g_PlayEdofCallbackEx(errorCode, lastFilledRingBufferEntry, NULL);
}

//----------------------------------------------------------------------------
void __stdcall g_PlayAFCallbackEx2(eErrorCode errorCode, int lastFilledRingBufferEntry, void* callbackUserData, double dbBestFocusStep, double dbScore, double dbMatchedStep)
{
	if (theApp.m_pMainWnd)
	{
		auto pcPara = new tMsgParaReceiveAf;
		pcPara->lastFilledRingBufferEntry = lastFilledRingBufferEntry;
		pcPara->dbBestFocusStep = dbBestFocusStep;
		pcPara->dbScore = dbScore;
		pcPara->dbMatchedStep = dbMatchedStep;
		theApp.m_pMainWnd->PostMessageW(EUM_RECEIVE_AF, (WPARAM)errorCode, (LPARAM)pcPara);

		static void* g_prev = NULL; if (g_prev != callbackUserData) { g_prev = callbackUserData; callback_test_log(_T("AF"), errorCode, callbackUserData); }
	}
}

void __stdcall g_PlayAFCallbackEx(eErrorCode errorCode, int lastFilledRingBufferEntry, double dbBestFocusStep, double dbScore, double dbMatchedStep)
{
	g_PlayAFCallbackEx2(errorCode, lastFilledRingBufferEntry, NULL, dbBestFocusStep, dbScore, dbMatchedStep);
}

void __stdcall g_PlayAFCallback(eErrorCode errorCode, int lastFilledRingBufferEntry, double dbBestFocusStep, double dbScore)
{
	g_PlayAFCallbackEx2(errorCode, lastFilledRingBufferEntry, NULL, dbBestFocusStep, dbScore, NULL);
}

//----------------------------------------------------------------------------
void __stdcall g_SnapCallbackEx(eErrorCode errorCode, int lastFilledRingBufferEntry, void* callbackUserData)
{
	if (theApp.m_pMainWnd)
	{
		theApp.m_pMainWnd->PostMessageW(EUM_RECEIVE_SNAP, (WPARAM)errorCode, (LPARAM)lastFilledRingBufferEntry);

		static void* g_prev = NULL; if (g_prev != callbackUserData) { g_prev = callbackUserData; callback_test_log(_T("SNAP"), errorCode, callbackUserData); }
	}
}

void __stdcall g_SnapCallback(eErrorCode errorCode, int lastFilledRingBufferEntry)
{
	g_SnapCallbackEx(errorCode, lastFilledRingBufferEntry, NULL);
}
