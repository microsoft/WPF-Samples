//----------------------------------------------------------------------------------------
// THIS CODE AND INFORMATION IS PROVIDED "AS-IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------------------
//----------------------------------------------------------------------------------------
//  Description: Definition of BaseEncoder
//----------------------------------------------------------------------------------------
#pragma once

#include "UnknownImpl.h"

class BaseFrameEncode : public IWICBitmapFrameEncode
{
public:
    BaseFrameEncode(IWICImagingFactory *factory, IStream *pIStream, UINT num);
    ~BaseFrameEncode();


    // IUnknown Interface
    STDMETHOD(QueryInterface)(REFIID riid, void **ppv);
    STDMETHOD_(ULONG, AddRef)();
    STDMETHOD_(ULONG, Release)();


    // IWICBitmapFrameEncode
    STDMETHOD(Initialize)( 
        /* [in] [optional] */ IPropertyBag2 *pIEncoderOptions);

    STDMETHOD(SetSize)(
        /* [in] */ UINT width,
        /* [in] */ UINT height);

    STDMETHOD(SetResolution)( 
        /* [in] */ double dpiX,
        /* [in] */ double dpiY);

    STDMETHOD(SetPixelFormat)( 
        /* [out][in] */ WICPixelFormatGUID *pPixelFormat);

    STDMETHOD(SetColorContexts)(
        /* [in] */ UINT cCount,
        /* [in] */ IWICColorContext **ppIColorContext);

    STDMETHOD(SetPalette)(
        /* [in] */ IWICPalette *pIPalette);

    STDMETHOD(SetThumbnail)( 
        /* [in] */ IWICBitmapSource *pIThumbnail);

    STDMETHOD(WritePixels)( 
        /* [in] */ UINT lineCount,
        /* [in] */ UINT cbStride,
        /* [in] */ UINT cbBufferSize,
        /* [in] */ BYTE *pbPixels);

    STDMETHOD(WriteSource)( 
        /* [in] */ IWICBitmapSource *pIWICBitmapSource,
        /* [in] */ WICRect *prc);

    STDMETHOD(Commit)();

    STDMETHOD(GetMetadataQueryWriter)( 
        /* [out] */ IWICMetadataQueryWriter **ppIMetadataQueryWriter);

public :
    IWICImagingFactory *factory;
    IStream            *stream;
    UINT                frameNumber;    
    IWICBitmapSource   *destSource;
    UINT                destWidth, destHeight;
    double              destDpiX, destDpiY;
    WICPixelFormatGUID  destPixelFormat;
    std::vector<IWICColorContext*>  colorContexts;
    IWICPalette        *destPalette;
    IWICBitmapSource   *destThumbnail;
    IWICBitmapSource   *destPreview;

private:
    void ReleaseMembers();
    UnknownImpl         unknownImpl;
};


class BaseEncoder : public IWICBitmapEncoder
{
public:
    BaseEncoder(GUID Me, GUID Container);
    ~BaseEncoder();


    // IUnknown Interface
    STDMETHOD(QueryInterface)(REFIID riid, void **ppv);
    STDMETHOD_(ULONG, AddRef)();
    STDMETHOD_(ULONG, Release)();

    // IWICBitmapEncoder Interface
    STDMETHOD(Initialize)( 
        /* [in] */ IStream *pIStream,
        /* [in] */ WICBitmapEncoderCacheOption cacheOption)=0;

    STDMETHOD(GetContainerFormat)( 
        /* [out] */ GUID *pguidContainerFormat);

    STDMETHOD(GetEncoderInfo)( 
        /* [out] */ IWICBitmapEncoderInfo **ppIEncoderInfo);

    STDMETHOD(SetColorContexts)(
        /* [in] */ UINT cCount,
        /* [in] */ IWICColorContext **ppIColorContext);

    STDMETHOD(SetPalette)( 
        /* [in] */ IWICPalette *pIPalette);

    STDMETHOD(SetThumbnail)( 
        /* [in] */ IWICBitmapSource *pIThumbnail);

    STDMETHOD(SetPreview)( 
        /* [in] */ IWICBitmapSource *pIPreview);

    STDMETHOD(CreateNewFrame)( 
        /* [out] */ IWICBitmapFrameEncode **ppIFrameEncode,
        /* [out] [optional] */ IPropertyBag2 **ppIEncoderOptions);

    STDMETHOD(Commit)()=0;

    STDMETHOD(GetMetadataQueryWriter)( 
        /* [out] */ IWICMetadataQueryWriter **ppIMetadataQueryWriter)=0;

protected :
    //Additional functions
    virtual BaseFrameEncode* CreateNewEncoderFrame(
        /* [in] */  IWICImagingFactory *pIFactory,
        /* [in] */ IStream *pIStream, 
        /* [in] */ UINT num) = 0;
    virtual void AddEncoderFrame(BaseFrameEncode* frame); 

    HRESULT VerifyFactory();
     
    IWICImagingFactory           *factory;
    IStream                      *stream;
    std::vector<BaseFrameEncode*>  frames;
    IWICPalette                  *destPalette;
    std::vector<IWICColorContext*>  colorContexts;
    IWICBitmapSource             *destThumbnail;
    IWICBitmapSource             *destPreview;

    ULARGE_INTEGER                headerPos;

    GUID CLSID_Container;
    GUID CLSID_This;
private:
    void ReleaseMembers();
    UnknownImpl                   unknownImpl;
};
