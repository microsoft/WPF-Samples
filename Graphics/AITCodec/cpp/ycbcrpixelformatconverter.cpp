//----------------------------------------------------------------------------------------
// THIS CODE AND INFORMATION IS PROVIDED "AS-IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------------------
//----------------------------------------------------------------------------------------
//  Description: Definition of YCbCrPixelFormatConverter
//----------------------------------------------------------------------------------------
#include "precomp.hpp"

// The YCbCr format GUID {9D14B98B-4427-41f0-A7AC-92CC96AE0BEB}
static const GUID GUID_TestPixelFormat24bppYCbCr = 
{ 0x9d14b98b, 0x4427, 0x41f0, { 0xa7, 0xac, 0x92, 0xcc, 0x96, 0xae, 0xb, 0xeb } };

static const GUID CLSID_YCbCrPixelFormatConverter = 
{ 0x49CC393E, 0xCE46, 0x11D9, { 0x8B, 0xDE, 0xF6, 0x6B, 0xAD, 0x1E, 0x3F, 0x3A}};



YCbCrPixelFormatConverter::YCbCrPixelFormatConverter()
: BasePixelFormatConverter()
{}

STDMETHODIMP YCbCrPixelFormatConverter::CanConvert( 
        /* [in] */ REFWICPixelFormatGUID srcPixelFormat,
        /* [in] */ REFWICPixelFormatGUID dstPixelFormat,
        /* [out] */ BOOL *pfCanConvert)
{
    HRESULT result = E_INVALIDARG;

    if (NULL != pfCanConvert)
    {
        result = S_OK;

        //In thuis case the canonical format would be GUID_WICPixelFormat32bppBGRA

        // I can convert from YCbCr to RGB and back only
        if ((GUID_WICPixelFormat32bppBGRA   == srcPixelFormat) &&
            (GUID_TestPixelFormat24bppYCbCr == dstPixelFormat))
        {
            *pfCanConvert = TRUE;
        }
        else if ((GUID_TestPixelFormat24bppYCbCr   == srcPixelFormat) &&
            (GUID_WICPixelFormat32bppBGRA == dstPixelFormat))
        {
            *pfCanConvert = TRUE;
        }
        else
        {
            *pfCanConvert = FALSE;
        }
    }
    return result;
}

