/* SDOAQ_WS.h

	Comments : This file exports all types and functions needed to access the SDO acquisition engine.
	Date     : 2019/11/12
	Author   : YoungJu Lee
	Copyright (c) 2019 SD Optics,Inc. All rights reserved.

	========================================================================================================================================================
	Revision history
	========================================================================================================================================================
	Version     date      Author         Descriptions
	--------------------------------------------------------------------------------------------------------------------------------------------------------
	 1.00   2020-03-05  YoungJu Lee     - The illumination operates in continuous mode and the maximum intensity is limited to 30
	--------------------------------------------------------------------------------------------------------------------------------------------------------
	 1.10   2020-03-16  YoungJu Lee     - Fix the rotation of the illumination pattern
										- Add the default calibration setting functions for each calibration element
	--------------------------------------------------------------------------------------------------------------------------------------------------------
	 1.20   2020.03.31  YoungJu Lee		- Remove limit on the memory usage
										- Release used memory when FINALIZE
										- Close serial port when FINALIZE
										- Fix error message when FINALIZE while performing algorithm
										- Fix an issue that ObjectiveChangedCallback is called when calibration data is set additionally without changing lens
										- Fix an issue that ObjectiveChangedCallback is called several times when the add-on lens is mounted or unmounted
										- Change the reflex pattern to perform the OFF command for the light if the intensity of the light is 0
										- Remove the intensity limit of ring light
	--------------------------------------------------------------------------------------------------------------------------------------------------------
	 1.30   2020.06.29  YoungJu Lee		- Fix an issue that dll locks up when the USB port is detached after SDOAQ_Initialize
										- Fix an issue that dll locks up when the acquisition starts after the USB port is re-attached
										- Return low-memory error if there is not enough memory to proceed by checking the physical memory
										- Remove unused SW triggering when acquisition fails
										- Change the capture waiting time 3000 to 1000
										- Fix intermittent capture timeout issue
										- Update the latest standard calibration table
										- Update sdedof library (center aligned version)
										  : When using sub ROI, calibration data for the area is applied. Previously, center calibration was applied
	--------------------------------------------------------------------------------------------------------------------------------------------------------
	 1.40   2020.08.10  YoungJu Lee     - Update Basler Pylon SDK 4.1.0.3660 to 6.1.1.19832
	--------------------------------------------------------------------------------------------------------------------------------------------------------
	 2.0.0  2020.12.14  YoungJu Lee     - Add piSaveFileFormat, piFocusLeftTop and piFocusRightBottom parameters
										- Add AcquisitionFixedParameters_V2
										- Add SDOAQ_AcquireEdof_V2 function (arguments pPointCloudBuffer is added in SDOAQ_AcquireEdof)
										- Add SDOAQ_StartContinuousEdof_V2 function (pair with SDOAQ_AcquireEdof)
										- Add SDOAQ_AcquireAF, SDOAQ_StartContinuousAF and SDOAQ_StopContinuousAF
										- Add PointCloud generating process
										- Set the pixel format according to the camera reverse situation (Basler acA2040-120uc)
										- Support both Basler Pylon SDK 4.1.0.3660 and 6.1.1.19832
	--------------------------------------------------------------------------------------------------------------------------------------------------------
			2021.03.24  YoungJu Lee     - Remove the right and the bottom black edges from the captured image (Basler acA2040-120uc)
										  : The second to last pixels are just copied into the last
										- Fix an issue that the current lighting intensity is set to the maximum intensity when performing auto illumination
										  with a maximum intensity lower than the current lighting intensity
										- Add 5 types of add-on lenses (0.75x, 1.2x, 1.8x, 0.5x, 1.0x)
										- Add patch version and API to get the patch version
										- Change the maximum number of images for MALS controller trigger kick 50 to 320
										- Add eReflexCorrectionMethod enum type
										- Change the property of pi_edof_calc_resize_ratio to writable
										  (The resize ratio has no effect on the size of the image. It only affects the algorithm execution speed)
										- Support Basler camera acA2040-55uc
	--------------------------------------------------------------------------------------------------------------------------------------------------------
			2022.06.20  YoungJu Lee     - Support Basler camera acA2440-75uc, acA2440-75um, acA2040-90uc and acA2040-90um
										- Support binning (with algorithm sdedof.dll version 0.6.0)
										- Add API set for camera ROI parameter (SDOAQ_GetCameraParameter and SDOAQ_SetCameraParameter)
	--------------------------------------------------------------------------------------------------------------------------------------------------------
			2022.08.02  YoungJu Lee  	- Add SDOAQ_SetExternalCalibrationTable API and objective id oi_user_defined
										- Add piSavePixelBits as the depth of the pixel values when saving image
	--------------------------------------------------------------------------------------------------------------------------------------------------------
			2022.10.25  YoungJu Lee		- Add SDOAQ_StartContinuousSingleFocusStack API and piSingleFocus parameter for continuous acquisition while changing single focus
										- Add piFocusMeasureMethod parameter for selecting focus measure method
	--------------------------------------------------------------------------------------------------------------------------------------------------------
			2023.01.25  YoungJu Lee		- Add CSLCB-PWM illumination (piIntensityGeneralChannel_1 ~ 8)
	--------------------------------------------------------------------------------------------------------------------------------------------------------
	 2.1.0  2023.06.20  YoungJu Lee		- Add SDOAQ_StartContinuousMF, SDOAQ_StopContinuousMF and SDOAQ_UpdateContinuousMF
										- Add eSaveFormat enum type
										- Add SDOAQ_PlaySnap
										- Add grab camera interface
										- Add TPSU light
	--------------------------------------------------------------------------------------------------------------------------------------------------------
	 2.2.0  2023.08.04  YoungJu Lee		- Add string type parameter
										- Add EDoF algorithm method selection (support 3rd party algorithm)
										- Add Nikon motorized nosepiece controller
										- Fix an issue that camera register was not updated after running auto whitebalance
										- Do hard stop when acquisition is stopped
	--------------------------------------------------------------------------------------------------------------------------------------------------------
	 2.2.1  2023.08.30  YoungJu Lee		- Fix an issue that light could not be turned off when hard stop go too fast
	--------------------------------------------------------------------------------------------------------------------------------------------------------
	 2.2.2  2023.09.22  YoungJu Lee		- Set critical section for image buffer
	--------------------------------------------------------------------------------------------------------------------------------------------------------
	 2.3.0  2023.10.04  YoungJu Lee		- Update sdedof library v0.82
										- Add SDOAQ_SetCalibrationFile
										- Add edof scale correction parameters (pi_edof_is_scale_correction_enabled, pi_edof_scale_correction_dst_step)
										- Add API to get algorithm version
										- Add SDOAQ_PlayAfCallbackEx API with an matched focus step as a parameter
			2023.11.03  YoungJu Lee		- Add parameters to measure acquisition performance (piVpsReportCycleSeconds, piVpsReportTimeSeconds)
										- Add parameter to specify MALS highest steps for simulation (piSimulMalsHighestStep, piSimulMalsLowestStep)
										- Update the parameter in SNAP API with structure type
	--------------------------------------------------------------------------------------------------------------------------------------------------------
	 2.3.1  2023.11.23  YoungJu Lee		- Set Windows periodic timers to 1 millisecond
										- If the ring buffer size is 1, image acquisition runs only once and then stops
	--------------------------------------------------------------------------------------------------------------------------------------------------------
*/

#pragma once

