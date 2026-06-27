using CrossForms.Components;
using CrossForms.Native;

try {
	Application.Start();
	Application.MainWindow = new Form("MainWindow", "Demo Window") {
		Width = 440,
		Height = 420
	};

	var label1 = new Label("Type something:") { X = 10, Y = 10, Width = 200 };
	Application.MainWindow.Append(label1);

	var input1 = new TextBox { X = 10, Y = 30, Width = 200, Height = 22 };
	Application.MainWindow.Append(input1);

	var btnRead = new Button("Read") { X = 220, Y = 30, Width = 100, Height = 22 };
	btnRead.OnClick += (_, _) => Console.WriteLine($"Input: {input1.Text}");
	Application.MainWindow.Append(btnRead);

	var checkEnable = new CheckBox("Input enabled") {
		X = 10, Y = 58, Width = 200, Height = 22, Checked = true
	};
	checkEnable.OnChange += (_, _) => { input1.Enabled = checkEnable.Checked; };
	Application.MainWindow.Append(checkEnable);

	var progressLabel = new Label("Progress (controlled by RadioGroup):") {
		X = 10, Y = 96, Width = 300
	};
	Application.MainWindow.Append(progressLabel);

	var progress = new ProgressBar {
		X = 10, Y = 116, Width = 400, Height = 18,
		Min = 0, Max = 100, Value = 0
	};
	Application.MainWindow.Append(progress);

	var rb1 = new RadioButton("0%") { X = 10, Y = 142, Width = 90 };
	var rb2 = new RadioButton("25%") { X = 110, Y = 142, Width = 90 };
	var rb3 = new RadioButton("50%") { X = 210, Y = 142, Width = 90 };
	var rb4 = new RadioButton("100%") { X = 310, Y = 142, Width = 90 };

	double[] radioValues = [0, 25, 50, 100];
	var radioGroup = new RadioGroup(rb1, rb2, rb3, rb4);
	radioGroup.OnChange += (_, e) => { progress.Value = radioValues[e.selectedIndex]; };
	Application.MainWindow.Append(radioGroup);

	var selectLabel = new Label("Or pick a preset:") { X = 10, Y = 174, Width = 180 };
	Application.MainWindow.Append(selectLabel);

	(string Name, double Value)[] presets = [
		("Not started", 0),
		("Quarter", 25),
		("Half", 50),
		("Three quarters", 75),
		("Done", 100)
	];

	var presetSelect = new Select<(string Name, double Value)>(p => p.Name, presets) {
		X = 10, Y = 194, Width = 200
	};
	presetSelect.OnChange += (_, _) => { progress.Value = presetSelect.ActiveItem.Value; };
	Application.MainWindow.Append(presetSelect);

	var spinnerLabel = new Label("Indeterminate:") { X = 10, Y = 232, Width = 140 };
	Application.MainWindow.Append(spinnerLabel);

	var spinner = new ProgressBar {
		X = 10, Y = 252, Width = 400, Height = 18,
		Indeterminate = true
	};
	Application.MainWindow.Append(spinner);

	var checkSpinner = new CheckBox("Show indeterminate") {
		X = 10, Y = 280, Width = 220, Height = 22, Checked = true
	};
	checkSpinner.OnChange += (_, _) => { spinner.Indeterminate = checkSpinner.Checked; };
	Application.MainWindow.Append(checkSpinner);

	Application.MainWindow.Show();
	Application.Run();
} catch (NativeException err) {
	Console.Error.WriteLine(err.ToString());
}
