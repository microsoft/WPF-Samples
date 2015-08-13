//----------------------------------------------------------------------------------------
// THIS CODE AND INFORMATION IS PROVIDED "AS-IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------------------
//----------------------------------------------------------------------------------------
//  Description: Definition of AitDecoder
//----------------------------------------------------------------------------------------
#include "precomp.hpp"

static const GUID CLSID_AitContainer           = { 0xCCC50239, 0x14BB, 0x4F70, { 0xAD, 0x2A, 0xB7, 0x12, 0x62, 0x19, 0xA6, 0xDC } };
static const GUID CLSID_AitDecoder             = { 0xDEC50239, 0x14BB, 0x4F70, { 0xAD, 0x2A, 0xB7, 0x12, 0x62, 0x19, 0xA6, 0xDC } };

struct AitBlockHeader
{
    char Name[4];
    UINT FrameNumber;
    UINT Size;
};
//----------------------------------------------------------------------------------------
// SPECIAL UTILITY FUNCTIONS
//----------------------------------------------------------------------------------------
static HRESULT InputBitmapSource(IStream *stream, IWICImagingFactory *factory, IWICBitmapSource **bitmapSource)
{
    HRESULT result = S_OK;

    IWICBitmap *bitmap = NULL;
    IWICBitmapLock *bitmapLock = NULL;

    UINT width = 0, height = 0;
    double dpiX = 0, dpiY = 0;
    WICPixelFormatGUID pixelFormat = { 0 };
    UINT srcStride = 0;
    UINT destStride = 0;
    UINT cbBufferSize = 0;
    BYTE *data = NULL;

    if ((NULL == stream) || (NULL == factory) || (NULL == bitmapSource))
    {
        result = E_INVALIDARG;
    }

    if (SUCCEEDED(result))
    {
        InputValue(stream, width);        
        InputValue(stream, height);
        InputValue(stream, dpiX);
        InputValue(stream, dpiY);
        InputValue(stream, srcStride);
        result = InputValue(stream, pixelFormat);
    }

    // Create the bitmap
    if (SUCCEEDED(result))
    {
        result = factory->CreateBitmap(width, height, pixelFormat, WICBitmapCacheOnLoad, &bitmap);
    }

    // Set the resolution
    if (SUCCEEDED(result))
    {
        result = bitmap->SetResolution(dpiX, dpiY);
    }

    // Lock it so that we can store the data
    if (SUCCEEDED(result))
    {
        WICRect rct;
        rct.X = 0;
        rct.Y = 0;
        rct.Width = width;
        rct.Height = height;

        result = bitmap->Lock(&rct, WICBitmapLockWrite, &bitmapLock);
    }

    if (SUCCEEDED(result))
    {
        result = bitmapLock->GetDataPointer(&cbBufferSize, &data);
    }

    if (SUCCEEDED(result))
    {
        result = bitmapLock->GetStride(&destStride);
    }

    // Read the data from the stream
    if (SUCCEEDED(result))
    {
        // We must read one scanline at a time because the input stride
        // may not equal the output stride
        for (UINT y = 0; y < height; y++)
        {
            InputValues(stream, data, srcStride);             
            // Prepare for the next scanline
            data += destStride;
        }
    }

    // Close the lock
    if (bitmapLock && bitmap)
    {
        if(bitmapLock)
        {
            bitmapLock->Release();
            bitmapLock = NULL;
        }
    }

    // Finish
    if (SUCCEEDED(result))
    {
        result = bitmap->QueryInterface(IID_IWICBitmapSource, (void**)bitmapSource);
        if (SUCCEEDED(result))
        {
            bitmap->Release();
        }
    }
    else
    {
        if (bitmap)
        {
            bitmap->Release();
        }
        *bitmapSource = NULL;
    }

    return result;
}

static HRESULT InputColorContext(IStream *stream, IWICImagingFactory *factory, IWICColorContext **colorContext)
{
    HRESULT result = S_OK;

    if ((NULL == stream) || (NULL == factory) || (NULL == colorContext))
    {
        result = E_INVALIDARG;
    }

    if (SUCCEEDED(result))
    {
        // Once the unmanaged API fully supports color contexts,
        // implement this function.
        *colorContext = NULL;
    }

    return result;
}

