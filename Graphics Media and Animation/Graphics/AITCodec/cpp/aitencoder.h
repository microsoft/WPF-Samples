//----------------------------------------------------------------------------------------
// THIS CODE AND INFORMATION IS PROVIDED "AS-IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------------------
//----------------------------------------------------------------------------------------
//  Description: Definition of AitEncoder
//----------------------------------------------------------------------------------------
#pragma once

#include "UnknownImpl.h"
extern const GUID CLSID_AitEncoder;

class AitFrameEncode : public BaseFrameEncode
{
public:
    AitFrameEncode(IWICImagingFactory *factory, IStream *pIStream, UINT num);
    ~AitFrameEncode();

    STDMETHOD(Commit)();
    STDMETHOD(GetMetadataQueryWriter)( 
        /* [out] */ IWICMetadataQueryWriter **ppIMetadataQueryWriter);

private:
};


class AitEncoder : public BaseEncoder
{
public:
    AitEncoder();
    ~AitEncoder();

    // IWICBitmapEncoder Interface
    STDMETHOD(Initialize)( 
        /* [in] */ IStream *pIStream,
        /* [in] */ WICBitmapEncoderCacheOption cacheOption);

    STDMETHOD(Commit)();

    STDMETHOD(GetMetadataQueryWriter)( 
        /* [out] */ IWICMetadataQueryWriter **ppIMetadataQueryWriter);

    //Additional function
    virtual BaseFrameEncode* CreateNewEncoderFrame(
    /* [in] */  IWICImagingFactory *pIFactory,
    /* [in] */ IStream *pIStream, 
    /* [in] */ UINT num);

private:   
};
