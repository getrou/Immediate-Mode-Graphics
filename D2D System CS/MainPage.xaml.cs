using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Windows.Sdk;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace D2D_System_CS
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private unsafe void ClickHandler(object sender, RoutedEventArgs e)
        {
            myButton.Content = "Clicked";
            D3D11_CREATE_DEVICE_FLAG creationFlags = D3D11_CREATE_DEVICE_FLAG.D3D11_CREATE_DEVICE_BGRA_SUPPORT;

            D3D_FEATURE_LEVEL[] featureLevelArray =
            {
                D3D_FEATURE_LEVEL.D3D_FEATURE_LEVEL_11_1,
                D3D_FEATURE_LEVEL.D3D_FEATURE_LEVEL_11_0,
                D3D_FEATURE_LEVEL.D3D_FEATURE_LEVEL_10_1,
                D3D_FEATURE_LEVEL.D3D_FEATURE_LEVEL_10_0,
                D3D_FEATURE_LEVEL.D3D_FEATURE_LEVEL_9_3,
                D3D_FEATURE_LEVEL.D3D_FEATURE_LEVEL_9_2,
                D3D_FEATURE_LEVEL.D3D_FEATURE_LEVEL_9_1
            };

            ReadOnlySpan<D3D_FEATURE_LEVEL> featureLevels = new ReadOnlySpan<D3D_FEATURE_LEVEL>(featureLevelArray);

            ID3D11Device d3dDevice;
            D3D_FEATURE_LEVEL* pFeatureLevel = null;
            ID3D11DeviceContext immediateContext;

            PInvoke.D3D11CreateDevice(
                null,
                D3D_DRIVER_TYPE.D3D_DRIVER_TYPE_HARDWARE,
                new IntPtr(0),
                creationFlags,
                featureLevels,
                7,
                out d3dDevice,
                pFeatureLevel,
                out immediateContext);

            IDXGIDevice dxgiDevice = d3dDevice as IDXGIDevice;

            ID2D1Device d2dDevice;

            D2D1_CREATION_PROPERTIES* creationProperties = null;
            PInvoke.D2D1CreateDevice(dxgiDevice, creationProperties, out d2dDevice);

            ID2D1DeviceContext d2dDeviceContext;
            d2dDevice.CreateDeviceContext(D2D1_DEVICE_CONTEXT_OPTIONS.D2D1_DEVICE_CONTEXT_OPTIONS_NONE, out d2dDeviceContext);

            IDXGIAdapter dxgiAdapter;
            dxgiDevice.GetAdapter(out dxgiAdapter);

            IDXGIFactory2 dxgiFactory;
            //System.GUID* riid;
            //dxgiAdapter.GetParent(typeof(IDXGIFactory2).GUID, out dxgiFactory);
            
        }
    }
}
