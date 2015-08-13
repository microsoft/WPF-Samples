//----------------------------------------------------------------------------------------
// THIS CODE AND INFORMATION IS PROVIDED "AS-IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------------------
//----------------------------------------------------------------------------------------
//  Description: Declaration of BaseDecoder
//----------------------------------------------------------------------------------------
#pragma once

#include "UnknownImpl.h"


class BaseFrameDecode : public IWICBitmapFrameDecode
{
public:
    BaseFrameDecode(IWICImagingFactory *pIFactory, UINT num);
    ~BaseFrameDecode();


    // IUnknown Interface
    STDMETHOD(QueryInterface)(REFIID riid, void **ppv);
    STDMETHOD_(ULONG, AddRef)();
    STDMETHOD_(ULONG, Release)();


    // IWICBitmapFrameDecode Interface
    STDMETHOD(GetMetadataQueryReader)( 
        /* [out] */ IWICMetadataQueryReader **ppIMetadataQueryReader);

    STDMETHOD(GetColorContexts)(
        /* [in] */ UINT cCount,
        /* [in] [out] */ IWICColorContext **ppIColorContexts,
        /* [out] */ UINT *pcActualCount);

    STDMETHOD(GetThumbnail)( 
        /* [out] */ IWICBitmapSource **ppIThumbnail);

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
        /* [out] */ BYTE *pbPixels
        );
public:
    IWICImagingFactory             *factory;
    UINT                            frameNumber;
    IWICBitmapSource               *bitmapSource;
    IWICPalette                    *palette;
    std::vector<IWICColorContext*>  colorContexts;
    IWICBitmapSource               *thumbnail;
    IWICBitmapSource               *preview; 

private:
     void ReleaseMembers();
    UnknownImpl         unknownImpl;
};

class BaseDecoder : public IWICBitmapDecoder
{
public:
    BaseDecoder(GUID Me, GUID Container);
    ~BaseDecoder();


    // IUnknown Interface
    STDMETHOD(QueryInterface)(REFIID riid, void **ppv);
    STDMETHOD_(ULONG, AddRef)();
    STDMETHOD_(ULONG, Release)();


    // IWICBitmapDecoder Interface
    STDMETHOD(QueryCapability)( 
        /* [in] */ IStream *pIStream,
        /* [out] */ DWORD *pCapability)=0;

    STDMETHOD(Initialize)( 
        /* [in] */ IStream *pIStream,
        /* [in] */ WICDecodeOptions cacheOptions)=0;

    STDMETHOD(GetContainerFormat)( 
        /* [out] */ GUID *pguidContainerFormat);

    STDMETHOD(GetDecoderInfo)( 
        /* [out] */ IWICBitmapDecoderInfo **ppIDecoderInfo);

    STDMETHOD(CopyPalette)( 
        /* [in] */ IWICPalette *pIPalette);

    STDMETHOD(GetMetadataQueryReader)( 
        /* [out] */ IWICMetadataQueryReader **ppIMetadataQueryReader);

    STDMETHOD(GetPreview)( 
        /* [out] */ IWICBitmapSource **ppIPreview);

    STDMETHOD(GetColorContexts)(
        /* [in] */ UINT cCount,
        /* [in] [out] */ IWICColorContext **ppIColorContexts,
        /* [out] */ UINT *pcActualCount);

    STDMETHOD(GetThumbnail)( 
        /* [out] */ IWICBitmapSource **ppIThumbnail);

    STDMETHOD(GetFrameCount)( 
        /* [out] */ UINT *pCount);

    STDMETHOD(GetFrame)( 
        /* [in] */ UINT index,
        /* [out] */ IWICBitmapFrameDecode **ppIBitmapFrame);

protected :
    //Additional functions
    virtual BaseFrameDecode* CreateNewDecoderFrame(IWICImagingFactory* factory , UINT i)=0; 
    virtual void AddDecoderFrame(BaseFrameDecode* frame); 
    HRESULT VerifyFactory();
    void ReleaseMembers(bool releaseFactory);   

    IWICImagingFactory             *factory;
    std::vector<BaseFrameDecode*>   frames;
    IWICPalette                    *palette;
    std::vector<IWICColorContext*>  colorContexts;
    IWICBitmapSource               *thumbnail;
    IWICBitmapSource               *preview;

    GUID CLSID_Container;
    GUID CLSID_This;

private:
    UnknownImpl                     unknownImpl;    
};
