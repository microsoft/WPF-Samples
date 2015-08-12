//----------------------------------------------------------------------------------------
// THIS CODE AND INFORMATION IS PROVIDED "AS-IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------------------
//----------------------------------------------------------------------------------------
//  Description: Declaration of AitDecoder
//----------------------------------------------------------------------------------------
#pragma once

#include "UnknownImpl.h"

extern const GUID CLSID_AitContainer;
extern const GUID CLSID_AitDecoder;

struct AitBlockHeader;

class AitFrameDecode : public BaseFrameDecode
{
public:
    AitFrameDecode(IWICImagingFactory *pIFactory, UINT num);
    ~AitFrameDecode();

    // IWICBitmapFrameDecode Interface

    HRESULT InputColBlock(IStream *stream, const AitBlockHeader &bh);
    HRESULT InputFraBlock(IStream *stream, const AitBlockHeader &bh);
    HRESULT InputPalBlock(IStream *stream, const AitBlockHeader &bh);
    HRESULT InputPreBlock(IStream *stream, const AitBlockHeader &bh);
    HRESULT InputThuBlock(IStream *stream, const AitBlockHeader &bh);

private:
};


class AitDecoder : public BaseDecoder
{
public:
    AitDecoder();
    ~AitDecoder();


    // IWICBitmapDecoder Interface
    STDMETHOD(QueryCapability)( 
        /* [in] */ IStream *pIStream,
        /* [out] */ DWORD *pCapability);

    STDMETHOD(Initialize)( 
        /* [in] */ IStream *pIStream,
        /* [in] */ WICDecodeOptions cacheOptions);
 
   //Additional Functions
   virtual BaseFrameDecode* CreateNewDecoderFrame(IWICImagingFactory* factory , UINT i); 

private:
    HRESULT InputBlock(IStream *stream, AitBlockHeader &bh);
    HRESULT InputAitBlock(IStream *stream, const AitBlockHeader &bh);
    HRESULT InputColBlock(IStream *stream, const AitBlockHeader &bh);
    HRESULT InputFraBlock(IStream *stream, const AitBlockHeader &bh);
    HRESULT InputPalBlock(IStream *stream, const AitBlockHeader &bh);
    HRESULT InputPreBlock(IStream *stream, const AitBlockHeader &bh);
    HRESULT InputThuBlock(IStream *stream, const AitBlockHeader &bh);
};
