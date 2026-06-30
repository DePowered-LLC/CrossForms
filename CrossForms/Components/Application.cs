using System.Reflection;

namespace CrossForms.Components;


public static class Application {
	public static readonly Assembly Assembly = Assembly.GetEntryAssembly()!;
	public static Form? MainWindow;

	public static void Start () {
		NativeApplicationBase.Start();
	}

	public static void Quit () => NativeApplicationBase.Quit();

	public static void Run () {
		while (NativeApplicationBase.EventLoop()) ;
		NativeApplicationBase.Dispose();
	}
}
