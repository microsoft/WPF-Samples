//----------------------------------------------------------------------------------------
// THIS CODE AND INFORMATION IS PROVIDED "AS-IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------------------
//----------------------------------------------------------------------------------------
//  Description: Declaration of YCbCrPixelFormatConverter
//----------------------------------------------------------------------------------------
#pragma once

extern const GUID CLSID_YCbCrPixelFormatConverter;

class YCbCrPixelFormatConverter : public BasePixelFormatConverter
{
public:
    YCbCrPixelFormatConverter();

    STDMETHOD(CanConvert)( 
        /* [in] */ REFWICPixelFormatGUID srcPixelFormat,
        /* [in] */ REFWICPixelFormatGUID dstPixelFormat,
        /* [out] */ BOOL *pfCanConvert);   

    STDMETHOD(CopyPixels)( 
        /* [in] */ const WICRect *prc,
        /* [in] */ UINT cbStride,
        /* [in] */ UINT cbPixelsSize,
        /* [out] */ BYTE *pbPixels);

    //Additional Functions
private :
    HRESULT ConvertYCbCrToRgb( 
        /* [in] */ const WICRect *prc,
        /* [in] */ UINT cbStride,
        /* [in] */ UINT cbPixelsSize,
        /* [out] */ BYTE *pbPixels);
    HRESULT ConvertRgbToYCbCr( 
        /* [in] */ const WICRect *prc,
        /* [in] */ UINT cbStride,
        /* [in] */ UINT cbPixelsSize,
        /* [out] */ BYTE *pbPixels);
};