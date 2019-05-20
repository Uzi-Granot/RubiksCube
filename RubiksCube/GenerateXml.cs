/////////////////////////////////////////////////////////////////////
// This class was used during the development to generate
// tha MainWindows.xml file.
// This source code is kept here just in case of a major
// change to the program.
/////////////////////////////////////////////////////////////////////

/*
using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace UziRubiksCube
{
public static class GenerateXml
	{
	private const double WindowWidth = 1100;
	private const double WindowHeight = 680;
	private const double CubeWidth = 800;
	private const double CubeHeight = 540;

	private const double ButtonsPanelWidth = 280;
	private const double InfoPanelHeight = 100;
	private const double LabelWidth = 120;
	public  const double LabelHeight = 26;
	private const double RotateButtonWidth = 60;
	private const double RotateButtonHeight = 34;
	private const double ButtonHeight = 34;
	private const double ColorButtonWidth = ButtonHeight;
	private const double ControlButtonWidth = 120;
	private const double MarginX = 4;
	private const double MarginY = 4;

	private static StreamWriter File;

	public static void Create()
		{
		// open existing or create new trace file
		File = new StreamWriter(new FileStream("ControlToXml.txt", FileMode.Create, FileAccess.Write), Encoding.UTF8);

		// layout constants
		double PosX;
		double PosY;

		double InfoAreaWidth = CubeWidth;
		double CubePosInfoWidth1 = 60;
		double CubePosInfoWidth2 = 50;
		double HeaderLabelWidth = (InfoAreaWidth - 2 * MarginX) / 3;
		double StepsLabelWidth = (InfoAreaWidth - 2 * CubePosInfoWidth1 - 3 * CubePosInfoWidth2 - 6 * MarginX) / 2;
		double ColorButtonsPosX = 0.5 * (ButtonsPanelWidth - 6 * ColorButtonWidth - 5 * MarginX);

		// Heading Lable
		PosX = ButtonsPanelWidth;
		PosY = 2 * MarginY;
		XmlControl(true, "Heading", PosX, PosY, InfoAreaWidth, LabelHeight, Brushes.LightYellow, HorizontalAlignment.Center, null,
			"Rubik's Cube for beginners --- Copyright \u00a9 2017 Granotech Limited --- All rights reserved --- Author: Uzi Granot", null);

		// information label 1
		PosY += LabelHeight + MarginY;
		XmlControl(true, "SolveLabel1", PosX, PosY, HeaderLabelWidth, LabelHeight, Brushes.LightYellow, HorizontalAlignment.Left, null, null, null);

		// information label 2
		PosX += HeaderLabelWidth + MarginX;
		XmlControl(true, "SolveLabel2", PosX, PosY, HeaderLabelWidth, LabelHeight, Brushes.LightYellow, HorizontalAlignment.Left, null, null, null);

		// information label 3
		PosX += HeaderLabelWidth + MarginX;
		XmlControl(true, "SolveLabel3", PosX, PosY, HeaderLabelWidth, LabelHeight, Brushes.LightYellow, HorizontalAlignment.Left, null, null, null);

		// information label 4
		PosX = ButtonsPanelWidth;
		PosY += LabelHeight + MarginY;
		XmlControl(true, "SolveLabel4", PosX, PosY, CubePosInfoWidth2, LabelHeight, Brushes.LightYellow, HorizontalAlignment.Center, null, null, null);

		// information label 5
		PosX += CubePosInfoWidth2 + MarginX;
		XmlControl(true, "SolveLabel5", PosX, PosY, CubePosInfoWidth1, LabelHeight, Brushes.LightYellow, HorizontalAlignment.Center, null, null, null);

		// information label 6
		PosX += CubePosInfoWidth1 + MarginX;
		XmlControl(true, "SolveLabel6", PosX, PosY, CubePosInfoWidth2, LabelHeight, Brushes.LightYellow, HorizontalAlignment.Center, null, null, null);

		// information label 7
		PosX += CubePosInfoWidth2 + MarginX;
		XmlControl(true, "SolveLabel7", PosX, PosY, CubePosInfoWidth1, LabelHeight, Brushes.LightYellow, HorizontalAlignment.Center, null, null, null);

		// information label 8
		PosX += CubePosInfoWidth1 + MarginX;
		XmlControl(true, "SolveLabel8", PosX, PosY, StepsLabelWidth, LabelHeight, Brushes.LightYellow, HorizontalAlignment.Left, null, null, null);

		// information label 9
		PosX += StepsLabelWidth + MarginX;
		XmlControl(true, "SolveLabel9", PosX, PosY, CubePosInfoWidth2, LabelHeight, Brushes.LightYellow, HorizontalAlignment.Center, null, null, null);

		// information label 10
		PosX += CubePosInfoWidth2 + MarginX;
		XmlControl(true, "SolveLabel10", PosX, PosY, StepsLabelWidth, LabelHeight, Brushes.LightYellow, HorizontalAlignment.Left, null, null, null);

		// sides rotations header
		PosY = 2 * MarginY;
		PosX = 0.5 * (ButtonsPanelWidth - LabelWidth);
		XmlControl(true, null, PosX, PosY, LabelWidth, LabelHeight, Brushes.LightYellow, HorizontalAlignment.Center, null, "Sides Rotations", null);
		PosY += LabelHeight + MarginY;

		// rotation color buttons
		double Left = 0.5 * (ButtonsPanelWidth - 3 * RotateButtonWidth - 2 * MarginX);
		for(int Side = 0; Side < Cube.FaceColors; Side++)
			{
			PosX = Left;
			for(int Move = 0; Move < Cube.RotMovesPerColor; Move++)
				{
				XmlControl(false, "RotationButton" + (Cube.RotMovesPerColor * Side + Move).ToString(),
					PosX, PosY, RotateButtonWidth, RotateButtonHeight, Cube3D.FaceColor[Side], HorizontalAlignment.Center,
					(Cube.RotMovesPerColor * Side + Move).ToString(), Cube3D.RotMoveName[Move], "RotationButtonClick");
				PosX += RotateButtonWidth + MarginX;
				}
			PosY += RotateButtonHeight + MarginY;
			}
		PosY += MarginY;

		// top face header
		PosX = 0.5 * (ButtonsPanelWidth - LabelWidth);
		XmlControl(true, null, PosX, PosY, LabelWidth, LabelHeight, Brushes.LightYellow, HorizontalAlignment.Center, null, "Up Face", null);
		PosY += LabelHeight + MarginY;

		// top face color buttons
		PosX = ColorButtonsPosX;
		for(int Side = 0; Side < Cube.FaceColors; Side++)
			{
			XmlControl(false, "UpFaceButton" + Side.ToString(),
				PosX, PosY, ColorButtonWidth, RotateButtonHeight, Cube3D.FaceColor[Side], HorizontalAlignment.Center,
				Side.ToString(), null, "UpFaceButtonClick");
			PosX += ColorButtonWidth + MarginX;
			}
		PosY += ButtonHeight + 2 * MarginY;

		// Front face header
		PosX = 0.5 * (ButtonsPanelWidth - LabelWidth);
		XmlControl(true, null, PosX, PosY, LabelWidth, LabelHeight, Brushes.LightYellow, HorizontalAlignment.Center, null, "Front Face", null);
		PosY += LabelHeight + MarginY;

		// front face color buttons
		PosX = ColorButtonsPosX;
		for(int Side = 0; Side < Cube.FaceColors; Side++)
			{
			XmlControl(false, "FrontFaceButton" + Side.ToString(),
				PosX, PosY, ColorButtonWidth, RotateButtonHeight, Cube3D.FaceColor[Side], HorizontalAlignment.Center,
				Side.ToString(), null, "FrontFaceButtonClick");
			PosX += ColorButtonWidth + MarginX;
			}
		PosY += ButtonHeight + 2 * MarginY;

		// user cube selected color
		PosX = 0.5 * (ButtonsPanelWidth - 2 * LabelWidth);
		XmlControl(true, "SetColorHeading", PosX, PosY, 2 * LabelWidth, LabelHeight, Cube3D.FaceColor[Cube.WhiteFace],
			HorizontalAlignment.Center, null, "User Cube Selected Color", null);
		PosY += LabelHeight + MarginY;

		// user cube color buttons
		PosX = ColorButtonsPosX;
		for(int Side = 0; Side < Cube.FaceColors; Side++)
			{
			XmlControl(false, "SetColorButton" + Side.ToString(),
				PosX, PosY, ColorButtonWidth, RotateButtonHeight, Cube3D.FaceColor[Side], HorizontalAlignment.Center,
				Side.ToString(), null, "SetColorButtonClick");
			PosX += ColorButtonWidth + MarginX;
			}
		PosY += ButtonHeight + 2 * MarginY;

		// Solve one step
		double ControlButtonPosX = 0.5 * (ButtonsPanelWidth - 2 * ControlButtonWidth - 2 * MarginX);
		PosX = ControlButtonPosX;
		XmlControl(false, "SolveStepButton",
			PosX, PosY, ControlButtonWidth, ButtonHeight, SystemColors.ControlBrush, HorizontalAlignment.Center,
			null, "Solve Step", "SolveStepButtonClick");

		// auto solve
		PosX += ControlButtonWidth + 2 * MarginX;
		XmlControl(false, "AutoSolveButton",
			PosX, PosY, ControlButtonWidth, ButtonHeight, SystemColors.ControlBrush, HorizontalAlignment.Center,
			null, "Auto Solve", "AutoSolveButtonClick");
		PosY += ButtonHeight + MarginY;

		// save solution
		PosX = ControlButtonPosX;
		XmlControl(false, "SaveSolutionButton",
			PosX, PosY, ControlButtonWidth, ButtonHeight, SystemColors.ControlBrush, HorizontalAlignment.Center,
			null, "Save Solution", "SaveSolutionButtonClick");

		// load solution
		PosX += ControlButtonWidth + 2 * MarginX;
		XmlControl(false, "LoadSolutionButton",
			PosX, PosY, ControlButtonWidth, ButtonHeight, SystemColors.ControlBrush, HorizontalAlignment.Center,
			null, "Load Solution", "LoadSolutionButtonClick");
		PosY += ButtonHeight + MarginY;

		// reset cube to solved state
		PosX = ControlButtonPosX;
		XmlControl(false, "ResetButton",
			PosX, PosY, ControlButtonWidth, ButtonHeight, SystemColors.ControlBrush, HorizontalAlignment.Center,
			null, "Reset", "ResetButtonClick");

		// randomly scramble cube
		PosX += ControlButtonWidth + 2 * MarginX;
		XmlControl(false, "ScrambleButton",
			PosX, PosY, ControlButtonWidth, ButtonHeight, SystemColors.ControlBrush, HorizontalAlignment.Center,
			null, "Scramble", "ScrambleButtonClick");
		PosY += ButtonHeight + MarginY;

		// undo single rotation
		PosX = ControlButtonPosX;
		XmlControl(false, "UndoButton",
			PosX, PosY, ControlButtonWidth, ButtonHeight, SystemColors.ControlBrush, HorizontalAlignment.Center,
			null, "Undo", "UndoButtonClick");

		// User cube
		PosX += ControlButtonWidth + 2 * MarginX;
		XmlControl(false, "UserCubeButton",
			PosX, PosY, ControlButtonWidth, ButtonHeight, SystemColors.ControlBrush, HorizontalAlignment.Center,
			null, "User Cube", "UserCubeButtonClick");

		File.Close();
		return;
		}

	private static void XmlControl
			(
			bool LabelType,
			string Name,
			double PosX,
			double PosY,
			double Width,
			double Height,
			Brush Background,
			HorizontalAlignment HorAlignment,
			string Tag,
			string Content,
			string Click
			)
		{
		StringBuilder Str = new StringBuilder();
		Str.Append(LabelType ? "<Label" : "<Button");

		if(!string.IsNullOrWhiteSpace(Name)) Str.AppendFormat("\r\n\t\t\tName=\"{0}\"", Name);

		Str.AppendFormat("\r\n\t\t\tWidth=\"{0}\" Height=\"{1}\"",
			Math.Round(Width).ToString(), Math.Round(Height).ToString());

		Str.Append("\r\n\t\t\tHorizontalAlignment=\"Left\" VerticalAlignment=\"Top\"");

		Str.AppendFormat("\r\n\t\t\tMargin=\"{0},{1},0,0\" Background=\"{2}\"",
			Math.Round(PosX).ToString(), Math.Round(PosY).ToString(), Background.ToString());

		Str.Append("\r\n\t\t\tFontFamily=\"Arial\" FontSize=\"14\" FontWeight=\"Bold\"");

		if(LabelType) Str.AppendFormat("\r\n\t\t\tHorizontalContentAlignment=\"{0}\" VerticalContentAlignment=\"Center\"",
			HorAlignment == HorizontalAlignment.Left ? "Left" : "Center");

		if(!LabelType && !string.IsNullOrWhiteSpace(Tag)) Str.AppendFormat("\r\n\t\t\tTag=\"{0}\"", Tag);

		if(!string.IsNullOrWhiteSpace(Content)) Str.AppendFormat("\r\n\t\t\tContent=\"{0}\"", Content);

		if(!LabelType && !string.IsNullOrWhiteSpace(Click)) Str.AppendFormat("\r\n\t\t\tClick=\"{0}\"", Click);

		Str.Append(" />");

		File.WriteLine(Str.ToString());
		return;
		}
	}
}
*/