// hwndInWPF.cpp : Defines the entry point for the DLL application.
//

#include "stdafx.h"
#include "resource.h"



#ifdef _MANAGED
#pragma managed(push, off)
#endif

HINSTANCE hInstance;								// current instance

BOOL APIENTRY DllMain( HINSTANCE hInst,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
					 )
{
    hInstance = hInst;
    return TRUE;
}

#ifdef _MANAGED
#pragma managed(pop)
#endif


#define MAX_LOADSTRING 100

// Global Variables:
TCHAR szTitle[MAX_LOADSTRING];					// The title bar text
TCHAR szWindowClass[MAX_LOADSTRING];			// the main window class name

// Forward declarations of functions included in this code module:
ATOM				MyRegisterClass(HINSTANCE hInstance);
LRESULT CALLBACK	WndProc(HWND, UINT, WPARAM, LPARAM);
INT_PTR CALLBACK	About(HWND, UINT, WPARAM, LPARAM);


//
//  FUNCTION: MyRegisterClass()
//
//  PURPOSE: Registers the window class.
//
//  COMMENTS:
//
//    This function and its usage are only necessary if you want this code
//    to be compatible with Win32 systems prior to the 'RegisterClassEx'
//    function that was added to Windows 95. It is important to call this function
//    so that the application will get 'well formed' small icons associated
//    with it.
//
ATOM MyRegisterClass(HINSTANCE hInstance)
{
	WNDCLASSEX wcex;

	wcex.cbSize = sizeof(WNDCLASSEX);

	wcex.style			= CS_HREDRAW | CS_VREDRAW;
	wcex.lpfnWndProc	= WndProc;
	wcex.cbClsExtra		= 0;
	wcex.cbWndExtra		= 0;
	wcex.hInstance		= hInstance;
	wcex.hIcon			= LoadIcon(hInstance, MAKEINTRESOURCE(IDI_TYPICALWIN32DIALOG));
	wcex.hCursor		= LoadCursor(NULL, IDC_ARROW);
	wcex.hbrBackground	= (HBRUSH)(COLOR_WINDOW+1);
	wcex.lpszMenuName	= MAKEINTRESOURCE(IDC_TYPICALWIN32DIALOG);
	wcex.lpszClassName	= szWindowClass;
	wcex.hIconSm		= LoadIcon(wcex.hInstance, MAKEINTRESOURCE(IDI_SMALL));

	return RegisterClassEx(&wcex);
}


//
//  FUNCTION: WndProc(HWND, UINT, WPARAM, LPARAM)
//
//  PURPOSE:  Processes messages for the main window.
//
//  WM_COMMAND	- process the application menu
//  WM_PAINT	- Paint the main window
//  WM_DESTROY	- post a quit message and return
//
//
LRESULT CALLBACK WndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam)
{
	int wmId, wmEvent;
	PAINTSTRUCT ps;
	HDC hdc;

	switch (message)
	{
	case WM_COMMAND:
		wmId    = LOWORD(wParam);
		wmEvent = HIWORD(wParam);
		// Parse the menu selections:
		switch (wmId)
		{
		case IDM_ABOUT:
			DialogBox(hInstance, MAKEINTRESOURCE(IDD_ABOUTBOX), hWnd, About);
			break;
		case IDM_EXIT:
			DestroyWindow(hWnd);
			break;
		default:
			return DefWindowProc(hWnd, message, wParam, lParam);
		}
		break;
	case WM_PAINT:
		hdc = BeginPaint(hWnd, &ps);
		// TODO: Add any drawing code here...
		EndPaint(hWnd, &ps);
		break;
	case WM_DESTROY:
		PostQuitMessage(0);
		break;
	default:
		return DefWindowProc(hWnd, message, wParam, lParam);
	}
	return 0;
}

