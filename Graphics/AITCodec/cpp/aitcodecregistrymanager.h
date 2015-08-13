//----------------------------------------------------------------------------------------
// THIS CODE AND INFORMATION IS PROVIDED "AS-IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------------------
//----------------------------------------------------------------------------------------
//  Description: Declaration and Definition of TestCodecsRegistryManager
//----------------------------------------------------------------------------------------
#pragma once


class TestCodecsRegistryManager : public RegistryValueManager
{
public:
     virtual HRESULT PopulateValues()
    {
        SetCodecDLLPath();

        RegisterAITDecoder();
        RegisterAITEncoder();        
        RegisterYCbCrFormatConverter();
        RegisterCMYKFormatConverter();
        return S_OK;
    }

private:

   //Register function for every extensible element
    void RegisterAITDecoder()
    {
        BYTE bytes[8] = { 0 };
        // AIT Decoder
        Val("CLSID\\{7ED96837-96F0-4812-B211-F13C24117ED3}\\Instance\\{DEC50239-14BB-4F70-AD2A-B7126219A6DC}",
            "CLSID", "{DEC50239-14BB-4F70-AD2A-B7126219A6DC}");
        Val("CLSID\\{7ED96837-96F0-4812-B211-F13C24117ED3}\\Instance\\{DEC50239-14BB-4F70-AD2A-B7126219A6DC}",
            "FriendlyName", "Test AIT Decoder");
        Val("CLSID\\{DEC50239-14BB-4F70-AD2A-B7126219A6DC}",
            "Version", "1.0.0.0");
        Val("CLSID\\{DEC50239-14BB-4F70-AD2A-B7126219A6DC}",
            "Date", "2004-09-01");
        Val("CLSID\\{DEC50239-14BB-4F70-AD2A-B7126219A6DC}",
            "SpecVersion", "1.0.0.0");
        Val("CLSID\\{DEC50239-14BB-4F70-AD2A-B7126219A6DC}",
            "ColorManagementVersion", "1.0.0.0");
        Val("CLSID\\{DEC50239-14BB-4F70-AD2A-B7126219A6DC}",
            "MimeTypes", "x-image/ait");
        Val("CLSID\\{DEC50239-14BB-4F70-AD2A-B7126219A6DC}",
            "FileExtensions", ".ait");
        Val("CLSID\\{DEC50239-14BB-4F70-AD2A-B7126219A6DC}",
            "SupportsAnimation", 1);
        Val("CLSID\\{DEC50239-14BB-4F70-AD2A-B7126219A6DC}",
            "SupportChromakey", 1);
        Val("CLSID\\{DEC50239-14BB-4F70-AD2A-B7126219A6DC}",
            "SupportLossless", 1);
        Val("CLSID\\{DEC50239-14BB-4F70-AD2A-B7126219A6DC}",
            "SupportMultiframe", 1);
        Val("CLSID\\{DEC50239-14BB-4F70-AD2A-B7126219A6DC}",
            "ContainerFormat", "{CCC50239-14BB-4F70-AD2A-B7126219A6DC}");
        Val("CLSID\\{DEC50239-14BB-4F70-AD2A-B7126219A6DC}",
            "Author", "WPF Imaging Test");
        Val("CLSID\\{DEC50239-14BB-4F70-AD2A-B7126219A6DC}",
            "Description", "WPF Imaging Test Decoder");
        Val("CLSID\\{DEC50239-14BB-4F70-AD2A-B7126219A6DC}",
            "FriendlyName", "Test AIT Decoder");
        Val("CLSID\\{DEC50239-14BB-4F70-AD2A-B7126219A6DC}\\Formats",
            "", "");
        Val("CLSID\\{DEC50239-14BB-4F70-AD2A-B7126219A6DC}\\Formats\\{6fddc324-4e03-4bfe-b185-3d77768dc901}",
            "", "");
        Val("CLSID\\{DEC50239-14BB-4F70-AD2A-B7126219A6DC}\\Formats\\{6fddc324-4e03-4bfe-b185-3d77768dc902}",
            "", "");
        Val("CLSID\\{DEC50239-14BB-4F70-AD2A-B7126219A6DC}\\Formats\\{6fddc324-4e03-4bfe-b185-3d77768dc903}",
            "", "");
        Val("CLSID\\{DEC50239-14BB-4F70-AD2A-B7126219A6DC}\\Formats\\{6fddc324-4e03-4bfe-b185-3d77768dc904}",
            "", "");
        Val("CLSID\\{DEC50239-14BB-4F70-AD2A-B7126219A6DC}\\Formats\\{6fddc324-4e03-4bfe-b185-3d77768dc905}",
            "", "");
        Val("CLSID\\{DEC50239-14BB-4F70-AD2A-B7126219A6DC}\\Formats\\{6fddc324-4e03-4bfe-b185-3d77768dc906}",
            "", "");
        Val("CLSID\\{DEC50239-14BB-4F70-AD2A-B7126219A6DC}\\Formats\\{6fddc324-4e03-4bfe-b185-3d77768dc907}",
            "", "");
        Val("CLSID\\{DEC50239-14BB-4F70-AD2A-B7126219A6DC}\\Formats\\{6fddc324-4e03-4bfe-b185-3d77768dc908}",
            "", "");
        Val("CLSID\\{DEC50239-14BB-4F70-AD2A-B7126219A6DC}\\Formats\\{6fddc324-4e03-4bfe-b185-3d77768dc909}",
            "", "");
        Val("CLSID\\{DEC50239-14BB-4F70-AD2A-B7126219A6DC}\\Formats\\{6fddc324-4e03-4bfe-b185-3d77768dc90a}",
            "", "");
        Val("CLSID\\{DEC50239-14BB-4F70-AD2A-B7126219A6DC}\\Formats\\{6fddc324-4e03-4bfe-b185-3d77768dc90b}",
            "", "");
        Val("CLSID\\{DEC50239-14BB-4F70-AD2A-B7126219A6DC}\\Formats\\{6fddc324-4e03-4bfe-b185-3d77768dc90c}",
            "", "");
        Val("CLSID\\{DEC50239-14BB-4F70-AD2A-B7126219A6DC}\\Formats\\{6fddc324-4e03-4bfe-b185-3d77768dc90d}",
            "", "");
        Val("CLSID\\{DEC50239-14BB-4F70-AD2A-B7126219A6DC}\\Formats\\{6fddc324-4e03-4bfe-b185-3d77768dc90e}",
            "", "");
        Val("CLSID\\{DEC50239-14BB-4F70-AD2A-B7126219A6DC}\\Formats\\{6fddc324-4e03-4bfe-b185-3d77768dc90f}",
            "", "");
        Val("CLSID\\{DEC50239-14BB-4F70-AD2A-B7126219A6DC}\\Formats\\{6fddc324-4e03-4bfe-b185-3d77768dc910}",
            "", "");
        Val("CLSID\\{DEC50239-14BB-4F70-AD2A-B7126219A6DC}\\Formats\\{6fddc324-4e03-4bfe-b185-3d77768dc911}",
            "", "");
        Val("CLSID\\{DEC50239-14BB-4F70-AD2A-B7126219A6DC}\\Formats\\{6fddc324-4e03-4bfe-b185-3d77768dc912}",
            "", "");
        Val("CLSID\\{DEC50239-14BB-4F70-AD2A-B7126219A6DC}\\Formats\\{6fddc324-4e03-4bfe-b185-3d77768dc913}",
            "", "");
        Val("CLSID\\{DEC50239-14BB-4F70-AD2A-B7126219A6DC}\\Formats\\{6fddc324-4e03-4bfe-b185-3d77768dc914}",
            "", "");
        Val("CLSID\\{DEC50239-14BB-4F70-AD2A-B7126219A6DC}\\Formats\\{6fddc324-4e03-4bfe-b185-3d77768dc915}",
            "", "");
        Val("CLSID\\{DEC50239-14BB-4F70-AD2A-B7126219A6DC}\\Formats\\{6fddc324-4e03-4bfe-b185-3d77768dc916}",
            "", "");
        Val("CLSID\\{DEC50239-14BB-4F70-AD2A-B7126219A6DC}\\Formats\\{6fddc324-4e03-4bfe-b185-3d77768dc917}",
            "", "");
        Val("CLSID\\{DEC50239-14BB-4F70-AD2A-B7126219A6DC}\\Formats\\{6fddc324-4e03-4bfe-b185-3d77768dc918}",
            "", "");
        Val("CLSID\\{DEC50239-14BB-4F70-AD2A-B7126219A6DC}\\Formats\\{6fddc324-4e03-4bfe-b185-3d77768dc919}",
            "", "");
        Val("CLSID\\{DEC50239-14BB-4F70-AD2A-B7126219A6DC}\\Formats\\{6fddc324-4e03-4bfe-b185-3d77768dc91a}",
            "", "");
        Val("CLSID\\{DEC50239-14BB-4F70-AD2A-B7126219A6DC}\\Formats\\{6fddc324-4e03-4bfe-b185-3d77768dc91b}",
            "", "");
        Val("CLSID\\{DEC50239-14BB-4F70-AD2A-B7126219A6DC}\\Formats\\{6fddc324-4e03-4bfe-b185-3d77768dc91c}",
            "", "");
        Val("CLSID\\{DEC50239-14BB-4F70-AD2A-B7126219A6DC}\\InprocServer32",
            "", moduleFilename);
        Val("CLSID\\{DEC50239-14BB-4F70-AD2A-B7126219A6DC}\\Patterns",
            "", "");
        Val("CLSID\\{DEC50239-14BB-4F70-AD2A-B7126219A6DC}\\Patterns\\0",
            "Position", (DWORD)0);
        Val("CLSID\\{DEC50239-14BB-4F70-AD2A-B7126219A6DC}\\Patterns\\0",
            "Length", 4);
        bytes[0] = 'A'; bytes[1] = 'I'; bytes[2] = 'T'; bytes[3] = '\0';
        Val("CLSID\\{DEC50239-14BB-4F70-AD2A-B7126219A6DC}\\Patterns\\0",
            "Pattern", bytes, 4);
        bytes[0] = bytes[1] = bytes[2] = bytes[3] = 0xFF;
        Val("CLSID\\{DEC50239-14BB-4F70-AD2A-B7126219A6DC}\\Patterns\\0",
            "Mask", bytes, 4);
    };

