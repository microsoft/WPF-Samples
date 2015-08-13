//----------------------------------------------------------------------------------------
// THIS CODE AND INFORMATION IS PROVIDED "AS-IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------------------

//----------------------------------------------------------------------------------------
//  Description: Declaration of CmykPixelFormatConverter
//----------------------------------------------------------------------------------------
#pragma once

// {B312A459-2A44-4833-AA45-0F50BC669A28}
extern const GUID CLSID_CmykPixelFormatConverter;

class CmykPixelFormatConverter : public BasePixelFormatConverter
{
public:
    CmykPixelFormatConverter();

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
    HRESULT ConvertCmykToRgb( 
        /* [in] */ const WICRect *prc,
        /* [in] */ UINT cbStride,
        /* [in] */ UINT cbPixelsSize,
        /* [out] */ BYTE *pbPixels);
    HRESULT ConvertRgbToCmyk( 
        /* [in] */ const WICRect *prc,
        /* [in] */ UINT cbStride,
        /* [in] */ UINT cbPixelsSize,
        /* [out] */ BYTE *pbPixels);    
};