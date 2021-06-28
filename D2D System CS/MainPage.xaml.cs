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
            d2dDevice.CreateDeviceContext(D2D1_DEVICE_CONTEXT_OPTIONS.D2D1_DEVICE_CONTEXT_OPTIONS_NONE, d2dDeviceContext);

            IDXGIAdapter dxgiAdapter;
            dxgiDevice.GetAdapter(out dxgiAdapter);

            object dxgiFactoryObject;
            Guid guid = typeof(IDXGIFactory2).GUID;
            dxgiAdapter.GetParent(guid, out dxgiFactoryObject);
            IDXGIFactory2 dxgiFactory = (IDXGIFactory2)dxgiFactoryObject;

            DXGI_SWAP_CHAIN_DESC1 swapChainDesc = new DXGI_SWAP_CHAIN_DESC1();
            swapChainDesc.Width = 500;
            swapChainDesc.Height = 500;
            swapChainDesc.Format = DXGI_FORMAT.DXGI_FORMAT_B8G8R8A8_UNORM;
            swapChainDesc.Stereo = false;
            swapChainDesc.SampleDesc.Count = 1;
            //swapChainDesc.Quality = 0;
            swapChainDesc.BufferUsage = 0x00000020;
            swapChainDesc.BufferCount = 2;
            swapChainDesc.Scaling = DXGI_SCALING.DXGI_SCALING_STRETCH;
            swapChainDesc.SwapEffect = DXGI_SWAP_EFFECT.DXGI_SWAP_EFFECT_FLIP_SEQUENTIAL;
            swapChainDesc.Flags = 0;

            IDXGISwapChain1 swapChain;
            dxgiFactory.CreateSwapChainForComposition(d3dDevice, swapChainDesc, null, out swapChain);

            ISwapChainPanelNative panelNative = swapChainPanel as ISwapChainPanelNative;

            panelNative.SetSwapChain(swapChain);

            D2D1_PIXEL_FORMAT pixelFormat;
            pixelFormat.format = DXGI_FORMAT.DXGI_FORMAT_B8G8R8A8_UNORM;
            pixelFormat.alphaMode = D2D1_ALPHA_MODE.D2D1_ALPHA_MODE_PREMULTIPLIED;

            D2D1_BITMAP_PROPERTIES1 bitmapProperties = new D2D1_BITMAP_PROPERTIES1();
            bitmapProperties.bitmapOptions = D2D1_BITMAP_OPTIONS.D2D1_BITMAP_OPTIONS_TARGET | D2D1_BITMAP_OPTIONS.D2D1_BITMAP_OPTIONS_CANNOT_DRAW;
            bitmapProperties.pixelFormat = pixelFormat;
            bitmapProperties.dpiX = 96;
            bitmapProperties.dpiY = 96;


            object dxgiBackBufferObject;
            Guid dxgiSurfaceGuid = typeof(IDXGISurface).GUID;
            swapChain.GetBuffer(0, dxgiSurfaceGuid, out dxgiBackBufferObject);

            IDXGISurface dxgiBackBuffer = (IDXGISurface) dxgiBackBufferObject;

            ID2D1Bitmap1 targetBitmap;
            d2dDeviceContext.CreateBitmapFromDxgiSurface(
                dxgiBackBuffer,
                bitmapProperties,
                out targetBitmap);

            d2dDeviceContext.SetTarget(targetBitmap);

            d2dDeviceContext.BeginDraw();
            
        }
    }
}
