namespace CrossForms.Native.Win32;

[Flags]
public enum WindowStyle: uint {
    /// <summary>The window has a thin-line border.</summary>
    Border = 0x800000,

    /// <summary>The window has a title bar (includes the Border style).</summary>
    Caption = 0xc00000,

    /// <summary>The window is a child window. A window with this style cannot have a menu bar. This style cannot be used with the Popup style.</summary>
    Child = 0x40000000,

    /// <summary>Excludes the area occupied by child windows when drawing occurs within the parent window. This style is used when creating the parent window.</summary>
    ClipChildren = 0x2000000,

    /// <summary>
    /// Clips child windows relative to each other; that is, when a particular child window receives a WM_PAINT message, the ClipSiblings style clips all other overlapping child windows out of the region of the child window to be updated.
    /// If ClipSiblings is not specified and child windows overlap, it is possible, when drawing within the client area of a child window, to draw within the client area of a neighboring child window.
    /// </summary>
    ClipSiblings = 0x4000000,

    /// <summary>The window is initially disabled. A disabled window cannot receive input from the user. To change this after a window has been created, use the EnableWindow function.</summary>
    Disabled = 0x8000000,

    /// <summary>The window has a border of a style typically used with dialog boxes. A window with this style cannot have a title bar.</summary>
    DialogFrame = 0x400000,

    /// <summary>
    /// The window is the first control of a group of controls. The group consists of this first control and all controls defined after it, up to the next control with the Group style.
    /// The first control in each group usually has the TabStop style so that the user can move from group to group. The user can subsequently change the keyboard focus from one control in the group to the next control in the group by using the direction keys.
    /// You can turn this style on and off to change dialog box navigation. To change this style after a window has been created, use the SetWindowLong function.
    /// </summary>
    Group = 0x20000,

    /// <summary>The window has a horizontal scroll bar.</summary>
    HorizontalScroll = 0x100000,

    /// <summary>The window is initially maximized.</summary>
    Maximize = 0x1000000,

    /// <summary>The window has a maximize button. Cannot be combined with the WS_EX_CONTEXTHELP style. The WS_SYSMENU style must also be specified.</summary>
    MaximizeBox = 0x10000,

    /// <summary>The window is initially minimized.</summary>
    Minimize = 0x20000000,

    /// <summary>The window has a minimize button. Cannot be combined with the WS_EX_CONTEXTHELP style. The WS_SYSMENU style must also be specified.</summary>
    MinimizeBox = 0x20000,

    /// <summary>The window is an overlapped window. An overlapped window has a title bar and a border.</summary>
    Overlapped = 0x0,

    /// <summary>The window is an overlapped window.</summary>
    OverlapperWindow = Overlapped | Caption | SysMenu | SizeFrame | MinimizeBox | MaximizeBox,

    /// <summary>The window is a pop-up window. This style cannot be used with the Child style.</summary>
    Popup = 0x80000000u,

    /// <summary>The window is a pop-up window. The Caption and WS_POPUPWINDOW styles must be combined to make the window menu visible.</summary>
    PopupWindow = Popup | Border | SysMenu,

    /// <summary>The window has a sizing border.</summary>
    SizeFrame = 0x40000,

    /// <summary>The window has a window menu on its title bar. The Caption style must also be specified.</summary>
    SysMenu = 0x80000,

    /// <summary>
    /// The window is a control that can receive the keyboard focus when the user presses the TAB key.
    /// Pressing the TAB key changes the keyboard focus to the next control with the TabStop style.
    /// You can turn this style on and off to change dialog box navigation. To change this style after a window has been created, use the SetWindowLong function.
    /// For user-created windows and modeless dialogs to work with tab stops, alter the message loop to call the IsDialogMessage function.
    /// </summary>
    TabStop = 0x10000,

    /// <summary>The window is initially visible. This style can be turned on and off by using the ShowWindow or SetWindowPos function.</summary>
    Visible = 0x10000000,

    /// <summary>The window has a vertical scroll bar.</summary>
    VerticalScroll = 0x200000
}

[Flags]
public enum WindowStyleEx: uint {
    /// <summary>Specifies a window that accepts drag-drop files.</summary>
    AcceptFiles = 0x00000010,