    void RegisterAITEncoder()
    {
         // AIT Encoder
        Val("CLSID\\{AC757296-3522-4e11-9862-C17BE5A1767E}\\Instance\\{EEC50239-14BB-4F70-AD2A-B7126219A6DC}",
            "CLSID", "{EEC50239-14BB-4F70-AD2A-B7126219A6DC}");
        Val("CLSID\\{AC757296-3522-4e11-9862-C17BE5A1767E}\\Instance\\{EEC50239-14BB-4F70-AD2A-B7126219A6DC}",
            "FriendlyName", "Test AIT Encoder");
        Val("CLSID\\{EEC50239-14BB-4F70-AD2A-B7126219A6DC}",
            "Version", "1.0.0.0");
        Val("CLSID\\{EEC50239-14BB-4F70-AD2A-B7126219A6DC}",
            "Date", "2004-09-01");
        Val("CLSID\\{EEC50239-14BB-4F70-AD2A-B7126219A6DC}",
            "SpecVersion", "1.0.0.0");
        Val("CLSID\\{EEC50239-14BB-4F70-AD2A-B7126219A6DC}",
            "ColorManagementVersion", "1.0.0.0");
        Val("CLSID\\{EEC50239-14BB-4F70-AD2A-B7126219A6DC}",
            "MimeTypes", "x-image/ait");
        Val("CLSID\\{EEC50239-14BB-4F70-AD2A-B7126219A6DC}",
            "SupportsAnimation", 1);
        Val("CLSID\\{EEC50239-14BB-4F70-AD2A-B7126219A6DC}",
            "SupportChromakey", 1);
        Val("CLSID\\{EEC50239-14BB-4F70-AD2A-B7126219A6DC}",
            "SupportLossless", 1);
        Val("CLSID\\{EEC50239-14BB-4F70-AD2A-B7126219A6DC}",
            "SupportMultiframe", 1);
        Val("CLSID\\{EEC50239-14BB-4F70-AD2A-B7126219A6DC}",
            "ContainerFormat", "{CCC50239-14BB-4F70-AD2A-B7126219A6DC}");
        Val("CLSID\\{EEC50239-14BB-4F70-AD2A-B7126219A6DC}",
            "Author", "WPF Imaging Test");
        Val("CLSID\\{EEC50239-14BB-4F70-AD2A-B7126219A6DC}",
            "Description", "WPF Imaging Test Encoder");
        Val("CLSID\\{EEC50239-14BB-4F70-AD2A-B7126219A6DC}",
            "FriendlyName", "Test AIT Encoder");
        Val("CLSID\\{EEC50239-14BB-4F70-AD2A-B7126219A6DC}\\Formats",
            "", "");
        Val("CLSID\\{EEC50239-14BB-4F70-AD2A-B7126219A6DC}\\Formats\\{6fddc324-4e03-4bfe-b185-3d77768dc901}",
            "", "");
        Val("CLSID\\{EEC50239-14BB-4F70-AD2A-B7126219A6DC}\\Formats\\{6fddc324-4e03-4bfe-b185-3d77768dc902}",
            "", "");
        Val("CLSID\\{EEC50239-14BB-4F70-AD2A-B7126219A6DC}\\Formats\\{6fddc324-4e03-4bfe-b185-3d77768dc903}",
            "", "");
        Val("CLSID\\{EEC50239-14BB-4F70-AD2A-B7126219A6DC}\\Formats\\{6fddc324-4e03-4bfe-b185-3d77768dc904}",
            "", "");
        Val("CLSID\\{EEC50239-14BB-4F70-AD2A-B7126219A6DC}\\Formats\\{6fddc324-4e03-4bfe-b185-3d77768dc905}",
            "", "");
        Val("CLSID\\{EEC50239-14BB-4F70-AD2A-B7126219A6DC}\\Formats\\{6fddc324-4e03-4bfe-b185-3d77768dc906}",
            "", "");
        Val("CLSID\\{EEC50239-14BB-4F70-AD2A-B7126219A6DC}\\Formats\\{6fddc324-4e03-4bfe-b185-3d77768dc907}",
            "", "");
        Val("CLSID\\{EEC50239-14BB-4F70-AD2A-B7126219A6DC}\\Formats\\{6fddc324-4e03-4bfe-b185-3d77768dc908}",
            "", "");
        Val("CLSID\\{EEC50239-14BB-4F70-AD2A-B7126219A6DC}\\Formats\\{6fddc324-4e03-4bfe-b185-3d77768dc909}",
            "", "");
        Val("CLSID\\{EEC50239-14BB-4F70-AD2A-B7126219A6DC}\\Formats\\{6fddc324-4e03-4bfe-b185-3d77768dc90a}",
            "", "");
        Val("CLSID\\{EEC50239-14BB-4F70-AD2A-B7126219A6DC}\\Formats\\{6fddc324-4e03-4bfe-b185-3d77768dc90b}",
            "", "");
        Val("CLSID\\{EEC50239-14BB-4F70-AD2A-B7126219A6DC}\\Formats\\{6fddc324-4e03-4bfe-b185-3d77768dc90c}",
            "", "");
        Val("CLSID\\{EEC50239-14BB-4F70-AD2A-B7126219A6DC}\\Formats\\{6fddc324-4e03-4bfe-b185-3d77768dc90d}",
            "", "");
        Val("CLSID\\{EEC50239-14BB-4F70-AD2A-B7126219A6DC}\\Formats\\{6fddc324-4e03-4bfe-b185-3d77768dc90e}",
            "", "");
        Val("CLSID\\{EEC50239-14BB-4F70-AD2A-B7126219A6DC}\\Formats\\{6fddc324-4e03-4bfe-b185-3d77768dc90f}",
            "", "");
        Val("CLSID\\{EEC50239-14BB-4F70-AD2A-B7126219A6DC}\\Formats\\{6fddc324-4e03-4bfe-b185-3d77768dc910}",
            "", "");
        Val("CLSID\\{EEC50239-14BB-4F70-AD2A-B7126219A6DC}\\Formats\\{6fddc324-4e03-4bfe-b185-3d77768dc911}",
            "", "");
        Val("CLSID\\{EEC50239-14BB-4F70-AD2A-B7126219A6DC}\\Formats\\{6fddc324-4e03-4bfe-b185-3d77768dc912}",
            "", "");
        Val("CLSID\\{EEC50239-14BB-4F70-AD2A-B7126219A6DC}\\Formats\\{6fddc324-4e03-4bfe-b185-3d77768dc913}",
            "", "");
        Val("CLSID\\{EEC50239-14BB-4F70-AD2A-B7126219A6DC}\\Formats\\{6fddc324-4e03-4bfe-b185-3d77768dc914}",
            "", "");
        Val("CLSID\\{EEC50239-14BB-4F70-AD2A-B7126219A6DC}\\Formats\\{6fddc324-4e03-4bfe-b185-3d77768dc915}",
            "", "");
        Val("CLSID\\{EEC50239-14BB-4F70-AD2A-B7126219A6DC}\\Formats\\{6fddc324-4e03-4bfe-b185-3d77768dc916}",
            "", "");
        Val("CLSID\\{EEC50239-14BB-4F70-AD2A-B7126219A6DC}\\Formats\\{6fddc324-4e03-4bfe-b185-3d77768dc917}",
            "", "");
        Val("CLSID\\{EEC50239-14BB-4F70-AD2A-B7126219A6DC}\\Formats\\{6fddc324-4e03-4bfe-b185-3d77768dc918}",
            "", "");
        Val("CLSID\\{EEC50239-14BB-4F70-AD2A-B7126219A6DC}\\Formats\\{6fddc324-4e03-4bfe-b185-3d77768dc919}",
            "", "");
        Val("CLSID\\{EEC50239-14BB-4F70-AD2A-B7126219A6DC}\\Formats\\{6fddc324-4e03-4bfe-b185-3d77768dc91a}",
            "", "");
        Val("CLSID\\{EEC50239-14BB-4F70-AD2A-B7126219A6DC}\\Formats\\{6fddc324-4e03-4bfe-b185-3d77768dc91b}",
            "", "");
        Val("CLSID\\{EEC50239-14BB-4F70-AD2A-B7126219A6DC}\\Formats\\{6fddc324-4e03-4bfe-b185-3d77768dc91c}",
            "", "");
        Val("CLSID\\{EEC50239-14BB-4F70-AD2A-B7126219A6DC}\\InprocServer32",
            "", moduleFilename);
    };

