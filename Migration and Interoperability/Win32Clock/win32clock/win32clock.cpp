// w32clock.cpp : Defines the entry point for the application.
//

#include "stdafx.h"
#include "win32clock.h"
#include "commctrl.h"
#include "uxtheme.h"

#define MAX_LOADSTRING 100

// Global Variables:
HINSTANCE hInst;                                // current instance
TCHAR szTitle[MAX_LOADSTRING];                  // The title bar text
TCHAR szWindowClass[MAX_LOADSTRING];            // the main window class name

// Forward declarations of functions included in this code module:
LRESULT CALLBACK    About(HWND, UINT, WPARAM, LPARAM);

[System::STAThreadAttribute]
int APIENTRY _tWinMain(HINSTANCE hInstance,
                     HINSTANCE hPrevInstance,
                     LPTSTR    lpCmdLine,
                     int       nCmdShow)
{
    UNREFERENCED_PARAMETER(hPrevInstance);
    UNREFERENCED_PARAMETER(lpCmdLine);

    INITCOMMONCONTROLSEX st;
    st.dwSize = sizeof(INITCOMMONCONTROLSEX);
    st.dwICC = ICC_TAB_CLASSES | ICC_DATE_CLASSES;
    InitCommonControlsEx(&st);


    // Initialize global strings
    LoadString(hInstance, IDS_APP_TITLE, szTitle, MAX_LOADSTRING);
    LoadString(hInstance, IDC_W32CLOCK, szWindowClass, MAX_LOADSTRING);

    DialogBox(hInst, (LPCTSTR)IDD_PROPPAGE_MEDIUM, NULL, (DLGPROC)About);

    return 0;
}

namespace ManagedCode
{
    using namespace System;
    using namespace System::Windows;
    using namespace System::Windows::Interop;
    using namespace System::Windows::Media;

    HWND GetHwnd(HWND parent, int x, int y, int width, int height) {
        HwndSource^ source = gcnew HwndSource(
            0, // class style
            WS_VISIBLE | WS_CHILD, // style
            0, // exstyle
            x, y, width, height,
            "hi", // NAME
            IntPtr(parent)        // parent window 
            );
        
        UIElement^ page = gcnew WPFClock::Clock();
        source->RootVisual = page;
        return (HWND) source->Handle.ToPointer();
    }
}

void Reparent(HWND hwnd, HWND oldParent, HWND newParent) {
    int result = 0;
    RECT  rectangle; 
    GetWindowRect(hwnd, &rectangle);
    int width = rectangle.right - rectangle.left;
    int height = rectangle.bottom - rectangle.top;
    POINT point;
    point.x = rectangle.left;
    point.y = rectangle.top;
    result = MapWindowPoints(NULL, newParent, &point, 1);
    SetWindowPos( hwnd, HWND_TOP, point.x, point.y, width, height, SWP_NOSIZE);
    SetParent(hwnd, newParent);
}

LRESULT CALLBACK About(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
{
    UNREFERENCED_PARAMETER(lParam);
    switch (message)
    {
    case WM_INITDIALOG:
        {
            int result = 0;

            //EnableThemeDialogTexture(hDlg, ETDT_USETABTEXTURE);
            
            //  initialize tab control
            TCITEM tie; 
            HWND hwndTab = GetDlgItem(hDlg, IDC_TAB1);
            tie.mask = TCIF_TEXT | TCIF_IMAGE; 
            tie.iImage = -1; 
            tie.pszText = "Date && Time"; 
            TabCtrl_InsertItem(hwndTab, 0, &tie); 
            tie.pszText = "Time Zone"; 
            TabCtrl_InsertItem(hwndTab, 1, &tie); 

            // Initialize edit and combo box
            HWND edit = GetDlgItem(hDlg, IDC_EDIT1);
            SetWindowText( edit, "2005");
            HWND  combo = GetDlgItem(hDlg, IDC_COMBO1);
            SendMessage(combo, CB_ADDSTRING, 0, (LPARAM)  "January"); 
            SendMessage(combo, CB_ADDSTRING, 0, (LPARAM)  "February"); 
            SendMessage( combo, WM_SETTEXT, 0, (LPARAM)  "March"); 

            // Find out where the clock should go 
            // by looking for the placeholder hwnd
            HWND placeholder = GetDlgItem(hDlg, IDC_CLOCK);
            RECT rectangle; 
            GetWindowRect(placeholder, &rectangle);
            int width = rectangle.right - rectangle.left;
            int height = rectangle.bottom - rectangle.top;
            POINT point;
            point.x = rectangle.left;
            point.y = rectangle.top;
            result = MapWindowPoints(NULL, hDlg, &point, 1);

            ShowWindow( placeholder, SW_HIDE);

            // demo #3
            HWND clock = ManagedCode::GetHwnd(hDlg, point.x, point.y, width, height);
			System::Windows::Interop::HwndSource^ hws = ManagedCode::HwndSource::FromHwnd(System::IntPtr(clock));
            
            return TRUE;
        }
    case WM_COMMAND:
        if (LOWORD(wParam) == IDOK || LOWORD(wParam) == IDC_BUTTON2 )
        {
            EndDialog(hDlg, LOWORD(wParam));
            return TRUE;
        }
        break;
    case WM_CLOSE:
        {
            EndDialog(hDlg, LOWORD(wParam));
            return TRUE;
        }
        break;
    }
    return FALSE;
}


