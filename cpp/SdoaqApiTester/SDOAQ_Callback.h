#pragma once
#include "..\..\Include\SDOAQ\SDOAQ_WS.h"

void __stdcall g_LogCallback(eLogSeverity severity, char* pMessage);
void __stdcall g_ErrorCallback(eErrorCode errorCode, char* pErrorMessage);
void __stdcall g_ObjectiveChanged(eObjectiveId newObjectiveId);
void __stdcall g_InitDoneCallback(eErrorCode errorCode, char* pErrorMessage);
void __stdcall g_MoveokCallback(eErrorCode errorCode, void* callbackUserData);
void __stdcall g_PlayFocusStackCallbackEx(eErrorCode errorCode, int lastFilledRingBufferEntry, void* callbackUserData);
void __stdcall g_PlayFocusStackCallback(eErrorCode errorCode, int lastFilledRingBufferEntry);
void __stdcall g_PlayFocusSingleFocusCallbackEx(eErrorCode errorCode, int lastFilledRingBufferEntry, void* callbackUserData);
void __stdcall g_PlayEdofCallbackEx(eErrorCode errorCode, int lastFilledRingBufferEntry, void* callbackUserData);
void __stdcall g_PlayEdofCallback(eErrorCode errorCode, int lastFilledRingBufferEntry);
void __stdcall g_PlayAFCallbackEx2(eErrorCode errorCode, int lastFilledRingBufferEntry, void* callbackUserData, double dbBestFocusStep, double dbScore, double dbMatchedStep);
void __stdcall g_PlayAFCallbackEx(eErrorCode errorCode, int lastFilledRingBufferEntry, double dbBestFocusStep, double dbScore, double dbMatchedStep);
void __stdcall g_PlayAFCallback(eErrorCode errorCode, int lastFilledRingBufferEntry, double dbBestFocusStep, double dbScore);
void __stdcall g_SnapCallbackEx(eErrorCode errorCode, int lastFilledRingBufferEntry, void* callbackUserData);
void __stdcall g_SnapCallback(eErrorCode errorCode, int lastFilledRingBufferEntry);