static HRESULT InputBitmapPalette(IStream *stream, IWICImagingFactory *factory, IWICPalette **palette)
{
    HRESULT result = S_OK;

    WICColor *colors = NULL;

    UINT numColors = 0;

    if ((NULL == stream) || (NULL == factory) || (NULL == palette))
    {
        result = E_INVALIDARG;
    }

    // Create the palette
    if (SUCCEEDED(result))
    {
        result = factory->CreatePalette(palette);
    }

    // Read the colors
    if (SUCCEEDED(result))
    {
        result = InputValue(stream, numColors);
    }

    if (SUCCEEDED(result))
    {
        colors = new WICColor[numColors];

        if (NULL == colors)
        {
            result = E_OUTOFMEMORY;
        }
    }

    if (SUCCEEDED(result))
    {
        result = InputValues(stream, colors, numColors);
    }

    if (SUCCEEDED(result))
    {
        result = (*palette)->InitializeCustom(colors, numColors);
    }

    return result;
}


//----------------------------------------------------------------------------------------
// FRAME DECODE
//----------------------------------------------------------------------------------------

AitFrameDecode::AitFrameDecode(IWICImagingFactory *pIFactory, UINT num)
    : BaseFrameDecode(pIFactory, num) 
{    
}

AitFrameDecode::~AitFrameDecode()
{
}

HRESULT AitFrameDecode::InputColBlock(IStream *stream, const AitBlockHeader &bh)
{
    HRESULT result = S_OK;

    if (NULL == stream)
    {
        return E_INVALIDARG;
    }

    if (SUCCEEDED(result))
    {
        colorContexts.push_back(NULL);
        result = InputColorContext(stream, factory, &colorContexts.back());
    }

    return result;
}

HRESULT AitFrameDecode::InputFraBlock(IStream *stream, const AitBlockHeader &bh)
{
    HRESULT result = S_OK;

    if (NULL == stream)
    {
        return E_INVALIDARG;
    }

    if (SUCCEEDED(result))
    {
        if (bitmapSource)
        {
            bitmapSource->Release();
        }

        result = InputBitmapSource(stream, factory, &bitmapSource);
    }

    return result;
}

HRESULT AitFrameDecode::InputPalBlock(IStream *stream, const AitBlockHeader &bh)
{
    HRESULT result = S_OK;

    if (NULL == stream)
    {
        return E_INVALIDARG;
    }

    if (SUCCEEDED(result))
    {
        if (palette)
        {
            palette->Release();
        }

        result = InputBitmapPalette(stream, factory, &palette);
    }

    return result;
}

HRESULT AitFrameDecode::InputPreBlock(IStream *stream, const AitBlockHeader &bh)
{
    HRESULT result = S_OK;

    if (NULL == stream)
    {
        return E_INVALIDARG;
    }

    if (SUCCEEDED(result))
    {
        if (preview)
        {
            preview->Release();
        }
        result = InputBitmapSource(stream, factory, &preview);
    }

    return result;
}

HRESULT AitFrameDecode::InputThuBlock(IStream *stream, const AitBlockHeader &bh)
{
    HRESULT result = S_OK;

    if (NULL == stream)
    {
        return E_INVALIDARG;
    }

    if (SUCCEEDED(result))
    {
        if (thumbnail)
        {
            thumbnail->Release();
        }

        result = InputBitmapSource(stream, factory, &thumbnail);
    }

    return result;
}




//----------------------------------------------------------------------------------------
// DECODER
//----------------------------------------------------------------------------------------

AitDecoder::AitDecoder()
    : BaseDecoder(CLSID_AitDecoder, CLSID_AitContainer)
{
}

AitDecoder::~AitDecoder()
{    
}

HRESULT AitDecoder::InputBlock(IStream *stream, AitBlockHeader &bh)
{
    HRESULT result = S_OK;

    if (NULL == stream)
    {
        result = E_INVALIDARG;
    }

    if (SUCCEEDED(result))
    {
        result = InputValue(stream, bh);
    }

    // Ensure that the name of this block is valid
    if (SUCCEEDED(result))
    {
        result = ('\0' == bh.Name[3]) ? S_OK : E_UNEXPECTED;
    }

    return result;
}

