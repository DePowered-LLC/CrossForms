namespace CrossForms;

class Application {
	internal static Form? mainWindow;

	public static void Run () {
		while (NativeApplication.EventLoop());

		NativeApplication.Dispose();
		Console.WriteLine("Event loop exited");
	}
}