// Message handler for about box.
INT_PTR CALLBACK About(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
{
	UNREFERENCED_PARAMETER(lParam);
	switch (message)
	{
	case WM_INITDIALOG:
		return (INT_PTR)TRUE;

	case WM_COMMAND:
		if (LOWORD(wParam) == IDOK || LOWORD(wParam) == IDCANCEL)
		{
			// EndDialog(hDlg, LOWORD(wParam));
			return (INT_PTR)TRUE;
		}
		break;
	}
	return (INT_PTR)FALSE;
}

    bool initialized = false;
    void InitializeGlobals() {
        if (initialized) return;
        initialized = true;

        // Initialize global strings
        LoadString(hInstance, IDS_APP_TITLE, szTitle, MAX_LOADSTRING);
        LoadString(hInstance, IDC_TYPICALWIN32DIALOG, szWindowClass, MAX_LOADSTRING);
        MyRegisterClass(hInstance);
    }


namespace ManagedCpp
{
	using namespace System;
	using namespace System::Windows;
    using namespace System::Windows::Interop;
    using namespace System::Windows::Input;
    using namespace System::Windows::Media;
    using namespace System::Runtime::InteropServices;

    public ref class MyHwndHost : public HwndHost, IKeyboardInputSink {
    private:
        HWND dialog;

        // helpful conversion function
        ::MSG ConvertMessage(System::Windows::Interop::MSG% msg) {
            ::MSG m;
            m.hwnd = (HWND) msg.hwnd.ToPointer();
            m.lParam = (LPARAM) msg.lParam.ToPointer();
            m.message = msg.message;
            m.wParam = (WPARAM) msg.wParam.ToPointer();
            
            m.time = msg.time;

            POINT pt;
            pt.x = msg.pt_x;
            pt.y = msg.pt_y;
            m.pt = pt;

            return m;
    }

    protected: 
        virtual HandleRef BuildWindowCore(HandleRef hwndParent) override {
            InitializeGlobals();            
            dialog = CreateDialog(hInstance, 
                MAKEINTRESOURCE(IDD_DIALOG1), 
                (HWND) hwndParent.Handle.ToPointer(),
                (DLGPROC) About); 
            return HandleRef(this, IntPtr(dialog));
        }

        virtual void DestroyWindowCore(HandleRef hwnd) override {
            // hwnd will be disposed for us
        }

    public: 
        virtual bool TabIntoCore(TraversalRequest^ request) override {
            if (request->FocusNavigationDirection == FocusNavigationDirection::Last) {
                HWND lastTabStop = GetDlgItem(dialog, IDCANCEL);
                SetFocus(lastTabStop);
            }
            else {
                HWND firstTabStop = GetDlgItem(dialog, IDC_EDIT1);
                SetFocus(firstTabStop);
            }
            return true;
        }

#undef TranslateAccelerator
        virtual bool TranslateAcceleratorCore(System::Windows::Interop::MSG% msg, 
            ModifierKeys modifiers) override 
        {
            ::MSG m = ConvertMessage(msg);

            // Win32's IsDialogMessage() will handle most of our tabbing, but doesn't know 
            // what to do when it reaches the last tab stop
            if (m.message == WM_KEYDOWN && m.wParam == VK_TAB) {
                HWND firstTabStop = GetDlgItem(dialog, IDC_EDIT1);
                HWND lastTabStop = GetDlgItem(dialog, IDCANCEL);
                TraversalRequest^ request = nullptr;

                if (GetKeyState(VK_SHIFT) && GetFocus() == firstTabStop) {
                    // this code should work, but there’s a bug with interop shift-tab in current builds                    
                    request = gcnew TraversalRequest(FocusNavigationDirection::Last);
                }
                else if (!GetKeyState(VK_SHIFT) && GetFocus() == lastTabStop) {
                    request = gcnew TraversalRequest(FocusNavigationDirection::Next);
                }

				if (request != nullptr) {
                    return ((IKeyboardInputSink^)this)->KeyboardInputSite->OnNoMoreTabStops(request);
				}
            }

            // Only call IsDialogMessage for keys it will do something with.
            if (msg.message == WM_SYSKEYDOWN || msg.message == WM_KEYDOWN) {
                switch (m.wParam) {
                    case VK_TAB:
                    case VK_LEFT:
                    case VK_UP:
                    case VK_RIGHT:
                    case VK_DOWN:
                    case VK_EXECUTE:
                    case VK_RETURN:
                    case VK_ESCAPE:
                    case VK_CANCEL:
                        IsDialogMessage(dialog, &m);
                        // IsDialogMessage should be called ProcessDialogMessage --
                        // it processes messages without ever really telling you
                        // if it handled a specific message or not
                        return true;
                }
            }

            return false; // not a key we handled
        }

        virtual bool OnMnemonicCore(System::Windows::Interop::MSG% msg, ModifierKeys modifiers) override {
            ::MSG m = ConvertMessage(msg);

            // If it's one of our mnemonics, set focus to the appropriate hwnd
            if (msg.message == WM_SYSCHAR && GetKeyState(VK_MENU /*alt*/)) {
                int dialogitem = 9999;
                switch (m.wParam) {
                    case 's': dialogitem = IDOK; break;
                    case 'c': dialogitem = IDCANCEL; break;
                    case 'f': dialogitem = IDC_EDIT1; break;
                    case 'l': dialogitem = IDC_EDIT2; break;
                    case 'p': dialogitem = IDC_EDIT3; break;
                    case 'a': dialogitem = IDC_EDIT4; break;
                    case 'i': dialogitem = IDC_EDIT5; break;
                    case 't': dialogitem = IDC_EDIT6; break;
                    case 'z': dialogitem = IDC_EDIT7; break;
                }
                if (dialogitem != 9999) {
                    HWND hwnd = GetDlgItem(dialog, dialogitem);
                    SetFocus(hwnd);
                    return true;
                }
            }
            return false; // key unhandled
        };
    };
}
