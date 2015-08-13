//----------------------------------------------------------------------------------------
// THIS CODE AND INFORMATION IS PROVIDED "AS-IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------------------
//----------------------------------------------------------------------------------------
//  Description: Definition of BaseDecoder
//----------------------------------------------------------------------------------------
#include "precomp.hpp"

//----------------------------------------------------------------------------------------
// FRAME DECODE
//----------------------------------------------------------------------------------------

 // IUnknown Interface   
STDMETHODIMP_(ULONG) BaseFrameDecode::AddRef()
{
    return unknownImpl.AddRef();
}

STDMETHODIMP_(ULONG) BaseFrameDecode::Release()
{
    ULONG result = unknownImpl.Release();
    if (0 == result)
    {
        delete this;
    }
    return result;
}

STDMETHODIMP BaseFrameDecode::QueryInterface(REFIID iid, void **ppvObject)
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
        else if (iid == IID_IWICBitmapFrameDecode)
        {
            *ppvObject = static_cast<IWICBitmapFrameDecode*>(this);
            AddRef();

            result = S_OK;
        }
        else if (iid == IID_IWICBitmapSource)
        {
            *ppvObject = bitmapSource;

            if (NULL != bitmapSource)
            {
                bitmapSource->AddRef();
            }

            result = S_OK;
        }
        else
        {
            result = E_NOINTERFACE;
        }
    }

    return result;
}


// IWICBitmapFrameDecode Interface
void BaseFrameDecode::ReleaseMembers()
{
    if (factory)
    {
        factory->Release();
        factory = NULL;
    }
    if (bitmapSource)
    {
        bitmapSource->Release();
        bitmapSource = NULL;
    }
    if (palette)
    {
        palette->Release();
        palette = NULL;
    }
    for (size_t i = 0; i < colorContexts.size(); i++)
    {
        if (NULL != colorContexts[i])
        {
            colorContexts[i]->Release();
        }
    }
    colorContexts.clear();
    if (thumbnail)
    {
        thumbnail->Release();
        thumbnail = NULL;
    }
    if (preview)
    {
        preview->Release();
        preview = NULL;
    }
}

BaseFrameDecode::BaseFrameDecode(IWICImagingFactory *pIFactory, UINT num)
    : factory(pIFactory)
    , frameNumber(num)
    , bitmapSource(NULL)
    , palette(NULL)
    , thumbnail(NULL)
    , preview(NULL)
{
    if (NULL != factory)
    {
        factory->AddRef();
    }
}

BaseFrameDecode::~BaseFrameDecode()
{
   ReleaseMembers();
}


STDMETHODIMP BaseFrameDecode::GetMetadataQueryReader(IWICMetadataQueryReader **ppIMetadataQueryReader)
{
    return WINCODEC_ERR_UNSUPPORTEDOPERATION;
}


STDMETHODIMP BaseFrameDecode::GetColorContexts(UINT cCount, IWICColorContext **ppIColorContexts, UINT *pcActualCount)
{
    HRESULT result = S_OK;
    if (ppIColorContexts == NULL)
    {
        //return the number of color contexts
        if (pcActualCount != NULL)
        {
            *pcActualCount = colorContexts.size();        
        }
        else
        {
            result = E_INVALIDARG;
        }
    }
    else
    {
        //return the actual color contexts
        for (int i = 0; i < (UINT)colorContexts.size(); i++)
        {
            ppIColorContexts[i] = colorContexts[i];
            colorContexts[i]->AddRef();            
        }        
    }
    return result;
} 

STDMETHODIMP BaseFrameDecode::GetThumbnail(IWICBitmapSource **ppIThumbnail)
{
    HRESULT result = S_OK;

    if (NULL == ppIThumbnail)
    {
        result = E_INVALIDARG;
    }

    if (SUCCEEDED(result))
    {
        *ppIThumbnail = thumbnail;

        if (NULL != thumbnail)
        {
            thumbnail->AddRef();
        }
        else
        {
            result = WINCODEC_ERR_CODECNOTHUMBNAIL;
        }
    }

    return result;
}

