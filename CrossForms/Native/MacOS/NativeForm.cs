using CrossForms.Native.Common;
using CrossForms.Native.MacOS.Internals;

namespace CrossForms.Native.MacOS;

public class NativeForm : IForm {
    public string id { get; set; } = "";
    public string title { get; set; } = "";
    public ushort width { get; set; }
    public ushort height { get; set; }

    private NsWindow? _window;
    private readonly List<NativeButton> _children = [];
    private NativeButton? _initialControl;

    public void SetInitialControl (IButton button) => _initialControl = (NativeButton) button;

    public void Append (NativeButton button) {
        if (_children.Count > 0 && _children[^1]._nextControl == null)
            _children[^1]._nextControl = button;
        _children.Add(button);
        if (_window != null) AttachButton(button);
    }

    public void Show () {
        var w = width > 0 ? (double) width : 800;
        var h = height > 0 ? (double) height : 600;

        _window = new NsWindow(
            new CGRect(0, 0, w, h),
            NsWindow.StyleMask.Titled | NsWindow.StyleMask.Closable | NsWindow.StyleMask.Resizable,
            2
        );
        _window.Title = title;

        _window.OnClose(() => {
            if (!NsApplication.current.isRunning) return;
            NsApplication.current.Terminate();
        });

        foreach (var child in _children) AttachButton(child);

        foreach (var btn in _children)
            if (btn._nextControl?._nsButton != null)
                btn._nsButton!.SetNextKeyView(btn._nextControl._nsButton);

        var first = _initialControl ?? (_children.Count > 0 ? _children[0] : null);
        if (first?._nsButton != null)
            _window.SetInitialFirstResponder(first._nsButton);
    }

    private void AttachButton (NativeButton button) {
        var nsBtn = button.CreateNsButton();
        button._nsButton = nsBtn;
        _window!.Append(nsBtn);
        if (button._nextControl?._nsButton != null)
            nsBtn.SetNextKeyView(button._nextControl._nsButton);

        var contentView = _window.ContentView;
        var bw = button.width > 0 ? (double) button.width : 120;
        var bh = button.height > 0 ? (double) button.height : 22;

        nsBtn.LeadingAnchor.ConstraintToAnchor(contentView.LeadingAnchor, button.x).Active = true;
        nsBtn.TopAnchor.ConstraintToAnchor(contentView.TopAnchor, button.y).Active = true;
        nsBtn.WidthAnchor.ConstraintToConstant(bw).Active = true;
        nsBtn.HeightAnchor.ConstraintToConstant(bh).Active = true;
    }
}
