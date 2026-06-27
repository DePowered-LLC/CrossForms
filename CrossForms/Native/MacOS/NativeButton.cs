using CrossForms.Native.Common;
using CrossForms.Native.MacOS.Internals;

namespace CrossForms.Native.MacOS;

public class NativeButton : IButton {
    public string text { get; set; } = "";
    public int x { get; set; }
    public int y { get; set; }
    public ushort width { get; set; } = 120;
    public ushort height { get; set; } = 22;

    private EventHandler<ClickEvent>? _onClick;
    public EventHandler<ClickEvent> OnClick {
        get => _onClick!;
        set => _onClick = value;
    }

    internal NsButton CreateNsButton () {
        var btn = new NsButton(text);
        btn.OnClick(() => {
            var clickPos = new ClickEvent();
            var ev = NsApplication.current.CurrentEvent;
            if (ev != null) {
                var loc = ev.LocationInWindow;
                var frame = btn.Window?.ContentView.Frame;
                if (frame.HasValue) {
                    clickPos.x = (int) loc.x;
                    clickPos.y = (int) (frame.Value.size.height - loc.y);
                }
            }
            _onClick?.Invoke(this, clickPos);
        });
        return btn;
    }
}