STDMETHODIMP YCbCrPixelFormatConverter::CopyPixels( 
        /* [in] */ const WICRect *prc,
        /* [in] */ UINT cbStride,
        /* [in] */ UINT cbPixelsSize,
        /* [out] */ BYTE *pbPixels)
{
    HRESULT result = E_UNEXPECTED;

    if (NULL != bitmapSource)
    {
        //Do the actual conversion
        if (destPixelFormat == GUID_WICPixelFormat32bppBGRA)
        {
            //Convert back to canonical 32ARGB            
            result = ConvertYCbCrToRgb(prc, cbStride, cbPixelsSize, pbPixels);
        }
        else if (destPixelFormat == GUID_TestPixelFormat24bppYCbCr)
        {
            //Convert back to newly supported YCbCr
            result = ConvertRgbToYCbCr(prc, cbStride, cbPixelsSize, pbPixels);
        }
    }
    return result;
}

    BYTE Clamp(BYTE min, BYTE max, double value)
    {
        if (value > max)
            return max;
        if (value < min)
            return min;
        return (BYTE)value;
    }

    HRESULT YCbCrPixelFormatConverter::ConvertYCbCrToRgb( 
        /* [in] */ const WICRect *prc,
        /* [in] */ UINT cbStride,
        /* [in] */ UINT cbPixelsSize,
        /* [out] */ BYTE *pbPixels)
    {
        HRESULT result = S_OK;
        
        //Sanity check
        WICPixelFormatGUID srcPixelFormat;
        bitmapSource->GetPixelFormat(&srcPixelFormat);
        if (srcPixelFormat != GUID_TestPixelFormat24bppYCbCr)
        {
             result = E_UNEXPECTED;                  
        }  

        UINT width, height;

        if (prc == NULL)
        {
            if (SUCCEEDED(result))
            {
                result = bitmapSource->GetSize(&width, &height);
            }
        }else
        {
            width = prc->Width;
            height = prc->Height;
        }
        
        BYTE* tempPixels = NULL;
        if (SUCCEEDED(result))
        {
            //Get the YCbCr pixels
            tempPixels = new BYTE[height*width*3];
            result = bitmapSource->CopyPixels(prc, width*3, height*width*3, tempPixels);
        }
        
        //Loop on the data and do the conversion
        //We cannot do an inplace conversion here since the pixel formats have different
        //number of bytes
        if (SUCCEEDED(result))
        {
            BYTE *srcPos = NULL;
            BYTE *destPos = NULL;
            srcPos = tempPixels;
            destPos = pbPixels;
            for (int i = 0 ; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    BYTE R, G ,B;
                    BYTE Y, Cb, Cr;

                    Cr = *srcPos;
                    Cb = *(srcPos+1);
                    Y = *(srcPos+2);      

                    //Do the maths
                    R = Clamp(0, 255, 1.164*(Y - 16) + 1.596*(Cr - 128));
                    G = Clamp(0, 255, 1.164*(Y - 16) - 0.813*(Cr - 128) - 0.392*(Cb - 128));
                    B = Clamp(0, 255, 1.164*(Y - 16) + 2.017*(Cb - 128));

                    *destPos = B;
                    *(destPos+1) = G;
                    *(destPos+2) = R;                     
                    *(destPos+3) = 255;      
                       
                    //Advance to next pixel
                    srcPos += 3;
                    destPos += 4;
                }
                destPos += (cbStride - (width * 4));
            }
        }
        
        delete[] tempPixels;        
        return result;    
    }


    HRESULT YCbCrPixelFormatConverter::ConvertRgbToYCbCr( 
        /* [in] */ const WICRect *prc,
        /* [in] */ UINT cbStride,
        /* [in] */ UINT cbPixelsSize,
        /* [out] */ BYTE *pbPixels)
    {
        HRESULT result = S_OK;
        
        //Sanity check
        WICPixelFormatGUID srcPixelFormat;
        bitmapSource->GetPixelFormat(&srcPixelFormat);
        if (srcPixelFormat != GUID_WICPixelFormat32bppBGRA)
        {
             result = E_UNEXPECTED;
             return result;
        }

        IWICImagingFactory *codecFactory = NULL;
        IWICFormatConverter *formatConverter = NULL;    

        if (SUCCEEDED(result))
        {
            result = CoCreateInstance(CLSID_WICImagingFactory, NULL, CLSCTX_INPROC_SERVER, IID_IWICImagingFactory, (LPVOID*) &codecFactory);
        }

        if (SUCCEEDED(result))
        {             
            result = codecFactory->CreateFormatConverter(&formatConverter);        
        }
        
        if (SUCCEEDED(result))
        {
            //We will convert to 24RGB first, since it is easier to convert to YCbCr from there
            result = formatConverter->Initialize(bitmapSource,
                GUID_WICPixelFormat24bppBGR, WICBitmapDitherTypeSolid, NULL, 1.0, WICBitmapPaletteTypeFixedWebPalette);
        }

        if (SUCCEEDED(result))
        {
            result = formatConverter->CopyPixels(prc, cbStride, cbPixelsSize, pbPixels);
        }
        //Since the two formats have same number of bytes, we will do an inplace conversion
        UINT width, height;
        if (prc == NULL)
        {
            if (SUCCEEDED(result))
            {
                result = bitmapSource->GetSize(&width, &height);
            }
        }
        else
        {
            width = prc->Width;
            height = prc->Height;
        }

        if (SUCCEEDED(result))
        {
            //Loop on the data and do the conversion
            BYTE *curPos = NULL;            
            curPos = pbPixels;
            for (int i = 0 ; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                BYTE R, G ,B;
                BYTE Y, Cb, Cr;

                B = *curPos;
                G = *(curPos+1);
                R = *(curPos+2);      

                //Do the maths
                Y = Clamp(0, 255, (0.257*R) + (0.504*G) + (0.098*B) + 16);
                Cb = Clamp(0, 255, (-0.148*R) - (0.291*G) + (0.439*B) + 128);
                Cr = Clamp(0, 255, (0.439*R) - (0.368*G) - (0.071*B) + 128);

                *curPos = Cr;
                *(curPos+1) = Cb;
                *(curPos+2) = Y;      
                   
                //Advance to next pixel
                curPos += 3;
                }
                curPos += (cbStride - (width * 3)); //Fast forward remaining part of the stride
            }
        }
        if (formatConverter)
        {
            formatConverter->Release();
        }

        if (codecFactory)
        {
            codecFactory->Release();
        }
        return result;
       }  

