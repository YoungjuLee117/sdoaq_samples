using SDOAQ;

namespace SDOAQNet
{
    /// <summary>
    /// ICamera interface provides a standardized abstraction for low-level control of cameras
    /// based on the SDOAQ lib
    /// </summary>
    public interface ICamera
    {
        uint RecivedFrameCount { get; }

        bool SetTriggerMode(SDOAQ_API.eCameraTriggerMode triggerMode);
        bool SetExeSoftwareTrigger();

        bool SetFOV(int width, int height, int offset_X = 0, int offset_Y = 0, int bining = 1);
        bool GetFOV(out int width, out int height, out int offset_X, out int offset_Y, out int bining);

        bool SetExposureTime(int exposureTime);
        bool GetExposureTime(out int exposureTime);

        bool SetGrabState(SDOAQ_API.eCameraGrabbingStatus grabState);
        bool GetGrabState(out SDOAQ_API.eCameraGrabbingStatus grabState);

        bool SetGain(double gain);
        bool GetGain(out double gain);

        bool SetWhiteBalance(double red, double green, double blue);

        bool SetReverseX(bool bReverse);
        bool SetReverseY(bool bReverse);
    }
}
