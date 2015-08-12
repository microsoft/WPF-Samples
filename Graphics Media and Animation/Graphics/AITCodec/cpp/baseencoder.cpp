//----------------------------------------------------------------------------------------
// THIS CODE AND INFORMATION IS PROVIDED "AS-IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------------------
//----------------------------------------------------------------------------------------
//  Description: Declaration of BaseEncoder
//----------------------------------------------------------------------------------------
#include "precomp.hpp"

//----------------------------------------------------------------------------------------
// FRAME ENCODE
//----------------------------------------------------------------------------------------

BaseFrameEncode::BaseFrameEncode(IWICImagingFactory *pIFactory, IStream *pIStream, UINT num)
    : factory(pIFactory)
    , stream(pIStream)
    , frameNumber(num)
    , destSource(NULL)
    , destWidth(0), destHeight(0)
    , destDpiX(0), destDpiY(0)
    , destPixelFormat(GUID_WICPixelFormatUndefined)    
    , destPalette(NULL)
    , destThumbnail(NULL)
    , destPreview(NULL)
{
    if (NULL != factory)
    {
        factory->AddRef();
    }
    if (NULL != stream)
    {
        stream->AddRef();
    }
}

BaseFrameEncode::~BaseFrameEncode()
{
    ReleaseMembers();
}


//IUnknown Interface
STDMETHODIMP_(ULONG) BaseFrameEncode::AddRef()
    {
        return unknownImpl.AddRef();
    }
STDMETHODIMP_(ULONG) BaseFrameEncode::Release()
    {
        ULONG result = unknownImpl.Release();
        if (0 == result)
        {
            delete this;
        }
        return result;
    }
STDMETHODIMP BaseFrameEncode::QueryInterface(REFIID iid, void **ppvObject)
{
    HRESULT result = E_INVALIDARG;

    if (ppvObject)
    {
        if (iid == IID_IUnknown)
        {
            *ppvObject = static_cast<IUnknown*>(this);
            AddRef();

            result = S_OK;
        }
        else if (iid == IID_IWICBitmapFrameEncode)
        {
            *ppvObject = static_cast<IWICBitmapFrameEncode*>(this);
            AddRef();

            result = S_OK;
        }
        else
        {
            result = E_NOINTERFACE;
        }
    }

    return result;
}


// IWICBitmapFrameEncode
void BaseFrameEncode::ReleaseMembers()
{
    if (NULL != factory)
    {
        factory->Release();
    }
    if (NULL != stream)
    {
        stream->Release();
    }
    if (NULL != destSource)
    {
        destSource->Release();
    }
    for (size_t i = 0; i < colorContexts.size(); i++)
    {
        if (NULL != colorContexts[i])
        {
            colorContexts[i]->Release();
        }
    }
    if (NULL != destPalette)
    {
        destPalette->Release();
    }
    if (NULL != destThumbnail)
    {
        destThumbnail->Release();
    }
    if (NULL != destPreview)
    {
        destPreview->Release();
    }
}
STDMETHODIMP BaseFrameEncode::Initialize(IPropertyBag2 *pIEncoderOptions)
{
    return S_OK;
}

STDMETHODIMP BaseFrameEncode::SetSize(UINT width, UINT height)
{
    HRESULT result = E_UNEXPECTED;

    if (stream)
    {
        result = S_OK;

        destWidth = width;
        destHeight = height;
    }

    return result;
}

STDMETHODIMP BaseFrameEncode::SetResolution(double dpiX, double dpiY)
{
    HRESULT result = E_UNEXPECTED;

    if (stream)
    {
        result = S_OK;

        destDpiX = dpiX;
        destDpiY = dpiY;
    }

    return result;
}

STDMETHODIMP BaseFrameEncode::SetPixelFormat(WICPixelFormatGUID *pPixelFormat)
{
    HRESULT result = E_UNEXPECTED;

    if (stream)
    {
        result = S_OK;

        if (NULL == pPixelFormat)
        {
            result = E_INVALIDARG;
        }

        if (SUCCEEDED(result))
        {
            destPixelFormat = *pPixelFormat;
        }
    }

    return result;
}

STDMETHODIMP BaseFrameEncode::SetColorContexts(UINT cCount, IWICColorContext **ppIColorContext)
{
    HRESULT result = S_OK;
    if (stream)
    {
        if (ppIColorContext != NULL)
        {
            int i; 
            for (i = 0; i < (UINT)colorContexts.size(); i++)
            {
                colorContexts[i]->Release();
            }  
            colorContexts.clear();
            for (i = 0; i < cCount; i++)
            {
                colorContexts.push_back(ppIColorContext[i]);
                ppIColorContext[i]->AddRef();
            }
        }
        else
        {
            result = E_INVALIDARG;
        }
    }
    else
    {
        result = E_INVALIDARG;
    }
    return result;
}

STDMETHODIMP BaseFrameEncode::SetPalette(IWICPalette *pIPalette)
{
    HRESULT result = E_UNEXPECTED;

    if (stream)
    {
        result = S_OK;

        if (NULL != destPalette)
        {
            destPalette->Release();
        }

        destPalette = pIPalette;

        if (NULL != pIPalette)
        {
            pIPalette->AddRef();
        }
    }

    return result;
}

