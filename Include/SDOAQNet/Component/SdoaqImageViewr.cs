using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using SDOAQNet.Tool;
using SDOWSIO;

namespace SDOAQNet.Component
{
	public partial class SdoaqImageViewr : UserControl
	{
		private readonly bool _visiblePointCloud;
		public bool VisiblePointCloud => _visiblePointCloud;

        private bool _visiBleImageListBox = true;
        public bool VisiBleImageListBox
        {
            get => _visiBleImageListBox;

            set
            {
                if (_visiBleImageListBox != value)
                {
                    _visiBleImageListBox = value;

                    listbox_ImageList.Visible = value;

                    LayouyUpdate();
                }
            }
           
        }

		private SdoaqController _sdoaqObj;

		private List<SdoaqImageInfo> _imageList = new List<SdoaqImageInfo>();
		private SdoaqPointCloudInfo _pointCloudInfoInfo = null;
		private int _lastSelectImageIndex = 0;

		private int _stackImgCount = 0;
		private int _afImgCount = 0;
		private int _edofImgCount = 0;

        private const int WM_IV_EVENT = 0xA000;
        private const int ECD_BRIGHTNESS = 0;
        public SdoaqImageViewr(bool visiblePointCloud)
		{
			InitializeComponent();
            _visiblePointCloud = visiblePointCloud;

			this.Resize += UserControl_Resize;
			this.Disposed += UserControl_Disposed;
			this.listbox_ImageList.SelectedIndexChanged += listbox_SelectedIndexChanged;
			this.Load += UserControl_Load;
		}

		public void Set_SdoaqObj(SdoaqController sdoaqObj)
		{
			if (_sdoaqObj != null)
			{
				_sdoaqObj.CallBackMessageProcessed -= SdoaqObj_MessageProcessed;
			}
			_sdoaqObj = sdoaqObj;
            _sdoaqObj.CallBackMessageProcessed += SdoaqObj_MessageProcessed;
		}

        

        public Rectangle GetImageViewrDragArea()
        {
            SDOWSIO.WSIO.UTIL.WSUT_IV_GetFunctionRectData((IntPtr)pb_ImageViewer.Tag,
                WSIO.UTIL.WSUTIVRESOURCE.WSUTIVRESOURCE_MRBRIGHTNESS,
                out int left, out int top, out int right, out int bottom);

            return new Rectangle(left, top, right - left, bottom - top);
        }

        public void SetImageViewrDragArea(Rectangle rect)
        {
            SDOWSIO.WSIO.UTIL.WSUT_IV_SetFunctionRectData((IntPtr)pb_ImageViewer.Tag,
                WSIO.UTIL.WSUTIVRESOURCE.WSUTIVRESOURCE_MRBRIGHTNESS,
                rect.Left, rect.Top, rect.Right, rect.Bottom);
        }

        private void SdoaqObj_MessageProcessed(object sender, SdoaqController.CallBackMessageEventArgs e)
        {
            switch (e.Message)
			{
				case SdoaqController.emCallBackMessage.FocusStack:
					{
						_stackImgCount++;
						_afImgCount = 0;
						_edofImgCount = 0;

						this.Invoke(() => UpdatgeImageList(e.ImgInfoList, $"Focus Stack {_stackImgCount}"));
					}
					break;
				case SdoaqController.emCallBackMessage.Af:
					{
						_stackImgCount = 0;
						_afImgCount++;
						_edofImgCount = 0;

						this.Invoke(() => UpdatgeImageList(e.ImgInfoList, $"AF {_afImgCount}"));
					}
					break;
				case SdoaqController.emCallBackMessage.Mf:
					{
                        _stackImgCount = 0;
                        _afImgCount++;
                        _edofImgCount = 0;

                        this.Invoke(() => UpdatgeImageList(e.ImgInfoList, $"MF {_afImgCount}"));
                    }
					break;
				case SdoaqController.emCallBackMessage.Edof:
					{
						_stackImgCount = 0;
						_afImgCount = 0;
						_edofImgCount++;

						this.Invoke(() =>
						{
							UpdatgeImageList(e.ImgInfoList, $"Edof {_edofImgCount}");
							if (_visiblePointCloud)
							{
								UpdatePointCloud(e.PointCloudInfo);
							}
						});
					}
					break;
			}
		}

