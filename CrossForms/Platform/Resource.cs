using CrossForms.Components;

namespace CrossForms.Platform;


public readonly struct Resource {
	public string Id { get; }

	private Resource (string id) {
		Id = id;
	}

	public Stream Stream => Application.Assembly.GetManifestResourceStream(Id)!;

	public byte[] ToBuffer () {
		using var ms = new MemoryStream();
		Stream.CopyTo(ms);
		return ms.ToArray();
	}

	public static string[] GetIds () {
		return Application.Assembly.GetManifestResourceNames();
	}
	
	public static Resource[] GetAll () {
		return GetIds().Select(id => new Resource(id)).ToArray();
	}

	public static Resource? Get (string id) {
		if (GetIds().Contains(id)) {
			return new Resource(id);
		}
		
		return null;
	}
}
