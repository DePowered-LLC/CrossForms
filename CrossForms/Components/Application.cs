namespace CrossForms.Components;


public class Application {
	public static Form? MainWindow;

	public static void Start () {
		NativeApplicationBase.Start();
	}

	public static void Run () {
		while (NativeApplicationBase.EventLoop()) ;

		NativeApplicationBase.Dispose();
		Console.WriteLine("Event loop exited");
	}
}
