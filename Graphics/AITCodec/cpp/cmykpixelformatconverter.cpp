//----------------------------------------------------------------------------------------
// THIS CODE AND INFORMATION IS PROVIDED "AS-IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------------------
//----------------------------------------------------------------------------------------
//  Description: Definition of CmykPixelFormatConverter
//----------------------------------------------------------------------------------------
#include "precomp.hpp"

// The Cmyk format GUID {9D14B98B-4427-41f0-A7AC-92CC96AE0BEB}
// {710194EC-F8B2-4b7a-94AD-945A7DCDEADB}
static const GUID GUID_TestPixelFormat32bppCmyk = 
{ 0x710194ec, 0xf8b2, 0x4b7a, { 0x94, 0xad, 0x94, 0x5a, 0x7d, 0xcd, 0xea, 0xdb } };

static const GUID CLSID_CmykPixelFormatConverter = 
{ 0xb312a459, 0x2a44, 0x4833, { 0xaa, 0x45, 0xf, 0x50, 0xbc, 0x66, 0x9a, 0x28 }};



CmykPixelFormatConverter::CmykPixelFormatConverter()
: BasePixelFormatConverter()
{}

STDMETHODIMP CmykPixelFormatConverter::CanConvert( 
        /* [in] */ REFWICPixelFormatGUID srcPixelFormat,
        /* [in] */ REFWICPixelFormatGUID dstPixelFormat,
        /* [out] */ BOOL *pfCanConvert)
{
    HRESULT result = E_INVALIDARG;

    if (NULL != pfCanConvert)
    {
        result = S_OK;

        // I can convert from Cmyk to RGB and back only
        if ((GUID_WICPixelFormat32bppBGRA == srcPixelFormat) &&
            (GUID_TestPixelFormat32bppCmyk == dstPixelFormat))
        {
            *pfCanConvert = TRUE;
        }
        else if ((GUID_TestPixelFormat32bppCmyk   == srcPixelFormat) &&
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

STDMETHODIMP CmykPixelFormatConverter::CopyPixels( 
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
            //Convert back to canonical 32PARGB            
            result = ConvertCmykToRgb(prc, cbStride, cbPixelsSize, pbPixels);
        }
        else if (destPixelFormat == GUID_TestPixelFormat32bppCmyk)
        {
            //Convert to newly supported Cmyk
            result = ConvertRgbToCmyk(prc, cbStride, cbPixelsSize, pbPixels);
        }
    }
    return result;
}


    HRESULT CmykPixelFormatConverter::ConvertCmykToRgb( 
        /* [in] */ const WICRect *prc,
        /* [in] */ UINT cbStride,
        /* [in] */ UINT cbPixelsSize,
        /* [out] */ BYTE *pbPixels)
    {
        HRESULT result = S_OK;
        
        //Sanity check
        WICPixelFormatGUID srcPixelFormat;
        bitmapSource->GetPixelFormat(&srcPixelFormat);
        if (srcPixelFormat != GUID_TestPixelFormat32bppCmyk)
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
        
        if (SUCCEEDED(result))
        {        
            result = bitmapSource->CopyPixels(prc, cbStride, cbPixelsSize, pbPixels);
        }

        //Since the two formats have same number of bytes, we will do an inplace conversion
        //Loop on the data and do the conversion
        if (SUCCEEDED(result))
        {
            BYTE *curPos = NULL;
            curPos = pbPixels;
            for (int i = 0 ; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    float R, G ,B;
                    float C, M, Y, K;

                    K = *curPos / 100.0;
                    Y = *(curPos+1) / 100.0;
                    M = *(curPos+2) / 100.0;      
                    C = *(curPos+3) / 100.0;      

                    //Do the maths
                    C = C*(1 - K) + K;      
                    M = M*(1 - K) + K;      
                    Y = Y*(1 - K) + K;

                    R = 1 - C;
                    G = 1 - M;
                    B = 1 - Y;
                    
                    *curPos = (BYTE)(B*255);
                    *(curPos+1) = (BYTE)(G*255);
                    *(curPos+2) = (BYTE)(R*255);                       
                    *(curPos+3) = 255;                             

                    //Advance to next pixel
                    curPos = curPos + 4;
                }
                curPos += (cbStride - (width * 4));
            }
        }    
        return result;    
    }


    HRESULT CmykPixelFormatConverter::ConvertRgbToCmyk( 
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

        if (SUCCEEDED(result))
        {
            result = bitmapSource->CopyPixels(prc, cbStride, cbPixelsSize, pbPixels);
        }
        //Since the two formats have same number of bytes, we will do an inplace conversion
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


        if (SUCCEEDED(result))
        {
            //Loop on the data and do the conversion
            BYTE *curPos = NULL;            
            curPos = pbPixels;
            for (int i = 0 ; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    float R, G ,B, A;
                    float C, M, Y, K;

                    
                    //Do the maths
                    A = *(curPos+3);
                    B = (*curPos)    * (A/255.0) / 255.0;
                    G = (*(curPos+1) * (A/255.0)) / 255.0;
                    R = (*(curPos+2) * (A/255.0)) / 255.0;      
                    

                    C = 1 - R;
                    M = 1 - G;
                    Y = 1 - B;
                    K = min(C,min(M, Y));
                    if (K >= 1)
                    {
                        C = 0;
                        M = 0;
                        Y = 0;                    
                    }
                    else
                    {
                    C = (C - K) / (1 - K);
                    M = (M - K) / (1 - K);
                    Y = (Y - K) / (1 - K);
                    }
     
                    *curPos = (BYTE)(K*100);
                    *(curPos+1) = (BYTE)(Y*100);
                    *(curPos+2) = (BYTE)(M*100);      
                    *(curPos+3) = (BYTE)(C*100);      
                       
                    //Advance to next pixel
                    curPos += 4;
                }
                curPos += (cbStride - (width * 4));
            }
        }
        return result;
       }