STDMETHODIMP BaseFrameDecode::GetSize(UINT *puiWidth, UINT *puiHeight)
{
    HRESULT result = E_UNEXPECTED;

    if (bitmapSource)
    {
        result = bitmapSource->GetSize(puiWidth, puiHeight);
    }

    return result;
}

STDMETHODIMP BaseFrameDecode::GetPixelFormat(WICPixelFormatGUID *pPixelFormat)
{
    HRESULT result = E_UNEXPECTED;

    if (bitmapSource)
    {
        result = bitmapSource->GetPixelFormat(pPixelFormat);
    }

    return result;
}

STDMETHODIMP BaseFrameDecode::GetResolution(double *pDpiX, double *pDpiY)
{
    HRESULT result = E_UNEXPECTED;

    if (bitmapSource)
    {
        result = bitmapSource->GetResolution(pDpiX, pDpiY);
    }

    return result;
}

STDMETHODIMP BaseFrameDecode::CopyPalette(IWICPalette *pIPalette)
{
    HRESULT result = S_OK;

    if (NULL == pIPalette)
    {
        result = E_INVALIDARG;
    }

    if (SUCCEEDED(result))
    {
        if (NULL != palette)
        {
            pIPalette->InitializeFromPalette(palette);
        }
        else
        {
            result = E_UNEXPECTED;
        }
    }

    return result;
}

STDMETHODIMP BaseFrameDecode::CopyPixels(const WICRect *prc, UINT cbStride, UINT cbPixelsSize, BYTE *pbPixels)
{
    HRESULT result = E_UNEXPECTED;

    if (bitmapSource)
    {
        result = bitmapSource->CopyPixels(prc, cbStride, cbPixelsSize, pbPixels);
    }

    return result;
}


//----------------------------------------------------------------------------------------
// DECODER
//----------------------------------------------------------------------------------------

BaseDecoder::BaseDecoder(GUID Me, GUID Container)
    : factory(NULL)
    , palette(NULL)
    , thumbnail(NULL)
    , preview(NULL)
{
    CLSID_This = Me;
    CLSID_Container = Container;
}

BaseDecoder::~BaseDecoder()
{
    ReleaseMembers(true);
}


// IUnknown Interface    

STDMETHODIMP_(ULONG) BaseDecoder::AddRef()
{
    return unknownImpl.AddRef();
}

STDMETHODIMP_(ULONG) BaseDecoder::Release()
{
    ULONG result = unknownImpl.Release();
    if (0 == result)
    {
        delete this;
    }
    return result;
}

STDMETHODIMP BaseDecoder::QueryInterface(REFIID iid, void **ppvObject)
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
        else if (iid == IID_IWICBitmapDecoder)
        {
            *ppvObject = static_cast<IWICBitmapDecoder*>(this);
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
   //IWICBitmapDecoder Interface
HRESULT BaseDecoder::VerifyFactory()
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

void BaseDecoder::ReleaseMembers(bool releaseFactory)
{
    if (releaseFactory && factory)
    {
        factory->Release();
        factory = NULL;
    }
    for (size_t i=0; i < frames.size(); ++i)
    {
        if (NULL != frames[i])
        {
            frames[i]->Release();
        }
    }
    frames.clear();
    if (palette)
    {
        palette->Release();
        palette = NULL;
    }
    for (size_t i = 0; i < colorContexts.size(); ++i)
    {
        if (NULL != colorContexts[i])
        {
            colorContexts[i]->Release();
        }
    }
    colorContexts.clear();
    if (thumbnail)
    {
        thumbnail->Release();
        thumbnail = NULL;
    }
    if (preview)
    {
        preview->Release();
        preview = NULL;
    }
}

STDMETHODIMP BaseDecoder::GetContainerFormat(GUID *pguidContainerFormat)
{
    HRESULT result = E_INVALIDARG;

    if (NULL != pguidContainerFormat)
    {
        result = S_OK;

        *pguidContainerFormat = CLSID_Container;
    }

    return result;
}

STDMETHODIMP BaseDecoder::GetDecoderInfo(IWICBitmapDecoderInfo **ppIDecoderInfo)
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
        result = compInfo->QueryInterface(IID_IWICBitmapDecoderInfo, (void**)ppIDecoderInfo);
    }

    if (compInfo)
    {
        compInfo->Release();
    }

    return result;
}

