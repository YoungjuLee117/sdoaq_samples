using System;
using System.Runtime.InteropServices;
using System.Text;

/* SDOAQ_EDOF.cs

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

namespace SDOAQ_EDOF
{
	public enum SDOAQ_EDOF_ErrorCode
	{
		SUCCESS = 1,
		ERROR_NOT_INITIALIZED = -1,
		ERROR_CALIB_DATA = -2,
		ERROR_PROCESS_STOPPED = -3,
		ERROR_INPUT_PARAMETERS = -4,
		ERROR_OUTPUT_PARAMETERS = -5,
		ERROR_INVALID_FOCUS = -6,
		ERROR_NOT_LICENSED = -7,
	}

	public static class SDOAQ_EDOF_API
	{
		private const string SDOAQ_DLL = "SDOAQ.dll";

		/*
		* refer to Shree K. Nayar, "Shape form focus", 1989
		* Modified Laplacian method is recommended for default focus measure
		*/
		public enum SDOAQ_EDOF_FocusMeasure
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
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
		public struct SDOAQ_EDOF_FocalStackParams
		{
			public SDOAQ_EDOF_FocusMeasure focus_measure; // default = SDOAQ_EDOF_FocusMeasure::MODIFIED_LAPLACIAN

			// input image parameters
			// memory size for one image = image_width * image_height * num_channel * byte_per_channel (bytes)
			public int image_num;
			public int image_width;
			public int image_height;

			// image_offset_x and image_offset_y represent an origin of current image against image sensor origin with full resolution
			// x_fullframe = binning_x * x_local + image_offset_x
			// y_fullframe = binning_y * y_local + image_offset_y
			public int image_offset_x;
			public int image_offset_y;

			public int binning_x;           // 1, 2, 4 (default = 1)
			public int binning_y;           // 1, 2, 4 (default = 1)

			public int num_channel;         // 1, 3, 4
			public int byte_per_channel;    // multi-byte not supported. use only 1
			public int num_padding_bit;     // less than 8

			// ROI bounds (not supported yet)
			public int roi_top;
			public int roi_left;
			public int roi_width;
			public int roi_height;

			// algorithm parameters
			public double resize_ratio; // 0.5, 0.25, 0.125...
			public int pixelwise_kernel_size; // larger kernel size -> smoother depthmap
			public int pixelwise_iteration;
			public int depthwise_kernel_size; // larger kernel size -> less noise, if depthwise_kernel_size == -1, this parameter is automatically adjusted.
			public double depth_quality_th;
			public double bilateral_sigma_color;
			public double bilateral_sigma_space;
			public int num_thread; // -1 for max. num. of thread

			[MarshalAs(UnmanagedType.I1)]
			public bool is_scale_correction_enabled;
			public int scale_correction_dst_step;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
		public struct SDOAQ_EDOF_ImageParams
		{
			[MarshalAs(UnmanagedType.I1)]
			public bool is_allocated; //For unnecessary output image, set this false, this parameter will be deprecated
			public int image_width;
			public int image_height;

			// image_offset_x and image_offset_y represent an origin of current image against image sensor origin with full resolution
			// x_fullframe = binning_x * x_local + image_offset_x
			// y_fullframe = binning_y * y_local + image_offset_y
			public int image_offset_x;
			public int image_offset_y;

			public int binning_x;           // 1, 2, 4 (default = 1)
			public int binning_y;           // 1, 2, 4 (default = 1)

			public int num_channel;         // 1, 3, 4
			public int byte_per_channel;    // 1 for unsigned char, 4 for single-precision floating point number
			public int num_padding_bit;     // less than 8

			[MarshalAs(UnmanagedType.I1)]
			public bool is_floating_point; // for floating-point depth
			[MarshalAs(UnmanagedType.I1)]
			public bool is_scale_correction_enabled;
			public int scale_correction_dst_step;
		}

		[DllImport(SDOAQ_DLL, CallingConvention = CallingConvention.StdCall)]
		public static extern int SDOAQ_EDOF_InitializeFromCalibFile(string calibFileName);

		[DllImport(SDOAQ_DLL, CallingConvention = CallingConvention.StdCall)]
		public static extern int SDOAQ_EDOF_Finalize();

		[DllImport(SDOAQ_DLL, CallingConvention = CallingConvention.StdCall)]
		public static extern int SDOAQ_EDOF_GetVersion();

		// return negative number when algorithm failed
		[DllImport(SDOAQ_DLL, CallingConvention = CallingConvention.StdCall)]
		public static extern int SDOAQ_EDOF_Run(
			ref SDOAQ_EDOF_FocalStackParams in_params,
			IntPtr[] in_images,
			int[] in_steps,
			ref SDOAQ_EDOF_ImageParams out_edof_params,
			byte[] out_edof_image
		);
	}

	//================================================================================================
	//		CUDA-based EDOF
	//================================================================================================

	public static class SDOAQ_CUDA_EDOF_API
	{
		private const string SDOAQ_DLL = "SDOAQ.dll";

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
		public struct SDOAQ_CUDA_EDOF_Params
		{
			// number of MALS steps in a scan ( = number of images in a scan)
			public int numMalsStep;

			// image width and height of image (scanned raw image)
			public int imageWidth;
			public int imageHeight;
			public int imageOffsetX;
			public int imageOffsetY;

			// mono image -> 1
			// color image -> 3
			public int pixelColorType;

			// image sub-sampling for heightmap estimation (0.25, 0.5, 1)
			public double samplingMode;

			// larger value -> smoother heightmap, larger value -> more computation -> need more time
			public int pixelwiseKernelIteration;

			// threshold of depth-map noise filtering
			public double depthThreshold; // default 2.5

			// first MALS step and MALS step interval in a scan
			// ex) firstStep = 110, stepInterval = 10
			// scanned MALS steps are = 100,110,120,130...
			public int stepInterval;
			public int firstStep;

			// this flag enables image scale correction, more computation needed
			// edof image has a constant pixel pitch which refer to scaleCorrectionDstStep
			[MarshalAs(UnmanagedType.I1)]
			public bool isScaleCorrectionEnabled;

			// reference MALS step for image scale correction
			// manually adjust image scale correction destination step(MALS) = {MALS_MIN_STEP ~ MALS_MAX_STEP}
			// auto-adjustment: negative number (ex. -1)
			public int scaleCorrectionDstStep;
		};

		//------------------------------------------------------------------------------------------------------------------
		// Returns the version number of the SDOAQ_CUDAEDOF library.
		// Return: Version number as an integer (e.g., 1003 for v1.3)
		//------------------------------------------------------------------------------------------------------------------
		[DllImport(SDOAQ_DLL, CallingConvention = CallingConvention.StdCall)]
		public static extern int SDOAQ_CUDAEDOF_GetVersion();


		//------------------------------------------------------------------------------------------------------------------
		// Checks if the system supports CUDA and the required GPU functions for EDoF.
		// Return: positive value if available, negative value otherwise
		// The absolute value of the return indicates the compute capability of the installed NVIDIA.
		//------------------------------------------------------------------------------------------------------------------
		[DllImport(SDOAQ_DLL, CallingConvention = CallingConvention.StdCall)]
		public static extern int SDOAQ_CUDAEDOF_CheckAvailability();


		//------------------------------------------------------------------------------------------------------------------
		// Initialize EDoF algorithm library with EDoF parameter and calibration file:
		// Sets up internal algorithm state and prepares necessary calibration information for accurate image generating.
		// Call SDOAQ_CUDAEDOF_InitializeLibrary() when calibration data or core parameters are changed.
		// For updating certain tunable parameters at runtime, use SDOAQ_CUDAEDOF_UpdateParams() without reinitializing.
		// Return: 0 on success, negative value on error
		//------------------------------------------------------------------------------------------------------------------
		[DllImport(SDOAQ_DLL, CallingConvention = CallingConvention.StdCall)]
		public static extern int SDOAQ_CUDAEDOF_InitializeLibrary(ref SDOAQ_CUDA_EDOF_Params param, string calibFileName);


		//------------------------------------------------------------------------------------------------------------------
		// Finalizes and releases all resources associated with the EDoF library.
		// This should be called when the library is no longer needed.
		// Return: 0 on success, negative value on error
		//------------------------------------------------------------------------------------------------------------------
		[DllImport(SDOAQ_DLL, CallingConvention = CallingConvention.StdCall)]
		public static extern int SDOAQ_CUDAEDOF_FinalizeLibrary();


		//------------------------------------------------------------------------------------------------------------------
		// Add focus stack image to the algorithm:
		// Adds a single image from the focus stack to the EDoF processing queue at the specified focus depth index.
		// note: Multiple images (e.g., 10~20) should be added before running the final algorithm.
		// Return: 0 on success, negative value on error
		//------------------------------------------------------------------------------------------------------------------
		[DllImport(SDOAQ_DLL, CallingConvention = CallingConvention.StdCall)]
		public static extern int SDOAQ_CUDAEDOF_AddImage(IntPtr imagePtr, int depthIndex);


		//------------------------------------------------------------------------------------------------------------------
		// Run EDoF algorithm and generate output image:
		// Executes the EDoF algorithm using the previously added focus stack images.
		// return negative number when algorithm failed (not initialized or no output data)
		// note: Ensure that the output buffer is properly allocated and registered if needed.	
		// Return: 0 on success, negative value on error
		//------------------------------------------------------------------------------------------------------------------
		[DllImport(SDOAQ_DLL, CallingConvention = CallingConvention.StdCall)]
		public static extern int SDOAQ_CUDAEDOF_Run(IntPtr edof);


		//------------------------------------------------------------------------------------------------------------------
		// Register memory with cudaHostRegister:
		// Registers the host memory to enable fast, direct memory access between the CPU and GPU.
		// note: The memory must be de - registered later using SDOAQ_CUDAEDOF_UnregisterMemory().
		// Return: 0 on success, negative value on error
		//------------------------------------------------------------------------------------------------------------------
		[DllImport(SDOAQ_DLL, CallingConvention = CallingConvention.StdCall)]
		public static extern int SDOAQ_CUDAEDOF_RegisterMemory(IntPtr addr, int size);


		//------------------------------------------------------------------------------------------------------------------
		// Unregister memory with cudaHostUnregister:
		// Releases the previously pinned memory, restoring it to normal pageable host memory.
		// Prevents memory leaks and system resource issues caused by excessive pinned memory usage.
		// note: Always unregister memory after processing is complete to free system resources.
		// Return: 0 on success, negative value on error
		//------------------------------------------------------------------------------------------------------------------
		[DllImport(SDOAQ_DLL, CallingConvention = CallingConvention.StdCall)]
		public static extern int SDOAQ_CUDAEDOF_UnregisterMemory(IntPtr addr);


		//------------------------------------------------------------------------------------------------------------------
		// Updates internal EDoF algorithm parameters at runtime without reinitialization.
		// Return: 0 on success, negative value on error
		//------------------------------------------------------------------------------------------------------------------
		[DllImport(SDOAQ_DLL, CallingConvention = CallingConvention.StdCall)]
		public static extern int SDOAQ_CUDAEDOF_UpdateParams(int pixelwiseKernelIteration, int pixelwiseKernelSize, int depthwiseKernelSize, double depthThreshold);
	}
}
