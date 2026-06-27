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

    public void Append (NativeButton button) {
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
    }

    private void AttachButton (NativeButton button) {
        var nsBtn = button.CreateNsButton();
        _window!.Append(nsBtn);

        var contentView = _window.ContentView;
        var bw = button.width > 0 ? (double) button.width : 120;
        var bh = button.height > 0 ? (double) button.height : 22;

        nsBtn.LeadingAnchor.ConstraintToAnchor(contentView.LeadingAnchor, button.x).Active = true;
        nsBtn.TopAnchor.ConstraintToAnchor(contentView.TopAnchor, button.y).Active = true;
        nsBtn.WidthAnchor.ConstraintToConstant(bw).Active = true;
        nsBtn.HeightAnchor.ConstraintToConstant(bh).Active = true;
    }
}