    void RegisterYCbCrFormatConverter()
    {
        Val("CLSID\\{7835EAE8-BF14-49D1-93CE-533A407B2248}\\Instance\\{49CC393E-CE46-11D9-8BDE-F66BAD1E3F3A}",
            "CLSID", "{49CC393E-CE46-11D9-8BDE-F66BAD1E3F3A}");                      
        Val("CLSID\\{7835EAE8-BF14-49D1-93CE-533A407B2248}\\Instance\\{49CC393E-CE46-11D9-8BDE-F66BAD1E3F3A}",
            "FriendlyName", "Test YcbCr Pixel Format Converter");
        Val("CLSID\\{49CC393E-CE46-11D9-8BDE-F66BAD1E3F3A}",
            "Date", "2005-05-26");
        Val("CLSID\\{49CC393E-CE46-11D9-8BDE-F66BAD1E3F3A}",
            "SpecVersion", "1.0.0.0");
        Val("CLSID\\{49CC393E-CE46-11D9-8BDE-F66BAD1E3F3A}",
            "Version", "1.0.0.0");
        Val("CLSID\\{49CC393E-CE46-11D9-8BDE-F66BAD1E3F3A}\\InprocServer32",
            "", moduleFilename);
        Val("CLSID\\{49CC393E-CE46-11D9-8BDE-F66BAD1E3F3A}\\InprocServer32",
            "ThreadingModel", "Both");
        Val("CLSID\\{49CC393E-CE46-11D9-8BDE-F66BAD1E3F3A}\\PixelFormats\\{9D14B98B-4427-41f0-A7AC-92CC96AE0BEB}",
            "", "");      
         
        Val("CLSID\\{2b46e70f-cda7-473e-89f6-dc9630a2390b}\\Instance\\{9D14B98B-4427-41f0-A7AC-92CC96AE0BEB}",
            "CLSID", "{9D14B98B-4427-41f0-A7AC-92CC96AE0BEB}");
        Val("CLSID\\{2b46e70f-cda7-473e-89f6-dc9630a2390b}\\Instance\\{9D14B98B-4427-41f0-A7AC-92CC96AE0BEB}",
            "FriendlyName", "Test YCbCr Pixel Format");

        Val("CLSID\\{9D14B98B-4427-41f0-A7AC-92CC96AE0BEB}",
            "Author", "WPF Imaging Test");
        Val("CLSID\\{9D14B98B-4427-41f0-A7AC-92CC96AE0BEB}",
            "FriendlyName", "Test YCbCr Pixel Format");

        Val("CLSID\\{9D14B98B-4427-41f0-A7AC-92CC96AE0BEB}",
            "ChannelCount", (DWORD)3);
        Val("CLSID\\{9D14B98B-4427-41f0-A7AC-92CC96AE0BEB}",
            "BitLength", (DWORD)24);

        BYTE mask0[] = {0xFF,0,0};
        BYTE mask1[] = {0,0xFF,0};
        BYTE mask2[] = {0,0,0xFF};
        Val("CLSID\\{9D14B98B-4427-41f0-A7AC-92CC96AE0BEB}\\ChannelMasks",
            "0", mask0, 3);
        Val("CLSID\\{9D14B98B-4427-41f0-A7AC-92CC96AE0BEB}\\ChannelMasks",
            "1", mask1, 3);
        Val("CLSID\\{9D14B98B-4427-41f0-A7AC-92CC96AE0BEB}\\ChannelMasks",
            "2", mask2, 3);
    };

