namespace CrossForms.Native;

public abstract class NativeException: Exception {
	override public string Message => _message;

	private readonly string _message;
    protected abstract string PrepareMessage (string message);

	public NativeException (string message) {
		_message = PrepareMessage(message);
	}
}
