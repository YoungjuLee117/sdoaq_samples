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

  * SdoaqAutoFocus\SdoaqAutoFocus.vcxproj: A sample program to run autofocus operation.
    ![image](https://github.com/YoungjuLee117/sdoaq_samples/assets/93625956/c2011489-6781-4f6b-b254-63871555be8c)

  * SdoaqEdof\SdoaqEdof.vcxproj: A sample program to acquire EDOF images. It includes calibration data settings, ROI, scan area, and algorithm-related parameter settings.
    ![image](https://github.com/YoungjuLee117/sdoaq_samples/assets/93625956/00c9c198-2503-4fe5-a511-08600ebf613a)

  * SdoaqMultiFocus\SdoaqMultiFocus.vcxproj: A sample program to run multi-focus operation.
    ![스크린샷 2024-02-06 092645](https://github.com/YoungjuLee117/sdoaq_samples/assets/93625956/9f71c3a0-0646-4e94-bfa9-29669fa4bfb2)

  * SdoaqCameraFrameCallback\SdoaqCameraFrameCallback.vcxproj: A sample program that receives only the camera frame.
    ![image](https://github.com/YoungjuLee117/sdoaq_samples/assets/93625956/c16a239c-b3e9-4cf7-827d-ae7ff8add725)

  * SdoaqMultiWS\sdoaqMultiWS.vcxproj: A sample program that run multiple WiseScopes.
    ![스크린샷 2024-04-08 115441](https://github.com/YoungjuLee117/sdoaq_samples/assets/93625956/23a06cc4-1c15-42b8-8bac-944ecf8c4672)

  * SdoaqMultiLighting\SdoaqMultiLighting.vcxproj: A sample program that run multiple lighting.
    ![image](https://github.com/YoungjuLee117/sdoaq_samples/assets/93625956/3f6bacf3-1d77-4e08-96ef-dd731a145368)

  * SdoaqMultiCameraFrameCallback\SdoaqMultiCameraFrameCallback.vcxproj: A sample program that receives only the multiple camera frame.
    ![image](https://github.com/YoungjuLee117/sdoaq_samples/assets/93625956/e831acf5-8f42-42bf-a28b-9841bf9fcd12)



  * ⚠️ Set working directory to project directory.
  * ⚠️ The sample program simulates with saved image files.
  * ⚠️ Compressed core DLL files (opencv_world4130.dll and xiapi64.dll) into a ZIP archive for distribution.
---
#### [updll.bat]
  * Update required dlls to system dlls.