    void RegisterCMYKFormatConverter()
    {
                                                                       
        Val("CLSID\\{7835EAE8-BF14-49D1-93CE-533A407B2248}\\Instance\\{B312A459-2A44-4833-AA45-0F50BC669A28}",
            "CLSID", "{B312A459-2A44-4833-AA45-0F50BC669A28}");                      
        Val("CLSID\\{7835EAE8-BF14-49D1-93CE-533A407B2248}\\Instance\\{B312A459-2A44-4833-AA45-0F50BC669A28}",
            "FriendlyName", "Test CMYK Pixel Format Converter");
        Val("CLSID\\{B312A459-2A44-4833-AA45-0F50BC669A28}",
            "Date", "2005-05-26");
        Val("CLSID\\{B312A459-2A44-4833-AA45-0F50BC669A28}",
            "SpecVersion", "1.0.0.0");
        Val("CLSID\\{B312A459-2A44-4833-AA45-0F50BC669A28}",
            "Version", "1.0.0.0");
        Val("CLSID\\{B312A459-2A44-4833-AA45-0F50BC669A28}\\InprocServer32",
            "", moduleFilename);
        Val("CLSID\\{B312A459-2A44-4833-AA45-0F50BC669A28}\\InprocServer32",
            "ThreadingModel", "Both");
        Val("CLSID\\{B312A459-2A44-4833-AA45-0F50BC669A28}\\PixelFormats\\{710194EC-F8B2-4b7a-94AD-945A7DCDEADB}",
            "", "");      

        Val("CLSID\\{2b46e70f-cda7-473e-89f6-dc9630a2390b}\\Instance\\{710194EC-F8B2-4b7a-94AD-945A7DCDEADB}",
            "CLSID", "{710194EC-F8B2-4b7a-94AD-945A7DCDEADB}");
        Val("CLSID\\{2b46e70f-cda7-473e-89f6-dc9630a2390b}\\Instance\\{710194EC-F8B2-4b7a-94AD-945A7DCDEADB}",
            "FriendlyName", "Test CMYK Pixel Format");

        Val("CLSID\\{710194EC-F8B2-4b7a-94AD-945A7DCDEADB}",
            "Author", "WPF Imaging Test");
        Val("CLSID\\{710194EC-F8B2-4b7a-94AD-945A7DCDEADB}",
            "FriendlyName", "Test CMYK Pixel Format");

        Val("CLSID\\{710194EC-F8B2-4b7a-94AD-945A7DCDEADB}",
            "ChannelCount", (DWORD)4);
        Val("CLSID\\{710194EC-F8B2-4b7a-94AD-945A7DCDEADB}",
            "BitLength", (DWORD)32);

        BYTE mask0[] = {0xFF,0,0,0};
        BYTE mask1[] = {0,0xFF,0,0};
        BYTE mask2[] = {0,0,0xFF,0};
        BYTE mask3[] = {0,0,0,0xFF};
        Val("CLSID\\{710194EC-F8B2-4b7a-94AD-945A7DCDEADB}\\ChannelMasks",
            "0", mask0, 4);
        Val("CLSID\\{710194EC-F8B2-4b7a-94AD-945A7DCDEADB}\\ChannelMasks",
            "1", mask1, 4);
        Val("CLSID\\{710194EC-F8B2-4b7a-94AD-945A7DCDEADB}\\ChannelMasks",
            "2", mask2, 4);
        Val("CLSID\\{710194EC-F8B2-4b7a-94AD-945A7DCDEADB}\\ChannelMasks",
            "3", mask3, 4);
    };

    void SetCodecDLLPath()
    {
        HMODULE curModule = GetModuleHandle("AITCodecs.dll");
        
        if (curModule != NULL)
        {
            GetModuleFileNameA(curModule, moduleFilename, MAX_PATH);
        }
    }
 //Codec Dll full path
 char moduleFilename[MAX_PATH];
};
