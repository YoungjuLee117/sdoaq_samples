using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDOAQ;

namespace SDOAQNet
{
    partial class SdoaqController
    {

        /// <summary>
        /// The parameter name varies from camera API to camera API.
        /// In the example, the name is based on the Pylon API
        /// </summary>
        private class CameraParameterName
        {
            public const string Width = "Width";
            public const string Height = "Height";
            public const string OffsetX = "OffsetX";
            public const string OffsetY = "OffsetY";
            public const string Gain = "Gain";
            public const string ReverseX = "ReverseX";
            public const string ReverseY = "ReverseY";
            public const string BalanceRatioSelector = "BalanceRatioSelector";
            public const string BalanceRatioSelector_Red = "Red";
            public const string BalanceRatioSelector_Green = "Green";
            public const string BalanceRatioSelector_Blue = "Blue";
            public const string BalanceRatioAbs = "BalanceRatioAbs";
            public const string BalanceRatio = "BalanceRatio";
        }

        private uint _recivedFrameCount;
        uint ICamera.RecivedFrameCount => _recivedFrameCount;

        bool ICamera.SetTriggerMode(SDOAQ_API.eCameraTriggerMode triggerMode)
        {
            if (IsInitialize == false)
            {
                return false;
            }

            SelectMultiWS(CamIndex);

            return SDOAQ_API.SDOAQ_SetCameraTriggerMode(triggerMode) == SDOAQ_API.eErrorCode.ecNoError;
        }

        bool ICamera.SetExeSoftwareTrigger()
        {
            if (IsInitialize == false)
            {
                return false;
            }

            SelectMultiWS(CamIndex);

            return SDOAQ_API.SDOAQ_ExecCameraSoftwareTrigger() == SDOAQ_API.eErrorCode.ecNoError;
        }

        

        bool ICamera.SetFOV(int width, int height, int offset_X, int offset_Y, int bining)
        {
            if (IsInitialize == false)
            {
                return false;
            }

            SelectMultiWS(CamIndex);

            return SDOAQ_API.SDOAQ_SetCameraRoiParameter(width, height, offset_X, offset_Y, bining) == SDOAQ_API.eErrorCode.ecNoError;
        }

        bool ICamera.GetFOV(out int width, out int height, out int offset_X, out int offset_Y, out int bining)
        {
            width = 0;
            height = 0;
            offset_X = 0;
            offset_Y = 0;
            bining = 1;

            if (IsInitialize == false)
            {
                return false;
            }

            int cam_width = 0;
            int cam_height = 0;
            int cam_offset_X = 0;
            int cam_offset_Y = 0;
            int cam_binning = 0;

            SelectMultiWS(CamIndex);

            var rv = SDOAQ_API.SDOAQ_GetCameraRoiParameter(ref cam_width, ref cam_height, ref cam_offset_X, ref cam_offset_Y, ref cam_binning);

            width = cam_width;
            height = cam_height;
            offset_X = cam_offset_X;
            offset_Y = cam_offset_Y;
            bining = cam_binning;

            return rv == SDOAQ_API.eErrorCode.ecNoError;
        }

        bool ICamera.SetExposureTime(int exposureTime)
        {
            SelectMultiWS(CamIndex);

            return SDOAQ_API.SDOAQ_SetIntParameterValue(SDOAQ_API.eParameterId.piCameraExposureTime, exposureTime) == SDOAQ_API.eErrorCode.ecNoError;
        }

        bool ICamera.GetExposureTime(out int exposureTime)
        {
            SelectMultiWS(CamIndex);

            exposureTime = 0;

            if (IsInitialize == false)
            {
                return false;
            }

            var rv = SDOAQ_API.SDOAQ_GetIntParameterValue(SDOAQ_API.eParameterId.piCameraExposureTime, ref exposureTime);

            return rv == SDOAQ_API.eErrorCode.ecNoError;
        }

