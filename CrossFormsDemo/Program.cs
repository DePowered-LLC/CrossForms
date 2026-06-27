using CrossForms.Components;
using CrossForms.Native;

try {
	Application.Start();
	Application.MainWindow = new Form("MainWindow", "Тестовое окно");

	var testBtn1 = new Button("Hello!") {
		X = 10,
		Y = 10,
		Width = 200,
		Height = 22
	};
	testBtn1.OnClick += (_, e) => {
		Console.WriteLine($"Button 1 click at ({e.x}; {e.y})");
	};
	Application.MainWindow.Append(testBtn1);
	
	var label1 = new Label("Пример текста\n123-321") {
		X = 10,
		Y = 36,
		Height = 40
	};
	Application.MainWindow.Append(label1);

	var input1 = new TextBox("default") {
		X = 10,
		Y = 100,
		Width = 200,
		Height = 22
	};
	Application.MainWindow.Append(input1);

	var testBtn2 = new Button("Read input below") {
		X = 10,
		Y = 72,
		Width = 200,
		Height = 22
	};
	testBtn2.OnClick += (_, _) => {
		Console.WriteLine($"Input value: {input1.Text}");
	};
	testBtn2.SetNextControl(testBtn1);
	Application.MainWindow.Append(testBtn2);
	Application.MainWindow.SetInitialControl(testBtn2);
	
	var checkBox1 = new CheckBox("Enable input above") {
		X = 10,
		Y = 130,
		Width = 200,
		Height = 22,
		Checked = true
	};
	checkBox1.OnChange += (_, _) => {
		input1.Enabled = checkBox1.Checked;
	};
	Application.MainWindow.Append(checkBox1);

	var rb1 = new RadioButton("Option A") { X = 10, Y = 162, Width = 200 };
	var rb2 = new RadioButton("Option B") { X = 10, Y = 186, Width = 200 };
	var rb3 = new RadioButton("Option C") { X = 10, Y = 210, Width = 200 };
	
	var radioGroup = new RadioGroup(rb1, rb2, rb3) { SelectedIndex = 0 };
	radioGroup.OnChange += (_, e) => {
		Console.WriteLine($"Radio selected: {e.selectedIndex}");
	};
	Application.MainWindow.Append(radioGroup);

	var select = new Select<string>("Option A", "Option B", "Option C") {
		X = 10, 
		Y = 242, 
		Width = 200
	};
	select.OnChange += (_, _) => {
		Console.WriteLine($"Select: {select.ActiveItem}");
	};
	Application.MainWindow.Append(select);

	Application.MainWindow.Show();
	Application.Run();
} catch (NativeException err) {
	Console.Error.WriteLine(err.ToString());
}
