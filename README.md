# SDOAQ_samples

These are sample programs that use the SDOAQ dll to acquire images and WSIO dll to display the results.

---
#### [cpp folder]
  * Sample projects implemented in c++.
---
#### [cs folder]
 * Sample projects implemented in c#.
---
#### [Include folder]
  * SDOAQ: SDO acqusition dlls and header files.
  * WSIO: ImageViewer dlls and header files.
---
#### [vs solution files]
  * SdoaqAllSample_CPP.sIn: All sample projects implemented in c++.
  * SdoaqAllSample_CS.sIn: All sample projects implemented in c#.
---
#### [project files]
  * SdoaqApiTester\SDOAQ_App.vcxproj: A sample program to check the operation of SDOAQ dll API. All parameters can be selected from combo box list to specify values. An examples of visualizing the EDoF result image in 3D is included.
   ![image](https://github.com/YoungjuLee117/sdoaq_samples/assets/93625956/a2042c0c-c6f8-4dcc-a344-3e43d91a10b2)

  * SdoaqAutoFocus\SdoaqAutoFocus.vcxproj: A sample program to check autofocus operation.
    ![image](https://github.com/YoungjuLee117/sdoaq_samples/assets/93625956/c2011489-6781-4f6b-b254-63871555be8c)

  * SdoaqEdof\SdoaqEdof.vcxproj: A sample program to acquire EDOF images. It includes calibration data settings, ROI, scan area, and algorithm-related parameter settings.
    ![image](https://github.com/YoungjuLee117/sdoaq_samples/assets/93625956/00c9c198-2503-4fe5-a511-08600ebf613a)

  * SdoaqCameraFrameCallback\SdoaqCameraFrameCallback.vcxproj: A sample program that receives only the camera frame.
    ![image](https://github.com/YoungjuLee117/sdoaq_samples/assets/93625956/9b71f640-72dd-4d4b-b497-1a34923308d1)

  * ⚠️ Set working directory to project directory.
  * ⚠️ The sample program simulates with saved image files.
---
#### [updll.bat]
  * Update required dlls to system dlls.