		private bool CompareImageList(List<SdoaqImageInfo> imageList, ListBox listBox)
		{
			if (imageList.Count != listBox.Items.Count)
			{
				return false;
			}

			for (int i = 0; i < imageList.Count; i++)
			{
				if (imageList[i].Name != listBox.Items[i].ToString())
				{
					return false;
				}
			}
			return true;
		}
		private void UpdatgeImageList(List<SdoaqImageInfo> imgList, string labelText)
		{
			var listBox = listbox_ImageList;

            listBox.BeginUpdate();

			foreach (var img in _imageList)
			{
				img.Dispose();
			}

			_imageList.Clear();

			_imageList = imgList;

			if (CompareImageList(_imageList, listBox) == false)
			{
				listBox.Items.Clear();

				foreach (var imgInfo in _imageList)
				{
					listBox.Items.Add(imgInfo.Name);
				}

				if (listBox.Items.Count <= _lastSelectImageIndex)
				{
					if (listBox.Items.Count > 0)
					{
						_lastSelectImageIndex = listBox.Items.Count - 1;
					}
					else
					{
						_lastSelectImageIndex = -1;
					}
				}
			}


			if (_lastSelectImageIndex >= 0)
			{
				if (listBox.SelectedIndex == -1)
				{
					listBox.SelectedIndex = _lastSelectImageIndex;
				}
				else
				{
					UpdateImage(_lastSelectImageIndex);
				}
			}

			lbl_ImageViewer.Text = labelText;

            listBox.EndUpdate();
		}

		private void UpdatePointCloud(SdoaqPointCloudInfo poinCloudimg)
		{
			if (pb_PointCloudViewer.Tag == null)
			{
				return;
			}

			var hwnd3DViewer = (IntPtr)pb_PointCloudViewer.Tag;

			if (hwnd3DViewer == null)
			{
				return;
			}

			_pointCloudInfoInfo?.Dispose();
            _pointCloudInfoInfo = null;
            _pointCloudInfoInfo = poinCloudimg;

			if (_visiblePointCloud == false || _pointCloudInfoInfo == null)
			{
				WSIO.GL.WSGL_Display_BG(hwnd3DViewer);
				return;
			}

            var paraDisplay25D = new WSIO.GL.tPara_Display25D()
            {
                width = (uint)_pointCloudInfoInfo.Width,
                height = (uint)_pointCloudInfoInfo.Height,
                z_offset1 = 0,
                z_offset2 = 0,
                z_slices = (uint)_pointCloudInfoInfo.SliceCount,
                scx1 = 0,
                scx2 = 0,
                scy1 = 0,
                scy2 = 0,
                scz1 = 0,
                scz2 = 0,
            };


            int displyMode = (int)(WSIO.GL.EDisplayMode.EDM_BGR_BYTE
				| WSIO.GL.EDisplayMode.EDM_DIMENSION_CALXY_25D
				| WSIO.GL.EDisplayMode.EDM_NDC_XY_ONLY);

			uint imageSize = (uint)(_pointCloudInfoInfo.Width * _pointCloudInfoInfo.Height);

			WSIO.GL.WSGL_Display_25D(hwnd3DViewer, WSIO.GL.GL_MG_ONSTAGE, "main",
				_pointCloudInfoInfo.VertexDataBuffer, imageSize * 3,
				_pointCloudInfoInfo.ImgDataBuffer, imageSize,
				displyMode, 1.0f, 
                ref paraDisplay25D);
		}

		private void UpdateImage(int idx)
		{
			if (_imageList.Count > idx)
			{
				var img = _imageList[idx];

				WSIO.UTIL.WSUT_IV_AttachRawImgData((IntPtr)pb_ImageViewer.Tag, (uint)img.Width, (uint)img.Height,
						  (uint)(img.Width * img.ColorByte),
						  (uint)img.ColorByte, img.Data,
						  (uint)img.Data.Length);

			}
		}

		private void LayouyUpdate()
		{
			if (_visiblePointCloud)
			{
				var rectList = this.ClientRectangle.DivideRect_Col(2);

				pnl_ImageViewerGroup.SetBounds(rectList[0]);
				pnl_PointCloudViewerGroup.SetBounds(rectList[1]);

				pnl_ImageViewerGroup.Visible = true;
			}
			else
			{
				pnl_ImageViewerGroup.SetBounds(this.ClientRectangle);

				pnl_PointCloudViewerGroup.Visible = false;
			}
            
            Rectangle rectImgList;
            const int WIDTH_IMG_LIST = 100;
            if (VisiBleImageListBox)
            {
                rectImgList = new Rectangle(pnl_ImageViewerGroup.Width - WIDTH_IMG_LIST, 
                    lbl_ImageViewer.Bottom,
                    WIDTH_IMG_LIST, 
                    pnl_ImageViewerGroup.Height - lbl_ImageViewer.Height);
            }
            else
            {
                rectImgList = new Rectangle(0, 0, 0, 0);
            }

            listbox_ImageList.SetBounds(rectImgList);

            var rectImageViewer = new Rectangle(0,
				lbl_ImageViewer.Bottom,
				pnl_ImageViewerGroup.Width - listbox_ImageList.Width,
				pnl_ImageViewerGroup.Height - lbl_ImageViewer.Height);

			pb_ImageViewer.SetBounds(rectImageViewer);

			var rectPointCloudViewer = new Rectangle(0,
				lbl_PointCloudViewer.Bottom,
				pnl_PointCloudViewerGroup.Width,
				pnl_PointCloudViewerGroup.Height - lbl_PointCloudViewer.Height);

			pb_PointCloudViewer.SetBounds(rectPointCloudViewer);

			if (pb_ImageViewer.Tag != null)
			{
				var ctrl = pb_ImageViewer;
				var hwnd = (IntPtr)pb_ImageViewer.Tag;

				WSIO.UTIL.WSUT_IV_ShowWindow(hwnd, 1, ctrl.Left, ctrl.Top - 35, ctrl.Right, ctrl.Bottom - 35);
			}

			if (pb_PointCloudViewer.Tag != null)
			{
				var ctrl = pb_PointCloudViewer;
				var hwnd = (IntPtr)pb_PointCloudViewer.Tag;

				WSIO.GL.WSGL_ShowWindow(hwnd, _visiblePointCloud, ctrl.Left, ctrl.Top - 35, ctrl.Right, ctrl.Bottom - 35);
			}
		}