        bool ICamera.SetGrabState(SDOAQ_API.eCameraGrabbingStatus grabState)
        {
            if (IsInitialize == false)
            {
                return false;
            }
            SelectMultiWS(CamIndex);
            return SDOAQ_API.SDOAQ_SetCameraGrabbingStatus(grabState) == SDOAQ_API.eErrorCode.ecNoError;
        }

        bool ICamera.GetGrabState(out SDOAQ_API.eCameraGrabbingStatus grabState)
        {
            grabState = SDOAQ_API.eCameraGrabbingStatus.cgsOffGrabbing;
            if (IsInitialize == false)
            {
                return false;
            }
            SelectMultiWS(CamIndex);
            return SDOAQ_API.SDOAQ_GetCameraGrabbingStatus(out grabState) == SDOAQ_API.eErrorCode.ecNoError;
        }

        bool ICamera.SetGain(double gain)
        {
            SelectMultiWS(CamIndex);
            return SDOAQ_API.SDOAQ_SetCameraParameterDouble(CameraParameterName.Gain, gain) == SDOAQ_API.eErrorCode.ecNoError;
        }

        bool ICamera.GetGain(out double gain)
        {
            gain = 0;
            if (IsInitialize == false)
            {
                return false;
            }

            SelectMultiWS(CamIndex);
            var rv = SDOAQ_API.SDOAQ_GetCameraParameterDouble(CameraParameterName.Gain, out gain, SDOAQ_API.eCameraParameterType.cptValue);
            return rv == SDOAQ_API.eErrorCode.ecNoError;
        }

        bool ICamera.SetWhiteBalance(double red, double green, double blue)
        {
            if (IsInitialize == false)
            {
                return false;
            }

            SelectMultiWS(CamIndex);

            SDOAQ_API.eErrorCode rv = SDOAQ_API.eErrorCode.ecNoError;
            if (SDOAQ_API.eErrorCode.ecNoError != (rv = SDOAQ_API.SDOAQ_SetCameraParameterString(CameraParameterName.BalanceRatioSelector, CameraParameterName.BalanceRatioSelector_Red)))
            {
                return false;
            }

            if (SDOAQ_API.eErrorCode.ecNoError != (SDOAQ_API.SDOAQ_SetCameraParameterDouble(CameraParameterName.BalanceRatio, red)))
            {
                return false;
            }

            if (SDOAQ_API.eErrorCode.ecNoError != (SDOAQ_API.SDOAQ_SetCameraParameterString(CameraParameterName.BalanceRatioSelector, CameraParameterName.BalanceRatioSelector_Green)))
            {
                return false;
            }

            if (SDOAQ_API.eErrorCode.ecNoError != (SDOAQ_API.SDOAQ_SetCameraParameterDouble(CameraParameterName.BalanceRatio, green)))
            {
                return false;
            }

            if (SDOAQ_API.eErrorCode.ecNoError != (SDOAQ_API.SDOAQ_SetCameraParameterString(CameraParameterName.BalanceRatioSelector, CameraParameterName.BalanceRatioSelector_Blue)))
            {
                return false;
            }

            if (SDOAQ_API.eErrorCode.ecNoError != (SDOAQ_API.SDOAQ_SetCameraParameterDouble(CameraParameterName.BalanceRatio, blue)))
            {
                return false;
            }

            return true;
        }


        bool ICamera.SetReverseX(bool bReverse)
        {
            SelectMultiWS(CamIndex);

            return SDOAQ_API.SDOAQ_SetCameraParameterBool(CameraParameterName.ReverseX, bReverse) == SDOAQ_API.eErrorCode.ecNoError;

        }

        bool ICamera.SetReverseY(bool bReverse)
        {
            SelectMultiWS(CamIndex);

            return SDOAQ_API.SDOAQ_SetCameraParameterBool(CameraParameterName.ReverseY, bReverse) == SDOAQ_API.eErrorCode.ecNoError;
        }
    }
}