STDMETHODIMP BaseFrameEncode::SetThumbnail(IWICBitmapSource *pIThumbnail)
{
    HRESULT result = E_UNEXPECTED;

    if (stream)
    {
        result = S_OK;

        if (NULL != destThumbnail)
        {
            destThumbnail->Release();
        }

        destThumbnail = pIThumbnail;

        if (NULL != pIThumbnail)
        {
            pIThumbnail->AddRef();
        }
    }

    return result;
}


STDMETHODIMP BaseFrameEncode::WritePixels(UINT lineCount, UINT cbStride, UINT cbBufferSize, BYTE *pbPixels)
{
    HRESULT result = E_UNEXPECTED;

    if (stream && (destWidth > 0) && (destHeight > 0) &&
        (destPixelFormat != GUID_WICPixelFormatUndefined))
    {
        result = S_OK;

        if (NULL != destSource)
        {
            destSource->Release();
            destSource = NULL;
        }

        IWICBitmap *bitmap = NULL;

        result = factory->CreateBitmapFromMemory(destWidth, destHeight,
            destPixelFormat, cbStride, cbBufferSize, pbPixels, &bitmap);

        if (SUCCEEDED(result))
        {
            destSource = bitmap;
        }
    }

    return result;
}

STDMETHODIMP BaseFrameEncode::WriteSource(IWICBitmapSource *pIWICBitmapSource, WICRect *prc)
{
    HRESULT result = E_UNEXPECTED;

    if (stream)
    {
        result = S_OK;

        if (NULL != destSource)
        {
            destSource->Release();
        }

        destSource = pIWICBitmapSource;

        if (NULL != prc)
        {
            IWICBitmapClipper *clipper = NULL;

            result = factory->CreateBitmapClipper(&clipper);

            if (SUCCEEDED(result))
            {
                result = clipper->Initialize(pIWICBitmapSource, prc);

                if (FAILED(result))
                {
                    clipper->Release();
                }
            }

            if (SUCCEEDED(result))
            {
                destSource = clipper;
            }
        }
        else
        {
            if (NULL != pIWICBitmapSource)
            {
                pIWICBitmapSource->AddRef();
            }
        }
    }

    return result;
}



STDMETHODIMP BaseFrameEncode::Commit()
{
    HRESULT result = S_OK;
    
    IWICBitmapScaler *scaler = NULL;
    IWICFormatConverter *formatConverter = NULL;

    result = S_OK;

    // Create a scaler to match the requested width and height
    if (SUCCEEDED(result))
        {
        result = factory->CreateBitmapScaler(&scaler);
    }

    if (SUCCEEDED(result))
    {
        result = scaler->Initialize(destSource, destWidth, destHeight, WICBitmapInterpolationModeFant);
    }

    // Create a format converter to output into the proper format
    if (SUCCEEDED(result))
    {
        result = factory->CreateFormatConverter(&formatConverter);
    }
    if (SUCCEEDED(result))
    {
        result = formatConverter->Initialize(scaler, destPixelFormat, WICBitmapDitherTypeErrorDiffusion,
            destPalette, 50.0, WICBitmapPaletteTypeCustom);
    }

    // Cleanup
    if (formatConverter)
    {
        destSource->Release();
        destSource = formatConverter;
    }    
    return result;
}

STDMETHODIMP BaseFrameEncode::GetMetadataQueryWriter(IWICMetadataQueryWriter **ppIMetadataQueryWriter)
{
    return E_NOTIMPL;
}

//----------------------------------------------------------------------------------------
// ENCODER
//----------------------------------------------------------------------------------------

BaseEncoder::BaseEncoder(GUID Me, GUID Container)
    : factory(NULL)
    , stream(NULL)
    , destPalette(NULL)    
    , destThumbnail(NULL)
    , destPreview(NULL)
{
    CLSID_This = Me;
    CLSID_Container = Container;
}

BaseEncoder::~BaseEncoder()
{
    ReleaseMembers();
}


//IUnknown Interface
STDMETHODIMP_(ULONG) BaseEncoder::AddRef()
    {
        return unknownImpl.AddRef();
    }

STDMETHODIMP_(ULONG) BaseEncoder::Release()
    {
        ULONG result = unknownImpl.Release();
        if (0 == result)
        {
            delete this;
        }
        return result;
    }
STDMETHODIMP BaseEncoder::QueryInterface(REFIID iid, void **ppvObject)
{
    HRESULT result = E_INVALIDARG;

    if (ppvObject)
    {
        if (iid == IID_IUnknown)
        {
            *ppvObject = static_cast<IUnknown*>(this);
            AddRef();

            result = S_OK;
        }
        else if (iid == IID_IWICBitmapEncoder)
        {
            *ppvObject = static_cast<IWICBitmapEncoder*>(this);
            AddRef();

            result = S_OK;
        }
        else
        {
            result = E_NOINTERFACE;
        }
    }

    return result;
}

