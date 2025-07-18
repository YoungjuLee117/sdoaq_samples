using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDOAQNet.Tool
{
    public class SdoaqCamInfo
    {
        public bool IsColor => ColorByte != 1;

        /// <summary>
        /// mono == 1 , color == 3
        /// </summary>
        public int ColorByte { get; set; } = 1; 

        /// <summary>
        /// roi width * height
        /// </summary>
        public int PixelSize => AcqParam.cameraRoiWidth * AcqParam.cameraRoiHeight;
        /// <summary>
        /// roi width * height * color byte 
        /// </summary>
        public int ImgSize => PixelSize * ColorByte;
        
        public int DataSize => PixelSize * sizeof(float);
        public int PixelWidth => AcqParam.cameraRoiWidth;
        public int PixelHeight => AcqParam.cameraRoiHeight;
        public int Edofrecsize => 5;

        private SDOAQ.SDOAQ_API.AcquisitionFixedParametersEx _acqParam;
        public SDOAQ.SDOAQ_API.AcquisitionFixedParametersEx AcqParam
        {
            get { return _acqParam; }
            set { _acqParam = value; }
        }

        public SdoaqCamInfo()
        {
            _acqParam = SDOAQ.SDOAQ_API.AcquisitionFixedParametersEx.Create();
        }

        public ref SDOAQ.SDOAQ_API.AcquisitionFixedParametersEx GetAcqParamRef()
        {
            return ref _acqParam;
        }

        public string GetCamRoi()
        {
            return $"{AcqParam.cameraRoiLeft},{AcqParam.cameraRoiTop},{AcqParam.cameraRoiWidth},{AcqParam.cameraRoiHeight}";
        }
    }
}
