namespace CrossForms.Native;


public abstract class NativeException: Exception {
	private readonly string _message;

	public NativeException (string message) {
		_message = PrepareMessage(message);
	}

	public override string Message => _message;
	protected abstract string PrepareMessage (string message);
}