STDMETHODIMP BaseDecoder::CopyPalette(IWICPalette *pIPalette)
{
    HRESULT result = S_OK;

    if (NULL == pIPalette)
    {
        result = E_INVALIDARG;
    }

    if (SUCCEEDED(result))
    {
        if (NULL != palette)
        {
            pIPalette->InitializeFromPalette(palette);
        }
        else
        {
            result = E_UNEXPECTED;
        }
    }

    return result;
}

STDMETHODIMP BaseDecoder::GetPreview(IWICBitmapSource **ppIPreview)
{
    HRESULT result = S_OK;

    if (NULL == ppIPreview)
    {
        result = E_INVALIDARG;
    }

    if (SUCCEEDED(result))
    {
        if (NULL != preview)
        {
            result = preview->QueryInterface(IID_IWICBitmapSource, (void**)ppIPreview);
        }
        else
        {
            result = E_UNEXPECTED;
        }
    }

    return result;
}



STDMETHODIMP BaseDecoder::GetColorContexts(UINT cCount, IWICColorContext **ppIColorContexts, UINT *pcActualCount)
{
    HRESULT result = S_OK;
    if (ppIColorContexts == NULL)
    {
        //return the number of color contexts
        if (pcActualCount != NULL)
        {
            *pcActualCount = colorContexts.size();        
        }
        else
        {
            result = E_INVALIDARG;
        }
    }
    else
    {
        //return the actual color contexts
        for (int i = 0; i < (UINT)colorContexts.size(); i++)
        {
            ppIColorContexts[i] = colorContexts[i];
            colorContexts[i]->AddRef();            
        }        
    }
    return result;
}

STDMETHODIMP BaseDecoder::GetThumbnail(IWICBitmapSource **ppIThumbnail)
{
    HRESULT result = S_OK;

    if (NULL == ppIThumbnail)
    {
        result = E_INVALIDARG;
    }

    if (SUCCEEDED(result))
    {
        if (NULL != thumbnail)
        {
            result = thumbnail->QueryInterface(IID_IWICBitmapSource, (void**)ppIThumbnail);
        }
        else
        {
            result = WINCODEC_ERR_CODECNOTHUMBNAIL;
        }
    }

    return result;
}

STDMETHODIMP BaseDecoder::GetFrameCount(UINT *pCount)
{
    HRESULT result = S_OK;

    if (NULL == pCount)
    {
        result = E_INVALIDARG;
    }

    if (SUCCEEDED(result))
    {
        *pCount = static_cast<UINT>(frames.size());
    }

    return result;
}

STDMETHODIMP BaseDecoder::GetFrame(UINT index, IWICBitmapFrameDecode **ppIBitmapFrame)
{
    HRESULT result = S_OK;

    if ((index >= static_cast<UINT>(frames.size())) || (NULL == ppIBitmapFrame))
    {
        result = E_INVALIDARG;
    }

    if (SUCCEEDED(result))
    {
        result = frames[index]->QueryInterface(IID_IWICBitmapFrameDecode, (void**)ppIBitmapFrame);
    }

    return result;
}

STDMETHODIMP BaseDecoder::GetMetadataQueryReader(IWICMetadataQueryReader **ppIMetadataQueryReader)
{
    return E_NOTIMPL;
}
void BaseDecoder::AddDecoderFrame(BaseFrameDecode* frame)
{
     frames.push_back(frame);
     frames.back()->AddRef();
}
