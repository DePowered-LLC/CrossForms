namespace CrossForms.Components;


public class Application {
	public static Form? mainWindow;

	public static void Start () {
		NativeApplication.Start();
	}

	public static void Run () {
		while (NativeApplication.EventLoop()) ;

		NativeApplication.Dispose();
		Console.WriteLine("Event loop exited");
	}
}