#ifdef __cplusplus
extern "C"
{
#endif
	// In the following context "SDOAQ" is used as synonymous for "SD Optics Acquisition API(DLL)"

	//////////////////////////////////////////////////////////////////////////////////////////////////////////
	//
	// Functions and types needed for error handling ...
	//
	//////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// This enum describes the type of error that has occurred or ecNoError if no error has occurred.
	/// </summary>
	enum eErrorCode
	{
		/// <summary>This value means that no error has occurred.</summary>
		ecNoError = 0,

		/// <summary>A not better definable error has occurred.</summary>
		ecUnknownError = 1,

		/// <summary>This value signals a low memory error.</summary>
		ecLowMemory = 2,

		/// <summary>This given file was not found.</summary>
		ecFileNotFound = 3,

		/// <summary>This error signals that SDOAQ is currently not initialized.</summary>
		ecNotInitialized = 4,

		/// <summary>Function is called with an invalid function parameter.</summary>
		ecInvalidParameter = 5,

		/// <summary>This error signals a timeout has occurred.</summary>
		ecTimeoutOccurred = 6,

		/// <summary>This error occurs if a write access to an readonly device parameter was requested.</summary>
		ecParameterIsNotWritable = 7,

		/// <summary>This error occurs if a auto adjustment function e.g. SDOAQ_AutoExposure() failed to reach the given target value.</summary>
		ecAutoAdjustTargetNotReached = 8,

		/// <summary>This error occurs if the function is not implemnted.</summary>
		ecNotImplemented = 9,

		/// <summary>This error occurs if the function is not supported.</summary>
		ecNotSupported = 10,

		/// <summary>This error occurs when there is no authorization.</summary>
		ecNoAuthorization = 11,

		/// <summary>ToDo: Further values have to be defined ...</summary>
	};

	/// <summary>
	/// This function is a callback. It is called by SDOAQ if an error has occurred on a running task. 
	/// This callback must be assigned during initialization.
	/// </summary>
	typedef void(__stdcall* SDOAQ_ErrorCallback)(eErrorCode errorCode, char* pErrorMessage);


	//////////////////////////////////////////////////////////////////////////////////////////////////////////
	//
	// Functions and types needed for logging/tracing ...
	//
	//////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// This enum describes the importance of the log message. Depending on this importance,
	/// the logger can decide whether this message is displayed/saved or not.
	/// </summary>
	enum eLogSeverity
	{
		/// <summary>The log message should be treated as an error. An error level message is always logged.</summary>
		lsError = 0,

		/// <summary>The log message should be treated as an warning. An warning level message is always logged.</summary>
		lsWarning = 1,

		/// <summary>The log message has an info level. Info level usually describes an important step in a workflow and is usually logged.</summary>
		lsInfo = 2,

		/// <summary>The log message has only trace/Verbose level. This is usually used for development and troubleshooting and is only logged in this cases.</summary>
		lsVerbose = 3
	};

	/// <summary>
	/// This function is a callback. It is called by SDOAQ and is implemented in the client (ZENService).
	/// The message should be logged by the client to a window or a file. ZENService currently logs to log4net.
	/// The message buffer is allocated by SDOAQ. The client has to process/copy the message before this
	/// function returns back to SDOAQ, because the message may be allocated on the function stack.
	/// This callback must be assigned during initialization.
	/// </summary>
	typedef void(__stdcall* SDOAQ_LogCallback)(eLogSeverity severity, char* pMessage);


	//////////////////////////////////////////////////////////////////////////////////////////////////////////
	//
	// Functions and types needed for calibration
	//
	// The MALS controller steps do not reflect an equidistant height difference in micrometers. Therefore
	// a Z calibration value for every single controller step is needed. 
	// Because the non telecentricity of the acquired focus, edof images additionally a X, and Y calibration
	// value is needed for every controller step for measurements too.
	// The easiest way to do this is to get a calibration value for X, Y and Z for each possible MALS
	// controller step. So the client can calculate every distance needed.
	// 
	//////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Enum with all objectives which can be assembled. 
	/// </summary>
	enum eObjectiveId
	{
		/// <summary>0.35x objective is assembled.</summary>
		oi00_35x = 1,

		/// <summary>objective 1.3x (main standard) is assembled. objective 1.3x is always mounted.</summary>
		oi01_30x = 2,

		/// <summary>objective 2.5x is is assembled.</summary>
		oi02_50x = 3,

		/// <summary>objective 0.75x is is assembled.</summary>
		oi00_75x = 4,

		/// <summary>objective 1.2x is is assembled.</summary>
		oi01_20x = 5,

		/// <summary>objective 1.8x is is assembled.</summary>
		oi01_80x = 6,

		/// <summary>objective 0.5x is is assembled.</summary>
		oi00_50x = 7,

		/// <summary>objective 1.0x is is assembled.</summary>
		oi01_00x = 8,

		/// <summary>User defined objective.</summary>
		oi_user_defined = 100
	};

	/// <summary>
	/// This callback signals the client that a objective has been changed.
	/// It is called once during initialization to inform about the current objective and whenever objective is changed.
	/// Basic lens 1.3x objective is always mounted.
	/// </summary>
	typedef void(__stdcall* SDOAQ_ObjectiveChanged)(eObjectiveId newObjectiveId);

	/// <summary>This function registers the objectiveChanged callback funtion.</summary>
	__declspec(dllexport) eErrorCode SDOAQ_RegisterObjectiveChangedCallback(SDOAQ_ObjectiveChanged cbf);

	/// <summary>
	/// The calibration data is read from an external file. Set up the calibration file after initialization is done.
	/// </summary>
	__declspec(dllexport) eErrorCode SDOAQ_SetCalibrationFile(const char* sFilename);

	/// <summary>
	/// This function sets the calibration data for objetive that are not defined inside the dll.
	/// The calibration data is read from an external file. Set up the calibration file after initialization is done.
	/// </summary>
	__declspec(dllexport) eErrorCode SDOAQ_SetExternalCalibrationTable(
		int allocatedSize,
		double* pHeight, double* pPitchX, double* pPitchY,
		double* pScaleX, double* pScaleY,
		double* pShiftX, double* pShiftY,
		double* pCoefficients);

	/// <summary>This function requests the size (height or rows) of the XYZ calibration map.</summary>
	/* deprecated */__declspec(dllexport) eErrorCode SDOAQ_GetCalibrationDataMapSize(int* size);

	/// <summary>This function requests the XYZ calibration map.</summary>
	/* deprecated */__declspec(dllexport) eErrorCode SDOAQ_GetCalibrationDataMap(eObjectiveId objective, int allocatedSize, double* pCalX, double* pCalY, double* pCalZ);

	/// <summary>This function requests the XY calibration scale map.</summary>
	/* deprecated */__declspec(dllexport) eErrorCode SDOAQ_GetCalibrationScaleDataMap(eObjectiveId objective, int allocatedSize, double* pScaleX, double* pScaleY);

	/// <summary>This function requests the XY calibration shift map.</summary>
	/* deprecated */__declspec(dllexport) eErrorCode SDOAQ_GetCalibrationShiftDataMap(eObjectiveId objective, int allocatedSize, double* pShiftX, double* pShiftY);

	/// <summary>This function requests the curvature coefficients. example> p00 = 6.532, p10 = -0.006271, p20 = 3.684e-06</summary>
	/* deprecated */__declspec(dllexport) eErrorCode SDOAQ_GetCalibrationCurvatureData(eObjectiveId objective, int allocatedSize, double* pCoefficients);

	/// <summary>This function sets the XYZ calibration map.</summary>
	/* deprecated */__declspec(dllexport) eErrorCode SDOAQ_SetCalibrationDataMap(eObjectiveId objective, int allocatedSize, double* pCalX, double* pCalY, double* pCalZ);

	/// <summary>This function sets the XY calibration scale map.</summary>
	/* deprecated */__declspec(dllexport) eErrorCode SDOAQ_SetCalibrationScaleDataMap(eObjectiveId objective, int allocatedSize, double* pScaleX, double* pScaleY);

	/// <summary>This function sets the XY calibration shift map.</summary>
	/* deprecated */__declspec(dllexport) eErrorCode SDOAQ_SetCalibrationShiftDataMap(eObjectiveId objective, int allocatedSize, double* pShiftX, double* pShiftY);

	/// <summary>This function sets the curvature coefficients. example> p00 = 6.532, p10 = -0.006271, p20 = 3.684e-06.</summary>
	/* deprecated */__declspec(dllexport) eErrorCode SDOAQ_SetCalibrationCurvatureData(eObjectiveId objective, int allocatedSize, double* pCoefficients);

	/// <summary>This function sets the default XYZ calibration map.</summary>
	/* deprecated */__declspec(dllexport) eErrorCode SDOAQ_SetDefaultCalibrationDataMap(eObjectiveId objective);

	/// <summary>This function sets the default XY calibration scale map.</summary>
	/* deprecated */__declspec(dllexport) eErrorCode SDOAQ_SetDefaultCalibrationScaleDataMap(eObjectiveId objective);

	/// <summary>This function sets the default XY calibration shift map.</summary>
	/* deprecated */__declspec(dllexport) eErrorCode SDOAQ_SetDefaultCalibrationShiftDataMap(eObjectiveId objective);

	/// <summary>This function sets the default curvature coefficients. example> p00 = 6.532, p10 = -0.006271, p20 = 3.684e-06</summary>
	/* deprecated */__declspec(dllexport) eErrorCode SDOAQ_SetDefaultCalibrationCurvatureData(eObjectiveId objective);


	//////////////////////////////////////////////////////////////////////////////////////////////////////////
	//
	// Functions and types needed by initialization and finalization of the system ...
	//
	//////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// The call of SDOAQ_Initialize() can take a while. Therefore SDOAQ_Initialize() is a non blocking function.
	/// If all initialization on SDOAQ side is done, this callback is called by SDOAQ to inform the client.
	/// This callback must be assigned during intialization.
	/// </summary>
	typedef void(__stdcall* SDOAQ_InitDoneCallback)(eErrorCode errorCode, char* pErrorMessage);

	/// <summary>
	/// This function starts the initialization of the SDOAQ. The loggingCallback, 
	/// the errorCallback and the initDoneCallback are given by the client (ZENService).
	/// This function only kicks of the initialization and returns to the client. 
	/// When the initialization is done, "initDoneCallback" is called with the error
	/// result value.
	/// </summary>
	__declspec(dllexport) eErrorCode SDOAQ_Initialize(SDOAQ_LogCallback loggingCallback, SDOAQ_ErrorCallback errorCallback, SDOAQ_InitDoneCallback initDoneCallback);

	/// <summary>
	/// This function is the counterpart of SDOAQ_Initialize() and finalizes SDOAQ. 
	/// </summary>
	__declspec(dllexport) eErrorCode SDOAQ_Finalize();

	/// <summary>
	/// ToDo: must be defined ...
	/// </summary>
	__declspec(dllexport) eErrorCode SDOAQ_GetStatus(char* statusData);

	/// <summary>
	/// This function returns the minor version number of the API.
	/// </summary>
	__declspec(dllexport) int SDOAQ_GetMinorVersion();

	/// <summary>
	/// This function returns the major version number of the API.
	/// </summary>
	__declspec(dllexport) int SDOAQ_GetMajorVersion();

	/// <summary>
	/// This function returns the patch version number of the API.
	/// </summary>
	__declspec(dllexport) int SDOAQ_GetPatchVersion();

	/// <summary>
	/// This function returns the algorithm version number of the API.
	/// </summary>
	__declspec(dllexport) int SDOAQ_GetAlgorithmVersion();


	//////////////////////////////////////////////////////////////////////////////////////////////////////////
	//
	// Functions and types needed to get available parameters / parameter ranges / parameter values
	//
	//////////////////////////////////////////////////////////////////////////////////////////////////////////

	enum eParameterType
	{
		ptInt = 0,
		ptDouble = 1,
		ptString = 2
	};

	enum eParameterId
	{
		piCameraExposureTime = 0,				// I - R/W  (microseconds)
		piCameraFullFrameSizeX = 1,				// I -  R	(pixels)
		piCameraFullFrameSizeY = 2,				// I -  R	(pixels)
		piCameraPixelSizeX = 3,					// I -  R	(nanometers)
		piCameraPixelSizeY = 4,					// I -  R	(nanometers)

		/// <summary>
		/// Gets current binning of the camera. This parameter is readonly because it must only be set
		/// with sAcquisitionFixedParameters because it affects image buffer sizes.
		/// </summary>
		piCameraBinning = 5,					// I -  R   (matrix size)
		piCameraGain = 6,						// D - R/W
		piWhiteBalanceRed = 7,					// D - R/W
		piWhiteBalanceGreen = 8,				// D - R/W
		piWhiteBalanceBlue = 9,					// D - R/W

		/// <summary>Sets the intensity of ring illumination 1 (inner ring).</summary>
		piInnerRingIntensity = 10,				// D - R/W	 (%)

		/// <summary>Sets the intensity of ring illumination 2 (middle ring).</summary>
		piMiddleRingIntensity = 11,				// D - R/W	 (%)

		/// <summary>Sets the intensity of ring illumination 3 (outer ring).</summary>
		piOuterRingIntensity = 12,				// D - R/W	 (%)

		/// <summary>Sets the intensity of coax illumination.</summary>
		piCoaxIntensity = 13,					// D - R/W	 (%)

		/// <summary>Gets the minimum and maximum focus position of the MALS controller.</summary>
		piFocusPosition = 14,					// I - R	 (MALS step)

		/// <summary>
		/// Defines the reflex correction algorithm which is to be used.
		/// Whether or not reflex correction is applied is determined by the piReflexCorrectionPattern parameter.
		/// </summary>
		piReflexCorrectionAlgorithm = 15,		// I - R/W

		/// <summary>
		/// Defines which illumination pattern is used for reflex correction.
		/// refer to 16bit-PatternID
		/// </summary>
		piReflexCorrectionPattern = 16,			// I - R/W

		/// <summary>
		/// Maybe we need to reduce system load on weak computers. Therefore we need the possibility to 
		/// define the maximal number of focus stacks which should be acquired per second. 
		/// Using a double value allows fine adjustment on values < 1.0. e.g. 0.2 stacks/s.
		/// A range of 0.1 .. 140.0 should be enough.
		/// </summary>
		piMaxStacksPerSecond = 17,				// D - R/W

		/// <summary>
		/// The id/enum of the currently used objective.
		/// It is read-only for Visioner 1 and refer to eObjectiveId. 2 => currently no add-on objective is detected.
		/// For products using a motorized nosepiece controller,
		/// it is read-write and the objective id is different depending on the device controller.
		/// </summary>
		piObjectiveId = 18,						// I - R/W

		/// <summary>
		/// EDoF Algorithm method. Select an algorithm to generate EDoF image, including the built-in sdedof algorithm. 
		/// To add a 3rd party algorithm, pre-definition is required.
		/// </summary>
		pi_allgorithm_method_edof = 67,			// I - R/W

		/// <summary>
		/// {1.0, 0.5, 0.25}
		/// Gets current active resize ratio of the EDoF calculation. 
		/// old - This parameter is readonly because it
		/// must only be set with sEdofCalculationFixedParameters because it affects image buffer sizes.
		/// modified - It can be writable because it does not affect buffer size.
		/// </summary>
		pi_edof_calc_resize_ratio = 19,			// D - R/W

		/// <summary>larger kernel size -> smoother depth map.
		/// default = 3 (when resize_ratio = 0.25f)
		/// default = 5 (when resize_ratio = 0.5f)
		/// default = 9 (when resize_ratio = 1.0f)
		/// </summary>
		pi_edof_calc_pixelwise_kernel_size = 20,// I - R/W

		/// <summary>algorithm parameter</summary>
		pi_edof_calc_pixelwise_iteration = 21,	// I - R/W

		/// <summary>larger kernel size -> less noise.
		/// default = 0 (when mals_step_interval >= 16)
		/// default = 1 (when mals_step_interval < 16 && mals_step_interval >= 8)
		/// default = 2 (when mals_step_interval < 8 && mals_step_interval >= 4)
		/// default = 3 (when mals_step_interval = 3)
		/// default = 5 (when mals_step_interval = 2)
		/// default = 10 (when mals_step_interval = 1)
		/// if the value is set to -1, this parameter is automatically adjusted.
		/// </summary>
		pi_edof_depthwise_kernel_size = 22,		// I - R/W

		/// <summary>algorithm parameter</summary>
		pi_edof_depth_quality_th = 23,			// D - R/W

		/// <summary>algorithm parameter
		/// default = round(8.0f / mals_step_interval)
		/// if bilateral_sigma_color <= 0, bilateral filtering is replaced with Gaussian filtering
		/// </summary>
		pi_edof_bilateral_sigma_color = 24,		// D - R/W

		/// <summary>algorithm parameter
		/// default = 3 (when resize_ratio = 0.25f)
		/// default = 5 (when resize_ratio = 0.5f)
		/// default = 9 (when resize_ratio = 1.0f)
		/// if bilateral_sigma_space <= 0, bilateral filtering is disabled.
		/// </summary>
		pi_edof_bilateral_sigma_space = 25,		// D - R/W

		/// <summary>-1 for max. num. of thread.</summary>
		pi_edof_num_thread = 26,				// I - R/W

		/// <summary>
		/// Enables image scale correction
		/// more computation needed
		/// edof image has a constant pixel pitch which refer to scale_correction_dst_step
		/// </summary>
		pi_edof_is_scale_correction_enabled = 68,// I - R/W

		/// <summary>
		/// reference MALS step for image scale correction
		/// range = {MALS_MIN_STEP ~ MALS_MAX_STEP}
		/// </summary>
		pi_edof_scale_correction_dst_step = 69,	// I - R/W

		/// <summary>
		/// Specifies parameters to pass to the HeliconFocus executable when using the HeliconFocus edof algorithm.
		/// </summary>
		piAlgoParamHeliconFocus = 66,			// S - R/W

		/// <summary>
		/// Defines the format of the raw images and the resulting image to be saved. Refer to eSaveFormat.
		/// </summary>
		piSaveFileFormat = 27,					// I - R/W

		/// <summary>pixel size of camera, 8/10/12 bit. This is defined by the pixel format.</summary>
		piSavePixelBits = 28,					// I - R

		/// <summary>left and top point of focus ROI
		/// The upper 16 bits are left and lower 16 bits are top position. 0xAAAABBBB
		/// </summary>
		piFocusLeftTop = 29,					// I - R/W

		/// <summary>right and bottom point of focus ROI
		/// The upper 16 bits are right and lower 16 bits are bottom position. 0xAAAABBBB
		/// </summary>
		piFocusRightBottom = 30,				// I - R/W

		/// <summary>color type of camera sensor, mono or color. Refer to eCameraColor.</summary>
		piCameraColor = 31,						// I - R

		/// <summary>Defines which focus measurement method is used. It has eFocusMeasureMethod value.</summary>
		piFocusMeasureMethod = 32,				// I - R/W

		/// <summary>Defines a single focus for continuous single focus acqisition.</summary>
		piSingleFocus = 33,						// I - R/W	 (MALS step)

		/// <summary>Sets the intensity of general channel 1~8. These values are all consecutive integer values. We reserve a range of values for 32 channels.</summary>
		piIntensityGeneralChannel_1 = 34,		// D - R/W	 (%)
		piIntensityGeneralChannel_2 = 35,		// D - R/W	 (%)
		piIntensityGeneralChannel_3 = 36,		// D - R/W	 (%)
		piIntensityGeneralChannel_4 = 37,		// D - R/W	 (%)
		piIntensityGeneralChannel_5 = 38,		// D - R/W	 (%)
		piIntensityGeneralChannel_6 = 39,		// D - R/W	 (%)
		piIntensityGeneralChannel_7 = 40,		// D - R/W	 (%)
		piIntensityGeneralChannel_8 = 41,		// D - R/W	 (%)
		/// <summary>Reserved for light channel expansion.</summary>
		//piIntensityGeneralChannel_32 = 65,	// D - R/W	 (%)

		/// <summary>Defines the cycle, in second, that determines how often vps reports will be logged for debug purposes.</summary>
		piVpsReportCycleSeconds = 70,			// D - R/W	 (seconds)

		/// <summary>Defines the data to be used for statistics at the time of reporting. Specifies how many recent seconds of data to use when calculating vps.</summary>
		piVpsReportTimeSeconds = 71,			// D - R/W	 (seconds)

		/// <summary>Sets MALS highest step for simulation.</summary>
		piSimulMalsHighestStep = 72,			// I - R/W	 (MALS step)

		/// <summary>Sets MALS lowest step for simulation.</summary>
		piSimulMalsLowestStep = 73,				// I - R/W	 (MALS step)

		//piNextParameterValue = 74,

		/// <summary>Unsupported parameter was requested. Also used as "end" marker internally.</summary>
		piInvalidParameter = 100
	};

	enum eCameraColor
	{
		ccColor = 0,
		ccMono = 1
	};

	enum eSaveFormat
	{
		sfNone = 0,
		sfBmp = 1,
		sfCzi = 2,
		sfZip = 3,
		sfTiff = 4,
		sfJpg = 5,
		sfMax
	};

	enum eReflexCorrectionMethod
	{
		rcNone = 0,
		rcCustomized_1 = 1, // reserved
		rcMedian = 2,
		rcAverage = 3,
		rcMin = 4,
		rcMedianExpFiltered = 5,
		rcAverageExpFiltered = 6,
		rcMax
	};

	enum eFocusMeasureMethod
	{
		fmModifiedLaplacian = 0,
		fmGrayLevelLocalVariance = 1,
		fmTenengradGradient = 2,
		//fmCustomized1, // reserved, not yet implemented
		//fmCustomized2 // reserved, not yet implemented
		fmMax
	};

	#define SDOAQ_AM56_DLL_EDOF_ZMAP				56
	#define SDOAQ_AM60_DLL_EDOF_DEMO_CMP			60
	#define SDOAQ_AM61_DLL_EDOF_HELICONFOCUS		61


	// gets information about parameter
	// The correct value is read after the SDOAQ_Initialize API completes.
	__declspec(dllexport) eErrorCode SDOAQ_IsParameterAvailable(eParameterId parameterId, bool* pIsAvailable);
	__declspec(dllexport) eErrorCode SDOAQ_IsParameterWritable(eParameterId parameterId, bool* pIsWritable);
	__declspec(dllexport) eErrorCode SDOAQ_GetParameterType(eParameterId parameterId, eParameterType* pParameterType);

	// manages integer value parameters
	__declspec(dllexport) eErrorCode SDOAQ_GetIntParameterRange(eParameterId parameterId, int* pMinValue, int* pMaxValue);
	__declspec(dllexport) eErrorCode SDOAQ_GetIntParameterValue(eParameterId parameterId, int* pValue);
	__declspec(dllexport) eErrorCode SDOAQ_SetIntParameterValue(eParameterId parameterId, int value);

	// manages double value parameters
	__declspec(dllexport) eErrorCode SDOAQ_GetDblParameterRange(eParameterId parameterId, double* pMinValue, double* pMaxValue);
	__declspec(dllexport) eErrorCode SDOAQ_GetDblParameterValue(eParameterId parameterId, double* pValue);
	__declspec(dllexport) eErrorCode SDOAQ_SetDblParameterValue(eParameterId parameterId, double value);

	// manages string value parameters	
	// Read the parameterId value of string type and save it in the memory pointed to by ¡®pString¡¯ having the size of the value indicated by ¡®pSize¡¯.
	// Write the size of the saved string in ¡®pSize¡¯ again.
	__declspec(dllexport) eErrorCode SDOAQ_GetStringParameterValue(eParameterId parameterId, char* pString, int* pSize);
	// Set up to NULL in the memory pointed to by ¡®pString¡¯.
	__declspec(dllexport) eErrorCode SDOAQ_SetStringParameterValue(eParameterId parameterId, const char* pString);

	// manages camera parameter
	/// <summary>
	/// This function requests the current ROI and binning value of camera.
	/// The full ROI is changed when binning is applied. Therefore, the size of image to be acquired is adjusted based on the current FOV and the binning value.
	/// This function should be called to check ROI after calling SDOAQ_SetCameraParameter() function.
	/// </summary>
	/// <param name="pWidth, pHeight">
	/// These values are the current ROI in pixels.
	/// </param>
	/// <param name="pBinning">
	/// This value will be pass 1 if the camera does not support binning.
	/// Value: 1(=1x1), 2(=2x2), 4(=4x4), Odd-sized matrices (e.g. 3X3) are not supported.
	/// </param>
	__declspec(dllexport) eErrorCode SDOAQ_GetCameraParameter(int* pWidth, int* pHeight, int* pBinning);

	/// <summary>
	/// Specifies the FOV and binning value.
	/// This function cannot be called while acquisition is in progress.
	/// </summary>
	/// <param name="FovWidth, FovHeight">
	/// The FovWidth and FovHeight are not the size of the image to be acquired, but the image size in pixels to be scanned by the camera sensor.
	/// </param>
	/// <param name="binning">
	/// This value is ignored if the camera does not support binning.
	/// Value: 1(=1x1), 2(=2x2), 4(=4x4), Odd-sized matrices (e.g. 3X3) are not supported.
	/// </param>
	__declspec(dllexport) eErrorCode SDOAQ_SetCameraParameter(int FovWidth, int FovHeight, int binning);

	/// <summary>
	/// Parameters of the sAcquisitionFixedParameters-struct must be defined before calling an acquisition function and must
	/// not be changed during a running continuous acquisition (preview) because a change will have an impact on the
	/// size of the predefined memory blocks allocated for the ring buffer.
	/// </summary>
	typedef struct sAcquisitionFixedParameters
	{
		/// <summary>The upper edge of the camera ROI used for acquisition in pixels. This value is 0 if full ROI should be used.</summary>
		int cameraRoiTop;
		/// <summary>The left edge of the camera ROI used for acquisition in pixels. This value is 0 if full ROI should be used.</summary>
		int cameraRoiLeft;
		/// <summary>The width of the camera ROI used for acquisition in pixels.</summary>
		int cameraRoiWidth;
		/// <summary>The height of the camera ROI used for acquisition in pixels.</summary>
		int cameraRoiHeight;
		/// <summary>
		/// The camera binning to be used. Available only if it is supported (camera binning).
		/// Value: 1(=1x1), 2(=2x2), 4(=4x4), Odd-sized matrices (e.g. 3X3) are not supported.
		///</summary>
		int cameraBinning;
	} AcquisitionFixedParameters;

	/* deprecated. Instead, use sAcquisitionFixedParameters */typedef struct sAcquisitionFixedParameters_V2
	{
		int cameraRoiTop;
		int cameraRoiLeft;
		int cameraRoiWidth;
		int cameraRoiHeight;
		int cameraBinning;
	} AcquisitionFixedParameters_V2;


	//////////////////////////////////////////////////////////////////////////////////////////////////////////
	//
	// Functions needed for auto adjustments. Only "one-shot" functionality is needed.
	//
	//////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// This function tries to adjust the exposure time of the camera that a given average brightness of
	/// the image is reached. Illumination is not be touched by this function.
	/// This function is a blocking "one-shot"-function. No continuous adjustment is done.
	/// </summary>
	/// <param name="maxExposureTime">
	/// Exposure should be increased only up to this expsoure time in microseconds. If the average brightness
	/// of the image can not be reached with this maximal exposure time, "ecAutoAdjustTargetNotReached"
	/// should be returned.
	/// </param>
	/// <param name="targetImageBrightness">
	/// Defines the average image brightness which should be reached.
	/// </param>
	/// <param name="roiLeft, roiTop, roiWidth, roiHeight">
	/// These values are defining a ROI in which the average brightness should be reached.
	/// </param>
	__declspec(dllexport) eErrorCode SDOAQ_AutoExposure(int maxExposureTime, int targetImageBrightness,
		int roiLeft, int roiTop, int roiWidth, int roiHeight);

	/// <summary>
	/// This function tries to adjust the illumination that a given average brightness of the image
	/// is reached. The exposure time of the camera is not be changed by this function.
	/// This function is a blocking "one-shot"-function. No continuous adjustment is done.
	/// </summary>
	/// <param name="illuminationType">
	/// Defines which type of illumination should be adjusted.
	/// Can we always use the "current" active illumination (what is active? Intensity > 0)
	/// Does it make sense to adjust each "ring" extra?
	/// </param>
	/// <param name="maxIntensity">
	/// Maximal intensity in percent which should be used to reach the target brightness. If the target
	/// average brightness of the image can not be reached with this maximal intenstiy,
	/// "ecAutoAdjustTargetNotReached" should be returned.
	/// </param>
	/// <param name="targetImageBrightness">
	/// Defines the average image brightness which should be reached.
	/// </param>
	/// <param name="roiLeft, roiTop, roiWidth, roiHeight">
	/// These values are defining a ROI in which the average brightness should be reached.
	/// </param>
	__declspec(dllexport) eErrorCode SDOAQ_AutoIlluminate(int illuminationType, int maxIntensity,
		int targetImageBrightness, int roiLeft, int roiTop, int roiWidth, int roiHeight);

	/// <summary>
	/// This function executes a one shot auto white balance.
	/// </summary>
	/// </param>
	/// <param name="roiLeft, roiTop, roiWidth, roiHeight">
	/// These values are defining a ROI to adjust the white balance.
	/// </param>
	__declspec(dllexport) eErrorCode SDOAQ_AutoWhiteBalance(int roiLeft, int roiTop, int roiWidth, int roiHeight);

	/// <summary>
	/// After a call of SDOAQ_PlayFocusStack() or SDOAQ_PlayEdof()
	/// focus stacks or edof images are acquired in continuously until a call of SDOAQ_StopStack() or SDOAQ_StopEdof() is done.
	/// During this continuous acquisition SDOAQ_PlayCallback() is called
	/// after every complete focus stack or edof acquisition by the SDO acquisition engine.
	/// </summary>
	/* deprecated. Instead, use SDOAQ_PlayCallback */typedef void(__stdcall* SDOAQ_ContinuousAcquisitionCallback)(eErrorCode errorCode, int lastFilledRingBufferEntry);
	typedef void(__stdcall* SDOAQ_PlayCallback)(eErrorCode errorCode, int lastFilledRingBufferEntry);

	//////////////////////////////////////////////////////////////////////////////////////////////////////////
	//
	// Functions and types needed to acquire a single focus stack or an focus stack preview ...
	//
	//////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// This function acquires one complete focus stack using the given acquisition parameters and
	/// delivers these focus stack images back in the ppFocusImages-Buffers.
	/// These ppFocusImages-Buffers are allocated by the client and are filled by this function.
	/// This function is blocking!
	/// </summary>
	/// <param name="pAcquisitionParams">
	/// This struct holds acquisition parameters.
	/// Please refer to sAcquisitionFixedParameters.
	/// </param>
	/// <param name="pPositions">The list of positions used to acquire the focus stack.</param>
	/// <param name="positionsCount">Number of positions in the position list.</param>
	/// <param name="ppFocusImages">
	/// Array with pointers to enough memory for each single focus stack image acquired.
	/// This memory is pre allocated by the client.
	/// Example:
	///    ringBufferSize == 2 and positionsCount == 4 =>
	///       ppFocusImages[0] => image acquired at MALS position 0 and for ring buffer entry  0
	///       ppFocusImages[1] => image acquired at MALS position 1 and for ring buffer entry  0
	///       ppFocusImages[2] => image acquired at MALS position 2 and for ring buffer entry  0
	///       ppFocusImages[3] => image acquired at MALS position 3 and for ring buffer entry  0
	///       ppFocusImages[4] => image acquired at MALS position 0 and for ring buffer entry  1
	///       ppFocusImages[5] => image acquired at MALS position 1 and for ring buffer entry  1
	///       ppFocusImages[6] => image acquired at MALS position 2 and for ring buffer entry  1
	///       ppFocusImages[7] => image acquired at MALS position 3 and for ring buffer entry  1
	/// If a image pointer (e.g. ppEdofImages[3]) == nullptr this image should not be generated/copied.
	/// </param>
	/// <param name="pFocusImagesBufferSizes">
	/// Pointer to an array holding the size of the allocated memory for every ppFocusImages pointer in bytes.
	/// These values should be checked before acquisition/copying images to detect allocation errors and prevent
	/// exception faults.
	/// </param>
	/* deprecated. Instead, use SDOAQ_SingleShotFocusStack */__declspec(dllexport) eErrorCode SDOAQ_AcquireFocusStack(sAcquisitionFixedParameters* pAcquisitionParams, int* pPositions, int positionsCount, unsigned char** ppFocusImages, size_t* pFocusImagesBufferSizes);
	/* deprecated. Instead, use SDOAQ_SingleShotFocusStack */__declspec(dllexport) eErrorCode SDOAQ_AcquireFocusStack_V2(sAcquisitionFixedParameters_V2* pAcquisitionParams, int* pPositions, int positionsCount, unsigned char** ppFocusImages, size_t* pFocusImagesBufferSizes);

	__declspec(dllexport) eErrorCode SDOAQ_SingleShotFocusStack(
		sAcquisitionFixedParameters* pAcquisitionParams,
		int* pPositions, int positionsCount,
		unsigned char** ppFocusImages,
		size_t* pFocusImagesBufferSizes);

	/// <summary>
	/// This function starts a focus stack preview. Stack images are produced in a continuous
	/// way until SDOAQ_StopStack() is called.
	/// Every focus stack is acquired using currently set parameters, the values in pAcquisitionParams
	/// and the given MALS controller positions stored in pPositions. All focus stack images
	/// are copied to the given ring buffer memory. After this the given callback function
	/// "stackFinishedCb" is called. Subsequently a new focus stack is acquired and
	/// so on until SDOAQ_StopStack() is called.
	/// </summary>
	/// <param name="pAcquisitionParams">
	/// This struct holds acquisition parameters which must not be changed during continuous focus
	/// stack acquisition because they affect the pre allocated memory in the ring buffer.
	/// Please refer to sAcquisitionFixedParameters.
	/// </param>
	/// <param name="stackFinishedCb">This callback function is called after each stack acquisition.</param>
	/// <param name="pPositions">The list of positions used to acquire the focus stack.</param>
	/// <param name="positionsCount">Number of positions in the position list.</param>
	/// <param name="ringBufferSize">
	/// Number of ring buffer entries. Each ring buffer entry consists of a complete set of focus images.
	/// If the ring buffer size is 1, stack image is acquired only once and then stops.
	/// </param>
	/// <param name="ppFocusImages">
	/// Array with pointers to enough memory for each single focus stack image acquired.
	/// This memory is pre allocated by the client.
	/// Example:
	///    ringBufferSize == 2 and positionsCount == 4 =>
	///       ppFocusImages[0] => image acquired at MALS position 0 and for ring buffer entry  0
	///       ppFocusImages[1] => image acquired at MALS position 1 and for ring buffer entry  0
	///       ppFocusImages[2] => image acquired at MALS position 2 and for ring buffer entry  0
	///       ppFocusImages[3] => image acquired at MALS position 3 and for ring buffer entry  0
	///       ppFocusImages[4] => image acquired at MALS position 0 and for ring buffer entry  1
	///       ppFocusImages[5] => image acquired at MALS position 1 and for ring buffer entry  1
	///       ppFocusImages[6] => image acquired at MALS position 2 and for ring buffer entry  1
	///       ppFocusImages[7] => image acquired at MALS position 3 and for ring buffer entry  1
	/// If a image pointer (e.g. ppEdofImages[3]) == nullptr this image should not be generated/copied.
	/// </param>
	/// <param name="pFocusImagesBufferSizes">
	/// Pointer to an array holding the size of the allocated memory for every ppFocusImages pointer in bytes.
	/// These values should be checked before acquisition/copying images to detect allocation errors and prevent
	/// exception faults.
	/// </param>
	/* deprecated. Instead, use SDOAQ_PlayFocusStack */__declspec(dllexport) eErrorCode SDOAQ_StartContinuousFocusStack(sAcquisitionFixedParameters* pAcquisitionParams, SDOAQ_ContinuousAcquisitionCallback stackFinishedCb,
		int* pPositions, int positionsCount, int ringBufferSize, unsigned char** ppFocusImages, size_t* pFocusImagesBufferSizes);
	/* deprecated. Instead, use SDOAQ_PlayFocusStack */__declspec(dllexport) eErrorCode SDOAQ_StartContinuousFocusStack_V2(sAcquisitionFixedParameters_V2* pAcquisitionParams, SDOAQ_ContinuousAcquisitionCallback stackFinishedCb,
		int* pPositions, int positionsCount, int ringBufferSize, unsigned char** ppFocusImages, size_t* pFocusImagesBufferSizes);

	__declspec(dllexport) eErrorCode SDOAQ_PlayFocusStack(
		sAcquisitionFixedParameters* pAcquisitionParams,
		SDOAQ_PlayCallback stackFinishedCb,
		int* pPositions, int positionsCount,
		int ringBufferSize,
		unsigned char** ppFocusImages,		// array of (ringBufferSize * positionsCount) unsigned char* entries
		size_t* pFocusImagesBufferSizes);	// size of each buffer => size of array == (ringBufferSize * positionsCount);

	/* deprecated. Instead, use SDOAQ_PlaySingleFocus */__declspec(dllexport) eErrorCode SDOAQ_StartContinuousSingleFocusStack(sAcquisitionFixedParameters* pAcquisitionParams, SDOAQ_PlayCallback stackFinishedCb,
		int ringBufferSize, unsigned char** ppFocusImages, size_t* pFocusImagesBufferSizes);
	
	/// <summary>
	/// This function starts a focus stack preview with only one focus.
	/// MALS controller position is given by eParameterId piSingleFocus.
	/// The position can be changed without stopping focus acquisition.
	/// A image is produced in a continuous way until SDOAQ_StopFocusStack() is called.
	/// </summary>
	__declspec(dllexport) eErrorCode SDOAQ_PlaySingleFocus(
		sAcquisitionFixedParameters* pAcquisitionParams,
		SDOAQ_PlayCallback singleFinishedCb,
		int ringBufferSize,
		unsigned char** ppFocusImage,		// array of (ringBufferSize) unsigned char* entries
		size_t* pFocusImageBufferSizes);	// size of each buffer => size of array == (ringBufferSize);

	/// <summary>
	/// This function requests a stop of the running continuous focus stack acquisition. After this function
	/// returns the ring buffer allocated by the client can be released. No further calls of the callback will be done.
	/// </summary>
	/* deprecated. Instead, use SDOAQ_StopFocusStack */__declspec(dllexport) eErrorCode SDOAQ_StopContinuousFocusStack();
	__declspec(dllexport) eErrorCode SDOAQ_StopFocusStack();


	//////////////////////////////////////////////////////////////////////////////////////////////////////////
	//
	// Functions and types needed to acquire a single EDoF image or an EDoF preview ...
	// including step map, quality map, height map and pointcloud.
	//
	//////////////////////////////////////////////////////////////////////////////////////////////////////////

	#define NAME_STEP_MAP "StepMap"
	#define NAME_EDOF_IMAGE "EDoF"
	#define NAME_MIDDLE_STEP_IMAGE "MiddleStep"
	#define NAME_QUALITY_MAP "QualityMap"
	#define NAME_HEIGHT_MAP "HeightMap"
	#define NAME_POINT_CLOUD "PointCloud"

	/// <summary>
	/// Parameters of the sEdofCalculationFixedParameters-struct must be defined before calling an
	/// EDoF acquisition function and must not be changed during a running continuous acquisition (preview)
	/// because a change will have an impact on the size of the pre-allocated memory blocks for
	/// the image buffers or the ring buffer.
	/// </summary>
	// deprecated struct.
	typedef struct sEdofCalculationFixedParameters
	{
		/// <summary>
		/// 1.0, 0.5, 0.25
		/// Deprecated.
		/// Although resize_ratio is included as a member in the structure, the resize ratio value is applied via the pi_edof_calc_resize_ratio parameter.
		/// </summary>
		double resize_ratio;
	} EdofCalculationFixedParameters;

	/// <summary>
	/// This function acquires a complete focus stack using the given acquisition parameters and
	/// calculates an Edof-Image, a step map, a quality map and a height map from the focus stack
	/// using the given parameters for calculation.
	/// All image buffers are allocated by the client and are filled by this function.
	/// This function is blocking!
	/// </summary>
	/// <param name="pAcquisitionParams">
	/// This struct holds acquisition parameters which must not be changed during continuous EDoF
	/// acquisition because they affect the pre allocated memory in the ring buffer.
	/// Please refer to sAcquisitionFixedParameters.
	/// </param>
	/// <param name="pPositions">The list of positions used to acquire the focus stack.</param>
	/// <param name="positionsCount">Number of positions in the position list.</param>
	/// <param name="pStepMapBuffer">Pointer to the allocated memory for step map</param>
	/// <param name="stepMapBufferSize">The size of the step map buffer</param>
	/// <param name="pEdofImageBuffer">Pointer to the allocated memory for EDoF image</param>
	/// <param name="edofImageBufferSize">The size of the EDoF image buffer</param>
	/// <param name="pQualityMapBuffer">Pointer to the allocated memory for quality map</param>
	/// <param name="qualityMapBufferSize">The size of the quality map buffer</param>
	/// <param name="pHeightMapBuffer">Pointer to the allocated memory for height map</param>
	/// <param name="heightMapBufferSize">The size of the height map buffer</param>
	/// <param name="pPointCloudBuffer">Pointer to the allocated memory for point cloud</param>
	/// <param name="pointCloudBufferSize">The size of the point cloud buffer</param>

	/* deprecated. Instead, use SDOAQ_SingleShotEdof */__declspec(dllexport) eErrorCode SDOAQ_AcquireEdof(sAcquisitionFixedParameters* pAcquisitionParams, sEdofCalculationFixedParameters* pCalculationParams, int* pPositions, int positionsCount,
		float* pStepMapBuffer, size_t stepMapBufferSize, unsigned char* pEdofImageBuffer, size_t edofImageBufferSize, float* pQualityMapBuffer, size_t qualityMapBufferSize, float* pHeightMapBuffer, size_t heightMapBufferSize);
	/* deprecated. Instead, use SDOAQ_SingleShotEdof */__declspec(dllexport) eErrorCode SDOAQ_AcquireEdof_V2(sAcquisitionFixedParameters_V2* pAcquisitionParams, sEdofCalculationFixedParameters* pCalculationParams, int* pPositions, int positionsCount,
		float* pStepMapBuffer, size_t stepMapBufferSize, unsigned char* pEdofImageBuffer, size_t edofImageBufferSize, float* pQualityMapBuffer, size_t qualityMapBufferSize, float* pHeightMapBuffer, size_t heightMapBufferSize, float* pPointCloudBuffer, size_t pointCloudBufferSize);

	__declspec(dllexport) eErrorCode SDOAQ_SingleShotEdof(sAcquisitionFixedParameters* pAcquisitionParams, int* pPositions, int positionsCount,
		float* pStepMapBuffer, size_t stepMapBufferSize, unsigned char* pEdofImageBuffer, size_t edofImageBufferSize,
		float* pQualityMapBuffer, size_t qualityMapBufferSize, float* pHeightMapBuffer, size_t heightMapBufferSize,
		float* pPointCloudBuffer, size_t pointCloudBufferSize);

	/// <summary>
	/// This function starts an EDoF-Preview. EDoF images are produced in a continuous way until
	/// SDOAQ_StopEdof() is called.
	/// First a focus stack is acquired using currently set parameters, the values in pAcquisitionParams
	/// and the given MALS controller positions stored in pPositions.
	/// From this set of focus stack images, a StepMap, a EDoF image, a QualityMap and a HeightMap
	/// is calculated using the currently stored parameters and pCalculationsParameters. This set of
	/// calculated images is copied to the next ring buffer memory set. After this the given callback
	/// function edofFinishedCb is called.
	/// After that a new focus stack is acquired and so on until SDOAQ_StopEdof() is called.
	/// </summary>
	/// <param name="pAcquisitionParams">
	/// This struct holds acquisition parameters which must not be changed during continuous EDoF
	/// acquisition because they affect the pre allocated memory in the ring buffer.
	/// Please refer to sAcquisitionFixedParameters.
	/// </param>	
	/// <param name="edofFinishedCb">This callback function is called after each EDoF-Calculation.</param>
	/// <param name="pPositions">The list of positions used to acquire the focus stack.</param>
	/// <param name="positionsCount">Number of positions in the position list.</param>
	/// <param name="ringBufferSize">
	/// Number of ring buffer entries. Each ring buffer entry consists of one EDoF image, one StepMap,
	/// one QualityMap and one HeightMap.
	/// If the ring buffer size is 1, EDoF image is acquired only once and then stops.
	/// </param>
	/// <param name="ppRingBufferImages">
	/// Pointer to array with pointers to enough memory for each single image acquired. This memory
	/// is pre allocated by the client.
	/// Example:
	///    ringBufferSize = 2 =>
	///       ppRingBufferImages[0] => EDoF image of ring buffer position 0
	///       ppRingBufferImages[1] => StepMap of ring buffer position 0
	///       ppRingBufferImages[2] => QualityMap of ring buffer position 0
	///       ppRingBufferImages[3] => HeightMap of ring buffer position 0
	///       ppRingBufferImages[4] => PintCloudData of ring buffer position 0
	///       ppRingBufferImages[5] => EDoF image of ring buffer position 1
	///       ppRingBufferImages[6] => StepMap of ring buffer position 1
	///       ppRingBufferImages[7] => QualityMap of ring buffer position 1
	///       ppRingBufferImages[8] => HeightMap of ring buffer position 1
	///       ppRingBufferImages[9] => PintCloudData of ring buffer position 1
	/// If a image pointer (e.g. ppRingBufferImages_V2[3]) == nullptr this image should not be generated/copied.
	/// </param>
	/// <param name="pRingBufferSizes">
	/// Pointer to an array holding the size of the allocated memory for every ppRingBufferImages pointer in bytes.
	/// These values should be checked before acquisition/copying images to detect allocation errors and prevent
	/// exception faults.
	/// If one of the array entries is zero, the associated image is not acquired / copied.
	/// </param>	
	/* deprecated. Instead, use SDOAQ_PlayEdof */__declspec(dllexport) eErrorCode SDOAQ_StartContinuousEdof(sAcquisitionFixedParameters* pAcquisitionParams, sEdofCalculationFixedParameters* pCalculationParams, SDOAQ_ContinuousAcquisitionCallback edofFinishedCb,
		int* pPositions, int positionsCount, int ringBufferSize, void** ppRingBufferImages, size_t* pRingBufferSizes);
	/* deprecated. Instead, use SDOAQ_PlayEdof */__declspec(dllexport) eErrorCode SDOAQ_StartContinuousEdof_V2(sAcquisitionFixedParameters_V2* pAcquisitionParams, sEdofCalculationFixedParameters* pCalculationParams, SDOAQ_ContinuousAcquisitionCallback edofFinishedCb,
		int* pPositions, int positionsCount, int ringBufferSize, void** ppRingBufferImages_V2, size_t* pRingBufferSizes_V2);

	__declspec(dllexport) eErrorCode SDOAQ_PlayEdof(
		sAcquisitionFixedParameters* pAcquisitionParams,
		SDOAQ_PlayCallback edofFinishedCb,
		int* pPositions, int positionsCount,
		int ringBufferSize,
		void** ppRingBufferImages,		// poniter to pointer array of (ringBufferSize * (stepMap* + edof* + qualityMap* + heightMap* + pointCloud*)) unsigned char* entries
		size_t* pRingBufferSizes);		// size of each image buffer => size of array == (ringBufferSize * 5);

	/// <summary>
	/// This function requests a stop of the running continuous EDoF acquisition. After this functions returns the ring buffer
	/// allocated by the client can be released. No further calls of the callback will be done.
	/// </param>
	/* deprecated. Instead, use SDOAQ_StopEdof */__declspec(dllexport) eErrorCode SDOAQ_StopContinuousEdof();
	__declspec(dllexport) eErrorCode SDOAQ_StopEdof();


	//////////////////////////////////////////////////////////////////////////////////////////////////////////
	//
	// Functions and types needed to acquire a single AF image or an AF preview ...
	//
	//////////////////////////////////////////////////////////////////////////////////////////////////////////

	#define NAME_AF_IMAGE "AF"

	/// <summary>
	/// After a call of SDOAQ_PlayAF()
	/// AF data and AF images are acquired in continuously until a call of SDOAQ_StopAF() is done.
	/// During this continuous acquisition SDOAQ_PlayAfCallback() is called
	/// after every complete auto focus data and image by the SDO acquisition engine.
	/// </summary>
	/* deprecated. Instead, use SDOAQ_PlayAfCallback */typedef void(__stdcall* SDOAQ_ContinuousAfCallback)(eErrorCode errorCode, int lastFilledRingBufferEntry, double dbBestFocusStep, double dbBestScore);
	typedef void(__stdcall* SDOAQ_PlayAfCallback)(eErrorCode errorCode, int lastFilledRingBufferEntry, double dbBestFocusStep, double dbBestScore);
	typedef void(__stdcall* SDOAQ_PlayAfCallbackEx)(eErrorCode errorCode, int lastFilledRingBufferEntry, double dbBestFocusStep, double dbBestScore, double dbMatchedFocusStep);

	/* deprecated. Instead, use SDOAQ_SingleShotAF */__declspec(dllexport) eErrorCode SDOAQ_AcquireAF(sAcquisitionFixedParameters_V2* pAcquisitionParams, sEdofCalculationFixedParameters* pCalculationParams,
		int* pPositions, int positionsCount, unsigned char* pAFImageBuffer, size_t AFImageBufferSize, double* pBestFocusStep, double* pBestScore);

	__declspec(dllexport) eErrorCode SDOAQ_SingleShotAF(
		sAcquisitionFixedParameters* pAcquisitionParams,
		int* pPositions, int positionsCount,
		unsigned char* pAFImageBuffer, size_t AFImageBufferSize,
		double* pBestFocusStep, double* pBestScore);

	__declspec(dllexport) eErrorCode SDOAQ_SingleShotAF_Ex(
		sAcquisitionFixedParameters* pAcquisitionParams,
		int* pPositions, int positionsCount,
		unsigned char* pAFImageBuffer, size_t AFImageBufferSize,
		double* pBestFocusStep, double* pBestScore, double* pMatchedSFocusStep);

	/* deprecated. Instead, use SDOAQ_PlayAF */__declspec(dllexport) eErrorCode SDOAQ_StartContinuousAF(sAcquisitionFixedParameters_V2* pAcquisitionParams, sEdofCalculationFixedParameters* pCalculationParams, SDOAQ_ContinuousAfCallback afFinishedCb,
		int* pPositions, int positionsCount, int ringBufferSize, void** ppRingBufferImages, size_t* pRingBufferSizes);

	__declspec(dllexport) eErrorCode SDOAQ_PlayAF(
		sAcquisitionFixedParameters* pAcquisitionParams,
		SDOAQ_PlayAfCallback afFinishedCb,
		int* pPositions, int positionsCount,
		int ringBufferSize,
		void** ppRingBufferImages,		// array of (ringBufferSize * AF) unsigned char* entries
		size_t* pRingBufferSizes);		// size of each image buffer => size of array == (ringBufferSize * 1);

	__declspec(dllexport) eErrorCode SDOAQ_PlayAF_Ex(
		sAcquisitionFixedParameters* pAcquisitionParams,
		SDOAQ_PlayAfCallbackEx afFinishedCb,
		int* pPositions, int positionsCount,
		int ringBufferSize,
		void** ppRingBufferImages,		// array of (ringBufferSize * AF) unsigned char* entries
		size_t* pRingBufferSizes);		// size of each image buffer => size of array == (ringBufferSize * 1);

	/* deprecated. Instead, use SDOAQ_StopAF */__declspec(dllexport) eErrorCode SDOAQ_StopContinuousAF();
	__declspec(dllexport) eErrorCode SDOAQ_StopAF();


	//////////////////////////////////////////////////////////////////////////////////////////////////////////
	//
	// Functions and types needed to acquire a single MF image or an MF preview ...
	//
	//////////////////////////////////////////////////////////////////////////////////////////////////////////

	#define NAME_MF_IMAGE "MF"

	/// <summary>
	/// After a call of SDOAQ_PlayMF()
	/// MF data and MF images are acquired in continuously until a call of SDOAQ_StopMF() is done.
	/// During this continuous acquisition SDOAQ_ContinuousMfCallback() is called
	/// after every complete auto multi focus information and image by the SDO acquisition engine.
	/// </summary>
	/* deprecated. Instead, use SDOAQ_PlayMfCallback */typedef void(__stdcall* SDOAQ_ContinuousMfCallback)(eErrorCode errorCode, int lastFilledRingBufferEntry, int countRects, int* pRectIdArray, int* pRectStepArray);
	typedef void(__stdcall* SDOAQ_PlayMfCallback)(eErrorCode errorCode, int lastFilledRingBufferEntry, int countRects, int* pRectIdArray, int* pRectStepArray);

	/* deprecated. Instead, use SDOAQ_PlayMF */__declspec(dllexport) eErrorCode SDOAQ_StartContinuousMF(sAcquisitionFixedParameters_V2* pAcquisitionParams, sEdofCalculationFixedParameters* pCalculationParams, SDOAQ_ContinuousMfCallback mfFinishedCb,
		int* pPositions, int positionsCount, const char* sFunctionScript, int ringBufferSize, void** ppRingBufferImages, size_t* pRingBufferSizes);

	__declspec(dllexport) eErrorCode SDOAQ_PlayMF(
		sAcquisitionFixedParameters* pAcquisitionParams,
		SDOAQ_PlayMfCallback mfFinishedCb,
		int* pPositions, int positionsCount,
		const char* sFunctionScript,
		int ringBufferSize,
		void** ppRingBufferImages,		// array of (ringBufferSize * MF) unsigned char* entries
		size_t* pRingBufferSizes);		// size of each image buffer => size of array == (ringBufferSize * 1);

	/* deprecated. Instead, use SDOAQ_PlayMF */__declspec(dllexport) eErrorCode SDOAQ_StopContinuousMF();
	__declspec(dllexport) eErrorCode SDOAQ_StopMF();

	/* deprecated. Instead, use SDOAQ_UpdatePlayMF */__declspec(dllexport) eErrorCode SDOAQ_UpdateContinuousMF(const char* sFunctionScript);
	__declspec(dllexport) eErrorCode SDOAQ_UpdatePlayMF(const char* sFunctionScript);


	//////////////////////////////////////////////////////////////////////////////////////////////////////////
	//
	// Functions and types needed to snap image set ...
	//
	//////////////////////////////////////////////////////////////////////////////////////////////////////////

	typedef SDOAQ_PlayCallback SDOAQ_SnapCallback;

	typedef struct sSnapParameters
	{
		void* version;
		union
		{
			struct sVer2
			{//version is 2
				const char* sSnapPath;
				const char* sConfigFilename;
				const char* sConfigData;
			} v2;
		};
	} SnapParameters;

	/// <summary>
	/// Acquire the focus stack using the given parameters WITHOUT stopping the current acquisition operation
	/// and save the raw images and result
	/// </summary>
	__declspec(dllexport) eErrorCode SDOAQ_PlaySnap(
		SDOAQ_SnapCallback snapCb,
		int* pPositions, int positionsCount,
		const SnapParameters* pSnapParameters);

	/// <summary>
	/// Acquire the focus stack using the given parameters WITH stopping the current acquisition operation 
	/// and save the raw images and result. Stop the acquisition operation after snap. 
	/// </summary>
	__declspec(dllexport) eErrorCode SDOAQ_PlaySnap_and_Stop(
		SDOAQ_SnapCallback snapCb,
		int* pPositions, int positionsCount,
		const SnapParameters* pSnapParameters);

#ifdef __cplusplus
}
#endif