    /// <summary>Forces a top-level window onto the taskbar when the window is visible.</summary>
    AppWindow = 0x00040000,

    /// <summary>Specifies a window that has a border with a sunken edge.</summary>
    ClientEdge = 0x00000200,

    /// <summary>
    /// Specifies a window that paints all descendants in bottom-to-top painting order using double-buffering.
    /// This cannot be used if the window has a class style of either CS_OWNDC or CS_CLASSDC. This style is not supported in Windows 2000.
    /// </summary>
    /// <remarks>
    /// With Composited set, all descendants of a window get bottom-to-top painting order using double-buffering.
    /// Bottom-to-top painting order allows a descendent window to have translucency (alpha) and transparency (color-key) effects,
    /// but only if the descendent window also has the Transparent bit set.
    /// Double-buffering allows the window and its descendents to be painted without flicker.
    /// </remarks>
    Composited = RaisedEdge,

    /// <summary>
    /// Specifies a window that includes a question mark in the title bar. When the user clicks the question mark,
    /// the cursor changes to a question mark with a pointer. If the user then clicks a child window, the child receives a WM_HELP message.
    /// The child window should pass the message to the parent window procedure, which should call the WinHelp function using the HELP_WM_HELP command.
    /// The Help application displays a pop-up window that typically contains help for the child window.
    /// ContextHelp cannot be used with the MaximizeBox or MinimizeBox styles.
    /// </summary>
    ContextHelp = 0x00000400,

    /// <summary>
    /// Specifies a window which contains child windows that should take part in dialog box navigation.
    /// If this style is specified, the dialog manager recurses into children of this window when performing navigation operations
    /// such as handling the TAB key, an arrow key, or a keyboard mnemonic.
    /// </summary>
    ControlParent = 0x00010000,

    /// <summary>Specifies a window that has a double border.</summary>
    DialogModalFrame = 0x00000001,

    /// <summary>
    /// Specifies a window that is a layered window.
    /// This cannot be used for child windows or if the window has a class style of either CS_OWNDC or CS_CLASSDC.
    /// </summary>
    Layered = 0x00080000,

    /// <summary>
    /// Specifies a window with the horizontal origin on the right edge. Increasing horizontal values advance to the left.
    /// The shell language must support reading-order alignment for this to take effect.
    /// </summary>
    LayoutRTL = 0x00400000,

    /// <summary>Specifies a window that has generic left-aligned properties. This is the default.</summary>
    Left = 0x00000000,

    /// <summary>
    /// Specifies a window with the vertical scroll bar (if present) to the left of the client area.
    /// The shell language must support reading-order alignment for this to take effect.
    /// </summary>
    LeftScrollBar = 0x00004000,

    /// <summary>
    /// Specifies a window that displays text using left-to-right reading-order properties. This is the default.
    /// </summary>
    LTR_Reading = 0x00000000,

    /// <summary>
    /// Specifies a multiple-document interface (MDI) child window.
    /// </summary>
    MDI_Child = 0x00000040,

    /// <summary>
    /// Specifies a top-level window created with this style does not become the foreground window when the user clicks it.
    /// The system does not bring this window to the foreground when the user minimizes or closes the foreground window.
    /// The window does not appear on the taskbar by default. To force the window to appear on the taskbar, use the AppWindow style.
    /// To activate the window, use the SetActiveWindow or SetForegroundWindow function.
    /// </summary>
    NoActivate = 0x08000000,

    /// <summary>
    /// Specifies a window which does not pass its window layout to its child windows.
    /// </summary>
    NoInheritLayout = 0x00100000,

    /// <summary>
    /// Specifies that a child window created with this style does not send the WM_PARENTNOTIFY message to its parent window when it is created or destroyed.
    /// </summary>
    NoParentNotify = 0x00000004,

    /// <summary>
    /// The window does not render to a redirection surface.
    /// This is for windows that do not have visible content or that use mechanisms other than surfaces to provide their visual.
    /// </summary>
    NoRedirectionBitmap = 0x00200000,

    /// <summary>Specifies an overlapped window.</summary>
    Overlapped = RaisedEdge | ClientEdge,

