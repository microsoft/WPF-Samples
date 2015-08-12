//----------------------------------------------------------------------------------------
// THIS CODE AND INFORMATION IS PROVIDED "AS-IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------------------
//----------------------------------------------------------------------------------------
//  Description: Declaration of BasePixelFormatConverter
//----------------------------------------------------------------------------------------
#pragma once

#include "UnknownImpl.h"

class BasePixelFormatConverter : public IWICFormatConverter
{
public:
    BasePixelFormatConverter();


    // IUnknown Interface
    STDMETHOD(QueryInterface)(REFIID riid, void **ppv);
    STDMETHOD_(ULONG, AddRef)();
    STDMETHOD_(ULONG, Release)();


    // IWICFormatConverter Interface
    STDMETHOD(Initialize)( 
        /* [in] */ IWICBitmapSource *pISource,
        /* [in] */ REFWICPixelFormatGUID dstFormat,
        /* [in] */ WICBitmapDitherType dither,
        /* [in] */ IWICPalette *pIPalette,
        /* [in] */ double alphaThresholdPercent,
        /* [in] */ WICBitmapPaletteType paletteTranslate);

    STDMETHOD(CanConvert)( 
        /* [in] */ REFWICPixelFormatGUID srcPixelFormat,
        /* [in] */ REFWICPixelFormatGUID dstPixelFormat,
        /* [out] */ BOOL *pfCanConvert)=0;

    // IWICBitmapSource Interface
    STDMETHOD(GetSize)( 
        /* [out] */ UINT *puiWidth,
        /* [out] */ UINT *puiHeight);

    STDMETHOD(GetPixelFormat)( 
        /* [out] */ WICPixelFormatGUID *pPixelFormat);

    STDMETHOD(GetResolution)( 
        /* [out] */ double *pDpiX,
        /* [out] */ double *pDpiY);

    STDMETHOD(CopyPalette)( 
        /* [in] */ IWICPalette *pIPalette);

    STDMETHOD(CopyPixels)( 
        /* [in] */ const WICRect *prc,
        /* [in] */ UINT cbStride,
        /* [in] */ UINT cbPixelsSize,
        /* [out] */ BYTE *pbPixels)=0;

protected:
    IWICBitmapSource   *bitmapSource;
    WICPixelFormatGUID  destPixelFormat;
    GUID CLSID_This;

private:
    UnknownImpl         unknownImpl;
};

