using System;
using System.Collections.Generic;
using SDOAQ_EDOF;
using SDOAQNet.Tool;

namespace SDOAQNet
{
	partial class SdoaqController
    {
        public int RunEdof(IntPtr[] focusImagePointerList, int[] focusList, 
            int imageSize, int colorByte,
            ref SDOAQ.SDOAQ_API.AcquisitionFixedParametersEx acqParam,
            double resize_ratio, int pixelwise_kernel_size, int pixelwise_iteration, double depth_quality_th, int dst_step)
        {            
            var inParams = new SDOAQ_EDOF_API.SDOAQ_EDOF_FocalStackParams
            {
                focus_measure = SDOAQ_EDOF_API.SDOAQ_EDOF_FocusMeasure.MODIFIED_LAPLACIAN,
                image_num = focusList.Length,
                image_width = acqParam.cameraRoiWidth,
                image_height = acqParam.cameraRoiHeight,
                image_offset_x = acqParam.cameraRoiLeft,
                image_offset_y = acqParam.cameraRoiTop,
                binning_x = 1,
                binning_y = 1,
                num_channel = colorByte, // depending on the captured image color
                byte_per_channel = 1,
                num_padding_bit = 0,

                resize_ratio = resize_ratio,
                pixelwise_kernel_size = pixelwise_kernel_size,

                pixelwise_iteration = pixelwise_iteration,

                depthwise_kernel_size = -1,

                depth_quality_th = depth_quality_th,

                bilateral_sigma_color = 6,
                bilateral_sigma_space = 5,
                num_thread = -1,
                is_scale_correction_enabled = true,

                scale_correction_dst_step = dst_step
            };

            var edofParams = new SDOAQ_EDOF_API.SDOAQ_EDOF_ImageParams
            {
                is_allocated = true,
                image_width = inParams.image_width,
                image_height = inParams.image_height,
                image_offset_x = inParams.image_offset_x,
                image_offset_y = inParams.image_offset_y,
                binning_x = inParams.binning_x,
                binning_y = inParams.binning_y,
                num_channel = inParams.num_channel,
                byte_per_channel = inParams.byte_per_channel,
                num_padding_bit = inParams.num_padding_bit,
                is_floating_point = false,
                is_scale_correction_enabled = inParams.is_scale_correction_enabled,
                scale_correction_dst_step = inParams.scale_correction_dst_step
            };

            var bufferEdofImage = new byte[imageSize];
            var rv_edof = SDOAQ_EDOF_API.SDOAQ_EDOF_Run(ref inParams, focusImagePointerList, focusList, ref edofParams, bufferEdofImage);

            var imgInfoList = new List<SdoaqImageInfo>();
            if (rv_edof >= 0)
            {
                imgInfoList.Add(new SdoaqImageInfo("Edof",
                    acqParam.cameraRoiWidth, acqParam.cameraRoiHeight,
                    acqParam.cameraRoiWidth * colorByte,
                    colorByte,
                    bufferEdofImage));

               CallBackMessageProcessed?.Invoke(this, new CallBackMessageEventArgs(emCallBackMessage.Edof, imgInfoList));
            }

            return rv_edof;
        }

		public int RunCudaEdof(IntPtr[] focusImagePointerList, int[] focusList,
		   int imageSize, int colorByte,
		   ref SDOAQ.SDOAQ_API.AcquisitionFixedParametersEx acqParam,
		   double resize_ratio, int pixelwise_iteration, double depth_quality_th, int dst_step, string calib_file)
		{	
			SDOAQ_CUDA_EDOF_API.SDOAQ_CUDA_EDOF_Params edofParams = new SDOAQ_CUDA_EDOF_API.SDOAQ_CUDA_EDOF_Params();
			edofParams.numMalsStep = focusList.Length;
			edofParams.imageWidth = acqParam.cameraRoiWidth;
			edofParams.imageHeight = acqParam.cameraRoiHeight;
			edofParams.imageOffsetX = acqParam.cameraRoiLeft;
			edofParams.imageOffsetY = acqParam.cameraRoiTop;
			edofParams.pixelColorType = colorByte;
			edofParams.samplingMode = resize_ratio;
			edofParams.pixelwiseKernelIteration = pixelwise_iteration;
			edofParams.depthThreshold = depth_quality_th;
			edofParams.isScaleCorrectionEnabled = true;
			edofParams.scaleCorrectionDstStep = dst_step;


			// 1. Initialize EDoF algorithm library with EDoF parameter and calibration file
			// Call SDOAQ_CUDAEDOF_InitializeLibrary() when calibration data or core parameters are changed.
			// For updating certain tunable parameters at runtime, use SDOAQ_CUDAEDOF_UpdateParams() without reinitializing.
			var rv_edof = SDOAQ_CUDA_EDOF_API.SDOAQ_CUDAEDOF_InitializeLibrary(ref edofParams, calib_file);
			if (0 > rv_edof)
			{				
				return rv_edof;
			}


			// 2. Register memory with cudaHostRegister
			for (int i = 0; i < focusList.Length; i++)
			{
				SDOAQ_CUDA_EDOF_API.SDOAQ_CUDAEDOF_RegisterMemory(focusImagePointerList[i], sizeof(byte) * imageSize);
			}


			// 3. Add focus stack image to the algorithm
			for (int i = 0; i < focusList.Length; i++)
			{
				SDOAQ_CUDA_EDOF_API.SDOAQ_CUDAEDOF_AddImage(focusImagePointerList[i], i);
			}


			// 4. Run EDoF algorithm and generate output image
			var bufferEdofImage = new byte[imageSize];
			unsafe
			{
				fixed (byte* p = bufferEdofImage)
				{
					IntPtr ptr = (IntPtr)p;

					SDOAQ_CUDA_EDOF_API.SDOAQ_CUDAEDOF_RegisterMemory(ptr, sizeof(byte) * imageSize);

					rv_edof = SDOAQ_CUDA_EDOF_API.SDOAQ_CUDAEDOF_Run(ptr);

					SDOAQ_CUDA_EDOF_API.SDOAQ_CUDAEDOF_UnregisterMemory(ptr);
				}
			}


			// 5. Unregister memory with cudaHostUnregister
			for (int i = 0; i < focusList.Length; i++)
			{
				SDOAQ_CUDA_EDOF_API.SDOAQ_CUDAEDOF_UnregisterMemory(focusImagePointerList[i]);
			}
			

			var imgInfoList = new List<SdoaqImageInfo>();
			if (rv_edof >= 0)
			{
				imgInfoList.Add(new SdoaqImageInfo("CUDA Edof",
					acqParam.cameraRoiWidth, acqParam.cameraRoiHeight,
					acqParam.cameraRoiWidth * colorByte,
					colorByte,
					bufferEdofImage));

				CallBackMessageProcessed?.Invoke(this, new CallBackMessageEventArgs(emCallBackMessage.Edof, imgInfoList));
			}

			return rv_edof;
		}
	}
}