		private void UserControl_Load(object sender, EventArgs e)
		{
			if (DesignMode)
			{
				return;
			}

			var attributes_ImageViewer = WSIO.UTIL.WSUTIVOPMODE.WSUTIVOPMODE_VISION
									   | WSIO.UTIL.WSUTIVOPMODE.WSUTIVOPMODE_INFOOSD;

			var rvWsio = WSIO.UTIL.WSUT_IV_CreateImageViewer("Image Viewer"
					, pb_ImageViewer.Handle
					, out IntPtr hwndImageViewr
					, 0
					, attributes_ImageViewer);


            WSIO.UTIL.WSUT_IV_ActivateFunction(hwndImageViewr,
                WSIO.UTIL.WSUTIVRESOURCE.WSUTIVRESOURCE_MRBRIGHTNESS,
                (ushort)WSIO.UTIL.WSUTIVOBJFUNC.WSUTIVOBJFUNC_UPDATERECT,
                ECD_BRIGHTNESS,
                null,
                this.Handle,
                WM_IV_EVENT);

            SdoaqController.WriteLog(Logger.emLogLevel.API, $"WSUT_IV_CreateImageViewer(), rv = {rvWsio}");

			pb_ImageViewer.Tag = hwndImageViewr;


			if (_visiblePointCloud)
			{
				rvWsio = WSIO.GL.WSGL_Initialize(pb_PointCloudViewer.Handle, out IntPtr hwnd3DViewer);

                SdoaqController.WriteLog(Logger.emLogLevel.API, $"WSGL_Initialize(), rv = {rvWsio}");

				var attributes_3D = WSIO.GL.EDisplayAttributes.EDA_SHOW_GUIDER_OBJECTS
								  | WSIO.GL.EDisplayAttributes.EDA_SHOW_SCALE_OBJECTS
								  | WSIO.GL.EDisplayAttributes.EDA_SHOW_COLORMAPBAR_OBJECTS
								  | WSIO.GL.EDisplayAttributes.EDA_NOHIDE_PICKER;

				WSIO.GL.WSGL_SetDisplayAttributes(hwnd3DViewer, (int)attributes_3D);

				pb_PointCloudViewer.Tag = hwnd3DViewer;

				WSIO.GL.WSGL_ShowWindow(hwnd3DViewer, _visiblePointCloud, pb_PointCloudViewer.Left, pb_PointCloudViewer.Top - 35, pb_PointCloudViewer.Right, pb_PointCloudViewer.Bottom - 35);

				WSIO.GL.WSGL_Display_BG(hwnd3DViewer);
			}

			LayouyUpdate();
		}

		private void UserControl_Disposed(object sender, EventArgs e)
		{
			if (_sdoaqObj != null)
			{
				_sdoaqObj.CallBackMessageProcessed -= SdoaqObj_MessageProcessed;
				_sdoaqObj = null;
			}

			foreach (var view in _imageList)
			{
				view.NullifyArray();
			}

			_pointCloudInfoInfo?.NullifyArray();

			if (pb_ImageViewer.Tag != null)
			{
				var hwnd = (IntPtr)pb_ImageViewer.Tag;
				WSIO.UTIL.WSUT_IV_DestroyImageViewer(hwnd);

				pb_ImageViewer.Tag = null;
			}

			if (pb_PointCloudViewer.Tag != null)
			{
				var hwnd = (IntPtr)pb_PointCloudViewer.Tag;

				WSIO.GL.WSGL_Finalize(hwnd);

				pb_PointCloudViewer.Tag = null;
			}
		}

		private void UserControl_Resize(object sender, EventArgs e)
		{
			LayouyUpdate();
		}

		private void listbox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (listbox_ImageList.SelectedIndex < 0)
			{
				return;
			}

			int idx = listbox_ImageList.SelectedIndex;

			if (_imageList.Count > idx)
			{
				_lastSelectImageIndex = idx;
				UpdateImage(idx);
			}
		}
	}
}
