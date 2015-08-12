//----------------------------------------------------------------------------------------
// THIS CODE AND INFORMATION IS PROVIDED "AS-IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------------------
//----------------------------------------------------------------------------------------
//  Description: Definition of BasePixelFormatConverter
//----------------------------------------------------------------------------------------
#include "precomp.hpp"

//IUnkown Interface
STDMETHODIMP_(ULONG) BasePixelFormatConverter::AddRef()
{
    return unknownImpl.AddRef();
}

STDMETHODIMP_(ULONG) BasePixelFormatConverter::Release()
{
    ULONG result = unknownImpl.Release();
    if (0 == result)
    {
        delete this;
    }
    return result;
}

STDMETHODIMP BasePixelFormatConverter::QueryInterface(REFIID iid, void **ppvObject)
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
        else if (iid == IID_IWICFormatConverter)
        {
            *ppvObject = static_cast<IWICFormatConverter*>(this);
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


BasePixelFormatConverter::BasePixelFormatConverter()
: bitmapSource(NULL)
{
    memset(&destPixelFormat, 0, sizeof(GUID));
}

STDMETHODIMP BasePixelFormatConverter::Initialize( 
        /* [in] */ IWICBitmapSource *pISource,
        /* [in] */ REFWICPixelFormatGUID dstFormat,
        /* [in] */ WICBitmapDitherType dither,
        /* [in] */ IWICPalette *pIPalette,
        /* [in] */ double alphaThresholdPercent,
        /* [in] */ WICBitmapPaletteType paletteTranslate)
{
    if (NULL != bitmapSource)
    {
        bitmapSource->Release();
    }

    bitmapSource = pISource;
    destPixelFormat = dstFormat;

    if (NULL != bitmapSource)
    {
        bitmapSource->AddRef();
    }

    return S_OK;
}


STDMETHODIMP BasePixelFormatConverter::GetSize(
        /* [out] */ UINT *puiWidth,
        /* [out] */ UINT *puiHeight)
{
    HRESULT result = E_UNEXPECTED;

    if (NULL != bitmapSource)
    {
        result = bitmapSource->GetSize(puiWidth, puiHeight);
    }

    return result;
}

STDMETHODIMP BasePixelFormatConverter::GetPixelFormat( 
        /* [out] */ WICPixelFormatGUID *pPixelFormat)
{
    HRESULT result = E_UNEXPECTED;

    if (NULL != bitmapSource)
    {
        if (NULL != pPixelFormat)
        {
            result = S_OK;
            *pPixelFormat = destPixelFormat;
        }
        else
        {
            result = E_INVALIDARG;
        }
    }

    return result;
}

STDMETHODIMP BasePixelFormatConverter::GetResolution(
        /* [out] */ double *pDpiX,
        /* [out] */ double *pDpiY)
{
    HRESULT result = E_UNEXPECTED;

    if (NULL != bitmapSource)
    {
        result = bitmapSource->GetResolution(pDpiX, pDpiY);
    }

    return result;
}

STDMETHODIMP BasePixelFormatConverter::CopyPalette( 
        /* [in] */ IWICPalette *pIPalette)
{
    HRESULT result = E_UNEXPECTED;

    if (NULL != bitmapSource)
    {
        result = bitmapSource->CopyPalette(pIPalette);
    }

    return result;
}
