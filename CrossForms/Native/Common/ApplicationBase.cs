namespace CrossForms.Native.Common;


public abstract class ApplicationBase {
	internal static bool HasActiveTray { get; set; }
	internal static bool HasVisibleForm { get; set; }

	public static void Start () {
		throw new NotImplementedException();
	}

	public static bool EventLoop () {
		throw new NotImplementedException();
	}

	public static void Dispose () {
		throw new NotImplementedException();
	}

	public static void Quit () {
		throw new NotImplementedException();
	}
}