/// An AIT Block is the start of the image. It must come at the beginning of the file.
HRESULT AitDecoder::InputAitBlock(IStream *stream, const AitBlockHeader &bh)
{
    HRESULT result = S_OK;

    if (NULL == stream)
    {
        result = E_INVALIDARG;
    }

    if (SUCCEEDED(result))
    {
        result = VerifyFactory();
    }

    // Get the number of frames
    UINT numFrames;

    if (SUCCEEDED(result))
    {
        result = InputValue(stream, numFrames);
    }

    if (SUCCEEDED(result))
    {
        ReleaseMembers(false);

        for (UINT i=0; i < numFrames; ++i)
        {
            AddDecoderFrame(CreateNewDecoderFrame(factory, i));            
        }
    }
    return result;
}

BaseFrameDecode* AitDecoder::CreateNewDecoderFrame(IWICImagingFactory* factory , UINT i){
    return new AitFrameDecode(factory, i);
}

HRESULT AitDecoder::InputColBlock(IStream *stream, const AitBlockHeader &bh)
{
    HRESULT result = S_OK;

    if (NULL == stream)
    {
        return E_INVALIDARG;
    }

    if (SUCCEEDED(result))
    {
        result = VerifyFactory();
    }

    if (SUCCEEDED(result))
    {
        if (0xFFFFFFFF == bh.FrameNumber)
        {
            colorContexts.push_back(NULL);
            result = InputColorContext(stream, factory, &colorContexts.back());
        }
        else
        {
            if (bh.FrameNumber < static_cast<UINT>(frames.size()))
            {
                result = ((AitFrameDecode*)frames[bh.FrameNumber])->InputColBlock(stream, bh);
            }
            else
            {
                result = E_UNEXPECTED;
            }
        }
    }

    return result;
}

HRESULT AitDecoder::InputFraBlock(IStream *stream, const AitBlockHeader &bh)
{
    HRESULT result = S_OK;

    if (NULL == stream)
    {
        return E_INVALIDARG;
    }

    if (SUCCEEDED(result))
    {
        if (0xFFFFFFFF == bh.FrameNumber)
        {
            result = E_UNEXPECTED;
        }
        else
        {
            if (bh.FrameNumber < static_cast<UINT>(frames.size()))
            {
                result = ((AitFrameDecode*)frames[bh.FrameNumber])->InputFraBlock(stream, bh);
            }
            else
            {
                result = E_UNEXPECTED;
            }
        }
    }

    return result;
}

HRESULT AitDecoder::InputPalBlock(IStream *stream, const AitBlockHeader &bh)
{
    HRESULT result = S_OK;

    if (NULL == stream)
    {
        return E_INVALIDARG;
    }

    if (SUCCEEDED(result))
    {
        result = VerifyFactory();
    }

    if (SUCCEEDED(result))
    {
        if (0xFFFFFFFF == bh.FrameNumber)
        {
            if (palette)
            {
                palette->Release();
            }

            result = InputBitmapPalette(stream, factory, &palette);
        }
        else
        {
            if (bh.FrameNumber < static_cast<UINT>(frames.size()))
            {
                result = ((AitFrameDecode*)frames[bh.FrameNumber])->InputPalBlock(stream, bh);
            }
            else
            {
                result = E_UNEXPECTED;
            }
        }
    }

    return result;
}

HRESULT AitDecoder::InputPreBlock(IStream *stream, const AitBlockHeader &bh)
{
    HRESULT result = S_OK;

    if (NULL == stream)
    {
        return E_INVALIDARG;
    }

    if (SUCCEEDED(result))
    {
        result = VerifyFactory();
    }

    if (SUCCEEDED(result))
    {
        if (0xFFFFFFFF == bh.FrameNumber)
        {
            if (preview)
            {
                preview->Release();
            }

            result = InputBitmapSource(stream, factory, &preview);
        }
        else
        {
            if (bh.FrameNumber < static_cast<UINT>(frames.size()))
            {
                result = ((AitFrameDecode*)frames[bh.FrameNumber])->InputPreBlock(stream, bh);
            }
            else
            {
                result = E_UNEXPECTED;
            }
        }
    }

    return result;
}