    /// <summary>Specifies a palette window, which is a modeless dialog box that presents an array of commands.</summary>
    Palette = RaisedEdge | Toolbar | Topmost,

    /// <summary>
    /// Specifies a window that has generic "right-aligned" properties. This depends on the window class.
    /// The shell language must support reading-order alignment for this to take effect.
    /// Using the Rigth style has the same effect as using the SS_RIGHT (static), ES_RIGHT (edit), and BS_RIGHT/BS_RIGHTBUTTON (button) control styles.
    /// </summary>
    Rigth = 0x00001000,

    /// <summary>Specifies a window with the vertical scroll bar (if present) to the right of the client area. This is the default.</summary>
    RightScrollBar = 0x00000000,

    /// <summary>
    /// Specifies a window that displays text using right-to-left reading-order properties.
    /// The shell language must support reading-order alignment for this to take effect.
    /// </summary>
    RTL_Reading = 0x00002000,

    /// <summary>Specifies a window with a three-dimensional border style intended to be used for items that do not accept user input.</summary>
    StaticEdge = 0x00020000,

    /// <summary>
    /// Specifies a window that is intended to be used as a floating toolbar.
    /// A tool window has a title bar that is shorter than a normal title bar, and the window title is drawn using a smaller font.
    /// A tool window does not appear in the taskbar or in the dialog that appears when the user presses ALT+TAB.
    /// If a tool window has a system menu, its icon is not displayed on the title bar.
    /// However, you can display the system menu by right-clicking or by typing ALT+SPACE.
    /// </summary>
    Toolbar = 0x00000080,

    /// <summary>
    /// Specifies a window that should be placed above all non-topmost windows and should stay above them, even when the window is deactivated.
    /// To add or remove this style, use the SetWindowPos function.
    /// </summary>
    Topmost = 0x00000008,

    /// <summary>
    /// Specifies a window that should not be painted until siblings beneath the window (that were created by the same thread) have been painted.
    /// The window appears transparent because the bits of underlying sibling windows have already been painted.
    /// To achieve transparency without these restrictions, use the SetWindowRgn function.
    /// </summary>
    Transparent = 0x00000020,

    /// <summary>Specifies a window that has a border with a raised edge.</summary>
    RaisedEdge = 0x00000100
}

[Flags]
public enum ShowWindowCommand: int {
    /// <summary>Hides the window and activates another window.</summary>
    Hide = 0,

    /// <summary>Activates and displays a window. If the window is minimized or maximized, the system restores it to its original size and position. An application should specify this flag when displaying the window for the first time.</summary>
    ShowNormal = 1,

    /// <summary>Activates the window and displays it as a minimized window.</summary>
    ShowMinimized = 2,

    /// <summary>Activates the window and displays it as a maximized window.</summary>
    ShowMaximized = 3,

    /// <summary>Displays a window in its most recent size and position. This value is similar to ShowNormal, except that the window is not activated.</summary>
    ShowNormalNoActive = 4,

    /// <summary>Activates the window and displays it in its current size and position.</summary>
    Show = 5,

    /// <summary>Minimizes the specified window and activates the next top-level window in the Z order.</summary>
    Minimize = 6,

    /// <summary>Displays the window as a minimized window. This value is similar to ShowMinimized, except the window is not activated.</summary>
    ShowMinimizedNoActive = 7,

    /// <summary>Displays the window in its current size and position. This value is similar to Show, except that the window is not activated.</summary>
    ShowNoActive = 8,

    /// <summary>Activates and displays the window. If the window is minimized or maximized, the system restores it to its original size and position. An application should specify this flag when restoring a minimized window.</summary>
    Restore = 9,

    /// <summary>Sets the show state based on the SW_ value specified in the STARTUPINFO structure passed to the CreateProcess function by the program that started the application.</summary>
    ShowDefault = 10,

    /// <summary>Minimizes a window, even if the thread that owns the window is not responding. This flag should only be used when minimizing windows from a different thread.</summary>
    ForceMinimize = 11
}

[Flags]
public enum GWL: int {
    WNDPROC = -4,
    HINSTANCE = -6,
    HWNDPARENT = -8,
    STYLE = -16,
    EXSTYLE = -20,
    USERDATA = -21,
    ID = -12
}
