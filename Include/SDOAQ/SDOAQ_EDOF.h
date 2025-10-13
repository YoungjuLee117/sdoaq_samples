/* SDOAQ_EDOF.h

	Comments : This file exports all types and functions required to directly access the SDO EDoF algorithm.
	Date     : 2025/06/09
	Author   : YoungJu Lee
	Copyright (c) 2019 SD Optics,Inc. All rights reserved.

	========================================================================================================================================================
	Revision history
	========================================================================================================================================================
	Version     date      Author         Descriptions
	--------------------------------------------------------------------------------------------------------------------------------------------------------
	 2.8.2  2025.06.09  YoungJu Lee     - Added EDoF algorithm interface
	--------------------------------------------------------------------------------------------------------------------------------------------------------
	 2.8.6  2020.07.15  YoungJu Lee     - Added CUDA-based EDoF algorithm interface
	--------------------------------------------------------------------------------------------------------------------------------------------------------
	 2.8.7  2025.09.00  YoungJu Lee		- Added new items to eFocusMeasureMethod enumeration
	--------------------------------------------------------------------------------------------------------------------------------------------------------
*/

#pragma once

#ifdef __cplusplus
extern "C"
{
#endif

	enum SDOAQ_EDOF_ErrorCode
	{
		SUCCESS = 1,
		ERROR_NOT_INITIALIZED = -1,
		ERROR_CALIB_DATA = -2,
		ERROR_PROCESS_STOPPED = -3,
		ERROR_INPUT_PARAMETERS = -4,
		ERROR_OUTPUT_PARAMETERS = -5,
		ERROR_INVALID_FOCUS = -6,
		ERROR_NOT_LICENSED = -7,
	};

	/*
	* refer to Shree K. Nayar, "Shape form focus", 1989
	* Modified Laplacian method is recommended for default focus measure
	*/
	enum class SDOAQ_EDOF_FocusMeasure
	{
		// common
		MODIFIED_LAPLACIAN = 0,
		TENENGRAD_GRADIENT = 2,
		BRIGHTNESS = 3,
		DARKNESS = 4,

		// based-on cpu
		GRAYLEVEL_LOCAL_VARIANCE = 1,
		
		// based-on cuda
		//AVERAGE = 5,
	};

	struct SDOAQ_EDOF_FocalStackParams
	{
		SDOAQ_EDOF_FocusMeasure focus_measure; // default = SDOAQ_EDOF_FocusMeasure::MODIFIED_LAPLACIAN

		// input image parameters
		// memory size for one image = image_width * image_height * num_channel * byte_per_channel (bytes)
		int image_num;
		int image_width;
		int image_height;

		// image_offset_x and image_offset_y represent an origin of current image against image sensor origin with full resolution
		// x_fullframe = binning_x * x_local + image_offset_x
		// y_fullframe = binning_y * y_local + image_offset_y
		int image_offset_x;
		int image_offset_y;

		int binning_x = 1;		// 1, 2, 4 (default = 1)
		int binning_y = 1;		// 1, 2, 4 (default = 1)

		int num_channel;		// 1, 3, 4
		int byte_per_channel;	// 1, 2,...
		int num_padding_bit;	// less than 8

		// ROI bounds (not supported yet)
		int roi_top;
		int roi_left;
		int roi_width;
		int roi_height;

		/*
		range = {0.25f, 0.5f, 1.0f}
		default = 0.5f
		*/
		double resize_ratio;

		/*
		range = {3 ~ 15}
		recommendation
		default = 3 (when resize_ratio = 0.25f)
		default = 5 (when resize_ratio = 0.5f)
		default = 9 (when resize_ratio = 1.0f)
		*/
		int pixelwise_kernel_size;

		/*
		range = {0 ~ 16}
		default = 4
		*/
		int pixelwise_iteration;

		/*
		range = {-1 ~ 20},
		default = -1
		if depthwise_kernel_size == -1, this parameter is automatically adjusted.
		*/
		int depthwise_kernel_size; // larger kernel size -> less noise

		/*
		range = {0.0f ~ 9.9f}
		default = 2.0f
		*/
		double depth_quality_th;

		/*
		range = {0 ~ 9}
		default = 4
		if bilateral_sigma_color <= 0, bilateral filtering is replaced with Gaussian filtering
		*/
		double bilateral_sigma_color;

		/*
		range = {0 ~ 15}
		default = 3 (when resize_ratio = 0.25f)
		default = 5 (when resize_ratio = 0.5f)
		default = 9 (when resize_ratio = 1.0f)
		if bilateral_sigma_space <= 0, bilateral filtering is disabled.
		*/
		double bilateral_sigma_space;

		int num_thread; // -1 for max. num. of thread

		/*
		* this flag enables image scale correction
		* more computation needed
		* edof image has a constant pixel pitch which refer to scale_correction_dst_step
		*/
		bool is_scale_correction_enabled; // default = false;

		/*
		* reference MALS step for image scale correction
		* range = {MALS_MIN_STEP ~ MALS_MAX_STEP}
		*/
		int scale_correction_dst_step;
	};

	struct SDOAQ_EDOF_ImageParams
	{
		bool is_allocated; //For unnecessary output image, set this false, this parameter will be deprecated
		int image_width;
		int image_height;

		// image_offset_x and image_offset_y represent an origin of current image against image sensor origin with full resolution
		// x_fullframe = binning_x * x_local + image_offset_x
		// y_fullframe = binning_y * y_local + image_offset_y
		int image_offset_x;
		int image_offset_y;

		int binning_x = 1;		// 1, 2, 4 (default = 1)
		int binning_y = 1;		// 1, 2, 4 (default = 1)

		int num_channel;		// 1, 3, 4
		int byte_per_channel;	// 1, 2,...
		int num_padding_bit;	// less than 8
		bool is_floating_point;	// for floating-point depth

		// if SDEDOF_FocalStackParams.is_scale_correction_enabled == true, set this flag true, too
		bool is_scale_correction_enabled; // default = false;

		/*
		* must be same with SDEDOF_FocalStackParams.scale_correction_dst_step
		* reference MALS step for image scale correction
		* range = {MALS_MIN_STEP ~ MALS_MAX_STEP}
		*/
		int scale_correction_dst_step;
	};

	__declspec(dllexport) int SDOAQ_EDOF_InitializeFromCalibFile(const char* calib_file_name);

	__declspec(dllexport) int SDOAQ_EDOF_Finalize();

	__declspec(dllexport) int SDOAQ_EDOF_GetVersion();

	// return negative number when algorithm failed
	__declspec(dllexport) int SDOAQ_EDOF_Run(
		SDOAQ_EDOF_FocalStackParams* in_params, unsigned char** in_images, unsigned int* in_steps,
		SDOAQ_EDOF_ImageParams* out_edof_params, unsigned char* out_edof_image
	);


	//================================================================================================
	//		CUDA-based EDOF
	//================================================================================================

	struct SDOAQ_CUDA_EDOF_Params
	{
		// number of MALS steps in a scan ( = number of images in a scan)
		int numMalsStep = 0;

		// image width and height of image (scanned raw image)
		int imageWidth = 0;
		int imageHeight = 0;
		int imageOffsetX = 0;
		int imageOffsetY = 0;

		// color image:3, mono image:1
		int pixelColorType = 3;

		// image sub-sampling for heightmap estimation (0.25, 0.5, 1)
		double samplingMode = 0.5;

		// larger value -> smoother heightmap, larger value -> more computation -> need more time
		int pixelwiseKernelIteration = 8;

		// threshold of depth-map noise filtering
		double depthThreshold = 0.0; // default 2.5

		// first MALS step and MALS step interval in a scan
		// ex) firstStep = 110, stepInterval = 10
		// scanned MALS steps are = 100,110,120,130...
		int stepInterval = 0;
		int firstStep = 0;

		// this flag enables image scale correction, more computation needed
		// edof image has a constant pixel pitch which refer to scaleCorrectionDstStep
		bool isScaleCorrectionEnabled = false;

		// reference MALS step for image scale correction
		// manually adjust image scale correction destination step(MALS) = {MALS_MIN_STEP ~ MALS_MAX_STEP}
		// auto-adjustment: negative number (ex. -1)
		int scaleCorrectionDstStep = -1;
	};


	//------------------------------------------------------------------------------------------------------------------
	// Returns the version number of the SDOAQ_CUDAEDOF library.
	// Return: Version number as an integer (e.g., 1003 for v1.3)
	//------------------------------------------------------------------------------------------------------------------
	__declspec(dllexport) int SDOAQ_CUDAEDOF_GetVersion();


	//------------------------------------------------------------------------------------------------------------------
	// Checks if the system supports CUDA and the required GPU functions for EDoF.
	// Return: positive value if available, negative value otherwise
	// The absolute value of the return indicates the compute capability of the installed NVIDIA.
	//------------------------------------------------------------------------------------------------------------------
	__declspec(dllexport) int SDOAQ_CUDAEDOF_CheckAvailability();


	//------------------------------------------------------------------------------------------------------------------
	// Initialize EDoF algorithm library with EDoF parameter and calibration file:
	// Sets up internal algorithm state and prepares necessary calibration information for accurate image generating.
	// Call SDOAQ_CUDAEDOF_InitializeLibrary() when calibration data or core parameters are changed.
	// For updating certain tunable parameters at runtime, use SDOAQ_CUDAEDOF_UpdateParams() without reinitializing.
	// Return: 0 on success, negative value on error
	//------------------------------------------------------------------------------------------------------------------
	__declspec(dllexport) int SDOAQ_CUDAEDOF_InitializeLibrary(SDOAQ_CUDA_EDOF_Params param, const char* calibFileName);


	//------------------------------------------------------------------------------------------------------------------
	// Finalizes and releases all resources associated with the EDoF library.
	// This should be called when the library is no longer needed.
	// Return: 0 on success, negative value on error
	//------------------------------------------------------------------------------------------------------------------
	__declspec(dllexport) int SDOAQ_CUDAEDOF_FinalizeLibrary();


	//------------------------------------------------------------------------------------------------------------------
	// Add focus stack image to the algorithm:
	// Adds a single image from the focus stack to the EDoF processing queue at the specified focus depth index.
	// note: Multiple images (e.g., 10~20) should be added before running the final algorithm.
	// Return: 0 on success, negative value on error
	//------------------------------------------------------------------------------------------------------------------
	__declspec(dllexport) int SDOAQ_CUDAEDOF_AddImage(unsigned char* image, int depthIndex);


	//------------------------------------------------------------------------------------------------------------------
	// Run EDoF algorithm and generate output image:
	// Executes the CUDA EDoF algorithm using the previously added focus stack images.
	// return negative number when algorithm failed (not initialized or no output data)
	// note: Ensure that the output buffer is properly allocated and registered if needed.	
	// Return: 0 on success, negative value on error
	//------------------------------------------------------------------------------------------------------------------
	__declspec(dllexport) int SDOAQ_CUDAEDOF_Run(unsigned char* edof);


	//------------------------------------------------------------------------------------------------------------------
	// Register memory with cudaHostRegister:
	// Registers the host memory to enable fast, direct memory access between the CPU and GPU.
	// note: The memory must be de - registered later using SDOAQ_CUDAEDOF_UnregisterMemory().
	// Return: 0 on success, negative value on error
	//------------------------------------------------------------------------------------------------------------------
	__declspec(dllexport) int SDOAQ_CUDAEDOF_RegisterMemory(void* ptr, int size);


	//------------------------------------------------------------------------------------------------------------------
	// Unregister memory with cudaHostUnregister:
	// Releases the previously pinned memory, restoring it to normal pageable host memory.
	// Prevents memory leaks and system resource issues caused by excessive pinned memory usage.
	// note: Always unregister memory after processing is complete to free system resources.
	// Return: 0 on success, negative value on error
	//------------------------------------------------------------------------------------------------------------------
	__declspec(dllexport) int SDOAQ_CUDAEDOF_UnregisterMemory(void* ptr);


	//------------------------------------------------------------------------------------------------------------------
	// Updates internal EDoF algorithm parameters at runtime without reinitialization.
	// Return: 0 on success, negative value on error
	//------------------------------------------------------------------------------------------------------------------
	__declspec(dllexport) int SDOAQ_CUDAEDOF_UpdateParams(int pixelwiseKernelIteration, int pixelwiseKernelSize, int depthwiseKernelSize, double depthThreshold);


#ifdef __cplusplus
}
#endif