HRESULT AitDecoder::InputThuBlock(IStream *stream, const AitBlockHeader &bh)
{
    HRESULT result = S_OK;

    if (NULL == stream)
    {
        return E_INVALIDARG;
    }

    if (SUCCEEDED(result))
    {
        result = VerifyFactory();
    }

    if (SUCCEEDED(result))
    {
        if (0xFFFFFFFF == bh.FrameNumber)
        {
            if (thumbnail)
            {
                thumbnail->Release();
            }

            result = InputBitmapSource(stream, factory, &thumbnail);
        }
        else
        {
            if (bh.FrameNumber < static_cast<UINT>(frames.size()))
            {
                result = ((AitFrameDecode*)frames[bh.FrameNumber])->InputThuBlock(stream, bh);
            }
            else
            {
                result = E_UNEXPECTED;
            }
        }
    }

    return result;
}

STDMETHODIMP AitDecoder::QueryCapability(IStream *pIStream, DWORD *pCapability)
{
    HRESULT result = S_OK;

    ULARGE_INTEGER curPos = { 0 };    
    AitBlockHeader bh = { 0 };

    if ((NULL == pIStream) || (NULL == pCapability))
    {
        result = E_INVALIDARG;
    }

    // Remember the position of the stream
    if (SUCCEEDED(result))
    {        
        LARGE_INTEGER zero = { 0 };

        result = pIStream->Seek(zero, STREAM_SEEK_CUR, &curPos);
    }

    // Assume that we can't do anything
    if (SUCCEEDED(result))
    {
        *pCapability = 0;
    }

    // Prove ourselves wrong
    if (SUCCEEDED(result))
    {
        result = InputBlock(pIStream, bh);

        // Always seek back
        {
            LARGE_INTEGER cp;
            cp.QuadPart = static_cast<LONGLONG>(curPos.QuadPart);

            pIStream->Seek(cp, STREAM_SEEK_SET, NULL);
        }
    }

    if (SUCCEEDED(result))
    {
        // If this is our format, we can do everything
        if (strcmp(bh.Name, "AIT") == 0)
        {
            *pCapability = WICBitmapDecoderCapabilityCanDecodeAllImages ||
                           WICBitmapDecoderCapabilityCanDecodeThumbnail ||
                           WICBitmapDecoderCapabilityCanEnumerateMetadata ||
                           WICBitmapDecoderCapabilitySameEncoder;
        }
    }

    return result;
}

STDMETHODIMP AitDecoder::Initialize(IStream *pIStream, WICDecodeOptions cacheOptions)
{
    HRESULT result = E_INVALIDARG;

    ReleaseMembers(true);

    if (pIStream)
    {
        HRESULT blockRead = S_OK;
        AitBlockHeader bh = { 0 };

        result = S_OK;

        // Read blocks until we have reached the end or there was an error
        for (blockRead = InputBlock(pIStream, bh); SUCCEEDED(result) && SUCCEEDED(blockRead); blockRead = InputBlock(pIStream, bh))
        {
            ULARGE_INTEGER endPos = { 0 };

            // Store the end position
            if (SUCCEEDED(result))
            {
                LARGE_INTEGER zero = { 0 };

                result = pIStream->Seek(zero, STREAM_SEEK_CUR, &endPos);

                endPos.QuadPart += bh.Size;
            }

            // Dispatch on the block name
            if (strcmp(bh.Name, "AIT") == 0)
            {
                result = InputAitBlock(pIStream, bh);
            }
            else if (strcmp(bh.Name, "COL") == 0)
            {
                result = InputColBlock(pIStream, bh);
            }
            else if (strcmp(bh.Name, "FRA") == 0)
            {
                result = InputFraBlock(pIStream, bh);
            }
            else if (strcmp(bh.Name, "PAL") == 0)
            {
                result = InputPalBlock(pIStream, bh);
            }
            else if (strcmp(bh.Name, "PRE") == 0)
            {
                result = InputPreBlock(pIStream, bh);
            }
            else if (strcmp(bh.Name, "THU") == 0)
            {
                result = InputThuBlock(pIStream, bh);
            }

            // Ensure that we are at the end of the block no matter what happened
            if (0 != endPos.QuadPart)
            {
                LARGE_INTEGER ep;
                ep.QuadPart = static_cast<LONGLONG>(endPos.QuadPart);

                pIStream->Seek(ep, STREAM_SEEK_SET, NULL);
            }
        }
    }
    return result;
}

