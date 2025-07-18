using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using SDOAQ;
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
            //----------------------------------------------------------------------------
            //
            //		Starting point of the EDOF algorithm execution.
            //
            //		Make sure to set each parameter to a suitable value.
            //
            //		Don't forget to specify the calibration file before proceeding.
            //
            //----------------------------------------------------------------------------
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
    }
}
