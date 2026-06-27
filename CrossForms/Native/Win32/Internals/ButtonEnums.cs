namespace CrossForms.Native.Win32;

[Flags]
public enum ButtonStyle: uint {
	/// <summary>Creates a push button that posts a WM_COMMAND message to the owner window when the user selects the button.</summary>
	Push = 0x00000000,

	/// <summary>Creates a push button that behaves like a BS_PUSHBUTTON style button, but has a distinct appearance. If the button is in a dialog box, the user can select the button by pressing the ENTER key, even when the button does not have the input focus. This style is useful for enabling the user to quickly select the most likely (default) option.</summary>
	DefPush = 0x00000001,

	/// <summary>Creates a small, empty check box with text. By default, the text is displayed to the right of the check box. To display the text to the left of the check box, combine this flag with the BS_LEFTTEXT style (or with the equivalent BS_RIGHTBUTTON style).</summary>
	CheckBox = 0x00000002,

	/// <summary>Creates a button that is the same as a check box, except that the check state automatically toggles between checked and cleared each time the user selects the check box.</summary>
	AutoCheckBox = 0x00000003,

	/// <summary>Creates a small circle with text. By default, the text is displayed to the right of the circle. To display the text to the left of the circle, combine this flag with the BS_LEFTTEXT style (or with the equivalent BS_RIGHTBUTTON style). Use radio buttons for groups of related, but mutually exclusive choices.</summary>
	Radio = 0x00000004,

	/// <summary>Creates a button that is the same as a check box, except that the box can be grayed as well as checked or cleared. Use the grayed state to show that the state of the check box is not determined.</summary>
	CheckBox3 = 0x00000005,

	/// <summary>Creates a button that is the same as a three-state check box, except that the box changes its state when the user selects it. The state cycles through checked, indeterminate, and cleared.</summary>
	AutoCheckBox3 = 0x00000006,

	/// <summary>Creates a rectangle in which other controls can be grouped. Any text associated with this style is displayed in the rectangle's upper left corner.</summary>
	GroupBox = 0x00000007,

	/// <summary>Obsolete, but provided for compatibility with 16-bit versions of Windows. Applications should use BS_OWNERDRAW instead.</summary>
	UserButton = 0x00000008,

	/// <summary>Creates a button that is the same as a radio button, except that when the user selects it, the system automatically sets the button's check state to checked and automatically sets the check state for all other buttons in the same group to cleared.</summary>
	AutoRadioButton = 0x00000009,

	/// <summary>Creates an owner-drawn button. The owner window receives a WM_DRAWITEM message when a visual aspect of the button has changed. Do not combine the BS_OWNERDRAW style with any other button styles.</summary>
	OwnerDraw = 0x0000000B,

	/// <summary>Places text on the left side of the radio button or check box when combined with a radio button or check box style. Same as the BS_RIGHTBUTTON style.</summary>
	LeftText = 0x00000020,

	/// <summary>Specifies that the button displays text.</summary>
	Text = 0x00000000,

	/// <summary>Specifies that the button displays an icon. See the Remarks section for its interaction with BS_BITMAP.</summary>
	Icon = 0x00000040,

	/// <summary>Specifies that the button displays a bitmap. See the Remarks section for its interaction with BS_ICON.</summary>
	Bitmap = 0x00000080,

	/// <summary>Left-justifies the text in the button rectangle. However, if the button is a check box or radio button that does not have the BS_RIGHTBUTTON style, the text is left justified on the right side of the check box or radio button.</summary>
	Left = 0x00000100,

	/// <summary>Right-justifies text in the button rectangle. However, if the button is a check box or radio button that does not have the BS_RIGHTBUTTON style, the text is right justified on the right side of the check box or radio button.</summary>
	Right = 0x00000200,

	/// <summary>Centers text horizontally in the button rectangle.</summary>
	Center = 0x00000300,

	/// <summary>Places text at the top of the button rectangle.</summary>
	Top = 0x00000400,

	/// <summary>Places text at the bottom of the button rectangle.</summary>
	Bottom = 0x00000800,

	/// <summary>Places text in the middle (vertically) of the button rectangle.</summary>
	VertCenter = 0x00000C00,

	/// <summary>Makes a button (such as a check box, three-state check box, or radio button) look and act like a push button. The button looks raised when it isn't pushed or checked, and sunken when it is pushed or checked.</summary>
	PushLike = 0x00001000,

	/// <summary>Wraps the button text to multiple lines if the text string is too long to fit on a single line in the button rectangle.</summary>
	Multiline = 0x00002000,

	/// <summary>
	/// Enables a button to send BN_KILLFOCUS and BN_SETFOCUS notification codes to its parent window.
	/// Note that buttons send the BN_CLICKED notification code regardless of whether it has this style. To get BN_DBLCLK notification codes, the button must have the BS_RADIOBUTTON or BS_OWNERDRAW style.
	/// </summary>
	Notify = 0x00004000,

	/// <summary>Specifies that the button is two-dimensional; it does not use the default shading to create a 3-D image.</summary>
	Flat = 0x00008000,

	/// <summary>Positions a radio button's circle or a check box's square on the right side of the button rectangle. Same as the BS_LEFTTEXT style.</summary>
	RightButton = LeftText
}

[Flags]
public enum ButtonCommand: ushort {
	Clicked = 0,
	Paint = 1,
	Highlight = 2,
	UnHighlight = 3,
	Disable = 4,
	DoubleClicked = 5,
	Pushed = Highlight,
	UnPushed = UnHighlight,
	Setfocus = 6,
	Killfocus = 7
}

[Flags]
public enum ButtonMessage: ushort {
	GetCheck = 0x00F0,
	SetCheck = 0x00F1,
	GetState = 0x00F2,
	SetState = 0x00F3,
	SetStyle = 0x00F4,
	Click = 0x00F5,
	GetImage = 0x00F6,
	SetImage = 0x00F7,
	SetDontClick = 0x00F8,
	UnChecked = 0x0000,
	Checked = 0x0001,
	Indeterminate = 0x0002,
	Pushed = 0x0004,
	Focus = 0x0008
}