// IWICBitmapEncoder Interface
HRESULT BaseEncoder::VerifyFactory()
{
    if (NULL == factory)
    {
        return CoCreateInstance(CLSID_WICImagingFactory, NULL, CLSCTX_INPROC_SERVER, IID_IWICImagingFactory, (LPVOID*) &factory);
    }
    else
    {
        return S_OK;
    }
}


void BaseEncoder::ReleaseMembers()
{
    if (factory)
    {
        factory->Release();
    }
    if (destPalette)
    {
        destPalette->Release();
    }
    for (size_t i = 0; i < colorContexts.size(); i++)
    {
        if (NULL != colorContexts[i])
        {
            colorContexts[i]->Release();
        }
    }
    if (destThumbnail)
    {
        destThumbnail->Release();
    }
    if (destPreview)
    {
        destPreview->Release();
    }
    for (size_t i = 0; i < frames.size(); ++i)
    {
        frames[i]->Release();
    }
}
STDMETHODIMP BaseEncoder::GetContainerFormat(GUID *pguidContainerFormat)
{
    HRESULT result = E_INVALIDARG;

    if (NULL != pguidContainerFormat)
    {
        result = S_OK;

        *pguidContainerFormat = CLSID_Container;
    }

    return result;
}

STDMETHODIMP BaseEncoder::GetEncoderInfo(IWICBitmapEncoderInfo **ppIEncoderInfo)
{
    HRESULT result = S_OK;

    IWICComponentInfo *compInfo = NULL;

    if (SUCCEEDED(result))
    {
        result = VerifyFactory();
    }

    if (SUCCEEDED(result))
    {
        result = factory->CreateComponentInfo(CLSID_This, &compInfo);
    }

    if (SUCCEEDED(result))
    {
        result = compInfo->QueryInterface(IID_IWICBitmapEncoderInfo, (void**)ppIEncoderInfo);
    }

    if (compInfo)
    {
        compInfo->Release();
    }

    return result;
}

STDMETHODIMP BaseEncoder::SetColorContexts(UINT cCount, IWICColorContext **ppIColorContext)
{
    HRESULT result = S_OK;
    if (stream)
    {
        if (ppIColorContext != NULL)
        {
            int i; 
            for (i = 0; i < (UINT)colorContexts.size(); i++)
            {
                colorContexts[i]->Release();
            }  
            colorContexts.clear();
            for (i = 0; i < cCount; i++)
            {
                colorContexts.push_back(ppIColorContext[i]);
                ppIColorContext[i]->AddRef();
            }
        }
        else
        {
            result = E_INVALIDARG;
        }
    }
    else
    {
        result = E_INVALIDARG;
    }
    return result;
}

STDMETHODIMP BaseEncoder::SetPalette(IWICPalette *pIPalette)
{
    HRESULT result = E_UNEXPECTED;

    if (stream)
    {
        result = S_OK;

        if (NULL != destPalette)
        {
            destPalette->Release();
        }

        destPalette = pIPalette;

        if (NULL != pIPalette)
        {
            pIPalette->AddRef();
        }
    }

    return result;
}

STDMETHODIMP BaseEncoder::SetThumbnail(IWICBitmapSource *pIThumbnail)
{
    HRESULT result = E_UNEXPECTED;

    if (stream)
    {
        result = S_OK;

        if (NULL != destThumbnail)
        {
            destThumbnail->Release();
        }

        destThumbnail = pIThumbnail;

        if (NULL != pIThumbnail)
        {
            pIThumbnail->AddRef();
        }
    }

    return result;
}

STDMETHODIMP BaseEncoder::SetPreview(IWICBitmapSource *pIPreview)
{
    HRESULT result = E_UNEXPECTED;

    if (stream)
    {
        result = S_OK;

        if (NULL != destPreview)
        {
            destPreview->Release();
        }

        destPreview = pIPreview;

        if (NULL != pIPreview)
        {
            pIPreview->AddRef();
        }
    }

    return result;
}


STDMETHODIMP BaseEncoder::CreateNewFrame(IWICBitmapFrameEncode **ppIFrameEncode, IPropertyBag2 **ppIEncoderOptions)
{
    HRESULT result = E_UNEXPECTED;

    if (stream)
    {
        BaseFrameEncode *frameEncode = NULL;

        result = S_OK;

        if (NULL == ppIFrameEncode)
        {
            result = E_INVALIDARG;
        }
        if (SUCCEEDED(result))
        {
            result = VerifyFactory();
        }
        if (SUCCEEDED(result))
        {
            frameEncode = CreateNewEncoderFrame(factory, stream, frames.size());

            if (NULL == frameEncode)
            {
                result = E_OUTOFMEMORY;
            }
        }
        if (SUCCEEDED(result))
        {
            AddEncoderFrame(frameEncode);
            result = frameEncode->QueryInterface(IID_IWICBitmapFrameEncode, (void**)ppIFrameEncode);
        }
        if (ppIEncoderOptions != NULL)
        {
            *ppIEncoderOptions = NULL;
        }        
    }
    return result;
}

void BaseEncoder::AddEncoderFrame(BaseFrameEncode* frame)
{
     frames.push_back(frame);
     frames.back()->AddRef();
}
